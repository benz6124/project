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
            return string.Format("select {6}.{0},{1},{2},{3},{4},{5} from {6},{7},{8} " +
                          "where {9} = '{10}' " +
                          "and {6}.{0} = {7}.{11} and {6}.{12} = {8}.{13} order by {5} ",
                          User_list.FieldName.USER_ID, FieldName.T_PRENAME,
                          FieldName.T_NAME, FieldName.CURRI_ID,
                          FieldName.FILE_NAME_PIC, User_type.FieldName.USER_TYPE_NAME, User_list.FieldName.TABLE_NAME,
                          User_curriculum.FieldName.TABLE_NAME,User_type.FieldName.TABLE_NAME,
                          User_curriculum.FieldName.CURRI_ID,
                          curri_id, User_curriculum.FieldName.USER_ID, User_list.FieldName.USER_TYPE_ID,
                          User_type.FieldName.USER_TYPE_ID);
        }
        public async Task<object> SelectPersonnelIdAndTName(string curri_id,int selectmode) 
            //selectmode 0 : only teacher and staff,1 for all
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<Personnel_with_t_name> result = new List<Personnel_with_t_name>();

            if(selectmode == 0)
            {
                d.iCommand.CommandText = string.Format("select {4}.{0},{1},{2},{3} from {4},{5},{9} " +
                                         "where {6} = '{7}' " +
                                         "and {4}.{0} = {5}.{8} " +
                                         "and ({4}.{10} = 1 or {4}.{10} = 2) " +  /*teacher = 1,staff = 2*/
                                         "and {4}.{10} = {9}.{11}",
                                         FieldName.USER_ID, FieldName.T_PRENAME,
                                         Student.FieldName.T_NAME, User_type.FieldName.USER_TYPE_NAME,
                                         User_list.FieldName.TABLE_NAME, User_curriculum.FieldName.TABLE_NAME,
                                         FieldName.CURRI_ID, curri_id, User_curriculum.FieldName.USER_ID,
                                         User_type.FieldName.TABLE_NAME, User_list.FieldName.USER_TYPE_ID,
                                         User_type.FieldName.USER_TYPE_ID
                                         );
            }
            else
            {
                if (curri_id != "999")
                {
                    d.iCommand.CommandText = string.Format("select {4}.{0},{1},{2},{3} from {4},{5},{9} " +
                                             "where {6} = '{7}' " +
                                             "and {4}.{0} = {5}.{8} and {9}.{10} = {4}.{11} ",
                                             FieldName.USER_ID, FieldName.T_PRENAME,
                                             Student.FieldName.T_NAME, User_type.FieldName.USER_TYPE_NAME,
                                             User_list.FieldName.TABLE_NAME, User_curriculum.FieldName.TABLE_NAME,
                                             FieldName.CURRI_ID, curri_id, User_curriculum.FieldName.USER_ID,
                                             User_type.FieldName.TABLE_NAME,User_type.FieldName.USER_TYPE_ID,
                                             User_list.FieldName.USER_TYPE_ID);
                }
                else
                {
                    //curri_id = 999 => select the person who didn't in any curriculum
                    d.iCommand.CommandText = string.Format("select {0},{1},{2},{3} from {4},{5} " +
                         "where not exists(select * from {6} where {6}.{7} = {4}.{0}) and {4}.{8} != 7 and {4}.{8} = {5}.{9} ",
                         FieldName.USER_ID, FieldName.T_PRENAME,
                         Student.FieldName.T_NAME, User_type.FieldName.USER_TYPE_NAME,
                         User_list.FieldName.TABLE_NAME, User_type.FieldName.TABLE_NAME,
                         User_curriculum.FieldName.TABLE_NAME,
                         User_curriculum.FieldName.USER_ID,User_list.FieldName.USER_TYPE_ID,
                         User_type.FieldName.USER_TYPE_ID
                         );
                }
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
                        if(item.ItemArray[data.Columns[User_type.FieldName.USER_TYPE_NAME].Ordinal].ToString() == "อาจารย์")
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
                return WebApiApplication.CONNECTDBERRSTRING;
            List<Personnel_educational> result = new List<Personnel_educational>();

            string selpersonnel = string.Format("select {0}, {1}.{2}," +
                "{3} = {4} " +
                ", {5}, {6}, {7}, {8}, {9} " +
                "from {1}, {10} " +
                "where {1}.{2} in(1,2) " +
                "and exists (select * from {11} " +
                            "where {1}.{0} = {12} and {13} = '{14}') " +
                "and {1}.{2} = {10}.{15} ",
                User_list.FieldName.USER_ID,User_list.FieldName.TABLE_NAME,
                User_list.FieldName.USER_TYPE_ID, User_list.FieldName.T_PRENAME,
                NameManager.GatherSQLCASEForPrename(User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_TYPE_ID, User_list.FieldName.T_PRENAME),
                User_list.FieldName.T_NAME, User_list.FieldName.FILE_NAME_PIC,
                User_list.FieldName.EMAIL, User_list.FieldName.TEL,
                User_type.FieldName.USER_TYPE_NAME, User_type.FieldName.TABLE_NAME,
                User_curriculum.FieldName.TABLE_NAME, User_curriculum.FieldName.USER_ID,
                User_curriculum.FieldName.CURRI_ID, curri_id_data,
                User_type.FieldName.USER_TYPE_ID
                );

            string seleducation = string.Format("select * from {0} " +
                "where " +
                "exists (select * from {1} " +
                        "where {2} = {3} and {4} = '{5}') " +
                "and exists (select * from {6} " +
                        "where {2} = {7} and {8} in(1, 2)) ",
                Educational_teacher_staff.FieldName.TABLE_NAME,
                User_curriculum.FieldName.TABLE_NAME,
                Educational_teacher_staff.FieldName.PERSONNEL_ID,
                User_curriculum.FieldName.USER_ID,
                User_curriculum.FieldName.CURRI_ID, curri_id_data,
                User_list.FieldName.TABLE_NAME,
                User_list.FieldName.USER_ID, User_list.FieldName.USER_TYPE_ID
                );

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} END",selpersonnel, seleducation);
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
                            if (data.Columns.Contains(Educational_teacher_staff.FieldName.EDUCATION_ID))
                            {//Educational table data
                                personnel_id = Convert.ToInt32(item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.PERSONNEL_ID].Ordinal]);
                                result.First(p => p.user_id == personnel_id).history.Add(new Educational_teacher_staff
                                {
                                    college = item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.COLLEGE].Ordinal].ToString(),
                                    degree = Convert.ToChar(item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.DEGREE].Ordinal]),
                                    grad_year = Convert.ToInt32(item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.GRAD_YEAR].Ordinal]),
                                    pre_major = item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.PRE_MAJOR].Ordinal].ToString(),
                                    major = item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.MAJOR].Ordinal].ToString(),
                                    personnel_id = personnel_id
                                });
                            }
                            else
                                result.Add(new Personnel_educational
                                { //Main user table data
                                    user_id = Convert.ToInt32(item.ItemArray[data.Columns[User_list.FieldName.USER_ID].Ordinal]),
                                    email = item.ItemArray[data.Columns[FieldName.EMAIL].Ordinal].ToString(),
                                    file_name_pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[data.Columns[FieldName.FILE_NAME_PIC].Ordinal].ToString()),
                                    tel = item.ItemArray[data.Columns[FieldName.TEL].Ordinal].ToString(),
                                    user_type = item.ItemArray[data.Columns[User_type.FieldName.USER_TYPE_NAME].Ordinal].ToString(),
                                    fullname = item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString() +
                                             item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                                });
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
                return WebApiApplication.CONNECTDBERRSTRING;
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
                        string usrtype = item.ItemArray[data.Columns[User_type.FieldName.USER_TYPE_NAME].Ordinal].ToString();
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