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
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("root");
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("root");
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
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.questionInput_TB = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.resualt_TB3 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.answerTreeView = new System.Windows.Forms.TreeView();
            this.send_BTN2 = new System.Windows.Forms.Button();
            this.userAnswer_TB2 = new System.Windows.Forms.TextBox();
            this.systemAnswer_TB = new System.Windows.Forms.TextBox();
            this.userAnswerTreeView = new System.Windows.Forms.TreeView();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // question_TB
            // 
            this.question_TB.Location = new System.Drawing.Point(368, 68);
            this.question_TB.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.question_TB.Multiline = true;
            this.question_TB.Name = "question_TB";
            this.question_TB.Size = new System.Drawing.Size(1186, 190);
            this.question_TB.TabIndex = 0;
            this.question_TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(20, 20);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1576, 1205);
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
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Size = new System.Drawing.Size(1568, 1172);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "add Question";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // addQuestion_BTN
            // 
            this.addQuestion_BTN.Location = new System.Drawing.Point(24, 1120);
            this.addQuestion_BTN.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.addQuestion_BTN.Name = "addQuestion_BTN";
            this.addQuestion_BTN.Size = new System.Drawing.Size(168, 35);
            this.addQuestion_BTN.TabIndex = 14;
            this.addQuestion_BTN.Text = "הוסף שאלה למאגר";
            this.addQuestion_BTN.UseVisualStyleBackColor = true;
            this.addQuestion_BTN.Click += new System.EventHandler(this.addQuestion_BTN_Click);
            // 
            // subQuestionsPanel
            // 
            this.subQuestionsPanel.AutoScroll = true;
            this.subQuestionsPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.subQuestionsPanel.Location = new System.Drawing.Point(24, 346);
            this.subQuestionsPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.subQuestionsPanel.Name = "subQuestionsPanel";
            this.subQuestionsPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.subQuestionsPanel.Size = new System.Drawing.Size(1532, 765);
            this.subQuestionsPanel.TabIndex = 13;
            this.subQuestionsPanel.WrapContents = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1458, 322);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 20);
            this.label6.TabIndex = 12;
            this.label6.Text = "תתי שאלות";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1305, 269);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(250, 35);
            this.button2.TabIndex = 10;
            this.button2.Text = "הוסף תת שאלה";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // subSubject_TB
            // 
            this.subSubject_TB.Location = new System.Drawing.Point(24, 111);
            this.subSubject_TB.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.subSubject_TB.Name = "subSubject_TB";
            this.subSubject_TB.Size = new System.Drawing.Size(214, 26);
            this.subSubject_TB.TabIndex = 9;
            // 
            // subject_TB
            // 
            this.subject_TB.Location = new System.Drawing.Point(24, 68);
            this.subject_TB.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.subject_TB.Name = "subject_TB";
            this.subject_TB.Size = new System.Drawing.Size(214, 26);
            this.subject_TB.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(261, 115);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "תת נושא";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(286, 72);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "נושא";
            // 
            // pictureName_LBL
            // 
            this.pictureName_LBL.AutoSize = true;
            this.pictureName_LBL.Location = new System.Drawing.Point(280, 240);
            this.pictureName_LBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pictureName_LBL.Name = "pictureName_LBL";
            this.pictureName_LBL.Size = new System.Drawing.Size(49, 20);
            this.pictureName_LBL.TabIndex = 5;
            this.pictureName_LBL.Text = "תמונה";
            this.pictureName_LBL.Visible = false;
            // 
            // loadPic
            // 
            this.loadPic.Location = new System.Drawing.Point(24, 200);
            this.loadPic.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.loadPic.Name = "loadPic";
            this.loadPic.Size = new System.Drawing.Size(314, 35);
            this.loadPic.TabIndex = 4;
            this.loadPic.Text = "טען תמונה לשאלת מקור";
            this.loadPic.UseVisualStyleBackColor = true;
            this.loadPic.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(249, 163);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
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
            this.questionType_CB.Location = new System.Drawing.Point(24, 158);
            this.questionType_CB.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.questionType_CB.Name = "questionType_CB";
            this.questionType_CB.Size = new System.Drawing.Size(214, 28);
            this.questionType_CB.TabIndex = 2;
            this.questionType_CB.SelectedIndexChanged += new System.EventHandler(this.questionType_CB_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1452, 43);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "טקסט שאלה";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.userAnswerTreeView);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.questionInput_TB);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.resualt_TB3);
            this.tabPage2.Controls.Add(this.button4);
            this.tabPage2.Controls.Add(this.answerTreeView);
            this.tabPage2.Controls.Add(this.send_BTN2);
            this.tabPage2.Controls.Add(this.userAnswer_TB2);
            this.tabPage2.Controls.Add(this.systemAnswer_TB);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Size = new System.Drawing.Size(1568, 1172);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "debug";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(420, 33);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(150, 20);
            this.label11.TabIndex = 33;
            this.label11.Text = "user answer entities";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1394, 349);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 20);
            this.label10.TabIndex = 31;
            this.label10.Text = "user answer";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1375, 137);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 20);
            this.label9.TabIndex = 30;
            this.label9.Text = "system answer";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(169, 20);
            this.label8.TabIndex = 29;
            this.label8.Text = "system answer entities";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1419, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 20);
            this.label5.TabIndex = 26;
            this.label5.Text = "question";
            // 
            // questionInput_TB
            // 
            this.questionInput_TB.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.questionInput_TB.Location = new System.Drawing.Point(884, 57);
            this.questionInput_TB.Multiline = true;
            this.questionInput_TB.Name = "questionInput_TB";
            this.questionInput_TB.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.questionInput_TB.Size = new System.Drawing.Size(605, 70);
            this.questionInput_TB.TabIndex = 25;
            this.questionInput_TB.Text = "שאלה";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(312, 1145);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(322, 34);
            this.button1.TabIndex = 24;
            this.button1.Text = "update entity table";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // resualt_TB3
            // 
            this.resualt_TB3.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resualt_TB3.Location = new System.Drawing.Point(424, 560);
            this.resualt_TB3.Multiline = true;
            this.resualt_TB3.Name = "resualt_TB3";
            this.resualt_TB3.ReadOnly = true;
            this.resualt_TB3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.resualt_TB3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.resualt_TB3.Size = new System.Drawing.Size(1065, 222);
            this.resualt_TB3.TabIndex = 23;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(884, 506);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(112, 34);
            this.button4.TabIndex = 22;
            this.button4.Text = "compare";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // answerTreeView
            // 
            this.answerTreeView.Location = new System.Drawing.Point(27, 57);
            this.answerTreeView.Margin = new System.Windows.Forms.Padding(4);
            this.answerTreeView.Name = "answerTreeView";
            treeNode2.Name = "root";
            treeNode2.Text = "root";
            this.answerTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.answerTreeView.Size = new System.Drawing.Size(374, 725);
            this.answerTreeView.TabIndex = 21;
            // 
            // send_BTN2
            // 
            this.send_BTN2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.send_BTN2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.send_BTN2.Location = new System.Drawing.Point(-72, 1079);
            this.send_BTN2.Name = "send_BTN2";
            this.send_BTN2.Size = new System.Drawing.Size(100, 38);
            this.send_BTN2.TabIndex = 20;
            this.send_BTN2.Text = "send";
            this.send_BTN2.UseVisualStyleBackColor = true;
            // 
            // userAnswer_TB2
            // 
            this.userAnswer_TB2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userAnswer_TB2.Location = new System.Drawing.Point(884, 374);
            this.userAnswer_TB2.Multiline = true;
            this.userAnswer_TB2.Name = "userAnswer_TB2";
            this.userAnswer_TB2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.userAnswer_TB2.Size = new System.Drawing.Size(605, 125);
            this.userAnswer_TB2.TabIndex = 19;
            this.userAnswer_TB2.Text = "מתאים להחלטה של האום ומאפשר גיוס לצהל";
            // 
            // systemAnswer_TB
            // 
            this.systemAnswer_TB.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.systemAnswer_TB.Location = new System.Drawing.Point(884, 160);
            this.systemAnswer_TB.Multiline = true;
            this.systemAnswer_TB.Name = "systemAnswer_TB";
            this.systemAnswer_TB.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.systemAnswer_TB.Size = new System.Drawing.Size(605, 186);
            this.systemAnswer_TB.TabIndex = 15;
            this.systemAnswer_TB.Text = "לנצל את ההזדמנות | פגיעה במורל אם לא יכריזו | לאפשר חוק גיוס חובה ולחזק את הצבא |" +
    " הפסקת אש תחזק את מדינות ערב | תואמת את החלטת האו\"ם |";
            // 
            // userAnswerTreeView
            // 
            this.userAnswerTreeView.Location = new System.Drawing.Point(424, 57);
            this.userAnswerTreeView.Margin = new System.Windows.Forms.Padding(4);
            this.userAnswerTreeView.Name = "userAnswerTreeView";
            treeNode1.Name = "root";
            treeNode1.Text = "root";
            this.userAnswerTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.userAnswerTreeView.Size = new System.Drawing.Size(395, 483);
            this.userAnswerTreeView.TabIndex = 34;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1614, 844);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
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
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox questionInput_TB;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox resualt_TB3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TreeView answerTreeView;
        private System.Windows.Forms.Button send_BTN2;
        private System.Windows.Forms.TextBox userAnswer_TB2;
        private System.Windows.Forms.TextBox systemAnswer_TB;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TreeView userAnswerTreeView;
    }
}

