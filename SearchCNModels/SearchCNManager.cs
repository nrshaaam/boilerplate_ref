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

namespace SOTSCanvas.SearchCnModels
{
    public class SearchCnManager : DomainService, ISearchCnManager
    {
        private readonly UserManager _userManager;

        public SearchCnManager(UserManager userManager)
        {
            _userManager = userManager;
        }

        string conString = System.Configuration.ConfigurationManager.ConnectionStrings["Main"].ConnectionString;

        public IEnumerable<reportDetail> getReportdetail(string searchCN)
        {
            try
            {
                List<reportDetail> result = new List<reportDetail>();

                string com = "select cbagid as [BAG ID],ccn as CN, cmpsno as MPSNo, cdespatchno  as Manifest, cBatchNo as Batch, ctrx as [Transaction], ctype as Type," +
                    " cDestination as Destination, cBranch as Branch,  dtScan as DateScan, cuserid as ScanBy, cOrigin as Origin, " +
                    " case when cstatus = '1' then '' when cstatus='0' then 'Delete' end as Status, dupdateon as DateUpdate, cupdateby as UpdateBy" +
                    " from manifestdb.cust.CSMDATA where  ccn='"+searchCN.Trim()+"'";

                SqlConnection con = new SqlConnection(conString);
                con.Open();
                using (SqlCommand command = new SqlCommand(com, con))
                {
                    command.CommandTimeout = 0;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reportDetail item = new reportDetail();

                            item.BAGID = reader["BAG ID"].ToString().Trim();
                            item.CN = reader["CN"].ToString().Trim();
                            item.Manifest = reader["Manifest"].ToString().Trim();
                            item.Batch = reader["Batch"].ToString().Trim();
                            item.Transaction = reader["Transaction"].ToString().Trim();
                            item.Type = reader["Type"].ToString().Trim();
                            item.Destination = reader["Destination"].ToString().Trim();
                            item.Branch = reader["Branch"].ToString().Trim();
                            item.DateScan = reader["DateScan"].ToString().Trim();
                            item.ScanBy = reader["ScanBy"].ToString().Trim();
                            item.Origin = reader["Origin"].ToString().Trim();
                            item.Status = reader["Status"].ToString().Trim();
                            item.DateUpdate = reader["DateUpdate"].ToString().Trim();
                            item.UpdateBy = reader["UpdateBy"].ToString().Trim();

                            result.Add(item);
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