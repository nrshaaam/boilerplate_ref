using Abp.Application.Services;
using AutoMapper;
using SOTSCanvas.HubOutboundModels;
using SOTSCanvas.InboundfModel;
using SOTSCanvas.SummaryReportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTSCanvas.SummaryReportApp
{
    public class SummaryReportAppService : ApplicationService, ISummaryReportAppService
    {
        private readonly ISummaryReportManager _SummaryReportManager;

        public SummaryReportAppService(ISummaryReportManager SummaryReportManager)
        {
            _SummaryReportManager = SummaryReportManager;
        }


        public reportDetail mainSearch(string reportType, DateTime dateStart, DateTime dateEnd)
        {
            return _SummaryReportManager.searchMain(reportType, dateStart, dateEnd);
        }
    }
}
