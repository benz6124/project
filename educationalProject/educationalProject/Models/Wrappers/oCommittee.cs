using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oCommittee : Committee
    {
        public object SelectWithBriefDetail()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Committee_with_detail> result = new List<Committee_with_detail>();

            d.iCommand.CommandText = string.Format("select {0}.*,{1},{2},{3},{12} from {0},{4} where {5} = '{6}' and {7} = {8} and {0}.{9} = {4}.{10} order by {11}", 
                FieldName.TABLE_NAME,Teacher.FieldName.T_PRENAME,Teacher.FieldName.T_NAME,Teacher.FieldName.FILE_NAME_PIC,
                Teacher.FieldName.TABLE_NAME,FieldName.CURRI_ID,curri_id,FieldName.ACA_YEAR,aca_year,FieldName.TEACHER_ID,Teacher.FieldName.TEACHER_ID,FieldName.DATE_PROMOTED,Teacher.FieldName.EMAIL);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new Committee_with_detail
                        {
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            file_name_pic = item.ItemArray[data.Columns[Teacher.FieldName.FILE_NAME_PIC].Ordinal].ToString(),
                            date_promoted = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.DATE_PROMOTED].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                            teacher_id = item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal].ToString(),
                            t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) +
                                     item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString(),
                            email = item.ItemArray[data.Columns[Teacher.FieldName.EMAIL].Ordinal].ToString()
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

        public object SelectNonCommitteeWithBriefDetail(List<string> user_list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Committee_with_detail> result = new List<Committee_with_detail>();

            string temp5tablename = "#temp5";
            string createtabletemp5 = string.Format("create table {0}( " +
                                      "[row_num] int identity(1, 1) not null," +
                                      "[{1}] {5} null," +
                                      "[{2}] {6} null," +
                                      "[{3}] VARCHAR(16) NULL," +
                                      "[{4}] VARCHAR(60) NULL," +
                                      "primary key([row_num])) " +

                                      "alter table {0} " +
                                      "alter column {1} {5} collate database_default " +

                                      "alter table {0} " +
                                      "alter column {2} {6} collate database_default " +

                                      "alter table {0} " +
                                      "alter column[{3}] VARCHAR(16) collate database_default " +

                                      "alter table {0} " +
                                      "alter column[{4}] VARCHAR(60) collate database_default ",
                                      temp5tablename, FieldName.TEACHER_ID, FieldName.CURRI_ID,
                                      Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                                      DBFieldDataType.USER_ID_TYPE, DBFieldDataType.CURRI_ID_TYPE);

            string insertintotemp5_1 = string.Format("insert into {0} " +
                "select {17}, {2}, {3}, {4} from {5}, {6} " +
                "where {17} = {7} and {2} = '{8}' " +
                "and not exists (select * from {9} where {10} = '{8}' and {11} = {12} and {9}.{13} = {5}.{17}) " +
                "and not exists (select * from {14} where {15} = '{8}' and {16} = {12} and {14}.{1} = {5}.{17}) ",
                temp5tablename, FieldName.TEACHER_ID, User_curriculum.FieldName.CURRI_ID, Teacher.FieldName.T_PRENAME,
                Teacher.FieldName.T_NAME,/**5**/Teacher.FieldName.TABLE_NAME,/**6**/User_curriculum.FieldName.TABLE_NAME,
                User_curriculum.FieldName.USER_ID, curri_id, President_curriculum.FieldName.TABLE_NAME,
                President_curriculum.FieldName.CURRI_ID, President_curriculum.FieldName.ACA_YEAR,
                /**12**/this.aca_year, President_curriculum.FieldName.TEACHER_ID, FieldName.TABLE_NAME,
                FieldName.CURRI_ID, FieldName.ACA_YEAR, Teacher.FieldName.TEACHER_ID);

            string excludecond = "1=1 ";
            foreach (string user_id in user_list)
                excludecond += string.Format("and {2}.{0} != '{1}' ", FieldName.TEACHER_ID, user_id,FieldName.TABLE_NAME);

            string insertintotemp5_2 = string.Format("insert into {0} " +
                                       "select {1}.{2}, {3}, {4}, {5} " +
                                       "from {1}, {6} " +
                                       "where {3} = '{7}' and {8} = {9} and ({10}) " +
                                       "and {1}.{2} = {6}.{11} ",
                                       temp5tablename, FieldName.TABLE_NAME, FieldName.TEACHER_ID, FieldName.CURRI_ID,
                                       Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                                       Teacher.FieldName.TABLE_NAME, curri_id,
                                       FieldName.ACA_YEAR, aca_year, excludecond, Teacher.FieldName.TEACHER_ID);

            string selectcmd = string.Format("select * from {0} order by {1} ", temp5tablename, FieldName.TEACHER_ID);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} END ", createtabletemp5, insertintotemp5_1,
                insertintotemp5_2, selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new Committee_with_detail
                        {
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            aca_year = this.aca_year,
                            teacher_id = item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal].ToString(),
                            t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) +
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

        public object InsertNewCommitteeWithSelect(List<string> user_list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Committee_with_detail> result = new List<Committee_with_detail>();

            string insertcmd = "";
            foreach(string user_id in user_list)
            {
                insertcmd += string.Format("if not exists(select * from {0} where {1} = '{2}' and {3} = {4} and {5} = '{6}') " +
                             "insert into {0} values('{6}', '{2}', {4}, '{7}') ",
                             FieldName.TABLE_NAME, FieldName.CURRI_ID, curri_id, FieldName.ACA_YEAR, aca_year, FieldName.TEACHER_ID, user_id,
                             date_promoted);
            }

            string selectcmd = string.Format("select {0}.*,{1},{2},{3},{12} from {0},{4} where {5} = '{6}' and {7} = {8} and {0}.{9} = {4}.{10} order by {11} ",
                FieldName.TABLE_NAME, Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME, Teacher.FieldName.FILE_NAME_PIC,
                Teacher.FieldName.TABLE_NAME, FieldName.CURRI_ID, curri_id, FieldName.ACA_YEAR, aca_year, FieldName.TEACHER_ID, Teacher.FieldName.TEACHER_ID, FieldName.DATE_PROMOTED, Teacher.FieldName.EMAIL);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} END",insertcmd,selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new Committee_with_detail
                        {
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            file_name_pic = item.ItemArray[data.Columns[Teacher.FieldName.FILE_NAME_PIC].Ordinal].ToString(),
                            date_promoted = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.DATE_PROMOTED].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                            teacher_id = item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal].ToString(),
                            t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) +
                                     item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString(),
                            email = item.ItemArray[data.Columns[Teacher.FieldName.EMAIL].Ordinal].ToString()
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

        public object Delete(List<string> user_list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            string deletecmd = string.Format("delete from {0} where {1} = '{2}' and {3} = {4} ",
                FieldName.TABLE_NAME, FieldName.CURRI_ID, curri_id, FieldName.ACA_YEAR, aca_year);
            string excludecond = "1=1 ";

            foreach (string user_id in user_list)
                excludecond += string.Format("and {0} != '{1}' ", FieldName.TEACHER_ID, user_id);
            deletecmd += string.Format("and ({0}) ", excludecond);
            d.iCommand.CommandText = deletecmd;
            try
            {
                d.iCommand.ExecuteNonQuery();
                    return null;
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