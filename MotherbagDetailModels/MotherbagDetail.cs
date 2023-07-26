using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTSCanvas.MotherbagDetailModels
{
    public class reportResult
    {
        public string no { get; set; }

        public string manifestNo { get; set; }

        public string manifestDate { get; set; }

        public string coloader { get; set; }

        public string origin { get; set; }

        public string destination { get; set; }

        public string bagCondition { get; set; }

        public string weight { get; set; }

        public string staffId { get; set; }

        public string vehicleNo { get; set; }

        public string runningNo { get; set; }

        public string batchNo { get; set; }

        public string remark { get; set; }
    }

    public class reportDiscrepancy
    {
        public string dtscanhub { get; set; }

        public string dtscanhki { get; set; }

        public string manifestno { get; set; }

        public string weighthub { get; set; }

        public string weighthki { get; set; }

        public string weightdifference { get; set; }
    }
}
