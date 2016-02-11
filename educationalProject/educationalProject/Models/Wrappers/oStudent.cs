using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oStudent : Student
    {
        public static string getSelectStudentByJoinCommand()
        {
            return string.Format("select {0}.{1},{2},{3},{4},{5}," +
            "{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}," +
            "{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27} " +
            "from {28},{0} where {0}.{1} = {28}.{29}",
            /**tablename 0 **/ FieldName.TABLE_NAME, /**iden 1**/ FieldName.USER_ID, FieldName.USER_TYPE, FieldName.USERNAME,
            FieldName.PASSWORD, FieldName.T_PRENAME, FieldName.T_NAME, FieldName.E_PRENAME, FieldName.E_NAME,
            FieldName.CITIZEN_ID, FieldName.GENDER, FieldName.EMAIL, FieldName.TEL, FieldName.ADDR,
            FieldName.FILE_NAME_PIC, FieldName.TIMESTAMP,  /***common 15***/

            /**extended data**/
            FieldName.STUDENT_ID,FieldName.CURRI_ID,FieldName.TYPE,FieldName.ADMIS_YEAR, FieldName.ADMIS_DATE,
            FieldName.GRAD_YEAR,FieldName.GRAD_SEMESTER,FieldName.GRAD_DATE,FieldName.STATUS,FieldName.QUOTA,
            FieldName.SUBTYPE,FieldName.COOP,

            User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_ID);
        }
        public object Insert(List<UsernamePassword> list, List<string> target_curri_id_list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<string> result = new List<string>();

            string temp5tablename = "#temp5";
            string temp6tablename = "#temp6";
            string temp7tablename = "#temp7";
            string createtabletemp5 = string.Format("CREATE TABLE {0}(" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] VARCHAR(60) NULL," +
                                      "PRIMARY KEY ([row_num])) " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} VARCHAR(60) COLLATE DATABASE_DEFAULT ",
                                      temp5tablename, FieldName.EMAIL);

            string createtabletemp6 = string.Format("CREATE TABLE {0}(" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] INT NULL," +
                                      "PRIMARY KEY ([row_num])) ",
                                      temp6tablename, User_list.FieldName.USER_ID);


            string createtabletemp7 = string.Format("CREATE TABLE {0}(" +
                          "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                          "[{1}] {2} NULL," +
                          "PRIMARY KEY ([row_num])) " +
                          "ALTER TABLE {0} " +
                          "ALTER COLUMN {1} {2} COLLATE DATABASE_DEFAULT ",
                          temp7tablename, User_curriculum.FieldName.CURRI_ID, DBFieldDataType.CURRI_ID_TYPE);

            string insertintotemp7 = string.Format("insert into {0} values ", temp7tablename);

            int len = insertintotemp7.Length;

            foreach (string curriitem in target_curri_id_list)
            {
                if (insertintotemp7.Length <= len)
                    insertintotemp7 += string.Format("('{0}')", curriitem);
                else
                    insertintotemp7 += string.Format(",('{0}')", curriitem);
            }

            string insertintousercurri = "";
            if (insertintotemp7.Length > len)
                insertintousercurri = string.Format("insert into {0} " +
                                      "select {1},{2} from {3},{4} ",
                                      User_curriculum.FieldName.TABLE_NAME,
                                      User_curriculum.FieldName.USER_ID,
                                      User_curriculum.FieldName.CURRI_ID, temp6tablename, temp7tablename);
            else
                insertintotemp7 = "";

            string insertcmd = "";

            foreach (UsernamePassword item in list)
            {
                string ts = DateTime.Now.GetDateTimeFormats(new System.Globalization.CultureInfo("en-US"))[93];

                insertcmd += string.Format(
                                   "IF NOT EXISTS(select * from {0} where {1} = '{2}' or {3} = '{2}') " +
                                   "begin " +
                                   "insert into {4} " +
                                   "select * from (insert into {0} ({5}, {1}, {6}, {3}, {7}) output inserted.{8} " +
                                   "values ('{9}', '{2}', '{10}', '{2}', '{11}')) as outputinsert " +

                                   "insert into {12} ({13}) select {8} from {4} " +
                                   insertintousercurri + " " +

                                   "delete from {4} " +
                                   "end " +
                                   "else " +
                                   "begin " +
                                   "insert into {14} values ('{2}') " +
                                   "end ", User_list.FieldName.TABLE_NAME, Personnel.FieldName.USERNAME, item.username,
                                   Personnel.FieldName.EMAIL, temp6tablename,
                                   User_list.FieldName.USER_TYPE, FieldName.PASSWORD, FieldName.TIMESTAMP,
                                   User_list.FieldName.USER_ID,
                                   /*****9****/ "นักศึกษา", item.password, ts,
                                   /****12****/ FieldName.TABLE_NAME, FieldName.USER_ID, temp5tablename
                                   );

            }

            string selectcmd = string.Format("select {1} from {0} ", temp5tablename, FieldName.EMAIL);




            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} END ", createtabletemp5, createtabletemp6, createtabletemp7,
                insertintotemp7, insertcmd, selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(
                            item.ItemArray[data.Columns[FieldName.EMAIL].Ordinal].ToString()
                        );
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
            if (result.Count != 0)
                return result;
            else
                return null;
        }
    }
}