using Abp.Domain.Services;
using SOTSCanvas.HubOutboundModels;
using SOTSCanvas.InboundfModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTSCanvas.SummaryReportModels
{
    public interface ISummaryReportManager : IDomainService
    {

        reportDetail searchMain(string reportType, DateTime dateStart, DateTime dateEnd);

    }
}
