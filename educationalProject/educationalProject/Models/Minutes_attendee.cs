using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Minutes_attendee
    {
        internal struct FieldName
        {
            public static readonly string MINUTES_ID = "MINUTES_ID";
            public static readonly string TEACHER_ID = "TEACHER_ID";
            public static readonly string TABLE_NAME = "MINUTES_ATTENDEE";
        }
        private int _minutes_id;
        private int _teacher_id;
        public int minutes_id { get { return _minutes_id; } set { _minutes_id = value; } }
        public int teacher_id { get { return _teacher_id; } set { _teacher_id = value; } }
    }
}