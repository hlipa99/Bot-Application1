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
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("root");
            this.text_TB = new System.Windows.Forms.TextBox();
            this.input_TB = new System.Windows.Forms.TextBox();
            this.send_BTN = new System.Windows.Forms.Button();
            this.treeView = new System.Windows.Forms.TreeView();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.send_BTN2 = new System.Windows.Forms.Button();
            this.input_TB2 = new System.Windows.Forms.TextBox();
            this.text_TB2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.text_TB3 = new System.Windows.Forms.TextBox();
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
            this.input_TB.Location = new System.Drawing.Point(109, 349);
            this.input_TB.Margin = new System.Windows.Forms.Padding(2);
            this.input_TB.Name = "input_TB";
            this.input_TB.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.input_TB.Size = new System.Drawing.Size(543, 26);
            this.input_TB.TabIndex = 1;
            this.input_TB.Text = "הערבים יצאו במתקפות כלפי היהודים ואינם היו מרוצים מתכנית החלוקה";
            this.input_TB.TextChanged += new System.EventHandler(this.input_TB_TextChanged);
            this.input_TB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.input_TB_KeyPress);
            // 
            // send_BTN
            // 
            this.send_BTN.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.send_BTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.send_BTN.Location = new System.Drawing.Point(37, 349);
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
            this.treeView.Size = new System.Drawing.Size(529, 323);
            this.treeView.TabIndex = 3;
            // 
            // treeView2
            // 
            this.treeView2.Location = new System.Drawing.Point(666, 392);
            this.treeView2.Name = "treeView2";
            treeNode2.Name = "root";
            treeNode2.Text = "root";
            this.treeView2.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.treeView2.Size = new System.Drawing.Size(529, 323);
            this.treeView2.TabIndex = 7;
            // 
            // send_BTN2
            // 
            this.send_BTN2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.send_BTN2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.send_BTN2.Location = new System.Drawing.Point(37, 729);
            this.send_BTN2.Margin = new System.Windows.Forms.Padding(2);
            this.send_BTN2.Name = "send_BTN2";
            this.send_BTN2.Size = new System.Drawing.Size(67, 24);
            this.send_BTN2.TabIndex = 6;
            this.send_BTN2.Text = "send";
            this.send_BTN2.UseVisualStyleBackColor = true;
            this.send_BTN2.Click += new System.EventHandler(this.send_BTN2_Click);
            // 
            // input_TB2
            // 
            this.input_TB2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.input_TB2.Location = new System.Drawing.Point(109, 729);
            this.input_TB2.Margin = new System.Windows.Forms.Padding(2);
            this.input_TB2.Name = "input_TB2";
            this.input_TB2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.input_TB2.Size = new System.Drawing.Size(543, 26);
            this.input_TB2.TabIndex = 5;
            this.input_TB2.Text = "הערבים דחו את ההחלטה והתחילו בהתקפות על יהודים בכל הארץ";
            // 
            // text_TB2
            // 
            this.text_TB2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_TB2.Location = new System.Drawing.Point(22, 391);
            this.text_TB2.Margin = new System.Windows.Forms.Padding(2);
            this.text_TB2.Multiline = true;
            this.text_TB2.Name = "text_TB2";
            this.text_TB2.ReadOnly = true;
            this.text_TB2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.text_TB2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.text_TB2.Size = new System.Drawing.Size(630, 324);
            this.text_TB2.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(577, 772);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "compare";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // text_TB3
            // 
            this.text_TB3.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_TB3.Location = new System.Drawing.Point(666, 736);
            this.text_TB3.Margin = new System.Windows.Forms.Padding(2);
            this.text_TB3.Multiline = true;
            this.text_TB3.Name = "text_TB3";
            this.text_TB3.ReadOnly = true;
            this.text_TB3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.text_TB3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.text_TB3.Size = new System.Drawing.Size(529, 73);
            this.text_TB3.TabIndex = 9;
            // 
            // Form1
            // 
            this.AcceptButton = this.send_BTN;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1276, 820);
            this.Controls.Add(this.text_TB3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.treeView2);
            this.Controls.Add(this.send_BTN2);
            this.Controls.Add(this.input_TB2);
            this.Controls.Add(this.text_TB2);
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
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.Button send_BTN2;
        private System.Windows.Forms.TextBox input_TB2;
        private System.Windows.Forms.TextBox text_TB2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox text_TB3;
    }
}

