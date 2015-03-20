using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace HKreporter
{
    public delegate void MyDelegate(int i, int status);
    public delegate void CheckStuIDMethod(int status, string message);
    public partial class Form1 : Form
    {
        Thread thread;
        public Form1()
        {
            InitializeComponent();
            save_adr.Text = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            stu_id_start.Text = "17121010586";
            zipname.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "C://";
            openFileDialog1.Filter = "DBF files (*.dbf)|*.dbf|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                dbf_adr.Text = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "C://";
            openFileDialog1.Filter = "Excel files (*.xls,*.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                standard_adr.Text = openFileDialog1.FileName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "C://";
            openFileDialog1.Filter = "Excel files (*.xls,*.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                group_adr.Text = openFileDialog1.FileName;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openFolder = new FolderBrowserDialog();
            openFolder.ShowNewFolderButton = true;
            openFolder.Description = "保存至";
            if (openFolder.ShowDialog() == DialogResult.OK)
                save_adr.Text = openFolder.SelectedPath;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!checkconfig())
                return;
            startProcess start = new startProcess(this);
            ExecutorMethod exe = start.exe;
            Utils.subject = exam.SelectedItem.ToString().Trim();
            Utils.date = dateTimePicker1.Value.Year.ToString() + "年" + dateTimePicker1.Value.Month.ToString() + "月";
            Utils.dbf_adr = dbf_adr.Text;
            Utils.standard_adr = standard_adr.Text;
            Utils.groups_adr = group_adr.Text;
            Utils.excellent = excellent.Value;
            Utils.well = well.Value;
            Utils.pass = pass.Value;
            Utils.stu_id_start = stu_id_start.Text;
            Utils.stu_id_end = stu_id_end.Text;
            Utils.fullmark = fullmark.Value;
            Utils.save_adr = save_adr.Text;
            Utils.isVisible = isVisible.Checked;

            Utils.zipname = zipname.Text;

            thread = new Thread(new ThreadStart(start.data_process));
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

        }
        public class startProcess
        {
            public ExecutorMethod exe;
            public Form1 form;
            public startProcess(Form1 _form)
            {
                exe = new ExecutorMethod();
                form = _form;
            }
            public void data_process()
            {
                form.ShowPro(0, 0);
                exe.pre_process(form);
                form.ShowPro(100, 5);
            }
        }
        bool checkconfig()
        {
            if (exam.SelectedItem == null)
                return Error("请选择考试科目！");
            if (string.IsNullOrEmpty(dbf_adr.Text.Trim()))
                return Error("请选择数据文件地址！");
            if (string.IsNullOrEmpty(standard_adr.Text.Trim()))
                return Error("请选择标准答案地址！");
            if (string.IsNullOrEmpty(group_adr.Text.Trim()))
                return Error("请选择分组文件地址！");
            if (string.IsNullOrEmpty(stu_id_start.Text.Trim()))
                return Error("请输入学号！");
            if (Math.Abs(fullmark.Value) != fullmark.Value || fullmark.Value == 0)
                return Error("科目总分应为非负数");
            return true;
        }

        private bool Error(string errormessage)
        {
            MessageBox.Show(errormessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        public void ShowPro(int value, int status)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MyDelegate(ShowPro), value, status);
            }
            else
            {
                this.progressBar1.Value = value;
                switch (status)
                {
                    case 0:
                        run_button.Enabled = false;
                        cancel.Enabled = true;
                        this.label13.Text = "标准答案处理中...";
                        break;
                    case 1:
                        this.label13.Text = "分组信息处理中...";
                        break;
                    case 2:
                        this.label13.Text = "数据文件读入处理中...";
                        break;
                    case 3:
                        this.label13.Text = "数据处理中...";
                        break;
                    case 4:
                        this.label13.Text = "文档生成中...";
                        break;
                    case 5:
                        this.label13.Text = "完成！";
                        run_button.Enabled = true;
                        cancel.Enabled = false;
                        break;
                    default:
                        break;
                }


            }
        }

        public void CheckStuID(int status, string message)
        {
            if (this.InvokeRequired)
                this.Invoke(new CheckStuIDMethod(CheckStuID), status, message);
            else
            {
                switch (status)
                {
                    case 1:

                        MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                        DialogResult dr = MessageBox.Show(message + "\n仍然继续嘛？", "是否继续", messButton);
                        if (dr == DialogResult.Cancel)
                        {
                            thread.Abort();
                            ShowPro(100, 5);
                            cancel.Enabled = false;
                            run_button.Enabled = true;
                        }
                        break;
                    case 2:

                        Error(message);
                        thread.Abort();
                        ShowPro(100, 5);
                        cancel.Enabled = false;
                        run_button.Enabled = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (thread.IsAlive)
            {
                thread.Abort();
                ShowPro(100, 5);
                cancel.Enabled = false;
                run_button.Enabled = true;
            }
        }

        private void stu_id_end_TextChanged(object sender, EventArgs e)
        {
            int result;
            if (Int32.TryParse(stu_id_end.Text, out result))
            {
                if (result > 0)
                {
                    zipname.Enabled = true;
                    zipname.Text = DateTime.Now.ToString("yyMMddHHmmss");
                }
                else
                {
                    zipname.Enabled = false;
                    zipname.Text = "";
                }
            }
            
        }
      
    }
}
