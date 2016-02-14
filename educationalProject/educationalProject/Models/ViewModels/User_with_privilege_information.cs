using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
namespace educationalProject.Models.ViewModels
{
    public class User_information_with_privilege_information
    {
        private int _user_id;
        private string _username;
        private string _user_type;
        private User_information _information;

        private Dictionary<string, Dictionary<int, int>> _privilege;
        private Dictionary<string, List<int>> _president_in;
        private Dictionary<string, List<int>> _committee_in;
        private List<string> _curri_id_in;
        public int user_id { get { return _user_id; } set { _user_id = value; } }
        public string username { get { return _username; } set { _username = value; } }
        public string user_type { get { return _user_type; } set { _user_type = value; } }
        public User_information information { get { return _information; } set { _information = value; } }
        public Dictionary<string, Dictionary<int, int>> privilege { get { return _privilege; } set { _privilege = value; } }
        public List<string> curri_id_in { get { return _curri_id_in; } set { _curri_id_in = value; } }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, List<int>> president_in { get { return _president_in; } set { _president_in = value; } }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, List<int>> committee_in { get { return _committee_in; } set { _committee_in = value; } }
        public User_information_with_privilege_information()
        {
            information = new User_information();
            privilege = new Dictionary<string, Dictionary<int, int>>();
            curri_id_in = new List<string>();
            president_in = null;
            committee_in = null;
        }
    }
}