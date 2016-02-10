﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oAlumni : Alumni
    {
        public static string getSelectAlumniByJoinCommand()
        {
            return string.Format("select {0}.{1},{2},{3},{4},{5}," +
            "{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}," +
            "{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29} " +
            "from {30},{0} where {0}.{1} = {30}.{31}",
            /**tablename 0 **/ ExtraFieldName.TABLE_NAME, /**iden 1**/ FieldName.USER_ID, FieldName.USER_TYPE, FieldName.USERNAME,
            FieldName.PASSWORD, FieldName.T_PRENAME, FieldName.T_NAME, FieldName.E_PRENAME, FieldName.E_NAME,
            FieldName.CITIZEN_ID, FieldName.GENDER, FieldName.EMAIL, FieldName.TEL, FieldName.ADDR,
            FieldName.FILE_NAME_PIC, FieldName.TIMESTAMP,  /***common 15***/

            /**extended data**/
            FieldName.STUDENT_ID, FieldName.CURRI_ID, FieldName.TYPE, FieldName.ADMIS_YEAR, FieldName.ADMIS_DATE,
            FieldName.GRAD_YEAR, FieldName.GRAD_SEMESTER, FieldName.GRAD_DATE, FieldName.STATUS, FieldName.QUOTA,
            FieldName.SUBTYPE, FieldName.COOP,ExtraFieldName.COMPANY_ADDR,ExtraFieldName.COMPANY_TEL,

            User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_ID);
        }
        public object Insert(List<UsernamePassword> list, List<string> target_curri_id_list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<string> result = new List<string>();

            string temp5tablename = "#temp5";

            string createtabletemp5 = string.Format("CREATE TABLE {0}(" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] VARCHAR(60) NULL," +
                                      "PRIMARY KEY ([row_num])) " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} VARCHAR(60) COLLATE DATABASE_DEFAULT ",
                                      temp5tablename, FieldName.EMAIL);

            string insertcmd = "";
            string insertintousercurri;
            foreach (UsernamePassword item in list)
            {
                insertintousercurri = string.Format("insert into {0} values ", User_curriculum.FieldName.TABLE_NAME);
                int len = insertintousercurri.Length;

                foreach (string curriitem in target_curri_id_list)
                {
                    if (insertintousercurri.Length <= len)
                        insertintousercurri += string.Format("('{0}', '{1}')", item.username, curriitem);
                    else
                        insertintousercurri += string.Format(",('{0}', '{1}')", item.username, curriitem);
                }
                //since no value to be insert in user_curriculum so we make this var as empty string
                if (insertintousercurri.Length <= len)
                    insertintousercurri = "";

                string ts = DateTime.Now.GetDateTimeFormats(new System.Globalization.CultureInfo("en-US"))[93];

                insertcmd += string.Format("IF NOT EXISTS(select * from {0} where {1} = '{2}') and " +
                                   "NOT EXISTS(select * from {3} where {4} = '{2}' or {13} = '{2}') and " +
                                   "NOT EXISTS(select * from {5} where {4} = '{2}' or {13} = '{2}') and " +
                                   "NOT EXISTS(select * from {6} where {4} = '{2}' or {13} = '{2}') and " +
                                   "NOT EXISTS(select * from {7} where {4} = '{2}' or {13} = '{2}') and " +
                                   "NOT EXISTS(select * from {8} where {4} = '{2}' or {13} = '{2}') and " +
                                   "NOT EXISTS(select * from {9} where {4} = '{2}' or {13} = '{2}') and " +
                                   "NOT EXISTS(select * from {19} where {4} = '{2}' or {13} = '{2}') " +
                                   "begin " +
                                   "insert into {0} values('{2}', '{10}') " +
                                   "insert into {3} ({11}, {12}, {13}, {14}, {4}, {15}) values ('{2}', '{10}', '{2}', '{16}', '{2}', '{17}') " +
                                   insertintousercurri + " " +
                                   "end " +
                                   "else " +
                                   "begin " +
                                   "insert into {18} values ('{2}') " +
                                   "end ",
                                   User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_ID, item.username,
                                   /*Main table index 3 must SWAP!*/ ExtraFieldName.TABLE_NAME,
                                   FieldName.EMAIL, Student.FieldName.TABLE_NAME,
                                   Teacher.FieldName.TABLE_NAME, Staff.FieldName.TABLE_NAME,
                                   Company.FieldName.TABLE_NAME, Assessor.FieldName.TABLE_NAME,
                                   /*******10*/ "ศิษย์เก่า",
                                   /*******11 ID*/FieldName.STUDENT_ID, FieldName.USER_TYPE, FieldName.USERNAME,
                                   FieldName.PASSWORD, FieldName.TIMESTAMP, item.password, ts, temp5tablename,
                                   Admin.FieldName.TABLE_NAME);

            }

            string selectcmd = string.Format("select {1} from {0} ", temp5tablename, FieldName.EMAIL);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} END ", createtabletemp5, insertcmd, selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(
                            item.ItemArray[data.Columns[FieldName.EMAIL].Ordinal].ToString()
                        );
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