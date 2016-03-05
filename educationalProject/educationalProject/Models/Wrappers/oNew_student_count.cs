using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oNew_student_count : New_student_count
    {
        public object Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oNew_student_count> result = new List<oNew_student_count>();
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
                        result.Add(new oNew_student_count
                        {
                            num_admis_f = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_ADMIS_F].Ordinal]),
                            num_admis_m = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_ADMIS_M].Ordinal]),
                            num_childstaff_f = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_CHILDSTAFF_F].Ordinal]),
                            num_childstaff_m = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_CHILDSTAFF_M].Ordinal]),
                            num_direct_f = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_DIRECT_F].Ordinal]),
                            num_direct_m = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_DIRECT_M].Ordinal]),
                            num_goodstudy_f = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_GOODSTUDY_F].Ordinal]),
                            num_goodstudy_m = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_GOODSTUDY_M].Ordinal]),
                            num_others_f = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_OTHERS_F].Ordinal]),
                            num_others_m = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_OTHERS_M].Ordinal]),
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.YEAR].Ordinal])
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

        public async Task<object> SelectWhereByCurriculumAcademic()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oNew_student_count> result = new List<oNew_student_count>();
            d.iCommand.CommandText = string.Format("select * from {0} where {1} = {2} and {3} = {4}", FieldName.TABLE_NAME,
                FieldName.CURRI_ID, ParameterName.CURRI_ID, FieldName.YEAR, ParameterName.YEAR);
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURRI_ID, curri_id));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.YEAR, year));
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oNew_student_count
                        {
                            num_admis_f = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_ADMIS_F].Ordinal]),
                            num_admis_m = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_ADMIS_M].Ordinal]),
                            num_childstaff_f = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_CHILDSTAFF_F].Ordinal]),
                            num_childstaff_m = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_CHILDSTAFF_M].Ordinal]),
                            num_direct_f = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_DIRECT_F].Ordinal]),
                            num_direct_m = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_DIRECT_M].Ordinal]),
                            num_goodstudy_f = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_GOODSTUDY_F].Ordinal]),
                            num_goodstudy_m = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_GOODSTUDY_M].Ordinal]),
                            num_others_f = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_OTHERS_F].Ordinal]),
                            num_others_m = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NUM_OTHERS_M].Ordinal]),
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.YEAR].Ordinal])
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

        public async Task<object> InsertOrUpdate()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";

            d.iCommand.CommandText = string.Format("IF NOT EXISTS (select * from {0} where {1}={2} and {3} = {4}) " +
                                       "BEGIN " +
                                       "INSERT INTO {0} VALUES " +
                                       "({2}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11},{12},{13},{14}) " +
                                       "END " +
                                       "ELSE " +
                                       "BEGIN " +
                                       "UPDATE {0} SET {15} = {5},{16} = {6},{17} = {7},{18} = {8},{19} = {9},{20} = {10},{21} = {11},{22} = {12},{23} = {13},{24} = {14} where {1} = {2} and {3} = {4} " +
                                       "END",
                FieldName.TABLE_NAME, FieldName.CURRI_ID, ParameterName.CURRI_ID, FieldName.YEAR, ParameterName.YEAR,
                ParameterName.NUM_GOODSTUDY_M, ParameterName.NUM_GOODSTUDY_F, ParameterName.NUM_CHILDSTAFF_M,
                ParameterName.NUM_CHILDSTAFF_F, ParameterName.NUM_DIRECT_M, ParameterName.NUM_DIRECT_F,
                ParameterName.NUM_ADMIS_M, ParameterName.NUM_ADMIS_F, ParameterName.NUM_OTHERS_M, ParameterName.NUM_OTHERS_F,
                    FieldName.NUM_GOODSTUDY_M, FieldName.NUM_GOODSTUDY_F, FieldName.NUM_CHILDSTAFF_M, FieldName.NUM_CHILDSTAFF_F, FieldName.NUM_DIRECT_M, FieldName.NUM_DIRECT_F, FieldName.NUM_ADMIS_M, FieldName.NUM_ADMIS_F,FieldName.NUM_OTHERS_M,FieldName.NUM_OTHERS_F);
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURRI_ID, curri_id));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.YEAR, year));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.NUM_GOODSTUDY_M, num_goodstudy_m));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.NUM_GOODSTUDY_F, num_goodstudy_f));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.NUM_CHILDSTAFF_M, num_childstaff_m));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.NUM_CHILDSTAFF_F, num_childstaff_f));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.NUM_DIRECT_M, num_direct_m));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.NUM_DIRECT_F, num_direct_f));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.NUM_ADMIS_M, num_admis_m));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.NUM_ADMIS_F, num_admis_f));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.NUM_OTHERS_M, num_others_m));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.NUM_OTHERS_F, num_others_f));
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
    }
}