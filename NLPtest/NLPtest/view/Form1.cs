﻿
using Model.dataBase;
using NLPtest.Controllers;
using NLPtest.HebWords;
using NLPtest.QnA;
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
        List<WorldObject> answer1 = new List<WorldObject>();
        List<WorldObject> answer2 = new List<WorldObject>();

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
            var httpCtrl = new OuterAPIController();

            //foreach (var line in new string[] { input.ToString() })
            //{
            //    text_TB.AppendText(line + Environment.NewLine);
            //}
            answer1 = botResualt;
            drawTree(botResualt,treeView);
            text_TB.AppendText(log + Environment.NewLine);

        }

        private void drawTree(List<WorldObject> content,TreeView tv)
        {
            tv.Nodes.Clear();
            while (content.Count > 0)
            {
                var c = content.FirstOrDefault();
                content.Remove(c);
                var objectNode = drawObject(c);
                tv.Nodes.Add(objectNode);
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

        private void send_BTN2_Click(object sender, EventArgs e)
        {
            var inputText = input_TB2.Text;
            text_TB2.AppendText("User:" + Environment.NewLine);
            text_TB2.AppendText(inputText + Environment.NewLine);
            input_TB2.Text = String.Empty;
            text_TB2.AppendText("Bot:" + Environment.NewLine);
            string log = "";
            var botResualt = NLPControler.getInstence().testAnalizer(inputText, out log);
            var httpCtrl = new OuterAPIController();

            //foreach (var line in new string[] { input.ToString() })
            //{
            //    text_TB.AppendText(line + Environment.NewLine);
            //}
            answer2 = botResualt;
            drawTree(botResualt,treeView2);
            text_TB2.AppendText(log + Environment.NewLine);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QAEngin qna = new QAEngin();
            string str;
            var subQuestion = new SubQuestion();
            subQuestion.answerText = input_TB.Text;
            subQuestion.flags = "needAll";
            var match = qna.matchAnswers(subQuestion,
                input_TB2.Text);
            var answerText = "Score:" + match.score + Environment.NewLine;
            foreach (var ent in match.missingEntitis)
            {
                answerText += "Missing Entity:" + ent.entityValue + Environment.NewLine;
              
            }

            foreach (var ans in match.missingAnswers)
            {
                answerText += "Missing Answer:" + ans + Environment.NewLine;

            }
            text_TB3.Text = answerText;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NLPControler.getInstence().updateEntityTable();
        }
    }
}
