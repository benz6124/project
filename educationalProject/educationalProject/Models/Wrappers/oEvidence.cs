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
        public object SelectWhere(string wherecond)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
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
                return WebApiApplication.CONNECTDBERRSTRING;
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
                return WebApiApplication.CONNECTDBERRSTRING;
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
                return WebApiApplication.CONNECTDBERRSTRING;
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

        public async Task<object> SelectAllEvidenceWithCurriculumList()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            Dictionary<string, Curriculum_aca_year_evidence_list> result = new Dictionary<string, Curriculum_aca_year_evidence_list>();
            string selectcurrinamewithacayear = string.Format("select {0}.{1},{2},{3} " +
                                                "from {0},{4} " +
                                                "where {0}.{1} = {4}.{5} ",
                                                Cu_curriculum.FieldName.TABLE_NAME, Cu_curriculum.FieldName.CURRI_ID,
                                                Cu_curriculum.FieldName.CURR_TNAME, Curriculum_academic.FieldName.ACA_YEAR,
                                                Curriculum_academic.FieldName.TABLE_NAME, Curriculum_academic.FieldName.CURRI_ID);

            string selectallevidence = string.Format("select {0}, {1}, {2}, {3}, {4}, {5} from {6} ",
                                       FieldName.EVIDENCE_NAME, FieldName.INDICATOR_NUM, FieldName.EVIDENCE_REAL_CODE, FieldName.CURRI_ID, FieldName.ACA_YEAR,
                                       FieldName.FILE_NAME, FieldName.TABLE_NAME);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} END", selectcurrinamewithacayear, selectallevidence);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                do
                {
                    if (res.HasRows)
                    {
                        DataTable data = new DataTable();
                        data.Load(res);
                        if (data.Columns.Count == 3) //read curriculum academic data
                        {
                            foreach (DataRow item in data.Rows)
                            {
                                string curr_tname = item.ItemArray[data.Columns[Cu_curriculum.FieldName.CURR_TNAME].Ordinal].ToString();

                                //Add curriculum as key if not exists!
                                if (!result.ContainsKey(curr_tname))
                                {
                                    result.Add(curr_tname, new Curriculum_aca_year_evidence_list());
                                    result[curr_tname].curri_id = item.ItemArray[data.Columns[Cu_curriculum.FieldName.CURRI_ID].Ordinal].ToString();
                                }

                                //add curriculum aca_year to all_years list and create year key in 'in_year' property
                                result[curr_tname].all_years.Add(item.ItemArray[data.Columns[Curriculum_academic.FieldName.ACA_YEAR].Ordinal].ToString());
                                result[curr_tname].in_year.Add(result[curr_tname].all_years.Last(), new Evidence_in_year_list());

                            }

                        }
                        else //read evidence data
                        {
                            foreach (DataRow item in data.Rows)
                            {
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString();
                                aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]);
                                evidence_name = item.ItemArray[data.Columns[FieldName.EVIDENCE_NAME].Ordinal].ToString();

                                //Find the target curriculum which evidence will be add to
                                Curriculum_aca_year_evidence_list c = result.First(t => t.Value.curri_id == curri_id).Value;
                                //If evidence name is not exist in preferred aca_year => add it noww
                                if (!c.in_year[aca_year.ToString()].detail_evidence.ContainsKey(evidence_name))
                                {
                                    c.in_year[aca_year.ToString()].all_evidences.Add(evidence_name);
                                    c.in_year[aca_year.ToString()].detail_evidence.Add(evidence_name, new Evidence_tiny_detail
                                    {
                                        path = item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString(),
                                        code = item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal].ToString() + "-" +
                                        item.ItemArray[data.Columns[FieldName.EVIDENCE_REAL_CODE].Ordinal].ToString()
                                    });
                                }
                                else
                                {
                                    int readcode = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]);
                                    //handling duplicate evidence name by use indicator num...
                                    if (readcode == indicator_num)
                                    {
                                        c.in_year[aca_year.ToString()].detail_evidence[evidence_name].path = item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString();
                                        c.in_year[aca_year.ToString()].detail_evidence[evidence_name].code = readcode.ToString() + "-" +
                                        item.ItemArray[data.Columns[FieldName.EVIDENCE_REAL_CODE].Ordinal].ToString();
                                    }
                                }
                            }
                        }
                        data.Dispose();
                    }
                    else if (!res.IsClosed)
                    {
                        if (!res.NextResult())
                            break;
                    }
                } while (!res.IsClosed);
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

        public async Task<object> SelectAllEvidenceWithCurriculum2()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<object> result = new List<object>();
            d.iCommand.CommandText = string.Format("select {0},{1},{2},{3},{4},{5} " +
                                     "from {6}, {7} " +
                                     "where {7}.{8} = {6}.{9} and {6}.{9} = {10} " +
                                     "order by {0},{1},{2},{3} ",
                                     Cu_curriculum.FieldName.CURR_TNAME, FieldName.ACA_YEAR, FieldName.INDICATOR_NUM, FieldName.EVIDENCE_REAL_CODE,
                                     FieldName.EVIDENCE_NAME, FieldName.FILE_NAME, FieldName.TABLE_NAME, Cu_curriculum.FieldName.TABLE_NAME,
                                     Cu_curriculum.FieldName.CURRI_ID, FieldName.CURRI_ID,ParameterName.CURRI_ID);
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURRI_ID, curri_id));

            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        string indnum = item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal].ToString();
                        string evicode = item.ItemArray[data.Columns[FieldName.EVIDENCE_REAL_CODE].Ordinal].ToString();
                        string evidencestr = string.Format("{0}, {1}, [เอกสารที่ {2}-{3}], {4}",
                            item.ItemArray[data.Columns[Cu_curriculum.FieldName.CURR_TNAME].Ordinal].ToString(),
                            item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal].ToString(),
                            indnum,
                            evicode,
                            item.ItemArray[data.Columns[FieldName.EVIDENCE_NAME].Ordinal].ToString()
                            );
                        List<string> pathanddocnumlist = new List<string> { WebApiApplication.SERVERURL + item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString(), indnum + "-" + evicode };
                        result.Add(new List<object> { evidencestr, pathanddocnumlist });
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


        public async Task<object> InsertNewEvidenceWithSelect(List<oEvidence> existslist)
        {
            DBConnector d = new DBConnector();
            List<Evidence_with_t_name> evidencelistresult = new List<Evidence_with_t_name>();
            List<string> filenametodellist = new List<string>();
            BulkEvidenceTransactionResult result = new BulkEvidenceTransactionResult();
            result.mainresult = evidencelistresult;
            result.filenametodellist = filenametodellist;

            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;

            //UPDATE PART
            string updatecmd = "";
            string delete_and_condition = "";
            string deletewhereclause = "";
            string temp1tablename = "#temp1";
            string createtabletemp1 = "";
            string insertintotemp1 = "";
            string getfilenamefromtemp1 = "";

            if (existslist.Count != 0)
            {
                foreach (oEvidence item in existslist)
                {
                    updatecmd += string.Format("update {0} set {1} = {2},{3} = '{4}' where {5} = {6} ",
                        FieldName.TABLE_NAME, FieldName.EVIDENCE_REAL_CODE, item.evidence_real_code, FieldName.EVIDENCE_NAME,
                        item.evidence_name, FieldName.EVIDENCE_CODE, item.evidence_code);
                    //Generate delete cmd
                    delete_and_condition += string.Format("and {0} != {1} ", FieldName.EVIDENCE_CODE, item.evidence_code);
                }
            }
            deletewhereclause = string.Format("{0} = '{1}' and {2} = {3} and {4} = {5} and (1 = 1 {6})",
                                FieldName.CURRI_ID, curri_id,
                                FieldName.ACA_YEAR, aca_year, FieldName.INDICATOR_NUM, indicator_num,
                                delete_and_condition);

            createtabletemp1 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] {2} NOT NULL," +
                                      "PRIMARY KEY([row_num])) " +
                                      /*Alter column in temp table to make table accept Thai data*/
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} {2} COLLATE DATABASE_DEFAULT ",
                                      temp1tablename, FieldName.FILE_NAME, DBFieldDataType.FILE_NAME_TYPE);

            insertintotemp1 = string.Format("insert into {0}({1}) " +
                                     "select * from (delete from {2} output Deleted.{1} where {3}) " +
                                     "as outputdelete ", temp1tablename, FieldName.FILE_NAME, FieldName.TABLE_NAME,
                                     deletewhereclause);

            //Query to-be delete file name from db
            getfilenamefromtemp1 = string.Format("select {1} as FILE_NAME_TO_DELETE from {0} where {1} not in (select {1} from {2}) ",
                temp1tablename, FieldName.FILE_NAME, FieldName.TABLE_NAME);

            string updateevidencetransactcmd = string.Format("BEGIN {0} {1} {2} {3} END", updatecmd, createtabletemp1, insertintotemp1, getfilenamefromtemp1);


            //INSERT PART
            string ifexistscond = string.Format("if not exists (select * from {0} where {1} = {2} and {3} = '{4}' and {5} = {6} and {7} = {8}) ",
                FieldName.TABLE_NAME, FieldName.INDICATOR_NUM, indicator_num, FieldName.CURRI_ID,
                curri_id, FieldName.ACA_YEAR, aca_year, FieldName.EVIDENCE_REAL_CODE, evidence_real_code);
            ;
            string insertcmd = string.Format("INSERT INTO {0}({1},{2},{3},{4},{5},{6},{7},{8},{9}) VALUES " +
                      "(null, '{10}', '{11}', {12}, {13}, '{14}', '{15}', '{16}', {17}) ",
                FieldName.TABLE_NAME, FieldName.PRIMARY_EVIDENCE_NUM, FieldName.TEACHER_ID, FieldName.CURRI_ID, FieldName.INDICATOR_NUM, FieldName.EVIDENCE_REAL_CODE, FieldName.FILE_NAME, FieldName.EVIDENCE_NAME, FieldName.SECRET, FieldName.ACA_YEAR,
                teacher_id, curri_id, indicator_num, evidence_real_code, file_name, evidence_name, secret, aca_year);
            string selectcmd = string.Format("select e.*,{13}.{10},{13}.{11} from (select * from {0} " +
                "where {1} = {2} and {3} = '{4}' and {5} = {6}) as e inner join ({7}) as {13} on e.{8} = {13}.{9} order by e.{12}",
                FieldName.TABLE_NAME, FieldName.INDICATOR_NUM, indicator_num, FieldName.CURRI_ID,
                curri_id, FieldName.ACA_YEAR, aca_year, oTeacher.getSelectTeacherByJoinCommand(),
                FieldName.TEACHER_ID, Teacher.FieldName.TEACHER_ID, Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                FieldName.EVIDENCE_REAL_CODE, Teacher.FieldName.ALIAS_NAME
                );

            string selecterrormsg = "select 'inserterror' as errormsg";
            d.iCommand.CommandText = string.Format("{4} {0} BEGIN {1} END ELSE BEGIN {3} END {2}", ifexistscond, insertcmd, selectcmd, selecterrormsg, updateevidencetransactcmd);
            
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();

                do
                {
                    if (res.HasRows)
                    {
                        DataTable data = new DataTable();
                        data.Load(res);

                        //Case current resultset is evidence(with teacher name) table
                        if (data.Columns.Contains("evidence_name"))
                        {
                            foreach (DataRow item in data.Rows)
                            {
                                evidencelistresult.Add(new Evidence_with_t_name
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
                        }

                        //Case current resultset is temp1 table (file_name_to_delete list)
                        else if (data.Columns.Contains("FILE_NAME_TO_DELETE"))
                        {
                            foreach (DataRow item in data.Rows)
                            {
                                filenametodellist.Add(
                                item.ItemArray[data.Columns["FILE_NAME_TO_DELETE"].Ordinal].ToString()
                                );
                            }
                        }

                        //Case current resultset is inserterror
                        else
                        {
                            result.message = "รหัสหลักฐานดังกล่าวมีอยู่แล้วในระบบ";
                        }
                        data.Dispose();
                    }
                    else if (!res.IsClosed)
                    {
                        if (!res.NextResult())
                            break;
                    }
                } while (!res.IsClosed);
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


        public async Task<object> InsertNewPrimaryEvidenceWithSelect(List<oEvidence> existslist)
        {
            int errcode = 0;
            DBConnector d = new DBConnector();

            List<Evidence_with_t_name> evidencelistresult = new List<Evidence_with_t_name>();
            List<string> filenametodellist = new List<string>();
            BulkEvidenceTransactionResult result = new BulkEvidenceTransactionResult();
            result.mainresult = evidencelistresult;
            result.filenametodellist = filenametodellist;

            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;

            //UPDATE PART
            string updatecmd = "";
            string delete_and_condition = "";
            string deletewhereclause = "";
            string temp1tablename = "#temp1";
            string createtabletemp1 = "";
            string insertintotemp1 = "";
            string getfilenamefromtemp1 = "";

            if (existslist.Count != 0)
            {
                foreach (oEvidence item in existslist)
                {
                    updatecmd += string.Format("update {0} set {1} = {2},{3} = '{4}' where {5} = {6} ",
                        FieldName.TABLE_NAME, FieldName.EVIDENCE_REAL_CODE, item.evidence_real_code, FieldName.EVIDENCE_NAME,
                        item.evidence_name, FieldName.EVIDENCE_CODE, item.evidence_code);
                    //Generate delete cmd
                    delete_and_condition += string.Format("and {0} != {1} ", FieldName.EVIDENCE_CODE, item.evidence_code);
                }
            }
            deletewhereclause = string.Format("{0} = '{1}' and {2} = {3} and {4} = {5} and (1 = 1 {6})",
                                FieldName.CURRI_ID, curri_id,
                                FieldName.ACA_YEAR, aca_year, FieldName.INDICATOR_NUM, indicator_num,
                                delete_and_condition);

            createtabletemp1 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] {2} NOT NULL," +
                                      "PRIMARY KEY([row_num])) " +
                                      /*Alter column in temp table to make table accept Thai data*/
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} {2} COLLATE DATABASE_DEFAULT ",
                                      temp1tablename, FieldName.FILE_NAME, DBFieldDataType.FILE_NAME_TYPE);

            insertintotemp1 = string.Format("insert into {0}({1}) " +
                                     "select * from (delete from {2} output Deleted.{1} where {3}) " +
                                     "as outputdelete ", temp1tablename, FieldName.FILE_NAME, FieldName.TABLE_NAME,
                                     deletewhereclause);

            //Query to-be delete file name from db
            getfilenamefromtemp1 = string.Format("select {1} as FILE_NAME_TO_DELETE from {0} where {1} not in (select {1} from {2}) ",
                temp1tablename, FieldName.FILE_NAME, FieldName.TABLE_NAME);

            string updateevidencetransactcmd = string.Format("BEGIN {0} {1} {2} {3} END", updatecmd, createtabletemp1, insertintotemp1, getfilenamefromtemp1);


            //INSERT PRIMARY EVIDENCE PART
            //The first condition check that is target primary evidence is already uploaded?
            string ifexistscond1 = string.Format("if exists (select * from {0} where {1} = {2} and {3} = {4} and {5} = '{6}' and {7} = {8} ) select 1 as errcode ",
                FieldName.TABLE_NAME, FieldName.PRIMARY_EVIDENCE_NUM, primary_evidence_num, FieldName.ACA_YEAR, aca_year,
                FieldName.CURRI_ID, curri_id, FieldName.INDICATOR_NUM, indicator_num);

            //The second condition check that is evidence real code of to be add target primary evidence is aleady exists?
            string ifexistscond2 = string.Format("if exists (select * from {0} where {1} = {2} and {3} = '{4}' and {5} = {6} and {7} = {8}) select 2 as errcode ",
                FieldName.TABLE_NAME, FieldName.INDICATOR_NUM, indicator_num, FieldName.CURRI_ID,
                curri_id, FieldName.ACA_YEAR, aca_year, FieldName.EVIDENCE_REAL_CODE, evidence_real_code);

            //The third condition check that is primary target evidence is exist in primary evidence database
            string ifexistscond3 = string.Format("if not exists (select * from {0} where {1} = {2}) select 3 as errcode ",
                Primary_evidence.FieldName.TABLE_NAME, Primary_evidence.FieldName.PRIMARY_EVIDENCE_NUM, primary_evidence_num);

            //The fourth condition check that is teacher(user) have responsibility to add target primary evidence?
            string ifexistscond4 = string.Format("if not exists (select * from {0} where {1} = {2} and {3} = '{4}' and {5} = {6}) select 4 as errcode ",
                Primary_evidence_status.FieldName.TABLE_NAME, Primary_evidence_status.FieldName.TEACHER_ID, teacher_id,
                Primary_evidence_status.FieldName.CURRI_ID, curri_id, Primary_evidence_status.FieldName.PRIMARY_EVIDENCE_NUM, primary_evidence_num);


            string insertintoevidencecmd = string.Format("INSERT INTO {0}({1},{2},{3},{4},{5},{6},{7},{8},{9}) VALUES " +
                      "({18}, '{10}', '{11}', {12}, {13}, '{14}', '{15}', '{16}', {17}) ",
                FieldName.TABLE_NAME, FieldName.PRIMARY_EVIDENCE_NUM, FieldName.TEACHER_ID, FieldName.CURRI_ID, FieldName.INDICATOR_NUM, FieldName.EVIDENCE_REAL_CODE, FieldName.FILE_NAME, FieldName.EVIDENCE_NAME, FieldName.SECRET, FieldName.ACA_YEAR,
                teacher_id, curri_id, indicator_num, evidence_real_code, file_name, evidence_name, secret, aca_year, primary_evidence_num);

            string updateprimaryevidencestatuscmd = string.Format("update {0} set {1} = '1' where {1} = '0' and {2} = {3} and {4} = '{5}' " +
                              "update {0} set {1} = '5' where {1} = '4' and {2} = {3} and {4} = '{5}' ",
                        Primary_evidence_status.FieldName.TABLE_NAME, Primary_evidence_status.FieldName.STATUS,
                        Primary_evidence_status.FieldName.PRIMARY_EVIDENCE_NUM, primary_evidence_num,
                        Primary_evidence_status.FieldName.CURRI_ID, curri_id);

            string selectcmd = string.Format("select e.*,{13}.{10},{13}.{11} from (select * from {0} " +
                "where {1} = {2} and {3} = '{4}' and {5} = {6}) as e inner join ({7}) as {13} on e.{8} = {13}.{9} order by e.{12} ",
                FieldName.TABLE_NAME, FieldName.INDICATOR_NUM, indicator_num, FieldName.CURRI_ID,
                curri_id, FieldName.ACA_YEAR, aca_year, oTeacher.getSelectTeacherByJoinCommand(),
                FieldName.TEACHER_ID, Teacher.FieldName.TEACHER_ID, Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                FieldName.EVIDENCE_REAL_CODE, Teacher.FieldName.ALIAS_NAME);

            d.iCommand.CommandText = string.Format("{7} {0} " +
                "else {1} " +
                "else {2} " +
                "else {3} " +
                "else begin " +
                "{4} {5} {6} end", ifexistscond1, ifexistscond2, ifexistscond3, ifexistscond4, insertintoevidencecmd, updateprimaryevidencestatuscmd,
                selectcmd,updateevidencetransactcmd);

            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();

                do
                {
                    if (res.HasRows)
                    {
                        DataTable data = new DataTable();
                        data.Load(res);

                        //Case current resultset is evidence(with teacher name) table
                        if (data.Columns.Contains("evidence_name"))
                        {
                            foreach (DataRow item in data.Rows)
                            {
                                evidencelistresult.Add(new Evidence_with_t_name
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
                        }

                        //Case current resultset is temp1 table (file_name_to_delete list)
                        else if (data.Columns.Contains("FILE_NAME_TO_DELETE"))
                        {
                            foreach (DataRow item in data.Rows)
                            {
                                filenametodellist.Add(
                                item.ItemArray[data.Columns["FILE_NAME_TO_DELETE"].Ordinal].ToString()
                                );
                            }
                        }

                        //Case current resultset is errcode
                        else
                        {
                            errcode = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns["errcode"].Ordinal]);
                        }

                        data.Dispose();
                    }
                    else if (!res.IsClosed)
                    {
                        if (!res.NextResult())
                            break;
                    }
                } while (!res.IsClosed);
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
            if (errcode == 0)
                return result;
            else if (errcode == 1)
                return "หลักฐานพื้นฐานดังกล่าวได้ถูกอัพโหลดไปก่อนแล้ว";
            else if (errcode == 2)
                return "รหัสหลักฐานดังกล่าวมีอยู่แล้วในระบบ";
            else if (errcode == 3)
                return "ไม่พบข้อมูลหลักฐานพื้นฐานดังกล่าวในระบบ";
            else
                return "ท่านไม่มีสิทธิในการอัพโหลดหลักฐานพื้นฐานดังกล่าว";
        }

        public async Task<object> Update(List<Evidence_with_t_name> list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;

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
                return WebApiApplication.CONNECTDBERRSTRING;

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
 