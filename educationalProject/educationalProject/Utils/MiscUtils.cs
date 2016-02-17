using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Utils
{
    public class MiscUtils
    {
        public static string GatherProfilePicturePath(string path)
        {
            return path != "" ? path : "myimages/profile_pic/nopic.jpg";
        }
    }
}