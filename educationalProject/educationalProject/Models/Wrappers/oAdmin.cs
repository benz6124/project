using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    
    public class oAdmin : Admin
    {
        private string getselectcmd()
        {
            string temp5tablename = "#temp5";

            string createtabletemp5 = string.Format("CREATE TABLE {0}( " +
                                      "[row_num] int identity(1,1) not null," +
                                      "[{1}] VARCHAR(40) NULL," +
                                      "[{2}] {16}  NOT NULL," +
                                      "[{3}] VARCHAR(MAX) NULL," +
                                      "[{4}] {16} NULL," +
                                      "[{5}] VARCHAR(16) NULL," +
                                      "[{6}] VARCHAR(60) NULL," +
                                      "[{7}] VARCHAR(16) NULL," +
                                      "[{8}] VARCHAR(60) NULL," +
                                      "[{9}] CHAR(13) NULL," +
                                      "[{10}] CHAR NULL," +
                                      "[{11}] VARCHAR(60) NULL," +
                                      "[{12}] VARCHAR(20) NULL," +
                                      "[{13}] VARCHAR(80) NULL," +
                                      "[{14}] {17} NULL," +
                                      "[{15}] DATETIME2 NULL," +
                                      "[creator_name] VARCHAR(70) NULL," +
                                      "PRIMARY KEY([row_num])) " +

                                      "alter table {0} " +
                                      "alter column {1} VARCHAR(40) collate database_default " +

                                      "alter table {0} " +
                                      "alter column [{2}] {16} collate database_default " +

                                      "alter table {0} " +
                                      "alter column [{3}] VARCHAR(MAX) collate database_default " +

                                      "alter table {0} " +
                                      "alter column [{4}] {16} collate database_default " +

                                      "alter table {0} " +
                                      "alter column [{5}] VARCHAR(16) collate database_default " +

                                      "alter table {0} " +
                                      "alter column [{6}] VARCHAR(60) collate database_default " +

                                      "alter table {0} " +
                                      "alter column [{7}] VARCHAR(16) collate database_default " +

                                      "alter table {0} " +
                                      "alter column [{8}] VARCHAR(60) collate database_default " +

                                      "alter table {0} " +
                                      "alter column [{9}] CHAR(13) collate database_default " +

                                      "alter table {0} " +
                                      "alter column [{10}] CHAR collate database_default " +

                                      "alter table {0} " +
                                      "alter column [{11}] VARCHAR(60) collate database_default " +

                                      "alter table {0} " +
                                      "alter column [{12}] VARCHAR(20) collate database_default " +

                                      "alter table {0} " +
                                      "alter column [{13}] VARCHAR(80) collate database_default " +

                                      "alter table {0} " +
                                      "alter column [{14}] {17} collate database_default " +

                                      "alter table {0} " +
                                      "alter column [creator_name] VARCHAR(70) collate database_default ",
                                      temp5tablename, FieldName.USER_TYPE, FieldName.USERNAME, FieldName.PASSWORD,
                                      FieldName.ADMIN_CREATOR_ID, FieldName.T_PRENAME, FieldName.T_NAME,
                                      FieldName.E_PRENAME, FieldName.E_NAME, FieldName.CITIZEN_ID,
                                      FieldName.GENDER, FieldName.EMAIL, FieldName.TEL, FieldName.ADDR,
                                      FieldName.FILE_NAME_PIC, FieldName.TIMESTAMP, DBFieldDataType.USERNAME_TYPE,
                                      DBFieldDataType.FILE_NAME_TYPE);


            string insertintotemp5_1 = string.Format("insert into {0} " +
                "select a1.*, a2.{1} from {2} as a1, {2} as a2 where a1.{3} = a2.{4} ",
                temp5tablename, FieldName.T_NAME, FieldName.TABLE_NAME, FieldName.ADMIN_CREATOR_ID,
                FieldName.USERNAME);
            string insertintotemp5_2 = string.Format("insert into {0} " +
                "select *, null from {1} where {2} = 'ดั้งเดิม' ",
                temp5tablename, FieldName.TABLE_NAME, FieldName.ADMIN_CREATOR_ID);

            string selectcmd = string.Format("select * from {0} order by {1} ", temp5tablename, FieldName.TIMESTAMP);

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
                            creator_name = item.ItemArray[data.Columns["creator_name"].Ordinal].ToString(),
                            t_name = item.ItemArray[data.Columns[FieldName.T_NAME].Ordinal].ToString(),
                            file_name_pic = item.ItemArray[data.Columns[FieldName.FILE_NAME_PIC].Ordinal].ToString(),
                            email = item.ItemArray[data.Columns[FieldName.EMAIL].Ordinal].ToString(),
                            username = item.ItemArray[data.Columns[FieldName.USERNAME].Ordinal].ToString(),
                            user_type = item.ItemArray[data.Columns[FieldName.USER_TYPE].Ordinal].ToString()
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
            /*List<string> result = new List<string>();

            string temp5tablename = "#temp5";

            string createtabletemp5 = string.Format("CREATE TABLE {0}(" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] VARCHAR(60) NULL," +
                                      "PRIMARY KEY ([row_num])) " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} VARCHAR(60) COLLATE DATABASE_DEFAULT ",
                                      temp5tablename, FieldName.EMAIL);*/

            string insertcmd = "";
            //string insertintousercurri;
            //foreach (UsernamePassword item in list)
            //{
                /*insertintousercurri = string.Format("insert into {0} values ", User_curriculum.FieldName.TABLE_NAME);
                int len = insertintousercurri.Length;

                foreach (string curriitem in target_curri_id_list)
                {
                    if (insertintousercurri.Length <= len)
                        insertintousercurri += string.Format("('{0}', '{1}')", item.username, curriitem);
                    else
                        insertintousercurri += string.Format(",('{0}', '{1}')", item.username, curriitem);
                }
                //since no value to be insert in user_curriculum so we make this var as empty string
                if (insertintousercurri.Length <= len)
                    insertintousercurri = "";*/

                string ts = DateTime.Now.GetDateTimeFormats(new System.Globalization.CultureInfo("en-US"))[93];

                insertcmd += string.Format("IF NOT EXISTS(select * from {0} where {1} = '{2}') and " +
                                   "NOT EXISTS(select * from {3} where {4} = '{2}' or {12} = '{2}') and " +
                                   "NOT EXISTS(select * from {5} where {4} = '{2}' or {12} = '{2}') and " +
                                   "NOT EXISTS(select * from {6} where {4} = '{2}' or {12} = '{2}') and " +
                                   "NOT EXISTS(select * from {7} where {4} = '{2}' or {12} = '{2}') and " +
                                   "NOT EXISTS(select * from {8} where {4} = '{2}' or {12} = '{2}') and " +
                                   "NOT EXISTS(select * from {9} where {4} = '{2}' or {12} = '{2}') and " +
                                   "NOT EXISTS(select * from {10} where {4} = '{2}' or {12} = '{2}') " +
                                   "begin " +
                                   "insert into {0} values('{2}', '{11}') " +
                                   "insert into {3} ({12},{13}, {14},{18}, {4}, {15}) values ('{2}', '{11}', '{16}',{19}, '{2}', '{17}') " +
                                   //insertintousercurri + " " +
                                   "end " +
                                   "else " +
                                   "return ",
                                   //"insert into {17} values ('{2}') " +
                                   //"end ",
                                   User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_ID, /**/username,
                                   /*Main table index 3 must SWAP!*/ FieldName.TABLE_NAME,
                                   FieldName.EMAIL, Student.FieldName.TABLE_NAME,
                                   Alumni.ExtraFieldName.TABLE_NAME, Staff.FieldName.TABLE_NAME,
                                   Company.FieldName.TABLE_NAME, Teacher.FieldName.TABLE_NAME,
                                   Assessor.FieldName.TABLE_NAME,
                                   /*******11*/ "ผู้ดูแลระบบ",
                                   /*******12 ID*/FieldName.USERNAME, FieldName.USER_TYPE,
                                   FieldName.PASSWORD, FieldName.TIMESTAMP, /**/password, ts,FieldName.ADMIN_CREATOR_ID,admin_creator_id);

            //}

            //string selectcmd = string.Format("select {1} from {0} ", temp5tablename, FieldName.EMAIL);



            d.iCommand.CommandText = insertcmd;
           // d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} END ", createtabletemp5, insertcmd, selectcmd);
            try
            {
                int rowaffected = d.iCommand.ExecuteNonQuery();
                if(rowaffected > 0)
                {
                    return null;
                }
                else
                {
                    return "มีผู้ใช้งานอีเมล์นี้แล้วในระบบ";
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
        }
    }
}