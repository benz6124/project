using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Utils;
using educationalProject.Models.ViewModels;
namespace educationalProject.Models.Wrappers
{
    public class oPresident_curriculum : President_curriculum
    {
        public async Task<object> InsertOrUpdate()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            string deletefromcommittee = string.Format("delete from {0} where {1} = {2} and {3} = {4} and {5} = {6} ",
                Committee.FieldName.TABLE_NAME, FieldName.CURRI_ID, ParameterName.CURRI_ID, FieldName.ACA_YEAR, ParameterName.ACA_YEAR,
                FieldName.TEACHER_ID, ParameterName.TEACHER_ID);
            d.iCommand.CommandText = deletefromcommittee + string.Format("IF NOT EXISTS (select * from {0} where {1} = {2} and {3} = {4}) " +
                                       "BEGIN " +
                                       "INSERT INTO {0} VALUES " +
                                       "({5}, {2},{4}) " +
                                       "END " +
                                       "ELSE " +
                                       "BEGIN " +
                                       "UPDATE {0} SET {6} = {5} where {1} = {2} and {3} = {4} " +
                                       "END",
                FieldName.TABLE_NAME, FieldName.CURRI_ID, ParameterName.CURRI_ID, FieldName.ACA_YEAR, ParameterName.ACA_YEAR, ParameterName.TEACHER_ID, FieldName.TEACHER_ID);

            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.ACA_YEAR, aca_year));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURRI_ID, curri_id));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.TEACHER_ID, teacher_id));
            try
            {
                await d.iCommand.ExecuteNonQueryAsync();
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
        public async Task<object> SelectAllCurriculumsAndAllPresidents()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            Curriculums_presidents_detail result = new Curriculums_presidents_detail();

            string selcurriculumdata = string.Format("select {0}.{1},{2} " +
                                       "from {0},{3} " +
                                       "where {0}.{1} = {3}.{4} and {5} = {6} ",
                                       Cu_curriculum.FieldName.TABLE_NAME, Cu_curriculum.FieldName.CURRI_ID, Cu_curriculum.FieldName.CURR_TNAME,
                                       Curriculum_academic.FieldName.TABLE_NAME, Curriculum_academic.FieldName.CURRI_ID,
                                       Curriculum_academic.FieldName.ACA_YEAR, ParameterName.ACA_YEAR);

            string selallpresident = string.Format("select {0}.*,{1},{2},{3},{4} from {0},{5} " +
                                     "where {6} = {7} and {8} = {9} ",
                                     FieldName.TABLE_NAME,Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME, Teacher.FieldName.FILE_NAME_PIC,
                                     Teacher.FieldName.EMAIL,User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_ID,FieldName.TEACHER_ID,
                                     FieldName.ACA_YEAR,ParameterName.ACA_YEAR);

            string selallteacherwithcurri = string.Format("select {0}.*,{1},{2},{3},{4} from {0},{5} " +
                                            "where {6} = 'อาจารย์' and {0}.{7} = {5}.{8} " +
                                            "and {9} in (select {10} from {11} where {12} = {13}) ",
                                            User_curriculum.FieldName.TABLE_NAME, Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME, Teacher.FieldName.FILE_NAME_PIC,
                                            Teacher.FieldName.EMAIL, User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_TYPE,
                                            User_curriculum.FieldName.USER_ID, User_list.FieldName.USER_ID,
                                            User_curriculum.FieldName.CURRI_ID,Curriculum_academic.FieldName.CURRI_ID, Curriculum_academic.FieldName.TABLE_NAME,
                                            Curriculum_academic.FieldName.ACA_YEAR,ParameterName.ACA_YEAR);
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.ACA_YEAR, aca_year));
            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} END", selcurriculumdata,selallpresident,selallteacherwithcurri);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                do
                {
                    if (res.HasRows)
                    {
                        DataTable data = new DataTable();
                        data.Load(res);
                        if(data.Columns.Count == 2)
                        {
                            foreach (DataRow item in data.Rows)
                            {
                                curri_id = item.ItemArray[data.Columns[Cu_curriculum.FieldName.CURRI_ID].Ordinal].ToString();
                                result.all_curri_id.Add(curri_id);
                                result.all_presidents[curri_id] = new Curri_with_pres_and_cand
                                {
                                    curri_tname = item.ItemArray[data.Columns[Cu_curriculum.FieldName.CURR_TNAME].Ordinal].ToString()
                                };
                            }
                        }
                        else if(data.Columns.Count == 7)
                        {
                            foreach (DataRow item in data.Rows)
                            {
                                curri_id = item.ItemArray[data.Columns[Cu_curriculum.FieldName.CURRI_ID].Ordinal].ToString();
                                result.all_presidents[curri_id].presidents.Add(new Personnel_brief_detail
                                {
                                    tname = NameManager.GatherPreName(item.ItemArray[data.Columns[Personnel.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Personnel.FieldName.T_NAME].Ordinal].ToString(),
                                    pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[data.Columns[Teacher.FieldName.FILE_NAME_PIC].Ordinal].ToString()),
                                    email = item.ItemArray[data.Columns[Teacher.FieldName.EMAIL].Ordinal].ToString(),
                                    user_id = item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal].ToString()
                                });
                            }
                        }
                        else
                        {
                            foreach (DataRow item in data.Rows)
                            {
                                curri_id = item.ItemArray[data.Columns[Cu_curriculum.FieldName.CURRI_ID].Ordinal].ToString();
                                result.all_presidents[curri_id].candidates.Add(new Personnel_brief_detail
                                {
                                    tname = NameManager.GatherPreName(item.ItemArray[data.Columns[Personnel.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Personnel.FieldName.T_NAME].Ordinal].ToString(),
                                    pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[data.Columns[Teacher.FieldName.FILE_NAME_PIC].Ordinal].ToString()),
                                    email = item.ItemArray[data.Columns[Teacher.FieldName.EMAIL].Ordinal].ToString(),
                                    user_id = item.ItemArray[data.Columns[User_list.FieldName.USER_ID].Ordinal].ToString()
                                });
                            }
                        }
                        data.Dispose();
                    }
                    else if (!res.IsClosed)
                    {
                        if (!res.NextResult())
                            break;
                    }
                } while (!res.IsClosed);
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

        public async Task<object> UpdatePresidentData(Curriculums_presidents_detail data)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            string deletefrompresident = string.Format("delete from {0} where {1} = {2} ", FieldName.TABLE_NAME,
                FieldName.ACA_YEAR, ParameterName.ACA_YEAR
                );
            string insertintopresident = string.Format("insert into {0} values ", FieldName.TABLE_NAME);
            int oldlength = insertintopresident.Length;
            foreach(KeyValuePair<string,Curri_with_pres_and_cand> kv in data.all_presidents)
            {
                foreach(Personnel_brief_detail p in kv.Value.presidents)
                {
                    if (insertintopresident.Length <= oldlength)
                        insertintopresident += string.Format("({0},'{1}',{2}) ", p.user_id, kv.Key, ParameterName.ACA_YEAR);
                    else
                        insertintopresident += string.Format(",({0},'{1}',{2}) ", p.user_id, kv.Key, ParameterName.ACA_YEAR);
                }
            }
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.ACA_YEAR, aca_year));
            d.iCommand.CommandText = string.Format("BEGIN {0} {1} END", deletefrompresident, insertintopresident);
            try
            {
                await d.iCommand.ExecuteNonQueryAsync();
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