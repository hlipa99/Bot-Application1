
using Model.dataBase;
using NLPtest.Controllers;
using NLPtest.WorldObj;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NLPtest
{
    public partial class Form1 : Form
    {
        HebDictionary heb;
     //   DataBaseControler

        public Form1()
        {
            InitializeComponent();
            heb = new HebDictionary();
            heb.loadWords();
            

        }

  //      public BotControler bot = new BotControler();

        private void Form1_Load(object sender, EventArgs e)
        {
            input_TB.Select();
            

        }

        private void input_TB_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (Char.IsControl(e.KeyChar))
            {
              //  button.PerformClick();
            }
        }


        private void send_BTN_Click(object sender, EventArgs e)
        {
            var inputText = input_TB.Text;
            text_TB.AppendText("User:" + Environment.NewLine);
            text_TB.AppendText(inputText + Environment.NewLine);
            input_TB.Text = String.Empty;
            text_TB.AppendText("Bot:" + Environment.NewLine);
            string log = "";
            var botResualt = NLPControler.getInstence().testAnalizer(inputText,out log);
            var httpCtrl = new HttpController();
           
            //foreach (var line in new string[] { input.ToString() })
            //{
            //    text_TB.AppendText(line + Environment.NewLine);
            //}
            drawTree(botResualt);
            text_TB.AppendText(log + Environment.NewLine);

        }

        private void drawTree(ContentTurn content)
        {
            treeView.Nodes.Clear();
            while (!content.empty())
            {
                var c = content.pop();
                var objectNode = drawObject(c);
                treeView.Nodes.Add(objectNode);
            }
        }

        private TreeNode drawObject(IWorldObject obj)
        {
            var objectNode = new TreeNode();
            if (obj!= null)
            {
                objectNode.Text = obj.ToString();
                foreach (var r in (obj as WorldObject).Relations)
                {
                    var rNode = drawObject(r as RelationObject);
                    var objec = drawObject(r.Objective as WorldObject);
                    rNode.Nodes.Add(objec);
                    objectNode.Nodes.Add(rNode);
                }



            }else
            {
                objectNode.Text = "null object";
            }
                return objectNode;
        }




        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void input_TB_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
