using Dapper;
using Sino.Dapper;
using Sino.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateBase2PDF
{
    public class PeopleReporitory : TestRepository<People, int>, IPeopleReporitory
    {
        public PeopleReporitory(IDapperConfiguration configuration)
			: base(configuration)
        {

        }

        public async Task<List<People>> GetPeoples()
        {
            string sql = "Select * From people";
            var peoples = ReadConnection.Query<People>(sql).ToList();
            return peoples;
        }
    }
}
