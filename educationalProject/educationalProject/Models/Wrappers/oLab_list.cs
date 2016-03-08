using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oLab_list : Lab_list
    {
        private string getSelectByCurriculumAcademicCommand()
        {
            return string.Format("select {0}.*,{1},{2},{3},{4} from {0},{5},{6} " +
                                 "where {7} = '{8}' and {9} = {10} and " +
                                 "{0}.{11} = {5}.{12} and {13} = {1} order by {14} ",
                                 FieldName.TABLE_NAME,Lab_officer.FieldName.OFFICER,Personnel.FieldName.USER_TYPE,
                                 Personnel.FieldName.T_PRENAME, Personnel.FieldName.T_NAME,
                                 Lab_officer.FieldName.TABLE_NAME,User_list.FieldName.TABLE_NAME,
                                 FieldName.CURRI_ID,curri_id,FieldName.ACA_YEAR,aca_year,
                                 FieldName.LAB_NUM,Lab_officer.FieldName.LAB_NUM,Personnel.FieldName.USER_ID,
                                 FieldName.NAME) ;
        }
        public async Task<object> SelectByCurriculumAcademic()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<Lab_list_detail> result = new List<Lab_list_detail>();

            d.iCommand.CommandText = getSelectByCurriculumAcademicCommand();
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        int labid = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.LAB_NUM].Ordinal]);
                        if (result.FirstOrDefault(t => t.lab_num == labid) == null)
                            result.Add(new Lab_list_detail
                            {
                                lab_num = labid,
                                aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                                room = item.ItemArray[data.Columns[FieldName.ROOM].Ordinal].ToString()
                            });
                        if (item.ItemArray[data.Columns[Personnel.FieldName.USER_TYPE].Ordinal].ToString() == "อาจารย์")
                            result.First(t => t.lab_num == labid).officer.Add(new Personnel_with_t_name
                            {
                                user_id = Convert.ToInt32(item.ItemArray[data.Columns[Lab_officer.FieldName.OFFICER].Ordinal]),
                                t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Personnel.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Personnel.FieldName.T_NAME].Ordinal].ToString()
                            });
                        else
                            result.First(t => t.lab_num == labid).officer.Add(new Personnel_with_t_name
                            {
                                user_id = Convert.ToInt32(item.ItemArray[data.Columns[Lab_officer.FieldName.OFFICER].Ordinal]),
                                t_name = item.ItemArray[data.Columns[Personnel.FieldName.T_PRENAME].Ordinal].ToString() + item.ItemArray[data.Columns[Personnel.FieldName.T_NAME].Ordinal].ToString()
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

        public async Task<object> Delete(List<Lab_list_detail> list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
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


        public async Task<object> UpdateLabListWithSelect(Lab_list_detail ldata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
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
                    insertintolabofficer += string.Format("({0},{1})", ldata.lab_num, p.user_id);
                if (p != ldata.officer.Last())
                    insertintolabofficer += ",";
            }

            curri_id = ldata.curri_id;
            aca_year = ldata.aca_year;
            string selectcmd = getSelectByCurriculumAcademicCommand();
            string updatecondition = string.Format("if exists (select * from {0} where {1} = {2}) ", FieldName.TABLE_NAME, FieldName.LAB_NUM, ldata.lab_num);
            d.iCommand.CommandText = string.Format("{4} BEGIN {0} {1} {2} {3} END", updatelablistcmd,
                deletefromlabofficer, insertintolabofficer, selectcmd,updatecondition);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        int labid = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.LAB_NUM].Ordinal]);
                        if (result.FirstOrDefault(t => t.lab_num == labid) == null)
                            result.Add(new Lab_list_detail
                            {
                                lab_num = labid,
                                aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                                room = item.ItemArray[data.Columns[FieldName.ROOM].Ordinal].ToString()
                            });
                        if (item.ItemArray[data.Columns[Personnel.FieldName.USER_TYPE].Ordinal].ToString() == "อาจารย์")
                            result.First(t => t.lab_num == labid).officer.Add(new Personnel_with_t_name
                            {
                                user_id = Convert.ToInt32(item.ItemArray[data.Columns[Lab_officer.FieldName.OFFICER].Ordinal]),
                                t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Personnel.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Personnel.FieldName.T_NAME].Ordinal].ToString()
                            });
                        else
                            result.First(t => t.lab_num == labid).officer.Add(new Personnel_with_t_name
                            {
                                user_id = Convert.ToInt32(item.ItemArray[data.Columns[Lab_officer.FieldName.OFFICER].Ordinal]),
                                t_name = item.ItemArray[data.Columns[Personnel.FieldName.T_PRENAME].Ordinal].ToString() + item.ItemArray[data.Columns[Personnel.FieldName.T_NAME].Ordinal].ToString()
                            });
                    }
                    data.Dispose();
                }
                else
                {
                    //Reserved for return error string
                    res.Close();
                    return "ไม่พบข้อมูลห้องปฏิบัติการที่ต้องการแก้ไขในระบบ";
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


        public async Task<object> InsertNewLabListWithSelect(Lab_list_detail ldata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<Lab_list_detail> result = new List<Lab_list_detail>();
            string temp1tablename = "#temp1";
            string temp2tablename = "#temp2";
            string createtabletemp1 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] INT NOT NULL," +
                                      "PRIMARY KEY ([row_num])) ", temp1tablename, FieldName.LAB_NUM);
            string createtabletemp2 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] INT NULL," +
                                      "PRIMARY KEY ([row_num])) "
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
                    insertintotemp2 += string.Format(",({0})", p.user_id);
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
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        int labid = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.LAB_NUM].Ordinal]);
                        if (result.FirstOrDefault(t => t.lab_num == labid) == null)
                            result.Add(new Lab_list_detail
                            {
                                lab_num = labid,
                                aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                                room = item.ItemArray[data.Columns[FieldName.ROOM].Ordinal].ToString()
                            });
                        if (item.ItemArray[data.Columns[Personnel.FieldName.USER_TYPE].Ordinal].ToString() == "อาจารย์")
                            result.First(t => t.lab_num == labid).officer.Add(new Personnel_with_t_name
                            {
                                user_id = Convert.ToInt32(item.ItemArray[data.Columns[Lab_officer.FieldName.OFFICER].Ordinal]),
                                t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Personnel.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Personnel.FieldName.T_NAME].Ordinal].ToString()
                            });
                        else
                            result.First(t => t.lab_num == labid).officer.Add(new Personnel_with_t_name
                            {
                                user_id = Convert.ToInt32(item.ItemArray[data.Columns[Lab_officer.FieldName.OFFICER].Ordinal]),
                                t_name = item.ItemArray[data.Columns[Personnel.FieldName.T_PRENAME].Ordinal].ToString() + item.ItemArray[data.Columns[Personnel.FieldName.T_NAME].Ordinal].ToString()
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