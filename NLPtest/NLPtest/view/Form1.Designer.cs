namespace NLPtest
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("root");
            this.text_TB = new System.Windows.Forms.TextBox();
            this.input_TB = new System.Windows.Forms.TextBox();
            this.send_BTN = new System.Windows.Forms.Button();
            this.treeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // text_TB
            // 
            this.text_TB.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_TB.Location = new System.Drawing.Point(22, 11);
            this.text_TB.Margin = new System.Windows.Forms.Padding(2);
            this.text_TB.Multiline = true;
            this.text_TB.Name = "text_TB";
            this.text_TB.ReadOnly = true;
            this.text_TB.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.text_TB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.text_TB.Size = new System.Drawing.Size(630, 324);
            this.text_TB.TabIndex = 0;
            this.text_TB.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // input_TB
            // 
            this.input_TB.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.input_TB.Location = new System.Drawing.Point(109, 366);
            this.input_TB.Margin = new System.Windows.Forms.Padding(2);
            this.input_TB.Name = "input_TB";
            this.input_TB.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.input_TB.Size = new System.Drawing.Size(543, 26);
            this.input_TB.TabIndex = 1;
            this.input_TB.Text = "גרגמל המכשף התגורר ביער. הוא היה רשע!";
            this.input_TB.TextChanged += new System.EventHandler(this.input_TB_TextChanged);
            this.input_TB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.input_TB_KeyPress);
            // 
            // send_BTN
            // 
            this.send_BTN.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.send_BTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.send_BTN.Location = new System.Drawing.Point(37, 366);
            this.send_BTN.Margin = new System.Windows.Forms.Padding(2);
            this.send_BTN.Name = "send_BTN";
            this.send_BTN.Size = new System.Drawing.Size(67, 24);
            this.send_BTN.TabIndex = 2;
            this.send_BTN.Text = "send";
            this.send_BTN.UseVisualStyleBackColor = true;
            this.send_BTN.Click += new System.EventHandler(this.send_BTN_Click);
            // 
            // treeView
            // 
            this.treeView.Location = new System.Drawing.Point(666, 12);
            this.treeView.Name = "treeView";
            treeNode1.Name = "root";
            treeNode1.Text = "root";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeView.Size = new System.Drawing.Size(529, 378);
            this.treeView.TabIndex = 3;
            // 
            // Form1
            // 
            this.AcceptButton = this.send_BTN;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1207, 418);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.send_BTN);
            this.Controls.Add(this.input_TB);
            this.Controls.Add(this.text_TB);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox text_TB;
        private System.Windows.Forms.TextBox input_TB;
        private System.Windows.Forms.Button send_BTN;
        private System.Windows.Forms.TreeView treeView;
    }
}

