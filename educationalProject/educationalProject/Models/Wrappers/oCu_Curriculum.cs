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
            d.iCommand.CommandText = String.Format("select * from cu_curriculum");
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        this.curri_id = item.ItemArray[data.Columns["curri_id"].Ordinal].ToString();
                        this.curr_tname = item.ItemArray[data.Columns["curr_tname"].Ordinal].ToString();
                        this.curr_ename = item.ItemArray[data.Columns["curr_ename"].Ordinal].ToString();
                        this.degree_e_bf = item.ItemArray[data.Columns["degree_e_bf"].Ordinal].ToString();
                        this.degree_e_full = item.ItemArray[data.Columns["degree_e_full"].Ordinal].ToString();
                        this.degree_t_bf = item.ItemArray[data.Columns["degree_t_bf"].Ordinal].ToString();
                        this.degree_t_full = item.ItemArray[data.Columns["degree_t_full"].Ordinal].ToString();
                        this.level = Convert.ToChar(item.ItemArray[data.Columns["level"].Ordinal]);
                        this.period = Convert.ToChar(item.ItemArray[data.Columns["period"].Ordinal]);
                        this.year = item.ItemArray[data.Columns["year"].Ordinal].ToString();
                        result.Add(this);
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
            d.iCommand.CommandText = String.Format("select * from cu_curriculum where {0}", wherecond);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        this.curri_id = item.ItemArray[data.Columns["curri_id"].Ordinal].ToString();
                        this.curr_tname = item.ItemArray[data.Columns["curr_tname"].Ordinal].ToString();
                        this.curr_ename = item.ItemArray[data.Columns["curr_ename"].Ordinal].ToString();
                        this.degree_e_bf = item.ItemArray[data.Columns["degree_e_bf"].Ordinal].ToString();
                        this.degree_e_full = item.ItemArray[data.Columns["degree_e_full"].Ordinal].ToString();
                        this.degree_t_bf = item.ItemArray[data.Columns["degree_t_bf"].Ordinal].ToString();
                        this.degree_t_full = item.ItemArray[data.Columns["degree_t_full"].Ordinal].ToString();
                        this.level = Convert.ToChar(item.ItemArray[data.Columns["level"].Ordinal]);
                        this.period = Convert.ToChar(item.ItemArray[data.Columns["period"].Ordinal]);
                        this.year = item.ItemArray[data.Columns["year"].Ordinal].ToString();
                        result.Add(this);
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

        public object SelectCustom(string wherecond,string groupbycol,string havingcond,string orderbycol)
        {
            return "Ok";
        }
    }
}