using Sino.Dependency;
using Sino.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DateBase2PDF
{
    public interface IPeopleReporitory : ITestRepository<People, int>, ITransientDependency
    {
        Task<List<People>> GetPeoples();
    }
}
