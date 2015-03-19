using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;

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

        

/// 压缩zip
        /// </summary>
        /// <param name="fileToZips">文件路径集合</param>
        /// <param name="zipedFile">想要压成zip的文件名</param>
        public static void zip(string dir, string zipedFile)
        {
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(zipedFile + ".zip", Encoding.Default))
            {
                zip.AddDirectory(dir, new FileInfo(dir).Name);
                zip.Save();
            }
        }

        public static void unZip(string zipFile, string outputdir)
        {
            ReadOptions options = new ReadOptions();
            options.Encoding = Encoding.Default;
            using (ZipFile zip = ZipFile.Read(zipFile, options))
            {
                foreach (ZipEntry z in zip)
                {
                    FileInfo f = new FileInfo(outputdir + "/" + z.FileName);
                    if (f.Exists)
                    {
                        string parent = f.Directory.Name;
                        string name = f.Name.Replace(f.Extension, "");
                        string str = parent + "|" + name;
                        //if (MessageBox.Show("文件(" + str + ")已经存在，是否替换？", "确认", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        //{
                        //    f.Delete();
                        //}
                        //else
                        //{
                        //    continue;
                        //}
                    }
                    z.Extract(outputdir);
                }
            }

        }

    }
}
