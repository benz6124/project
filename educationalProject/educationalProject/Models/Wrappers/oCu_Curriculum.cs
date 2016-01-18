using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oCu_curriculum : Cu_curriculum
    {
        public object Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oCu_curriculum> result = new List<oCu_curriculum>();
            d.iCommand.CommandText = String.Format("select * from {0}",FieldName.TABLE_NAME);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
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
        
        public object SelectWhere(string wherecond)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oCu_curriculum> result = new List<oCu_curriculum>();
            d.iCommand.CommandText = String.Format("select * from {0} where {1}",FieldName.TABLE_NAME,wherecond);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
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

        public object Insert()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            
            d.iCommand.CommandText = String.Format("insert into {0} values ((select MAX({1})+1 FROM {0}),'{2}','{3}','','','','','','','')",
                FieldName.TABLE_NAME, FieldName.CURRI_ID, year, curr_tname);
            try
            {
                int rowAffected = d.iCommand.ExecuteNonQuery();
                if (rowAffected == 1)
                {
                    return null;
                }
                else
                {
                    return "No cu_curriculum are inserted.";
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
        }
        public object SelectCustom(string wherecond,string groupbycol,string havingcond,string orderbycol)
        {
            return "Ok";
        }
    }
}