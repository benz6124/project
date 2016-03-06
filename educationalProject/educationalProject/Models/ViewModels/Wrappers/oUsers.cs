using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using educationalProject.Models.Wrappers;
using educationalProject.Utils;
namespace educationalProject.Models.ViewModels.Wrappers
{
    public class oUsers : User
    {
        private static readonly PasswordHasher hasher = new PasswordHasher();

        private string getSelectPrivilegeCommand(string user_id,string tablename)
        {
            string temp5tablename = tablename;

            string createtabletemp5 = string.Format("CREATE TABLE {0}(" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[privilege_type] INT NULL," +
                                      "[{1}] INT NULL," +
                                      "[{2}] {5} NULL," +
                                      "[{3}] INT NULL," +
                                      "[{4}] INT null," +
                                      "PRIMARY KEY([row_num])) " +

                                      "alter table {0} " +
                                      "alter column {2} {5} collate database_default ",
                                      temp5tablename, Extra_privilege.FieldName.PERSONNEL_ID,
                                      Extra_privilege.FieldName.CURRI_ID,
                                      Extra_privilege.FieldName.TITLE_CODE,
                                      Extra_privilege.FieldName.TITLE_PRIVILEGE_CODE,
                                      DBFieldDataType.CURRI_ID_TYPE);

            string select_user_type_subquery = string.Format("select {0} from {1} where {2} = {3}",
            User_list.FieldName.USER_TYPE,User_list.FieldName.TABLE_NAME,User_list.FieldName.USER_ID,user_id);

            string insertintotemp5_1 = string.Format("insert into {0} " +
                                       "select 1,* from {1} where {2} = {3} ",
                                       temp5tablename, Extra_privilege.FieldName.TABLE_NAME,
                                       Extra_privilege.FieldName.PERSONNEL_ID, user_id);

            string insertintotemp5_2 = string.Format("insert into {0} " +
                                       "select 1,{1}, {2}, {3}, {4} from {5} where {6} IN ({7}) " +
                                       "and {2} in (select {8} from {9} where {10} = {1}) " +
                                       "and not exists(select * from {11} where {12} = {1} " +
                                       "and {11}.{13} = {5}.{2} and {11}.{14} = {5}.{3}) ",
                                       temp5tablename, user_id, Extra_privilege_by_type.FieldName.CURRI_ID,
                                       Extra_privilege_by_type.FieldName.TITLE_CODE, Extra_privilege_by_type.FieldName.TITLE_PRIVILEGE_CODE,
                                       Extra_privilege_by_type.FieldName.TABLE_NAME, Extra_privilege_by_type.FieldName.USER_TYPE,
                                       select_user_type_subquery, User_curriculum.FieldName.CURRI_ID, User_curriculum.FieldName.TABLE_NAME,
                                       User_curriculum.FieldName.USER_ID, Extra_privilege.FieldName.TABLE_NAME,
                                       Extra_privilege.FieldName.PERSONNEL_ID,
                                       Extra_privilege.FieldName.CURRI_ID,
                                       Extra_privilege.FieldName.TITLE_CODE);

            string insertintotemp5_3 = string.Format("insert into {0} " +
                                       "select 1,{1},{2},{3},{4} from {5}," +
                                       "(select {2} from {6} where {7} = {1}) as usrcurri " +
                                       "where {8} IN ({9}) " +
                                       "and not exists(select * from {0} where " +
                                       "{0}.{10} = usrcurri.{2} and {0}.{11} = {5}.{3}) ",
                                       temp5tablename, user_id, User_curriculum.FieldName.CURRI_ID, Default_privilege_by_type.FieldName.TITLE_CODE,
                                       Default_privilege_by_type.FieldName.TITLE_PRIVILEGE_CODE, Default_privilege_by_type.FieldName.TABLE_NAME,
                                       User_curriculum.FieldName.TABLE_NAME, User_curriculum.FieldName.USER_ID,
                                       Default_privilege_by_type.FieldName.USER_TYPE, select_user_type_subquery,
                                       Extra_privilege.FieldName.CURRI_ID,
                                       Extra_privilege.FieldName.TITLE_CODE);
            string insertintotemp5_4 = string.Format("insert into {0} " +
                                       "select 2, {1}, {2}, {3}, {4} " +
                                       "from {5} where {6} = 'กรรมการหลักสูตร' " +
                                       "and {2} in (select distinct {7} from {8} where {9} = {1}) ",
                                       temp5tablename, user_id, Extra_privilege_by_type.FieldName.CURRI_ID,
                                       Extra_privilege_by_type.FieldName.TITLE_CODE, Extra_privilege_by_type.FieldName.TITLE_PRIVILEGE_CODE,
                                       Extra_privilege_by_type.FieldName.TABLE_NAME,
                                       Extra_privilege_by_type.FieldName.USER_TYPE,
                                       Committee.FieldName.CURRI_ID, Committee.FieldName.TABLE_NAME, Committee.FieldName.TEACHER_ID);

            string insertintotemp5_5 = string.Format("insert into {0} " +
                                       "select 2, {1}, {2}, {3}, {4} " +
                                       "from (select distinct {2} from {5} where {6} = {1}) as committeeres, {7} " +
                                       "where {8} = 'กรรมการหลักสูตร' " +
                                       "and not exists(select * from {9} " +
                                       "where {10} = 'กรรมการหลักสูตร' and {9}.{11} = committeeres.{2} " +
                                       "and {9}.{12} = {7}.{13}) ",
                                       temp5tablename, user_id, Committee.FieldName.CURRI_ID,
                                       Default_privilege_by_type.FieldName.TITLE_CODE,
                                       Default_privilege_by_type.FieldName.TITLE_PRIVILEGE_CODE,
                                       Committee.FieldName.TABLE_NAME, Committee.FieldName.TEACHER_ID,
                                       Default_privilege_by_type.FieldName.TABLE_NAME,
                                       Default_privilege_by_type.FieldName.USER_TYPE,
                                       Extra_privilege_by_type.FieldName.TABLE_NAME,
                                       Extra_privilege_by_type.FieldName.USER_TYPE,
                                       Extra_privilege_by_type.FieldName.CURRI_ID,
                                       Extra_privilege_by_type.FieldName.TITLE_CODE,
                                       Default_privilege_by_type.FieldName.TITLE_CODE);

            string selectcmd = string.Format("select * from {0} order by {1},{2} ", temp5tablename, Extra_privilege.FieldName.CURRI_ID, Extra_privilege.FieldName.TITLE_CODE);

            return string.Format("BEGIN {0} {1} {2} {3} {4} {5} {6} END", createtabletemp5, insertintotemp5_1,
                insertintotemp5_2, insertintotemp5_3, insertintotemp5_4, insertintotemp5_5, selectcmd);
        }

