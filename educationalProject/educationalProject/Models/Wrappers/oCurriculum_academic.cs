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
                        result.Add(new oCurriculum_academic
                        {
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal])
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
            List<oCurriculum_academic> result = new List<oCurriculum_academic>();
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
                        result.Add(new oCurriculum_academic
                        {
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal])
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

            d.iCommand.CommandText = String.Format("insert into {0} values ('{1}',{2})",
                FieldName.TABLE_NAME, curri_id, aca_year);
            try
            {
                int rowAffected = d.iCommand.ExecuteNonQuery();
                if (rowAffected == 1)
                {
                    return null;
                }
                else
                {
                    return "No cu_curriculum academic are inserted.";
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

        public object SelectCustom(string wherecond, string groupbycol, string havingcond, string orderbycol)
        {
            return "Ok";
        }
    }
}