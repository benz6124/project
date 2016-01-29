using System;
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
    }
}