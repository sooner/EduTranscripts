using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace HKreporter
{
    class HK_database
    {
        public DataTable _groups;
        public int _group_num;
        public decimal _divider;
        public DataTable _standard_ans;
        public DataTable _basic_data;
        public DataTable _group_data;

        public DataTable newStandard;


        string filePath;
        string file;
        string path;
        string filename;
        string filext;
        Form1 _form;

        List<List<string>> name_list;
        OleDbConnection dbfConnection;

        public HK_database(Form1 form, DataTable standard_ans, DataTable groups)
        {
            _form = form;
            _groups = groups;
            _standard_ans = standard_ans;
            try
            {
                _standard_ans.PrimaryKey = new DataColumn[] { _standard_ans.Columns["th"] };
            }
            catch (Exception e)
            {
                if (e.Message.ToString().Equals("These columns don't currently have unique values."))
                {
                    List<string> result = standard_ans.AsEnumerable().GroupBy(c => c.Field<string>("th")).Select(c => new
                    {
                        th = c.Key,
                        count = c.Count()
                    }).Where(c => c.count > 1).Select(c => c.th).ToList();

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < result.Count; )
                    {
                        sb.Append(result[i]);
                        i++;
                        if (i != result.Count)
                            sb.Append(" ");
                    }
                    throw new ArgumentException("标准答案中" + sb.ToString() + "重复！");
                }
                else
                    throw e;
            }
            _basic_data = new DataTable();
            _group_data = new DataTable();

            name_list = new List<List<string>>();

        }

        public string DBF_data_process(string fileadd)
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            filePath = @fileadd;
            file = System.IO.Path.GetFileName(filePath);
            path = System.IO.Path.GetDirectoryName(filePath);
            filename = System.IO.Path.GetFileNameWithoutExtension(filePath);
            filext = System.IO.Path.GetExtension(filePath);

            string conn = @"Provider=vfpoledb;Data Source=" + path + ";Collating Sequence=machine;";
            Regex topic = new Regex("^[Tt]\\d+$");
            dbfConnection = new OleDbConnection(conn);


            OleDbDataAdapter adpt = new OleDbDataAdapter("select * from " + file + " where Qk<>'Q'", dbfConnection);
            DataSet mySet = new DataSet();
            try
            {
                adpt.Fill(mySet);
            }
            catch (OleDbException e)
            {
                throw new Exception("数据库文件被占用，请关闭！");
            }
            dbfConnection.Close();
            //form.ShowPro(15, 2);
            if (mySet.Tables.Count > 1)
                return "more than 1 tables";
            DataTable dt = mySet.Tables[0];
            int count = dt.Columns.Count;
            try
            {
                dt.PrimaryKey = new DataColumn[] { dt.Columns["zkzh"] };
            }
            catch (Exception e)
            {
                if (e.Message.ToString().Equals("These columns don't currently have unique values."))
                {
                    List<string> result = dt.AsEnumerable().GroupBy(c => c.Field<string>("zkzh")).Select(c => new
                    {
                        th = c.Key,
                        count = c.Count()
                    }).Where(c => c.count > 1).Select(c => c.th).ToList();

                    StringBuilder sb = new StringBuilder();
                    for (int j = 0; j < result.Count; )
                    {
                        sb.Append(result[j]);
                        j++;
                        if (j != result.Count)
                            sb.Append(" ");
                    }
                    _form.CheckStuID(3, "数据库文件中准考证号为" + sb.ToString() + "重复！");
                    foreach (string id in result)
                    {
                        var rows = dt.Select("zkzh = '" + id + "'");
                        bool temp_first = true;
                        foreach (var row in rows)
                        {
                            if (temp_first)
                            {
                                temp_first = false;
                                continue;
                            }
                            else
                                row.Delete();
                            
                        }

                    }

                    dt.AcceptChanges();
                }
                else
                    throw e;
            }
            int i;
            DataTable basic_data = new DataTable();
            basic_data.Columns.Add("studentid", System.Type.GetType("System.String"));
            basic_data.Columns.Add("schoolcode", System.Type.GetType("System.String"));
            basic_data.Columns.Add("studentname", typeof(string));
            basic_data.Columns.Add("schoolname", typeof(string));
            basic_data.Columns.Add("totalmark", typeof(decimal));
            //for (i = 0; i < _standard_ans.Rows.Count; i++)
            //    basic_data.Columns.Add("T" + ((string)_standard_ans.Rows[i]["th"]).Trim(), System.Type.GetType("System.Decimal"));
            //for (i = 1; i <= 66; i++)
            //    basic_data.Columns.Add("T" + i.ToString().Trim(), typeof(decimal));
            bool first = true;

            string omrstr = dt.Columns.Contains("Ttxx") ? "Ttxx" : "Info";

            newStandard = StandardAnsRecontruction(_standard_ans, name_list);

            foreach (DataRow dr in dt.Rows)
            {
                string an = (string)dr[omrstr];
                char[] ans = an.Trim().ToCharArray();

                string mark_string = dr["xf"].ToString().Trim();

                string[] single_mark = check_remark(mark_string.ToCharArray()).Split(' ');

                if (single_mark.Length != _standard_ans.Rows.Count)
                    throw new ArgumentException("标准答案题目数量和数据文件题目数量不一致");



                if (first)
                {

                    for (i = 0; i < newStandard.Rows.Count; i++)
                    {
                        try
                        {
                            basic_data.Columns.Add("T" + newStandard.Rows[i]["th"].ToString().Trim(), typeof(decimal));
                        }
                        catch (DuplicateNameException e)
                        {
                            throw new System.ArgumentException("标准答案题号“" + newStandard.Rows[i]["th"].ToString().Trim() + "”重复");
                        }
                    }
                    int single_count = 0;
                    for (i = 0; i < newStandard.Rows.Count; i++)
                    {
                        if (!newStandard.Rows[i]["da"].ToString().Trim().Equals(""))
                        {
                            basic_data.Columns.Add("D" + newStandard.Rows[i]["th"].ToString().Trim(), typeof(string));
                            single_count++;
                        }
                    }
                    if (single_count != ans.Length)
                        throw new ArgumentException("选择题答案与数据库文件答案数量不一致");
                    first = false;
                    basic_data.Columns.Add("Groups", typeof(string));
                    basic_data.Columns.Add("QX", typeof(string));
                    basic_data.Columns.Add("rank", typeof(string));

                    
                }

                DataRow newRow = basic_data.NewRow();
                newRow["studentid"] = dr["zkzh"].ToString().Trim();
                newRow["schoolcode"] = dr["xxdm"].ToString().Trim();
                newRow["studentname"] = dr["xm"].ToString().Trim();
                newRow["schoolname"] = dr["xxmc"].ToString().Trim();
                newRow["totalmark"] = 0m;
                decimal obj_mark = 0;
                decimal sub_mark = 0;
                int obj_count = 0, total_count = 0, org_total = 0;

                foreach (DataRow ans_dr in newStandard.Rows)
                {
                    if (name_list[total_count] == null)
                    {
                        decimal temp_mark = Convert.ToDecimal(single_mark[org_total]);
                        if (temp_mark > Convert.ToDecimal(ans_dr["fs"]))
                            throw new ArgumentException("准考证号为" + dr["zkzh"].ToString().Trim() + "的学生第" + (string)ans_dr["th"] + "题满分值小于实际分值！");
                        newRow["T" + (string)ans_dr["th"]] = temp_mark;
                        org_total++;
                        if (!ans_dr["da"].ToString().Trim().Equals(""))
                        {
                            if (obj_count > ans.Length)
                                throw new ArgumentException("标准答案选择题数量大于数据库中选择题数量");
                            newRow["D" + ((string)ans_dr["th"]).Trim()] = ans[obj_count].ToString();
                            obj_count++;
                            obj_mark += temp_mark;
                        }
                        else
                            sub_mark += temp_mark;
                        newRow["totalmark"] = (decimal)newRow["totalmark"] + temp_mark;
                    }
                    else
                    {
                        decimal temp_mark = 0;
                        foreach (string temp_th in name_list[total_count])
                        {
                            temp_mark += (decimal)newRow["T" + temp_th];
                        }
                        newRow["T" + (string)ans_dr["th"]] = temp_mark;

                    }
                    total_count++;


                }
               
                if ((decimal)newRow["totalmark"] > Utils.fullmark)
                    throw new ArgumentException("科目总分设置错误，存在学生满分大于总分的情况");
                


                newRow["Groups"] = "";
                newRow["QX"] = dr["Qxdm"].ToString().Trim();

                decimal totalmark = (decimal)newRow["totalmark"];
                if (totalmark >= Utils.excellent)
                {
                    newRow["rank"] = "excellent";
                }
                else if (totalmark >= Utils.well)
                {
                    newRow["rank"] = "well";
                }
                else if (totalmark >= Utils.pass)
                {
                    newRow["rank"] = "pass";
                }
                else
                {
                    newRow["rank"] = "fail";
                }
                
                basic_data.Rows.Add(newRow);
            }
            _basic_data = basic_data.Copy();
            DataView dv = basic_data.DefaultView;
            dv.Sort = "totalmark desc";
            _basic_data = dv.ToTable();
            //form.ShowPro(30, 2);
            int totalsize = _basic_data.Rows.Count;
            

            create_groups();
            //if (Utils.saveMidData)
            //{
            //    create_db_tables();
            //    create_groups_file();
            //}

            return "";
        }
        public string check_remark(char[] remark)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < remark.Length; i++)
            {
                if (i > 3 && (i - 4) % 5 == 0 && !remark[i].Equals(' '))
                {
                    sb.Append(" ");
                }
                sb.Append(remark[i]);
            }
            return sb.ToString();
        }
        public void create_db_tables()
        {
            #region create table insert data
            int i = 0;
            StringBuilder objectdata = new StringBuilder();
            string newTable = filename + "_full";
            objectdata.Append("CREATE TABLE `" + newTable + "` (\n");
            objectdata.Append("\t`studentid` c(10),\n");
            objectdata.Append("\t`schoolcode` c(10),\n");
            objectdata.Append("\t`totalmark` n(4,1),\n");

            for (i = 3; i < _basic_data.Columns["d1"].Ordinal; i++)
            {
                objectdata.Append("\t`" + _basic_data.Columns[i].ColumnName + "` n(4,1),\n");
            }
            for (i = _basic_data.Columns["D1"].Ordinal; i < _basic_data.Columns.Count - 2; i++)
            {
                objectdata.Append("\t`" + _basic_data.Columns[i].ColumnName + "`c(1),\n");
            }
            objectdata.Append("\t`" + _basic_data.Columns[i].ColumnName + "` c(4),\n");
            objectdata.Append("\t`" + _basic_data.Columns[i + 1].ColumnName + "` c(4));\n");

            OleDbCommand createcommand = new OleDbCommand(objectdata.ToString(), dbfConnection);

            OleDbCommand insertcommand = new OleDbCommand();
            insertcommand.Connection = dbfConnection;
            dbfConnection.Open();
            createcommand.ExecuteNonQuery();
            //form.ShowPro(40, 2);
            OleDbTransaction trans = null;
            trans = insertcommand.Connection.BeginTransaction();
            insertcommand.Transaction = trans;

            foreach (DataRow dr in _basic_data.Rows)
            {
                objectdata.Clear();
                objectdata.Append("INSERT INTO " + newTable + " VALUES ('");
                objectdata.Append(dr[0] + "','" + dr[1] + "',");

                for (i = 2; i < _basic_data.Columns["D1"].Ordinal; i++)
                {
                    objectdata.Append(dr[i] + ",");
                }
                objectdata.Append("'");
                for (i = _basic_data.Columns["D1"].Ordinal; i < _basic_data.Columns.Count - 1; i++)
                    objectdata.Append(dr[i] + "','");

                objectdata.Append(dr[_basic_data.Columns.Count - 1] + "');");
                insertcommand.CommandText = objectdata.ToString();
                insertcommand.ExecuteNonQuery();

            }
            trans.Commit();
            dbfConnection.Close();
            #endregion
        }
        public void create_groups()
        {
            #region divide the table into groups
            //StringBuilder objectdata = new StringBuilder();
            _group_data.Columns.Add("studentid", System.Type.GetType("System.String"));
            _group_data.PrimaryKey = new DataColumn [] { _group_data.Columns["studentid"] };
            _group_data.Columns.Add("schoolcode", System.Type.GetType("System.String"));
            _group_data.Columns.Add("totalmark", System.Type.GetType("System.Decimal"));
            _group_data.Columns.Add("PR_total", System.Type.GetType("System.Decimal"));
            ArrayList tm = new ArrayList();
            string spattern = "^\\d+~\\d+$";
            List<decimal> group_totalmark = new List<decimal>();
            for (int i = 0; i < _groups.Rows.Count; i++)
            {
                ArrayList tz = new ArrayList();
                string row_name = _groups.Rows[i][0].ToString().Trim();
                try
                {
                    _group_data.Columns.Add("G" + (i + 1).ToString(), System.Type.GetType("System.Decimal"));
                    _group_data.Columns.Add("PR" + (i + 1).ToString(), System.Type.GetType("System.Decimal"));
                }
                catch (DuplicateNameException e)
                {
                    throw new System.ArgumentException("分组名“" + row_name + "”重复");
                }
                string org = _groups.Rows[i][1].ToString().Trim();
                string[] org_char = org.Split(new char[3] { ',', '，', '、' });
                foreach (string th in org_char)
                {

                    if (System.Text.RegularExpressions.Regex.IsMatch(th, spattern))
                    {
                        string[] num = th.Split('~');
                        int j;
                        int size = Convert.ToInt32(num[0]) < Convert.ToInt32(num[1]) ? Convert.ToInt32(num[1]) : Convert.ToInt32(num[0]);
                        int start = Convert.ToInt32(num[0]) > Convert.ToInt32(num[1]) ? Convert.ToInt32(num[1]) : Convert.ToInt32(num[0]);
                        if (Math.Abs(size) != size || Math.Abs(start) != start)
                            throw new ArgumentException("题组“" + row_name + "”的题号" + th + "错误: " + "题号不能为负");
                        for (j = start; j < size + 1; j++)
                        {
                            if (!newStandard.Rows.Contains(j.ToString()))
                                throw new ArgumentException("题组“" + row_name + "”的题号" + th + "错误: " + "该题号标准答案中不存在");
                            tz.Add(j.ToString());

                        }

                    }
                    else if (newStandard.Rows.Contains(th))
                        tz.Add(th);
                    else
                    {
                        if (th.Equals(""))
                            throw new ArgumentException("题组“" + row_name + "”的题号错误: " + "结尾多一逗号");
                        else
                            throw new ArgumentException("题组“" + row_name + "”的题号" + th + "错误: " + "该题号标准答案中不存在");
                    }
                }
                tm.Add(tz);
                decimal mark = 0;
                foreach(string s in tz)
                {
                    mark += Convert.ToDecimal(newStandard.Rows.Find(s)["fs"]);
                }
                group_totalmark.Add(mark);
            }
            _group_data.Columns.Add("Groups", typeof(string));
            _group_data.Columns.Add("Qx", typeof(string));
            _group_data.Columns.Add("rank", typeof(string));
            foreach (DataRow dr in _basic_data.Rows)
            {
                DataRow newRow = _group_data.NewRow();
                newRow["studentid"] = ((string)dr["studentid"]).Trim();
                newRow["schoolcode"] = ((string)dr["schoolcode"]).Trim();
                newRow["Groups"] = ((string)dr["Groups"]).Trim();
                newRow["Qx"] = dr["Qx"].ToString().Trim();
                newRow["totalmark"] = dr["totalmark"];
                newRow["PR_total"] = 0;
                newRow["rank"] = dr["rank"];
                int j;
                for (j = 0; j < _groups.Rows.Count; j++)
                {
                    decimal count_ = 0;
                    foreach (object s in (ArrayList)tm[j])
                    {
                        count_ += (decimal)dr["T" + s.ToString()];
                    }
                    newRow["G" + (j + 1).ToString()] = count_ / group_totalmark[j];
                    newRow["PR" + (j + 1).ToString()] = 0;

                }
                _group_data.Rows.Add(newRow);
            }

            //st.Stop();
            #endregion
        }
        public void create_groups_file()
        {
            StringBuilder objectdata = new StringBuilder();
            objectdata.Clear();
            int i = 0;
            string group_Table = filename + "_groups";
            objectdata.Append("CREATE TABLE `" + group_Table + "` (\n");
            objectdata.Append("\t`studentid` c(10),\n");
            objectdata.Append("\t`schoolcode` c(10),\n");
            objectdata.Append("\t`totalmark` n(4,1),\n");
            for (i = 3; i < _group_data.Columns.Count - 2; i++)
            {
                objectdata.Append("\t`" + _group_data.Columns[i].ColumnName + "` n(4,1),\n");
            }
            objectdata.Append("\t`" + _group_data.Columns[i].ColumnName + "` c(4),\n");
            objectdata.Append("\t`" + _group_data.Columns[i + 1].ColumnName + "` c(4));");
            OleDbCommand group_create = new OleDbCommand(objectdata.ToString(), dbfConnection);
            dbfConnection.Open();
            group_create.ExecuteNonQuery();
            OleDbCommand group_insert = new OleDbCommand();
            group_insert.Connection = dbfConnection;
            OleDbTransaction group_trans = null;
            group_trans = group_insert.Connection.BeginTransaction();
            group_insert.Transaction = group_trans;

            foreach (DataRow dr in _group_data.Rows)
            {
                objectdata.Clear();
                objectdata.Append("INSERT INTO " + group_Table + " VALUES ('");
                objectdata.Append(dr[0] + "','" + dr[1] + "',");

                for (i = 2; i < _group_data.Columns.Count - 2; i++)
                {
                    objectdata.Append(dr[i] + ",");
                }
                objectdata.Append("'");
                objectdata.Append(dr[_group_data.Columns.Count - 2] + "','" + dr[_group_data.Columns.Count - 1] + "');");
                group_insert.CommandText = objectdata.ToString();
                group_insert.ExecuteNonQuery();

            }
            group_trans.Commit();
            dbfConnection.Close();
        }
        public DataTable StandardAnsRecontruction(DataTable dt, List<List<string>> name)
        {
            DataTable newtable = dt.Clone();
            Stack<string> sk = new Stack<string>();

            newtable.PrimaryKey = new DataColumn[] { newtable.Columns["th"] };
            foreach (DataRow dr in dt.Rows)
            {
                DataRow newrow = newtable.NewRow();
                string th = dr["th"].ToString().Trim();
                //if (!th.Contains("_"))
                //{
                //    newrow.ItemArray = dr.ItemArray;
                //    newtable.Rows.Add(newrow);
                //    name.Add(null);
                //    continue;
                //}
                if (sk.Count == 0)
                {
                    if (th.Contains("_"))
                        sk.Push(th);
                    newrow.ItemArray = dr.ItemArray;
                    newtable.Rows.Add(newrow);
                    name.Add(null);
                }
                else
                {
                    string prefix = omit_tail(sk.Peek());
                    if (th.StartsWith(prefix))
                    {
                        if (th.Contains("_"))
                            sk.Push(th);
                        newrow.ItemArray = dr.ItemArray;
                        newtable.Rows.Add(newrow);
                        name.Add(null);
                    }
                    else
                    {
                        while (true)
                        {

                            popstack(newtable, sk, name);
                            if (!sk.Peek().Contains("_"))
                            {
                                sk.Pop();
                                if (th.Contains("_"))
                                    sk.Push(th);
                                newrow.ItemArray = dr.ItemArray;
                                newtable.Rows.Add(newrow);
                                name.Add(null);
                                break;
                            }
                            else if (th.StartsWith(omit_tail(sk.Peek())))
                            {
                                if (th.Contains("_"))
                                    sk.Push(th);
                                newrow.ItemArray = dr.ItemArray;
                                newtable.Rows.Add(newrow);
                                name.Add(null);
                                break;
                            }
                            else
                                continue;

                        }
                    }
                }

            }
            while (sk.Count > 0)
            {

                popstack(newtable, sk, name);
                if (!sk.Peek().Contains("_"))
                    sk.Pop();

            }
            return newtable;
        }
        public void popstack(DataTable dt, Stack<string> sk, List<List<string>> name)
        {
            List<string> record = new List<string>();
            string temp_th;
            DataRow dr = dt.NewRow();
            double mark = 0;
            while (true)
            {
                temp_th = sk.Pop();
                record.Add(temp_th);
                mark += Convert.ToDouble(dt.Rows.Find(temp_th)["fs"]);
                if (sk.Count != 0 && sk.Peek().StartsWith(omit_tail(temp_th)))
                    continue;
                else
                    break;
            }
            sk.Push(omit_tail(temp_th));
            if (record.Count > 1)
            {
                dr["th"] = omit_tail(temp_th);
                dr["fs"] = Convert.ToInt32(mark).ToString();
                dr["da"] = "";
                dt.Rows.Add(dr);
                name.Add(record);
            }
        }
        public string omit_tail(string serial)
        {
            Regex num_regex = new Regex(@"(\d+_)+\d+$");
            if (!num_regex.IsMatch(serial))
                throw new ArgumentException("标准答案 " + serial + " 题号格式不正确！");
            MatchCollection match = Regex.Matches(serial, @"\w+(?=_\d+$)");
            if (match.Count > 1)
                throw new ArgumentException("标准答案 " + serial + " 题号格式不正确！");
            return match[0].ToString();
        }
    }
}
