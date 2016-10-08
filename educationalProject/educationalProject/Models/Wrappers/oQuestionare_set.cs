using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oQuestionare_set : Questionare_set
    {
        private string getSelectByCurriculumAcademicCommand()
        {
            return string.Format("SELECT {0}.*,{1},{2}, " +
                "{3} = {4}" +
                ", {5} from {0}, {6}, {7}, {8} " +
                "where {9} = '{10}' and {11} = {12} " +
                "and {0}.{13} = {6}.{14} " +
                "and {15} = {16} " +
                "and {17}.{18} = {1} order by {19} desc ",
                FieldName.TABLE_NAME,Questionare_privilege.FieldName.PRIVILEGE_TYPE_ID,
                User_type.FieldName.USER_TYPE_NAME,User_list.FieldName.T_PRENAME,
                NameManager.GatherSQLCASEForPrename(User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_TYPE_ID, User_list.FieldName.T_PRENAME),
                User_list.FieldName.T_NAME,Questionare_privilege.FieldName.TABLE_NAME, /*6*/
                User_type.FieldName.TABLE_NAME /*7*/,
                User_list.FieldName.TABLE_NAME /*8*/,
                FieldName.CURRI_ID, curri_id, FieldName.ACA_YEAR, aca_year,
                FieldName.QUESTIONARE_SET_ID, Questionare_privilege.FieldName.QUESTIONARE_SET_ID,
                FieldName.PERSONNEL_ID,User_list.FieldName.USER_ID,
                User_type.FieldName.TABLE_NAME, User_type.FieldName.USER_TYPE_ID,
                FieldName.DATE
                );
        }

        public async Task<object> SelectWithDetail(oCurriculum_academic curriacadata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<Questionare_set_detail> result = new List<Questionare_set_detail>();

            curri_id = curriacadata.curri_id;
            aca_year = curriacadata.aca_year;
            d.iCommand.CommandText = getSelectByCurriculumAcademicCommand();

            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        int qid = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.QUESTIONARE_SET_ID].Ordinal]);
                        if (result.FirstOrDefault(t => t.questionare_set_id == qid) == null)
                        {
                                result.Add(new Questionare_set_detail
                                {
                                    aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                    name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                                    personnel_id = item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal]) : 0,
                                    questionare_set_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.QUESTIONARE_SET_ID].Ordinal]),
                                    date = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                                    curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                    t_name = item.ItemArray[data.Columns[User_list.FieldName.T_PRENAME].Ordinal].ToString() + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                                });
                        }
                        result.First(t => t.questionare_set_id == qid).target.Add(new User_type {
                            user_type_id = Convert.ToInt32(item.ItemArray[data.Columns[Questionare_privilege.FieldName.PRIVILEGE_TYPE_ID].Ordinal]),
                            user_type = item.ItemArray[data.Columns[User_type.FieldName.USER_TYPE_NAME].Ordinal].ToString()
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

        public async Task<object> SelectWithResult(int qid)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            Questionare_set_sub_result result = new Questionare_set_sub_result();
            result.main_result_list = new List<Questionare_set_main_result>();
            string temp1tablename = "#temp1";
            string createtabletemp1 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] INT NOT NULL," +
                                      "[{2}] INT NOT NULL," +
                                      "[{3}] VARCHAR(2000) NOT NULL," +
                                      "[{4}] VARCHAR(5000) NOT NULL," +
                                      "[count] INT NOT NULL," +
                                      "PRIMARY KEY([row_num])) " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {4} VARCHAR(5000) COLLATE DATABASE_DEFAULT " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {3} VARCHAR(2000) COLLATE DATABASE_DEFAULT ",
                                      temp1tablename,FieldName.QUESTIONARE_SET_ID,Questionare_question_obj.FieldName.QUESTIONARE_QUESTION_ID,
                                      Questionare_question_obj.FieldName.DETAIL,Questionare_result_obj.FieldName.ANSWER);
            string insertintotemp1_1 = string.Format("insert into {0} " +
                                       "select {1}, {2}, {3}, 0, 0 from " +
                                       "{4} where {1} = {5} ",
                                       temp1tablename, Questionare_question_obj.FieldName.QUESTIONARE_SET_ID,
                                       Questionare_question_obj.FieldName.QUESTIONARE_QUESTION_ID,
                                       Questionare_question_obj.FieldName.DETAIL,
                                       Questionare_question_obj.FieldName.TABLE_NAME, qid);

            string insertintotemp1_2 = string.Format("insert into {0} " +
                                       "select {1}, {2}, '{3}', {4}, count(*) from " +
                                       //Below select will query answer for each choice
                                       "(select {5}.{2}, {1},{4} from " +
                                       "{5}, {6} where " +
                                       "{7} = {8} and {5}.{2} = {6}.{9}) " +
                                       "as ansresult " +

                                       "group by {1}, {2}, {4} " +
                                       "order by {1}, {2}, {4} ",
                                       temp1tablename,FieldName.QUESTIONARE_SET_ID,Questionare_question_obj.FieldName.QUESTIONARE_QUESTION_ID,
                                       "",Questionare_result_obj.FieldName.ANSWER,
                                       Questionare_question_obj.FieldName.TABLE_NAME,Questionare_result_obj.FieldName.TABLE_NAME,
                                       Questionare_question_obj.FieldName.QUESTIONARE_SET_ID,qid,
                                       Questionare_result_obj.FieldName.QUESTIONARE_QUESTION_ID);

            string insertintotemp1_3 = string.Format("insert into {0} " +
                                       "select 0, 0, 0, {1}, 0 from {2} where {3} = {4} ",
                                       temp1tablename, Questionare_result_sub.FieldName.SUGGESTION, Questionare_result_sub.FieldName.TABLE_NAME,
                                       Questionare_result_sub.FieldName.QUESTIONARE_SET_ID, qid);

            string selectcmd = string.Format("select {0}, {1}, {2}, {3}, count from {4} "
                               ,Questionare_question_obj.FieldName.QUESTIONARE_SET_ID,
                               Questionare_question_obj.FieldName.QUESTIONARE_QUESTION_ID,
                               Questionare_question_obj.FieldName.DETAIL,
                               Questionare_result_obj.FieldName.ANSWER,
                               temp1tablename);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} END", createtabletemp1, insertintotemp1_1, insertintotemp1_2, insertintotemp1_3, selectcmd);
    
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    int questionare_question_id = -1;
                    int mainresultind = -1;
                    foreach (DataRow item in data.Rows)
                    {
                        int readqqid = Convert.ToInt32(item.ItemArray[data.Columns[Questionare_question_obj.FieldName.QUESTIONARE_QUESTION_ID].Ordinal]);

                        if (readqqid == 0)
                        {
                            result.suggestion.Add(item.ItemArray[data.Columns[Questionare_result_obj.FieldName.ANSWER].Ordinal].ToString());
                        }
                        //Read top row (count = 0)
                        else if (Convert.ToInt32(item.ItemArray[data.Columns["count"].Ordinal]) == 0)
                        {
                            result.main_result_list.Add(new Questionare_set_main_result(
                                item.ItemArray[data.Columns[Questionare_question_obj.FieldName.DETAIL].Ordinal].ToString()));
                        }
                        else
                        {
                            if (questionare_question_id != readqqid)
                            {
                                questionare_question_id = readqqid;
                                mainresultind++;
                            }
                            result.main_result_list[mainresultind].
                                answer[Convert.ToInt32(item.ItemArray[data.Columns[Questionare_result_obj.FieldName.ANSWER].Ordinal])-1] =
                                Convert.ToInt32(item.ItemArray[data.Columns["count"].Ordinal]);
                        }
                    }
                    data.Dispose();
                }
                else
                {
                    //Reserved for return error string
                    res.Close();
                    return "ไม่พบข้อมูลของแบบสอบถามที่ต้องการดูผล";
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

        public async Task<object> Delete(List<Questionare_set_detail> list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            string deleteprecmd = string.Format("DELETE FROM {0} WHERE {1} = '{2}' and {3} = {4}",
                FieldName.TABLE_NAME,FieldName.CURRI_ID,list.First().curri_id,FieldName.ACA_YEAR, list.First().aca_year);
            string excludecond = "1=1 ";
            foreach(Questionare_set_detail item in list)
            {
                excludecond += string.Format("and {0} != {1} ", FieldName.QUESTIONARE_SET_ID, item.questionare_set_id);
            }

            d.iCommand.CommandText = string.Format("{0} and ({1})", deleteprecmd, excludecond);
            try
            {
                await d.iCommand.ExecuteNonQueryAsync();
                return null;
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

        public async Task<object> InsertNewQuestionareWithSelect(Questionare_set_detail_full qdata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<Questionare_set_detail> result = new List<Questionare_set_detail>();
            string temp1tablename = "#temp1";
            string temp2tablename = "#temp2";
            string temp3tablename = "#temp3";
            string createtabletemp1 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] INT NOT NULL," +
                                      "PRIMARY KEY ([row_num])) ", temp1tablename, FieldName.QUESTIONARE_SET_ID);

            string createtabletemp2 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] VARCHAR(2000) NOT NULL," +
                                      "PRIMARY KEY ([row_num])) " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} VARCHAR(2000) COLLATE DATABASE_DEFAULT "
                                      , temp2tablename, Questionare_question_obj.FieldName.DETAIL);

             string createtabletemp3 = string.Format("create table {0} (" +
                                       "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                       "[{1}] INT NULL," +
                                       "PRIMARY KEY ([row_num])) "
                                       , temp3tablename, Questionare_privilege.FieldName.PRIVILEGE_TYPE_ID);


            string insertintotemp1 = string.Format("INSERT INTO {0} " +
                                     "select * from " +
                                     "(insert into {1} output inserted.{2} values " +
                                     "('{3}','{4}',{5},'{6}','{7}')) " +
                                     "as outputinsert ",
                                     temp1tablename, FieldName.TABLE_NAME, FieldName.QUESTIONARE_SET_ID,
                                     qdata.personnel_id, qdata.curri_id,qdata.aca_year, qdata.name,qdata.date);


            string insertintotemp2 = string.Format("INSERT INTO {0} VALUES (null)", temp2tablename);

            foreach (Questionare_question_obj q in qdata.my_questions)
            {
                    insertintotemp2 += string.Format(",('{0}')", q.detail);
            }

            string insertintotemp3 = string.Format("INSERT INTO {0} VALUES (null)", temp3tablename);

            foreach (User_type u in qdata.my_target)
            {
                insertintotemp3 += string.Format(",('{0}')", u.user_type_id);
            }

            string insertintoquestionareprivilege = string.Format(" INSERT INTO {0} " +
                                        "select {1},{2} from {3},{4} where {2} is not null ",
                                        Questionare_privilege.FieldName.TABLE_NAME, FieldName.QUESTIONARE_SET_ID, Questionare_privilege.FieldName.PRIVILEGE_TYPE_ID,
                                        temp1tablename, temp3tablename);

            string insertintoquestionarequestionobj = string.Format(" INSERT INTO {0} " +
                                        "select {1},{2} from {3},{4} where {2} is not null ",
                                        Questionare_question_obj.FieldName.TABLE_NAME, FieldName.QUESTIONARE_SET_ID, Questionare_question_obj.FieldName.DETAIL,
                                        temp1tablename, temp2tablename);

            curri_id = qdata.curri_id;
            aca_year = qdata.aca_year;

            string selectcmd = getSelectByCurriculumAcademicCommand();

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} {6} {7} {8} END", createtabletemp1, createtabletemp2,createtabletemp3,
                insertintotemp1, insertintotemp2, insertintotemp3, insertintoquestionareprivilege,
                insertintoquestionarequestionobj, selectcmd);

            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        int qid = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.QUESTIONARE_SET_ID].Ordinal]);
                        if (result.FirstOrDefault(t => t.questionare_set_id == qid) == null)
                        {
                            result.Add(new Questionare_set_detail
                            {
                                aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                                personnel_id = item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal]) : 0,
                                questionare_set_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.QUESTIONARE_SET_ID].Ordinal]),
                                date = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                t_name = item.ItemArray[data.Columns[User_list.FieldName.T_PRENAME].Ordinal].ToString() + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                            });
                        }
                        result.First(t => t.questionare_set_id == qid).target.Add(new User_type
                        {
                            user_type_id = Convert.ToInt32(item.ItemArray[data.Columns[Questionare_privilege.FieldName.PRIVILEGE_TYPE_ID].Ordinal]),
                            user_type = item.ItemArray[data.Columns[User_type.FieldName.USER_TYPE_NAME].Ordinal].ToString()
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