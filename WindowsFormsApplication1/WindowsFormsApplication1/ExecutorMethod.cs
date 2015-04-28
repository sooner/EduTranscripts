using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace HKreporter
{
    public class ExecutorMethod
    {
        public List<DataRow> SearchIndex = new List<DataRow>();
        public List<long> NotFoundIndex = new List<long>();
        Excel_process ans;
        Excel_process group;
        HK_database db;
        WordCreator wc;
        Calculate cal;
        public void pre_process(Form1 form)
        {
            try
            {
                ans = new Excel_process(Utils.standard_adr);
                ans.run(true);
                form.ShowPro(10, 1); ;
                group = new Excel_process(Utils.groups_adr);
                group.run(false);
                form.ShowPro(20, 2);
                db = new HK_database(form, ans.dt, group.dt);
                db.DBF_data_process(Utils.dbf_adr);
                db._basic_data.PrimaryKey = new DataColumn[] {db._basic_data.Columns["studentid"] };
                form.ShowPro(50, 3);
                if (Utils.stu_id_end.Equals("0"))
                {
                    if (FindRow(Utils.stu_id_start, db._group_data) == null)
                        form.CheckStuID(2, "找不到该ID学生成绩！");
                }
                else
                {
                    FindRows(Utils.stu_id_start, Utils.stu_id_end, db._group_data);
                    if (NotFoundIndex.Count > 0 && SearchIndex.Count != 0)
                    {
                        StringBuilder sb = new StringBuilder("找不到学号为:");
                        foreach (long id in NotFoundIndex)
                            sb.Append(id.ToString() + " ");
                        sb.Append("的学生");
                        form.CheckStuID(1, sb.ToString());
                    }
                    else if (SearchIndex.Count == 0)
                    {
                        form.CheckStuID(2, "找不到该学号段的学生成绩！");
                    }
                }

                cal = new Calculate();
                cal.execute(db.newStandard, group.dt, db._group_data);
                form.ShowPro(80, 4);
                wc = new WordCreator();
                if (Utils.stu_id_end.Equals("0"))
                {
                    DataRow dr = FindRow(Utils.stu_id_start, cal._data);
                    DataRow basic_dr = FindRow(Utils.stu_id_start, db._basic_data);
                    wc.create_word(cal.total, dr, group.dt, group.groups_group, basic_dr);
                }
                else
                {
                    FindRows(Utils.stu_id_start, Utils.stu_id_end, cal._data);
                    
                    if (Utils.save_adr[Utils.save_adr.Length - 1] != Path.DirectorySeparatorChar)
                        Utils.save_adr += Path.DirectorySeparatorChar;
                    
                    
                    Utils.save_adr += Utils.zipname;
                    Utils.save_adr += Path.DirectorySeparatorChar;
                    if (!Directory.Exists(Utils.save_adr))
                        Directory.CreateDirectory(Utils.save_adr);
                    foreach (DataRow dr in SearchIndex)
                    {
                        DataRow basic_dr = FindRow(dr["studentid"].ToString().Trim(), db._basic_data);
                        wc.create_word(cal.total, dr, group.dt, group.groups_group, basic_dr);
                    }
                    Utils.zip(Utils.save_adr, Utils.zipname);
                }
            }
            catch (System.Threading.ThreadAbortException e)
            {
            }
            catch (Exception e)
            {
                form.CheckStuID(2, e.Message.ToString());
            }
        }
        public DataRow FindRow(string stu_id, DataTable dt)
        {
            return dt.Rows.Find(stu_id);
        }
        public void FindRows(string stu_id_start, string stu_id_stop, DataTable dt)
        {
            char[] idChar = stu_id_start.Trim().ToCharArray();
            int size = idChar.Length;
            long start = Convert.ToInt64(stu_id_start);
            long stop = Convert.ToInt64(stu_id_stop);

            if (start > stop)
                throw new System.ArgumentException("起始学生ID不能大于终止学生ID!");

            SearchIndex.Clear();
            NotFoundIndex.Clear();

            for (long i = start; i <= stop; i++)
            {
                char[] temp = i.ToString().ToCharArray();
                StringBuilder sb = new StringBuilder(i.ToString());

                if(temp.Length < size)
                {
                    for(int j = temp.Length; j < size; j++)
                        sb.Insert(0, "0");
                }
                DataRow dr = FindRow(sb.ToString(), dt);
                if (dr == null)
                    NotFoundIndex.Add(i);
                else
                    SearchIndex.Add(dr);

            }
        }
    }
}
