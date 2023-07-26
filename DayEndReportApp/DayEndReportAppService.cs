using Abp.Application.Services;
using AutoMapper;
using SOTSCanvas.HubOutboundModels;
using SOTSCanvas.InboundfModel;
using SOTSCanvas.DayEndReportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTSCanvas.DayEndReportApp
{
    public class DayEndReportAppService : ApplicationService, IDayEndReportAppService
    {
        private readonly IDayEndReportManager _DayEndReportManager;

        public DayEndReportAppService(IDayEndReportManager DayEndReportManager)
        {
            _DayEndReportManager = DayEndReportManager;
        }


        public reportDetail mainSearch(string searchReport)
        {
            return _DayEndReportManager.searchMain(searchReport);
        }
        public reportDetail reportGet(string searchReport)
        {
            return _DayEndReportManager.getReport(searchReport);
        }
    }
}
