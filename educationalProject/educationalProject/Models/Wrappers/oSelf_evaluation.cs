//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Data;
//using educationalProject.Utils;
//using educationalProject.Models.ViewModels;
//namespace educationalProject.Models.Wrappers
//{
//    public class oSelf_evaluation : Self_evaluation
//    {
//        public object Select()
//        {
//            DBConnector d = new DBConnector();
//            if (!d.SQLConnect())
//                return "Cannot connect to database.";
//            List<vSelf_evaluation> result = new List<vSelf_evaluation>();
//            d.iCommand.CommandText = String.Format("select * from {0}", FieldName.TABLE_NAME);
//            try
//            {
//                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
//                if (res.HasRows)
//                {
//                    DataTable data = new DataTable();
//                    data.Load(res);
//                    foreach (DataRow item in data.Rows)
//                    {
//                        result.Add(new vSelf_evaluation
//                        {
//                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
//                            date = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.DATE].Ordinal],System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
//                            time = item.ItemArray[data.Columns[FieldName.TIME].Ordinal].ToString(),
//                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
//                            evaluation_score = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EVALUATION_SCORE].Ordinal]),
//                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
//                            sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.SUB_INDICATOR_NUM].Ordinal]),
//                            teacher_id = item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal].ToString()
//                        });
//                    }
//                    res.Close();
//                    data.Dispose();
//                }
//                else
//                {
//                    //Reserved for return error string
//                }
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

//        public object SelectWhere(string wherecond)
//        {
//            DBConnector d = new DBConnector();
//            if (!d.SQLConnect())
//                return "Cannot connect to database.";
//            List<vSelf_evaluation> result = new List<vSelf_evaluation>();
//            d.iCommand.CommandText = String.Format("select * from {0} where {1}", FieldName.TABLE_NAME, wherecond);
//            try
//            {
//                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
//                if (res.HasRows)
//                {
//                    DataTable data = new DataTable();
//                    data.Load(res);
//                    foreach (DataRow item in data.Rows)
//                    {
//                        result.Add(new vSelf_evaluation
//                        {
//                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
//                            date = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.DATE].Ordinal], System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
//                            time = item.ItemArray[data.Columns[FieldName.TIME].Ordinal].ToString(),
//                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
//                            evaluation_score = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EVALUATION_SCORE].Ordinal]),
//                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
//                            sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.SUB_INDICATOR_NUM].Ordinal]),
//                            teacher_id = item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal].ToString()
//                        });
//                    }
//                    res.Close();
//                    data.Dispose();
//                }
//                else
//                {
//                    //Reserved for return error string
//                }
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

//        public object SelectWithTeacher()
//        {
//            DBConnector d = new DBConnector();
//            if (!d.SQLConnect())
//                return "Cannot connect to database.";
//            List<vSelf_evaluation_teacher> result = new List<vSelf_evaluation_teacher>();
//            d.iCommand.CommandText = String.Format("select * from {0} as t1, {1} as t2 where t1.teacher {2}", FieldName.TABLE_NAME);
//            try
//            {
//                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
//                if (res.HasRows)
//                {
//                    DataTable data = new DataTable();
//                    data.Load(res);
//                    foreach (DataRow item in data.Rows)
//                    {
//                        result.Add(new vSelf_evaluation
//                        {
//                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
//                            date = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.DATE].Ordinal], System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
//                            time = item.ItemArray[data.Columns[FieldName.TIME].Ordinal].ToString(),
//                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
//                            evaluation_score = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EVALUATION_SCORE].Ordinal]),
//                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
//                            sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.SUB_INDICATOR_NUM].Ordinal]),
//                            teacher_id = item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal].ToString()
//                        });
//                    }
//                    res.Close();
//                    data.Dispose();
//                }
//                else
//                {
//                    //Reserved for return error string
//                }
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