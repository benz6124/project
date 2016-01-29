using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oResearch : Research
    {
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
    }
}