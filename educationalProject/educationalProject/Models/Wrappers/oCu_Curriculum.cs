using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oCu_curriculum : Cu_curriculum
    {
        public async Task<object> Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<oCu_curriculum> result = new List<oCu_curriculum>();
            d.iCommand.CommandText = string.Format("select * from {0}", FieldName.TABLE_NAME);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oCu_curriculum
                        {
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            curr_tname = item.ItemArray[data.Columns[FieldName.CURR_TNAME].Ordinal].ToString(),
                            curr_ename = item.ItemArray[data.Columns[FieldName.CURR_ENAME].Ordinal].ToString(),
                            degree_e_bf = item.ItemArray[data.Columns[FieldName.DEGREE_E_BF].Ordinal].ToString(),
                            degree_e_full = item.ItemArray[data.Columns[FieldName.DEGREE_E_FULL].Ordinal].ToString(),
                            degree_t_bf = item.ItemArray[data.Columns[FieldName.DEGREE_T_BF].Ordinal].ToString(),
                            degree_t_full = item.ItemArray[data.Columns[FieldName.DEGREE_T_FULL].Ordinal].ToString(),
                            level = Convert.ToChar(item.ItemArray[data.Columns[FieldName.LEVEL].Ordinal]),
                            period = Convert.ToChar(item.ItemArray[data.Columns[FieldName.PERIOD].Ordinal]),
                            year = item.ItemArray[data.Columns[FieldName.YEAR].Ordinal].ToString()
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
        
        public async Task<object> SelectByCurriID()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            d.iCommand.CommandText = string.Format("select * from {0} where {1} = {2}",FieldName.TABLE_NAME,FieldName.CURRI_ID,ParameterName.CURRI_ID);
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURRI_ID, curri_id));
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString();
                        curr_tname = item.ItemArray[data.Columns[FieldName.CURR_TNAME].Ordinal].ToString();
                        curr_ename = item.ItemArray[data.Columns[FieldName.CURR_ENAME].Ordinal].ToString();
                        degree_e_bf = item.ItemArray[data.Columns[FieldName.DEGREE_E_BF].Ordinal].ToString();
                        degree_e_full = item.ItemArray[data.Columns[FieldName.DEGREE_E_FULL].Ordinal].ToString();
                        degree_t_bf = item.ItemArray[data.Columns[FieldName.DEGREE_T_BF].Ordinal].ToString();
                        degree_t_full = item.ItemArray[data.Columns[FieldName.DEGREE_T_FULL].Ordinal].ToString();
                        level = Convert.ToChar(item.ItemArray[data.Columns[FieldName.LEVEL].Ordinal]);
                        period = Convert.ToChar(item.ItemArray[data.Columns[FieldName.PERIOD].Ordinal]);
                        year = item.ItemArray[data.Columns[FieldName.YEAR].Ordinal].ToString();
                    }
                    data.Dispose();
                }
                else
                {
                    res.Close();
                    return "ไม่พบข้อมูลหลักสูตรที่ท่านเลือก";
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
            return null;
        }

        public async Task<object> Insert()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            
            d.iCommand.CommandText = string.Format("insert into {0} values ((select MAX({1})+1 FROM {0}),{2},{3},{4},{5},{6},{7},{8},{9},{10})",
                FieldName.TABLE_NAME, FieldName.CURRI_ID, ParameterName.YEAR, ParameterName.CURR_TNAME, 
                ParameterName.CURR_ENAME, ParameterName.DEGREE_T_FULL, ParameterName.DEGREE_T_BF, 
                ParameterName.DEGREE_E_FULL, ParameterName.DEGREE_E_BF, ParameterName.LEVEL, ParameterName.PERIOD);

            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.YEAR, year));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURR_TNAME, curr_tname));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURR_ENAME, curr_ename));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.DEGREE_T_FULL, degree_t_full));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.DEGREE_T_BF, degree_t_bf));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.DEGREE_E_FULL, degree_e_full));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.DEGREE_E_BF, degree_e_bf));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.LEVEL, level));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.PERIOD, period));

            try
            {
                await d.iCommand.ExecuteNonQueryAsync();
                return null;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                //Handle error from sql execution
                if (ex.Number == 8152)
                    return "มีรายละเอียดของข้อมูลหลักสูตรบางส่วนที่ต้องการบันทึกมีขนาดที่ยาวเกินกำหนด";
                return ex.Message;
            }
            finally
            {
                //Whether it success or not it must close connection in order to end block
                d.SQLDisconnect();
            }
        }

        public async Task<object> InsertOrUpdate()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<oCu_curriculum> result = new List<oCu_curriculum>();
            string maincmd, selectcmd;
            if (curri_id == "0") /*curri_id 0 mean new curriculum*/
            {
                maincmd = string.Format("insert into {0} values ((select MAX({1})+1 FROM {0}),{2},{3},{4},{5},{6},{7},{8},{9},{10}) ",
                FieldName.TABLE_NAME, FieldName.CURRI_ID, ParameterName.YEAR, ParameterName.CURR_TNAME,
                ParameterName.CURR_ENAME, ParameterName.DEGREE_T_FULL, ParameterName.DEGREE_T_BF,
                ParameterName.DEGREE_E_FULL, ParameterName.DEGREE_E_BF, ParameterName.LEVEL, ParameterName.PERIOD);
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.YEAR, year));
            }
            else
            {
                maincmd = string.Format("update {0} set {1} = {2},{3} = {4},{5} = {6}" +
                    ",{7} = {8},{9} = {10},{11} = {12},{13} = {14},{15} = {16} where {17} = {18} ",
                    FieldName.TABLE_NAME, FieldName.CURR_TNAME, ParameterName.CURR_TNAME,
                    FieldName.CURR_ENAME, ParameterName.CURR_ENAME, FieldName.DEGREE_T_FULL, ParameterName.DEGREE_T_FULL,
                    FieldName.DEGREE_T_BF, ParameterName.DEGREE_T_BF, FieldName.DEGREE_E_FULL, ParameterName.DEGREE_E_FULL,
                    FieldName.DEGREE_E_BF, ParameterName.DEGREE_E_BF, FieldName.LEVEL, ParameterName.LEVEL,
                    FieldName.PERIOD, ParameterName.PERIOD, FieldName.CURRI_ID, ParameterName.CURRI_ID);
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURRI_ID, curri_id));
            }

            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURR_TNAME, curr_tname));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURR_ENAME, curr_ename));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.DEGREE_T_FULL, degree_t_full));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.DEGREE_T_BF, degree_t_bf));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.DEGREE_E_FULL, degree_e_full));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.DEGREE_E_BF, degree_e_bf));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.LEVEL, level));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.PERIOD, period));

            selectcmd = string.Format("select * from {0}", FieldName.TABLE_NAME);
            d.iCommand.CommandText = string.Format("BEGIN {0} {1} END", maincmd,selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oCu_curriculum
                        {
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            curr_tname = item.ItemArray[data.Columns[FieldName.CURR_TNAME].Ordinal].ToString(),
                            curr_ename = item.ItemArray[data.Columns[FieldName.CURR_ENAME].Ordinal].ToString(),
                            degree_e_bf = item.ItemArray[data.Columns[FieldName.DEGREE_E_BF].Ordinal].ToString(),
                            degree_e_full = item.ItemArray[data.Columns[FieldName.DEGREE_E_FULL].Ordinal].ToString(),
                            degree_t_bf = item.ItemArray[data.Columns[FieldName.DEGREE_T_BF].Ordinal].ToString(),
                            degree_t_full = item.ItemArray[data.Columns[FieldName.DEGREE_T_FULL].Ordinal].ToString(),
                            level = Convert.ToChar(item.ItemArray[data.Columns[FieldName.LEVEL].Ordinal]),
                            period = Convert.ToChar(item.ItemArray[data.Columns[FieldName.PERIOD].Ordinal]),
                            year = item.ItemArray[data.Columns[FieldName.YEAR].Ordinal].ToString()
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