using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTSCanvas.SearchCnModels
{
    public interface ISearchCnManager : IDomainService
    {
        IEnumerable<reportDetail> getReportdetail(string searchCN);
    }
}
