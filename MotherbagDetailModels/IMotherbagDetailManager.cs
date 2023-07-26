using Abp.Domain.Services;
using SOTSCanvas.HubOutboundModels;
using SOTSCanvas.InboundfModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTSCanvas.MotherbagDetailModels
{
    public interface IMotherbagDetailManager : IDomainService
    {
        List<string> getDest();
        List<string> getOrigin();

        List<dropdownlist> getColoader();

        List<dropdownlist> getTranstype();

        List<reportResult> searchMain(string reportType, string startDate, string endDate, string bagNo, string origin, string destination, string batchNo, string staffId, string coloader, string runningNo, string lorryplateNo, string transType, string flightType, string flightNo, string currentBranch);

        List<reportDiscrepancy> searchDiscrepancy(string reportFrom, string reportTo, string startDate, string endDate, string motherbagNo);
    }
}
