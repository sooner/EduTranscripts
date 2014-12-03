namespace HKreporter
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.dbf_adr = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.standard_adr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.group_adr = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.exam = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.stu_id_end = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.stu_id_start = new System.Windows.Forms.TextBox();
            this.pass = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.well = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.excellent = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.fullmark = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.save_adr = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.run_button = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.isVisible = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.well)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.excellent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fullmark)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据文件:";
            // 
            // dbf_adr
            // 
            this.dbf_adr.Location = new System.Drawing.Point(85, 38);
            this.dbf_adr.Name = "dbf_adr";
            this.dbf_adr.Size = new System.Drawing.Size(288, 21);
            this.dbf_adr.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(396, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "打开";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(396, 66);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(86, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "打开";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // standard_adr
            // 
            this.standard_adr.Location = new System.Drawing.Point(85, 68);
            this.standard_adr.Name = "standard_adr";
            this.standard_adr.Size = new System.Drawing.Size(288, 21);
            this.standard_adr.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "标准答案:";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(396, 98);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(86, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "打开";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // group_adr
            // 
            this.group_adr.Location = new System.Drawing.Point(85, 100);
            this.group_adr.Name = "group_adr";
            this.group_adr.Size = new System.Drawing.Size(288, 21);
            this.group_adr.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "分组文件:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "科目:";
            // 
            // exam
            // 
            this.exam.FormattingEnabled = true;
            this.exam.Items.AddRange(new object[] {
            "语文",
            "数学",
            "英语",
            "化学",
            "物理",
            "生物",
            "政治",
            "历史",
            "地理"});
            this.exam.Location = new System.Drawing.Point(85, 10);
            this.exam.Name = "exam";
            this.exam.Size = new System.Drawing.Size(91, 20);
            this.exam.TabIndex = 11;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(247, 9);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowUpDown = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(82, 21);
            this.dateTimePicker1.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(206, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "日期:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(233, 142);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "学号:";
            // 
            // stu_id_end
            // 
            this.stu_id_end.Location = new System.Drawing.Point(274, 171);
            this.stu_id_end.Name = "stu_id_end";
            this.stu_id_end.Size = new System.Drawing.Size(205, 21);
            this.stu_id_end.TabIndex = 17;
            this.stu_id_end.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(245, 174);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "至:";
            // 
            // stu_id_start
            // 
            this.stu_id_start.Location = new System.Drawing.Point(274, 138);
            this.stu_id_start.Name = "stu_id_start";
            this.stu_id_start.Size = new System.Drawing.Size(205, 21);
            this.stu_id_start.TabIndex = 15;
            // 
            // pass
            // 
            this.pass.Location = new System.Drawing.Point(75, 198);
            this.pass.Name = "pass";
            this.pass.Size = new System.Drawing.Size(120, 21);
            this.pass.TabIndex = 24;
            this.pass.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(33, 201);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 12);
            this.label10.TabIndex = 23;
            this.label10.Text = "合格:";
            // 
            // well
            // 
            this.well.Location = new System.Drawing.Point(75, 169);
            this.well.Name = "well";
            this.well.Size = new System.Drawing.Size(120, 21);
            this.well.TabIndex = 22;
            this.well.Value = new decimal(new int[] {
            70,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(33, 172);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 12);
            this.label9.TabIndex = 21;
            this.label9.Text = "良好:";
            // 
            // excellent
            // 
            this.excellent.Location = new System.Drawing.Point(75, 138);
            this.excellent.Name = "excellent";
            this.excellent.Size = new System.Drawing.Size(120, 21);
            this.excellent.TabIndex = 20;
            this.excellent.Value = new decimal(new int[] {
            85,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(33, 141);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 12);
            this.label8.TabIndex = 19;
            this.label8.Text = "优秀:";
            // 
            // fullmark
            // 
            this.fullmark.Location = new System.Drawing.Point(275, 201);
            this.fullmark.Name = "fullmark";
            this.fullmark.Size = new System.Drawing.Size(98, 21);
            this.fullmark.TabIndex = 26;
            this.fullmark.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(221, 203);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 12);
            this.label11.TabIndex = 25;
            this.label11.Text = "满分值:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(21, 236);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 12);
            this.label12.TabIndex = 27;
            this.label12.Text = "存储至:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(396, 231);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(86, 23);
            this.button4.TabIndex = 29;
            this.button4.Text = "打开";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // save_adr
            // 
            this.save_adr.Location = new System.Drawing.Point(85, 233);
            this.save_adr.Name = "save_adr";
            this.save_adr.Size = new System.Drawing.Size(288, 21);
            this.save_adr.TabIndex = 28;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(14, 273);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(271, 23);
            this.progressBar1.TabIndex = 30;
            // 
            // run_button
            // 
            this.run_button.Location = new System.Drawing.Point(304, 273);
            this.run_button.Name = "run_button";
            this.run_button.Size = new System.Drawing.Size(86, 23);
            this.run_button.TabIndex = 31;
            this.run_button.Text = "开始";
            this.run_button.UseVisualStyleBackColor = true;
            this.run_button.Click += new System.EventHandler(this.button5_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(396, 273);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(86, 23);
            this.cancel.TabIndex = 32;
            this.cancel.Text = "取消";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(16, 300);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(0, 12);
            this.label13.TabIndex = 33;
            // 
            // isVisible
            // 
            this.isVisible.AutoSize = true;
            this.isVisible.Location = new System.Drawing.Point(396, 203);
            this.isVisible.Name = "isVisible";
            this.isVisible.Size = new System.Drawing.Size(72, 16);
            this.isVisible.TabIndex = 34;
            this.isVisible.Text = "文档可视";
            this.isVisible.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 315);
            this.Controls.Add(this.isVisible);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.run_button);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.save_adr);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.fullmark);
            this.Controls.Add(this.stu_id_end);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.pass);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.stu_id_start);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.well);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.exam);
            this.Controls.Add(this.excellent);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.group_adr);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.standard_adr);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dbf_adr);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "会考";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.well)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.excellent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fullmark)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox dbf_adr;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox standard_adr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox group_adr;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox exam;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox stu_id_end;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox stu_id_start;
        private System.Windows.Forms.NumericUpDown pass;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown well;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown excellent;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown fullmark;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox save_adr;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button run_button;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox isVisible;
    }
}

