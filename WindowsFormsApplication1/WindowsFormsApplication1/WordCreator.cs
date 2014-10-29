using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using ZedGraph;
using System.Windows.Forms;
namespace HKreporter
{
    public class WordCreator
    {
        Word._Application oWord;
        Word._Document oDoc;
        object oMissing = System.Reflection.Missing.Value;
        public void create_word(DataTable dt, DataRow dr, DataTable group, List<string> group_name)
        {
            object filepath = @Utils.CurrentDirectory + @"\template.doc";
            oWord = new Word.Application();
            oWord.Visible = Utils.isVisible;
            oDoc = oWord.Documents.Add(ref filepath, ref oMissing,
            ref oMissing, ref oMissing);
            WriteIntoDocument("subject", Utils.subject);
            WriteIntoDocument("stu_id", dr["studentid"].ToString().Trim());
            WriteIntoDocument("date", Utils.date.Trim());
            WriteIntoDocument("totalmark", string.Format("{0:F1}", (decimal)dr["totalmark"]));

            DataRow rankDR = null;
            DataRow totalDR = dt.Rows.Find("total");
            string rank_name = "";
            switch ((string)dr["rank"])
            {
                case "excellent":
                    WriteIntoDocument("rank", "优秀");
                    rankDR = dt.Rows.Find("excellent");
                    rank_name = "优秀等级";
                    break;
                case "well":
                    WriteIntoDocument("rank", "良好");
                    rankDR = dt.Rows.Find("well");
                    rank_name = "良好等级";
                    break;
                case "pass":
                    WriteIntoDocument("rank", "及格");
                    rankDR = dt.Rows.Find("pass");
                    rank_name = "及格等级";
                    break;
                case "fail":
                    WriteIntoDocument("rank", "不及格");
                    rankDR = dt.Rows.Find("fail");
                    rank_name = "不及格等级";
                    break;
                default:
                    break;
            }
                  
            WriteIntoDocument("PR", string.Format("{0:F1}", (decimal)dr["PR_total"]));

            Word.Table table = oDoc.Tables[1];

            table.Cell(2, 3).Range.Text = rank_name;
            int previousRow = 3;
            for (int i = 0; i < group.Rows.Count; i++)
            {
                table.Cell(i+2, 3).Range.Rows.Add(oMissing);
                
                for (int j = 1; j < 6; j++)
                {
                    table.Cell(i + 3, j).Range.Font.Bold = 0;
                    table.Cell(i + 3, j).Range.Font.Size = 10;
                    table.Cell(i + 3, j).Range.Shading.BackgroundPatternColor = table.Cell(1, 1).Range.Shading.BackgroundPatternColor;
                    table.Cell(i + 3, j).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    table.Cell(i + 3, j).SetHeight(0.48f, Word.WdRowHeightRule.wdRowHeightAtLeast);
                }
                
                string key = group.Rows[i]["tz"].ToString().Trim();
                table.Cell(i + 3, 1).Range.Text = key;
                
                table.Cell(i + 3, 2).Range.Text = string.Format("{0:F2}", (decimal)dr["G" + (i + 1).ToString()]);
                table.Cell(i + 3, 3).Range.Text = string.Format("{0:F2}", (decimal)rankDR["G" + (i + 1).ToString()]);
                table.Cell(i + 3, 4).Range.Text = string.Format("{0:F2}", (decimal)totalDR["G" + (i + 1).ToString()]);
                table.Cell(i + 3, 5).Range.Text = string.Format("{0:F1}", (decimal)dr["PR" + (i + 1).ToString()]);

                if (group_name.Contains(key))
                {
                    table.Cell(i + 3, 1).Range.Font.Bold = 1;
                    table.Cell(i + 3, 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    previousRow = i + 3;
                    for (int j = 2; j < 6; j++)
                    {
                        table.Cell(i + 3, j).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        table.Cell(i + 3, j).Range.Shading.BackgroundPatternColor = Word.WdColor.wdColorGray10;
                    }

                }
                else
                {

                    table.Cell(previousRow, 1).Merge(table.Cell(i + 3, 1));

                }
            }
            
            draw(group, dr,rankDR,totalDR);
            Word.Range dist_rng = oDoc.Bookmarks.get_Item("pic").Range;
            dist_rng.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            dist_rng.Paste();

            Word.Table count_table = oDoc.Tables[2];
            count_table.Cell(2, 2).Range.Text = getPercent((int)dt.Rows[1]["num"], (int)dt.Rows[0]["num"]);
            count_table.Cell(2, 3).Range.Text = getPercent((int)dt.Rows[2]["num"], (int)dt.Rows[0]["num"]);
            count_table.Cell(2, 4).Range.Text = getPercent((int)dt.Rows[3]["num"], (int)dt.Rows[0]["num"]);
            count_table.Cell(2, 5).Range.Text = getPercent((int)dt.Rows[4]["num"], (int)dt.Rows[0]["num"]);

            Word.Table TH_table = oDoc.Tables[3];
            previousRow = 1;
            for (int i = 0; i < group.Rows.Count; i++)
            {
                
                for (int j = 1; j < 3; j++)
                {
                    TH_table.Cell(i + 2, j).Range.Font.Bold = 0;
                    TH_table.Cell(i + 2, j).Range.Font.Size = 10;
                    TH_table.Cell(i + 2, j).Range.Shading.BackgroundPatternColor = table.Cell(1, 1).Range.Shading.BackgroundPatternColor;
                    TH_table.Cell(i + 2, j).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    TH_table.Cell(i + 2, j).SetHeight(0.48f, Word.WdRowHeightRule.wdRowHeightAtLeast);
                }
                string key = group.Rows[i]["tz"].ToString().Trim();
                TH_table.Cell(i + 2, 1).Range.Text = key;
                TH_table.Cell(i + 2, 2).Range.Text = getTH(group.Rows[i]["th"].ToString().Trim());
                if (group_name.Contains(key))
                {
                    previousRow = i + 2;
                    TH_table.Cell(i + 2, 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    TH_table.Cell(i + 2, 1).Range.Font.Bold = 1;
                    TH_table.Cell(i + 2, 2).Range.Shading.BackgroundPatternColor = Word.WdColor.wdColorGray10;
                }
                else
                {
                    TH_table.Cell(i + 2, 2).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    TH_table.Cell(previousRow, 1).Merge(TH_table.Cell(i + 2, 1));
                }
                if(i < group.Rows.Count - 1)
                    TH_table.Cell(i + 2, 2).Range.Rows.Add(oMissing);
            }

            

            
            string name = "会考成绩_" + dr["studentid"] + ".docx";
            string addr = Utils.save_adr + @"\" + name;
            oDoc.SaveAs(addr, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
            oDoc.Close(oMissing, oMissing, oMissing);
            oWord.Quit(oMissing, oMissing, oMissing);
        }
        public string getPercent(int num, int total)
        {
            return string.Format("{0:F2}", num / Convert.ToDouble(total) * 100);
        }
        public string getTH(string th)
        {
            string[] th_string = th.ToString().Trim().Split(new char[2] { ',', '，' });
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < th_string.Length; i++)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(th_string[i], "^\\d+~\\d+$"))
                {
                    string[] num = th_string[i].Split('~');
                    sb.Append(num[0]);
                    sb.Append("～");
                    sb.Append(num[1]);
                }
                else
                    sb.Append(th_string[i]);
                if (i < th_string.Length - 1)
                    sb.Append("、");
            }
            return sb.ToString();
        }
        public void WriteIntoDocument(string BookmarkName, string FillName)
        {
            object bookmarkName = BookmarkName;
            Microsoft.Office.Interop.Word.Bookmark bm = oDoc.Bookmarks.get_Item(ref bookmarkName);//返回书签 
            bm.Range.Text = FillName;//设置书签域的内容
        }