        private string getSelectUserDataCommand(string user_id,int usrtype,string tablename)
        {
            //0 stand for teacher
            //1 stand for staff
            //2 stand for student
            //3 stand for alumni
            //4 stand for company
            //5 stand for assessor
            //6 stand for admin


            string mainusrdataselect = "";
            if (usrtype == 0)
                mainusrdataselect = string.Format("select * from ({0}) as tres where {1} = {2} ",
                oTeacher.getSelectTeacherByJoinCommand(), Teacher.FieldName.TEACHER_ID, user_id);
            else if (usrtype == 1)
                mainusrdataselect = string.Format("select * from ({0}) as sres where {1} = {2} ",
                oStaff.getSelectStaffByJoinCommand(), Staff.FieldName.STAFF_ID, user_id);
            else if (usrtype == 2)
                mainusrdataselect = string.Format("select * from ({0}) as sres where {1} = {2} ",
                oStudent.getSelectStudentByJoinCommand(), Student.FieldName.USER_ID, user_id);
            else if (usrtype == 3)
                mainusrdataselect = string.Format("select * from ({0}) as sres where {1} = {2} ",
                oAlumni.getSelectAlumniByJoinCommand(), Alumni.FieldName.USER_ID, user_id);
            else if (usrtype == 4)
                mainusrdataselect = string.Format("select * from ({0}) as sres where {1} = {2} ",
                oCompany.getSelectCompanyByJoinCommand(), Company.FieldName.COMPANY_ID, user_id);
            else if (usrtype == 5)
                mainusrdataselect = string.Format("select * from ({0}) as sres where {1} = {2} ",
                oAssessor.getSelectAssessorByJoinCommand(), Assessor.FieldName.ASSESSOR_ID, user_id);
            else if (usrtype == 6)
                mainusrdataselect = string.Format("select * from ({0}) as sres where {1} = {2} ",
                oAdmin.getSelectAdminByJoinCommand(), Admin.FieldName.ADMIN_ID, user_id);

            //1 select user_data from pre-defined select table command => mainusrdataselect

            //2 select education data(teacher and staff only) => selecteducation
            string selecteducation = "";
            if (usrtype != 2)
                selecteducation = string.Format("select * from {0} where {1} = {2} ",
                    Educational_teacher_staff.FieldName.TABLE_NAME, Educational_teacher_staff.FieldName.PERSONNEL_ID,
                    user_id);

            //3 select user_curri_id which user is in => selectcurri
            string selectcurri = string.Format("select {0} as user_curri_id from {1} where {2} = {3} ",
                User_curriculum.FieldName.CURRI_ID, User_curriculum.FieldName.TABLE_NAME,
                User_curriculum.FieldName.USER_ID, user_id);

            //4 select president_in (pres_curri_id, aca_year) when user is? (teacher only) => selectpresin
            string selectpresin = "";
            if (usrtype == 0)
                selectpresin = string.Format("select {0} as pres_curri_id,{1} from {2} where {3} = {4} ",
                President_curriculum.FieldName.CURRI_ID, President_curriculum.FieldName.ACA_YEAR,
                President_curriculum.FieldName.TABLE_NAME, President_curriculum.FieldName.TEACHER_ID, user_id);

            //5 select committee_in (comm_curri_id, aca_year) when user is? (teacher only) => selectcommitteein
            string selectcommitteein = "";
            if (usrtype == 0)
                selectcommitteein = string.Format("select {0} as comm_curri_id,{1} from {2} where {3} = {4} ",
                Committee.FieldName.CURRI_ID, Committee.FieldName.ACA_YEAR, Committee.FieldName.TABLE_NAME,
                Committee.FieldName.TEACHER_ID, user_id);

            //6 select topic_interested (teacher only) => selecttopic
            string selecttopic = "";
            if (usrtype == 0)
                selecttopic = string.Format("select {0} from {1} where {2} = {3} ",
                    Technical_interested.FieldName.TOPIC_INTERESTED, Technical_interested.FieldName.TABLE_NAME,
                    Technical_interested.FieldName.TEACHER_ID, user_id);

            //7 select not_send_primary (teacher only ? evid_curri_id,curr_tname, aca_year, evidence_name) => selectnotsendprimary
            string selectnotsendprimary = "";
            if (usrtype == 0)
                selectnotsendprimary = string.Format("select {0}.{1} as evid_curri_id,{2},{3},{4} from {0},{5},{6} " +
                "where {5}.{7} = {0}.{8} " +
                "and {9} = {10} and ({11} = '0' or {11} = '4') " +
                "and {6}.{12} = {0}.{1} ",
                Primary_evidence_status.FieldName.TABLE_NAME, Primary_evidence_status.FieldName.CURRI_ID,
                Cu_curriculum.FieldName.CURR_TNAME,
                Primary_evidence.FieldName.ACA_YEAR, Primary_evidence.FieldName.EVIDENCE_NAME,
                /*5*/Primary_evidence.FieldName.TABLE_NAME,Cu_curriculum.FieldName.TABLE_NAME,
                Primary_evidence.FieldName.PRIMARY_EVIDENCE_NUM, Primary_evidence_status.FieldName.PRIMARY_EVIDENCE_NUM,
                Primary_evidence_status.FieldName.TEACHER_ID, user_id,Primary_evidence_status.FieldName.STATUS,
                Cu_curriculum.FieldName.CURRI_ID
                );


            //8 select privilege (use predefined select from temp table cmd) except admin => selectprivilege
            string selectprivilege = "";
            if (usrtype != 6)
                selectprivilege = getSelectPrivilegeCommand(user_id,tablename);



            return string.Format(" BEGIN {0} {1} {2} {3} {4} {5} {6} {7} END ", mainusrdataselect, selecteducation, selectcurri, selectpresin,
                   selectcommitteein, selecttopic, selectnotsendprimary, selectprivilege);
        }
        public async Task<object> SelectUser(string preferredusername)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            User_information_with_privilege_information result = new User_information_with_privilege_information();

