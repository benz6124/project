using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oGallery : Gallery
    {
        private string getSelectByCurriculumAcademicCommand(bool isUpdate)
        {
            return string.Format("select {0}.*,{1}.{2},{1}.{15},{3},{4} " +
                "from {0},{1},{5} where {6} = '{7}' and {8} = {9} " +
                "and {0}.{10} = {1}.{11} and {12} = {13} order by {14} desc ",
                FieldName.TABLE_NAME, Picture.FieldName.TABLE_NAME, Picture.FieldName.FILE_NAME,
                Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME, Teacher.FieldName.TABLE_NAME,
                FieldName.CURRI_ID, curri_id, FieldName.ACA_YEAR, aca_year, FieldName.GALLERY_ID,
                Picture.FieldName.GALLERY_ID, Teacher.FieldName.TEACHER_ID, FieldName.PERSONNEL_ID,
                FieldName.DATE_CREATED,Picture.FieldName.CAPTION);
        }

        public object SelectByCurriculumAcademic()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Gallery_detail> result = new List<Gallery_detail>();

            d.iCommand.CommandText = getSelectByCurriculumAcademicCommand(false);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        gallery_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.GALLERY_ID].Ordinal]);
                        //If date is not null (means result is row with attendee data)
                        if (result.FirstOrDefault(g => g.gallery_id == gallery_id) == null)
                        {
                            result.Add(new Gallery_detail
                            {
                                aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                personnel_id = item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal].ToString(),
                                t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString(),
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

        public object Delete(List<Gallery_detail> list)
        {
            DBConnector d = new DBConnector();
            List<string> file_to_delete = new List<string>();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
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
                                      "[{1}] VARCHAR(255) NOT NULL," +
                                      "PRIMARY KEY([row_num])) " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} VARCHAR(255) COLLATE DATABASE_DEFAULT ",
                                      temp1tablename, Picture.FieldName.FILE_NAME);

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
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
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

        public object InsertNewGalleryWithSelect(Gallery_detail gdata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Gallery_detail> result = new List<Gallery_detail>();
            string temp1tablename = "#temp1";
            string temp2tablename = "#temp2";
            string createtabletemp1 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] INT NOT NULL," +
                                      "PRIMARY KEY ([row_num])) ", temp1tablename, FieldName.GALLERY_ID);

            string createtabletemp2 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] VARCHAR(255) NULL," +
                                      "[{2}] VARCHAR(MAX) NULL," +
                                      "PRIMARY KEY ([row_num])) " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} VARCHAR(255) COLLATE DATABASE_DEFAULT " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {2} VARCHAR(MAX) COLLATE DATABASE_DEFAULT "
                                      , temp2tablename, Picture.FieldName.FILE_NAME,Picture.FieldName.CAPTION);


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
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        gallery_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.GALLERY_ID].Ordinal]);
                        //If date is not null (means result is row with attendee data)
                        if (result.FirstOrDefault(g => g.gallery_id == gallery_id) == null)
                        {
                            result.Add(new Gallery_detail
                            {
                                aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                personnel_id = item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal].ToString(),
                                t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString(),
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                gallery_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.GALLERY_ID].Ordinal]),
                                date_created = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.DATE_CREATED].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                                name = item.ItemArray[data.Columns[FieldName.NAME].Ordinal].ToString()
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
    }
}