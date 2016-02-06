using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oAlumni : Alumni
    {
        public object Insert(List<UsernamePassword> list, List<string> target_curri_id_list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<string> result = new List<string>();

            string temp5tablename = "#temp5";

            string createtabletemp5 = string.Format("CREATE TABLE {0}(" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] VARCHAR(60) NULL," +
                                      "PRIMARY KEY ([row_num])) " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} VARCHAR(60) COLLATE DATABASE_DEFAULT ",
                                      temp5tablename, FieldName.EMAIL);

            string insertcmd = "";
            string insertintousercurri;
            foreach (UsernamePassword item in list)
            {
                insertintousercurri = string.Format("insert into {0} values ", User_curriculum.FieldName.TABLE_NAME);
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
                    insertintousercurri = "";

                string ts = DateTime.Now.GetDateTimeFormats(new System.Globalization.CultureInfo("en-US"))[93];

                insertcmd += string.Format("IF NOT EXISTS(select * from {0} where {1} = '{2}') and " +
                                   "NOT EXISTS(select * from {3} where {4} = '{2}') and " +
                                   "NOT EXISTS(select * from {5} where {4} = '{2}') and " +
                                   "NOT EXISTS(select * from {6} where {4} = '{2}') and " +
                                   "NOT EXISTS(select * from {7} where {4} = '{2}') and " +
                                   "NOT EXISTS(select * from {8} where {4} = '{2}') and " +
                                   "NOT EXISTS(select * from {9} where {4} = '{2}') " +
                                   "begin " +
                                   "insert into {0} values('{2}', '{10}') " +
                                   "insert into {3} ({11}, {12}, {13}, {14}, {4}, {15}) values ('{2}', '{10}', '{2}', '{16}', '{2}', '{17}') " +
                                   insertintousercurri + " " +
                                   "end " +
                                   "else " +
                                   "begin " +
                                   "insert into {18} values ('{2}') " +
                                   "end ",
                                   User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_ID, item.username,
                                   /*Main table index 3 must SWAP!*/ ExtraFieldName.TABLE_NAME,
                                   FieldName.EMAIL, Student.FieldName.TABLE_NAME,
                                   Teacher.FieldName.TABLE_NAME, Staff.FieldName.TABLE_NAME,
                                   Company.FieldName.TABLE_NAME, Assessor.FieldName.TABLE_NAME,
                                   /*******10*/ "อาจารย์",
                                   /*******11 ID*/FieldName.STUDENT_ID, FieldName.USER_TYPE, FieldName.USERNAME,
                                   FieldName.PASSWORD, FieldName.TIMESTAMP, item.password, ts, temp5tablename);

            }

            string selectcmd = string.Format("select {1} from {0} ", temp5tablename, FieldName.EMAIL);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} END ", createtabletemp5, insertcmd, selectcmd);
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
            return result;
        }
    }
}