        public void testCase()
        {
            object filepath = @"C:\Users\sooner\Documents\Visual Studio 2010\Projects\new2\WindowsFormsApplication1\WindowsFormsApplication1\bin\Debug\template.doc";
            oWord = new Word.Application();
            oWord.Visible = true;
            oDoc = oWord.Documents.Add(ref filepath, ref oMissing,
            ref oMissing, ref oMissing);
            Word.Table table = oDoc.Tables[2];

            //object beforerow = table.Rows[1];
           
            //table.Rows.Add(beforerow);
            table.Cell(2, 1).Range.Text = "听力";
            table.Cell(2, 2).Range.Text = "写作";
            //table.Cell(2, 3).Range.Text = "阅读";
            table.Cell(2, 2).Range.Rows.Add(oMissing);

        }

        public void draw(DataTable dt, DataRow basic, DataRow rank, DataRow total)
        {
            ZedGraphControl zgc = new ZedGraphControl();
            GraphPane myPane = zgc.GraphPane;
            zgc.Width = 531;
            zgc.Height = 291;
            List<double[]> data = new List<double[]>();

            string[] xlabels = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
                xlabels[i] = AxisTransfer(dt.Rows[i]["tz"].ToString().Trim());

            AddData(basic, data, dt.Rows.Count);
            AddData(rank, data, dt.Rows.Count);
            AddData(total, data, dt.Rows.Count);

            AddCurve(data[0], "本人", ref myPane, SymbolType.Diamond, Color.Red);
            AddCurve(data[1], "等第平均得分率", ref myPane, SymbolType.Square, Color.Blue);
            AddCurve(data[2], "总体平均得分率", ref myPane, SymbolType.Triangle, Color.DarkGreen);

            myPane.XAxis.Scale.TextLabels = xlabels;
            //myPane.XAxis.Scale.FontSpec.Angle = 90;
            myPane.XAxis.Type = AxisType.Text;
            myPane.XAxis.Scale.FontSpec.Size = 18;
            myPane.XAxis.Title.Text = "";
            myPane.YAxis.Title.Text = "";
            myPane.Title.Text = "";

            myPane.XAxis.Scale.Max = dt.Rows.Count+1;
            myPane.XAxis.Scale.MajorStep = 1;
            myPane.YAxis.Scale.Max = 1.1;
            myPane.YAxis.Scale.MajorStep = 0.5;
            myPane.YAxis.Scale.Min = 0;
            myPane.YAxis.MinorTic.IsAllTics = false;
            myPane.XAxis.MinorTic.IsAllTics = false;
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.XAxis.MajorTic.IsOpposite = false;

            myPane.Legend.IsVisible = true;
            myPane.Legend.Position = LegendPos.BottomCenter;
            myPane.Legend.FontSpec.Size = 15;

            myPane.Title.IsVisible = true;
            myPane.Chart.Fill = new Fill(Color.White);
            zgc.AxisChange();
            Bitmap sourceBitmap = new Bitmap(zgc.Width, zgc.Height, System.Drawing.Imaging.PixelFormat.Format48bppRgb);
            zgc.DrawToBitmap(sourceBitmap, new Rectangle(0, 0, zgc.Width, zgc.Height));
            //Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
            Clipboard.Clear();
            Clipboard.SetImage(sourceBitmap);
        }
        public void AddData(DataRow dr, List<double[]> list, int count)
        {
            double[] temp = new double[count];
            for (int i = 0; i < count; i++)
            {
                temp[i] = Convert.ToDouble(dr["G" + (i + 1).ToString()]);
            }

            list.Add(temp);

        }
        public void AddCurve(double[] data, string name, ref GraphPane pane, SymbolType type, Color color)
        {
            LineItem myCurve = pane.AddCurve(name, null, data, color, type);
            myCurve.Line.IsVisible = false;
            myCurve.Symbol.Size = 12;
            myCurve.Symbol.Fill = new Fill(color);
        }

        public static string AxisTransfer(string name)
        {
            char[] names = name.ToCharArray();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].Equals('（') || names[i].Equals('('))
                {
                    while (i < names.Length)
                    {
                        if (names[i].Equals('）') || names[i].Equals(')'))
                        {
                            sb.Append(names[i]);
                            break;
                        }
                        sb.Append(name[i]);
                        i++;
                    }
                    sb.Append("\n");
                }
                else
                {
                    sb.Append(names[i]);
                    sb.Append("\n");
                }

            }
            return sb.ToString();
        }
    }
}
