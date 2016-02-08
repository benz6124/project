using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oTitle_privilege : Title_privilege
    {
        public static string getSelectTitlePrivilegeCommand()
        {
            return string.Format("select {0}.*, {1}, {2} from {0}, {3} where {0}.{4} = {3}.{5}",
                    Title.FieldName.TABLE_NAME, FieldName.TITLE_PRIVILEGE_CODE, FieldName.PRIVILEGE,
                    FieldName.TABLE_NAME, Title.FieldName.TITLE_CODE, FieldName.TITLE_CODE);
        }
    }
}