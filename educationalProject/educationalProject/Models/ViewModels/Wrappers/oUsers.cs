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
        private string getSelectTeacherWithCurriculumCommand()
        {
            string temp99tablename = "#temp99";
            string createtabletemp99 = string.Format("create table {0}(" +
            "[row_num] int identity(1, 1) not null," +
            "[{1}] INT NULL," +
            "[{2}] VARCHAR(40) NULL," +
            "[{3}] {28} NULL," +
            "[{4}] VARCHAR(MAX) NULL," +
            "[{5}] VARCHAR(16) NULL," +
            "[{6}] VARCHAR(60) NULL," +
            "[{7}] VARCHAR(16) NULL," +
            "[{8}] VARCHAR(60) NULL," +
            "[{9}] CHAR(13) NULL," +
            "[{10}] CHAR NULL," +
            
            "[{11}] VARCHAR(1000) NULL," +
            "[{12}] VARCHAR(1000) NULL," +
            "[{13}] VARCHAR(1000) NULL," +
            "[{14}] {29} NULL," +
            
            "[{15}] DATETIME2 NULL," +

            "[{16}] VARCHAR(40) NULL," +
            "[{17}] CHAR NULL," +
            "[{18}] CHAR NULL," +
            "[{19}] VARCHAR(2) NULL," +
            "[{20}] VARCHAR(4) NULL," +
            "[{21}] VARCHAR(40) NULL," +
            "[{22}] TINYINT NULL," +

            "[{30}] INT NULL," +
            "[{23}] CHAR NULL," +
            "[{24}] VARCHAR(100) NULL," +
            "[{25}] VARCHAR(200) NULL," +
            "[{26}] INT NULL," +
            "[{27}] VARCHAR(200) NULL," +
            "PRIMARY KEY([row_num])) " +

            "alter table {0} " +
            "alter column [{2}] VARCHAR(40) collate database_default " +

            "alter table {0} " +
            "alter column [{3}] {28} collate database_default " +

            "alter table {0} " +
            "alter column [{4}] VARCHAR(MAX) collate database_default " +

            "alter table {0} " +
            "alter column [{5}] VARCHAR(16) collate database_default " +

            "alter table {0} " +
            "alter column [{6}] VARCHAR(60) collate database_default " +

            "alter table {0} " +
            "alter column [{7}] VARCHAR(16) collate database_default " +

            "alter table {0} " +
            "alter column [{8}] VARCHAR(60) collate database_default " +

            "alter table {0} " +
            "alter column [{9}] CHAR(13) collate database_default " +

            "alter table {0} " +
            "alter column [{10}] CHAR collate database_default " +

            "alter table {0} " +
            "alter column [{11}] VARCHAR(1000) collate database_default " +

            "alter table {0} " +
            "alter column [{12}] VARCHAR(1000) collate database_default " +

            "alter table {0} " +
            "alter column [{13}] VARCHAR(1000) collate database_default " +

            "alter table {0} " +
            "alter column [{14}] {29} collate database_default " +

            "alter table {0} " +
            "alter column [{16}] VARCHAR(40) collate database_default " +

            "alter table {0} " +
            "alter column [{17}] CHAR collate database_default " +

            "alter table {0} " +
            "alter column [{18}] CHAR collate database_default " +

            "alter table {0} " +
            "alter column [{19}] VARCHAR(2) collate database_default " +

            "alter table {0} " +
            "alter column [{20}] VARCHAR(4) collate database_default " +

            "alter table {0} " +
            "alter column [{21}] VARCHAR(40) collate database_default " +

            "alter table {0} " +
            "alter column [{23}] CHAR collate database_default " +

            "alter table {0} " +
            "alter column [{24}] VARCHAR(100) collate database_default " +

            "alter table {0} " +
            "alter column [{25}] VARCHAR(200) collate database_default " +

            "alter table {0} " +
            "alter column [{27}] VARCHAR(200) collate database_default ",
            temp99tablename, Teacher.FieldName.TEACHER_ID, Teacher.FieldName.USER_TYPE, Teacher.FieldName.USERNAME,
            Teacher.FieldName.PASSWORD,Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,Teacher.FieldName.E_PRENAME, Teacher.FieldName.E_NAME,
            Teacher.FieldName.CITIZEN_ID,Teacher.FieldName.GENDER,Teacher.FieldName.EMAIL,Teacher.FieldName.TEL, Teacher.FieldName.ADDR, Teacher.FieldName.FILE_NAME_PIC,
            Teacher.FieldName.TIMESTAMP, Teacher.FieldName.ROOM, Teacher.FieldName.DEGREE,Teacher.FieldName.POSITION, Teacher.FieldName.PERSONNEL_TYPE,
            Teacher.FieldName.PERSON_ID, Teacher.FieldName.STATUS, Teacher.FieldName.ALIVE, "edudeg", Educational_teacher_staff.FieldName.PRE_MAJOR,Educational_teacher_staff.FieldName.MAJOR,
            Educational_teacher_staff.FieldName.GRAD_YEAR, Educational_teacher_staff.FieldName.COLLEGE,DBFieldDataType.USERNAME_TYPE,DBFieldDataType.FILE_NAME_TYPE,
            Educational_teacher_staff.FieldName.EDUCATION_ID);

            string insertintotemp99truecase = string.Format("insert into {0} " +
            "select tsres.*, {1} ,{2}.{3} as edudeg, {4}, {5}, {6},{7} from " +
            "({8}) as tsres, {2} where {9} = '{10}' " +
            "and {11} = {12} ", temp99tablename, Educational_teacher_staff.FieldName.EDUCATION_ID,
            Educational_teacher_staff.FieldName.TABLE_NAME,
            Educational_teacher_staff.FieldName.DEGREE, Educational_teacher_staff.FieldName.PRE_MAJOR,
            Educational_teacher_staff.FieldName.MAJOR, Educational_teacher_staff.FieldName.GRAD_YEAR,
            Educational_teacher_staff.FieldName.COLLEGE, oTeacher.getSelectTeacherByJoinCommand(),
            Teacher.FieldName.USERNAME, username, Educational_teacher_staff.FieldName.PERSONNEL_ID,
            Teacher.FieldName.TEACHER_ID);

            string insertintotemp99falsecase = string.Format("insert into {0} " +
            "select *, null,null, null, null, null, null from ({3}) as stres where {1} = '{2}' ",
            temp99tablename, Teacher.FieldName.USERNAME, username, oTeacher.getSelectTeacherByJoinCommand());


            string insertintotemp99_1 = string.Format("if exists(select * from {0} where {1} = '{2}' " +
            "and {3} in (select {4} from {5} where {3} = {4})) " +
            insertintotemp99truecase + " " + "else " + insertintotemp99falsecase + " ",
            User_list.FieldName.TABLE_NAME, Teacher.FieldName.USERNAME, username,
            User_list.FieldName.USER_ID, Educational_teacher_staff.FieldName.PERSONNEL_ID,
            Educational_teacher_staff.FieldName.TABLE_NAME);

            //User_id = -1 => retrieve curri_in
            string insertintotemp99_2 = string.Format("insert into {0}({1},{2}) " +
            "select -1,{3} from (select max({1}) as {1} from {0}) as tid,{4} " +
            "where {1} = {5} ",
            temp99tablename, Teacher.FieldName.TEACHER_ID, Personnel.FieldName.EMAIL,
            User_curriculum.FieldName.CURRI_ID,User_curriculum.FieldName.TABLE_NAME,
            User_list.FieldName.USER_ID
            );
            
            //User_id = -2 => retrieve president_in
            string insertintotemp99_3 = string.Format("insert into {0}({1},{2},{3}) " +
                "select -2,{4},{5} from (select max({1}) as {1} from {0}) as tid,{6} " +
                "where tid.{1} = {6}.{7} ",
                temp99tablename, Teacher.FieldName.TEACHER_ID, Personnel.FieldName.EMAIL, Personnel.FieldName.TEL,
                President_curriculum.FieldName.CURRI_ID,President_curriculum.FieldName.ACA_YEAR,
                President_curriculum.FieldName.TABLE_NAME,President_curriculum.FieldName.TEACHER_ID);

            //User_id = -3 => retrieve topic_interested
            string insertintotemp99_4 = string.Format("insert into {0}({1},{2}) " +
                "select -3,{3} from (select max({1}) as {1} from {0}) as tid,{4} " +
                "where tid.{1} = {4}.{5} ",
                temp99tablename, Teacher.FieldName.TEACHER_ID, Personnel.FieldName.EMAIL, Technical_interested.FieldName.TOPIC_INTERESTED,
                Technical_interested.FieldName.TABLE_NAME, Technical_interested.FieldName.TEACHER_ID);

            //User_id = -4 => retrieve committee_in
            string insertintotemp99_5 = string.Format("insert into {0}({1},{2},{3}) " +
                "select -4,{4},{5} from (select max({1}) as {1} from {0}) as tid,{6} " +
                "where tid.{1} = {6}.{7} ",
                temp99tablename, Teacher.FieldName.TEACHER_ID, Personnel.FieldName.EMAIL, Personnel.FieldName.TEL,
                Committee.FieldName.CURRI_ID, Committee.FieldName.ACA_YEAR,
                Committee.FieldName.TABLE_NAME, Committee.FieldName.TEACHER_ID);

            string selectcmd = string.Format("select * from {0} ", temp99tablename);

            return string.Format(" BEGIN {0} {1} {2} {3} {4} {5} {6} END ", createtabletemp99, insertintotemp99_1, insertintotemp99_2,insertintotemp99_3,insertintotemp99_4,insertintotemp99_5, selectcmd);
        }

        private string getSelectStaffWithCurriculumCommand()
        {
            string temp99tablename = "#temp98";
            string createtabletemp99 = string.Format("create table {0}(" +
            "[row_num] int identity(1, 1) not null," +
            "[{1}] INT NULL," +
            "[{2}] VARCHAR(40) NULL," +
            "[{3}] {22} NULL," +
            "[{4}] VARCHAR(MAX) NULL," +
            "[{5}] VARCHAR(16) NULL," +
            "[{6}] VARCHAR(60) NULL," +
            "[{7}] VARCHAR(16) NULL," +
            "[{8}] VARCHAR(60) NULL," +
            "[{9}] CHAR(13) NULL," +
            "[{10}] CHAR NULL," +
            "[{11}] VARCHAR(1000) NULL," +
            "[{12}] VARCHAR(1000) NULL," +
            "[{13}] VARCHAR(1000) NULL," +
            "[{14}] {23} NULL," +
            "[{15}] DATETIME2 NULL," +
      
            "[{16}] VARCHAR(40) NULL," +

            "[{24}] INT NULL," +
            "[{17}] CHAR NULL," +
            "[{18}] VARCHAR(100) NULL," +
            "[{19}] VARCHAR(200) NULL," +
            "[{20}] INT NULL," +
            "[{21}] VARCHAR(200) NULL," +
            "PRIMARY KEY([row_num])) " +

            "alter table {0} " +
            "alter column [{2}] VARCHAR(40) collate database_default " +

            "alter table {0} " +
            "alter column [{3}] {22} collate database_default " +

            "alter table {0} " +
            "alter column [{4}] VARCHAR(MAX) collate database_default " +

            "alter table {0} " +
            "alter column [{5}] VARCHAR(16) collate database_default " +

            "alter table {0} " +
            "alter column [{6}] VARCHAR(60) collate database_default " +

            "alter table {0} " +
            "alter column [{7}] VARCHAR(16) collate database_default " +

            "alter table {0} " +
            "alter column [{8}] VARCHAR(60) collate database_default " +

            "alter table {0} " +
            "alter column [{9}] CHAR(13) collate database_default " +

            "alter table {0} " +
            "alter column [{10}] CHAR collate database_default " +

            "alter table {0} " +
            "alter column [{11}] VARCHAR(1000) collate database_default " +

            "alter table {0} " +
            "alter column [{12}] VARCHAR(1000) collate database_default " +

            "alter table {0} " +
            "alter column [{13}] VARCHAR(1000) collate database_default " +

            "alter table {0} " +
            "alter column [{14}] {23} collate database_default " +

            "alter table {0} " +
            "alter column [{16}] VARCHAR(40) collate database_default " +

            "alter table {0} " +
            "alter column [{17}] CHAR collate database_default " +

            "alter table {0} " +
            "alter column [{18}] VARCHAR(100) collate database_default " +

            "alter table {0} " +
            "alter column [{19}] VARCHAR(200) collate database_default " +

            "alter table {0} " +
            "alter column [{21}] VARCHAR(200) collate database_default ",
            temp99tablename, Staff.FieldName.STAFF_ID, Staff.FieldName.USER_TYPE, Staff.FieldName.USERNAME,
            Staff.FieldName.PASSWORD, Staff.FieldName.T_PRENAME, Staff.FieldName.T_NAME, Staff.FieldName.E_PRENAME, Staff.FieldName.E_NAME,
            Staff.FieldName.CITIZEN_ID, Staff.FieldName.GENDER, Staff.FieldName.EMAIL, Staff.FieldName.TEL, Staff.FieldName.ADDR, Staff.FieldName.FILE_NAME_PIC,
            Staff.FieldName.TIMESTAMP, Staff.FieldName.ROOM,"edudeg", Educational_teacher_staff.FieldName.PRE_MAJOR, Educational_teacher_staff.FieldName.MAJOR,
            Educational_teacher_staff.FieldName.GRAD_YEAR, Educational_teacher_staff.FieldName.COLLEGE, DBFieldDataType.USERNAME_TYPE, DBFieldDataType.FILE_NAME_TYPE,
            Educational_teacher_staff.FieldName.EDUCATION_ID);

            string insertintotemp99truecase = string.Format("insert into {0} " +
            "select tsres.*, {1} ,{2}.{3} as edudeg, {4}, {5}, {6},{7} from " +
            "({8}) as tsres, {2} where {9} = '{10}' " +
            "and {11} = {12} ", temp99tablename,Educational_teacher_staff.FieldName.EDUCATION_ID, 
            Educational_teacher_staff.FieldName.TABLE_NAME,
            Educational_teacher_staff.FieldName.DEGREE, Educational_teacher_staff.FieldName.PRE_MAJOR,
            Educational_teacher_staff.FieldName.MAJOR, Educational_teacher_staff.FieldName.GRAD_YEAR,
            Educational_teacher_staff.FieldName.COLLEGE, oStaff.getSelectStaffByJoinCommand(),
            Staff.FieldName.USERNAME, username, Educational_teacher_staff.FieldName.PERSONNEL_ID,
            Staff.FieldName.STAFF_ID);

            string insertintotemp99falsecase = string.Format("insert into {0} " +
            "select *, null, null, null, null, null from ({3}) as stres where {1} = '{2}' ",
            temp99tablename, Staff.FieldName.USERNAME, username, oStaff.getSelectStaffByJoinCommand());


            string insertintotemp99_1 = string.Format("if exists(select * from {0} where {1} = '{2}' " +
            "and {3} in (select {4} from {5} where {3} = {4})) " +
            insertintotemp99truecase + " " + "else " + insertintotemp99falsecase + " ",
            User_list.FieldName.TABLE_NAME, Staff.FieldName.USERNAME, username,
            User_list.FieldName.USER_ID, Educational_teacher_staff.FieldName.PERSONNEL_ID,
            Educational_teacher_staff.FieldName.TABLE_NAME);


            //User_id = -1 => retrieve curri_in
            string insertintotemp99_2 = string.Format("insert into {0}({1},{2}) " +
            "select -1,{3} from (select max({1}) as {1} from {0}) as tid,{4} " +
            "where {1} = {5} ",
            temp99tablename, Staff.FieldName.STAFF_ID, Personnel.FieldName.EMAIL,
            User_curriculum.FieldName.CURRI_ID, User_curriculum.FieldName.TABLE_NAME,
            User_list.FieldName.USER_ID
            );

            string selectcmd = string.Format("select * from {0} ", temp99tablename);

            return string.Format(" BEGIN {0} {1} {2} {3} END ", createtabletemp99, insertintotemp99_1, insertintotemp99_2, selectcmd);
        }

        private string getSelectAssessorWithCurriculumCommand()
        {
            string temp99tablename = "#temp97";
            string createtabletemp99 = string.Format("create table {0}(" +
            "[row_num] int identity(1, 1) not null," +
            "[{1}] INT NULL," +
            "[{2}] VARCHAR(40) NULL," +
            "[{3}] {17} NULL," +
            "[{4}] VARCHAR(MAX) NULL," +
            "[{5}] VARCHAR(16) NULL," +
            "[{6}] VARCHAR(60) NULL," +
            "[{7}] VARCHAR(16) NULL," +
            "[{8}] VARCHAR(60) NULL," +
            "[{9}] CHAR(13) NULL," +
            "[{10}] CHAR NULL," +
            "[{11}] VARCHAR(1000) NULL," +
            "[{12}] VARCHAR(1000) NULL," +
            "[{13}] VARCHAR(1000) NULL," +
            "[{14}] {18} NULL," +
            "[{15}] DATETIME2 NULL," +

            "[{16}] INT NULL," +

            "PRIMARY KEY([row_num])) " +

            "alter table {0} " +
            "alter column [{2}] VARCHAR(40) collate database_default " +

            "alter table {0} " +
            "alter column [{3}] {17} collate database_default " +

            "alter table {0} " +
            "alter column [{4}] VARCHAR(MAX) collate database_default " +

            "alter table {0} " +
            "alter column [{5}] VARCHAR(16) collate database_default " +

            "alter table {0} " +
            "alter column [{6}] VARCHAR(60) collate database_default " +

            "alter table {0} " +
            "alter column [{7}] VARCHAR(16) collate database_default " +

            "alter table {0} " +
            "alter column [{8}] VARCHAR(60) collate database_default " +

            "alter table {0} " +
            "alter column [{9}] CHAR(13) collate database_default " +

            "alter table {0} " +
            "alter column [{10}] CHAR collate database_default " +

            "alter table {0} " +
            "alter column [{11}] VARCHAR(1000) collate database_default " +

            "alter table {0} " +
            "alter column [{12}] VARCHAR(1000) collate database_default " +

            "alter table {0} " +
            "alter column [{13}] VARCHAR(1000) collate database_default " +

            "alter table {0} " +
            "alter column [{14}] {18} collate database_default ",
            temp99tablename, Assessor.FieldName.ASSESSOR_ID, Assessor.FieldName.USER_TYPE, Assessor.FieldName.USERNAME,
            Assessor.FieldName.PASSWORD, Assessor.FieldName.T_PRENAME, Assessor.FieldName.T_NAME, Assessor.FieldName.E_PRENAME, Assessor.FieldName.E_NAME,
            Assessor.FieldName.CITIZEN_ID, Assessor.FieldName.GENDER, Assessor.FieldName.EMAIL, Assessor.FieldName.TEL, Assessor.FieldName.ADDR, Assessor.FieldName.FILE_NAME_PIC,
            Assessor.FieldName.TIMESTAMP, Assessor.FieldName.TEACHER_ID, DBFieldDataType.USERNAME_TYPE, DBFieldDataType.FILE_NAME_TYPE);

            string insertintotemp99falsecase = string.Format("insert into {0} " +
            "select * from ({3}) as stres where {1} = '{2}' ",
            temp99tablename, Assessor.FieldName.USERNAME, username, oAssessor.getSelectAssessorByJoinCommand());


            string insertintotemp99_1 = insertintotemp99falsecase;


            //User_id = -1 => retrieve curri_in
            string insertintotemp99_2 = string.Format("insert into {0}({1},{2}) " +
            "select -1,{3} from (select max({1}) as {1} from {0}) as tid,{4} " +
            "where {1} = {5} ",
            temp99tablename, Assessor.FieldName.ASSESSOR_ID, Personnel.FieldName.EMAIL,
            User_curriculum.FieldName.CURRI_ID, User_curriculum.FieldName.TABLE_NAME,
            User_list.FieldName.USER_ID
            );

            string selectcmd = string.Format("select * from {0} ", temp99tablename);

            return string.Format(" BEGIN {0} {1} {2} {3} END ", createtabletemp99, insertintotemp99_1, insertintotemp99_2, selectcmd);
        }
        private string getSelectCompanyWithCurriculumCommand()
        {
            string temp99tablename = "#temp96";
            string createtabletemp99 = string.Format("create table {0}(" +
            "[row_num] int identity(1, 1) not null," +
            "[{1}] INT NULL," +
            "[{2}] VARCHAR(40) NULL," +
            "[{3}] {18} NULL," +
            "[{4}] VARCHAR(MAX) NULL," +
            "[{5}] VARCHAR(16) NULL," +
            "[{6}] VARCHAR(60) NULL," +
            "[{7}] VARCHAR(16) NULL," +
            "[{8}] VARCHAR(60) NULL," +
            "[{9}] CHAR(13) NULL," +
            "[{10}] CHAR NULL," +
            "[{11}] VARCHAR(1000) NULL," +
            "[{12}] VARCHAR(1000) NULL," +
            "[{13}] VARCHAR(1000) NULL," +
            "[{14}] {19} NULL," +
            "[{15}] DATETIME2 NULL," +

            "[{16}] INT NULL," +
            "[{17}] VARCHAR(200) NULL," +
            "PRIMARY KEY([row_num])) " +

            "alter table {0} " +
            "alter column [{2}] VARCHAR(40) collate database_default " +

            "alter table {0} " +
            "alter column [{3}] {18} collate database_default " +

            "alter table {0} " +
            "alter column [{4}] VARCHAR(MAX) collate database_default " +

            "alter table {0} " +
            "alter column [{5}] VARCHAR(16) collate database_default " +

            "alter table {0} " +
            "alter column [{6}] VARCHAR(60) collate database_default " +

            "alter table {0} " +
            "alter column [{7}] VARCHAR(16) collate database_default " +

            "alter table {0} " +
            "alter column [{8}] VARCHAR(60) collate database_default " +

            "alter table {0} " +
            "alter column [{9}] CHAR(13) collate database_default " +

            "alter table {0} " +
            "alter column [{10}] CHAR collate database_default " +

            "alter table {0} " +
            "alter column [{11}] VARCHAR(1000) collate database_default " +

            "alter table {0} " +
            "alter column [{12}] VARCHAR(1000) collate database_default " +

            "alter table {0} " +
            "alter column [{13}] VARCHAR(1000) collate database_default " +

            "alter table {0} " +
            "alter column [{14}] {19} collate database_default " +
            "alter table {0} " +
            "alter column [{17}] VARCHAR(200) collate database_default ",
            temp99tablename, Company.FieldName.COMPANY_ID, Company.FieldName.USER_TYPE, Company.FieldName.USERNAME,
            Company.FieldName.PASSWORD, Company.FieldName.T_PRENAME, Company.FieldName.T_NAME, Company.FieldName.E_PRENAME, Company.FieldName.E_NAME,
            Company.FieldName.CITIZEN_ID, Company.FieldName.GENDER, Company.FieldName.EMAIL, Company.FieldName.TEL, Company.FieldName.ADDR, Company.FieldName.FILE_NAME_PIC,
            Company.FieldName.TIMESTAMP, Company.FieldName.TEACHER_ID, Company.FieldName.COMPANY_NAME,DBFieldDataType.USERNAME_TYPE, DBFieldDataType.FILE_NAME_TYPE);

            string insertintotemp99falsecase = string.Format("insert into {0} " +
            "select * from ({3}) as stres where {1} = '{2}' ",
            temp99tablename, Company.FieldName.USERNAME, username, oCompany.getSelectCompanyByJoinCommand());


            string insertintotemp99_1 = insertintotemp99falsecase;


            //User_id = -1 => retrieve curri_in
            string insertintotemp99_2 = string.Format("insert into {0}({1},{2}) " +
            "select -1,{3} from (select max({1}) as {1} from {0}) as tid,{4} " +
            "where {1} = {5} ",
            temp99tablename, Company.FieldName.COMPANY_ID, Personnel.FieldName.EMAIL,
            User_curriculum.FieldName.CURRI_ID, User_curriculum.FieldName.TABLE_NAME,
            User_list.FieldName.USER_ID
            );

            string selectcmd = string.Format("select * from {0} ", temp99tablename);

            return string.Format(" BEGIN {0} {1} {2} {3} END ", createtabletemp99, insertintotemp99_1, insertintotemp99_2, selectcmd);
        }

        private string getSelectStudentWithCurriculumCommand()
        {
            string temp99tablename = "#temp95";
            string createtabletemp99 = string.Format("create table {0}(" +
            "[row_num] int identity(1, 1) not null," +
            "[{1}] INT NULL," +
            "[{2}] VARCHAR(40) NULL," +
            "[{3}] {28} NULL," +
            "[{4}] VARCHAR(MAX) NULL," +
            "[{5}] VARCHAR(16) NULL," +
            "[{6}] VARCHAR(60) NULL," +
            "[{7}] VARCHAR(16) NULL," +
            "[{8}] VARCHAR(60) NULL," +
            "[{9}] CHAR(13) NULL," +
            "[{10}] CHAR NULL," +
            "[{11}] VARCHAR(1000) NULL," +
            "[{12}] VARCHAR(1000) NULL," +
            "[{13}] VARCHAR(1000) NULL," +
            "[{14}] {29} NULL," +
            "[{15}] DATETIME2 NULL," +

            "[{16}] VARCHAR(120) NOT NULL," +
            "[{17}] {30} NULL," +
            "[{18}] VARCHAR(2) NULL," +
            "[{19}] INT NULL," +
            "[{20}] DATE NULL," +
            "[{21}] INT NULL," +
            "[{22}] INT NULL," +
            "[{23}] DATE NULL," +
            "[{24}] VARCHAR(64) NULL," +
            "[{25}] CHAR NULL," +
            "[{26}] TINYINT NULL," +
            "[{27}] CHAR NULL," +
            "PRIMARY KEY([row_num])) " +

            "alter table {0} " +
            "alter column [{2}] VARCHAR(40) collate database_default " +

            "alter table {0} " +
            "alter column [{3}] {28} collate database_default " +

            "alter table {0} " +
            "alter column [{4}] VARCHAR(MAX) collate database_default " +

            "alter table {0} " +
            "alter column [{5}] VARCHAR(16) collate database_default " +

            "alter table {0} " +
            "alter column [{6}] VARCHAR(60) collate database_default " +

            "alter table {0} " +
            "alter column [{7}] VARCHAR(16) collate database_default " +

            "alter table {0} " +
            "alter column [{8}] VARCHAR(60) collate database_default " +

            "alter table {0} " +
            "alter column [{9}] CHAR(13) collate database_default " +

            "alter table {0} " +
            "alter column [{10}] CHAR collate database_default " +

            "alter table {0} " +
            "alter column [{11}] VARCHAR(1000) collate database_default " +

            "alter table {0} " +
            "alter column [{12}] VARCHAR(1000) collate database_default " +

            "alter table {0} " +
            "alter column [{13}] VARCHAR(1000) collate database_default " +

            "alter table {0} " +
            "alter column [{14}] {29} collate database_default " +
            

            "alter table {0} " +
            "alter column [{16}] VARCHAR(120) collate database_default " +

            "alter table {0} " +
            "alter column [{17}] {30} collate database_default " +  //curri_id

            "alter table {0} " +
            "alter column [{18}] VARCHAR(2) collate database_default " +

            "alter table {0} " +
            "alter column [{24}] VARCHAR(64) collate database_default " +

            "alter table {0} " +
            "alter column [{25}] CHAR collate database_default " +

            "alter table {0} " +
            "alter column [{27}] CHAR collate database_default ",
            temp99tablename, Student.FieldName.USER_ID, Student.FieldName.USER_TYPE, Student.FieldName.USERNAME,
            Student.FieldName.PASSWORD, Student.FieldName.T_PRENAME, Student.FieldName.T_NAME, Student.FieldName.E_PRENAME, Student.FieldName.E_NAME,
            Student.FieldName.CITIZEN_ID, Student.FieldName.GENDER, Student.FieldName.EMAIL, Student.FieldName.TEL, Student.FieldName.ADDR, Student.FieldName.FILE_NAME_PIC,
            Student.FieldName.TIMESTAMP, Student.FieldName.STUDENT_ID,Student.FieldName.CURRI_ID,Student.FieldName.TYPE,
            Student.FieldName.ADMIS_YEAR, Student.FieldName.ADMIS_DATE,Student.FieldName.GRAD_YEAR,Student.FieldName.GRAD_SEMESTER,
            Student.FieldName.GRAD_DATE,Student.FieldName.STATUS,Student.FieldName.QUOTA,Student.FieldName.SUBTYPE,
            Student.FieldName.COOP,DBFieldDataType.USERNAME_TYPE, DBFieldDataType.FILE_NAME_TYPE,DBFieldDataType.CURRI_ID_TYPE);

            string insertintotemp99falsecase = string.Format("insert into {0} " +
            "select * from ({3}) as stres where {1} = '{2}' ",
            temp99tablename, Student.FieldName.USERNAME, username, oStudent.getSelectStudentByJoinCommand());


            string insertintotemp99_1 = insertintotemp99falsecase;


            //User_id = -1 => retrieve curri_in
            string insertintotemp99_2 = string.Format("insert into {0}({1},{2}) " +
            "select -1,{3} from (select max({1}) as {4} from {0}) as tid,{5} " +
            "where {4} = {6} ",
            temp99tablename, Student.FieldName.USER_ID, Personnel.FieldName.EMAIL,
            User_curriculum.FieldName.CURRI_ID,"std_id", User_curriculum.FieldName.TABLE_NAME,
            User_list.FieldName.USER_ID
            );

            string selectcmd = string.Format("select * from {0} ", temp99tablename);

            return string.Format(" BEGIN {0} {1} {2} {3} END ", createtabletemp99, insertintotemp99_1, insertintotemp99_2, selectcmd);
        }

        private string getSelectAlumniWithCurriculumCommand()
        {
            string temp99tablename = "#temp94";
            string createtabletemp99 = string.Format("create table {0}(" +
            "[row_num] int identity(1, 1) not null," +
            "[{1}] INT NULL," +
            "[{2}] VARCHAR(40) NULL," +
            "[{3}] {30} NULL," +
            "[{4}] VARCHAR(MAX) NULL," +
            "[{5}] VARCHAR(16) NULL," +
            "[{6}] VARCHAR(60) NULL," +
            "[{7}] VARCHAR(16) NULL," +
            "[{8}] VARCHAR(60) NULL," +
            "[{9}] CHAR(13) NULL," +
            "[{10}] CHAR NULL," +
            "[{11}] VARCHAR(1000) NULL," +
            "[{12}] VARCHAR(1000) NULL," +
            "[{13}] VARCHAR(1000) NULL," +
            "[{14}] {31} NULL," +
            "[{15}] DATETIME2 NULL," +

            "[{16}] VARCHAR(120) NOT NULL," +
            "[{17}] {32} NULL," +
            "[{18}] VARCHAR(2) NULL," +
            "[{19}] INT NULL," +
            "[{20}] DATE NULL," +
            "[{21}] INT NULL," +
            "[{22}] INT NULL," +
            "[{23}] DATE NULL," +
            "[{24}] VARCHAR(64) NULL," +
            "[{25}] CHAR NULL," +
            "[{26}] TINYINT NULL," +
            "[{27}] CHAR NULL," +
            "[{28}] VARCHAR(100) NULL," +
            "[{29}] VARCHAR(20) NULL," +
            "PRIMARY KEY([row_num])) " +

            "alter table {0} " +
            "alter column [{2}] VARCHAR(40) collate database_default " +

            "alter table {0} " +
            "alter column [{3}] {30} collate database_default " +

            "alter table {0} " +
            "alter column [{4}] VARCHAR(MAX) collate database_default " +

            "alter table {0} " +
            "alter column [{5}] VARCHAR(16) collate database_default " +

            "alter table {0} " +
            "alter column [{6}] VARCHAR(60) collate database_default " +

            "alter table {0} " +
            "alter column [{7}] VARCHAR(16) collate database_default " +

            "alter table {0} " +
            "alter column [{8}] VARCHAR(60) collate database_default " +

            "alter table {0} " +
            "alter column [{9}] CHAR(13) collate database_default " +

            "alter table {0} " +
            "alter column [{10}] CHAR collate database_default " +

            "alter table {0} " +
            "alter column [{11}] VARCHAR(1000) collate database_default " +

            "alter table {0} " +
            "alter column [{12}] VARCHAR(1000) collate database_default " +

            "alter table {0} " +
            "alter column [{13}] VARCHAR(1000) collate database_default " +

            "alter table {0} " +
            "alter column [{14}] {31} collate database_default " +


            "alter table {0} " +
            "alter column [{16}] VARCHAR(120) collate database_default " +

            "alter table {0} " +
            "alter column [{17}] {32} collate database_default " +  //curri_id

            "alter table {0} " +
            "alter column [{18}] VARCHAR(2) collate database_default " +

            "alter table {0} " +
            "alter column [{24}] VARCHAR(64) collate database_default " +

            "alter table {0} " +
            "alter column [{25}] CHAR collate database_default " +

            "alter table {0} " +
            "alter column [{27}] CHAR collate database_default " +

            "alter table {0} " +
            "alter column [{28}] VARCHAR(100) collate database_default " +

            "alter table {0} " +
            "alter column [{29}] VARCHAR(20) collate database_default ",

            temp99tablename, Alumni.FieldName.USER_ID, Alumni.FieldName.USER_TYPE, Alumni.FieldName.USERNAME,
            Alumni.FieldName.PASSWORD, Alumni.FieldName.T_PRENAME, Alumni.FieldName.T_NAME, Alumni.FieldName.E_PRENAME, Alumni.FieldName.E_NAME,
            Alumni.FieldName.CITIZEN_ID, Alumni.FieldName.GENDER, Alumni.FieldName.EMAIL, Alumni.FieldName.TEL, Alumni.FieldName.ADDR, Student.FieldName.FILE_NAME_PIC,
            Alumni.FieldName.TIMESTAMP, Alumni.FieldName.STUDENT_ID, Alumni.FieldName.CURRI_ID, Alumni.FieldName.TYPE,
            Alumni.FieldName.ADMIS_YEAR, Alumni.FieldName.ADMIS_DATE, Alumni.FieldName.GRAD_YEAR, Alumni.FieldName.GRAD_SEMESTER,
            Alumni.FieldName.GRAD_DATE, Alumni.FieldName.STATUS, Alumni.FieldName.QUOTA, Alumni.FieldName.SUBTYPE,
            Alumni.FieldName.COOP,Alumni.ExtraFieldName.COMPANY_ADDR,Alumni.ExtraFieldName.COMPANY_TEL,
            DBFieldDataType.USERNAME_TYPE, DBFieldDataType.FILE_NAME_TYPE, DBFieldDataType.CURRI_ID_TYPE);

            string insertintotemp99falsecase = string.Format("insert into {0} " +
            "select * from ({3}) as stres where {1} = '{2}' ",
            temp99tablename, Alumni.FieldName.USERNAME, username, oAlumni.getSelectAlumniByJoinCommand());


            string insertintotemp99_1 = insertintotemp99falsecase;


            //User_id = -1 => retrieve curri_in
            string insertintotemp99_2 = string.Format("insert into {0}({1},{2}) " +
            "select -1,{3} from (select max({1}) as {4} from {0}) as tid,{5} " +
            "where {4} = {6} ",
            temp99tablename, Student.FieldName.USER_ID, Personnel.FieldName.EMAIL,
            User_curriculum.FieldName.CURRI_ID, "alum_id", User_curriculum.FieldName.TABLE_NAME,
            User_list.FieldName.USER_ID
            );

            string selectcmd = string.Format("select * from {0} ", temp99tablename);

            return string.Format(" BEGIN {0} {1} {2} {3} END ", createtabletemp99, insertintotemp99_1, insertintotemp99_2, selectcmd);
        }

        private string getSelectPrivilegeCommand(int user_id,string tablename)
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

        private string getSelectUserDataCommand(int user_id,int usrtype,string tablename)
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
            if (usrtype == 0 || usrtype == 1)
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

            //7 select not_send_primary (teacher only ? evid_curri_id, aca_year, evidence_name) => selectnotsendprimary
            string selectnotsendprimary = "";
            if (usrtype == 0)
                selectnotsendprimary = string.Format("select {0}.{1} as evid_curri_id,{2},{3} " +
                "from {0},{4} " +
                "where {4}.{5} = {0}.{6} " +
                "and {7} = {8} and ({9} = '0' or {9} = '4') ",
                Primary_evidence_status.FieldName.TABLE_NAME, Primary_evidence_status.FieldName.CURRI_ID,
                Primary_evidence.FieldName.ACA_YEAR, Primary_evidence.FieldName.EVIDENCE_NAME,
                Primary_evidence.FieldName.TABLE_NAME, Primary_evidence.FieldName.PRIMARY_EVIDENCE_NUM,
                Primary_evidence_status.FieldName.PRIMARY_EVIDENCE_NUM,
                Primary_evidence_status.FieldName.TEACHER_ID, user_id,
                Primary_evidence_status.FieldName.STATUS);


            //8 select privilege (use predefined select from temp table cmd) => selectprivilege
            string selectprivilege = getSelectPrivilegeCommand(user_id,tablename);



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
            string selectcmd = string.Format(
                               "if exists(select * from ({0}) as tsres where {1} = '{2}') " +
                                getSelectTeacherWithCurriculumCommand() +
                                "else if exists(select * from ({3}) as stres where {1} = '{2}') " +
                                getSelectStaffWithCurriculumCommand() +
                                "else if exists(select * from ({4}) as std where {1} = '{2}') " +
                                getSelectStudentWithCurriculumCommand() +
                                "else if exists(select * from ({5}) as alum where {1} = '{2}') " +
                                getSelectAlumniWithCurriculumCommand() +
                                "else if exists(select * from ({6}) as comp where {1} = '{2}') " +
                                    getSelectCompanyWithCurriculumCommand() +
                                "else if exists(select * from ({7}) as assres where {1} = '{2}') " +
                                    getSelectAssessorWithCurriculumCommand() +
                                "else if exists(select * from ({8}) as admres where {1} = '{2}') " +
                                    "select * from ({8}) as admres where {1} = '{2}' ",
                               oTeacher.getSelectTeacherByJoinCommand(), Teacher.FieldName.USERNAME, preferredusername,
                               oStaff.getSelectStaffByJoinCommand(),oStudent.getSelectStudentByJoinCommand(),oAlumni.getSelectAlumniByJoinCommand(),
                               oCompany.getSelectCompanyByJoinCommand(),oAssessor.getSelectAssessorByJoinCommand(),oAdmin.getSelectAdminByJoinCommand()
                               );

            d.iCommand.CommandText = selectcmd;
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    int mode = -999;
                    foreach (DataRow item in data.Rows)
                    {
                        
                        string usrtype = item.ItemArray[data.Columns[User_list.FieldName.USER_TYPE].Ordinal].ToString();

                        //Admin didn't have any extra data (except creator and date which account was created.)
                        //So mode read is not necessery.
                        //column 1 = identifier for each user
                        if (usrtype != "ผู้ดูแลระบบ")
                            mode = Convert.ToInt32(item.ItemArray[data.Columns[1].Ordinal]);
                        //USER_TYPE != null mean that row have main data (main info + eduhistory)
                        if (usrtype != "")
                        {
                            //result.username == null means the first loop round (no data are read)
                            //Read main data if that condition is TRUE 
                            
                            if (result.username == null)
                            {
                                //MAIN INFORMRATION
                                if (usrtype == "อาจารย์")
                                    result.user_id = Convert.ToInt32(item.ItemArray[data.Columns[Teacher.FieldName.TEACHER_ID].Ordinal]);
                                else if (usrtype == "เจ้าหน้าที่")
                                    result.user_id = Convert.ToInt32(item.ItemArray[data.Columns[Staff.FieldName.STAFF_ID].Ordinal]);
                                else if (usrtype == "นักศึกษา" || usrtype == "ศิษย์เก่า")
                                    result.user_id = Convert.ToInt32(item.ItemArray[data.Columns[Student.FieldName.USER_ID].Ordinal]);
                                else if (usrtype == "บริษัท")
                                    result.user_id = Convert.ToInt32(item.ItemArray[data.Columns[Company.FieldName.COMPANY_ID].Ordinal]);
                                else if (usrtype == "ผู้ประเมินจากภายนอก")
                                    result.user_id = Convert.ToInt32(item.ItemArray[data.Columns[Assessor.FieldName.ASSESSOR_ID].Ordinal]);
                                else
                                    result.user_id = Convert.ToInt32(item.ItemArray[data.Columns[Admin.FieldName.ADMIN_ID].Ordinal]);


                                result.username = item.ItemArray[data.Columns[Teacher.FieldName.USERNAME].Ordinal].ToString();
                                result.user_type = usrtype;
                                //**********************************************

                                result.information.addr = item.ItemArray[data.Columns[Teacher.FieldName.ADDR].Ordinal].ToString();
                                result.information.citizen_id = item.ItemArray[data.Columns[Teacher.FieldName.CITIZEN_ID].Ordinal].ToString();
                                result.information.email = item.ItemArray[data.Columns[Teacher.FieldName.EMAIL].Ordinal].ToString();
                                result.information.tel = item.ItemArray[data.Columns[Teacher.FieldName.TEL].Ordinal].ToString();
                                result.information.gender = item.ItemArray[data.Columns[Teacher.FieldName.GENDER].Ordinal].ToString() != "" ? Convert.ToChar(item.ItemArray[data.Columns[Teacher.FieldName.GENDER].Ordinal]) : ' ';
                                result.information.file_name_pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[data.Columns[Teacher.FieldName.FILE_NAME_PIC].Ordinal].ToString());
                                result.information.timestamp = item.ItemArray[data.Columns[Teacher.FieldName.TIMESTAMP].Ordinal].ToString();
                                result.information.e_name = item.ItemArray[data.Columns[Teacher.FieldName.E_NAME].Ordinal].ToString();
                                result.information.e_prename = item.ItemArray[data.Columns[Teacher.FieldName.E_PRENAME].Ordinal].ToString();
                                result.information.t_name = item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString();
                                result.information.t_prename = item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString();
                                result.information.SetPassword(item.ItemArray[data.Columns[Teacher.FieldName.PASSWORD].Ordinal].ToString());

                                if (usrtype == "อาจารย์")
                                {
                                    result.information.degree = item.ItemArray[data.Columns[Teacher.FieldName.DEGREE].Ordinal].ToString() != "" ? Convert.ToChar(item.ItemArray[data.Columns[Teacher.FieldName.DEGREE].Ordinal]) : ' ';
                                    result.information.position = item.ItemArray[data.Columns[Teacher.FieldName.POSITION].Ordinal].ToString() != "" ? Convert.ToChar(item.ItemArray[data.Columns[Teacher.FieldName.POSITION].Ordinal]) : ' ';
                                    result.information.personnel_type = item.ItemArray[data.Columns[Teacher.FieldName.PERSONNEL_TYPE].Ordinal].ToString();
                                    result.information.person_id = item.ItemArray[data.Columns[Teacher.FieldName.PERSON_ID].Ordinal].ToString();
                                    result.information.room = item.ItemArray[data.Columns[Teacher.FieldName.ROOM].Ordinal].ToString();
                                    result.information.status = item.ItemArray[data.Columns[Teacher.FieldName.STATUS].Ordinal].ToString();
                                    result.information.alive = item.ItemArray[data.Columns[Teacher.FieldName.POSITION].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns[Teacher.FieldName.ALIVE].Ordinal]) : -1;
                                }
                                else if (usrtype == "เจ้าหน้าที่")
                                {
                                    result.information.room = item.ItemArray[data.Columns[Staff.FieldName.ROOM].Ordinal].ToString();
                                }
                                else if (usrtype == "บริษัท")
                                {
                                    result.information.company_name = item.ItemArray[data.Columns[Company.FieldName.COMPANY_NAME].Ordinal].ToString();
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
                            //read secondary data such as educational history (if exists)
                            if (usrtype == "อาจารย์" || usrtype == "เจ้าหน้าที่")
                            {
                                //if edudeg column value is not null => ADD IT
                                if(item.ItemArray[data.Columns["edudeg"].Ordinal].ToString() != "")
                                {
                                    result.information.education.Add(new Educational_teacher_staff
                                    {
                                        college = item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.COLLEGE].Ordinal].ToString(),
                                        degree = Convert.ToChar(item.ItemArray[data.Columns["edudeg"].Ordinal].ToString()),
                                        grad_year = item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.GRAD_YEAR].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.GRAD_YEAR].Ordinal]) : 0,
                                        major = item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.MAJOR].Ordinal].ToString(),
                                        personnel_id = result.user_id,
                                        education_id = Convert.ToInt32(item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.EDUCATION_ID].Ordinal]),
                                        pre_major = item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.PRE_MAJOR].Ordinal].ToString()
                                    });
                                }
                            }
                        }
                        //User_id = -1 => retrieve curri_in

                        //User_id = -2 => retrieve president_in


                        //User_id = -3 => retrieve topic_interested

                        //User_id = -4 => retrieve committee_in
                        else if (mode == -1)
                        {
                            //Read ternary data such as curriculum which personnel is in
                            //major column contain curri_id value
                            result.curri_id_in.Add(item.ItemArray[data.Columns[Personnel.FieldName.EMAIL].Ordinal].ToString());
                        }
                        else if(mode == -3)
                        {
                            //Read 5th data such as topic interested which teacher is interest
                            //major column contain topic_interest value
                            result.information.interest.Add(item.ItemArray[data.Columns[Personnel.FieldName.EMAIL].Ordinal].ToString());
                        }
                        else if(mode == -2)
                        {
                            //Read 4th data such as which curriculum+year that the login teacher is president 
                            //major and grad_year column contain curri_id and aca_year value
                            if (result.president_in == null)
                                result.president_in = new Dictionary<string, List<int>>();
                            string curri_id = item.ItemArray[data.Columns[Personnel.FieldName.EMAIL].Ordinal].ToString();
                            if (!result.president_in.ContainsKey(curri_id))
                            {
                                result.president_in.Add(curri_id, new List<int>());
                            }
                            result.president_in[curri_id].Add(Convert.ToInt32(item.ItemArray[data.Columns[Personnel.FieldName.TEL].Ordinal]));
                        }
                        else
                        {
                            //Read 6th data such as which curriculum+year that the login teacher is committee
                            //major and grad_year column contain curri_id and aca_year value
                            if (result.committee_in == null)
                                result.committee_in = new Dictionary<string, List<int>>();
                            string curri_id = item.ItemArray[data.Columns[Personnel.FieldName.EMAIL].Ordinal].ToString();
                            if (!result.committee_in.ContainsKey(curri_id))
                            {
                                result.committee_in.Add(curri_id, new List<int>());
                            }
                            result.committee_in[curri_id].Add(Convert.ToInt32(item.ItemArray[data.Columns[Personnel.FieldName.TEL].Ordinal]));
                        }
                    }
                    data.Dispose();
                }
                else
                {
                    return "ไม่พบชื่อผู้ใช้งานนี้ในระบบ";
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

        public object SelectUserPrivilege(ref User_information_with_privilege_information userdata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";

            d.iCommand.CommandText = getSelectPrivilegeCommand(userdata.user_id,"#temp5");

            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        string curri_id = item.ItemArray[data.Columns[User_curriculum.FieldName.CURRI_ID].Ordinal].ToString();

                        
                        if (Convert.ToInt32(item.ItemArray[data.Columns["privilege_type"].Ordinal]) == 1)
                        {
                            //Add normal privilege
                            if (!userdata.privilege.ContainsKey(curri_id))
                            {

                                userdata.privilege.Add(curri_id, new Dictionary<int, int>());
                            }
                            userdata.privilege[curri_id][Convert.ToInt32(item.ItemArray[data.Columns[Extra_privilege.FieldName.TITLE_CODE].Ordinal])] = Convert.ToInt32(item.ItemArray[data.Columns[Extra_privilege.FieldName.TITLE_PRIVILEGE_CODE].Ordinal]);
                        }
                        else
                        {
                            //Add committee privilege
                            if (userdata.committee_privilege == null)
                                userdata.committee_privilege = new Dictionary<string, Dictionary<int, int>>();
                            if (!userdata.committee_privilege.ContainsKey(curri_id))
                            {

                                userdata.committee_privilege.Add(curri_id, new Dictionary<int, int>());
                            }
                            userdata.committee_privilege[curri_id][Convert.ToInt32(item.ItemArray[data.Columns[Extra_privilege.FieldName.TITLE_CODE].Ordinal])] = Convert.ToInt32(item.ItemArray[data.Columns[Extra_privilege.FieldName.TITLE_PRIVILEGE_CODE].Ordinal]);
                        }

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
            return null;
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
            getSelectUserDataCommand(usrid, 0,"#temp99"), Staff.FieldName.TABLE_NAME, Staff.FieldName.STAFF_ID,
            getSelectUserDataCommand(usrid, 1,"#temp98"), Student.FieldName.TABLE_NAME, Student.FieldName.USER_ID,
            getSelectUserDataCommand(usrid, 2,"#temp97"), Alumni.ExtraFieldName.TABLE_NAME, Alumni.FieldName.USER_ID,
            getSelectUserDataCommand(usrid, 3,"#temp96"), Company.FieldName.TABLE_NAME, Company.FieldName.COMPANY_ID,
            getSelectUserDataCommand(usrid, 4,"#temp95"), Assessor.FieldName.TABLE_NAME, Assessor.FieldName.ASSESSOR_ID,
            getSelectUserDataCommand(usrid, 5,"#temp94"), Admin.FieldName.TABLE_NAME, Admin.FieldName.ADMIN_ID,
            getSelectUserDataCommand(usrid, 6,"#temp93"));

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
                                //2 retrieve education data(teacher and staff only)
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
                                //7 retrieve not_send_primary (teacher only ? evid_curri_id, aca_year, evidence_name)
                                if (result.not_send_primary == null)
                                    result.not_send_primary = new List<Evidence_brief_detail>();
                                result.not_send_primary.Add(new Evidence_brief_detail
                                {
                                    curri_id = item.ItemArray[tabledata.Columns["evid_curri_id"].Ordinal].ToString(),
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
                "{9} = '{10}', {11} = '{12}', {13} = '{14}' where {15} = {16} ",
                User_list.FieldName.TABLE_NAME, Teacher.FieldName.T_PRENAME, userdata.information.t_prename,
                Teacher.FieldName.T_NAME, userdata.information.t_name,
                Teacher.FieldName.E_PRENAME, userdata.information.e_prename,
                Teacher.FieldName.E_NAME, userdata.information.e_name,
                Teacher.FieldName.EMAIL, userdata.information.email,
                Teacher.FieldName.TEL, userdata.information.tel,
                Teacher.FieldName.ADDR, userdata.information.addr,
                User_list.FieldName.USER_ID, userdata.user_id);
            }
            else
            {
                mainupdatecmd = string.Format("insert into {19} " +
                "select * from " +
                "(update {0} set {1} = '{2}', {3} = '{4}', {5} = '{6}', {7} = '{8}'," +
                "{9} = '{10}', {11} = '{12}', {13} = '{14}',{15} = '{16}' output deleted.{15} where {17} = {18}) as outputupdate ",
                User_list.FieldName.TABLE_NAME, Teacher.FieldName.T_PRENAME, userdata.information.t_prename,
                Teacher.FieldName.T_NAME, userdata.information.t_name,
                Teacher.FieldName.E_PRENAME, userdata.information.e_prename,
                Teacher.FieldName.E_NAME, userdata.information.e_name,
                Teacher.FieldName.EMAIL, userdata.information.email,
                Teacher.FieldName.TEL, userdata.information.tel,
                Teacher.FieldName.ADDR, userdata.information.addr,
                Teacher.FieldName.FILE_NAME_PIC, userdata.information.file_name_pic,
                User_list.FieldName.USER_ID, userdata.user_id,temp80tablename);
            }

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

            if (userdata.user_type == "อาจารย์" || userdata.user_type == "เจ้าหน้าที่")
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
                selectuserdatacmd = getSelectUserDataCommand(userdata.user_id, 0, "#temp99");
            else if (userdata.user_type == "เจ้าหน้าที่")
                selectuserdatacmd = getSelectUserDataCommand(userdata.user_id, 1, "#temp98");
            else if (userdata.user_type == "นักศึกษา")
                selectuserdatacmd = getSelectUserDataCommand(userdata.user_id, 2, "#temp97");
            else if (userdata.user_type == "ศิษย์เก่า")
                selectuserdatacmd = getSelectUserDataCommand(userdata.user_id, 3, "#temp96");
            else if (userdata.user_type == "บริษัท")
                selectuserdatacmd = getSelectUserDataCommand(userdata.user_id, 4, "#temp95");
            else if (userdata.user_type == "ผู้ประเมินจากภายนอก")
                selectuserdatacmd = getSelectUserDataCommand(userdata.user_id, 5, "#temp94");
            else if (userdata.user_type == "ผู้ดูแลระบบ")
                selectuserdatacmd = getSelectUserDataCommand(userdata.user_id, 6, "#temp93");

            string selectfiletodelcmd = string.Format("select * from {0} ", temp80tablename);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} {6} {7} END ",
                createtabletemp80, mainupdatecmd, updateteachertable, deletefromtechin, insertintotechin,
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
                                //2 retrieve education data(teacher and staff only)
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
                                //7 retrieve not_send_primary (teacher only ? evid_curri_id, aca_year, evidence_name)
                                if (result.not_send_primary == null)
                                    result.not_send_primary = new List<Evidence_brief_detail>();
                                result.not_send_primary.Add(new Evidence_brief_detail
                                {
                                    curri_id = item.ItemArray[tabledata.Columns["evid_curri_id"].Ordinal].ToString(),
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


