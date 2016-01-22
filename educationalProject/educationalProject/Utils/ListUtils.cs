using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Utils
{
    public class ListUtils<T>
    {
        public static void Swap(List<T> source,int index1,int index2)
        {
            T temp = source[index1];
            source[index1] = source[index2];
            source[index2] = temp;
        }
    }
}