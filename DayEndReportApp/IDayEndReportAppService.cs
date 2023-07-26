using Abp.Application.Services;
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
    public interface IDayEndReportAppService : IApplicationService
    {


        reportDetail mainSearch(string searchReport);

        reportDetail reportGet(string searchReport);

    }
}
