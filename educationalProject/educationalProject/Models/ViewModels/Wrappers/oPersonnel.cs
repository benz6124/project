using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Utils;
using educationalProject.Models.Wrappers;
namespace educationalProject.Models.ViewModels.Wrappers
{
    public class oPersonnel : Personnel
    {
        private static readonly string USER_ID = "USER_ID";

        public static string GetSelectWithCurriculumCommand(string curri_id)
        {
            return string.Format("select {6}.{0},{1},{2},{3},{4},{5} from {6},{7} " +
                          "where {8} = '{9}' " +
                          "and {6}.{0} = {7}.{10} order by {5} ",
                          User_list.FieldName.USER_ID, FieldName.T_PRENAME,
                          FieldName.T_NAME, FieldName.CURRI_ID,
                          FieldName.FILE_NAME_PIC, FieldName.USER_TYPE, User_list.FieldName.TABLE_NAME,
                          User_curriculum.FieldName.TABLE_NAME, User_curriculum.FieldName.CURRI_ID,
                          curri_id, User_curriculum.FieldName.USER_ID);
        }
        public async Task<object> SelectPersonnelIdAndTName(string curri_id,int selectmode) 
            //selectmode 0 : only teacher and staff,1 for all
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Personnel_with_t_name> result = new List<Personnel_with_t_name>();

