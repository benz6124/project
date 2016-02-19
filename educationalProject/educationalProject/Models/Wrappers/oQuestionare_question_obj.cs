using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Utils;
using educationalProject.Models.ViewModels;
namespace educationalProject.Models.Wrappers
{
    public class oQuestionare_question_obj : Questionare_question_obj
    {
        public async Task<object> SelectByQuestionIdAsQuestionForm(int qid)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Questionare_question_answer> result = new List<Questionare_question_answer>();
            d.iCommand.CommandText = string.Format("select * from {0} where {1} = {2}", FieldName.TABLE_NAME,
                FieldName.QUESTIONARE_SET_ID, qid);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new Questionare_question_answer
                        {
                            detail = item.ItemArray[data.Columns[FieldName.DETAIL].Ordinal].ToString(),
                            questionare_set_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.QUESTIONARE_SET_ID].Ordinal]),
                            questionare_question_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.QUESTIONARE_QUESTION_ID].Ordinal])
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


        public async Task<object> InsertQuestionAnswer(Questionare_question_form qdata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";

            string insertintoquestionareresobj = string.Format("insert into {0} values ",Questionare_result_obj.FieldName.TABLE_NAME);
            
            foreach(Questionare_question_answer item in qdata.question_list)
            {
                insertintoquestionareresobj += string.Format("({0},{1})", item.questionare_question_id, item.answer);
                if (item != qdata.question_list.Last())
                    insertintoquestionareresobj += ",";
            }

            string insertintoquestionareressub = string.Format("insert into {0} values ({1},'{2}')",
                Questionare_result_sub.FieldName.TABLE_NAME, qdata.question_list.First().questionare_set_id,
                qdata.suggestion);

            string truecasecmd = string.Format("BEGIN {0} {1} END", insertintoquestionareresobj, insertintoquestionareressub);
            d.iCommand.CommandText = string.Format("if exists (select * from {0} where {1} = {2}) {3} else return ",
                Questionare_set.FieldName.TABLE_NAME, Questionare_set.FieldName.QUESTIONARE_SET_ID, qdata.question_list.First().questionare_set_id,
                truecasecmd);
            try
            {
                int rowaffacted = await d.iCommand.ExecuteNonQueryAsync();
                if (rowaffacted > 0)
                    return null;
                else
                    return "The to-be answer questionare is already deleted.";
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