using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
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
                            primary_evidence_num = item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal]):0
                        });
                    }
                    res.Close();
                    data.Dispose();
                }
                else
                {
                    //Reserved for return error string
                }
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
                            primary_evidence_num = item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal]) : 0
                        });
                    }
                    res.Close();
                    data.Dispose();
                }
                else
                {
                    //Reserved for return error string
                }
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
            d.iCommand.CommandText = String.Format("select * from {0} as e1 " + 
                "where e1.{1} IN (select e2.{1} from {2} as e2 where e2.{3} = {4}) and {5} = '{6}' and {7} = {8}", 
                FieldName.TABLE_NAME,FieldName.EVIDENCE_CODE,Evidence_indicator.FieldName.TABLE_NAME,
                Evidence_indicator.FieldName.INDICATOR_NUM,inddata.indicator_num,FieldName.CURRI_ID,
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
                            primary_evidence_num = item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal]) : 0
                        });
                    }
                    res.Close();
                    data.Dispose();
                }
                else
                {
                    //Reserved for return error string
                }
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