using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using educationalProject.Models.Wrappers;
namespace educationalProject.Models.ViewModels
{
    public class BulkEvidenceTransactionResult
    {
        private List<Evidence_with_t_name> _mainresult;
        private List<string> _filenametodellist;
        private string _message;

        public List<Evidence_with_t_name> mainresult { get { return _mainresult; } set { _mainresult = value; } }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> filenametodellist { get { return _filenametodellist; } set { _filenametodellist = value; } }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string message { get { return _message; } set { _message = value; } }

        public BulkEvidenceTransactionResult()
        {
            message = null;
        }
    }
}