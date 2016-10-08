using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Utils;

namespace educationalProject.Models.Wrappers
{
    public class oUser_type : User_type
    {
        public async Task<object> SelectExcludeUserType(int mode)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<oUser_type> result = new List<oUser_type>();
            if(mode == 0)
                d.iCommand.CommandText = string.Format("select * from {0} where {1} != 'ผู้ดูแลระบบ'", FieldName.TABLE_NAME,FieldName.USER_TYPE_NAME);
            else if(mode == 1)
                d.iCommand.CommandText = string.Format("select * from {0} where {1} != 'ผู้ดูแลระบบ' and {1} != 'กรรมการหลักสูตร'", FieldName.TABLE_NAME, FieldName.USER_TYPE_NAME);
            else
                d.iCommand.CommandText = string.Format("select * from {0} ", FieldName.TABLE_NAME);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oUser_type
                        {
                            user_type_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.USER_TYPE_ID].Ordinal]),
                            user_type = item.ItemArray[data.Columns[FieldName.USER_TYPE_NAME].Ordinal].ToString()
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

        public async Task<object> insertNewUserType(List<string> usrtypedata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            string insertintousrtypeanddefpriv = "";
            foreach (string type in usrtypedata)
            {
                //check whether target user_type is already exists!
                insertintousrtypeanddefpriv += string.Format("if not exists (select * from {0} where {1} = '{2}') ", FieldName.TABLE_NAME, FieldName.USER_TYPE_NAME, type);
                insertintousrtypeanddefpriv += string.Format("BEGIN insert into {0} values ('{1}') ", FieldName.TABLE_NAME, type);
                //insert default privilege as value 1 for every privilege to target user type
                insertintousrtypeanddefpriv += string.Format("insert into {0} " +
                                               "select * from " +
                                               "(select {1} from {2} where {3} = '{4}') as targetusrtype, " +
                                               "(select {5}, {6} from {7} where {6} = 1) as titleprivdefault END ",
                                               Default_privilege_by_type.FieldName.TABLE_NAME, FieldName.USER_TYPE_ID,
                                               FieldName.TABLE_NAME,
                                               FieldName.USER_TYPE_NAME, type, Title_privilege.FieldName.TITLE_CODE,
                                               Title_privilege.FieldName.TITLE_PRIVILEGE_CODE, Title_privilege.FieldName.TABLE_NAME);
            }
            d.iCommand.CommandText = insertintousrtypeanddefpriv;
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