using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class User_information
    {
        private string _password;
        private string _t_prename;
        private string _t_name;
        private string _e_prename;
        private string _e_name;
        private string _citizen_id;
        private char _gender;
        private string _email;
        private string _tel;
        private string _addr;
        private string _file_name_pic;
        private string _timestamp;

        public string t_prename { get { return _t_prename; } set { _t_prename = value; } }
        public string t_name { get { return _t_name; } set { _t_name = value; } }
        public string e_prename { get { return _e_prename; } set { _e_prename = value; } }
        public string e_name { get { return _e_name; } set { _e_name = value; } }
        public string citizen_id { get { return _citizen_id; } set { _citizen_id = value; } }
        public char gender { get { return _gender; } set { _gender = value; } }
        public string email { get { return _email; } set { _email = value; } }
        public string tel { get { return _tel; } set { _tel = value; } }
        public string addr { get { return _addr; } set { _addr = value; } }
        public string file_name_pic { get { return _file_name_pic; } set { _file_name_pic = value; } }
        public string timestamp { get { return _timestamp; } set { _timestamp = value; } }

        //For teacher
        private char _degree;
        private char _position;
        private string _personnel_type;
        private string _person_id;
        private string _room;
        private string _status;
        private int _alive;
        public char degree { get { return _degree; } set { _degree = value; } }
        public char position { get { return _position; } set { _position = value; } }
        public string personnel_type { get { return _personnel_type; } set { _personnel_type = value; } }
        public string person_id { get { return _person_id; } set { _person_id = value; } }
        public string status { get { return _status; } set { _status = value; } }
        public string room { get { return _room; } set { _room = value; } }
        public int alive { get { return _alive; } set { _alive = value; } }

        //Common for teacher and staff

        private List<Educational_teacher_staff> _education;
        public List<Educational_teacher_staff> education { get { return _education; } set { _education = value; } }

        public User_information()
        {
            education = new List<Educational_teacher_staff>();
        }
        public string GetPassword()
        {
            return _password;
        }
        public void SetPassword(string value)
        {
            _password = value;
        }
    }
}