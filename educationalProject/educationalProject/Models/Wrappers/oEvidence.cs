using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
using educationalProject.Models.ViewModels;
namespace educationalProject.Models.Wrappers
{
    public class oEvidence : Evidence
    {
        public object Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oEvidence> result = new List<oEvidence>();
            d.iCommand.CommandText = String.Format("select * from {0}", FieldName.TABLE_NAME);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oEvidence
                        {
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                            evidence_code = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EVIDENCE_CODE].Ordinal]),
                            evidence_name = item.ItemArray[data.Columns[FieldName.EVIDENCE_NAME].Ordinal].ToString(),
                            file_name = item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString(),
                            secret = Convert.ToChar(item.ItemArray[data.Columns[FieldName.SECRET].Ordinal]),
                            teacher_id = item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal].ToString(),
                            //DANGER NULLABLE ZONE
                            primary_evidence_num = item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal]):0,
                            evidence_real_code = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EVIDENCE_REAL_CODE].Ordinal]),
                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal])
                        });
                    }

                    data.Dispose();
                }
                else
                {
                    //Reserved for return error string
                }
                res.Close();
            }
            catch (Exception ex)
            {
                //Handle error from sql execution
                return ex.Message;
            }
            finally
            {
                //Whether it success or not it must close connection in order to end block
                d.SQLDisconnect();
            }
            return result;
        }

        public object SelectWhere(string wherecond)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oEvidence> result = new List<oEvidence>();
            d.iCommand.CommandText = String.Format("select * from {0} where {1}", FieldName.TABLE_NAME, wherecond);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oEvidence
                        {
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                            evidence_code = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EVIDENCE_CODE].Ordinal]),
                            evidence_name = item.ItemArray[data.Columns[FieldName.EVIDENCE_NAME].Ordinal].ToString(),
                            file_name = item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString(),
                            secret = Convert.ToChar(item.ItemArray[data.Columns[FieldName.SECRET].Ordinal]),
                            teacher_id = item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal].ToString(),
                            //DANGER NULLABLE ZONE
                            primary_evidence_num = item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal]) : 0,
                            evidence_real_code = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EVIDENCE_REAL_CODE].Ordinal]),
                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal])
                        });
                    }
                    data.Dispose();
                }
                else
                {
                    //Reserved for return error string
                }
                res.Close();
            }
            catch (Exception ex)
            {
                //Handle error from sql execution
                return ex.Message;
            }
            finally
            {
                //Whether it success or not it must close connection in order to end block
                d.SQLDisconnect();
            }
            return result;
        }
        
        public object SelectByIndicatorAndCurriculum(oIndicator inddata, string curri_id)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oEvidence> result = new List<oEvidence>();
            d.iCommand.CommandText = String.Format("select * from {0} " + 
                "where {1} = {2} and {3} = '{4}' and {5} = {6}", 
                FieldName.TABLE_NAME,FieldName.INDICATOR_NUM,inddata.indicator_num,FieldName.CURRI_ID,
                curri_id,FieldName.ACA_YEAR,inddata.aca_year);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oEvidence
                        {
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                            evidence_code = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EVIDENCE_CODE].Ordinal]),
                            evidence_name = item.ItemArray[data.Columns[FieldName.EVIDENCE_NAME].Ordinal].ToString(),
                            file_name = item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString(),
                            secret = Convert.ToChar(item.ItemArray[data.Columns[FieldName.SECRET].Ordinal]),
                            teacher_id = item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal].ToString(),
                            //DANGER NULLABLE ZONE
                            primary_evidence_num = item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal]) : 0,
                            evidence_real_code = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EVIDENCE_REAL_CODE].Ordinal]),
                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal])
                        });
                    }
                    data.Dispose();
                }
                else
                {
                    //Reserved for return error string
                }
                res.Close();
            }
            catch (Exception ex)
            {
                //Handle error from sql execution
                return ex.Message;
            }
            finally
            {
                //Whether it success or not it must close connection in order to end block
                d.SQLDisconnect();
            }
            return result;
        }

        public object SelectByIndicatorAndCurriculumWithTName(oIndicator inddata, string curri_id)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Evidence_with_t_name> result = new List<Evidence_with_t_name>();
            d.iCommand.CommandText = String.Format("select e.*,{7}.{10},{7}.{11} from (select * from {0} " +
                "where {1} = {2} and {3} = '{4}' and {5} = {6}) as e inner join {7} on e.{8} = {7}.{9}",
                FieldName.TABLE_NAME, FieldName.INDICATOR_NUM, inddata.indicator_num, FieldName.CURRI_ID,
                curri_id, FieldName.ACA_YEAR, inddata.aca_year,Teacher.FieldName.TABLE_NAME,
                FieldName.TEACHER_ID,Teacher.FieldName.TEACHER_ID,Teacher.FieldName.T_PRENAME,Teacher.FieldName.T_NAME
                );
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new Evidence_with_t_name
                        {
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                            evidence_code = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EVIDENCE_CODE].Ordinal]),
                            evidence_name = item.ItemArray[data.Columns[FieldName.EVIDENCE_NAME].Ordinal].ToString(),
                            file_name = item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString(),
                            secret = Convert.ToChar(item.ItemArray[data.Columns[FieldName.SECRET].Ordinal]),
                            teacher_id = item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal].ToString(),
                            //DANGER NULLABLE ZONE
                            primary_evidence_num = item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal]) : 0,
                            evidence_real_code = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EVIDENCE_REAL_CODE].Ordinal]),
                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
                            t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                        });
                    }
                    data.Dispose();
                }
                else
                {
                    //Reserved for return error string
                }
                res.Close();
            }
            catch (Exception ex)
            {
                //Handle error from sql execution
                return ex.Message;
            }
            finally
            {
                //Whether it success or not it must close connection in order to end block
                d.SQLDisconnect();
            }
            return result;
        }

    }
}