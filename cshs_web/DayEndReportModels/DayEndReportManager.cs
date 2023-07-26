using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Authorization.Users;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Configuration;
using GDEX.MasterCN;
using static Castle.MicroKernel.ModelBuilder.Descriptors.InterceptorDescriptor;
using System.Text.RegularExpressions;
using LazZiya.ImageResize.ResizeMethods;
using LibraryApp.Models.Pod;

namespace SOTSCanvas.DayEndReportModels
{
    public class DayEndReportManager : DomainService, IDayEndReportManager
    {
        private readonly UserManager _userManager;

        public DayEndReportManager(UserManager userManager)
        {
            _userManager = userManager;
        }

        string conString = System.Configuration.ConfigurationManager.ConnectionStrings["Main"].ConnectionString;

        public reportDetail searchMain(string searchReport)
        {
            try
            {
                reportDetail result = new reportDetail();
                result.header = new List<string>();
                result.item = new List<List<string>>();

                string com = @"SELECT case a.ctrx when '3' then '3rd Party CN' when 'M' then 'GDex CN' end as TYPE ,A.cDestination as DESTINATION, B.cProjectName as PROJECT, A.cBatchNo as BATCH,
							   A.cDespatchNo as [MANIFEST NO.], SUM(A.nCN) AS CN,
                              SUM(A.nPcs) AS PCS, CONVERT(VARCHAR, A.ddate, 103) as [DATE TIME] FROM manifestdb.cust.CSMDATA AS A LEFT OUTER JOIN manifestdb.cust.project AS B ON A.cPrjNo = B.id
                              WHERE CONVERT(date, A.ddate)  = '"+searchReport+@"' AND(A.cStatus = '1') AND(A.cTrx in ('M', '3'))
                              GROUP BY A.cDestination, B.cProjectName, A.cBatchNo, A.cDespatchNo, A.ddate, a.ctrx";


                SqlConnection con = new SqlConnection(conString);
                con.Open();
                using (SqlCommand command = new SqlCommand(com, con))
                {
                    command.CommandTimeout = 0;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int headerAdded = 0;

                        while (headerAdded < reader.FieldCount)
                        {
                            result.header.Add(reader.GetName(headerAdded));

                            headerAdded++;
                        }

                        while (reader.Read())
                        {
                            List<string> item = new List<string>();

                            int rowAdded = 0;

                            while (rowAdded < result.header.Count())
                            {
                                item.Add(reader[result.header[rowAdded]].ToString().Trim());

                                rowAdded++;
                            }

                            result.item.Add(item);
                        }

                        reader.Close();
                    }
                }

                con.Close();
                con.Dispose();

                return result;
            }

            catch (Exception ex)
            {
                Logger.Error("Error Message: " + ex.Message + ", \nInner Exception: " + ex.InnerException);
                throw new Abp.UI.UserFriendlyException(ex.Message);
            }
        }

        public reportDetail getReport(string searchReport)
        {
            try
            {
                reportDetail result = new reportDetail();
                result.header = new List<string>();
                result.item = new List<List<string>>();

                string com = @"SELECT CORIGIN as ORIGIN, CDESPATCHNO as [MANIFEST NO.], SUM(NCN) AS CN, SUM(NPCS) AS PCS, CONVERT(VARCHAR, ddate, 103) as [DATE TIME]
                                from manifestdb.cust.CSMDATA WHERE
                                CONVERT(date, ddate)  = '" + searchReport+@"' AND CSTATUS = '1'
                                AND CTRX = 'R' GROUP BY CORIGIN, CDESPATCHNO, DDATE";


                SqlConnection con = new SqlConnection(conString);
                con.Open();
                using (SqlCommand command = new SqlCommand(com, con))
                {
                    command.CommandTimeout = 0;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int headerAdded = 0;

                        while (headerAdded < reader.FieldCount)
                        {
                            result.header.Add(reader.GetName(headerAdded));

                            headerAdded++;
                        }

                        while (reader.Read())
                        {
                            List<string> item = new List<string>();

                            int rowAdded = 0;

                            while (rowAdded < result.header.Count())
                            {
                                item.Add(reader[result.header[rowAdded]].ToString().Trim());

                                rowAdded++;
                            }

                            result.item.Add(item);
                        }

                        reader.Close();
                    }
                }

                con.Close();
                con.Dispose();

                return result;
            }

            catch (Exception ex)
            {
                Logger.Error("Error Message: " + ex.Message + ", \nInner Exception: " + ex.InnerException);
                throw new Abp.UI.UserFriendlyException(ex.Message);
            }
        }

    }
}