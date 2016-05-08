using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oExtra_privilege : Extra_privilege
    {
        public async Task<object> SelectByCurriculumAndTitle()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            Extra_privilege_individual_list_with_privilege_choices result = new Extra_privilege_individual_list_with_privilege_choices();

            string temp5tablename = "#temp5";
            string createtabletemp5 = string.Format("CREATE TABLE {0}( " +
            "[row_num] int identity(1, 1) not null," +
            "[{1}] INT NULL," +
            "[{2}] VARCHAR(16) NULL," +
            "[{3}] VARCHAR(60) NULL," +
            "[{4}] {10} NULL," +
            "[{5}] {11} NULL," +
            "[{6}] INT NULL," +
            "[{7}] INT null," +
            "[{8}] varchar(80) null," +
            "[{9}] varchar(80) null," +
            "[{12}] VARCHAR(40) NULL," +
            "PRIMARY KEY([row_num])) " +

            "alter table {0} " +
            "alter column [{2}] VARCHAR(16) collate database_default " +

            "alter table {0} " +
            "alter column [{3}] VARCHAR(60) collate database_default " +

            "alter table {0} " +
            "alter column [{4}] {10} collate database_default " +

            "alter table {0} " +
            "alter column {5} {11} collate database_default " +

            "alter table {0} " +
            "alter column {8} varchar(80) collate database_default " +

            "alter table {0} " +
            "alter column {9} varchar(80) collate database_default " +
                        
            "alter table {0} " +
            "alter column {12} VARCHAR(40) collate database_default ",

            temp5tablename, FieldName.PERSONNEL_ID, Teacher.FieldName.T_PRENAME,
            Teacher.FieldName.T_NAME, Teacher.FieldName.FILE_NAME_PIC,
            FieldName.CURRI_ID, FieldName.TITLE_CODE, FieldName.TITLE_PRIVILEGE_CODE,
            Title.FieldName.NAME, Title_privilege.FieldName.PRIVILEGE,
            DBFieldDataType.FILE_NAME_TYPE,DBFieldDataType.CURRI_ID_TYPE,
            User_list.FieldName.USER_TYPE
            );

            string insertintotemp5_1 = string.Format("insert into {0} " +
            "select {1}, {2}, {3}, {4}, {5}, {6}.{7},{6}.{8},{9},{10},{18} " +
            "from {11},{6},({12}) as tp " +
            "where {5} = '{13}' and {14} = {1} " +
            "and {6}.{7} = tp.{15} " +
            "and {6}.{8} = tp.{16} " +
            "and {6}.{7} = {17} ",
            temp5tablename, FieldName.PERSONNEL_ID, Teacher.FieldName.T_PRENAME,
            Teacher.FieldName.T_NAME, Teacher.FieldName.FILE_NAME_PIC, FieldName.CURRI_ID,
            FieldName.TABLE_NAME, FieldName.TITLE_CODE, FieldName.TITLE_PRIVILEGE_CODE,
            Title.FieldName.NAME, Title_privilege.FieldName.PRIVILEGE,
            User_list.FieldName.TABLE_NAME, oTitle_privilege.getSelectTitlePrivilegeCommand(),
            curri_id, User_list.FieldName.USER_ID, Title.FieldName.TITLE_CODE,
            Title_privilege.FieldName.TITLE_PRIVILEGE_CODE,title_code,User_list.FieldName.USER_TYPE);

            string insertintotemp5_2 = string.Format("insert into {0} " +
            "select {1}.{2}, {3}, {4}, {5}, {6}.{7}, {6}.{8}, {6}.{9}, {10}, {11},{1}.{17} " +
            "from {1}, {12}, {6},({13}) as tp " +
            "where {12}.{14} = '{15}' " +
            "and {1}.{2} = {12}.{16} " +
            "and {1}.{17} = {6}.{18} " +
            "and {6}.{7} = {12}.{14} " +

            "and {6}.{8} = tp.{19} " +
            "and {6}.{8} = {20} " +

            "and {6}.{9} = tp.{21} " +


            "and not exists(select * from {22} " +
            "where {22}.{23} = {1}.{2} " +
            "and {24} = '{15}' " +
            "and {22}.{25} = {6}.{8}) ",
            temp5tablename, /*1*/User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_ID,
            Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME, Teacher.FieldName.FILE_NAME_PIC,
            Extra_privilege_by_type.FieldName.TABLE_NAME, Extra_privilege_by_type.FieldName.CURRI_ID,
            Extra_privilege_by_type.FieldName.TITLE_CODE, Extra_privilege_by_type.FieldName.TITLE_PRIVILEGE_CODE,
            Title.FieldName.NAME, Title_privilege.FieldName.PRIVILEGE,
            /*12*/ User_curriculum.FieldName.TABLE_NAME, oTitle_privilege.getSelectTitlePrivilegeCommand(),
            User_curriculum.FieldName.CURRI_ID, curri_id, User_curriculum.FieldName.USER_ID,
            User_list.FieldName.USER_TYPE, Extra_privilege_by_type.FieldName.USER_TYPE,
            Title.FieldName.TITLE_CODE, title_code, Title_privilege.FieldName.TITLE_PRIVILEGE_CODE,
            FieldName.TABLE_NAME, FieldName.PERSONNEL_ID, FieldName.CURRI_ID,
            FieldName.TITLE_CODE);

            string insertintotemp5_3 = string.Format("insert into {0} " +
            "select {1}.{2}, {3}, {4}, {5}, {6}, {7}.{8},{7}.{9}, {10}, {11},{1}.{17} " +
            "from {1}, {12}, {7},({13}) as tp " +
            "where {12}.{14} = '{15}' " +
            "and {1}.{2} = {12}.{16} " +
            "and {1}.{17} = {7}.{18} " +

            "and {7}.{8} = tp.{19} " +
            "and {7}.{8} = {20} " +
            "and {7}.{9} = tp.{21} " +

            "and not exists (select * from {0} where " +
            "{1}.{2} = {22} " +
            "and {0}.{23} = {7}.{8}) ",
            temp5tablename,/*1*/User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_ID,
            Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME, Teacher.FieldName.FILE_NAME_PIC,
            FieldName.CURRI_ID, /*7*/Default_privilege_by_type.FieldName.TABLE_NAME,
            Default_privilege_by_type.FieldName.TITLE_CODE, Default_privilege_by_type.FieldName.TITLE_PRIVILEGE_CODE,
            Title.FieldName.NAME, Title_privilege.FieldName.PRIVILEGE,
            /*12*/ User_curriculum.FieldName.TABLE_NAME, oTitle_privilege.getSelectTitlePrivilegeCommand(),
            User_curriculum.FieldName.CURRI_ID, curri_id, User_curriculum.FieldName.USER_ID,
            User_list.FieldName.USER_TYPE, Default_privilege_by_type.FieldName.USER_TYPE,
            Title.FieldName.TITLE_CODE, title_code, Title_privilege.FieldName.TITLE_PRIVILEGE_CODE,
            FieldName.PERSONNEL_ID, FieldName.TITLE_CODE);

            string insertintotemp5_4 = string.Format("insert into {0} " +
            "select null, null, null, null, null, {1}, {2}, null, {3},null " +
            "from({4}) as tp " +
            "where {1} = {5} ",
            temp5tablename, FieldName.TITLE_CODE, FieldName.TITLE_PRIVILEGE_CODE, Title_privilege.FieldName.PRIVILEGE,
            oTitle_privilege.getSelectTitlePrivilegeCommand(), title_code);

            string selectcmd = string.Format("select * from {0} order by {1},{2} ",
                temp5tablename, FieldName.PERSONNEL_ID, FieldName.TITLE_CODE);


            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} END", createtabletemp5, insertintotemp5_1,
                insertintotemp5_2, insertintotemp5_3, insertintotemp5_4,selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        //Row which have personnel id => normal data
                        if (item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal].ToString() != "")
                        {
                            string usertype = item.ItemArray[data.Columns[Teacher.FieldName.USER_TYPE].Ordinal].ToString();
                            if (usertype != "อาจารย์")
                                result.list.Add(new Extra_privilege_with_brief_detail
                                {
                                    curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                    name = item.ItemArray[data.Columns[Title.FieldName.NAME].Ordinal].ToString(),
                                    my_privilege = new Title_privilege(Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TITLE_CODE].Ordinal]), Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TITLE_PRIVILEGE_CODE].Ordinal]),
                                    item.ItemArray[data.Columns[Title_privilege.FieldName.PRIVILEGE].Ordinal].ToString()),
                                    file_name_pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[data.Columns[Teacher.FieldName.FILE_NAME_PIC].Ordinal].ToString()),
                                    personnel_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal]),
                                    t_name = item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString() +
                                                 item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                                });
                            else
                                result.list.Add(new Extra_privilege_with_brief_detail
                                {
                                    curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                    name = item.ItemArray[data.Columns[Title.FieldName.NAME].Ordinal].ToString(),
                                    my_privilege = new Title_privilege(Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TITLE_CODE].Ordinal]), Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TITLE_PRIVILEGE_CODE].Ordinal]),
                                    item.ItemArray[data.Columns[Title_privilege.FieldName.PRIVILEGE].Ordinal].ToString()),
                                    file_name_pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[data.Columns[Teacher.FieldName.FILE_NAME_PIC].Ordinal].ToString()),
                                    personnel_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal]),
                                    t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) +
                                             item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                                });
                        }
                        else
                            result.choices.Add(new Title_privilege(Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TITLE_CODE].Ordinal]), Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TITLE_PRIVILEGE_CODE].Ordinal]),
                                    item.ItemArray[data.Columns[Title_privilege.FieldName.PRIVILEGE].Ordinal].ToString()));
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

        public async Task<object> InsertOrUpdate(Extra_privilege_individual_list_with_privilege_choices edata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;

            string InsertOrUpdateCommand = "";
            foreach (Extra_privilege_with_brief_detail e in edata.list)
            {
                InsertOrUpdateCommand += string.Format("IF NOT EXISTS(select * from {0} where {1} = '{2}' and {3} = '{4}' and {5} = {6}) " +
                                         "BEGIN " +
                                         "INSERT INTO {0} values ('{2}','{4}',{6},{7}) " +
                                         "END " +
                                         "ELSE " +
                                         "BEGIN " +
                                         "UPDATE {0} set {8} = {7} where {1} = '{2}' and {3} = '{4}' and {5} = {6} " +
                                         "END ", FieldName.TABLE_NAME, FieldName.PERSONNEL_ID, e.personnel_id, FieldName.CURRI_ID, e.curri_id,
                                         FieldName.TITLE_CODE, e.my_privilege.title_code, e.my_privilege.title_privilege_code, FieldName.TITLE_PRIVILEGE_CODE);
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