using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
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
                return WebApiApplication.CONNECTDBERRSTRING;
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

        public async Task<object> SelectWhere(string wherecond)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<oPrimary_evidence> result = new List<oPrimary_evidence>();
            d.iCommand.CommandText = string.Format("select * from {0} where {1}", FieldName.TABLE_NAME, wherecond);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
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

        public async Task<object> SelectWithDetail(oIndicator inddata,string curri_id_param)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
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
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
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
                            teacher_id = Convert.ToInt32(item.ItemArray[data.Columns[Primary_evidence_status.FieldName.TEACHER_ID].Ordinal])
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
                            teacher_id = 0
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

        public async Task<object> UpdateDetail(List<Primary_evidence_detail> list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
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
                    updateprievistatuscmd += string.Format("update {0} set {1} = {2} where {3} = {4} and {5} = '{6}' ",
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
                    insertintoprievistatuscmd += string.Format("insert into {0} values ({1},'{2}',{3},{4}) ",
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
                    if (item.evidence_name.Length > DBFieldDataType.EVIDENCE_NAME_LENGTH)
                        return "ชื่อของหลักฐานพื้นฐานบางส่วนที่ต้องการบันทึกมีขนาดที่ยาวเกินกำหนด";
                    insertintoprimaryevidencecmd += string.Format("insert into {0} values ({1},{2},'{3}','{4}') ",
                        FieldName.TABLE_NAME, item.aca_year, item.indicator_num, item.curri_id, item.evidence_name);
                    insertintoprimaryevidencecmd += string.Format("insert into {0} values ({1},'{2}',{3},{4}) ",
                        Primary_evidence_status.FieldName.TABLE_NAME,
                        string.Format("(select max({0}) from {1})",FieldName.PRIMARY_EVIDENCE_NUM,FieldName.TABLE_NAME)
                        ,item.curri_id, item.teacher_id, '0');
                }
            }

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} END", insertintoprimaryevidencecmd,
                insertintoprievistatuscmd, updateprievistatuscmd, deletefromprimaryevidencecmd, deletefromprievistatuscmd,
                insertintoexclusiveprimaryevicmd);
            try
            {
                await d.iCommand.ExecuteNonQueryAsync();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                //Whether it success or not it must close connection in order to end block
                d.SQLDisconnect();
            }
        }

        public async Task<object> UpdateDetail(List<Primary_evidence> list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            string insertintoprimaryevidencecmd = string.Format("insert into {0} values ",FieldName.TABLE_NAME);
            int inslength = insertintoprimaryevidencecmd.Length;
            string deletefromprievi_and_condition = "";
            string deletefromprimaryevidencecmd = "";
            string deletewhereclause = "";


            string temp1tablename = "#temp1";
            string temp2tablename = "#temp2";

            string createtabletemp1 = "";

            //Insert row which curriculum has assign teacher of the to-be delete primary evidence into main table
            string insertintotemp1 = "";
            string createtabletemp2 = "";
            string insertintotemp2 = "";
            string updatetemp2 = "";

            /*Insert into use the combine result from both table and insert in back to databaseeee*/
            string insertintoprimaryevidencestatusfromdeleted = "";
            string updateevidence = "";

            if (list.First().primary_evidence_num != -1)
            {
                foreach (Primary_evidence item in list)
                {
                    if (item.primary_evidence_num == 0)
                    {
                        if(insertintoprimaryevidencecmd.Length <= inslength)
                        insertintoprimaryevidencecmd += string.Format("({0},{1},'{2}','{3}') ",
                            item.aca_year, item.indicator_num, 0, item.evidence_name);
                        else
                            insertintoprimaryevidencecmd += string.Format(",({0},{1},'{2}','{3}') ",
                            item.aca_year, item.indicator_num, 0, item.evidence_name);
                    }
                    else
                    {
                        //Delete before insert!
                        deletefromprievi_and_condition += string.Format("and {0} != {1} ", FieldName.PRIMARY_EVIDENCE_NUM, item.primary_evidence_num);
                    }
                }
            }
            else
            {
                deletefromprievi_and_condition += "and 1=1";
            }

            if (deletefromprievi_and_condition != "")
            {
                deletewhereclause = string.Format("{0} = {1} and {2} = {3} and {4} = '0' and (1=1 {5})",
                    FieldName.ACA_YEAR, list.First().aca_year,
                    FieldName.INDICATOR_NUM, list.First().indicator_num, FieldName.CURRI_ID, deletefromprievi_and_condition);
                deletefromprimaryevidencecmd = string.Format("delete from {0} where {1} ", FieldName.TABLE_NAME,deletewhereclause);

                //Make statement for insert in primary_evidence,primary_evidence status and update evidence of to-be delete admin 's primary_evidence
                createtabletemp1 = string.Format(
            "CREATE TABLE {0} ( " +
            "[row_num] INT IDENTITY(1, 1) NOT NULL," +
            "[{1}] INT NOT NULL," +
            "PRIMARY KEY ([row_num])) ", temp1tablename, FieldName.PRIMARY_EVIDENCE_NUM);

                //Insert row which curriculum has assign teacher of the to-be delete primary evidence into main table
                insertintotemp1 = string.Format("insert into {0} " +
                                                "select * from (INSERT INTO {1}({2}, {3}, {4}, {5}) " +
                                                "OUTPUT Inserted.{6} ", temp1tablename, FieldName.TABLE_NAME,
                                                FieldName.ACA_YEAR, FieldName.INDICATOR_NUM, FieldName.CURRI_ID, FieldName.EVIDENCE_NAME, FieldName.PRIMARY_EVIDENCE_NUM)
                                                +
                                         string.Format("select t2.{0}, t2.{1}, t1.{2}, t2.{3} " +
                                         "from(select * from {4} as p1 where p1.{5} in " +
                                         "(select p2.{6} from {7} as p2 where {8})) as t1 " + /* {22222} is deletewhereclause*/
                                         "inner join {7} as t2 on t2.{6} = t1.{5}) as outputresult ",
                                         FieldName.ACA_YEAR, FieldName.INDICATOR_NUM, FieldName.CURRI_ID, FieldName.EVIDENCE_NAME,
                                         Primary_evidence_status.FieldName.TABLE_NAME, Primary_evidence_status.FieldName.PRIMARY_EVIDENCE_NUM,
                                         FieldName.PRIMARY_EVIDENCE_NUM, FieldName.TABLE_NAME, deletewhereclause);
                createtabletemp2 = string.Format("CREATE TABLE {0} (" +
                                                        "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                                        "[{1}] INT NOT NULL," +
                                                        "[{2}] {5} NOT NULL," +
                                                        "[{3}] INT NOT NULL," +
                                                        "[{4}] CHAR NOT NULL," +
                                                        "PRIMARY KEY([row_num])) " +
                                                        "ALTER TABLE {0} " +
                                                        "ALTER COLUMN [{2}] {5} COLLATE DATABASE_DEFAULT ", temp2tablename,
                                                        FieldName.PRIMARY_EVIDENCE_NUM, FieldName.CURRI_ID,
                                                        Primary_evidence_status.FieldName.TEACHER_ID,
                                                        Primary_evidence_status.FieldName.STATUS,
                                                        DBFieldDataType.CURRI_ID_TYPE);
                insertintotemp2 = string.Format("insert into {0} " +
                                                       "select t1.{1}, t1.{2}, t1.{3}, t1.{4} from " +
                                                       "(select * from {5} as p1 where p1.{1} in " +
                                                       "(select p2.{6} from {7} as p2 where {8})) as t1 ",// +
                                                      // "inner join {7} as t2 on t2.{6} = t1.{1} ",
                                                       temp2tablename, Primary_evidence_status.FieldName.PRIMARY_EVIDENCE_NUM, Primary_evidence_status.FieldName.CURRI_ID,
                                                       Primary_evidence_status.FieldName.TEACHER_ID, Primary_evidence_status.FieldName.STATUS,
                                                       Primary_evidence_status.FieldName.TABLE_NAME, FieldName.PRIMARY_EVIDENCE_NUM,
                                                       FieldName.TABLE_NAME, deletewhereclause);
                updatetemp2 = string.Format("UPDATE {0} SET {1} = '0' where {1} = '4' " +
                                                   "UPDATE {0} SET {1} = '1' where {1} = '5' ",
                                                   temp2tablename, Primary_evidence_status.FieldName.STATUS);

                /*Insert into use the combine result from both table and insert in back to databaseeee*/
                insertintoprimaryevidencestatusfromdeleted = string.Format("insert into {0} " +
                                      "select {1}.{2},{3},{4},{5} from {1} inner join {6} on {1}.row_num = {6}.row_num ",
                                      Primary_evidence_status.FieldName.TABLE_NAME, temp1tablename, FieldName.PRIMARY_EVIDENCE_NUM,
                                      Primary_evidence_status.FieldName.CURRI_ID, Primary_evidence_status.FieldName.TEACHER_ID,
                                      Primary_evidence_status.FieldName.STATUS, temp2tablename);
                updateevidence = string.Format("update {0} set {1} = temp3.new_primary_evidence_num " +
                                      "from {0} as evi inner join " +
                                      "(select {2}.{3} as new_primary_evidence_num,{4}.{3} as old_primary_evidence_num,{5},{6},{7} from {2} inner join {4} on {2}.row_num = {4}.row_num) as temp3 " +
                                      "on evi.{1} = temp3.old_primary_evidence_num and evi.{5} COLLATE DATABASE_DEFAULT = temp3.{5} ",
                                      Evidence.FieldName.TABLE_NAME, Evidence.FieldName.PRIMARY_EVIDENCE_NUM,
                                      temp1tablename, FieldName.PRIMARY_EVIDENCE_NUM, temp2tablename, Primary_evidence_status.FieldName.CURRI_ID,
                                      Primary_evidence_status.FieldName.TEACHER_ID, Primary_evidence_status.FieldName.STATUS);
                //droptemptable = string.Format("DROP TABLE {0} DROP TABLE {1} ", temp2tablename, temp1tablename);
            }
            if (insertintoprimaryevidencecmd.Length <= inslength)
                insertintoprimaryevidencecmd = "";
                d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} {6} {7} {8} END", createtabletemp1, insertintotemp1, createtabletemp2,
                insertintotemp2, updatetemp2, insertintoprimaryevidencestatusfromdeleted,
                updateevidence, deletefromprimaryevidencecmd, insertintoprimaryevidencecmd);
            try
            {
                await d.iCommand.ExecuteNonQueryAsync();
                return null;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                //Handle error from sql execution
                if (ex.Number == 8152)
                    return "ชื่อของหลักฐานพื้นฐานบางส่วนที่ต้องการเพิ่มมีขนาดที่ยาวเกินกำหนด";
                else
                    return ex.Message;
            }
            finally
            {
                //Whether it success or not it must close connection in order to end block
                d.SQLDisconnect();
            }
        }

        public async Task<object> SelectOnlyNameAndId(string p_curri_id,int p_aca_year,string p_teacher_id,int p_indicator_num)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<Primary_evidence_name_id_only> result = new List<Primary_evidence_name_id_only>();
            d.iCommand.CommandText = string.Format("select p1.{0},{1}.{2} from " +
                   "( select * from {3} where {4} = '{5}' and {6} = {7} and ({8} = '0' or {8} = '4') " +
                   "and {0} IN " +
                   "(select {14} from {1} where {9} = {10} and {11} = {12} and " +
                   "({13} = '0' OR {13} = '{5}'))) as p1 INNER JOIN {1} on p1.{0} = {1}.{14}",
                   Primary_evidence_status.FieldName.PRIMARY_EVIDENCE_NUM,FieldName.TABLE_NAME,FieldName.EVIDENCE_NAME,
                   Primary_evidence_status.FieldName.TABLE_NAME, Primary_evidence_status.FieldName.CURRI_ID,
                   p_curri_id, Primary_evidence_status.FieldName.TEACHER_ID,p_teacher_id,
                   Primary_evidence_status.FieldName.STATUS,FieldName.ACA_YEAR,p_aca_year,
                   FieldName.INDICATOR_NUM,p_indicator_num,FieldName.CURRI_ID,
                   FieldName.PRIMARY_EVIDENCE_NUM
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
                        result.Add(new Primary_evidence_name_id_only
                        {
                            primary_evidence_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PRIMARY_EVIDENCE_NUM].Ordinal]),
                            evidence_name = item.ItemArray[data.Columns[FieldName.EVIDENCE_NAME].Ordinal].ToString()
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

        public async Task<object> SelectPrimaryEvidenceWithTeacherDetail()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            Evidence_with_teacher_curri_indicator_detail result = new Evidence_with_teacher_curri_indicator_detail();
            d.iCommand.CommandText = string.Format("select {0},{1},{2}.{3},{2}.{4},{5},{6},{7},{8} " +
            "from {9},{2},{10},{11},{12} " +
            "where {9}.{13} = '{14}' and {9}.{15} = {16} " +
            "and {9}.{15} = {2}.{17} " +
            "and {10}.{18} = {9}.{13} " +
            "and {19} = {20} " +

            "and {12}.{21} = {2}.{4} " +
            "and {12}.{22} = (select max({12}.{22}) from {12} where {12}.{22} <= {2}.{3}) " +
            "and {12}.{22} = {2}.{3} ",
            FieldName.EVIDENCE_NAME, Cu_curriculum.FieldName.CURR_TNAME,/*2 primary_evidence*/FieldName.TABLE_NAME,
            FieldName.ACA_YEAR, FieldName.INDICATOR_NUM, Indicator.FieldName.INDICATOR_NAME_T,
            Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME, Teacher.FieldName.EMAIL,
            /*9*/Primary_evidence_status.FieldName.TABLE_NAME,/*10*/Cu_curriculum.FieldName.TABLE_NAME,
            /*11*/User_list.FieldName.TABLE_NAME,/*12*/Indicator.FieldName.TABLE_NAME,
            Primary_evidence_status.FieldName.CURRI_ID, curri_id, Primary_evidence_status.FieldName.PRIMARY_EVIDENCE_NUM,
            primary_evidence_num, FieldName.PRIMARY_EVIDENCE_NUM, Cu_curriculum.FieldName.CURRI_ID,
            Primary_evidence_status.FieldName.TEACHER_ID, User_list.FieldName.USER_ID,
            /*21*/Indicator.FieldName.INDICATOR_NUM, Indicator.FieldName.ACA_YEAR);

            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.evidence_name = item.ItemArray[data.Columns[FieldName.EVIDENCE_NAME].Ordinal].ToString();
                        result.aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]);
                        result.curr_tname = item.ItemArray[data.Columns[Cu_curriculum.FieldName.CURR_TNAME].Ordinal].ToString();
                        result.indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]);
                        result.indicator_name_t = item.ItemArray[data.Columns[Indicator.FieldName.INDICATOR_NAME_T].Ordinal].ToString();

                        result.t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) +
                                     item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString();

                        result.email = item.ItemArray[data.Columns[Teacher.FieldName.EMAIL].Ordinal].ToString();
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
        public async Task<object> SelectAllPendingPrimaryEvidence()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<Personnel_with_pending_primary_evidence> result = new List<Personnel_with_pending_primary_evidence>();
            d.iCommand.CommandText = string.Format("select {0},{1}.{2},{3}.{4},{1}.{5},{6},{7},{8},{9} " +
            "from {3}, {1}, {10}, {11} " +
            "where ({12} = '0' or {12} = '4') " +
            "and {3}.{13} = {1}.{14} " +
            "and {10}.{15} = {3}.{4} " +
            "and {0} = {16} ",
            Teacher.FieldName.TEACHER_ID,/*1*/FieldName.TABLE_NAME, FieldName.EVIDENCE_NAME,
            /*3*/Primary_evidence_status.FieldName.TABLE_NAME, Primary_evidence_status.FieldName.CURRI_ID,
            FieldName.ACA_YEAR, Cu_curriculum.FieldName.CURR_TNAME, Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
            Teacher.FieldName.EMAIL,
            /*10*/Cu_curriculum.FieldName.TABLE_NAME,/*11*/User_list.FieldName.TABLE_NAME,
            Primary_evidence_status.FieldName.STATUS, Primary_evidence_status.FieldName.PRIMARY_EVIDENCE_NUM,
            FieldName.PRIMARY_EVIDENCE_NUM, Cu_curriculum.FieldName.CURRI_ID, User_list.FieldName.USER_ID);

            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        int uid = Convert.ToInt32(item.ItemArray[data.Columns[Teacher.FieldName.TEACHER_ID].Ordinal]);
                        if (result.FirstOrDefault(t => t.teacher_id == uid) == null)
                            result.Add(new Personnel_with_pending_primary_evidence
                            {
                                email = item.ItemArray[data.Columns[Teacher.FieldName.EMAIL].Ordinal].ToString(),
                                teacher_id = uid,
                                t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) +
                                     item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString(),
                                pendinglist = new List<Evidence_brief_detail>()
                            });
                        result.First(t => t.teacher_id == uid).pendinglist.Add(new Evidence_brief_detail {
                            evidence_name = item.ItemArray[data.Columns[FieldName.EVIDENCE_NAME].Ordinal].ToString(),
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                            curr_tname = item.ItemArray[data.Columns[Cu_curriculum.FieldName.CURR_TNAME].Ordinal].ToString(),
                            curri_id = item.ItemArray[data.Columns[Primary_evidence_status.FieldName.CURRI_ID].Ordinal].ToString()
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