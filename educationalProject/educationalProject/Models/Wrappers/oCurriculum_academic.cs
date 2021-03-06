﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oCurriculum_academic : Curriculum_academic
    {
        public object Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<oCurriculum_academic> result = new List<oCurriculum_academic>();
            d.iCommand.CommandText = string.Format("select * from {0}",FieldName.TABLE_NAME);
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
        public async Task<object> SelectMaxAcademicYear()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<oCurriculum_academic> result = new List<oCurriculum_academic>();
            d.iCommand.CommandText = string.Format("select MAX({1})+1 from {0}", FieldName.TABLE_NAME,FieldName.ACA_YEAR);
            try
            {
                object res = await d.iCommand.ExecuteScalarAsync();
                if (res != null)
                {
                    return res;
                }
                else
                {
                    //Reserved for return error string
                    return null;
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

        public async Task<object> SelectByCurriculum()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<oCurriculum_academic> result = new List<oCurriculum_academic>();
            d.iCommand.CommandText = string.Format("select * from {0} where {1} = {2}", FieldName.TABLE_NAME,FieldName.CURRI_ID,ParameterName.CURRI_ID);
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
                        result.Add(new oCurriculum_academic
                        {
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal])
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

        public async Task<object> Insert()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;

            d.iCommand.CommandText = string.Format("insert into {0} values ({1},{2})",
                FieldName.TABLE_NAME, ParameterName.CURRI_ID, ParameterName.ACA_YEAR);
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURRI_ID, curri_id));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.ACA_YEAR, aca_year));
            try
            {
                await d.iCommand.ExecuteNonQueryAsync();
                return null;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2601 || ex.Number == 2627)
                    //Handle error from sql execution 2601 - duplicate
                    return "Duplicate";
                else
                    return ex.Message;
            }
            finally
            {
                //Whether it success or not it must close connection in order to end block
                d.SQLDisconnect();
            }
        }

        public async Task<object> SelectDistinctAcademicYear()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<int> result = new List<int>();
            d.iCommand.CommandText = string.Format("select distinct {1} from {0}", FieldName.TABLE_NAME,FieldName.ACA_YEAR);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]));
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