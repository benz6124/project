using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
namespace educationalProject.Models.ViewModels.Wrappers
{
    public class oPersonnel
    {
        private static readonly string PERSONNEL_ID = "PERSONNEL_ID";
        public object SelectPersonnelIdAndTName(string curri_id)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Personnel_with_t_name> result = new List<Personnel_with_t_name>();

            string temp1tablename = "#temp1";

            string createtabletemp1 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] VARCHAR(5) NOT NULL," +
                                      "[{2}] VARCHAR(16) NULL," +
                                      "[{3}] VARCHAR(60) NULL," +
                                      "PRIMARY KEY([row_num])) " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} VARCHAR(5) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {2} VARCHAR(16) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {3} VARCHAR(60) COLLATE DATABASE_DEFAULT ",
                                      temp1tablename,PERSONNEL_ID,
                                      Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME);

            string insertintotemp1_1 = string.Format("INSERT INTO {0} " +
                                       "select {1},{2},{3} from {4} where " +
                                       "exists(select * from {5} where {4}.{1} = {5}.{6} and {7}='{8}')",
                                       temp1tablename, Teacher.FieldName.TEACHER_ID,
                                      Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME, Teacher.FieldName.TABLE_NAME,
                                      Curriculum_teacher_staff.FieldName.TABLE_NAME,
                                      Curriculum_teacher_staff.FieldName.PERSONNEL_ID,
                                      Curriculum_teacher_staff.FieldName.CURRI_ID, curri_id);

            string insertintotemp1_2 = string.Format("INSERT INTO {0} " +
                                       "select {1},{2},{3} from {4} where " +
                                       "exists(select * from {5} where {4}.{1} = {5}.{6} and {7}='{8}')",
                                       temp1tablename, Staff.FieldName.STAFF_ID,
                                      Staff.FieldName.T_PRENAME, Staff.FieldName.T_NAME, Staff.FieldName.TABLE_NAME,
                                      Curriculum_teacher_staff.FieldName.TABLE_NAME, 
                                      Curriculum_teacher_staff.FieldName.PERSONNEL_ID,
                                      Curriculum_teacher_staff.FieldName.CURRI_ID, curri_id);

            string selectcmd = string.Format("select {0},{1},{2} from {3} ", PERSONNEL_ID,
                                      Staff.FieldName.T_PRENAME, Staff.FieldName.T_NAME, temp1tablename);


            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} END",createtabletemp1,
                insertintotemp1_1,insertintotemp1_2,selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new Personnel_with_t_name
                        {
                            personnel_id = item.ItemArray[data.Columns[PERSONNEL_ID].Ordinal].ToString(),
                            t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) +
                                     item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                        });
                    }
                    res.Close();
                    data.Dispose();
                }
                else
                {
                    //Reserved for return error string
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
            return result;
        }
    }
}