            username = preferredusername;
            string usridvar = "@usrid";
            d.iCommand.CommandText = string.Format("declare {2} int = -1; " +

            "SET {2} = (select {22} from {23} where {24} = {25}) " +

           "if exists (select * from {0} where {1} = {2}) " +
           "{3} " +

           "else if exists (select * from {4} where {5} = {2}) " +
           "{6} " +

           "else if exists (select * from {7} where {8} = {2}) " +
           "{9} " +

           "else if exists (select * from {10} where {11} = {2}) " +
           "{12} " +

           "else if exists (select * from {13} where {14} = {2}) " +
           "{15} " +

           "else if exists (select * from {16} where {17} = {2}) " +
           "{18} " +

           "else if exists (select * from {19} where {20} = {2}) " +
           "{21} ",
           Teacher.FieldName.TABLE_NAME, Teacher.FieldName.TEACHER_ID, usridvar,
           getSelectUserDataCommand(usridvar, 0, "#temp99"), Staff.FieldName.TABLE_NAME, Staff.FieldName.STAFF_ID,
           getSelectUserDataCommand(usridvar, 1, "#temp98"), Student.FieldName.TABLE_NAME, Student.FieldName.USER_ID,
           getSelectUserDataCommand(usridvar, 2, "#temp97"), Alumni.ExtraFieldName.TABLE_NAME, Alumni.FieldName.USER_ID,
           getSelectUserDataCommand(usridvar, 3, "#temp96"), Company.FieldName.TABLE_NAME, Company.FieldName.COMPANY_ID,
           getSelectUserDataCommand(usridvar, 4, "#temp95"), Assessor.FieldName.TABLE_NAME, Assessor.FieldName.ASSESSOR_ID,
           getSelectUserDataCommand(usridvar, 5, "#temp94"), Admin.FieldName.TABLE_NAME, Admin.FieldName.ADMIN_ID,
           getSelectUserDataCommand(usridvar, 6, "#temp93"),User_list.FieldName.USER_ID,User_list.FieldName.TABLE_NAME,
           Teacher.FieldName.USERNAME, Teacher.ParameterName.USERNAME);
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(Teacher.ParameterName.USERNAME, username));
            result.user_id = 0;

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
                            if (tabledata.Columns.Contains(Teacher.FieldName.T_PRENAME))
                            {
                                //1 retrieve user_data from pre-defined select table command
                                string usrtype = item.ItemArray[tabledata.Columns[Teacher.FieldName.USER_TYPE].Ordinal].ToString();

                                if (usrtype == "อาจารย์")
                                    result.user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Teacher.FieldName.TEACHER_ID].Ordinal]);
                                else if (usrtype == "เจ้าหน้าที่")
                                    result.user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Staff.FieldName.STAFF_ID].Ordinal]);
                                else if (usrtype == "นักศึกษา" || usrtype == "ศิษย์เก่า")
                                    result.user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Student.FieldName.USER_ID].Ordinal]);
                                else if (usrtype == "บริษัท")
                                    result.user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Company.FieldName.COMPANY_ID].Ordinal]);
                                else if (usrtype == "ผู้ประเมินจากภายนอก")
                                    result.user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Assessor.FieldName.ASSESSOR_ID].Ordinal]);
                                else
                                    result.user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Admin.FieldName.ADMIN_ID].Ordinal]);

                                result.username = item.ItemArray[tabledata.Columns[Teacher.FieldName.USERNAME].Ordinal].ToString();
                                result.user_type = usrtype;
                                //**********************************************

                                result.information.addr = item.ItemArray[tabledata.Columns[Teacher.FieldName.ADDR].Ordinal].ToString();
                                result.information.citizen_id = item.ItemArray[tabledata.Columns[Teacher.FieldName.CITIZEN_ID].Ordinal].ToString();
                                result.information.email = item.ItemArray[tabledata.Columns[Teacher.FieldName.EMAIL].Ordinal].ToString();
                                result.information.tel = item.ItemArray[tabledata.Columns[Teacher.FieldName.TEL].Ordinal].ToString();
                                result.information.gender = item.ItemArray[tabledata.Columns[Teacher.FieldName.GENDER].Ordinal].ToString() != "" ? Convert.ToChar(item.ItemArray[tabledata.Columns[Teacher.FieldName.GENDER].Ordinal]) : ' ';
                                result.information.file_name_pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[tabledata.Columns[Teacher.FieldName.FILE_NAME_PIC].Ordinal].ToString());
                                result.information.timestamp = item.ItemArray[tabledata.Columns[Teacher.FieldName.TIMESTAMP].Ordinal].ToString();
                                result.information.e_name = item.ItemArray[tabledata.Columns[Teacher.FieldName.E_NAME].Ordinal].ToString();
                                result.information.e_prename = item.ItemArray[tabledata.Columns[Teacher.FieldName.E_PRENAME].Ordinal].ToString();
                                result.information.t_name = item.ItemArray[tabledata.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString();
                                result.information.t_prename = item.ItemArray[tabledata.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString();
                                result.information.SetPassword(item.ItemArray[tabledata.Columns[Teacher.FieldName.PASSWORD].Ordinal].ToString());

                                if (usrtype == "อาจารย์")
                                {
                                    result.information.degree = item.ItemArray[tabledata.Columns[Teacher.FieldName.DEGREE].Ordinal].ToString() != "" ? Convert.ToChar(item.ItemArray[tabledata.Columns[Teacher.FieldName.DEGREE].Ordinal]) : ' ';
                                    result.information.position = item.ItemArray[tabledata.Columns[Teacher.FieldName.POSITION].Ordinal].ToString() != "" ? Convert.ToChar(item.ItemArray[tabledata.Columns[Teacher.FieldName.POSITION].Ordinal]) : ' ';
                                    result.information.personnel_type = item.ItemArray[tabledata.Columns[Teacher.FieldName.PERSONNEL_TYPE].Ordinal].ToString();
                                    result.information.person_id = item.ItemArray[tabledata.Columns[Teacher.FieldName.PERSON_ID].Ordinal].ToString();
                                    result.information.room = item.ItemArray[tabledata.Columns[Teacher.FieldName.ROOM].Ordinal].ToString();
                                    result.information.status = item.ItemArray[tabledata.Columns[Teacher.FieldName.STATUS].Ordinal].ToString();
                                    result.information.alive = item.ItemArray[tabledata.Columns[Teacher.FieldName.POSITION].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[tabledata.Columns[Teacher.FieldName.ALIVE].Ordinal]) : -1;
                                }
                                else if (usrtype == "เจ้าหน้าที่")
                                {
                                    result.information.room = item.ItemArray[tabledata.Columns[Staff.FieldName.ROOM].Ordinal].ToString();
                                }
                                else if (usrtype == "บริษัท")
                                {
                                    result.information.company_name = item.ItemArray[tabledata.Columns[Company.FieldName.COMPANY_NAME].Ordinal].ToString();
                                }
                                else if (usrtype == "ผู้ประเมินจากภายนอก")
                                {

                                }
                                else if (usrtype == "นักศึกษา")
                                {

                                }
                                else if (usrtype == "ศิษย์เก่า")
                                {

                                }
                            }
                            else if (tabledata.Columns.Contains(Educational_teacher_staff.FieldName.COLLEGE))
                            {
                                //2 retrieve education data(all user type except student)
                                result.information.education.Add(new Educational_teacher_staff
                                {
                                    college = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.COLLEGE].Ordinal].ToString(),
                                    degree = Convert.ToChar(item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.DEGREE].Ordinal].ToString()),
                                    grad_year = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.GRAD_YEAR].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.GRAD_YEAR].Ordinal]) : 0,
                                    major = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.MAJOR].Ordinal].ToString(),
                                    personnel_id = result.user_id,
                                    education_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.EDUCATION_ID].Ordinal]),
                                    pre_major = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.PRE_MAJOR].Ordinal].ToString()
                                });
                            }
                            else if (tabledata.Columns.Contains("user_curri_id"))
                            {
                                //3 retrieve user_curri_id which user is in
                                result.curri_id_in.Add(item.ItemArray[tabledata.Columns["user_curri_id"].Ordinal].ToString());
                            }
                            else if (tabledata.Columns.Contains("pres_curri_id"))
                            {
                                //4 retrieve president_in (pres_curri_id, aca_year) when user is? (teacher only)
                                if (result.president_in == null)
                                    result.president_in = new Dictionary<string, List<int>>();
                                string curri_id = item.ItemArray[tabledata.Columns["pres_curri_id"].Ordinal].ToString();
                                if (!result.president_in.ContainsKey(curri_id))
                                {
                                    result.president_in.Add(curri_id, new List<int>());
                                }
                                result.president_in[curri_id].Add(Convert.ToInt32(item.ItemArray[tabledata.Columns[President_curriculum.FieldName.ACA_YEAR].Ordinal]));
                            }
                            else if (tabledata.Columns.Contains("comm_curri_id"))
                            {
                                //5 retrieve committee_in (comm_curri_id, aca_year) when user is? (teacher only)
                                if (result.committee_in == null)
                                    result.committee_in = new Dictionary<string, List<int>>();
                                string curri_id = item.ItemArray[tabledata.Columns["comm_curri_id"].Ordinal].ToString();
                                if (!result.committee_in.ContainsKey(curri_id))
                                {
                                    result.committee_in.Add(curri_id, new List<int>());
                                }
                                result.committee_in[curri_id].Add(Convert.ToInt32(item.ItemArray[tabledata.Columns[Committee.FieldName.ACA_YEAR].Ordinal]));
                            }
                            else if (tabledata.Columns.Contains(Technical_interested.FieldName.TOPIC_INTERESTED))
                            {
                                //6 retrieve topic_interested (teacher only)
                                result.information.interest.Add(item.ItemArray[tabledata.Columns[Technical_interested.FieldName.TOPIC_INTERESTED].Ordinal].ToString());
                            }
                            else if (tabledata.Columns.Contains("evid_curri_id"))
                            {
                                //7 retrieve not_send_primary (teacher only ? evid_curri_id,curr_tname, aca_year, evidence_name)
                                if (result.not_send_primary == null)
                                    result.not_send_primary = new List<Evidence_brief_detail>();
                                result.not_send_primary.Add(new Evidence_brief_detail
                                {
                                    curri_id = item.ItemArray[tabledata.Columns["evid_curri_id"].Ordinal].ToString(),
                                    curr_tname = item.ItemArray[tabledata.Columns[Cu_curriculum.FieldName.CURR_TNAME].Ordinal].ToString(),
                                    aca_year = Convert.ToInt32(item.ItemArray[tabledata.Columns[Primary_evidence.FieldName.ACA_YEAR].Ordinal]),
                                    evidence_name = item.ItemArray[tabledata.Columns[Primary_evidence.FieldName.EVIDENCE_NAME].Ordinal].ToString()
                                });
                            }
                            else if (tabledata.Columns.Contains(Extra_privilege.FieldName.TITLE_CODE))
                            {
                                //8 retrieve privilege (use predefined select from temp table cmd)
                                string curri_id = item.ItemArray[tabledata.Columns[User_curriculum.FieldName.CURRI_ID].Ordinal].ToString();

                                if (Convert.ToInt32(item.ItemArray[tabledata.Columns["privilege_type"].Ordinal]) == 1)
                                {
                                    //Add normal privilege
                                    if (!result.privilege.ContainsKey(curri_id))
                                    {

                                        result.privilege.Add(curri_id, new Dictionary<int, int>());
                                    }
                                    result.privilege[curri_id][Convert.ToInt32(item.ItemArray[tabledata.Columns[Extra_privilege.FieldName.TITLE_CODE].Ordinal])] = Convert.ToInt32(item.ItemArray[tabledata.Columns[Extra_privilege.FieldName.TITLE_PRIVILEGE_CODE].Ordinal]);
                                }
                                else
                                {
                                    //Add committee privilege
                                    if (result.committee_privilege == null)
                                        result.committee_privilege = new Dictionary<string, Dictionary<int, int>>();
                                    if (!result.committee_privilege.ContainsKey(curri_id))
                                    {

                                        result.committee_privilege.Add(curri_id, new Dictionary<int, int>());
                                    }
                                    result.committee_privilege[curri_id][Convert.ToInt32(item.ItemArray[tabledata.Columns[Extra_privilege.FieldName.TITLE_CODE].Ordinal])] = Convert.ToInt32(item.ItemArray[tabledata.Columns[Extra_privilege.FieldName.TITLE_PRIVILEGE_CODE].Ordinal]);
                                }
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
            if (result.user_id != 0)
                return result;
            else
                return "ไม่พบชื่อผู้ใช้งานนี้ในระบบ";
        }

        public async Task<object> UpdateUsername(string preferredusername, int user_id)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";

            d.iCommand.CommandText = string.Format("if not exists(select * from {0} where {1} = '{2}') " +
                "update {0} set {1} = '{2}' where {3} = {4}",
                User_list.FieldName.TABLE_NAME,Teacher.FieldName.USERNAME,preferredusername,
                User_list.FieldName.USER_ID,user_id);
            try
            {
                int rowaffected = await d.iCommand.ExecuteNonQueryAsync();
                if (rowaffected > 0)
                    return null;
                else
                    return "Username already exists!";
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

        public object UpdatePassword(string preferoldpassword,ref string newpassword,int user_id)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";

            d.iCommand.CommandText = string.Format("select {0} from {1} where {2} = {3}",
                Teacher.FieldName.PASSWORD,User_list.FieldName.TABLE_NAME,User_list.FieldName.USER_ID,user_id);
            try
            {
                object oldpass = d.iCommand.ExecuteScalar();
                if (oldpass != null)
                {
                    PasswordVerificationResult result = hasher.VerifyHashedPassword(oldpass.ToString(), preferoldpassword);
                    if(result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        newpassword = hasher.HashPassword(newpassword);
                        d.iCommand.CommandText = string.Format("update {0} set {1} = '{2}' where {3} = {4}",
                            User_list.FieldName.TABLE_NAME, Teacher.FieldName.PASSWORD, newpassword, User_list.FieldName.USER_ID, user_id);
                        d.iCommand.ExecuteNonQuery();
                        return null;
                    }
                    else
                    {
                        return "รหัสผ่านเก่าไม่ถูกต้อง";
                    }
                }
                else
                {
                    return "ไม่พบผู้ใช้งานนี้ในระบบ";
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
        }

        public async Task<object> selectUserData(int usrid)
        {
            DBConnector d = new DBConnector();
            User_information_with_privilege_information result = new User_information_with_privilege_information();

            if (!d.SQLConnect())
                return "Cannot connect to database.";

            d.iCommand.CommandText = string.Format("if exists (select * from {0} where {1} = {2}) " +
            "{3} " +

            "else if exists (select * from {4} where {5} = {2}) " +
            "{6} " +

            "else if exists (select * from {7} where {8} = {2}) " +
            "{9} " +

            "else if exists (select * from {10} where {11} = {2}) " +
            "{12} " +

            "else if exists (select * from {13} where {14} = {2}) " +
            "{15} " +

            "else if exists (select * from {16} where {17} = {2}) " +
            "{18} " +

            "else if exists (select * from {19} where {20} = {2}) " +
            "{21} ",
            Teacher.FieldName.TABLE_NAME, Teacher.FieldName.TEACHER_ID, usrid,
            getSelectUserDataCommand(usrid.ToString(), 0,"#temp99"), Staff.FieldName.TABLE_NAME, Staff.FieldName.STAFF_ID,
            getSelectUserDataCommand(usrid.ToString(), 1,"#temp98"), Student.FieldName.TABLE_NAME, Student.FieldName.USER_ID,
            getSelectUserDataCommand(usrid.ToString(), 2,"#temp97"), Alumni.ExtraFieldName.TABLE_NAME, Alumni.FieldName.USER_ID,
            getSelectUserDataCommand(usrid.ToString(), 3,"#temp96"), Company.FieldName.TABLE_NAME, Company.FieldName.COMPANY_ID,
            getSelectUserDataCommand(usrid.ToString(), 4,"#temp95"), Assessor.FieldName.TABLE_NAME, Assessor.FieldName.ASSESSOR_ID,
            getSelectUserDataCommand(usrid.ToString(), 5,"#temp94"), Admin.FieldName.TABLE_NAME, Admin.FieldName.ADMIN_ID,
            getSelectUserDataCommand(usrid.ToString(), 6,"#temp93"));

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
                            if (tabledata.Columns.Contains(Teacher.FieldName.T_PRENAME))
                            {
                                //1 retrieve user_data from pre-defined select table command
                                string usrtype = item.ItemArray[tabledata.Columns[Teacher.FieldName.USER_TYPE].Ordinal].ToString();

                                if (usrtype == "อาจารย์")
                                    result.user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Teacher.FieldName.TEACHER_ID].Ordinal]);
                                else if (usrtype == "เจ้าหน้าที่")
                                    result.user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Staff.FieldName.STAFF_ID].Ordinal]);
                                else if (usrtype == "นักศึกษา" || usrtype == "ศิษย์เก่า")
                                    result.user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Student.FieldName.USER_ID].Ordinal]);
                                else if (usrtype == "บริษัท")
                                    result.user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Company.FieldName.COMPANY_ID].Ordinal]);
                                else if (usrtype == "ผู้ประเมินจากภายนอก")
                                    result.user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Assessor.FieldName.ASSESSOR_ID].Ordinal]);
                                else
                                    result.user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Admin.FieldName.ADMIN_ID].Ordinal]);

                                result.username = item.ItemArray[tabledata.Columns[Teacher.FieldName.USERNAME].Ordinal].ToString();
                                result.user_type = usrtype;
                                //**********************************************

                                result.information.addr = item.ItemArray[tabledata.Columns[Teacher.FieldName.ADDR].Ordinal].ToString();
                                result.information.citizen_id = item.ItemArray[tabledata.Columns[Teacher.FieldName.CITIZEN_ID].Ordinal].ToString();
                                result.information.email = item.ItemArray[tabledata.Columns[Teacher.FieldName.EMAIL].Ordinal].ToString();
                                result.information.tel = item.ItemArray[tabledata.Columns[Teacher.FieldName.TEL].Ordinal].ToString();
                                result.information.gender = item.ItemArray[tabledata.Columns[Teacher.FieldName.GENDER].Ordinal].ToString() != "" ? Convert.ToChar(item.ItemArray[tabledata.Columns[Teacher.FieldName.GENDER].Ordinal]) : ' ';
                                result.information.file_name_pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[tabledata.Columns[Teacher.FieldName.FILE_NAME_PIC].Ordinal].ToString());
                                result.information.timestamp = item.ItemArray[tabledata.Columns[Teacher.FieldName.TIMESTAMP].Ordinal].ToString();
                                result.information.e_name = item.ItemArray[tabledata.Columns[Teacher.FieldName.E_NAME].Ordinal].ToString();
                                result.information.e_prename = item.ItemArray[tabledata.Columns[Teacher.FieldName.E_PRENAME].Ordinal].ToString();
                                result.information.t_name = item.ItemArray[tabledata.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString();
                                result.information.t_prename = item.ItemArray[tabledata.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString();
                                result.information.SetPassword(item.ItemArray[tabledata.Columns[Teacher.FieldName.PASSWORD].Ordinal].ToString());

                                if (usrtype == "อาจารย์")
                                {
                                    result.information.degree = item.ItemArray[tabledata.Columns[Teacher.FieldName.DEGREE].Ordinal].ToString() != "" ? Convert.ToChar(item.ItemArray[tabledata.Columns[Teacher.FieldName.DEGREE].Ordinal]) : ' ';
                                    result.information.position = item.ItemArray[tabledata.Columns[Teacher.FieldName.POSITION].Ordinal].ToString() != "" ? Convert.ToChar(item.ItemArray[tabledata.Columns[Teacher.FieldName.POSITION].Ordinal]) : ' ';
                                    result.information.personnel_type = item.ItemArray[tabledata.Columns[Teacher.FieldName.PERSONNEL_TYPE].Ordinal].ToString();
                                    result.information.person_id = item.ItemArray[tabledata.Columns[Teacher.FieldName.PERSON_ID].Ordinal].ToString();
                                    result.information.room = item.ItemArray[tabledata.Columns[Teacher.FieldName.ROOM].Ordinal].ToString();
                                    result.information.status = item.ItemArray[tabledata.Columns[Teacher.FieldName.STATUS].Ordinal].ToString();
                                    result.information.alive = item.ItemArray[tabledata.Columns[Teacher.FieldName.POSITION].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[tabledata.Columns[Teacher.FieldName.ALIVE].Ordinal]) : -1;
                                }
                                else if (usrtype == "เจ้าหน้าที่")
                                {
                                    result.information.room = item.ItemArray[tabledata.Columns[Staff.FieldName.ROOM].Ordinal].ToString();
                                }
                                else if (usrtype == "บริษัท")
                                {
                                    result.information.company_name = item.ItemArray[tabledata.Columns[Company.FieldName.COMPANY_NAME].Ordinal].ToString();
                                }
                                else if (usrtype == "ผู้ประเมินจากภายนอก")
                                {

                                }
                                else if (usrtype == "นักศึกษา")
                                {

                                }
                                else if (usrtype == "ศิษย์เก่า")
                                {

                                }
                            }
                            else if (tabledata.Columns.Contains(Educational_teacher_staff.FieldName.COLLEGE))
                            {
                                //2 retrieve education data(all user type except student)
                                result.information.education.Add(new Educational_teacher_staff
                                {
                                    college = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.COLLEGE].Ordinal].ToString(),
                                    degree = Convert.ToChar(item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.DEGREE].Ordinal].ToString()),
                                    grad_year = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.GRAD_YEAR].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.GRAD_YEAR].Ordinal]) : 0,
                                    major = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.MAJOR].Ordinal].ToString(),
                                    personnel_id = result.user_id,
                                    education_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.EDUCATION_ID].Ordinal]),
                                    pre_major = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.PRE_MAJOR].Ordinal].ToString()
                                });
                            }
                            else if (tabledata.Columns.Contains("user_curri_id"))
                            {
                                //3 retrieve user_curri_id which user is in
                                result.curri_id_in.Add(item.ItemArray[tabledata.Columns["user_curri_id"].Ordinal].ToString());
                            }
                            else if (tabledata.Columns.Contains("pres_curri_id"))
                            {
                                //4 retrieve president_in (pres_curri_id, aca_year) when user is? (teacher only)
                                if (result.president_in == null)
                                    result.president_in = new Dictionary<string, List<int>>();
                                string curri_id = item.ItemArray[tabledata.Columns["pres_curri_id"].Ordinal].ToString();
                                if (!result.president_in.ContainsKey(curri_id))
                                {
                                    result.president_in.Add(curri_id, new List<int>());
                                }
                                result.president_in[curri_id].Add(Convert.ToInt32(item.ItemArray[tabledata.Columns[President_curriculum.FieldName.ACA_YEAR].Ordinal]));
                            }
                            else if (tabledata.Columns.Contains("comm_curri_id"))
                            {
                                //5 retrieve committee_in (comm_curri_id, aca_year) when user is? (teacher only)
                                if (result.committee_in == null)
                                    result.committee_in = new Dictionary<string, List<int>>();
                                string curri_id = item.ItemArray[tabledata.Columns["comm_curri_id"].Ordinal].ToString();
                                if (!result.committee_in.ContainsKey(curri_id))
                                {
                                    result.committee_in.Add(curri_id, new List<int>());
                                }
                                result.committee_in[curri_id].Add(Convert.ToInt32(item.ItemArray[tabledata.Columns[Committee.FieldName.ACA_YEAR].Ordinal]));
                            }
                            else if (tabledata.Columns.Contains(Technical_interested.FieldName.TOPIC_INTERESTED))
                            {
                                //6 retrieve topic_interested (teacher only)
                                result.information.interest.Add(item.ItemArray[tabledata.Columns[Technical_interested.FieldName.TOPIC_INTERESTED].Ordinal].ToString());
                            }
                            else if (tabledata.Columns.Contains("evid_curri_id"))
                            {
                                //7 retrieve not_send_primary (teacher only ? evid_curri_id,curr_tname, aca_year, evidence_name)
                                if (result.not_send_primary == null)
                                    result.not_send_primary = new List<Evidence_brief_detail>();
                                result.not_send_primary.Add(new Evidence_brief_detail
                                {
                                    curri_id = item.ItemArray[tabledata.Columns["evid_curri_id"].Ordinal].ToString(),
                                    curr_tname = item.ItemArray[tabledata.Columns[Cu_curriculum.FieldName.CURR_TNAME].Ordinal].ToString(),
                                    aca_year = Convert.ToInt32(item.ItemArray[tabledata.Columns[Primary_evidence.FieldName.ACA_YEAR].Ordinal]),
                                    evidence_name = item.ItemArray[tabledata.Columns[Primary_evidence.FieldName.EVIDENCE_NAME].Ordinal].ToString()
                                });
                            }
                            else if (tabledata.Columns.Contains(Extra_privilege.FieldName.TITLE_CODE))
                            {
                                //8 retrieve privilege (use predefined select from temp table cmd)
                                string curri_id = item.ItemArray[tabledata.Columns[User_curriculum.FieldName.CURRI_ID].Ordinal].ToString();

                                if (Convert.ToInt32(item.ItemArray[tabledata.Columns["privilege_type"].Ordinal]) == 1)
                                {
                                    //Add normal privilege
                                    if (!result.privilege.ContainsKey(curri_id))
                                    {

                                        result.privilege.Add(curri_id, new Dictionary<int, int>());
                                    }
                                    result.privilege[curri_id][Convert.ToInt32(item.ItemArray[tabledata.Columns[Extra_privilege.FieldName.TITLE_CODE].Ordinal])] = Convert.ToInt32(item.ItemArray[tabledata.Columns[Extra_privilege.FieldName.TITLE_PRIVILEGE_CODE].Ordinal]);
                                }
                                else
                                {
                                    //Add committee privilege
                                    if (result.committee_privilege == null)
                                        result.committee_privilege = new Dictionary<string, Dictionary<int, int>>();
                                    if (!result.committee_privilege.ContainsKey(curri_id))
                                    {

                                        result.committee_privilege.Add(curri_id, new Dictionary<int, int>());
                                    }
                                    result.committee_privilege[curri_id][Convert.ToInt32(item.ItemArray[tabledata.Columns[Extra_privilege.FieldName.TITLE_CODE].Ordinal])] = Convert.ToInt32(item.ItemArray[tabledata.Columns[Extra_privilege.FieldName.TITLE_PRIVILEGE_CODE].Ordinal]);
                                }
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

        public async Task<object> UpdateUserData(User_information_with_privilege_information userdata)
        {
            DBConnector d = new DBConnector();
            User_information_with_privilege_information result = new User_information_with_privilege_information();

            if (!d.SQLConnect())
                return "Cannot connect to database.";

            string temp80tablename = "#temp80";
            string createtabletemp80 = string.Format("create table {0}(" +
            "[row_num] int identity(1, 1) not null," +
            "[file_name_pic_del] {1} null," +
            "primary key([row_num])) " +

            "alter table {0} " +
            "alter column[file_name_pic_del] {1} collate database_default ",
            temp80tablename, DBFieldDataType.FILE_NAME_TYPE);

            string mainupdatecmd = "";
            if(userdata.information.file_name_pic == null)
            {
                mainupdatecmd = string.Format("update {0} set {1} = '{2}', {3} = '{4}', {5} = '{6}', {7} = '{8}'," +
                "{9} = '{10}', {11} = '{12}' where {13} = {14} ",
                User_list.FieldName.TABLE_NAME, Teacher.FieldName.T_PRENAME, userdata.information.t_prename,
                Teacher.FieldName.T_NAME, userdata.information.t_name,
                Teacher.FieldName.E_PRENAME, userdata.information.e_prename,
                Teacher.FieldName.E_NAME, userdata.information.e_name,
                Teacher.FieldName.TEL, userdata.information.tel,
                Teacher.FieldName.ADDR, userdata.information.addr,
                User_list.FieldName.USER_ID, userdata.user_id);
            }
            else
            {
                mainupdatecmd = string.Format("insert into {17} " +
                "select * from " +
                "(update {0} set {1} = '{2}', {3} = '{4}', {5} = '{6}', {7} = '{8}'," +
                "{9} = '{10}', {11} = '{12}', {13} = '{14}' output deleted.{13} where {15} = {16}) as outputupdate ",
                User_list.FieldName.TABLE_NAME, Teacher.FieldName.T_PRENAME, userdata.information.t_prename,
                Teacher.FieldName.T_NAME, userdata.information.t_name,
                Teacher.FieldName.E_PRENAME, userdata.information.e_prename,
                Teacher.FieldName.E_NAME, userdata.information.e_name,
                Teacher.FieldName.TEL, userdata.information.tel,
                Teacher.FieldName.ADDR, userdata.information.addr,
                Teacher.FieldName.FILE_NAME_PIC, userdata.information.file_name_pic,
                User_list.FieldName.USER_ID, userdata.user_id,temp80tablename);
            }

            //email must UNIQUE
            string emailupdatecmd = string.Format("if not exists (select * from {0} where {1} = '{2}' and {3} != {4}) " +
            "BEGIN " +
            "update {0} set {1} = '{2}' where {3} = {4} " +
            "END ",
            User_list.FieldName.TABLE_NAME, Teacher.FieldName.EMAIL, userdata.information.email, User_list.FieldName.USER_ID, userdata.user_id);

            string updateteachertable = "";
            string deletefromtechin = "";
            string insertintotechin = "";

            if (userdata.user_type == "อาจารย์") {
                updateteachertable = string.Format("update {0} set {1} = '{2}' where {3} = {4} ",
                    Teacher.FieldName.TABLE_NAME, Teacher.FieldName.STATUS, userdata.information.status,
                    Teacher.FieldName.TEACHER_ID, userdata.user_id);
                deletefromtechin = string.Format("delete from {0} where {1} = {2} ",
                    Technical_interested.FieldName.TABLE_NAME,Technical_interested.FieldName.TEACHER_ID,
                    userdata.user_id);
                if (userdata.information.interest.Count != 0) {
                    insertintotechin = string.Format("insert into {0} values ",
                    Technical_interested.FieldName.TABLE_NAME);
                    int insertintotechinlength = insertintotechin.Length;
                    foreach (string topic in userdata.information.interest)
                    {
                        if (insertintotechin.Length <= insertintotechinlength)
                            insertintotechin += string.Format("({0},'{1}')", userdata.user_id, topic);
                        else
                            insertintotechin += string.Format(",({0},'{1}')", userdata.user_id, topic);
                    }
                }

            }

            string deletefromeducationcmd = "";

            if (userdata.user_type != "นักศึกษา")
            {
                deletefromeducationcmd = string.Format("delete from {0} where {1} = {2} ",
                    Educational_teacher_staff.FieldName.TABLE_NAME, Educational_teacher_staff.FieldName.PERSONNEL_ID,
                    userdata.user_id);
                string excludecmd = "1=1 ";
                foreach (Educational_teacher_staff e in userdata.information.education)
                    excludecmd += string.Format("and {0} != {1} ",Educational_teacher_staff.FieldName.EDUCATION_ID,e.education_id);
                deletefromeducationcmd += string.Format("and ({0}) ", excludecmd);
            }

            string selectuserdatacmd = "";

            if (userdata.user_type == "อาจารย์")
                selectuserdatacmd = getSelectUserDataCommand(userdata.user_id.ToString(), 0, "#temp99");
            else if (userdata.user_type == "เจ้าหน้าที่")
                selectuserdatacmd = getSelectUserDataCommand(userdata.user_id.ToString(), 1, "#temp98");
            else if (userdata.user_type == "นักศึกษา")
                selectuserdatacmd = getSelectUserDataCommand(userdata.user_id.ToString(), 2, "#temp97");
            else if (userdata.user_type == "ศิษย์เก่า")
                selectuserdatacmd = getSelectUserDataCommand(userdata.user_id.ToString(), 3, "#temp96");
            else if (userdata.user_type == "บริษัท")
                selectuserdatacmd = getSelectUserDataCommand(userdata.user_id.ToString(), 4, "#temp95");
            else if (userdata.user_type == "ผู้ประเมินจากภายนอก")
                selectuserdatacmd = getSelectUserDataCommand(userdata.user_id.ToString(), 5, "#temp94");
            else if (userdata.user_type == "ผู้ดูแลระบบ")
                selectuserdatacmd = getSelectUserDataCommand(userdata.user_id.ToString(), 6, "#temp93");

            string selectfiletodelcmd = string.Format("select * from {0} ", temp80tablename);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} {6} {7} {8} END ",
                createtabletemp80, mainupdatecmd, emailupdatecmd, updateteachertable, deletefromtechin, insertintotechin,
                deletefromeducationcmd, selectuserdatacmd, selectfiletodelcmd);

            file_name_pic = null;

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
                            if (tabledata.Columns.Contains(Teacher.FieldName.T_PRENAME))
                            {
                                //1 retrieve user_data from pre-defined select table command
                                string usrtype = item.ItemArray[tabledata.Columns[Teacher.FieldName.USER_TYPE].Ordinal].ToString();

                                if (usrtype == "อาจารย์")
                                    result.user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Teacher.FieldName.TEACHER_ID].Ordinal]);
                                else if (usrtype == "เจ้าหน้าที่")
                                    result.user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Staff.FieldName.STAFF_ID].Ordinal]);
                                else if (usrtype == "นักศึกษา" || usrtype == "ศิษย์เก่า")
                                    result.user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Student.FieldName.USER_ID].Ordinal]);
                                else if (usrtype == "บริษัท")
                                    result.user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Company.FieldName.COMPANY_ID].Ordinal]);
                                else if (usrtype == "ผู้ประเมินจากภายนอก")
                                    result.user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Assessor.FieldName.ASSESSOR_ID].Ordinal]);
                                else
                                    result.user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Admin.FieldName.ADMIN_ID].Ordinal]);

                                result.username = item.ItemArray[tabledata.Columns[Teacher.FieldName.USERNAME].Ordinal].ToString();
                                result.user_type = usrtype;
                                //**********************************************

                                result.information.addr = item.ItemArray[tabledata.Columns[Teacher.FieldName.ADDR].Ordinal].ToString();
                                result.information.citizen_id = item.ItemArray[tabledata.Columns[Teacher.FieldName.CITIZEN_ID].Ordinal].ToString();
                                result.information.email = item.ItemArray[tabledata.Columns[Teacher.FieldName.EMAIL].Ordinal].ToString();
                                result.information.tel = item.ItemArray[tabledata.Columns[Teacher.FieldName.TEL].Ordinal].ToString();
                                result.information.gender = item.ItemArray[tabledata.Columns[Teacher.FieldName.GENDER].Ordinal].ToString() != "" ? Convert.ToChar(item.ItemArray[tabledata.Columns[Teacher.FieldName.GENDER].Ordinal]) : ' ';
                                result.information.file_name_pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[tabledata.Columns[Teacher.FieldName.FILE_NAME_PIC].Ordinal].ToString());
                                result.information.timestamp = item.ItemArray[tabledata.Columns[Teacher.FieldName.TIMESTAMP].Ordinal].ToString();
                                result.information.e_name = item.ItemArray[tabledata.Columns[Teacher.FieldName.E_NAME].Ordinal].ToString();
                                result.information.e_prename = item.ItemArray[tabledata.Columns[Teacher.FieldName.E_PRENAME].Ordinal].ToString();
                                result.information.t_name = item.ItemArray[tabledata.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString();
                                result.information.t_prename = item.ItemArray[tabledata.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString();
                                result.information.SetPassword(item.ItemArray[tabledata.Columns[Teacher.FieldName.PASSWORD].Ordinal].ToString());

                                if (usrtype == "อาจารย์")
                                {
                                    result.information.degree = item.ItemArray[tabledata.Columns[Teacher.FieldName.DEGREE].Ordinal].ToString() != "" ? Convert.ToChar(item.ItemArray[tabledata.Columns[Teacher.FieldName.DEGREE].Ordinal]) : ' ';
                                    result.information.position = item.ItemArray[tabledata.Columns[Teacher.FieldName.POSITION].Ordinal].ToString() != "" ? Convert.ToChar(item.ItemArray[tabledata.Columns[Teacher.FieldName.POSITION].Ordinal]) : ' ';
                                    result.information.personnel_type = item.ItemArray[tabledata.Columns[Teacher.FieldName.PERSONNEL_TYPE].Ordinal].ToString();
                                    result.information.person_id = item.ItemArray[tabledata.Columns[Teacher.FieldName.PERSON_ID].Ordinal].ToString();
                                    result.information.room = item.ItemArray[tabledata.Columns[Teacher.FieldName.ROOM].Ordinal].ToString();
                                    result.information.status = item.ItemArray[tabledata.Columns[Teacher.FieldName.STATUS].Ordinal].ToString();
                                    result.information.alive = item.ItemArray[tabledata.Columns[Teacher.FieldName.POSITION].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[tabledata.Columns[Teacher.FieldName.ALIVE].Ordinal]) : -1;
                                }
                                else if (usrtype == "เจ้าหน้าที่")
                                {
                                    result.information.room = item.ItemArray[tabledata.Columns[Staff.FieldName.ROOM].Ordinal].ToString();
                                }
                                else if (usrtype == "บริษัท")
                                {
                                    result.information.company_name = item.ItemArray[tabledata.Columns[Company.FieldName.COMPANY_NAME].Ordinal].ToString();
                                }
                                else if (usrtype == "ผู้ประเมินจากภายนอก")
                                {

                                }
                                else if (usrtype == "นักศึกษา")
                                {

                                }
                                else if (usrtype == "ศิษย์เก่า")
                                {

                                }
                            }
                            else if (tabledata.Columns.Contains(Educational_teacher_staff.FieldName.COLLEGE))
                            {
                                //2 retrieve education data(all user type except student)
                                result.information.education.Add(new Educational_teacher_staff
                                {
                                    college = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.COLLEGE].Ordinal].ToString(),
                                    degree = Convert.ToChar(item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.DEGREE].Ordinal].ToString()),
                                    grad_year = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.GRAD_YEAR].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.GRAD_YEAR].Ordinal]) : 0,
                                    major = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.MAJOR].Ordinal].ToString(),
                                    personnel_id = result.user_id,
                                    education_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.EDUCATION_ID].Ordinal]),
                                    pre_major = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.PRE_MAJOR].Ordinal].ToString()
                                });
                            }
                            else if (tabledata.Columns.Contains("user_curri_id"))
                            {
                                //3 retrieve user_curri_id which user is in
                                result.curri_id_in.Add(item.ItemArray[tabledata.Columns["user_curri_id"].Ordinal].ToString());
                            }
                            else if (tabledata.Columns.Contains("pres_curri_id"))
                            {
                                //4 retrieve president_in (pres_curri_id, aca_year) when user is? (teacher only)
                                if (result.president_in == null)
                                    result.president_in = new Dictionary<string, List<int>>();
                                string curri_id = item.ItemArray[tabledata.Columns["pres_curri_id"].Ordinal].ToString();
                                if (!result.president_in.ContainsKey(curri_id))
                                {
                                    result.president_in.Add(curri_id, new List<int>());
                                }
                                result.president_in[curri_id].Add(Convert.ToInt32(item.ItemArray[tabledata.Columns[President_curriculum.FieldName.ACA_YEAR].Ordinal]));
                            }
                            else if (tabledata.Columns.Contains("comm_curri_id"))
                            {
                                //5 retrieve committee_in (comm_curri_id, aca_year) when user is? (teacher only)
                                if (result.committee_in == null)
                                    result.committee_in = new Dictionary<string, List<int>>();
                                string curri_id = item.ItemArray[tabledata.Columns["comm_curri_id"].Ordinal].ToString();
                                if (!result.committee_in.ContainsKey(curri_id))
                                {
                                    result.committee_in.Add(curri_id, new List<int>());
                                }
                                result.committee_in[curri_id].Add(Convert.ToInt32(item.ItemArray[tabledata.Columns[Committee.FieldName.ACA_YEAR].Ordinal]));
                            }
                            else if (tabledata.Columns.Contains(Technical_interested.FieldName.TOPIC_INTERESTED))
                            {
                                //6 retrieve topic_interested (teacher only)
                                result.information.interest.Add(item.ItemArray[tabledata.Columns[Technical_interested.FieldName.TOPIC_INTERESTED].Ordinal].ToString());
                            }
                            else if (tabledata.Columns.Contains("evid_curri_id"))
                            {
                                //7 retrieve not_send_primary (teacher only ? evid_curri_id,curr_tname, aca_year, evidence_name)
                                if (result.not_send_primary == null)
                                    result.not_send_primary = new List<Evidence_brief_detail>();
                                result.not_send_primary.Add(new Evidence_brief_detail
                                {
                                    curri_id = item.ItemArray[tabledata.Columns["evid_curri_id"].Ordinal].ToString(),
                                    curr_tname = item.ItemArray[tabledata.Columns[Cu_curriculum.FieldName.CURR_TNAME].Ordinal].ToString(),
                                    aca_year = Convert.ToInt32(item.ItemArray[tabledata.Columns[Primary_evidence.FieldName.ACA_YEAR].Ordinal]),
                                    evidence_name = item.ItemArray[tabledata.Columns[Primary_evidence.FieldName.EVIDENCE_NAME].Ordinal].ToString()
                                });
                            }
                            else if (tabledata.Columns.Contains(Extra_privilege.FieldName.TITLE_CODE))
                            {
                                //8 retrieve privilege (use predefined select from temp table cmd)
                                string curri_id = item.ItemArray[tabledata.Columns[User_curriculum.FieldName.CURRI_ID].Ordinal].ToString();

                                if (Convert.ToInt32(item.ItemArray[tabledata.Columns["privilege_type"].Ordinal]) == 1)
                                {
                                    //Add normal privilege
                                    if (!result.privilege.ContainsKey(curri_id))
                                    {

                                        result.privilege.Add(curri_id, new Dictionary<int, int>());
                                    }
                                    result.privilege[curri_id][Convert.ToInt32(item.ItemArray[tabledata.Columns[Extra_privilege.FieldName.TITLE_CODE].Ordinal])] = Convert.ToInt32(item.ItemArray[tabledata.Columns[Extra_privilege.FieldName.TITLE_PRIVILEGE_CODE].Ordinal]);
                                }
                                else
                                {
                                    //Add committee privilege
                                    if (result.committee_privilege == null)
                                        result.committee_privilege = new Dictionary<string, Dictionary<int, int>>();
                                    if (!result.committee_privilege.ContainsKey(curri_id))
                                    {

                                        result.committee_privilege.Add(curri_id, new Dictionary<int, int>());
                                    }
                                    result.committee_privilege[curri_id][Convert.ToInt32(item.ItemArray[tabledata.Columns[Extra_privilege.FieldName.TITLE_CODE].Ordinal])] = Convert.ToInt32(item.ItemArray[tabledata.Columns[Extra_privilege.FieldName.TITLE_PRIVILEGE_CODE].Ordinal]);
                                }
                            }
                            else if (tabledata.Columns.Contains("file_name_pic_del"))
                            {
                                //get file name pic to delete
                                file_name_pic = item.ItemArray[tabledata.Columns["file_name_pic_del"].Ordinal].ToString();
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
    }
}


