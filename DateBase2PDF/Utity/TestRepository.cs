using Dapper;
using Sino.Dapper;
using Sino.Dapper.Repositories;
using Sino.Domain.Entities;
using Sino.Domain.Entities.Auditing;
using Sino.Domain.Repositories;
using Sino.Extensions.Dapper.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateBase2PDF
{
    public abstract class TestRepository<TEntity, TPrimaryKey> : DapperRepositoryBase<TEntity, TPrimaryKey>, ITestRepository<TEntity, TPrimaryKey>
        where TEntity : class, IFullAudited, IEntity<TPrimaryKey>
    {
        public TestRepository(IDapperConfiguration configuration) : base(configuration)
        {
        }

        public async override Task<Tuple<int, IList<TEntity>>> GetListAsync(IQueryObject<TEntity> query)
        {
            var parameters = new DynamicParameters();
            var select = ExpressionHelper.Select<TEntity>();
            var count = ExpressionHelper.Count<TEntity>();

            foreach (var where in query.QueryExpression)
            {
                select.Where(where);
            }
            foreach (var where in query.QueryExpression)
            {
                count.Where(where);
            }
            select.Where(x => x.IsDeleted == false);
            count.Where(x => x.IsDeleted == false);
            if (query.OrderSort == SortOrder.ASC)
            {
                select.OrderBy(query.OrderField);
            }
            else if (query.OrderSort == SortOrder.DESC)
            {
                select.OrderByDesc(query.OrderField);
            }
            else
            {
                select.OrderByDesc(x => x.CreationTime);
            }
            if (query.Count >= 0)
            {
                select.Limit(query.Skip, query.Count);
            }

            foreach (KeyValuePair<string, object> item in select.DbParams)
            {
                parameters.Add(item.Key, item.Value);
            }

            using (Connection)
            {
                var customerRepresentativeList = await ReadConnection.QueryAsync<TEntity>(select.Sql, parameters);
                int totalCount = await ReadConnection.QuerySingleAsync<int>(count.Sql, parameters);

                return new Tuple<int, IList<TEntity>>(totalCount, customerRepresentativeList.ToList());
            }
        }

        public async override Task<int> CountAsync(IQueryObject<TEntity> query)
        {
            var parameters = new DynamicParameters();
            var count = ExpressionHelper.Count<TEntity>();
            foreach (var where in query.QueryExpression)
            {
                count.Where(where);
            }
            count.Where(x => x.IsDeleted == false);

            foreach (KeyValuePair<string, object> item in count.DbParams)
            {
                parameters.Add(item.Key, item.Value);
            }

            using (Connection)
            {
                var Count = await ReadConnection.QuerySingleAsync<int>(count.Sql, parameters);
                return Count;
            }
        }

        public async Task<int> GetCodeTotalCount(string tableName, string paramName)
        {
            int totalCount = 0;
            StringBuilder codeSql = new StringBuilder();
            codeSql.AppendFormat("SELECT {0} FROM {1} WHERE CreationTime >= @TodayStart AND CreationTime < @TodayEnd ORDER BY {0} DESC LIMIT 0,1;", paramName, tableName);
            DateTime TodayStart = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime TodayEnd = TodayStart.AddDays(1);
            var list = ReadConnection.Query<string>(codeSql.ToString(), new { TodayStart = TodayStart, TodayEnd = TodayEnd }).ToList();
            if (list?.Count > 0)
            {
                totalCount = int.Parse(list.FirstOrDefault().Substring(7, 4));
            }
            return totalCount;
        }

    }

}
