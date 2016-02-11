using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oEducation : Educational_teacher_staff
    {
        private string GetSelectEducationByPersonnelIdCommand()
        {
            return string.Format("select * from {0} where {1} = {2} ",
                FieldName.TABLE_NAME, FieldName.PERSONNEL_ID, personnel_id);
        }
        public object Insert()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Educational_teacher_staff> result = new List<Educational_teacher_staff>();
            string insertcmd = string.Format("insert into {0} values ({1},'{2}','{3}','{4}',{5},'{6}') ",
                FieldName.TABLE_NAME, personnel_id, degree, pre_major, major, grad_year, college);
            string selectcmd = GetSelectEducationByPersonnelIdCommand();
            d.iCommand.CommandText = string.Format("BEGIN {0} {1} END",insertcmd,selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
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
        public object Update()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Educational_teacher_staff> result = new List<Educational_teacher_staff>();
            string insertcmd = string.Format("update {0} set {1} = '{2}',{3} = '{4}',{5} = '{6}',{7} = {8},{9} = '{10}' where {11} = {12} ",
                FieldName.TABLE_NAME, FieldName.DEGREE, degree, FieldName.PRE_MAJOR, pre_major, FieldName.MAJOR, major, FieldName.GRAD_YEAR, grad_year, FieldName.COLLEGE, college, FieldName.EDUCATION_ID, education_id);
            string selectcmd = GetSelectEducationByPersonnelIdCommand();
            d.iCommand.CommandText = string.Format("BEGIN {0} {1} END", insertcmd, selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
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