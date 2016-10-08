using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oDefault_privilege_by_type : Default_privilege_by_type
    {
        public async Task<object> SelectByTitle()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            Default_privilege_by_type_with_privilege_choices result = new Default_privilege_by_type_with_privilege_choices();

            string selectTitle = string.Format("select * from {0} where {1} = {2} ", Title.FieldName.TABLE_NAME,
                Title.FieldName.TITLE_CODE, title_code);
            string selectPrivilegeChoices = string.Format("select * from {0} where {1} = {2} ",Title_privilege.FieldName.TABLE_NAME,
                Title_privilege.FieldName.TITLE_CODE, title_code);

            string selectMainData = string.Format("select {0}.*,{1} " +
                "from {0},{2} " +
                "where {3} = {4} " +
                "and {0}.{5} = {2}.{6} ",
                FieldName.TABLE_NAME, User_type.FieldName.USER_TYPE_NAME,
                User_type.FieldName.TABLE_NAME,
                FieldName.TITLE_CODE, title_code, FieldName.USER_TYPE_ID, User_type.FieldName.USER_TYPE_ID);

            string selectSetDefaultData = string.Format("select {0}, {1},1 as {2},{3} " +
                "from {4},{5} " +
                "where {1} = {6} and {0} != 7 " +
                "and not exists(select * from {7} " +
                "where {7}.{8} = {5}.{1} " +
                "and {7}.{9} = {4}.{0}) ",
                User_type.FieldName.USER_TYPE_ID,Title.FieldName.TITLE_CODE,Title_privilege.FieldName.TITLE_PRIVILEGE_CODE,
                User_type.FieldName.USER_TYPE_NAME, User_type.FieldName.TABLE_NAME,
                Title.FieldName.TABLE_NAME,title_code,FieldName.TABLE_NAME,FieldName.TITLE_CODE,
                FieldName.USER_TYPE_ID);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} END", selectTitle, selectPrivilegeChoices,
                selectMainData, selectSetDefaultData);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();

                do
                {
                    if (res.HasRows)
                    {
                        DataTable tabledata = new DataTable();
                        tabledata.Load(res);
                        foreach (DataRow item in tabledata.Rows)
                        {
                            if (tabledata.Columns.Contains(Title.FieldName.NAME))
                            {
                                //Set title name from title table result
                                result.title_code = Convert.ToInt32(item.ItemArray[tabledata.Columns[Title.FieldName.TITLE_CODE].Ordinal]);
                                result.name = item.ItemArray[tabledata.Columns[Title.FieldName.NAME].Ordinal].ToString();
                            }
                            else if (tabledata.Columns.Contains(Title_privilege.FieldName.PRIVILEGE))
                            {
                                //Set privilege choice for target title
                                result.choices.Add(new Privilege_choice
                                {
                                    title_privilege_code = Convert.ToInt32(item.ItemArray[tabledata.Columns[Title_privilege.FieldName.TITLE_PRIVILEGE_CODE].Ordinal]),
                                    privilege = item.ItemArray[tabledata.Columns[Title_privilege.FieldName.PRIVILEGE].Ordinal].ToString()
                                });
                            }
                            else
                            {
                                //Read main privilege data
                                int title_priv_code = Convert.ToInt32(item.ItemArray[tabledata.Columns[FieldName.TITLE_PRIVILEGE_CODE].Ordinal]);
                                result.privilege_list.Add(new User_type_privilege
                                {
                                    user_type_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[FieldName.USER_TYPE_ID].Ordinal]),
                                    user_type = item.ItemArray[tabledata.Columns[User_type.FieldName.USER_TYPE_NAME].Ordinal].ToString(),
                                    privilege = new Privilege_choice {
                                        title_privilege_code = title_priv_code,
                                        //Find privilege caption from choices array
                                        privilege = result.choices.First(t => t.title_privilege_code == title_priv_code).privilege
                                    }
                                });
                            }
                        }
                        tabledata.Dispose();
                    }
                    else if (!res.IsClosed)
                    {
                        if (!res.NextResult())
                            break;
                    }
                } while (!res.IsClosed);
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

        public async Task<object> InsertOrUpdate(Default_privilege_by_type_with_privilege_choices ddata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;

            string InsertOrUpdateCommand = "";
            foreach (User_type_privilege ditem in ddata.privilege_list)
            {
                InsertOrUpdateCommand += string.Format("IF NOT EXISTS(select * from {0} where {1} = '{2}' and {3} = {4}) " +
                                         "BEGIN " +
                                         "INSERT INTO {0} values ('{2}',{4},'{5}') " +
                                         "END " +
                                         "ELSE " +
                                         "BEGIN " +
                                         "UPDATE {0} set {6} = '{5}' where {1} = '{2}' and {3} = '{4}' " +
                                         "END ", FieldName.TABLE_NAME, FieldName.USER_TYPE_ID, ditem.user_type_id, 
                                         FieldName.TITLE_CODE, ddata.title_code, ditem.privilege.title_privilege_code, FieldName.TITLE_PRIVILEGE_CODE);
            }

            d.iCommand.CommandText = InsertOrUpdateCommand;
            try
            {
                await d.iCommand.ExecuteNonQueryAsync();
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
            return null;
        }
    }
}