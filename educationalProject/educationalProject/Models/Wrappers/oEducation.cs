using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oEducation : Educational_teacher_staff
    {
        private string GetSelectEducationByPersonnelIdCommand()
        {
            return string.Format("select * from {0} where {1} = {2} ",
                FieldName.TABLE_NAME, FieldName.PERSONNEL_ID, ParameterName.PERSONNEL_ID);
        }
        public async Task<object> Insert()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<Educational_teacher_staff> result = new List<Educational_teacher_staff>();
            string insertcmd = string.Format("if not exists(select * from {7} where {8} = {1} and {9} = 'นักศึกษา') BEGIN " +
                "insert into {0} values ({1},{2},{3},{4},{5},{6}) ",
                FieldName.TABLE_NAME, ParameterName.PERSONNEL_ID, ParameterName.DEGREE, ParameterName.PRE_MAJOR, ParameterName.MAJOR, ParameterName.GRAD_YEAR, ParameterName.COLLEGE,
                User_list.FieldName.TABLE_NAME,User_list.FieldName.USER_ID,User_list.FieldName.USER_TYPE);
            string selectcmd = GetSelectEducationByPersonnelIdCommand();

            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.PERSONNEL_ID, personnel_id));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.DEGREE, degree));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.PRE_MAJOR, pre_major));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.MAJOR, major));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.GRAD_YEAR, grad_year));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.COLLEGE, college));
            d.iCommand.CommandText = string.Format("BEGIN {0} {1} END END",insertcmd,selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new Educational_teacher_staff
                        {
                            personnel_id = this.personnel_id,
                            college = item.ItemArray[data.Columns[FieldName.COLLEGE].Ordinal].ToString(),
                            degree = Convert.ToChar(item.ItemArray[data.Columns[FieldName.DEGREE].Ordinal]),
                            education_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EDUCATION_ID].Ordinal]),
                            grad_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.GRAD_YEAR].Ordinal]),
                            major = item.ItemArray[data.Columns[FieldName.MAJOR].Ordinal].ToString(),
                            pre_major = item.ItemArray[data.Columns[FieldName.PRE_MAJOR].Ordinal].ToString()
                        });
                    }
                    data.Dispose();
                }
                else
                {
                    //Reserved for return error string
                    res.Close();
                    return "Student cannot insert education data.";
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
        public async Task<object> Update()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<Educational_teacher_staff> result = new List<Educational_teacher_staff>();
            string updatecmd = string.Format("update {0} set {1} = {2},{3} = {4},{5} = {6},{7} = {8},{9} = {10} where {11} = {12} ",
                FieldName.TABLE_NAME, FieldName.DEGREE, ParameterName.DEGREE, FieldName.PRE_MAJOR, ParameterName.PRE_MAJOR, FieldName.MAJOR, ParameterName.MAJOR, FieldName.GRAD_YEAR, ParameterName.GRAD_YEAR, FieldName.COLLEGE, ParameterName.COLLEGE, FieldName.EDUCATION_ID, ParameterName.EDUCATION_ID);
            string selectcmd = GetSelectEducationByPersonnelIdCommand();
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.PERSONNEL_ID, personnel_id));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.DEGREE, degree));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.PRE_MAJOR, pre_major));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.MAJOR, major));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.GRAD_YEAR, grad_year));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.COLLEGE, college));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.EDUCATION_ID, education_id));
            d.iCommand.CommandText = string.Format("BEGIN {0} {1} END", updatecmd, selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new Educational_teacher_staff
                        {
                            personnel_id = this.personnel_id,
                            college = item.ItemArray[data.Columns[FieldName.COLLEGE].Ordinal].ToString(),
                            degree = Convert.ToChar(item.ItemArray[data.Columns[FieldName.DEGREE].Ordinal]),
                            education_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EDUCATION_ID].Ordinal]),
                            grad_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.GRAD_YEAR].Ordinal]),
                            major = item.ItemArray[data.Columns[FieldName.MAJOR].Ordinal].ToString(),
                            pre_major = item.ItemArray[data.Columns[FieldName.PRE_MAJOR].Ordinal].ToString()
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