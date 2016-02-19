using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using educationalProject.Utils;
using educationalProject.Models.Wrappers;
namespace educationalProject.Models.ViewModels.Wrappers
{
    public class oTeacher_educational : Teacher_educational
    {
        public object SelectPresidentCurriAndAllTeacherInCurri(Curriculum_academic data)
        {
            int president = -1;
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oTeacher_educational> result = new List<oTeacher_educational>();
            result.Add(new oTeacher_educational());

            string selectpresid = string.Format("select {0} from {1} where {2} = '{3}' and {4} = {5} ",
            President_curriculum.FieldName.TEACHER_ID, President_curriculum.FieldName.TABLE_NAME,
            President_curriculum.FieldName.CURRI_ID, data.curri_id, President_curriculum.FieldName.ACA_YEAR,
            data.aca_year);

            string selteacher1 = string.Format("select {0}.*,{1},{2},{3},{4},{5} " +
            "from {0},{6} " +
            "where {7} = {8} and {9} = 'อาจารย์' " +
            "and exists(select * from {10} where {11} = '{12}' and {10}.{13} = {0}.{7}) ",
            User_list.FieldName.TABLE_NAME, Educational_teacher_staff.FieldName.DEGREE,
            Educational_teacher_staff.FieldName.PRE_MAJOR, Educational_teacher_staff.FieldName.MAJOR,
            Educational_teacher_staff.FieldName.GRAD_YEAR, Educational_teacher_staff.FieldName.COLLEGE,
            Educational_teacher_staff.FieldName.TABLE_NAME, Personnel.FieldName.USER_ID,
            Educational_teacher_staff.FieldName.PERSONNEL_ID, Personnel.FieldName.USER_TYPE,
            User_curriculum.FieldName.TABLE_NAME, User_curriculum.FieldName.CURRI_ID,
            data.curri_id, User_curriculum.FieldName.USER_ID);

            string selteacher2 = string.Format("select {0}.* from {0},{1} " +
            "where {0}.{2} = {1}.{3} and {4} = 'อาจารย์' and {5} = '{6}' " +
            "and not exists(select * from {7} where {8} = {0}.{2}) ",
            User_list.FieldName.TABLE_NAME, User_curriculum.FieldName.TABLE_NAME,
            User_list.FieldName.USER_ID, User_curriculum.FieldName.USER_ID, User_list.FieldName.USER_TYPE,
            User_curriculum.FieldName.CURRI_ID, data.curri_id, Educational_teacher_staff.FieldName.TABLE_NAME,
            Educational_teacher_staff.FieldName.PERSONNEL_ID);

                
            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} END", selectpresid, selteacher1,
                selteacher2);
            try
            {
                //Read teacher-eduhistory data with president curriculum's teacher id
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                do
                {
                    if (res.HasRows)
                    {
                        DataTable tabledata = new DataTable();
                        tabledata.Load(res);
                        foreach (DataRow item in tabledata.Rows)
                        {
                            if (!tabledata.Columns.Contains(FieldName.USER_TYPE))
                                president = Convert.ToInt32(item.ItemArray[tabledata.Columns[President_curriculum.FieldName.TEACHER_ID].Ordinal]);
                            else
                            {
                                int tid = Convert.ToInt32(item.ItemArray[tabledata.Columns[User_list.FieldName.USER_ID].Ordinal]);
                                if (result.FirstOrDefault(t => t.teacher_id == tid) == null)
                                    result.Add(new oTeacher_educational
                                    {
                                        username = item.ItemArray[tabledata.Columns[FieldName.USERNAME].Ordinal].ToString(),
                                        t_name = item.ItemArray[tabledata.Columns[FieldName.T_NAME].Ordinal].ToString(),
                                        e_name = item.ItemArray[tabledata.Columns[FieldName.E_NAME].Ordinal].ToString(),
                                        email = item.ItemArray[tabledata.Columns[FieldName.EMAIL].Ordinal].ToString(),
                                        teacher_id = tid,
                                        tel = item.ItemArray[tabledata.Columns[FieldName.TEL].Ordinal].ToString(),
                                        e_prename = item.ItemArray[tabledata.Columns[FieldName.E_PRENAME].Ordinal].ToString(),
                                        t_prename = NameManager.GatherPreName(item.ItemArray[tabledata.Columns[FieldName.T_PRENAME].Ordinal].ToString()),
                                        file_name_pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[tabledata.Columns[FieldName.FILE_NAME_PIC].Ordinal].ToString())
                                    });

                                if (tabledata.Columns.Contains(Educational_teacher_staff.FieldName.COLLEGE))
                                    result.First(t => t.teacher_id == tid).history.Add(new Educational_teacher_staff
                                    {
                                        college = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.COLLEGE].Ordinal].ToString(),
                                        degree = Convert.ToChar(item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.DEGREE].Ordinal]),
                                        grad_year = Convert.ToInt32(item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.GRAD_YEAR].Ordinal]),
                                        major = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.MAJOR].Ordinal].ToString(),
                                        pre_major = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.PRE_MAJOR].Ordinal].ToString(),
                                    });
                            }
                        }
                        tabledata.Dispose();
                    }
                    else if (!res.IsClosed) {
                        if (!res.NextResult())
                            break;
                    }
                } while (!res.IsClosed);
                    if (president != -1) {
                        //Find president curriculum in list and swap both
                        int swapindex = result.FindIndex(t => t.teacher_id == president);
                        ListUtils<oTeacher_educational>.Swap(result, 0, swapindex);
                        result.RemoveAt(swapindex);
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