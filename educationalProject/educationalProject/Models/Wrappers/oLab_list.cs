using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oLab_list : Lab_list
    {
        private string getSelectByCurriculumAcademicCommand()
        {
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
                                      temp1tablename, Lab_officer.FieldName.OFFICER,
                                      Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME);

            string insertintotemp1 = string.Format("insert into {0} " +
                                     "select {1}, {2}, {3} from {4} " +
                                     "insert into {0} " +
                                     "select {5}, {6}, {7} from {8} ",
                                     temp1tablename, Teacher.FieldName.TEACHER_ID, Teacher.FieldName.T_PRENAME,
                                     Teacher.FieldName.T_NAME, Teacher.FieldName.TABLE_NAME,
                                     Staff.FieldName.STAFF_ID, Staff.FieldName.T_PRENAME, Staff.FieldName.T_NAME,
                                     Staff.FieldName.TABLE_NAME);

            string selectcmd = string.Format("select {0}.*,pdata.{1}, {2}, {3} from " +
                               "{0}, {4}," +
                               "(select {1}, {2}, {3} from {5}) as pdata " +
                               "where {0}.{6} = {4}.{7} and {4}.{1} = pdata.{1} " +
                               "and {8} = '{9}' and {10} = {11} order by {6}, pdata.{1}",
                               FieldName.TABLE_NAME, Lab_officer.FieldName.OFFICER,
                               Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME, Lab_officer.FieldName.TABLE_NAME,
                               temp1tablename, FieldName.LAB_NUM, Lab_officer.FieldName.LAB_NUM,
                               FieldName.CURRI_ID, curri_id, FieldName.ACA_YEAR, aca_year);

            return string.Format("BEGIN {0} {1} {2} END", createtabletemp1, insertintotemp1, selectcmd);
        }
        public object SelectByCurriculumAcademic()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Lab_list_detail> result = new List<Lab_list_detail>();

            d.iCommand.CommandText = getSelectByCurriculumAcademicCommand();
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    lab_num = -1;
                    Lab_list_detail curr = null;
                    foreach (DataRow item in data.Rows)
                    {
                        if (lab_num != Convert.ToInt32(item.ItemArray[data.Columns[FieldName.LAB_NUM].Ordinal]))
                        {
                            curr = new Lab_list_detail
                            {
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                                room = item.ItemArray[data.Columns[FieldName.ROOM].Ordinal].ToString(),
                                lab_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.LAB_NUM].Ordinal])
                            };
                            lab_num = curr.lab_num;
                            result.Add(curr);
                        }
                            if(item.ItemArray[data.Columns[Lab_officer.FieldName.OFFICER].Ordinal].ToString()[0] != 'k')
                            {
                                curr.officer.Add(new Teacher_with_t_name
                                {
                                    teacher_id = item.ItemArray[data.Columns[Lab_officer.FieldName.OFFICER].Ordinal].ToString(),
                                    t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                                });
                            }
                            else
                            {
                                curr.officer.Add(new Staff_with_t_name
                                {
                                    staff_id = item.ItemArray[data.Columns[Lab_officer.FieldName.OFFICER].Ordinal].ToString(),
                                    t_name = item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString() + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                                });
                            }
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

        public object Delete(List<Lab_list_detail> list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            string deleteprecmd = string.Format("DELETE FROM {0} WHERE {1} = '{2}' and {3} = {4}",
                FieldName.TABLE_NAME, FieldName.CURRI_ID, list.First().curri_id, FieldName.ACA_YEAR, list.First().aca_year);
            string excludecond = "1=1 ";
            foreach (Lab_list_detail item in list)
            {
                excludecond += string.Format("and {0} != {1} ", FieldName.LAB_NUM, item.lab_num);
            }

            d.iCommand.CommandText = string.Format("{0} and ({1})", deleteprecmd, excludecond);
            try
            {
                int rowAffected = d.iCommand.ExecuteNonQuery();
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


        public object UpdateLabListWithSelect(Lab_list_detail ldata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Lab_list_detail> result = new List<Lab_list_detail>();

            string updatelablistcmd = string.Format("update {0} set {1} = '{2}', {3} = '{4}' " +
                                     "where {5} = {6} ", FieldName.TABLE_NAME, FieldName.NAME, ldata.name,
                                     FieldName.ROOM, ldata.room,FieldName.LAB_NUM,ldata.lab_num);

            string deletefromlabofficer = string.Format("DELETE FROM {0} where {1} = {2} ",
                Lab_officer.FieldName.TABLE_NAME, Lab_officer.FieldName.LAB_NUM, ldata.lab_num);

            string insertintolabofficer = string.Format("INSERT INTO {0} values ",
                Lab_officer.FieldName.TABLE_NAME);

            foreach (Personnel_with_t_name p in ldata.officer)
            {
                if(p.GetType().ToString() == "Teacher_with_t_name")
                    insertintolabofficer += string.Format("({0},'{1}')", ldata.lab_num, ((Teacher_with_t_name)p).teacher_id);
                else
                    insertintolabofficer += string.Format("({0},'{1}')", ldata.lab_num, ((Staff_with_t_name)p).staff_id);
                if (p != ldata.officer.Last())
                    insertintolabofficer += ",";
            }

            curri_id = ldata.curri_id;
            aca_year = ldata.aca_year;
            string selectcmd = getSelectByCurriculumAcademicCommand();

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} END", updatelablistcmd,
                deletefromlabofficer, insertintolabofficer, selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    lab_num = -1;
                    Lab_list_detail curr = null;
                    foreach (DataRow item in data.Rows)
                    {
                        if (lab_num != Convert.ToInt32(item.ItemArray[data.Columns[FieldName.LAB_NUM].Ordinal]))
                        {
                            curr = new Lab_list_detail
                            {
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                                room = item.ItemArray[data.Columns[FieldName.ROOM].Ordinal].ToString(),
                                lab_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.LAB_NUM].Ordinal])
                            };
                            lab_num = curr.lab_num;
                            result.Add(curr);
                        }
                        if (item.ItemArray[data.Columns[Lab_officer.FieldName.OFFICER].Ordinal].ToString()[0] != 'k')
                        {
                            curr.officer.Add(new Teacher_with_t_name
                            {
                                teacher_id = item.ItemArray[data.Columns[Lab_officer.FieldName.OFFICER].Ordinal].ToString(),
                                t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                            });
                        }
                        else
                        {
                            curr.officer.Add(new Staff_with_t_name
                            {
                                staff_id = item.ItemArray[data.Columns[Lab_officer.FieldName.OFFICER].Ordinal].ToString(),
                                t_name = item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString() + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                            });
                        }
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


        public object InsertNewLabListWithSelect(Lab_list_detail ldata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Lab_list_detail> result = new List<Lab_list_detail>();
            string temp1tablename = "#temp1";
            string temp2tablename = "#temp2";
            string createtabletemp1 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] INT NOT NULL," +
                                      "PRIMARY KEY ([row_num])) ", temp1tablename, FieldName.LAB_NUM);
            string createtabletemp2 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] VARCHAR(5) NULL," +
                                      "PRIMARY KEY ([row_num])) " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} VARCHAR(5) COLLATE DATABASE_DEFAULT "
                                      , temp2tablename, Lab_officer.FieldName.OFFICER);

            string insertintotemp1 = string.Format("INSERT INTO {0} " +
                                     "select * from " +
                                     "(insert into {1} output inserted.{2} values " +
                                     "('{3}','{4}','{5}',{6})) as outputinsert ",
                                     temp1tablename, FieldName.TABLE_NAME, FieldName.LAB_NUM,
                                     ldata.name, ldata.room, ldata.curri_id, ldata.aca_year);

            string insertintotemp2 = string.Format("INSERT INTO {0} VALUES (null)", temp2tablename);

            foreach (Personnel_with_t_name p in ldata.officer)
            {
                if (p.GetType().ToString() == "Teacher_with_t_name")
                    insertintotemp2 += string.Format(",('{0}')", ((Teacher_with_t_name)p).teacher_id);
                else
                    insertintotemp2 += string.Format(",('{0}')", ((Staff_with_t_name)p).staff_id);
            }

            string insertintolabofficer = string.Format(" INSERT INTO {0} " +
                                        "select {1},{2} from {3},{4} where {2} is not null ",
                                        Lab_officer.FieldName.TABLE_NAME, FieldName.LAB_NUM, Lab_officer.FieldName.OFFICER,
                                        temp1tablename, temp2tablename);

            curri_id = ldata.curri_id;
            aca_year = ldata.aca_year;

            string selectcmd = getSelectByCurriculumAcademicCommand();

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} END", createtabletemp1, createtabletemp2,
                insertintotemp1, insertintotemp2, insertintolabofficer, selectcmd);

            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    lab_num = -1;
                    Lab_list_detail curr = null;
                    foreach (DataRow item in data.Rows)
                    {
                        if (lab_num != Convert.ToInt32(item.ItemArray[data.Columns[FieldName.LAB_NUM].Ordinal]))
                        {
                            curr = new Lab_list_detail
                            {
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                                room = item.ItemArray[data.Columns[FieldName.ROOM].Ordinal].ToString(),
                                lab_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.LAB_NUM].Ordinal])
                            };
                            lab_num = curr.lab_num;
                            result.Add(curr);
                        }
                        if (item.ItemArray[data.Columns[Lab_officer.FieldName.OFFICER].Ordinal].ToString()[0] != 'k')
                        {
                            curr.officer.Add(new Teacher_with_t_name
                            {
                                teacher_id = item.ItemArray[data.Columns[Lab_officer.FieldName.OFFICER].Ordinal].ToString(),
                                t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                            });
                        }
                        else
                        {
                            curr.officer.Add(new Staff_with_t_name
                            {
                                staff_id = item.ItemArray[data.Columns[Lab_officer.FieldName.OFFICER].Ordinal].ToString(),
                                t_name = item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString() + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                            });
                        }
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