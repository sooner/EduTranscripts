using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKreporter
{
    public static class Utils
    {
        public static string save_adr;
        public static string subject;
        public static string date;
        public static string dbf_adr;
        public static string standard_adr;
        public static string groups_adr;
        public static decimal excellent;
        public static decimal well;
        public static decimal pass;
        public static string stu_id_start;
        public static string stu_id_end;
        public static decimal fullmark;
        public static string CurrentDirectory = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
        public static bool isVisible = false;
    }
}
