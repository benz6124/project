using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class User_information_with_privilege_information
    {
        private string _user_id;
        private string _username;
        private string _user_type;
        private User_information _information;

        private List<Privilege_by_curriculum> _privilege;
        public string user_id { get { return _user_id; } set { _user_id = value; } }
        public string username { get { return _username; } set { _username = value; } }
        public string user_type { get { return _user_type; } set { _user_type = value; } }
        public User_information information { get { return _information; } set { _information = value; } }
        public List<Privilege_by_curriculum> privilege { get { return _privilege; } set { _privilege = value; } }
        public User_information_with_privilege_information()
        {
            information = new User_information();
            privilege = new List<Privilege_by_curriculum>();
        }
    }
}