using Abp.Application.Services;
using SOTSCanvas.HubOutboundModels;
using SOTSCanvas.InboundfModel;
using SOTSCanvas.MotherbagDetailModels;
using SOTSCanvas.MotherbagOutboundModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTSCanvas.MotherbagDetailApp
{
    public interface IMotherbagDetailAppService : IApplicationService
    {
        List<string> destGet();
        List<string> originGet();

        List<dropdownlist> coloaderGet();

        List<dropdownlist> transtypeGet();

        List<reportResult> mainSearch(string reportType, string startDate, string endDate, string bagNo, string origin, string destination, string batchNo, string staffId, string coloader, string runningNo, string lorryplateNo, string transType, string flightType, string flightNo, string currentBranch);

        List<reportDiscrepancy> discrepancySearch(string reportFrom, string reportTo, string startDate, string endDate, string motherbagNo);
    }
}
