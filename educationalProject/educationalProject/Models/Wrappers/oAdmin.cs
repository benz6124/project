using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    
    public class oAdmin : Admin
    {
        public static string getSelectAdminByJoinCommand()
        {
            return string.Format("select {0}.{1},{2},{3},{4},{5}," +
            "{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}," +
            "{16} " +
            "from {17},{0} where {1} = {18}",
            /**tablename 0 **/ FieldName.TABLE_NAME, /**iden 1**/ FieldName.ADMIN_ID, FieldName.USER_TYPE, FieldName.USERNAME,
            FieldName.PASSWORD, FieldName.T_PRENAME, FieldName.T_NAME, FieldName.E_PRENAME, FieldName.E_NAME,
            FieldName.CITIZEN_ID, FieldName.GENDER, FieldName.EMAIL, FieldName.TEL, FieldName.ADDR,
            FieldName.FILE_NAME_PIC, FieldName.TIMESTAMP,  /***common 15***/

            /**extended data**/
            FieldName.ADMIN_CREATOR_ID,

            User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_ID);
        }
        private string getselectcmd()
        {
            string temp5tablename = "#temp5";
            string createtabletemp5 = string.Format("CREATE TABLE {0}( " +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] INT NULL," +
                                      "[{2}] VARCHAR(40) NULL," +
                                      "[{3}] {10} NULL," +
                                      "[{4}] VARCHAR(16) NULL," +
                                      "[{5}] VARCHAR(60) NULL," +
                                      "[{6}] VARCHAR(60) NULL," +
                                      "[{7}] {11} NULL," +
                                      "[{8}] DATETIME2 NULL," +
                                      "[{9}] INT NULL," +
                                      "[c_t_prename] VARCHAR(16) NULL," +
                                      "[c_t_name] VARCHAR(60) NULL," +
                                      "PRIMARY KEY([row_num])) " +

                                      "alter table {0} " +
                                      "alter column [{2}] VARCHAR(40) collate database_default " +

                                      "alter table {0} " +
                                      "alter column [{3}] {10} collate database_default " +

                                      "alter table {0} " +
                                      "alter column [{4}] VARCHAR(16) collate database_default " +

                                      "alter table {0} " +
                                      "alter column [{5}] VARCHAR(60) collate database_default " +

                                      "alter table {0} " +
                                      "alter column [{6}] VARCHAR(60) collate database_default " +

                                      "alter table {0} " +
                                      "alter column [{7}] {11} collate database_default " +

                                      "alter table {0} " +
                                      "alter column [c_t_prename] VARCHAR(16) collate database_default " +

                                      "alter table {0} " +
                                      "alter column [c_t_name] VARCHAR(60) collate database_default ",
                                      temp5tablename, FieldName.ADMIN_ID, FieldName.USER_TYPE, FieldName.USERNAME,
                                      FieldName.T_PRENAME, FieldName.T_NAME, FieldName.EMAIL, FieldName.FILE_NAME_PIC,
                                      FieldName.TIMESTAMP, FieldName.ADMIN_CREATOR_ID, DBFieldDataType.USERNAME_TYPE,
                                      DBFieldDataType.FILE_NAME_TYPE);

            string insertintotemp5_1 = string.Format("insert into {0} " +
                                       "select {1}, a1.{2}, a1.{3}, a1.{4}, a1.{5}, a1.{6}, a1.{7}, a1.{8}," +
                                       "a1.{9}, a2.{4}, a2.{5} " +
                                       "from ({10}) as a1, {11} as a2 where a1.{9} = a2.{12} ",
                                       temp5tablename, FieldName.ADMIN_ID, FieldName.USER_TYPE, FieldName.USERNAME,
                                       FieldName.T_PRENAME, FieldName.T_NAME, FieldName.EMAIL, FieldName.FILE_NAME_PIC,
                                       FieldName.TIMESTAMP, FieldName.ADMIN_CREATOR_ID, getSelectAdminByJoinCommand(),
                                       User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_ID);

            string insertintotemp5_2 = string.Format("insert into {0} " +
                                       "select {1}, {2}, {3}, {4},{5}, {6}, {7}, {8}, null, null, null " +
                                       "from ({9}) as adm where {10} is null ",
                                        temp5tablename, FieldName.ADMIN_ID, FieldName.USER_TYPE, FieldName.USERNAME,
                                       FieldName.T_PRENAME, FieldName.T_NAME, FieldName.EMAIL, FieldName.FILE_NAME_PIC,
                                       FieldName.TIMESTAMP, getSelectAdminByJoinCommand(), FieldName.ADMIN_CREATOR_ID);

            string selectcmd = string.Format("select * from {0} order by {1} DESC ", temp5tablename, FieldName.TIMESTAMP);

            return string.Format("BEGIN {0} {1} {2} {3} END ", createtabletemp5, insertintotemp5_1,
                insertintotemp5_2, selectcmd);
        }


        public object Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Admin_with_creator> result = new List<Admin_with_creator>();

            d.iCommand.CommandText = getselectcmd();
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new Admin_with_creator
                        {
                            timestamp = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.TIMESTAMP].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                            admin_creator_id = item.ItemArray[data.Columns[FieldName.ADMIN_CREATOR_ID].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ADMIN_CREATOR_ID].Ordinal]) : 0,
                            creator_name = item.ItemArray[data.Columns["c_t_prename"].Ordinal].ToString() + item.ItemArray[data.Columns["c_t_name"].Ordinal].ToString(),
                            t_name = item.ItemArray[data.Columns[FieldName.T_PRENAME].Ordinal].ToString() + item.ItemArray[data.Columns[FieldName.T_NAME].Ordinal].ToString(),
                            file_name_pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[data.Columns[FieldName.FILE_NAME_PIC].Ordinal].ToString()),
                            email = item.ItemArray[data.Columns[FieldName.EMAIL].Ordinal].ToString(),
                            username = item.ItemArray[data.Columns[FieldName.USERNAME].Ordinal].ToString(),
                            user_type = item.ItemArray[data.Columns[FieldName.USER_TYPE].Ordinal].ToString(),
                            admin_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ADMIN_ID].Ordinal])
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

        public object InsertWithSelect()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Admin_with_creator> result = new List<Admin_with_creator>();

            string temp6tablename = "#temp6";

            string createtabletemp6 = string.Format("CREATE TABLE {0}(" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] INT NULL," +
                                      "PRIMARY KEY ([row_num])) ",
                                      temp6tablename, User_list.FieldName.USER_ID);

            string insertcmd = "";

                string ts = DateTime.Now.GetDateTimeFormats(new System.Globalization.CultureInfo("en-US"))[93];
            
            insertcmd += string.Format(
                               "IF NOT EXISTS(select * from {0} where {1} = '{2}' or {3} = '{2}') " +
                               "begin " +
                               "insert into {4} " +
                               "select * from (insert into {0} ({5}, {1}, {6}, {3}, {7},{16}) output inserted.{8} " +
                               "values ('{9}', '{2}', '{10}', '{2}', '{11}','{17}')) as outputinsert " +

                               "insert into {12} ({13},{14}) select {8},{15} from {4} " +

                               getselectcmd() + " " +
                               "end " +
                               "else " +
                               "RETURN ", User_list.FieldName.TABLE_NAME, Personnel.FieldName.USERNAME, username,
                               Personnel.FieldName.EMAIL, temp6tablename,
                               User_list.FieldName.USER_TYPE, FieldName.PASSWORD, FieldName.TIMESTAMP,
                               User_list.FieldName.USER_ID,
                               /*****9****/ "ผู้ดูแลระบบ", password, ts,
                               /****12****/ FieldName.TABLE_NAME, FieldName.ADMIN_ID, FieldName.ADMIN_CREATOR_ID, admin_creator_id,
                               FieldName.T_NAME,t_name);

            
            d.iCommand.CommandText = string.Format("BEGIN {0} {1} END ", createtabletemp6, 
               insertcmd);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new Admin_with_creator
                        {
                            timestamp = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.TIMESTAMP].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                            admin_creator_id = item.ItemArray[data.Columns[FieldName.ADMIN_CREATOR_ID].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ADMIN_CREATOR_ID].Ordinal]) : 0,
                            creator_name = item.ItemArray[data.Columns["c_t_prename"].Ordinal].ToString() + item.ItemArray[data.Columns["c_t_name"].Ordinal].ToString(),
                            t_name = item.ItemArray[data.Columns[FieldName.T_PRENAME].Ordinal].ToString() + item.ItemArray[data.Columns[FieldName.T_NAME].Ordinal].ToString(),
                            file_name_pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[data.Columns[FieldName.FILE_NAME_PIC].Ordinal].ToString()),
                            email = item.ItemArray[data.Columns[FieldName.EMAIL].Ordinal].ToString(),
                            username = item.ItemArray[data.Columns[FieldName.USERNAME].Ordinal].ToString(),
                            user_type = item.ItemArray[data.Columns[FieldName.USER_TYPE].Ordinal].ToString(),
                            admin_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ADMIN_ID].Ordinal])
                        });
                    }
                    data.Dispose();
                }
                else
                {
                    return "มีชื่อผู้ใช้งานนี้แล้วในระบบ";
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