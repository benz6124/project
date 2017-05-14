using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Dynamic;
using System.Threading.Tasks;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oNew_student_count
    {

        public async Task<object> SelectWhereByCurriculumAcademicAndLevel(dynamic curri_aca_lv)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            dynamic result = new ExpandoObject();


            if (curri_aca_lv.lv == "2" || curri_aca_lv.lv == "3")
            {
                //select data from New_student_count_grad table
                d.iCommand.CommandText = string.Format("select * from {0} where {1} = {2} and {3} = {4}", New_student_count_grad.FieldName.TABLE_NAME,
                    New_student_count_grad.FieldName.CURRI_ID, New_student_count_grad.ParameterName.CURRI_ID, New_student_count_grad.FieldName.YEAR, New_student_count_grad.ParameterName.YEAR);
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_grad.ParameterName.CURRI_ID, curri_aca_lv.curri_id));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_grad.ParameterName.YEAR, curri_aca_lv.year));
            }
            else //curri lv = 1 or invalid value => select data from New_student_count_ungrad table
            {
                d.iCommand.CommandText = string.Format("select * from {0} where {1} = {2} and {3} = {4}", New_student_count_ungrad.FieldName.TABLE_NAME,
                New_student_count_ungrad.FieldName.CURRI_ID, New_student_count_ungrad.ParameterName.CURRI_ID, New_student_count_ungrad.FieldName.YEAR, New_student_count_ungrad.ParameterName.YEAR);
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_ungrad.ParameterName.CURRI_ID, curri_aca_lv.curri_id));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_ungrad.ParameterName.YEAR, curri_aca_lv.year));
            }

            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    //Set result to desired property base on lv of curri
                    if (curri_aca_lv.lv == "2" || curri_aca_lv.lv == "3")
                    {
                        result.a1_m = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_grad.FieldName.A1_M].Ordinal]);
                        result.a1_f = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_grad.FieldName.A1_F].Ordinal]);
                        result.a2_m = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_grad.FieldName.A2_M].Ordinal]);
                        result.a2_f = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_grad.FieldName.A2_F].Ordinal]);
                        result.b1_m = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_grad.FieldName.B1_M].Ordinal]);
                        result.b1_f = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_grad.FieldName.B1_F].Ordinal]);
                        result.fw41_m = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_grad.FieldName.FW41_M].Ordinal]);
                        result.fw41_f = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_grad.FieldName.FW41_F].Ordinal]);
                        result.others_m = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_grad.FieldName.OTHERS_M].Ordinal]);
                        result.others_f = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_grad.FieldName.OTHERS_F].Ordinal]);
                        result.curri_id = data.Rows[0].ItemArray[data.Columns[New_student_count_grad.FieldName.CURRI_ID].Ordinal].ToString();
                        result.year = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_grad.FieldName.YEAR].Ordinal]);
                    }
                    else
                    {
                        result.num_admis_f = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_ungrad.FieldName.NUM_ADMIS_F].Ordinal]);
                        result.num_admis_m = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_ungrad.FieldName.NUM_ADMIS_M].Ordinal]);
                        result.num_childstaff_f = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_ungrad.FieldName.NUM_CHILDSTAFF_F].Ordinal]);
                        result.num_childstaff_m = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_ungrad.FieldName.NUM_CHILDSTAFF_M].Ordinal]);
                        result.num_direct_f = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_ungrad.FieldName.NUM_DIRECT_F].Ordinal]);
                        result.num_direct_m = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_ungrad.FieldName.NUM_DIRECT_M].Ordinal]);
                        result.num_goodstudy_f = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_ungrad.FieldName.NUM_GOODSTUDY_F].Ordinal]);
                        result.num_goodstudy_m = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_ungrad.FieldName.NUM_GOODSTUDY_M].Ordinal]);
                        result.num_others_f = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_ungrad.FieldName.NUM_OTHERS_F].Ordinal]);
                        result.num_others_m = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_ungrad.FieldName.NUM_OTHERS_M].Ordinal]);
                        result.curri_id = data.Rows[0].ItemArray[data.Columns[New_student_count_ungrad.FieldName.CURRI_ID].Ordinal].ToString();
                        result.year = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[New_student_count_ungrad.FieldName.YEAR].Ordinal]);
                    }
                    data.Dispose();
                }
                else //if no row return => set default new student stat result to all zeros
                {
                    if (curri_aca_lv.lv == "2" || curri_aca_lv.lv == "3")
                    {
                        result.a1_m = 0;
                        result.a1_f = 0;
                        result.a2_m = 0;
                        result.a2_f = 0;
                        result.b1_m = 0;
                        result.b1_f = 0;
                        result.fw41_m = 0;
                        result.fw41_f = 0;
                        result.others_m = 0;
                        result.others_f = 0;
                        result.curri_id = curri_aca_lv.curri_id;
                        result.year = curri_aca_lv.year;
                    }
                    else
                    {
                        result.num_admis_f = 0;
                        result.num_admis_m = 0;
                        result.num_childstaff_f = 0;
                        result.num_childstaff_m = 0;
                        result.num_direct_f = 0;
                        result.num_direct_m = 0;
                        result.num_goodstudy_f = 0;
                        result.num_goodstudy_m = 0;
                        result.num_others_f = 0;
                        result.num_others_m = 0;
                        result.curri_id = curri_aca_lv.curri_id;
                        result.year = curri_aca_lv.year;
                    }
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

        public async Task<object> InsertOrUpdate(dynamic newstudentcountdata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;

            if (newstudentcountdata.lv == "2" || newstudentcountdata.lv == "3")
            {
                //insert/update data in New_student_count_grad table
                d.iCommand.CommandText = string.Format("IF NOT EXISTS (select * from {0} where {1}={2} and {3} = {4}) " +
                            "BEGIN " +
                            "INSERT INTO {0} VALUES " +
                            "({2}, {4}, {5}, {6}, {7}, {8}, {9}, {10},{11},{12},{13},{14}) " +
                            "END " +
                            "ELSE " +
                            "BEGIN " +
                            "UPDATE {0} SET {15} = {5},{16} = {6},{17} = {7},{18} = {8},{19} = {9},{20} = {10},{21} = {11},{22} = {12},{23} = {13},{24} = {14} where {1} = {2} and {3} = {4} " +
                            "END", New_student_count_grad.FieldName.TABLE_NAME, New_student_count_grad.FieldName.CURRI_ID, New_student_count_grad.ParameterName.CURRI_ID, New_student_count_grad.FieldName.YEAR, New_student_count_grad.ParameterName.YEAR,
                            New_student_count_grad.ParameterName.A1_M, New_student_count_grad.ParameterName.A1_F,
                            New_student_count_grad.ParameterName.A2_M, New_student_count_grad.ParameterName.A2_F,
                            New_student_count_grad.ParameterName.B1_M, New_student_count_grad.ParameterName.B1_F,
                            New_student_count_grad.ParameterName.FW41_M, New_student_count_grad.ParameterName.FW41_F,
                            New_student_count_grad.ParameterName.OTHERS_M, New_student_count_grad.ParameterName.OTHERS_F,

                            New_student_count_grad.FieldName.A1_M, New_student_count_grad.FieldName.A1_F,
                            New_student_count_grad.FieldName.A2_M, New_student_count_grad.FieldName.A2_F,
                            New_student_count_grad.FieldName.B1_M, New_student_count_grad.FieldName.B1_F,
                            New_student_count_grad.FieldName.FW41_M, New_student_count_grad.FieldName.FW41_F,
                            New_student_count_grad.FieldName.OTHERS_M, New_student_count_grad.FieldName.OTHERS_F
                            );

                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_grad.ParameterName.CURRI_ID, newstudentcountdata.curri_id));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_grad.ParameterName.YEAR, newstudentcountdata.year));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_grad.ParameterName.A1_M, newstudentcountdata.a1_m));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_grad.ParameterName.A1_F, newstudentcountdata.a1_f));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_grad.ParameterName.A2_M, newstudentcountdata.a2_m));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_grad.ParameterName.A2_F, newstudentcountdata.a2_f));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_grad.ParameterName.B1_M, newstudentcountdata.b1_m));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_grad.ParameterName.B1_F, newstudentcountdata.b1_f));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_grad.ParameterName.FW41_M, newstudentcountdata.fw41_m));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_grad.ParameterName.FW41_F, newstudentcountdata.fw41_f));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_grad.ParameterName.OTHERS_M, newstudentcountdata.others_m));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_grad.ParameterName.OTHERS_F, newstudentcountdata.others_f));
            }
            else //curri lv = 1 => insert/update data from New_student_count_ungrad table
            {
                d.iCommand.CommandText = string.Format("IF NOT EXISTS (select * from {0} where {1}={2} and {3} = {4}) " +
                           "BEGIN " +
                           "INSERT INTO {0} VALUES " +
                           "({2}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11},{12},{13},{14}) " +
                           "END " +
                           "ELSE " +
                           "BEGIN " +
                           "UPDATE {0} SET {15} = {5},{16} = {6},{17} = {7},{18} = {8},{19} = {9},{20} = {10},{21} = {11},{22} = {12},{23} = {13},{24} = {14} where {1} = {2} and {3} = {4} " +
                           "END",
    New_student_count_ungrad.FieldName.TABLE_NAME, New_student_count_ungrad.FieldName.CURRI_ID, New_student_count_ungrad.ParameterName.CURRI_ID, New_student_count_ungrad.FieldName.YEAR, New_student_count_ungrad.ParameterName.YEAR,
    New_student_count_ungrad.ParameterName.NUM_GOODSTUDY_M, New_student_count_ungrad.ParameterName.NUM_GOODSTUDY_F, New_student_count_ungrad.ParameterName.NUM_CHILDSTAFF_M,
    New_student_count_ungrad.ParameterName.NUM_CHILDSTAFF_F, New_student_count_ungrad.ParameterName.NUM_DIRECT_M, New_student_count_ungrad.ParameterName.NUM_DIRECT_F,
    New_student_count_ungrad.ParameterName.NUM_ADMIS_M, New_student_count_ungrad.ParameterName.NUM_ADMIS_F, New_student_count_ungrad.ParameterName.NUM_OTHERS_M, New_student_count_ungrad.ParameterName.NUM_OTHERS_F,
        New_student_count_ungrad.FieldName.NUM_GOODSTUDY_M, New_student_count_ungrad.FieldName.NUM_GOODSTUDY_F, New_student_count_ungrad.FieldName.NUM_CHILDSTAFF_M, New_student_count_ungrad.FieldName.NUM_CHILDSTAFF_F, New_student_count_ungrad.FieldName.NUM_DIRECT_M, New_student_count_ungrad.FieldName.NUM_DIRECT_F, New_student_count_ungrad.FieldName.NUM_ADMIS_M, New_student_count_ungrad.FieldName.NUM_ADMIS_F, New_student_count_ungrad.FieldName.NUM_OTHERS_M, New_student_count_ungrad.FieldName.NUM_OTHERS_F);
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_ungrad.ParameterName.CURRI_ID, newstudentcountdata.curri_id));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_ungrad.ParameterName.YEAR, newstudentcountdata.year));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_ungrad.ParameterName.NUM_GOODSTUDY_M, newstudentcountdata.num_goodstudy_m));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_ungrad.ParameterName.NUM_GOODSTUDY_F, newstudentcountdata.num_goodstudy_f));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_ungrad.ParameterName.NUM_CHILDSTAFF_M, newstudentcountdata.num_childstaff_m));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_ungrad.ParameterName.NUM_CHILDSTAFF_F, newstudentcountdata.num_childstaff_f));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_ungrad.ParameterName.NUM_DIRECT_M, newstudentcountdata.num_direct_m));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_ungrad.ParameterName.NUM_DIRECT_F, newstudentcountdata.num_direct_f));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_ungrad.ParameterName.NUM_ADMIS_M, newstudentcountdata.num_admis_m));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_ungrad.ParameterName.NUM_ADMIS_F, newstudentcountdata.num_admis_f));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_ungrad.ParameterName.NUM_OTHERS_M, newstudentcountdata.num_others_m));
                d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(New_student_count_ungrad.ParameterName.NUM_OTHERS_F, newstudentcountdata.num_others_f));
            }

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