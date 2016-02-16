using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class Technical_interested
    {
        internal struct FieldName
        {
            public static readonly string TEACHER_ID = "TEACHER_ID";
            public static readonly string TOPIC_INTERESTED = "TOPIC_INTERESTED";
            public static readonly string TABLE_NAME = "TECHNICAL_INTERESTED";
        }
        private int _teacher_id;
        private string _topic_interested;
        public int teacher_id { get { return _teacher_id; } set { _teacher_id = value; } }
        public string topic_interested { get { return _topic_interested; } set { _topic_interested = value; } }
    }
}