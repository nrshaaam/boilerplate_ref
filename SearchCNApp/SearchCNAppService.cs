using Abp.Application.Services;
using AutoMapper;
using SOTSCanvas.SearchCnModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTSCanvas.SearchCnApp
{
    public class SearchCnAppService : ApplicationService, ISearchCnAppService
    {
        private readonly ISearchCnManager _SearchCnManager;

        public SearchCnAppService(ISearchCnManager SearchCnManager)
        {
            _SearchCnManager = SearchCnManager;
        }

        public IEnumerable<reportDetail> reportdetailGet(string searchCN)
        {
            return _SearchCnManager.getReportdetail(searchCN);
        }

        
    }
}
