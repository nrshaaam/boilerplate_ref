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
using SOTSCanvas.InboundfModel;
using SOTSCanvas.HubOutboundModels;

namespace SOTSCanvas.MotherbagDetailModels
{
    public class MotherbagDetailManager : DomainService, IMotherbagDetailManager
    {
        private readonly UserManager _userManager;

        public MotherbagDetailManager(UserManager userManager)
        {
            _userManager = userManager;
        }

        string conString = System.Configuration.ConfigurationManager.ConnectionStrings["Main"].ConnectionString;

        public List<string> getDest()
        {
            try
            {
                List<string> result = new List<string>();

                string com = "manifestdb.dbo.mms_select 'PQ_Sv_Origin'";
                SqlConnection con = new SqlConnection(conString);
                con.Open();
                using (SqlCommand command = new SqlCommand(com, con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader["origin_code"].ToString().Trim());
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

        public List<string> getOrigin()
        {
            try
            {
                List<string> result = new List<string>();

                string com = "SELECT DISTINCT m.* from (select a.origin_code,a.origin_desc origin_defi FROM Gdexpdb.dbo.station a with (nolock) WHERE status=1 union select a.stnno,a.name FROM manifestdb.dbo.origin_ext a (nolock) WHERE a.type not in ('z') and NOT a.stnno IN('HUB','HKI','HKC')) m order by m.origin_code";
                SqlConnection con = new SqlConnection(conString);
                con.Open();
                using (SqlCommand command = new SqlCommand(com, con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader["origin_code"].ToString().Trim());
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

        public List<dropdownlist> getColoader()
        {
            try
            {
                List<dropdownlist> result = new List<dropdownlist>();

                string com = "SELECT [cColoaderName], [cCompany] FROM [manifestdb].[dbo].[coloader] WITH (NOLOCK) WHERE [cStatus] = '1'";
                SqlConnection con = new SqlConnection(conString);
                con.Open();
                using (SqlCommand command = new SqlCommand(com, con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dropdownlist item = new dropdownlist();

                            item.value = reader["cColoaderName"].ToString().Trim();
                            item.desc = reader["cCompany"].ToString().Trim();

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

        public List<dropdownlist> getTranstype()
        {
            try
            {
                List<dropdownlist> result = new List<dropdownlist>();

                string com = "SELECT [code], [shortname] FROM [manifestdb].[dbo].[master_type] WITH (NOLOCK) WHERE [catagory] = 'Flight_Type' AND [rec_status] = '1' ORDER BY [id]";
                SqlConnection con = new SqlConnection(conString);
                con.Open();
                using (SqlCommand command = new SqlCommand(com, con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dropdownlist item = new dropdownlist();

                            item.value = reader["code"].ToString().Trim();
                            item.desc = reader["shortname"].ToString().Trim();

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

        public List<reportResult> searchMain(string reportType, string startDate, string endDate, string bagNo, string origin, string destination, string batchNo, string staffId, string coloader, string runningNo, string lorryplateNo, string transType, string flightType, string flightNo, string currentBranch)
        {
            try
            {
                List<reportResult> result = new List<reportResult>();

                string whereClause = "WHERE A.[cstatus] = '1' AND A.[ctrx] " + (reportType == "J" ? "IN ('" + reportType + "', 'H')" : "= '" + reportType + "'" + " AND A.[cbranch] = '" + currentBranch + "' AND B.[ctrx] = '" + reportType + "'");

                if (startDate == endDate)
                {
                    whereClause = whereClause + " AND CAST(A.[dtscan] AS DATE) = '" + startDate + "'";
                }

                else
                {
                    whereClause = whereClause + " AND CAST(A.[dtscan] AS DATE) BETWEEN '" + startDate + "' AND '" + endDate + "'";
                }

                if (bagNo != "n0n3")
                {
                    whereClause = whereClause + " AND A.[cmpsno] = '" + bagNo.Replace("'", "`") + "'";
                }

                if (destination != "n0n3")
                {
                    whereClause = whereClause + " AND B.[cDestStn] = '" + destination + "'";
                }

                if (batchNo != "n0n3")
                {
                    whereClause = whereClause + " AND A.[cLocation] = '" + batchNo.Replace("'", "`") + "'";
                }

                if (staffId != "n0n3")
                {
                    whereClause = whereClause + " AND A.[cuserid] = '" + staffId.Replace("'", "`") + "'";
                }

                if (coloader != "n0n3" && reportType == "G")
                {
                    whereClause = whereClause + " AND A.[cReasonCode] = '" + coloader + "'";
                }

                if (runningNo != "n0n3" && reportType == "G")
                {
                    whereClause = whereClause + " AND A.[croute] = '" + runningNo.Replace("'", "`") + "'";
                }

                if (lorryplateNo != "n0n3" && reportType == "G")
                {
                    whereClause = whereClause + " AND A.[remarks] = '" + lorryplateNo.Replace("'", "`") + "'";
                }

                if (transType != "n0n3" && reportType == "J")
                {
                    whereClause = whereClause + " AND A.[ctype] = '" + transType + "'";
                }

                if (flightType != "n0n3" && reportType == "J")
                {
                    whereClause = whereClause + " AND A.[remarks2] = '" + flightType + "'";
                }

                if (flightNo != "n0n3" && reportType == "J")
                {
                    whereClause = whereClause + " AND A.[remarks] = '" + flightNo.Replace("'", "`") + "'";
                }

                string com = "SELECT A.[cmpsno], A.[dtscan], A.[cReasonCode],(select top 1 x.cbranch from manifestdb.dbo.bmsdata x(nolock) where x.cmanifest= CAST(A.[cmpsno] AS varchar) and x.ctrx in ('M', 'H', 'R') ORDER BY x.id) cOrigin, (select top 1 x.cstnno from manifestdb.dbo.bmsdata x(nolock) where x.cmanifest= CAST(A.[cmpsno] AS varchar) and x.ctrx in ('M', 'H', 'R') ORDER BY x.id) cDestStn, A.[cst], A.[nWeight], A.[cuserid], A.[remarks], A.[croute], A.[cLocation], A.[remarks2], B.[cRemark] FROM [manifestdb].[dbo].[bmsdata] A WITH(NOLOCK) LEFT JOIN [manifestdb].[dbo].[ManifestDoc] B WITH(NOLOCK) ON CAST(A.[cmpsno] AS varchar) = B.[cManifestNo] and B.ctrx = 'G' " + whereClause + " ORDER BY a.dtscan desc";
                
                SqlConnection con = new SqlConnection(conString);
                con.Open();
                using (SqlCommand command = new SqlCommand(com, con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int counter = 1;

                        while (reader.Read())
                        {
                            reportResult item = new reportResult();

                            item.no = counter.ToString();
                            item.manifestNo = reader["cmpsno"].ToString().Trim();
                            item.manifestDate = reader["dtscan"].ToString().Trim();
                            item.coloader = reader["cReasonCode"].ToString().Trim();
                            item.origin = reader["cOrigin"].ToString().Trim();
                            item.destination = reader["cDestStn"].ToString().Trim();
                            item.bagCondition = reader["cst"].ToString().Trim();
                            item.weight = reader["nWeight"].ToString().Trim();
                            item.staffId = reader["cuserid"].ToString().Trim();
                            item.vehicleNo = reader["remarks"].ToString().Trim();
                            item.runningNo = reader["croute"].ToString().Trim();
                            item.batchNo = reader["cLocation"].ToString().Trim();

                            if (item.bagCondition == "G")
                            {
                                item.bagCondition = "Good";
                            }

                            else if (item.bagCondition == "B")
                            {
                                item.bagCondition = "Bad";
                            }

                            if (reportType == "G")
                            {
                                item.remark = reader["remarks2"].ToString().Trim();
                            }

                            else
                            {
                                item.remark = reader["cRemark"].ToString().Trim();
                            }

                            result.Add(item);

                            counter++;
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

        public List<reportDiscrepancy> searchDiscrepancy(string reportFrom, string reportTo, string startDate, string endDate, string motherbagNo)
        {
            try
            {
                List<reportDiscrepancy> result = new List<reportDiscrepancy>();

                //string[] reportTypeSplit = reportType.Split('|');
                //string mainTable = "A";
                //string subTable = "B";

                //if (reportFrom == "HKI" || reportFrom == "HKC")
                //{
                //    mainTable = "B";
                //    subTable = "A";
                //}

                string whereClause = @"AND CAST(A.[dManifest] AS DATE) BETWEEN '" + startDate + "' AND '" + endDate + "' ";

                if (!String.IsNullOrEmpty(motherbagNo))
                {
                    whereClause = @"AND A.[cManifestNo] = '" + motherbagNo.Replace("'", "`") + "' ";
                }

                //string com = @"SELECT " + mainTable + ".[dtscan] AS [dtscanhub], " + subTable + ".[dtscan] AS [dtscanhki], " + mainTable + ".[cmpsno] AS [manifestno], " + mainTable + ".[nWeight] AS [weighthub], " + subTable + ".[nWeight] AS [weighthki] FROM [manifestdb].[dbo].[bmsdata] A WITH (NOLOCK) LEFT JOIN [manifestdb].[dbo].[bmsdata] B WITH (NOLOCK) ON " + mainTable + ".[cmpsno] = " + subTable + ".[cmpsno] AND " + subTable + ".[ctrx] = 'G' AND " + subTable + ".[cbranch] = '" + reportTo + "' LEFT JOIN [manifestdb].[dbo].[ManifestDoc] C WITH (NOLOCK) ON " + mainTable + ".[cmpsno] = C.[cManifestNo] AND (C.[ctrx] = 'J' OR C.[cLastTrx] = 'H') WHERE (" + mainTable + ".[ctrx] = 'J' OR " + mainTable + ".[ctrx] = 'H') AND " + mainTable + ".[cstatus] = '1' AND " + mainTable + ".[cbranch] = '" + reportFrom + "' AND C.[cBranch] = '" + reportFrom + "' " + whereClause + "ORDER BY " + mainTable + ".[dtscan]";
                string com = "Select A.dtCreated AS [dtscanhub], B.dtCreated AS [dtscanhki], A.cManifestNo AS [manifestno], A.nWeight AS [weighthub], B.nWeight AS [weighthki] FROM manifestdb.dbo.ManifestDoc A(NOLOCK) LEFT JOIN manifestdb.dbo.ManifestDoc B(nolock) ON A.cManifestNo = B.cManifestNo and B.[ctrx] = 'G' WHERE A.[cBranch] = '" + reportFrom + "' AND(A.[ctrx] = 'J' OR A.[cLastTrx] = 'H') AND B.cBranch = '" + reportTo + "' " + whereClause + " ORDER BY A.[dtCreated]";
                SqlConnection con = new SqlConnection(conString);
                con.Open();
                using (SqlCommand command = new SqlCommand(com, con))
                {
                    command.CommandTimeout = 0;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reportDiscrepancy item = new reportDiscrepancy();

                            item.dtscanhub = reader["dtscanhub"].ToString().Trim();
                            item.dtscanhki = reader["dtscanhki"].ToString().Trim();
                            item.manifestno = reader["manifestno"].ToString().Trim();
                            item.weighthub = reader["weighthub"].ToString().Trim();
                            item.weighthki = reader["weighthki"].ToString().Trim();

                            if (!String.IsNullOrEmpty(item.weighthub) && !String.IsNullOrEmpty(item.weighthki))
                            {
                                item.weightdifference = (decimal.Parse(item.weighthub) - decimal.Parse(item.weighthki)).ToString();
                            }

                            else
                            {
                                item.weightdifference = "";
                            }

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