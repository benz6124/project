using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oTeacher : Teacher
    {
        public object SelectTeacherIdAndTName(string curri_id)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Teacher_with_t_name> result = new List<Teacher_with_t_name>();
            d.iCommand.CommandText = string.Format("select * from {0} where exists(select * from {1} where {0}.{2} = {1}.{3} and {4}='{5}')", 
                FieldName.TABLE_NAME, Curriculum_teacher_staff.FieldName.TABLE_NAME,FieldName.TEACHER_ID,Curriculum_teacher_staff.FieldName.PERSONNEL_ID,Curriculum_teacher_staff.FieldName.CURRI_ID,curri_id);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new Teacher_with_t_name
                        {
                            teacher_id = item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal].ToString(),
                            t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[FieldName.T_PRENAME].Ordinal].ToString()) + 
                                     item.ItemArray[data.Columns[FieldName.T_NAME].Ordinal].ToString()
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
    }
}