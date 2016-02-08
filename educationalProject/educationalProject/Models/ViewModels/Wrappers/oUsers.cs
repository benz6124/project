using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.ViewModels.Wrappers
{
    public class oUsers : User
    {
        public object SelectUser(string preferredusername)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            User_information_with_privilege_information result = new User_information_with_privilege_information();
            string selectcmd = string.Format(
                               "if exists(select * from {0} where {1} = '{2}') " +
                                    "select * from {0} where {1} = '{2}' " +
                                "else if exists(select * from {3} where {1} = '{2}') " +
                                    "select * from {3} where {1} = '{2}' " +
                                "else if exists(select * from {4} where {1} = '{2}') " +
                                    "select * from {4} where {1} = '{2}' " +
                                "else if exists(select * from {5} where {1} = '{2}') " +
                                    "select * from {5} where {1} = '{2}' " +
                                "else if exists(select * from {6} where {1} = '{2}') " +
                                    "select * from {6} where {1} = '{2}' " +
                                "else if exists(select * from {7} where {1} = '{2}') " +
                                    "select * from {7} where {1} = '{2}' " +
                                "else if exists(select * from {8} where {1} = '{2}') " +
                                    "select * from {8} where {1} = '{2}' " ,
                               Teacher.FieldName.TABLE_NAME, Teacher.FieldName.USERNAME, preferredusername,
                               Staff.FieldName.TABLE_NAME,Student.FieldName.TABLE_NAME,Alumni.FieldName.TABLE_NAME,
                               Company.FieldName.TABLE_NAME,Assessor.FieldName.TABLE_NAME,Admin.FieldName.TABLE_NAME);

            d.iCommand.CommandText = selectcmd;
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        //MAIN INFOMRATION
                        string usrtype = item.ItemArray[data.Columns[User_list.FieldName.USER_TYPE].Ordinal].ToString();
                        if (usrtype == "อาจารย์")
                            result.user_id = item.ItemArray[data.Columns[Teacher.FieldName.TEACHER_ID].Ordinal].ToString();
                        else if (usrtype == "เจ้าหน้าที่")
                            result.user_id = item.ItemArray[data.Columns[Staff.FieldName.STAFF_ID].Ordinal].ToString();
                        else if (usrtype == "นักศึกษา" || usrtype == "ศิษย์เก่า")
                            result.user_id = item.ItemArray[data.Columns[Student.FieldName.STUDENT_ID].Ordinal].ToString();
                        else
                            result.user_id = item.ItemArray[data.Columns[Teacher.FieldName.USERNAME].Ordinal].ToString();

                        result.username = item.ItemArray[data.Columns[Teacher.FieldName.USERNAME].Ordinal].ToString();
                        result.user_type = usrtype;
                        //**********************************************
                        result.information.addr = item.ItemArray[data.Columns[Teacher.FieldName.ADDR].Ordinal].ToString();
                        result.information.citizen_id = item.ItemArray[data.Columns[Teacher.FieldName.CITIZEN_ID].Ordinal].ToString();
                        result.information.email = item.ItemArray[data.Columns[Teacher.FieldName.EMAIL].Ordinal].ToString();
                        result.information.tel = item.ItemArray[data.Columns[Teacher.FieldName.TEL].Ordinal].ToString();
                        result.information.gender = item.ItemArray[data.Columns[Teacher.FieldName.GENDER].Ordinal].ToString() != "" ? Convert.ToChar(item.ItemArray[data.Columns[Teacher.FieldName.GENDER].Ordinal]) : ' ';
                        result.information.file_name_pic = item.ItemArray[data.Columns[Teacher.FieldName.FILE_NAME_PIC].Ordinal].ToString();
                        result.information.timestamp = item.ItemArray[data.Columns[Teacher.FieldName.TIMESTAMP].Ordinal].ToString();
                        result.information.e_name = item.ItemArray[data.Columns[Teacher.FieldName.E_NAME].Ordinal].ToString();
                        result.information.e_prename = item.ItemArray[data.Columns[Teacher.FieldName.E_PRENAME].Ordinal].ToString();
                        result.information.t_name = item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString();
                        result.information.t_prename = item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString();
                        result.information.SetPassword(item.ItemArray[data.Columns[Teacher.FieldName.PASSWORD].Ordinal].ToString());
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

        public object SelectUserPrivilege(ref User_information_with_privilege_information userdata)
        {
            //DBConnector d = new DBConnector();
            //if (!d.SQLConnect())
            //    return "Cannot connect to database.";

            //string temp5tablename = "#temp5";

            //string createtabletemp5 = string.Format("create table {0}(" +
            //                          "[row_num] int identity(1, 1) not null," +
            //                          "[{1}] {5} null," +
            //                          "[{2}] {6} null," +
            //                          "[{3}] varchar(80) null," +
            //                          "[{4}] varchar(80) null," +
            //                          "primary key([row_num])) " +

            //                          "alter table {0} " +
            //                          "alter column {1} {5} collate database_default " +

            //                          "alter table {0} " +
            //                          "alter column {2} {6} collate database_default " +

            //                          "alter table {0} " +
            //                          "alter column {3} varchar(80) collate database_default " +

            //                          "alter table {0} " +
            //                          "alter column {4} varchar(80) collate database_default ",
            //                          temp5tablename,User_list.FieldName.USER_ID,User_curriculum.FieldName.CURRI_ID,
            //                          Extra_privilege.FieldName.TITLE,Extra_privilege.FieldName.PRIVILEGE,
            //                          DBFieldDataType.USER_ID_TYPE,DBFieldDataType.CURRI_ID_TYPE
            //                          );

            //string insertintotemp5_1 = string.Format("insert into {0} " +
            //                           "select * from {1} where {2} = '{3}' ",
            //                           temp5tablename,Extra_privilege.FieldName.TABLE_NAME,
            //                           Extra_privilege.FieldName.PERSONNEL_ID,userdata.user_id
            //                           );

            //string insertintotemp5_2 = string.Format("insert into {0} " +
            //                           "select '{1}',{2},{3},{4} " +
            //                           "from {5} " +
            //                           "where {2} in (select {6} from {7} where {8} = '{1}') " +
            //                           "and not exists " +
            //                           "(select * from {9} " +
            //                           "where {10} = '{1}' and {9}.{11} = {5}.{3} and {9}.{12} = {5}.{2}) " +
            //                           "and {13} = '{14}' ",
            //                           temp5tablename, userdata.user_id, Extra_privilege_by_type.FieldName.CURRI_ID,
            //                           Extra_privilege_by_type.FieldName.TITLE, Extra_privilege_by_type.FieldName.PRIVILEGE,
            //                           /***Main 5***/Extra_privilege_by_type.FieldName.TABLE_NAME,
            //                           User_curriculum.FieldName.CURRI_ID, User_curriculum.FieldName.TABLE_NAME,
            //                           User_curriculum.FieldName.USER_ID,
            //                           /***extra 9***/Extra_privilege.FieldName.TABLE_NAME,
            //                           Extra_privilege.FieldName.PERSONNEL_ID,
            //                           Extra_privilege.FieldName.TITLE,
            //                           Extra_privilege.FieldName.CURRI_ID,
            //                           Extra_privilege_by_type.FieldName.USER_TYPE, userdata.user_type);

            //string insertintotemp5_3 = string.Format("insert into {0} " +
            //                           "select usercurri.*, {1}, {2} " +
            //                           "from (select * from {3} where {4} = '{5}') as usercurri,{6} " +
            //                           "where {7} = '{8}' and not exists (select * from {0} where " +
            //                           "{0}.{9} = {6}.{1} and {0}.{10} = usercurri.{11}) ",
            //                           temp5tablename, Default_privilege_by_type.FieldName.TITLE, Default_privilege_by_type.FieldName.PRIVILEGE,
            //                           User_curriculum.FieldName.TABLE_NAME, User_curriculum.FieldName.USER_ID,
            //                           userdata.user_id,
            //                           /*default 6*/Default_privilege_by_type.FieldName.TABLE_NAME,
            //                           Default_privilege_by_type.FieldName.USER_TYPE, userdata.user_type,
            //                           Extra_privilege.FieldName.TITLE, Extra_privilege.FieldName.CURRI_ID,
            //                           User_curriculum.FieldName.CURRI_ID);

            //string selectcmd = string.Format("select * from {0} order by {1},{2} ", temp5tablename,
            //                   Extra_privilege.FieldName.TITLE, Extra_privilege.FieldName.CURRI_ID);
            
            // d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} END", createtabletemp5,insertintotemp5_1,
            //     insertintotemp5_2,insertintotemp5_3,selectcmd);

            //try
            //{
            //    System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
            //    if (res.HasRows)
            //    {
            //        DataTable data = new DataTable();
            //        data.Load(res);
            //        foreach (DataRow item in data.Rows)
            //        {
            //            string curri_id = item.ItemArray[data.Columns[User_curriculum.FieldName.CURRI_ID].Ordinal].ToString();
            //            if (userdata.privilege.FirstOrDefault(t => t.curri_id == curri_id) == null)
            //            {
            //                userdata.privilege.Add(new Privilege_by_curriculum
            //                {
            //                    curri_id = item.ItemArray[data.Columns[User_curriculum.FieldName.CURRI_ID].Ordinal].ToString()
            //                });
            //            }
            //            userdata.privilege.First(t => t.curri_id == curri_id).privilege_list.Add(new Title_privilege {
            //                privilege = item.ItemArray[data.Columns[Extra_privilege.FieldName.PRIVILEGE].Ordinal].ToString(),
            //                title = item.ItemArray[data.Columns[Extra_privilege.FieldName.TITLE].Ordinal].ToString()
            //            });
            //        }
            //        data.Dispose();
            //    }
            //    else
            //    {
            //        //Reserved for return error string
            //    }
            //    res.Close();
            //}
            //catch (Exception ex)
            //{
            //    //Handle error from sql execution
            //    return ex.Message;
            //}
            //finally
            //{
            //    //Whether it success or not it must close connection in order to end block
            //    d.SQLDisconnect();
            //}
            return null;
        }
    }
}