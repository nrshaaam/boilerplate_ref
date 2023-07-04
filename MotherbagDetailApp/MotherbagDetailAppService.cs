using Abp.Application.Services;
using AutoMapper;
using SOTSCanvas.HubOutboundModels;
using SOTSCanvas.InboundfModel;
using SOTSCanvas.MotherbagDetailModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTSCanvas.MotherbagDetailApp
{
    public class MotherbagDetailAppService : ApplicationService, IMotherbagDetailAppService
    {
        private readonly IMotherbagDetailManager _MotherbagDetailManager;

        public MotherbagDetailAppService(IMotherbagDetailManager MotherbagDetailManager)
        {
            _MotherbagDetailManager = MotherbagDetailManager;
        }

        public List<string> destGet()
        {
            return _MotherbagDetailManager.getDest();
        }
        public List<string> originGet()
        {
            return _MotherbagDetailManager.getOrigin();
        }

        public List<dropdownlist> coloaderGet()
        {
            return _MotherbagDetailManager.getColoader();
        }

        public List<dropdownlist> transtypeGet()
        {
            return _MotherbagDetailManager.getTranstype();
        }

        public List<reportResult> mainSearch(string reportType, string startDate, string endDate, string bagNo, string origin, string destination, string batchNo, string staffId, string coloader, string runningNo, string lorryplateNo, string transType, string flightType, string flightNo, string currentBranch)
        {
            return _MotherbagDetailManager.searchMain(reportType, startDate, endDate, bagNo, origin, destination, batchNo, staffId, coloader, runningNo, lorryplateNo, transType, flightType, flightNo, currentBranch);
        }
    
        public List<reportDiscrepancy> discrepancySearch(string reportFrom, string reportTo, string startDate, string endDate, string motherbagNo)
        {
            return _MotherbagDetailManager.searchDiscrepancy(reportFrom, reportTo, startDate, endDate, motherbagNo);
        }
    }
}
