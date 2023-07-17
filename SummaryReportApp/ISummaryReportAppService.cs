using Abp.Application.Services;
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
    public interface ISummaryReportAppService : IApplicationService
    {


        reportDetail mainSearch(string reportType, DateTime dateStart, DateTime dateEnd );

    }
}
