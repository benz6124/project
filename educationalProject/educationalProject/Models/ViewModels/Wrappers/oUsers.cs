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
                                    "select * from {7} where {1} = '{2}' " ,
                               Teacher.FieldName.TABLE_NAME, Teacher.FieldName.USERNAME, preferredusername,
                               Staff.FieldName.TABLE_NAME,Student.FieldName.TABLE_NAME,Alumni.FieldName.TABLE_NAME,
                               Company.FieldName.TABLE_NAME,Assessor.FieldName.TABLE_NAME);

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
                        result.information.gender = item.ItemArray[data.Columns[Teacher.FieldName.GENDER].Ordinal].ToString() != null ? Convert.ToChar(item.ItemArray[data.Columns[Teacher.FieldName.GENDER].Ordinal]) : ' ';
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
    }
}