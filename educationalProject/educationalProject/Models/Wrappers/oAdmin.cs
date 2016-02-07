//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Data;
//using educationalProject.Models.ViewModels;
//using educationalProject.Utils;
//namespace educationalProject.Models.Wrappers
//{
//    public class oAdmin : Admin
//    {
//        public object Select()
//        {
//            DBConnector d = new DBConnector();
//            if (!d.SQLConnect())
//                return "Cannot connect to database.";
//            List<Admin_with_creator> result = new List<Admin_with_creator>();

//            string temp5tablename = "#temp5";

//            string createtabletemp5 = string.Format("CREATE TABLE {0}( " +
//	                                  "[{1}] VARCHAR(40) NULL," +
//	                                  "[{2}] VARCHAR(120)  NOT NULL," +
//                                      "[{3}] VARCHAR(MAX) NULL," +
//	                                  "[{4}] VARCHAR(120) NULL," +
//	                                  "[{5}] VARCHAR(16) NULL," +
//	                                  "[{6}] VARCHAR(60) NULL," +
//	                                  "[{7}] VARCHAR(16) NULL," +
//	                                  "[{8}] VARCHAR(60) NULL," +
//	                                  "[{9}] CHAR(13) NULL," +
//	                                  "[gender] CHAR NULL," +
//                                      "[email] VARCHAR(60) NULL," +
//                                      "[tel] VARCHAR(20) NULL," +
//	                                  "[addr] VARCHAR(80) NULL," +
//	                                  "[file_name_pic] VARCHAR(255) NULL," +
//	                                  "[timestamp] DATETIME2 NULL," +
//                                      "[creator_name] VARCHAR(70) NULL," +
//                                      "PRIMARY KEY([{2}])) " +

//                                      "alter table {0} " +
//                                      "alter column {1} VARCHAR(40) collate database_default " +

//                                      "alter table {0} " +
//                                      "alter column [{2}] VARCHAR(120) collate database_default " +

//                                      "alter table {0} " +
//                                      "alter column [{3}] VARCHAR(MAX) collate database_default " +

//                                      "alter table {0} " +
//                                      "alter column [{4}] VARCHAR(120) collate database_default " +

//                                      "alter table {0} " +
//                                      "alter column [{5}] VARCHAR(16) collate database_default " +

//                                      "alter table {0} " +
//                                      "alter column [{6}] VARCHAR(60) collate database_default " +

//                                      "alter table {0} " +
//                                      "alter column [{7}] VARCHAR(16) collate database_default " +

//                                      "alter table {0} " +
//                                      "alter column [{8}] VARCHAR(60) collate database_default " +

//                                      "alter table {0} " +
//                                      "alter column [{9}] CHAR(13) collate database_default " +

//                                      "alter table {0} " +
//                                      "alter column [gender] CHAR collate database_default " +

//                                      "alter table {0} " +
//                                      "alter column [email] VARCHAR(60) collate database_default " +

//                                      "alter table {0} " +
//                                      "alter column [tel] VARCHAR(20) collate database_default " +

//                                      "alter table {0} " +
//                                      "alter column [addr] VARCHAR(80) collate database_default " +

//                                      "alter table {0} " +
//                                      "alter column [file_name_pic] VARCHAR(255) collate database_default " +

//                                      "alter table {0} " +
//                                      "alter column [creator_name] VARCHAR(70) collate database_default ",
//                                      temp5tablename,FieldName.USER_TYPE,FieldName.USERNAME,FieldName.PASSWORD,
//                                      FieldName.ADMIN_CREATOR_ID,FieldName.T_PRENAME,FieldName.T_NAME,
//                                      FieldName.E_PRENAME,FieldName.E_NAME,FieldName.CITIZEN_ID,
   







//            d.iCommand.CommandText = string.Format("select * from {0}", FieldName.TABLE_NAME);
//            try
//            {
//                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
//                if (res.HasRows)
//                {
//                    DataTable data = new DataTable();
//                    data.Load(res);
//                    foreach (DataRow item in data.Rows)
//                    {
//                        result.Add(new oCu_curriculum
//                        {
//                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
//                            curr_tname = item.ItemArray[data.Columns[FieldName.CURR_TNAME].Ordinal].ToString(),
//                            curr_ename = item.ItemArray[data.Columns[FieldName.CURR_ENAME].Ordinal].ToString(),
//                            degree_e_bf = item.ItemArray[data.Columns[FieldName.DEGREE_E_BF].Ordinal].ToString(),
//                            degree_e_full = item.ItemArray[data.Columns[FieldName.DEGREE_E_FULL].Ordinal].ToString(),
//                            degree_t_bf = item.ItemArray[data.Columns[FieldName.DEGREE_T_BF].Ordinal].ToString(),
//                            degree_t_full = item.ItemArray[data.Columns[FieldName.DEGREE_T_FULL].Ordinal].ToString(),
//                            level = Convert.ToChar(item.ItemArray[data.Columns[FieldName.LEVEL].Ordinal]),
//                            period = Convert.ToChar(item.ItemArray[data.Columns[FieldName.PERIOD].Ordinal]),
//                            year = item.ItemArray[data.Columns[FieldName.YEAR].Ordinal].ToString()
//                        });
//                    }
//                    data.Dispose();
//                }
//                else
//                {
//                    //Reserved for return error string
//                }
//                res.Close();
//            }
//            catch (Exception ex)
//            {
//                //Handle error from sql execution
//                return ex.Message;
//            }
//            finally
//            {
//                //Whether it success or not it must close connection in order to end block
//                d.SQLDisconnect();
//            }
//            return result;
//        }
//    }
//}