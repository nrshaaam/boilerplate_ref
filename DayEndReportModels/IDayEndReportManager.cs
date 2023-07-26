using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTSCanvas.DayEndReportModels
{
    public interface IDayEndReportManager : IDomainService
    {

        reportDetail searchMain(string searchReport);


        reportDetail getReport(string searchReport);

    }

    
}
