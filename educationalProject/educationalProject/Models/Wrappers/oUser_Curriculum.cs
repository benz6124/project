﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Utils;
using educationalProject.Models.ViewModels;
namespace educationalProject.Models.Wrappers
{
    public class oUser_curriculum : User_curriculum
    {
        public async Task<object> InsertNewUserCurriculumWithSelect(List<User_curriculum> list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<User_curriculum_with_brief_detail> result = new List<User_curriculum_with_brief_detail>();

            string insertifnotexistscmd = "";
            foreach (User_curriculum c in list)
            {
                insertifnotexistscmd += string.Format("if not exists(select * from {0} where {1} = '{2}' and {3} = {4}) " +
                "BEGIN " +
                "INSERT INTO {0} values({4}, '{2}') " +
                "END ",
                FieldName.TABLE_NAME, FieldName.CURRI_ID, c.curri_id, FieldName.USER_ID, c.user_id);
            }

            string selectcmd = ViewModels.Wrappers.oPersonnel.GetSelectWithCurriculumCommand(list.First().curri_id);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} END", insertifnotexistscmd, selectcmd);
            
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        string usrtype = item.ItemArray[data.Columns[User_type.FieldName.USER_TYPE_NAME].Ordinal].ToString();
                        if (usrtype == "อาจารย์")
                            result.Add(new User_curriculum_with_brief_detail
                            {
                                user_id = Convert.ToInt32(item.ItemArray[data.Columns[User_list.FieldName.USER_ID].Ordinal]),
                                t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[User_list.FieldName.T_PRENAME].Ordinal].ToString()) +
                                         item.ItemArray[data.Columns[User_list.FieldName.T_NAME].Ordinal].ToString(),
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                file_name_pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[data.Columns[User_list.FieldName.FILE_NAME_PIC].Ordinal].ToString()),
                                type = usrtype
                            });
                        else
                            result.Add(new User_curriculum_with_brief_detail
                            {
                                user_id = Convert.ToInt32(item.ItemArray[data.Columns[User_list.FieldName.USER_ID].Ordinal]),
                                t_name = item.ItemArray[data.Columns[User_list.FieldName.T_PRENAME].Ordinal].ToString() +
                                     item.ItemArray[data.Columns[User_list.FieldName.T_NAME].Ordinal].ToString(),
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                file_name_pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[data.Columns[User_list.FieldName.FILE_NAME_PIC].Ordinal].ToString()),
                                type = usrtype
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



        public async Task<object> Delete(List<User_curriculum> list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;


            string deletecmd = string.Format("delete from {0} where {1} = '{2}' ", FieldName.TABLE_NAME,
                FieldName.CURRI_ID,list.First().curri_id);
            string excludecond = "";
            foreach (User_curriculum c in list)
            {
                excludecond += string.Format("and {0} != {1} ", FieldName.USER_ID, c.user_id);
            }
            deletecmd += excludecond;

            d.iCommand.CommandText = deletecmd;

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