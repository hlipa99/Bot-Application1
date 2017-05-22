namespace MrHaddQNAConsole
{
    partial class subQuestion
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SQAddEntiti_BTN = new System.Windows.Forms.Button();
            this.entitiesDGV = new System.Windows.Forms.DataGridView();
            this.idx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entityValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entityType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entitySynunimus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subQuestionText = new System.Windows.Forms.Label();
            this.SQquestionTB = new System.Windows.Forms.TextBox();
            this.subQuestion_Answers_TB = new System.Windows.Forms.TextBox();
            this.sqAns = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SQ_nedded_LBL = new System.Windows.Forms.Label();
            this.SQ_NeededPicker_UDP = new System.Windows.Forms.DomainUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.entitiesDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // SQAddEntiti_BTN
            // 
            this.SQAddEntiti_BTN.Location = new System.Drawing.Point(40, 229);
            this.SQAddEntiti_BTN.Name = "SQAddEntiti_BTN";
            this.SQAddEntiti_BTN.Size = new System.Drawing.Size(112, 23);
            this.SQAddEntiti_BTN.TabIndex = 23;
            this.SQAddEntiti_BTN.Text = "הוסף כיישות";
            this.SQAddEntiti_BTN.UseVisualStyleBackColor = true;
            this.SQAddEntiti_BTN.Click += new System.EventHandler(this.SQAddEntiti_BTN_Click);
            // 
            // entitiesDGV
            // 
            this.entitiesDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.entitiesDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idx,
            this.entityValue,
            this.entityType,
            this.entitySynunimus});
            this.entitiesDGV.Location = new System.Drawing.Point(158, 177);
            this.entitiesDGV.Name = "entitiesDGV";
            this.entitiesDGV.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.entitiesDGV.Size = new System.Drawing.Size(783, 109);
            this.entitiesDGV.TabIndex = 22;
            // 
            // idx
            // 
            this.idx.HeaderText = "#";
            this.idx.Name = "idx";
            this.idx.ReadOnly = true;
            this.idx.Width = 30;
            // 
            // entityValue
            // 
            this.entityValue.HeaderText = "שם היישות";
            this.entityValue.Name = "entityValue";
            // 
            // entityType
            // 
            this.entityType.HeaderText = "סוג היישות";
            this.entityType.Name = "entityType";
            // 
            // entitySynunimus
            // 
            this.entitySynunimus.HeaderText = "ערכים מקבילים";
            this.entitySynunimus.Name = "entitySynunimus";
            this.entitySynunimus.Width = 400;
            // 
            // subQuestionText
            // 
            this.subQuestionText.AutoSize = true;
            this.subQuestionText.Location = new System.Drawing.Point(874, 9);
            this.subQuestionText.Name = "subQuestionText";
            this.subQuestionText.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.subQuestionText.Size = new System.Drawing.Size(69, 13);
            this.subQuestionText.TabIndex = 20;
            this.subQuestionText.Text = "טקסט שאלה";
            // 
            // SQquestionTB
            // 
            this.SQquestionTB.Location = new System.Drawing.Point(40, 25);
            this.SQquestionTB.Multiline = true;
            this.SQquestionTB.Name = "SQquestionTB";
            this.SQquestionTB.Size = new System.Drawing.Size(903, 74);
            this.SQquestionTB.TabIndex = 19;
            // 
            // subQuestion_Answers_TB
            // 
            this.subQuestion_Answers_TB.Location = new System.Drawing.Point(40, 118);
            this.subQuestion_Answers_TB.Multiline = true;
            this.subQuestion_Answers_TB.Name = "subQuestion_Answers_TB";
            this.subQuestion_Answers_TB.Size = new System.Drawing.Size(903, 53);
            this.subQuestion_Answers_TB.TabIndex = 18;
            // 
            // sqAns
            // 
            this.sqAns.AutoSize = true;
            this.sqAns.Location = new System.Drawing.Point(870, 102);
            this.sqAns.Name = "sqAns";
            this.sqAns.Size = new System.Drawing.Size(73, 13);
            this.sqAns.TabIndex = 21;
            this.sqAns.Text = "טקסט תשובה";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(112, 293);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(763, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "_________________________________________________________________________________" +
    "_____________________________________________";
            // 
            // SQ_nedded_LBL
            // 
            this.SQ_nedded_LBL.AutoSize = true;
            this.SQ_nedded_LBL.Location = new System.Drawing.Point(44, 187);
            this.SQ_nedded_LBL.Name = "SQ_nedded_LBL";
            this.SQ_nedded_LBL.Size = new System.Drawing.Size(108, 13);
            this.SQ_nedded_LBL.TabIndex = 25;
            this.SQ_nedded_LBL.Text = "מס\' תשובות נדרשות";
            // 
            // SQ_NeededPicker_UDP
            // 
            this.SQ_NeededPicker_UDP.Items.Add("1");
            this.SQ_NeededPicker_UDP.Items.Add("10");
            this.SQ_NeededPicker_UDP.Items.Add("2");
            this.SQ_NeededPicker_UDP.Items.Add("3");
            this.SQ_NeededPicker_UDP.Items.Add("4");
            this.SQ_NeededPicker_UDP.Items.Add("5");
            this.SQ_NeededPicker_UDP.Items.Add("6");
            this.SQ_NeededPicker_UDP.Items.Add("7");
            this.SQ_NeededPicker_UDP.Items.Add("8");
            this.SQ_NeededPicker_UDP.Items.Add("9");
            this.SQ_NeededPicker_UDP.Location = new System.Drawing.Point(104, 203);
            this.SQ_NeededPicker_UDP.Name = "SQ_NeededPicker_UDP";
            this.SQ_NeededPicker_UDP.Size = new System.Drawing.Size(48, 20);
            this.SQ_NeededPicker_UDP.Sorted = true;
            this.SQ_NeededPicker_UDP.TabIndex = 24;
            this.SQ_NeededPicker_UDP.Text = "1";
            this.SQ_NeededPicker_UDP.Wrap = true;
            // 
            // subQuestion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SQ_nedded_LBL);
            this.Controls.Add(this.SQ_NeededPicker_UDP);
            this.Controls.Add(this.SQAddEntiti_BTN);
            this.Controls.Add(this.entitiesDGV);
            this.Controls.Add(this.subQuestionText);
            this.Controls.Add(this.SQquestionTB);
            this.Controls.Add(this.subQuestion_Answers_TB);
            this.Controls.Add(this.sqAns);
            this.Name = "subQuestion";
            this.Size = new System.Drawing.Size(975, 307);
            ((System.ComponentModel.ISupportInitialize)(this.entitiesDGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button SQAddEntiti_BTN;
        private System.Windows.Forms.DataGridView entitiesDGV;
        private System.Windows.Forms.Label subQuestionText;
        private System.Windows.Forms.TextBox SQquestionTB;
        private System.Windows.Forms.TextBox subQuestion_Answers_TB;
        private System.Windows.Forms.Label sqAns;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label SQ_nedded_LBL;
        private System.Windows.Forms.DomainUpDown SQ_NeededPicker_UDP;
        private System.Windows.Forms.DataGridViewTextBoxColumn idx;
        private System.Windows.Forms.DataGridViewTextBoxColumn entityValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn entityType;
        private System.Windows.Forms.DataGridViewTextBoxColumn entitySynunimus;
    }
}
