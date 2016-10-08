using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oGallery : Gallery
    {
        private string getSelectByCurriculumAcademicCommand(bool isUpdate)
        {
            if (!isUpdate)
            {
                return string.Format("select {0}.*,{1},{2},{3},{4},{5} " +
                                     "from {0},{6},{7},{8} where {9} = '{10}' and {11} = {12} " +
                                     "and {0}.{13} = {6}.{14} and {15} = {16} " +
                                     "and {7}.{18} = {8}.{19} order by {17} desc ",
                                     FieldName.TABLE_NAME, Picture.FieldName.FILE_NAME, Picture.FieldName.CAPTION,
                                     User_type.FieldName.USER_TYPE_NAME, Personnel.FieldName.T_PRENAME,
                                     Personnel.FieldName.T_NAME,
                                     Picture.FieldName.TABLE_NAME, User_list.FieldName.TABLE_NAME,User_type.FieldName.TABLE_NAME,
                                     FieldName.CURRI_ID, curri_id, FieldName.ACA_YEAR, aca_year,
                                     FieldName.GALLERY_ID, Picture.FieldName.GALLERY_ID,
                                     User_list.FieldName.USER_ID, FieldName.PERSONNEL_ID,
                                     FieldName.DATE_CREATED, User_list.FieldName.USER_TYPE_ID, User_type.FieldName.USER_TYPE_ID);
            }

            string temp1tablename = "#temp1";
            string temp5tablename = "#temp5";
            string createtabletemp5 = string.Format("create table {0}(" +
                                      "[row_num] int identity(1,1) not null," +
                                      "[{1}] INT NULL," +
                                      "[{2}] INT NULL," +
                                      "[{3}] VARCHAR(1000) NULL," +
                                      "[{4}] DATE NULL," +
                                      "[{5}] {12} NULL," +
                                      "[{6}] INT NULL," +
                                      "[{7}] {13} NOT NULL," +
                                      "[{8}] VARCHAR(MAX) NULL," +
                                      "[{9}] VARCHAR(40) NULL," +
                                      "[{10}] varchar(16) null," +
                                      "[{11}] varchar(60) null," +
                                      "PRIMARY KEY ([row_num])) " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{3}] VARCHAR(1000) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{5}] {12} COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{7}] {13} COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{8}] VARCHAR(MAX) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{9}] VARCHAR(40) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{10}] varchar(16) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{11}] varchar(60) COLLATE DATABASE_DEFAULT "
                                      , temp5tablename, FieldName.GALLERY_ID, FieldName.PERSONNEL_ID, FieldName.NAME,
                                      FieldName.DATE_CREATED, FieldName.CURRI_ID, FieldName.ACA_YEAR,
                                      Picture.FieldName.FILE_NAME, Picture.FieldName.CAPTION,
                                      User_type.FieldName.USER_TYPE_NAME,
                                      Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                                      DBFieldDataType.CURRI_ID_TYPE,DBFieldDataType.FILE_NAME_TYPE);

            string insertintotemp5_1 = string.Format("insert into {20} " +
                                     "select {0}.*,{1},{2},{3},{4},{5} " +
                                     "from {0},{6},{7},{8} where {9} = '{10}' and {11} = {12} " +
                                     "and {0}.{13} = {6}.{14} and {15} = {16} " +
                                     "and {7}.{18} = {8}.{19} order by {17} desc ",
                                     FieldName.TABLE_NAME, Picture.FieldName.FILE_NAME, Picture.FieldName.CAPTION,
                                     User_type.FieldName.USER_TYPE_NAME, Personnel.FieldName.T_PRENAME,
                                     Personnel.FieldName.T_NAME,
                                     Picture.FieldName.TABLE_NAME, User_list.FieldName.TABLE_NAME, User_type.FieldName.TABLE_NAME,
                                     FieldName.CURRI_ID, curri_id, FieldName.ACA_YEAR, aca_year,
                                     FieldName.GALLERY_ID, Picture.FieldName.GALLERY_ID,
                                     User_list.FieldName.USER_ID, FieldName.PERSONNEL_ID,
                                     FieldName.DATE_CREATED, User_list.FieldName.USER_TYPE_ID, User_type.FieldName.USER_TYPE_ID,
                                     temp5tablename);

            string insertintotemp5_2 = string.Format("insert into {0} select 0, null, null,null, null, null,{1}, null,null,null, null " +
                                       "from {2} ", temp5tablename, Picture.FieldName.FILE_NAME, temp1tablename);

            string selectcmd = string.Format("select * from {0} ", temp5tablename);
            return string.Format("BEGIN {0} {1} {2} {3} END ", createtabletemp5, insertintotemp5_1, insertintotemp5_2, selectcmd);
        }

        public async Task<object> SelectByCurriculumAcademic()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<Gallery_detail> result = new List<Gallery_detail>();

            d.iCommand.CommandText = getSelectByCurriculumAcademicCommand(false);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        gallery_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.GALLERY_ID].Ordinal]);
                        if (result.FirstOrDefault(g => g.gallery_id == gallery_id) == null)
                        {
                            string real_t_prename;
                            if (item.ItemArray[data.Columns[User_type.FieldName.USER_TYPE_NAME].Ordinal].ToString() == "อาจารย์")
                                real_t_prename = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString());
                            else
                                real_t_prename = item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString();
                            result.Add(new Gallery_detail
                            {
                                aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                personnel_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal]),
                                t_name = real_t_prename + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString(),
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                gallery_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.GALLERY_ID].Ordinal]),
                                date_created = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.DATE_CREATED].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                                name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                                pictures = new List<Picture>()
                            });
                        }
                        result.First(g => g.gallery_id == gallery_id).pictures.Add(new Picture
                        {
                            gallery_id = Convert.ToInt32(item.ItemArray[data.Columns[Picture.FieldName.GALLERY_ID].Ordinal]),
                            file_name = item.ItemArray[data.Columns[Picture.FieldName.FILE_NAME].Ordinal].ToString(),
                            caption = item.ItemArray[data.Columns[Picture.FieldName.CAPTION].Ordinal].ToString()
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

        public async Task<object> Delete(List<Gallery_detail> list)
        {
            DBConnector d = new DBConnector();
            List<string> file_to_delete = new List<string>();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            string deleteprecmd = string.Format("DELETE FROM {0} WHERE {1} = '{2}' and {3} = {4} ",
                FieldName.TABLE_NAME, FieldName.CURRI_ID, list.First().curri_id, FieldName.ACA_YEAR, list.First().aca_year);
            string excludecond = "1=1 ";
            foreach (Gallery_detail item in list)
            {
                excludecond += string.Format("and {0} != {1} ", FieldName.GALLERY_ID, item.gallery_id);
            }

            string deletefullcmd = string.Format("{0} and ({1})", deleteprecmd, excludecond);

            string temp1tablename = "#temp1";
            string createtabletemp1 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] {2} NOT NULL," +
                                      "PRIMARY KEY([row_num])) " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} {2} COLLATE DATABASE_DEFAULT ",
                                      temp1tablename, Picture.FieldName.FILE_NAME,DBFieldDataType.FILE_NAME_TYPE);

            string insertintotemp1 = string.Format("INSERT INTO {0} " +
                                       "SELECT {1} from {2} where ({3}) " +
                                       "and {4} in " +
                                       "(select {5} from {6} where {7} = '{8}' and {9} = {10}) ",
                                       temp1tablename, Picture.FieldName.FILE_NAME, Picture.FieldName.TABLE_NAME,
                                       excludecond, Picture.FieldName.GALLERY_ID, FieldName.GALLERY_ID,
                                       FieldName.TABLE_NAME, FieldName.CURRI_ID, list.First().curri_id,
                                       FieldName.ACA_YEAR, list.First().aca_year);

            string selcmd = string.Format("select {0} from {1} ", Picture.FieldName.FILE_NAME, temp1tablename);
            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} END", createtabletemp1, insertintotemp1,
                deletefullcmd,selcmd);

            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        file_to_delete.Add(
                           item.ItemArray[data.Columns[Picture.FieldName.FILE_NAME].Ordinal].ToString()
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
            return file_to_delete;
        }

        public async Task<object> InsertNewGalleryWithSelect(Gallery_detail gdata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<Gallery_detail> result = new List<Gallery_detail>();
            string temp1tablename = "#temp1";
            string temp2tablename = "#temp2";
            string createtabletemp1 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] INT NOT NULL," +
                                      "PRIMARY KEY ([row_num])) ", temp1tablename, FieldName.GALLERY_ID);

            string createtabletemp2 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] {3} NULL," +
                                      "[{2}] VARCHAR(MAX) NULL," +
                                      "PRIMARY KEY ([row_num])) " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} {3} COLLATE DATABASE_DEFAULT " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {2} VARCHAR(MAX) COLLATE DATABASE_DEFAULT "
                                      , temp2tablename, Picture.FieldName.FILE_NAME,Picture.FieldName.CAPTION,
                                      DBFieldDataType.FILE_NAME_TYPE);


            string insertintotemp1 = string.Format("INSERT INTO {0} " +
                                     "select * from " +
                                     "(insert into {1}({2},{3},{4},{5},{6}) output inserted.{7} values " +
                                     "('{8}','{9}','{10}','{11}',{12})) " +
                                     "as outputinsert ",
                                     temp1tablename, FieldName.TABLE_NAME, FieldName.PERSONNEL_ID, FieldName.NAME, FieldName.DATE_CREATED,
                                     FieldName.CURRI_ID, FieldName.ACA_YEAR, FieldName.GALLERY_ID,
                                     gdata.personnel_id, gdata.name, gdata.date_created, gdata.curri_id, gdata.aca_year);


            string insertintotemp2 = string.Format("INSERT INTO {0} VALUES (null,null)", temp2tablename);

            foreach (Picture p in gdata.pictures)
            {
                insertintotemp2 += string.Format(",('{0}','{1}')", p.file_name,p.caption);
            }

            string insertintopicture = string.Format(" INSERT INTO {0} " +
                                        "select {1},{2},{3} from {4},{5} where {2} is not null ",
                                        Picture.FieldName.TABLE_NAME, FieldName.GALLERY_ID, Picture.FieldName.FILE_NAME,
                                        Picture.FieldName.CAPTION,
                                        temp1tablename, temp2tablename);

            curri_id = gdata.curri_id;
            aca_year = gdata.aca_year;

            string selectcmd = getSelectByCurriculumAcademicCommand(false);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} END", createtabletemp1, createtabletemp2,
                insertintotemp1, insertintotemp2,
                insertintopicture, selectcmd);

            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        gallery_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.GALLERY_ID].Ordinal]);
                        if (result.FirstOrDefault(g => g.gallery_id == gallery_id) == null)
                        {
                            string real_t_prename;
                            if (item.ItemArray[data.Columns[User_type.FieldName.USER_TYPE_NAME].Ordinal].ToString() == "อาจารย์")
                                real_t_prename = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString());
                            else
                                real_t_prename = item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString();
                            result.Add(new Gallery_detail
                            {
                                aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                personnel_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal]),
                                t_name = real_t_prename + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString(),
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                gallery_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.GALLERY_ID].Ordinal]),
                                date_created = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.DATE_CREATED].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                                name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                                pictures = new List<Picture>()
                            });
                        }
                        result.First(g => g.gallery_id == gallery_id).pictures.Add(new Picture
                        {
                            gallery_id = Convert.ToInt32(item.ItemArray[data.Columns[Picture.FieldName.GALLERY_ID].Ordinal]),
                            file_name = item.ItemArray[data.Columns[Picture.FieldName.FILE_NAME].Ordinal].ToString(),
                            caption = item.ItemArray[data.Columns[Picture.FieldName.CAPTION].Ordinal].ToString()
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

        public async Task<object> UpdateGalleryWithSelect(Gallery_detail gdata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<Gallery_detail> result = new List<Gallery_detail>();
            Gallery_detail dummyfordeletepictures = new Gallery_detail();
            string temp1tablename = "#temp1";
            string updatepicturecmd = "";
            string ifexistscond = string.Format("if exists (select * from {0} where {1} = {2}) ", FieldName.TABLE_NAME,
                FieldName.GALLERY_ID, gdata.gallery_id);
            string insertintopicturecmd = string.Format("insert into {0} values ", Picture.FieldName.TABLE_NAME);
            string deletefrompicturecmd = string.Format("delete from {0} output deleted.{1} where {2} = {3} ",
                                          Picture.FieldName.TABLE_NAME, Picture.FieldName.FILE_NAME, Picture.FieldName.GALLERY_ID,
                                          gdata.gallery_id);
            string excludecond = "1=1 ";
            int len = insertintopicturecmd.Length;
            string createtabletemp1 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] {2} NULL," +
                                      "PRIMARY KEY ([row_num])) " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} {2} COLLATE DATABASE_DEFAULT "
                                      , temp1tablename, Picture.FieldName.FILE_NAME,
                                      DBFieldDataType.FILE_NAME_TYPE);

            string updategallerycmd = string.Format("update {0} set {1} = '{2}', {3} = '{4}' where {5} = {6} ",
                FieldName.TABLE_NAME, FieldName.PERSONNEL_ID, gdata.personnel_id, FieldName.NAME, gdata.name,
                FieldName.GALLERY_ID, gdata.gallery_id);

            foreach (Picture p in gdata.pictures)
            {
                if (p.gallery_id != 0)
                {
                    updatepicturecmd += string.Format("update {0} set {1} = '{2}' where {3} = {4} and {5} = '{6}' ",
                        Picture.FieldName.TABLE_NAME, Picture.FieldName.CAPTION, p.caption, Picture.FieldName.GALLERY_ID,
                        gdata.gallery_id, Picture.FieldName.FILE_NAME, p.file_name);
                    //Gen delete cond
                    excludecond += string.Format("and {0} != '{1}' ", Picture.FieldName.FILE_NAME, p.file_name);
                }
                else
                {
                    if (insertintopicturecmd.Length <= len)
                        insertintopicturecmd += string.Format("({0},'{1}','{2}')", gdata.gallery_id, p.file_name, p.caption);
                    else
                        insertintopicturecmd += string.Format(",({0},'{1}','{2}')", gdata.gallery_id, p.file_name, p.caption);
                }
            }
            if (insertintopicturecmd.Length <= len)
                insertintopicturecmd = "";

            deletefrompicturecmd += string.Format("and ({0}) ", excludecond);

            string insertintotemp1 = string.Format("INSERT INTO {0} " +
                                     "select * from " +
                                     "({1}) " +
                                     "as outputdelete ",
                                     temp1tablename, deletefrompicturecmd);



            curri_id = gdata.curri_id;
            aca_year = gdata.aca_year;

            string selectcmd = getSelectByCurriculumAcademicCommand(true);

            d.iCommand.CommandText = string.Format("{0} BEGIN {1} {2} {3} {4} {5} {6} END",ifexistscond, createtabletemp1,updategallerycmd,updatepicturecmd,
                insertintotemp1,insertintopicturecmd,selectcmd);

            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        gallery_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.GALLERY_ID].Ordinal]);
                        if (gallery_id != 0)
                        {
                            //If date is not null (means result is row with picture data)
                            if (result.FirstOrDefault(g => g.gallery_id == gallery_id) == null)
                            {
                                string real_t_prename;
                                if (item.ItemArray[data.Columns[User_type.FieldName.USER_TYPE_NAME].Ordinal].ToString() == "อาจารย์")
                                    real_t_prename = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString());
                                else
                                    real_t_prename = item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString();
                                result.Add(new Gallery_detail
                                {
                                    aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                    personnel_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal]),
                                    t_name = real_t_prename + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString(),
                                    curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                    gallery_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.GALLERY_ID].Ordinal]),
                                    date_created = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.DATE_CREATED].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                                    name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString(),
                                    pictures = new List<Picture>()
                                });
                            }
                            result.First(g => g.gallery_id == gallery_id).pictures.Add(new Picture
                            {
                                gallery_id = Convert.ToInt32(item.ItemArray[data.Columns[Picture.FieldName.GALLERY_ID].Ordinal]),
                                file_name = item.ItemArray[data.Columns[Picture.FieldName.FILE_NAME].Ordinal].ToString(),
                                caption = item.ItemArray[data.Columns[Picture.FieldName.CAPTION].Ordinal].ToString()
                            });
                        }
                        else
                        {
                            dummyfordeletepictures.pictures.Add(new Picture
                            {
                                file_name = item.ItemArray[data.Columns[Picture.FieldName.FILE_NAME].Ordinal].ToString()
                            });
                        }
                    }
                    data.Dispose();
                }
                else
                {
                    //Reserved for return error string
                    res.Close();
                    return "ไม่พบข้อมูลอัลบั้มรูปภาพที่ต้องการแก้ไขในระบบ";
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
            result.Add(dummyfordeletepictures);
            return result;
        }
    }
}