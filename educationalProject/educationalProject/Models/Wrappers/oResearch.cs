using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
using educationalProject.Models.ViewModels;
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

        public object SelectWithDetailByCurriculum(string curri_id_data)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Research_detail> result = new List<Research_detail>();
            d.iCommand.CommandText = string.Format("select r.*,{0}.{1},{0}.{2} from " +
                "(select {3}.{4},{3}.{5},{3}.{6}," +
                "{3}.{7}, {3}.{8},{9} from {3}, {10} where " +
                "{5} = '{11}' and {3}.{4} = {10}.{12}) as r,{0} where r.{9} = {0}.{13}",
                Teacher.FieldName.TABLE_NAME,Teacher.FieldName.T_PRENAME,Teacher.FieldName.T_NAME,
                FieldName.TABLE_NAME,FieldName.RESEARCH_ID,FieldName.CURRI_ID,FieldName.FILE_NAME,
                FieldName.NAME,FieldName.YEAR_PUBLISH,Research_owner.FieldName.TEACHER_ID,
                Research_owner.FieldName.TABLE_NAME,curri_id_data,Research_owner.FieldName.RESEARCH_ID,
                Teacher.FieldName.TEACHER_ID);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    research_id = -1;
                    Research_detail curr = null;
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        if(research_id != Convert.ToInt32(item.ItemArray[data.Columns[FieldName.RESEARCH_ID].Ordinal]))
                        {
                            curr = new Research_detail
                            {
                                name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                file_name = item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString(),
                                research_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.RESEARCH_ID].Ordinal]),
                                year_publish = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.YEAR_PUBLISH].Ordinal])
                            };
                            research_id = curr.research_id;
                            result.Add(curr);
                        }
                        curr.researcher.Add(new Teacher_with_t_name
                        {
                            teacher_id = item.ItemArray[data.Columns[Teacher.FieldName.TEACHER_ID].Ordinal].ToString(),
                            t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
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