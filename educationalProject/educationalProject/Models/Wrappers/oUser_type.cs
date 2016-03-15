﻿using System;
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
                d.iCommand.CommandText = string.Format("select * from {0} where {1} != 'ผู้ดูแลระบบ'", FieldName.TABLE_NAME,FieldName.USER_TYPE);
            else if(mode == 1)
                d.iCommand.CommandText = string.Format("select * from {0} where {1} != 'ผู้ดูแลระบบ' and {1} != 'กรรมการหลักสูตร'", FieldName.TABLE_NAME, FieldName.USER_TYPE);
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
                            user_type = item.ItemArray[data.Columns[FieldName.USER_TYPE].Ordinal].ToString()
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
                return WebApiApplication.CONNECTDBERRSTRING;
            List<oUser_type> result = new List<oUser_type>();
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
                        result.Add(new oUser_type
                        {
                            user_type = item.ItemArray[data.Columns[FieldName.USER_TYPE].Ordinal].ToString()
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
        //public async Task<object> insertNewUserType(List<string> usrtypedata)
        //{
        //    DBConnector d = new DBConnector();
        //    if (!d.SQLConnect())
        //        return WebApiApplication.CONNECTDBERRSTRING;
        //    int oldlength = insertintousertype.Length;

        //    foreach(string type in usrtypedata)
        //    {
        //        if(insertintousertype.Length <= oldlength)
        //            insertintousertype += string.Format("('{0}'")
        //    }
        //}
    }
}