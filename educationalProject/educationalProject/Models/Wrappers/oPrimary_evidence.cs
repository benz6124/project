using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
using educationalProject.Models.ViewModels;
namespace educationalProject.Models.Wrappers
{
    public class oPrimary_evidence : Primary_evidence
    {
        public object Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oPrimary_evidence> result = new List<oPrimary_evidence>();
            d.iCommand.CommandText = string.Format("select * from {0}", FieldName.TABLE_NAME);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oPrimary_evidence
                        {
                            primary_evidence_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal]),
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
                            evidence_name = item.ItemArray[data.Columns[FieldName.EVIDENCE_NAME].Ordinal].ToString(),
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString()
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
            List<oPrimary_evidence> result = new List<oPrimary_evidence>();
            d.iCommand.CommandText = string.Format("select * from {0} where {1}", FieldName.TABLE_NAME, wherecond);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oPrimary_evidence
                        {
                            primary_evidence_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal]),
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
                            evidence_name = item.ItemArray[data.Columns[FieldName.EVIDENCE_NAME].Ordinal].ToString(),
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString()
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

        public object SelectWithDetail(oIndicator inddata,string curri_id_param)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Primary_evidence_detail> result = new List<Primary_evidence_detail>();
            //Retrieve already define primary evidence
            d.iCommand.CommandText = 
                string.Format("select {0}.{1},{0}.{2},{3},{4},{5},{6},{7} " +
                               "from {0},{8} " +
                               "where {0}.{1} = {8}.{9} "+
                               "and {0}.{2} = '{10}' and {5} = {11} and {6} = {12}",
                               Primary_evidence_status.FieldName.TABLE_NAME, Primary_evidence_status.FieldName.PRIMARY_EVIDENCE_NUM,
                               Primary_evidence_status.FieldName.CURRI_ID, Primary_evidence_status.FieldName.TEACHER_ID,
                               Primary_evidence_status.FieldName.STATUS,FieldName.ACA_YEAR,FieldName.INDICATOR_NUM,
                               FieldName.EVIDENCE_NAME,FieldName.TABLE_NAME,FieldName.PRIMARY_EVIDENCE_NUM,curri_id_param,
                               inddata.aca_year,inddata.indicator_num
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
                        result.Add(new Primary_evidence_detail
                        {
                            primary_evidence_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal]),
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
                            evidence_name = item.ItemArray[data.Columns[FieldName.EVIDENCE_NAME].Ordinal].ToString(),
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            status = Convert.ToChar(item.ItemArray[data.Columns[Primary_evidence_status.FieldName.STATUS].Ordinal]),
                            teacher_id = item.ItemArray[data.Columns[Primary_evidence_status.FieldName.TEACHER_ID].Ordinal].ToString()
                        });
                    }

                    data.Dispose();
                }
                else
                {
                    //Reserved for return error string
                }
                //Retrieve primary evidence which define by admin but not assign any responsible teacher yet
                res.Close();
                d.iCommand.CommandText =
                string.Format("select * from {0} where {1} = '0' and {2} = {3} and {4} = {5} " +
                              "and {0}.{6} not IN " +
                              "(select e.{7} from {8} as e where e.{9} = '{13}') and " +
                              "not exists(select * from {10} where {0}.{6} = {10}.{11} and {12} = '{13}')",
                              FieldName.TABLE_NAME,FieldName.CURRI_ID,FieldName.ACA_YEAR,inddata.aca_year,
                              FieldName.INDICATOR_NUM,inddata.indicator_num,FieldName.PRIMARY_EVIDENCE_NUM,
                              Exclusive_curriculum_evidence.FieldName.PRIMARY_EVIDENCE_NUM,
                              Exclusive_curriculum_evidence.FieldName.TABLE_NAME,
                              Exclusive_curriculum_evidence.FieldName.CURRI_ID,
                              Primary_evidence_status.FieldName.TABLE_NAME,
                              Primary_evidence_status.FieldName.PRIMARY_EVIDENCE_NUM,
                              Primary_evidence_status.FieldName.CURRI_ID,curri_id_param
                              );
                res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new Primary_evidence_detail
                        {
                            primary_evidence_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal]),
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
                            evidence_name = item.ItemArray[data.Columns[FieldName.EVIDENCE_NAME].Ordinal].ToString(),
                            curri_id = curri_id_param,
                            status = '6',
                            teacher_id = ""
                        });
                    }
                    data.Dispose();
                }
                else
                {

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

        public object UpdateDetail(List<Primary_evidence_detail> list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            string insertintoprimaryevidencecmd = "";
            string insertintoprievistatuscmd = "";
            string updateprievistatuscmd = "";
            string deletefromprimaryevidencecmd = "";
            string deletefromprievistatuscmd = "";
            string insertintoexclusiveprimaryevicmd = "";

            foreach(Primary_evidence_detail item in list)
            {
                if(item.status == '0' || item.status == '1' || item.status == '4' || item.status == '5')
                {
                    updateprievistatuscmd += string.Format("update {0} set {1} = '{2}' where {3} = {4} and {5} = '{6}' ",
                        Primary_evidence_status.FieldName.TABLE_NAME, Primary_evidence_status.FieldName.TEACHER_ID,
                        item.teacher_id, Primary_evidence_status.FieldName.PRIMARY_EVIDENCE_NUM, item.primary_evidence_num,
                        Primary_evidence_status.FieldName.CURRI_ID, item.curri_id);
                }
                else if(item.status == '3')
                {
                    deletefromprimaryevidencecmd += string.Format("delete from {0} where {1} = {2} ",
                        FieldName.TABLE_NAME, FieldName.PRIMARY_EVIDENCE_NUM, item.primary_evidence_num);
                }
                else if(item.status == '6')
                {
                    insertintoprievistatuscmd += string.Format("insert into {0} values ({1},'{2}','{3}',{4}) ",
                        Primary_evidence_status.FieldName.TABLE_NAME, item.primary_evidence_num, item.curri_id, item.teacher_id, '4');
                }

                else if(item.status == '7')
                {
                    deletefromprievistatuscmd += string.Format("delete from {0} where {1} = {2} and {3} = '{4}' ",
                        Primary_evidence_status.FieldName.TABLE_NAME, Primary_evidence_status.FieldName.PRIMARY_EVIDENCE_NUM,
                        item.primary_evidence_num, Primary_evidence_status.FieldName.CURRI_ID,
                        item.curri_id);
                    insertintoexclusiveprimaryevicmd += string.Format("insert into {0} values ({1},'{2}') ",
                        Exclusive_curriculum_evidence.FieldName.TABLE_NAME, item.primary_evidence_num, item.curri_id);
                }
                else if(item.status == '2')
                {
                    insertintoprimaryevidencecmd += string.Format("insert into {0} values ({1},{2},'{3}','{4}') ",
                        FieldName.TABLE_NAME, item.aca_year, item.indicator_num, item.curri_id, item.evidence_name);
                    insertintoprimaryevidencecmd += string.Format("insert into {0} values ({1},'{2}','{3}',{4}) ",
                        Primary_evidence_status.FieldName.TABLE_NAME,
                        string.Format("(select max({0}) from {1})",FieldName.PRIMARY_EVIDENCE_NUM,FieldName.TABLE_NAME)
                        ,item.curri_id, item.teacher_id, '0');
                }
            }

            d.iCommand.CommandText = String.Format("BEGIN {0} {1} {2} {3} {4} {5} END", insertintoprimaryevidencecmd,
                insertintoprievistatuscmd, updateprievistatuscmd, deletefromprimaryevidencecmd, deletefromprievistatuscmd,
                insertintoexclusiveprimaryevicmd);
            try
            {
                int rowAffected = d.iCommand.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    return null;
                }
                else
                {
                    return "No primary_evidence data are updated.";
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
        }

        public object UpdateDetail(List<Primary_evidence> list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            string insertintoprimaryevidencecmd = "";
            string deletefromprievi_and_clause = "";
            string deletefromprimaryevidencecmd = "";
            /*string insertintoprievistatuscmd = "";
            string updateprievistatuscmd = "";
            string deletefromprimaryevidencecmd = "";
            string deletefromprievistatuscmd = "";
            string insertintoexclusiveprimaryevicmd = "";*/

            foreach (Primary_evidence item in list)
            {
                if(item.primary_evidence_num == 0)
                {
                    insertintoprimaryevidencecmd += string.Format("insert into {0} values ({1},{2},'{3}','{4}') ",
                        FieldName.TABLE_NAME, item.aca_year, item.indicator_num, 0, item.evidence_name);
                }
                else
                {
                    //Delete before insert!
                    deletefromprievi_and_clause += string.Format("and {0} != {1} ",FieldName.PRIMARY_EVIDENCE_NUM,item.primary_evidence_num);
                }
            }
            if (deletefromprievi_and_clause != "")
            {
                deletefromprimaryevidencecmd = string.Format("delete from {0} where {1} = {2} " +
                    "and {3} = {4} and {5} = '0' and (1=1 {6})", FieldName.TABLE_NAME, FieldName.ACA_YEAR, list.First().aca_year,
                    FieldName.INDICATOR_NUM, list.First().indicator_num, FieldName.CURRI_ID, deletefromprievi_and_clause);
            }
            return null;
            /*
            d.iCommand.CommandText = String.Format("BEGIN {0} {1} {2} {3} {4} {5} END", insertintoprimaryevidencecmd,
                insertintoprievistatuscmd, updateprievistatuscmd, deletefromprimaryevidencecmd, deletefromprievistatuscmd,
                insertintoexclusiveprimaryevicmd);
            try
            {
                int rowAffected = d.iCommand.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    return null;
                }
                else
                {
                    return "No primary_evidence data are updated.";
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
            }*/
        }
    }
}