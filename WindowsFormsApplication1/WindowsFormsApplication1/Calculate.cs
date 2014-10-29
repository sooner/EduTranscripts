using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace HKreporter
{
    public class Calculate
    {
        public DataTable total;
        public DataTable _data;
        public void execute(DataTable ans, DataTable group, DataTable data)
        {

            int totalnum = data.Rows.Count;
            decimal mark = (decimal)data.Rows[0]["totalmark"];
            int count = 1;
            data.Rows[0]["PR_total"] = PercentRank(1, totalnum);
            for (int i = 1; i < data.Rows.Count; i++)
            {
                DataRow dr = data.Rows[i];
                if ((decimal)dr["totalmark"] == mark)
                {
                    dr["PR_total"] = PercentRank(count, totalnum);

                }
                else
                {
                    dr["PR_total"] = PercentRank(i+1, totalnum);
                    count = i + 1;
                    mark = (decimal)dr["totalmark"];
                }

            }
            DataView dv = null;
            for (int i = 0; i < group.Rows.Count; i++)
            {
                dv = data.DefaultView;
                dv.Sort = "G" + (i + 1).ToString() + " desc";
                data = dv.ToTable();
                mark = (decimal)data.Rows[0]["G" + (i + 1).ToString()];
                count = 1;
                data.Rows[0]["PR" + (i + 1).ToString()] = PercentRank(1, totalnum);
                for (int j = 1; j < data.Rows.Count; j++)
                {
                    DataRow dr = data.Rows[j];
                    if ((decimal)dr["G" + (i + 1).ToString()] == mark)
                        dr["PR" + (i + 1).ToString()] = PercentRank(count, totalnum);
                    else
                    {
                        dr["PR" + (i + 1).ToString()] = PercentRank(j + 1, totalnum);
                        count = j + 1;
                        mark = (decimal)dr["G" + (i + 1).ToString()];
                    }
                }
            }

            dv = data.DefaultView;
            dv.Sort = "totalmark desc";

            _data = dv.ToTable();
            _data.PrimaryKey = new DataColumn[] { _data.Columns["studentid"] };
            total = new DataTable();
            total.Columns.Add("type", System.Type.GetType("System.String"));
            total.Columns.Add("num", typeof(int));

            for (int i = 0; i < group.Rows.Count; i++)
                total.Columns.Add("G" + (i + 1).ToString(), typeof(decimal));
            total.PrimaryKey = new DataColumn[] {total.Columns["type"]};
            total.Rows.Add(getNewRow(total, "total", group.Rows.Count));
            total.Rows.Add(getNewRow(total, "excellent", group.Rows.Count));
            total.Rows.Add(getNewRow(total, "well", group.Rows.Count));
            total.Rows.Add(getNewRow(total, "pass", group.Rows.Count));
            total.Rows.Add(getNewRow(total, "fail", group.Rows.Count));

            for(int i = 0; i < group.Rows.Count; i++)
                total.Rows[0]["G" + (i + 1).ToString()] = (decimal)data.Compute("Avg(G" + (i + 1).ToString()+")", "");
            total.Rows[0]["num"] = data.Rows.Count;
            var num_count = from row in data.AsEnumerable()
                            group row by row.Field<string>("rank") into grp
                            select new
                            {
                                rank = grp.Key,
                                count = grp.Count()
                            };
            foreach (var item in num_count)
                total.Rows.Find(item.rank.ToString().Trim())["num"] = item.count;
            for (int i = 0; i < group.Rows.Count; i++)
            {
                var rank = from row in data.AsEnumerable()
                           group row by row.Field<string>("rank") into grp
                           select new
                           {
                               rank = grp.Key,
                               mark = grp.Average(row => row.Field<decimal>("G" + (i + 1).ToString())),
                               count = grp.Count()
                           };
                foreach (var item in rank)
                {
                    if (item.rank.Equals("excellent"))
                        total.Rows.Find("excellent")["G" + (i + 1).ToString()] = item.mark;
                    else if(item.rank.Equals("well"))
                        total.Rows.Find("well")["G" + (i + 1).ToString()] = item.mark;
                    else if(item.rank.Equals("pass"))
                        total.Rows.Find("pass")["G" + (i + 1).ToString()] = item.mark;
                    else if(item.rank.Equals("fail"))
                        total.Rows.Find("fail")["G" + (i + 1).ToString()] = item.mark;
                }
            }
            
        }

        decimal PercentRank(int rank, int count)
        {
            return 100 - (100.0m * rank - 50.0m) / count;
        }

        DataRow getNewRow(DataTable dt, string type, int count)
        {
            DataRow nr = dt.NewRow();
            nr["type"] = type;
            for (int i = 0; i < count; i++)
                nr["G" + (i + 1).ToString()] = 0m;
            return nr;
        }

    }
}
