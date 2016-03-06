using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
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
            d.iCommand.CommandText =  string.Format("select * from {0}", FieldName.TABLE_NAME);
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
                            teacher_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal]),
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
                        result.Add(new oEvidence
                        {
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                            evidence_code = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EVIDENCE_CODE].Ordinal]),
                            evidence_name = item.ItemArray[data.Columns[FieldName.EVIDENCE_NAME].Ordinal].ToString(),
                            file_name = item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString(),
                            secret = Convert.ToChar(item.ItemArray[data.Columns[FieldName.SECRET].Ordinal]),
                            teacher_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal]),
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
        
        public async Task<object> SelectByIndicatorAndCurriculum(oIndicator inddata, string curri_id)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oEvidence> result = new List<oEvidence>();
            d.iCommand.CommandText = string.Format("select * from {0} " + 
                "where {1} = {2} and {3} = '{4}' and {5} = {6} order by {7} ", 
                FieldName.TABLE_NAME,FieldName.INDICATOR_NUM,inddata.indicator_num,FieldName.CURRI_ID,
                curri_id,FieldName.ACA_YEAR,inddata.aca_year,FieldName.EVIDENCE_REAL_CODE);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
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
                            teacher_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal]),
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

        public async Task<object> SelectByIndicatorAndCurriculumWithTName(oIndicator inddata, string curri_id)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Evidence_with_t_name> result = new List<Evidence_with_t_name>();
            d.iCommand.CommandText = string.Format("select e.*,{13}.{10},{13}.{11} from (select * from {0} " +
                "where {1} = {2} and {3} = '{4}' and {5} = {6}) as e inner join ({7}) as {13} on e.{8} = {13}.{9} order by e.{12}",
                FieldName.TABLE_NAME, FieldName.INDICATOR_NUM, inddata.indicator_num, FieldName.CURRI_ID,
                curri_id, FieldName.ACA_YEAR, inddata.aca_year,oTeacher.getSelectTeacherByJoinCommand(),
                FieldName.TEACHER_ID,Teacher.FieldName.TEACHER_ID,Teacher.FieldName.T_PRENAME,Teacher.FieldName.T_NAME,
                FieldName.EVIDENCE_REAL_CODE,Teacher.FieldName.ALIAS_NAME
                );
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
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
                            teacher_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal]),
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

        public async Task<object> SelectByCurriculumAcademic()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Evidence_with_t_name> result = new List<Evidence_with_t_name>();
            d.iCommand.CommandText = string.Format("select * from {0} where {1} = '{2}' and {3} = {4}",
                FieldName.TABLE_NAME, FieldName.CURRI_ID,
                curri_id, FieldName.ACA_YEAR, aca_year);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
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

        public async Task<object> InsertNewEvidenceWithSelect()
        {
            DBConnector d = new DBConnector();
            List<Evidence_with_t_name> result = new List<Evidence_with_t_name>();
            if (!d.SQLConnect())
                return "Cannot connect to database.";

            d.iCommand.CommandText =
                //Insert part    
                string.Format("INSERT INTO {0}({1},{2},{3},{4},{5},{6},{7},{8},{9}) VALUES " +
                      "(null, '{10}', '{11}', {12}, {13}, '{14}', '{15}', '{16}', {17}) ",
                FieldName.TABLE_NAME, FieldName.PRIMARY_EVIDENCE_NUM,FieldName.TEACHER_ID,FieldName.CURRI_ID,FieldName.INDICATOR_NUM,FieldName.EVIDENCE_REAL_CODE,FieldName.FILE_NAME, FieldName.EVIDENCE_NAME,FieldName.SECRET,FieldName.ACA_YEAR,
                teacher_id, curri_id, indicator_num, evidence_real_code, file_name, evidence_name, secret, aca_year) +
                //Select part
                string.Format("select e.*,{13}.{10},{13}.{11} from (select * from {0} " +
                "where {1} = {2} and {3} = '{4}' and {5} = {6}) as e inner join ({7}) as {13} on e.{8} = {13}.{9} order by e.{12}",
                FieldName.TABLE_NAME, FieldName.INDICATOR_NUM, indicator_num, FieldName.CURRI_ID,
                curri_id, FieldName.ACA_YEAR, aca_year, oTeacher.getSelectTeacherByJoinCommand(),
                FieldName.TEACHER_ID, Teacher.FieldName.TEACHER_ID, Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                FieldName.EVIDENCE_REAL_CODE,Teacher.FieldName.ALIAS_NAME
                );

            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
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
                            teacher_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal]),
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


        public async Task<object> InsertNewPrimaryEvidenceWithSelect()
        {
            DBConnector d = new DBConnector();
            List<Evidence_with_t_name> result = new List<Evidence_with_t_name>();
            if (!d.SQLConnect())
                return "Cannot connect to database.";

            d.iCommand.CommandText = string.Format("if not exists (select * from {0} where {1} = {2} and {3} = {4} and {5} = '{6}' and {7} = {8} ) ",
                FieldName.TABLE_NAME, FieldName.PRIMARY_EVIDENCE_NUM, primary_evidence_num, FieldName.ACA_YEAR,aca_year, 
                FieldName.CURRI_ID,curri_id, FieldName.INDICATOR_NUM,indicator_num) +
                "BEGIN " +
                //Insert part    
                string.Format("INSERT INTO {0}({1},{2},{3},{4},{5},{6},{7},{8},{9}) VALUES " +
                      "({18}, '{10}', '{11}', {12}, {13}, '{14}', '{15}', '{16}', {17}) ",
                FieldName.TABLE_NAME, FieldName.PRIMARY_EVIDENCE_NUM, FieldName.TEACHER_ID, FieldName.CURRI_ID, FieldName.INDICATOR_NUM, FieldName.EVIDENCE_REAL_CODE, FieldName.FILE_NAME, FieldName.EVIDENCE_NAME, FieldName.SECRET, FieldName.ACA_YEAR,
                teacher_id, curri_id, indicator_num, evidence_real_code, file_name, evidence_name, secret, aca_year, primary_evidence_num) +
                //Update primary_evidence_status part
                string.Format("update {0} set {1} = '1' where {1} = '0' and {2} = {3} and {4} = '{5}' " +
                              "update {0} set {1} = '5' where {1} = '4' and {2} = {3} and {4} = '{5}' ",
                        Primary_evidence_status.FieldName.TABLE_NAME, Primary_evidence_status.FieldName.STATUS,
                        Primary_evidence_status.FieldName.PRIMARY_EVIDENCE_NUM, primary_evidence_num,
                        Primary_evidence_status.FieldName.CURRI_ID, curri_id) +
//Select part
string.Format("select e.*,{13}.{10},{13}.{11} from (select * from {0} " +
                "where {1} = {2} and {3} = '{4}' and {5} = {6}) as e inner join ({7}) as {13} on e.{8} = {13}.{9} order by e.{12}",
                FieldName.TABLE_NAME, FieldName.INDICATOR_NUM, indicator_num, FieldName.CURRI_ID,
                curri_id, FieldName.ACA_YEAR, aca_year, oTeacher.getSelectTeacherByJoinCommand(),
                FieldName.TEACHER_ID, Teacher.FieldName.TEACHER_ID, Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                FieldName.EVIDENCE_REAL_CODE,Teacher.FieldName.ALIAS_NAME
                ) + " END";

            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
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
                            teacher_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal]),
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
                    res.Close();
                    return "หลักฐานพื้นฐานดังกล่าวได้ถูกอัพโหลดไปก่อนแล้ว";
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

        public async Task<object> Update(List<Evidence_with_t_name> list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";

            string updatecmd = "";
            string delete_and_condition = "";
            string deletewhereclause = "";
            string temp1tablename = "#temp1";
            string createtabletemp1 = "";
            string insertintotemp1 = "";
            string getfilenamefromtemp1 = "";

            if (list.First().evidence_code != 0)
            {
                foreach (Evidence_with_t_name item in list)
                {
                    updatecmd += string.Format("update {0} set {1} = {2},{3} = '{4}' where {5} = {6} ",
                        FieldName.TABLE_NAME, FieldName.EVIDENCE_REAL_CODE, item.evidence_real_code, FieldName.EVIDENCE_NAME,
                        item.evidence_name, FieldName.EVIDENCE_CODE, item.evidence_code);
                    //Generate delete cmd
                    delete_and_condition += string.Format("and {0} != {1} ", FieldName.EVIDENCE_CODE, item.evidence_code);
                }
            }
                deletewhereclause = string.Format("{0} = '{1}' and {2} = {3} and {4} = {5} and (1 = 1 {6})",
                                    FieldName.CURRI_ID, list.First().curri_id,
                                    FieldName.ACA_YEAR, list.First().aca_year, FieldName.INDICATOR_NUM, list.First().indicator_num,
                                    delete_and_condition);

                createtabletemp1 = string.Format("create table {0} (" +
                                          "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                          "[{1}] {2} NOT NULL," +
                                          "PRIMARY KEY([row_num])) " +
                                          /*Alter column in temp table to make table accept Thai data*/
                                          "ALTER TABLE {0} " +
                                          "ALTER COLUMN {1} {2} COLLATE DATABASE_DEFAULT ",
                                          temp1tablename, FieldName.FILE_NAME,DBFieldDataType.FILE_NAME_TYPE);

                insertintotemp1 = string.Format("insert into {0}({1}) " +
                                         "select * from (delete from {2} output Deleted.{1} where {3}) " +
                                         "as outputdelete ", temp1tablename, FieldName.FILE_NAME, FieldName.TABLE_NAME,
                                         deletewhereclause);
                //Query to-be delete file name from db
                getfilenamefromtemp1 = string.Format("select {1} from {0} where {1} not in (select {1} from {2}) ", 
                    temp1tablename, FieldName.FILE_NAME,FieldName.TABLE_NAME);
            
            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} END", updatecmd, createtabletemp1, insertintotemp1, getfilenamefromtemp1);
            List<string> strlist = new List<string>();
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        strlist.Add(
                        item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString()
                        );
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
            return strlist;
        }


        public async Task<object> UpdateEvidenceWithSelect()
        {
            DBConnector d = new DBConnector();
            List<Evidence_with_t_name> result = new List<Evidence_with_t_name>();
            if (!d.SQLConnect())
                return "Cannot connect to database.";

            string updatecmd = "";
            string temp1tablename = "#temp1";
            string createtabletemp1 = "";
            string insertintotemp1 = "";
            string mainselectcmd = "";
            string updatetemp1forcountfilecmd = "";

                updatecmd += string.Format("update {0} set {1} = '{2}',{3} = '{4}',{7} = '{8}' output Deleted.{3} where {5} = {6} ",
                    FieldName.TABLE_NAME, FieldName.SECRET, secret, FieldName.FILE_NAME,
                    file_name, FieldName.EVIDENCE_CODE, evidence_code,FieldName.TEACHER_ID,teacher_id);

            createtabletemp1 = string.Format("create table {0} (" +
                          "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                          "[{1}] VARCHAR(300) NOT NULL," +
                          "PRIMARY KEY([row_num])) " +
                          /*Alter column in temp table to make table accept Thai data*/
                          "ALTER TABLE {0} " +
                          "ALTER COLUMN {1} VARCHAR(300) COLLATE DATABASE_DEFAULT ",
                          temp1tablename, FieldName.FILE_NAME);

            insertintotemp1 = string.Format("insert into {0}({1}) " +
                                     "select * from ({2}) " +
                                     "as outputupdate ", temp1tablename, FieldName.FILE_NAME, updatecmd);

            updatetemp1forcountfilecmd = string.Format("update {0} set {1} = " +
                                         "(select cast(c as varchar(20)) + {1} from {0}," +
                                         "(select count(*) as c from {2} where {1} in " +
                                         "(select {1} from {0})) as count_file_op) where row_num = 1 ",
                                         temp1tablename, FieldName.FILE_NAME, FieldName.TABLE_NAME);


            string selectfromevidencecmd = string.Format("select e.*,{12}.{10},{12}.{11} from (select * from {0} " +
                "where {1} = {2} and {3} = '{4}' and {5} = {6}) as e inner join ({7}) as {12} on e.{8} = {12}.{9}",
                FieldName.TABLE_NAME, FieldName.INDICATOR_NUM, indicator_num, FieldName.CURRI_ID,
                curri_id, FieldName.ACA_YEAR, aca_year, oTeacher.getSelectTeacherByJoinCommand(),
                FieldName.TEACHER_ID, Teacher.FieldName.TEACHER_ID, Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                Teacher.FieldName.ALIAS_NAME
                );

            mainselectcmd = string.Format("select {0}.{1},evires.* from {0},({2}) as evires order by evires.{3}",
                                temp1tablename, FieldName.FILE_NAME, selectfromevidencecmd, FieldName.EVIDENCE_REAL_CODE);


            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} END", createtabletemp1, insertintotemp1, updatetemp1forcountfilecmd, mainselectcmd);
            

            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    //set to-be deleted file_name back to object (will use by caller later.) 
                    file_name = data.Rows[0].ItemArray[0].ToString();
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new Evidence_with_t_name
                        {
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                            evidence_code = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EVIDENCE_CODE].Ordinal]),
                            evidence_name = item.ItemArray[data.Columns[FieldName.EVIDENCE_NAME].Ordinal].ToString(),
                            file_name = item.ItemArray[7].ToString(),
                            secret = Convert.ToChar(item.ItemArray[data.Columns[FieldName.SECRET].Ordinal]),
                            teacher_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal]),
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
 