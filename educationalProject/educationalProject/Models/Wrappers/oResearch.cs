using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
using educationalProject.Models.ViewModels;
namespace educationalProject.Models.Wrappers
{
    public class oResearch : Research
    {
        private string getSelectByCurriculumCommand()
        {
            return string.Format("select r.*,{0}.{1},{0}.{2} from " +
                "(select {3}.{4},{3}.{5},{3}.{6}," +
                "{3}.{7}, {3}.{8},{9} from {3}, {10} where " +
                "{5} = '{11}' and {3}.{4} = {10}.{12}) as r,{0} where r.{9} = {0}.{13}",
                Teacher.FieldName.TABLE_NAME, Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                FieldName.TABLE_NAME, FieldName.RESEARCH_ID, FieldName.CURRI_ID, FieldName.FILE_NAME,
                FieldName.NAME, FieldName.YEAR_PUBLISH, Research_owner.FieldName.TEACHER_ID,
                Research_owner.FieldName.TABLE_NAME, curri_id, Research_owner.FieldName.RESEARCH_ID,
                Teacher.FieldName.TEACHER_ID);
        }
        public object Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oResearch> result = new List<oResearch>();
            d.iCommand.CommandText = string.Format("select * from {0}", FieldName.TABLE_NAME);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oResearch
                        {
                            research_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.RESEARCH_ID].Ordinal]),
                            name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                            year_publish = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.YEAR_PUBLISH].Ordinal])
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

        public object SelectWhere(string wherecond)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oResearch> result = new List<oResearch>();
            d.iCommand.CommandText = string.Format("select * from {0} where {1}", FieldName.TABLE_NAME, wherecond);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oResearch
                        {
                            research_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.RESEARCH_ID].Ordinal]),
                            name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                            year_publish = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.YEAR_PUBLISH].Ordinal])
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

        public object SelectWithDetailByCurriculum(string curri_id_data)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Research_detail> result = new List<Research_detail>();
            curri_id = curri_id_data;
            d.iCommand.CommandText = getSelectByCurriculumCommand();
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    research_id = -1;
                    Research_detail curr = null;
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        if(research_id != Convert.ToInt32(item.ItemArray[data.Columns[FieldName.RESEARCH_ID].Ordinal]))
                        {
                            curr = new Research_detail
                            {
                                name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                file_name = item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString(),
                                research_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.RESEARCH_ID].Ordinal]),
                                year_publish = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.YEAR_PUBLISH].Ordinal])
                            };
                            research_id = curr.research_id;
                            result.Add(curr);
                        }
                        curr.researcher.Add(new Teacher_with_t_name
                        {
                            teacher_id = Convert.ToInt32(item.ItemArray[data.Columns[Teacher.FieldName.TEACHER_ID].Ordinal]),
                            t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
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

        public object Delete(List<Research_detail> list)
        {
            DBConnector d = new DBConnector();
            List<string> file_to_delete = new List<string>();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            string deleteprecmd = string.Format("DELETE FROM {0} OUTPUT DELETED.{3} WHERE {1} = '{2}'",
                FieldName.TABLE_NAME, FieldName.CURRI_ID, list.First().curri_id, FieldName.FILE_NAME);
            string excludecond = "1=1 ";
            foreach (Research_detail item in list)
            {
                excludecond += string.Format("and {0} != {1} ", FieldName.RESEARCH_ID, item.research_id);
            }

            d.iCommand.CommandText = string.Format("{0} and ({1})", deleteprecmd, excludecond);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        file_to_delete.Add(
                           item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString()
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
            return file_to_delete;
        }


        public object InsertNewResearchWithSelect(Research_detail rdata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Research_detail> result = new List<Research_detail>();
            string temp1tablename = "#temp1";
            string temp2tablename = "#temp2";
            string createtabletemp1 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] INT NOT NULL," +
                                      "PRIMARY KEY ([row_num])) ", temp1tablename, FieldName.RESEARCH_ID);
            string createtabletemp2 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] INT NULL," +
                                      "PRIMARY KEY ([row_num])) "
                                      , temp2tablename, Research_owner.FieldName.TEACHER_ID);

            string insertintotemp1 = string.Format("INSERT INTO {0} " +
                                     "select * from " +
                                     "(insert into {1} output inserted.{2} values " +
                                     "('{3}', '{4}', {5}, '{6}')) as outputinsert ",
                                     temp1tablename,FieldName.TABLE_NAME,FieldName.RESEARCH_ID,
                                     rdata.curri_id,rdata.name,rdata.year_publish,rdata.file_name);

            string insertintotemp2 = string.Format("INSERT INTO {0} VALUES (null)", temp2tablename);
                
                foreach(Teacher_with_t_name item in rdata.researcher)
                insertintotemp2 += string.Format(",('{0}')", item.teacher_id);

            string insertintoresowner = string.Format(" INSERT INTO {0} " +
                                        "select {1},{2} from {3},{4} where {2} is not null ",
                                        Research_owner.FieldName.TABLE_NAME, FieldName.RESEARCH_ID, Research_owner.FieldName.TEACHER_ID,
                                        temp1tablename, temp2tablename);

            curri_id = rdata.curri_id;
            string selectcmd = getSelectByCurriculumCommand();

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} END", createtabletemp1, createtabletemp2,
                insertintotemp1, insertintotemp2, insertintoresowner, selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    research_id = -1;
                    Research_detail curr = null;
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        if (research_id != Convert.ToInt32(item.ItemArray[data.Columns[FieldName.RESEARCH_ID].Ordinal]))
                        {
                            curr = new Research_detail
                            {
                                name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                file_name = item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString(),
                                research_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.RESEARCH_ID].Ordinal]),
                                year_publish = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.YEAR_PUBLISH].Ordinal])
                            };
                            research_id = curr.research_id;
                            result.Add(curr);
                        }
                        curr.researcher.Add(new Teacher_with_t_name
                        {
                            teacher_id = Convert.ToInt32(item.ItemArray[data.Columns[Teacher.FieldName.TEACHER_ID].Ordinal]),
                            t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
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


        public object UpdateResearchWithSelect(Research_detail rdata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Research_detail> result = new List<Research_detail>();
            string temp1tablename = "#temp1";

            string createtabletemp1 = string.Format("create table {0} (" +
                                     "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                     "[{1}] {2} NOT NULL," +
                                     "PRIMARY KEY([row_num])) " +
                                     "ALTER TABLE {0} " +
                                     "ALTER COLUMN {1} {2} COLLATE DATABASE_DEFAULT ",
                                     temp1tablename, FieldName.FILE_NAME,DBFieldDataType.FILE_NAME_TYPE);

            string insertintotemp1 = string.Format("INSERT INTO {0} " +
                                     "select * from " +
                                     "(update {1} set {2} = '{3}', {4} = '{5}', {6} = {7} " +
                                     "OUTPUT deleted.{4} where {8} = {9}) as outputupdate ",
                                     temp1tablename, FieldName.TABLE_NAME, FieldName.NAME, rdata.name,
                                     FieldName.FILE_NAME, rdata.file_name, FieldName.YEAR_PUBLISH, rdata.year_publish,
                                     FieldName.RESEARCH_ID, rdata.research_id);

            string deletefromresearchowner = string.Format("DELETE FROM {0} where {1} = {2} ",
                Research_owner.FieldName.TABLE_NAME, Research_owner.FieldName.RESEARCH_ID, rdata.research_id);

            string insertintoresearchowner = string.Format("INSERT INTO {0} values ",
                Research_owner.FieldName.TABLE_NAME);

            foreach (Teacher_with_t_name t in rdata.researcher)
            {
                insertintoresearchowner += string.Format("({0},'{1}')", rdata.research_id, t.teacher_id);
                if (t != rdata.researcher.Last())
                    insertintoresearchowner += ",";
            }

            
            string selectcmd = string.Format("select temp1out.{6},r.*,{0}.{1},{0}.{2} from " +
                "(select {3}.{4},{3}.{5},{3}.{6}," +
                "{3}.{7}, {3}.{8},{9} from {3}, {10} where " +
                "{5} = '{11}' and {3}.{4} = {10}.{12}) as r,(select {6} from {14}) as temp1out,{0} where r.{9} = {0}.{13}",
                Teacher.FieldName.TABLE_NAME, Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                FieldName.TABLE_NAME, FieldName.RESEARCH_ID, FieldName.CURRI_ID, FieldName.FILE_NAME,
                FieldName.NAME, FieldName.YEAR_PUBLISH, Research_owner.FieldName.TEACHER_ID,
                Research_owner.FieldName.TABLE_NAME, rdata.curri_id, Research_owner.FieldName.RESEARCH_ID,
                Teacher.FieldName.TEACHER_ID,temp1tablename);
            
            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} END", createtabletemp1,
                insertintotemp1, deletefromresearchowner, insertintoresearchowner, selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    research_id = -1;
                    Research_detail curr = null;
                    DataTable data = new DataTable();
                    data.Load(res);

                    //get to-be delete file_name set in file_name property of main object for future use
                    file_name = data.Rows[0].ItemArray[0].ToString();
                    foreach (DataRow item in data.Rows)
                    {
                        if (research_id != Convert.ToInt32(item.ItemArray[data.Columns[FieldName.RESEARCH_ID].Ordinal]))
                        {
                            curr = new Research_detail
                            {
                                name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                file_name = item.ItemArray[3].ToString(),
                                research_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.RESEARCH_ID].Ordinal]),
                                year_publish = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.YEAR_PUBLISH].Ordinal])
                            };
                            research_id = curr.research_id;
                            result.Add(curr);
                        }
                        curr.researcher.Add(new Teacher_with_t_name
                        {
                            teacher_id = Convert.ToInt32(item.ItemArray[data.Columns[Teacher.FieldName.TEACHER_ID].Ordinal]),
                            t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
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
    }
}