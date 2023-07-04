using Abp.Application.Services;
using SOTSCanvas.SearchCnModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTSCanvas.SearchCnApp
{
    public interface ISearchCnAppService : IApplicationService
    {
        IEnumerable<reportDetail> reportdetailGet(string searchCN);

    }
}
