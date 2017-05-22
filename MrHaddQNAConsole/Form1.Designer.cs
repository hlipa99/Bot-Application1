namespace MrHaddQNAConsole
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
            this.question_TB = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.addQuestion_BTN = new System.Windows.Forms.Button();
            this.subQuestionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.subSubject_TB = new System.Windows.Forms.TextBox();
            this.subject_TB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureName_LBL = new System.Windows.Forms.Label();
            this.loadPic = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.questionType_CB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // question_TB
            // 
            this.question_TB.Location = new System.Drawing.Point(245, 44);
            this.question_TB.Multiline = true;
            this.question_TB.Name = "question_TB";
            this.question_TB.Size = new System.Drawing.Size(792, 125);
            this.question_TB.TabIndex = 0;
            this.question_TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1051, 783);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.addQuestion_BTN);
            this.tabPage1.Controls.Add(this.subQuestionsPanel);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.subSubject_TB);
            this.tabPage1.Controls.Add(this.subject_TB);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.pictureName_LBL);
            this.tabPage1.Controls.Add(this.loadPic);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.questionType_CB);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.question_TB);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1043, 757);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // addQuestion_BTN
            // 
            this.addQuestion_BTN.Location = new System.Drawing.Point(16, 728);
            this.addQuestion_BTN.Name = "addQuestion_BTN";
            this.addQuestion_BTN.Size = new System.Drawing.Size(112, 23);
            this.addQuestion_BTN.TabIndex = 14;
            this.addQuestion_BTN.Text = "הוסף שאלה למאגר";
            this.addQuestion_BTN.UseVisualStyleBackColor = true;
            this.addQuestion_BTN.Click += new System.EventHandler(this.addQuestion_BTN_Click);
            // 
            // subQuestionsPanel
            // 
            this.subQuestionsPanel.AutoScroll = true;
            this.subQuestionsPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.subQuestionsPanel.Location = new System.Drawing.Point(16, 225);
            this.subQuestionsPanel.Name = "subQuestionsPanel";
            this.subQuestionsPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.subQuestionsPanel.Size = new System.Drawing.Size(1021, 497);
            this.subQuestionsPanel.TabIndex = 13;
            this.subQuestionsPanel.WrapContents = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(972, 209);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "תתי שאלות";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(870, 175);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(167, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "הוסף תת שאלה";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // subSubject_TB
            // 
            this.subSubject_TB.Location = new System.Drawing.Point(16, 72);
            this.subSubject_TB.Name = "subSubject_TB";
            this.subSubject_TB.Size = new System.Drawing.Size(144, 20);
            this.subSubject_TB.TabIndex = 9;
            // 
            // subject_TB
            // 
            this.subject_TB.Location = new System.Drawing.Point(16, 44);
            this.subject_TB.Name = "subject_TB";
            this.subject_TB.Size = new System.Drawing.Size(144, 20);
            this.subject_TB.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(174, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "תת נושא";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(191, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "נושא";
            // 
            // pictureName_LBL
            // 
            this.pictureName_LBL.AutoSize = true;
            this.pictureName_LBL.Location = new System.Drawing.Point(187, 156);
            this.pictureName_LBL.Name = "pictureName_LBL";
            this.pictureName_LBL.Size = new System.Drawing.Size(38, 13);
            this.pictureName_LBL.TabIndex = 5;
            this.pictureName_LBL.Text = "תמונה";
            this.pictureName_LBL.Visible = false;
            // 
            // loadPic
            // 
            this.loadPic.Location = new System.Drawing.Point(16, 130);
            this.loadPic.Name = "loadPic";
            this.loadPic.Size = new System.Drawing.Size(209, 23);
            this.loadPic.TabIndex = 4;
            this.loadPic.Text = "טען תמונה לשאלת מקור";
            this.loadPic.UseVisualStyleBackColor = true;
            this.loadPic.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(166, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "סוג שאלה";
            // 
            // questionType_CB
            // 
            this.questionType_CB.AutoCompleteCustomSource.AddRange(new string[] {
            "שאלה רגילה",
            "שאלת מקור",
            "",
            ""});
            this.questionType_CB.FormattingEnabled = true;
            this.questionType_CB.Items.AddRange(new object[] {
            "רגיל",
            "שאלת מקור"});
            this.questionType_CB.Location = new System.Drawing.Point(16, 103);
            this.questionType_CB.Name = "questionType_CB";
            this.questionType_CB.Size = new System.Drawing.Size(144, 21);
            this.questionType_CB.TabIndex = 2;
            this.questionType_CB.SelectedIndexChanged += new System.EventHandler(this.questionType_CB_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(968, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "טקסט שאלה";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(959, 757);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1076, 808);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox question_TB;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox questionType_CB;
        private System.Windows.Forms.TextBox subSubject_TB;
        private System.Windows.Forms.TextBox subject_TB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label pictureName_LBL;
        private System.Windows.Forms.Button loadPic;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button addQuestion_BTN;
        private System.Windows.Forms.FlowLayoutPanel subQuestionsPanel;
        private System.Windows.Forms.Label label6;
    }
}