            if(selectmode == 0)
            {
                d.iCommand.CommandText = string.Format("select {4}.{0},{1},{2},{3} from {4},{5} " +
                                         "where {6} = '{7}' " +
                                         "and {4}.{0} = {5}.{8} " +
                                         "and ({3} = 'อาจารย์' or {3} = 'เจ้าหน้าที่') ",
                                         FieldName.USER_ID, FieldName.T_PRENAME,
                                         Student.FieldName.T_NAME, FieldName.USER_TYPE,
                                         User_list.FieldName.TABLE_NAME, User_curriculum.FieldName.TABLE_NAME,
                                         FieldName.CURRI_ID, curri_id, User_curriculum.FieldName.USER_ID);
            }
            else
            {
                d.iCommand.CommandText = string.Format("select {4}.{0},{1},{2},{3} from {4},{5} " +
                                         "where {6} = '{7}' " +
                                         "and {4}.{0} = {5}.{8} " ,
                                         FieldName.USER_ID, FieldName.T_PRENAME,
                                         Student.FieldName.T_NAME, FieldName.USER_TYPE,
                                         User_list.FieldName.TABLE_NAME, User_curriculum.FieldName.TABLE_NAME,
                                         FieldName.CURRI_ID, curri_id, User_curriculum.FieldName.USER_ID);
            }

            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        if(item.ItemArray[data.Columns[FieldName.USER_TYPE].Ordinal].ToString() == "อาจารย์")
                        result.Add(new Personnel_with_t_name
                        {
                            user_id = Convert.ToInt32(item.ItemArray[data.Columns[USER_ID].Ordinal]),
                            t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) +
                                     item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                        });
                        else
                            result.Add(new Personnel_with_t_name
                            {
                                user_id = Convert.ToInt32(item.ItemArray[data.Columns[USER_ID].Ordinal]),
                                t_name = item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString() +
                                     item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
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

        public async Task<object> selectWithFullDetail(string curri_id_data)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Personnel_educational> result = new List<Personnel_educational>();

            string selpersonnel1 = string.Format("select {0}.*,{1},{2}.{3},{4},{5},{6},{7} " +
            "from {0},{2},{8} " +
            "where {9} = {10} and {11} = 'อาจารย์' " +
            "and {12} = {9} " +
            "and exists(select * from {13} where {14} = '{15}' and {13}.{16} = {0}.{9}) ",
            User_list.FieldName.TABLE_NAME,Teacher.FieldName.ROOM,Educational_teacher_staff.FieldName.TABLE_NAME,
            Educational_teacher_staff.FieldName.DEGREE, Educational_teacher_staff.FieldName.PRE_MAJOR,
            Educational_teacher_staff.FieldName.MAJOR, Educational_teacher_staff.FieldName.GRAD_YEAR,
            Educational_teacher_staff.FieldName.COLLEGE,Teacher.FieldName.TABLE_NAME,
            User_list.FieldName.USER_ID,Educational_teacher_staff.FieldName.PERSONNEL_ID,
            User_list.FieldName.USER_TYPE,Teacher.FieldName.TEACHER_ID /***12***/,
            User_curriculum.FieldName.TABLE_NAME,User_curriculum.FieldName.CURRI_ID,curri_id_data,
            User_curriculum.FieldName.USER_ID
            );

            string selpersonnel2 = string.Format("select {0}.*,{1} " +
            "from {0},{2},{3} " +
            "where {0}.{4} = {2}.{5} and {6} = 'อาจารย์' and {7} = '{8}' " +
            "and {0}.{4} = {9} " +
            "and not exists(select * from {10} where {11} = {0}.{4}) ",
            User_list.FieldName.TABLE_NAME, Teacher.FieldName.ROOM, User_curriculum.FieldName.TABLE_NAME,
            Teacher.FieldName.TABLE_NAME, User_list.FieldName.USER_ID, User_curriculum.FieldName.USER_ID,
            User_list.FieldName.USER_TYPE, User_curriculum.FieldName.CURRI_ID, curri_id_data,
            Teacher.FieldName.TEACHER_ID, Educational_teacher_staff.FieldName.TABLE_NAME,
            Educational_teacher_staff.FieldName.PERSONNEL_ID);

            string selpersonnel3 = string.Format("select {0}.*,{1},{2},{3},{4},{5},{6} " +
            "from {0},{7},{8} " +
            "where {9} = {10} and {11} = 'เจ้าหน้าที่' " +
            "and {12} = {9} " +
            "and exists(select * from {13} where {14} = '{15}' and {13}.{16} = {0}.{9}) ",
            User_list.FieldName.TABLE_NAME, Staff.FieldName.ROOM, Educational_teacher_staff.FieldName.DEGREE, Educational_teacher_staff.FieldName.PRE_MAJOR,
            Educational_teacher_staff.FieldName.MAJOR, Educational_teacher_staff.FieldName.GRAD_YEAR,
            Educational_teacher_staff.FieldName.COLLEGE, Educational_teacher_staff.FieldName.TABLE_NAME,
            Staff.FieldName.TABLE_NAME, User_list.FieldName.USER_ID, Educational_teacher_staff.FieldName.PERSONNEL_ID,
            User_list.FieldName.USER_TYPE, Staff.FieldName.STAFF_ID, User_curriculum.FieldName.TABLE_NAME,
            User_curriculum.FieldName.CURRI_ID, curri_id_data, User_curriculum.FieldName.USER_ID);

            string selpersonnel4 = string.Format("select {0}.*,{1} " +
            "from {0},{2},{3} " +
            "where {0}.{4} = {2}.{5} and {6} = 'เจ้าหน้าที่' and {7} = '{8}' " +
            "and {0}.{4} = {9} " +
            "and not exists(select * from {10} where {11} = {0}.{4}) ",
            User_list.FieldName.TABLE_NAME, Staff.FieldName.ROOM, User_curriculum.FieldName.TABLE_NAME,
            Staff.FieldName.TABLE_NAME, User_list.FieldName.USER_ID, User_curriculum.FieldName.USER_ID,
            User_list.FieldName.USER_TYPE, User_curriculum.FieldName.CURRI_ID, curri_id_data,
            Staff.FieldName.STAFF_ID, Educational_teacher_staff.FieldName.TABLE_NAME,
            Educational_teacher_staff.FieldName.PERSONNEL_ID);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} END",selpersonnel1,selpersonnel2,selpersonnel3,selpersonnel4);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                do
                {
                    if (res.HasRows)
                    {
                        DataTable data = new DataTable();
                        data.Load(res);
                        foreach (DataRow item in data.Rows)
                        {
                            personnel_id = Convert.ToInt32(item.ItemArray[data.Columns[USER_ID].Ordinal]);
                            if (result.FirstOrDefault(p => p.personnel_id == personnel_id) == null)
                            {
                                if (item.ItemArray[data.Columns[FieldName.USER_TYPE].Ordinal].ToString() == "อาจารย์")
                                    result.Add(new Personnel_educational
                                    {
                                        personnel_id = Convert.ToInt32(item.ItemArray[data.Columns[USER_ID].Ordinal]),
                                        addr = item.ItemArray[data.Columns[FieldName.ADDR].Ordinal].ToString(),
                                        citizen_id = item.ItemArray[data.Columns[FieldName.CITIZEN_ID].Ordinal].ToString(),
                                        curri_id = curri_id_data,
                                        email = item.ItemArray[data.Columns[FieldName.EMAIL].Ordinal].ToString(),
                                        e_name = item.ItemArray[data.Columns[FieldName.E_NAME].Ordinal].ToString(),
                                        e_prename = item.ItemArray[data.Columns[FieldName.E_PRENAME].Ordinal].ToString(),
                                        file_name_pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[data.Columns[FieldName.FILE_NAME_PIC].Ordinal].ToString()),
                                        gender = Convert.ToChar(item.ItemArray[data.Columns[FieldName.GENDER].Ordinal]),
                                        tel = item.ItemArray[data.Columns[FieldName.TEL].Ordinal].ToString(),
                                        timestamp = item.ItemArray[data.Columns[FieldName.TIMESTAMP].Ordinal].ToString(),
                                        room = item.ItemArray[data.Columns[FieldName.ROOM].Ordinal].ToString(),
                                        username = item.ItemArray[data.Columns[FieldName.USERNAME].Ordinal].ToString(),
                                        user_type = item.ItemArray[data.Columns[FieldName.USER_TYPE].Ordinal].ToString(),
                                        t_prename = item.ItemArray[data.Columns[FieldName.T_PRENAME].Ordinal].ToString(),
                                        t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) +
                                                 item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                                    });
                                else
                                    result.Add(new Personnel_educational
                                    {
                                        personnel_id = Convert.ToInt32(item.ItemArray[data.Columns[USER_ID].Ordinal]),
                                        addr = item.ItemArray[data.Columns[FieldName.ADDR].Ordinal].ToString(),
                                        citizen_id = item.ItemArray[data.Columns[FieldName.CITIZEN_ID].Ordinal].ToString(),
                                        curri_id = curri_id_data,
                                        email = item.ItemArray[data.Columns[FieldName.EMAIL].Ordinal].ToString(),
                                        e_name = item.ItemArray[data.Columns[FieldName.E_NAME].Ordinal].ToString(),
                                        e_prename = item.ItemArray[data.Columns[FieldName.E_PRENAME].Ordinal].ToString(),
                                        file_name_pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[data.Columns[FieldName.FILE_NAME_PIC].Ordinal].ToString()),
                                        gender = Convert.ToChar(item.ItemArray[data.Columns[FieldName.GENDER].Ordinal]),
                                        tel = item.ItemArray[data.Columns[FieldName.TEL].Ordinal].ToString(),
                                        timestamp = item.ItemArray[data.Columns[FieldName.TIMESTAMP].Ordinal].ToString(),
                                        room = item.ItemArray[data.Columns[FieldName.ROOM].Ordinal].ToString(),
                                        username = item.ItemArray[data.Columns[FieldName.USERNAME].Ordinal].ToString(),
                                        user_type = item.ItemArray[data.Columns[FieldName.USER_TYPE].Ordinal].ToString(),
                                        t_prename = item.ItemArray[data.Columns[FieldName.T_PRENAME].Ordinal].ToString(),
                                        t_name = item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString() +
                                                 item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                                    });
                            }

                            //If degree col is not null (mean that teacher have educational history:let's add it!)
                            if (data.Columns.Contains(Educational_teacher_staff.FieldName.DEGREE))
                            {
                                result.First(p => p.personnel_id == personnel_id).history.Add(new Educational_teacher_staff
                                {
                                    college = item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.COLLEGE].Ordinal].ToString(),
                                    degree = Convert.ToChar(item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.DEGREE].Ordinal]),
                                    grad_year = Convert.ToInt32(item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.GRAD_YEAR].Ordinal]),
                                    pre_major = item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.PRE_MAJOR].Ordinal].ToString(),
                                    major = item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.MAJOR].Ordinal].ToString(),
                                    personnel_id = Convert.ToInt32(item.ItemArray[data.Columns[USER_ID].Ordinal])
                                });
                            }
                        }
                        data.Dispose();
                    }
                    else if (!res.IsClosed)
                    {
                        if (!res.NextResult())
                            break;
                    }
                } while (!res.IsClosed);
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

        public async Task<object> SelectPersonnelWithCurriculum()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<User_curriculum_with_brief_detail> result = new List<User_curriculum_with_brief_detail>();

            d.iCommand.CommandText = GetSelectWithCurriculumCommand(curri_id);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        string usrtype = item.ItemArray[data.Columns[FieldName.USER_TYPE].Ordinal].ToString();
                        if (usrtype == "อาจารย์")
                            result.Add(new User_curriculum_with_brief_detail
                            {
                                user_id = Convert.ToInt32(item.ItemArray[data.Columns[USER_ID].Ordinal]),
                                t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) +
                                         item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString(),
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                file_name_pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[data.Columns[FieldName.FILE_NAME_PIC].Ordinal].ToString()),
                                type = usrtype
                            });
                        else
                            result.Add(new User_curriculum_with_brief_detail
                            {
                                user_id = Convert.ToInt32(item.ItemArray[data.Columns[USER_ID].Ordinal]),
                                t_name = item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString() +
                                     item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString(),
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                file_name_pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[data.Columns[FieldName.FILE_NAME_PIC].Ordinal].ToString()),
                                type = usrtype
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