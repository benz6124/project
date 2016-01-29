using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
using educationalProject.Models.ViewModels;
namespace educationalProject.Models.Wrappers
{
    public class oQuestionare_question_obj : Questionare_question_obj
    {
        public object SelectByQuestionId(int qid)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oQuestionare_question_obj> result = new List<oQuestionare_question_obj>();
            d.iCommand.CommandText = string.Format("select * from {0} where {1} = {2}", FieldName.TABLE_NAME,
                FieldName.QUESTIONARE_SET_ID,qid);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oQuestionare_question_obj
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


        public object SelectByQuestionIdAsQuestionForm(int qid)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Questionare_question_answer> result = new List<Questionare_question_answer>();
            d.iCommand.CommandText = string.Format("select * from {0} where {1} = {2}", FieldName.TABLE_NAME,
                FieldName.QUESTIONARE_SET_ID, qid);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
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
    }
}