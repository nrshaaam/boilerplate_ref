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

namespace SOTSCanvas.SummaryReportModels
{
    public class SummaryReportManager : DomainService, ISummaryReportManager
    {
        private readonly UserManager _userManager;

        public SummaryReportManager(UserManager userManager)
        {
            _userManager = userManager;
        }

        string conString = System.Configuration.ConfigurationManager.ConnectionStrings["Main"].ConnectionString;

        public reportDetail searchMain(string reportType, DateTime dateStart, DateTime dateEnd)

        {
            try
            {
                reportDetail result = new reportDetail();
                result.header = new List<string>();
                result.item = new List<List<string>>();

                string date1 = ",'" + dateStart.ToString("yyyy-MM-dd") + "'";
                string date2 = ",'" + dateEnd.ToString("yyyy-MM-dd") + "'";
                string exeqry = "";

                if (reportType == "AGSS Orders Status")
                {
                    exeqry =  date1;
                }
                else
                {
                    exeqry = date1 + date2;
                }

                string com = "EXEC manifestdb.cust.CUST_SELECT '" + reportType.Trim() + "'" + exeqry;


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