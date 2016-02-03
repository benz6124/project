using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
using educationalProject.Models.ViewModels;
namespace educationalProject.Models.Wrappers
{
    public class oCurriculum_teacher_staff : Curriculum_teacher_staff
    {
        public object InsertNewCurriculumTeacherStaffWithSelect(List<Curriculum_teacher_staff> list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Curriculum_teacher_staff_with_brief_detail> result = new List<Curriculum_teacher_staff_with_brief_detail>();

            string insertcmd = string.Format("insert into {0} values ", FieldName.TABLE_NAME);
            int len = insertcmd.Length;
            foreach (Curriculum_teacher_staff c in list)
            {
                if (insertcmd.Length <= len)
                    insertcmd += string.Format("('{0}','{1}')", c.personnel_id, c.curri_id);
                else
                    insertcmd += string.Format(",('{0}','{1}')", c.personnel_id, c.curri_id);
            }

            string selectcmd = ViewModels.Wrappers.oPersonnel.GetSelectWithCurriculumCommand(list.First().curri_id);

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
                        if (Convert.ToInt32(item.ItemArray[data.Columns["user_type_num"].Ordinal]) == 1)
                            result.Add(new Curriculum_teacher_staff_with_brief_detail
                            {
                                personnel_id = item.ItemArray[data.Columns[Personnel.FieldName.PERSONNEL_ID].Ordinal].ToString(),
                                t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Personnel.FieldName.T_PRENAME].Ordinal].ToString()) +
                                         item.ItemArray[data.Columns[Personnel.FieldName.T_NAME].Ordinal].ToString(),
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                file_name_pic = item.ItemArray[data.Columns[Personnel.FieldName.FILE_NAME_PIC].Ordinal].ToString(),
                                type = item.ItemArray[data.Columns[Personnel.FieldName.USER_TYPE].Ordinal].ToString()
                            });
                        else
                            result.Add(new Curriculum_teacher_staff_with_brief_detail
                            {
                                personnel_id = item.ItemArray[data.Columns[Personnel.FieldName.PERSONNEL_ID].Ordinal].ToString(),
                                t_name = item.ItemArray[data.Columns[Personnel.FieldName.T_PRENAME].Ordinal].ToString() +
                                     item.ItemArray[data.Columns[Personnel.FieldName.T_NAME].Ordinal].ToString(),
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                file_name_pic = item.ItemArray[data.Columns[Personnel.FieldName.FILE_NAME_PIC].Ordinal].ToString(),
                                type = item.ItemArray[data.Columns[Personnel.FieldName.USER_TYPE].Ordinal].ToString()
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



        public object Delete(List<Curriculum_teacher_staff> list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";


            string deletecmd = string.Format("delete from {0} where {1} = '{2}' ", FieldName.TABLE_NAME,
                FieldName.CURRI_ID,list.First().curri_id);
            string excludecond = "";
            foreach (Curriculum_teacher_staff c in list)
            {
                excludecond += string.Format("and {0} != '{1}' ", FieldName.PERSONNEL_ID, c.personnel_id);
            }
            deletecmd += excludecond;

            d.iCommand.CommandText = deletecmd;

            try
            {
                d.iCommand.ExecuteNonQuery();
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
            return null;
        }
    }
}