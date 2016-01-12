using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oCurriculum_academic : Curriculum_academic
    {
        public object Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oCurriculum_academic> result = new List<oCurriculum_academic>();
            d.iCommand.CommandText = String.Format("select * from curriculum_academic");
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
                        this.aca_year = Convert.ToInt32(item.ItemArray[data.Columns["aca_year"].Ordinal]);
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
            List<oCurriculum_academic> result = new List<oCurriculum_academic>();
            d.iCommand.CommandText = String.Format("select * from curriculum_academic where {0}",wherecond);
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
                        this.aca_year = Convert.ToInt32(item.ItemArray[data.Columns["aca_year"].Ordinal]);
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