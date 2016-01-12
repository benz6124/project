using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oIndicator : Indicator
    {
        public object Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oIndicator> result = new List<oIndicator>();
            d.iCommand.CommandText = String.Format("select * from indicator");
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        this.aca_year = Convert.ToInt32(item.ItemArray[data.Columns["aca_year"].Ordinal]);
                        this.indicator_num = Convert.ToInt32(item.ItemArray[data.Columns["indicator_num"].Ordinal]);
                        this.indicator_name = item.ItemArray[data.Columns["indicator_name"].Ordinal].ToString();
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
            List<oIndicator> result = new List<oIndicator>();
            d.iCommand.CommandText = String.Format("select * from indicator where {0}",wherecond);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        this.aca_year = Convert.ToInt32(item.ItemArray[data.Columns["aca_year"].Ordinal]);
                        this.indicator_num = Convert.ToInt32(item.ItemArray[data.Columns["indicator_num"].Ordinal]);
                        this.indicator_name = item.ItemArray[data.Columns["indicator_name"].Ordinal].ToString();
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

        public object SelectCustom(string wherecond, string groupbycol, string havingcond, string orderbycol)
        {
            return "Ok";
        }
    }
}