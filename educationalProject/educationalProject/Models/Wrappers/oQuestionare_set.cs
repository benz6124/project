﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oQuestionare_set : Questionare_set
    {
        public object Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oQuestionare_set> result = new List<oQuestionare_set>();
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
                        result.Add(new oQuestionare_set
                        {
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                            name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                            personnel_id = item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal].ToString(),
                            questionare_set_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.QUESTIONARE_SET_ID].Ordinal]),
                            date = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
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

        public object SelectWithDetail(oCurriculum_academic curriacadata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Questionare_set_detail> result = new List<Questionare_set_detail>();
            d.iCommand.CommandText = string.Format("select q.*,{0},{1} from " +
                "(select {2}.{3},{4},{5},{6},{7},{8},{9} " +
                "from {2}, {10} where {5} = '{11}' and {6} = {12} and {2}.{3} = {10}.{13}) as q " +
                "inner join {14} on q.{4} = {14}.{15}",Teacher.FieldName.T_PRENAME,
                Teacher.FieldName.T_NAME,FieldName.TABLE_NAME,FieldName.QUESTIONARE_SET_ID,FieldName.PERSONNEL_ID,
                FieldName.CURRI_ID,FieldName.ACA_YEAR,FieldName.NAME,FieldName.DATE,Questionare_privilege.FieldName.PRIVILEGE,
                Questionare_privilege.FieldName.TABLE_NAME,curriacadata.curri_id,curriacadata.aca_year,
                Questionare_privilege.FieldName.QUESTIONARE_SET_ID,Teacher.FieldName.TABLE_NAME,
                Teacher.FieldName.TEACHER_ID
                );
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    Questionare_set_detail curr = null;
                    questionare_set_id = -1;
                    foreach (DataRow item in data.Rows)
                    {
                        if(questionare_set_id != Convert.ToInt32(item.ItemArray[data.Columns[FieldName.QUESTIONARE_SET_ID].Ordinal]))
                        {
                            curr = new Questionare_set_detail
                            {
                                aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                                personnel_id = item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal].ToString(),
                                questionare_set_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.QUESTIONARE_SET_ID].Ordinal]),
                                date = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                            };
                            result.Add(curr);
                            questionare_set_id = curr.questionare_set_id;
                        }
                        curr.target.Add(item.ItemArray[data.Columns[Questionare_privilege.FieldName.PRIVILEGE].Ordinal].ToString());
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

        public object SelectWithResult(int qid)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
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
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
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

        public object Delete(List<Questionare_set_detail> list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
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
                int rowAffected = d.iCommand.ExecuteNonQuery();
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

    }
}