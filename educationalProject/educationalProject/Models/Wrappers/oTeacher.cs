using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
using educationalProject.Models.ViewModels.Wrappers;
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


        public object SelectPresidentCurriAndAllTeacherInCurri(Curriculum_academic data)
        {
            string president;
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oTeacher_educational> result = new List<oTeacher_educational>();
            d.iCommand.CommandText = string.Format
                ("select * from " +
                 "(select p1.{0} from {1} as p1 where p1.{2} = '{3}' and p1.{4} = {5}) as r1," +
                 "(select * from {6} as p2 inner join(select * from {7} where {8} = '{3}') as rr1 on p2.{9} = rr1.{10}) as r2 " +
                 "inner join {11} on r2.{9} = {11}.{12}",
                 FieldName.TEACHER_ID, President_curriculum.FieldName.TABLE_NAME, President_curriculum.FieldName.CURRI_ID,
                 data.curri_id, President_curriculum.FieldName.ACA_YEAR, data.aca_year, FieldName.TABLE_NAME,
                 Curriculum_teacher_staff.FieldName.TABLE_NAME, Curriculum_teacher_staff.FieldName.CURRI_ID, FieldName.TEACHER_ID,
                 Curriculum_teacher_staff.FieldName.PERSONNEL_ID, Educational_teacher_staff.FieldName.TABLE_NAME,
                 Educational_teacher_staff.FieldName.PERSONNEL_ID);
            try
            {
                //Read teacher-eduhistory data with president curriculum's teacher id
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable tabledata = new DataTable();
                    tabledata.Load(res);
                    president = tabledata.Rows[0].ItemArray[0].ToString();
                    oTeacher_educational t = null;
                    teacher_id = "-1";
                    foreach (DataRow item in tabledata.Rows)
                    {
                        if (item.ItemArray[1].ToString() != teacher_id)
                        {
                            t = new oTeacher_educational
                            {
                                addr = item.ItemArray[tabledata.Columns[FieldName.ADDR].Ordinal].ToString(),
                                username = item.ItemArray[tabledata.Columns[FieldName.USERNAME].Ordinal].ToString(),
                                t_name = item.ItemArray[tabledata.Columns[FieldName.T_NAME].Ordinal].ToString(),
                                e_name = item.ItemArray[tabledata.Columns[FieldName.E_NAME].Ordinal].ToString(),
                                email = item.ItemArray[tabledata.Columns[FieldName.EMAIL].Ordinal].ToString(),
                                gender = Convert.ToChar(item.ItemArray[tabledata.Columns[FieldName.GENDER].Ordinal]),
                                degree = Convert.ToChar(item.ItemArray[18]),
                                teacher_id = item.ItemArray[1].ToString(),
                                tel = item.ItemArray[tabledata.Columns[FieldName.TEL].Ordinal].ToString(),
                                e_prename = item.ItemArray[tabledata.Columns[FieldName.E_PRENAME].Ordinal].ToString(),
                                t_prename = NameManager.GatherPreName(item.ItemArray[tabledata.Columns[FieldName.T_PRENAME].Ordinal].ToString()),
                                personnel_type = item.ItemArray[tabledata.Columns[FieldName.PERSONNEL_TYPE].Ordinal].ToString(),
                                status = item.ItemArray[tabledata.Columns[FieldName.STATUS].Ordinal].ToString(),
                                user_type = item.ItemArray[tabledata.Columns[FieldName.USER_TYPE].Ordinal].ToString(),
                                file_name_pic = item.ItemArray[tabledata.Columns[FieldName.FILE_NAME_PIC].Ordinal].ToString(),
                                position = Convert.ToChar(item.ItemArray[tabledata.Columns[FieldName.POSITION].Ordinal]),
                                is_admin = Convert.ToChar(item.ItemArray[tabledata.Columns[FieldName.IS_ADMIN].Ordinal]),
                                
                            };
                            teacher_id = t.teacher_id;
                            result.Add(t);
                        }
                        t.history.Add(new Educational_teacher_staff
                        {
                            college = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.COLLEGE].Ordinal].ToString(),
                            degree = Convert.ToChar(item.ItemArray[27]),
                            grad_year = Convert.ToInt32(item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.GRAD_YEAR].Ordinal]),
                            major = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.MAJOR].Ordinal].ToString(),
                            personnel_id = item.ItemArray[26].ToString(),
                            pre_major = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.PRE_MAJOR].Ordinal].ToString(),
                        });
                    }
                    res.Close();
                    tabledata.Dispose();
                    d.iCommand.CommandText = string.Format("select * from {0} where " +
                                       "{1} IN(select {2} from {3} where {4} = '{5}') and " +
                                       "not exists(select * from {6} where {0}.{1} = {6}.{7})",
                                       FieldName.TABLE_NAME, FieldName.TEACHER_ID, Curriculum_teacher_staff.FieldName.PERSONNEL_ID,
                                       Curriculum_teacher_staff.FieldName.TABLE_NAME, Curriculum_teacher_staff.FieldName.CURRI_ID,
                                       data.curri_id, Educational_teacher_staff.FieldName.TABLE_NAME, Educational_teacher_staff.FieldName.PERSONNEL_ID);
                    res = d.iCommand.ExecuteReader();
                    //read teacher data without eduhistory
                    if (res.HasRows)
                    {
                        tabledata = new DataTable();
                        tabledata.Load(res);
                        foreach (DataRow item in tabledata.Rows)
                        {
                            result.Add(new oTeacher_educational
                            {
                                addr = item.ItemArray[tabledata.Columns[FieldName.ADDR].Ordinal].ToString(),
                                username = item.ItemArray[tabledata.Columns[FieldName.USERNAME].Ordinal].ToString(),
                                t_name = item.ItemArray[tabledata.Columns[FieldName.T_NAME].Ordinal].ToString(),
                                e_name = item.ItemArray[tabledata.Columns[FieldName.E_NAME].Ordinal].ToString(),
                                email = item.ItemArray[tabledata.Columns[FieldName.EMAIL].Ordinal].ToString(),
                                gender = Convert.ToChar(item.ItemArray[tabledata.Columns[FieldName.GENDER].Ordinal]),
                                degree = Convert.ToChar(item.ItemArray[tabledata.Columns[FieldName.DEGREE].Ordinal]),
                                teacher_id = item.ItemArray[tabledata.Columns[FieldName.TEACHER_ID].Ordinal].ToString(),
                                tel = item.ItemArray[tabledata.Columns[FieldName.TEL].Ordinal].ToString(),
                                e_prename = item.ItemArray[tabledata.Columns[FieldName.E_PRENAME].Ordinal].ToString(),
                                t_prename = NameManager.GatherPreName(item.ItemArray[tabledata.Columns[FieldName.T_PRENAME].Ordinal].ToString()),
                                personnel_type = item.ItemArray[tabledata.Columns[FieldName.PERSONNEL_TYPE].Ordinal].ToString(),
                                status = item.ItemArray[tabledata.Columns[FieldName.STATUS].Ordinal].ToString(),
                                user_type = item.ItemArray[tabledata.Columns[FieldName.USER_TYPE].Ordinal].ToString(),
                                file_name_pic = item.ItemArray[tabledata.Columns[FieldName.FILE_NAME_PIC].Ordinal].ToString(),
                                position = Convert.ToChar(item.ItemArray[tabledata.Columns[FieldName.POSITION].Ordinal]),
                                is_admin = Convert.ToChar(item.ItemArray[tabledata.Columns[FieldName.IS_ADMIN].Ordinal]),
                            });
                        }
                        res.Close();
                        tabledata.Dispose();
                    }
                    //Find president curriculum in list and swap both
                    int swapindex = 0;
                    foreach (oTeacher_educational i in result)
                    {
                        if (i.teacher_id == president)
                            break;
                        swapindex++;
                    }
                    ListUtils<oTeacher_educational>.Swap(result, 0, swapindex);
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