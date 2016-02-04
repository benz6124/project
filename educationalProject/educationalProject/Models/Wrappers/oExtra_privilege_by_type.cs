using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oExtra_privilege_by_type : Extra_privilege_by_type
    {
        public object SelectByCurriculumAndTitle()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect()) 
                return "Cannot connect to database.";
            Extra_privilege_by_type_list_with_privilege_choices result = new Extra_privilege_by_type_list_with_privilege_choices();

            string temp5tablename = "#temp5";
            string createtabletemp5 = string.Format("CREATE TABLE {0}(" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] VARCHAR(40) NULL," +
                                      "[{2}] VARCHAR(4) NULL," +
                                      "[{3}] VARCHAR(80) NULL," +
                                      "[{4}] VARCHAR(80) NULL," +
                                      "PRIMARY KEY([row_num])) " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} VARCHAR(40) COLLATE DATABASE_DEFAULT " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {2} VARCHAR(4) COLLATE DATABASE_DEFAULT " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {3} VARCHAR(80) COLLATE DATABASE_DEFAULT " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {4} VARCHAR(80) COLLATE DATABASE_DEFAULT ",
                                      temp5tablename, FieldName.USER_TYPE, FieldName.CURRI_ID, FieldName.TITLE, FieldName.PRIVILEGE);

            string insertintotemp5_1 = string.Format("insert into {5} " +
                                       "select * from {0} where {1} = '{2}' and {3} = '{4}' ",
                                       FieldName.TABLE_NAME, FieldName.CURRI_ID, curri_id, FieldName.TITLE, title,temp5tablename);

            string insertintotemp5_2 = string.Format("insert into {9} " +
                                       "select {0},'{1}','{2}','' from {3} where not exists " +
                                       "(select * from {4} where {3}.{5} = {4}.{6} and {7} = '{1}' and {8} = '{2}') ",
                                       FieldName.USER_TYPE, curri_id, title, User_type.FieldName.TABLE_NAME, FieldName.TABLE_NAME,
                                       User_type.FieldName.USER_TYPE, FieldName.USER_TYPE, FieldName.CURRI_ID, FieldName.TITLE,
                                       temp5tablename);

            string insertintotemp5_3 = string.Format("insert into {0} " +
                                       "select null,null,null,{1} from {2} where {3} = '{4}' ",
                                       temp5tablename, Title_privilege.FieldName.PRIVILEGE, Title_privilege.FieldName.TABLE_NAME,
                                       Title_privilege.FieldName.TITLE, title);
            string selcmd = string.Format("select * from {0} ",temp5tablename);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} END", createtabletemp5,insertintotemp5_1,
                insertintotemp5_2,insertintotemp5_3,selcmd);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        if (item.ItemArray[data.Columns[FieldName.USER_TYPE].Ordinal].ToString() != "" &&
                            item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString() != "" &&
                            item.ItemArray[data.Columns[FieldName.TITLE].Ordinal].ToString() != "")
                            result.list.Add(new Extra_privilege_by_type
                            {
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                privilege = item.ItemArray[data.Columns[FieldName.PRIVILEGE].Ordinal].ToString(),
                                title = item.ItemArray[data.Columns[FieldName.TITLE].Ordinal].ToString(),
                                user_type = item.ItemArray[data.Columns[FieldName.USER_TYPE].Ordinal].ToString()
                            });
                        else
                            result.choices.Add(item.ItemArray[data.Columns[FieldName.PRIVILEGE].Ordinal].ToString());
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