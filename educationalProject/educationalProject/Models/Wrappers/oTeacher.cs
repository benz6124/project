using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
using educationalProject.Models.ViewModels.Wrappers;
namespace educationalProject.Models.Wrappers
{
    public class oTeacher : Teacher
    {
        public static string getSelectTeacherByJoinCommand()
        {
            return string.Format("select {0}.{1},{2},{3},{4},{5}," +
            "{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}," +
            "{16},{17},{18},{19},{20},{21},{22} " +
            "from {23},{0},{24} where {1} = {25} and {23}.{26} = {24}.{27} ",
            /**tablename 0 **/ FieldName.TABLE_NAME, /**iden 1**/ FieldName.TEACHER_ID, User_type.FieldName.USER_TYPE_NAME, FieldName.USERNAME,
            FieldName.PASSWORD, FieldName.T_PRENAME, FieldName.T_NAME, FieldName.E_PRENAME, FieldName.E_NAME,
            FieldName.CITIZEN_ID, FieldName.GENDER, FieldName.EMAIL, FieldName.TEL, FieldName.ADDR,
            FieldName.FILE_NAME_PIC, FieldName.TIMESTAMP,  /***common 15***/

            /**extended data**/
            FieldName.ROOM, FieldName.DEGREE, FieldName.POSITION, FieldName.PERSONNEL_TYPE, FieldName.PERSON_ID,
            FieldName.STATUS, FieldName.ALIVE,

            User_list.FieldName.TABLE_NAME, User_type.FieldName.TABLE_NAME,
            User_list.FieldName.USER_ID, User_list.FieldName.USER_TYPE_ID, User_type.FieldName.USER_TYPE_ID);
        }
        public async Task<object> SelectTeacherIdAndTName(string curri_id)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<Teacher_with_t_name> result = new List<Teacher_with_t_name>();
            d.iCommand.CommandText = string.Format("select * from ({0}) as {6} where exists(select * from {1} where {6}.{2} = {1}.{3} and {4}='{5}')", 
                getSelectTeacherByJoinCommand(), User_curriculum.FieldName.TABLE_NAME,FieldName.TEACHER_ID,User_curriculum.FieldName.USER_ID,User_curriculum.FieldName.CURRI_ID,curri_id,
                FieldName.ALIAS_NAME);
            try  
            {  
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new Teacher_with_t_name
                        {
                            teacher_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal]),
                            t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[FieldName.T_PRENAME].Ordinal].ToString()) + 
                                     item.ItemArray[data.Columns[FieldName.T_NAME].Ordinal].ToString()
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

        public async Task<object> Insert(List<UsernamePassword> list, List<string> target_curri_id_list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
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
                          temp7tablename, User_curriculum.FieldName.CURRI_ID,DBFieldDataType.CURRI_ID_TYPE);

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
                                   "select * from (insert into {0} ({5}, {1}, {6}, {3}, {7}, {15}) output inserted.{8} " +
                                   "values ('{9}', '{2}', '{10}', '{2}', '{11}', '{2}')) as outputinsert " +

                                   "insert into {12} ({13}) select {8} from {4} " +
                                   insertintousercurri + " " +
                                   
                                   "delete from {4} " +
                                   "end " +
                                   "else " +
                                   "begin " +
                                   "insert into {14} values ('{2}') " +
                                   "end ",User_list.FieldName.TABLE_NAME, User_list.FieldName.USERNAME,item.username,
                                   User_list.FieldName.EMAIL,temp6tablename,
                                   User_list.FieldName.USER_TYPE_ID,FieldName.PASSWORD,FieldName.TIMESTAMP,
                                   User_list.FieldName.USER_ID,
                                   /*****9****/ 1,item.password,ts,
                                   /****12****/ FieldName.TABLE_NAME,FieldName.TEACHER_ID,temp5tablename,
                                   User_list.FieldName.T_NAME
                                   );

            }

            string selectcmd = string.Format("select {1} from {0} ", temp5tablename, FieldName.EMAIL);



        
            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} END ", createtabletemp5,createtabletemp6,createtabletemp7,
                insertintotemp7,insertcmd,selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
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