using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Staff_with_t_name : Personnel_with_t_name
    {
        public string staff_id { get { return GetPersonnelId(); } set { SetPersonnelId(value); } }
    }
}