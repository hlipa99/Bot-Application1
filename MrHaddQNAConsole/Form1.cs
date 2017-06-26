using com.sun.msv.reader;
using Model.dataBase;
using Model.Models;
using NLP.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;

using NLP.HebWords;
using NLP.QnA;
using NLP.WorldObj;
using NLPtest.QnA;


namespace MrHaddQNAConsole
{
    public partial class Form1 : Form
    {
        //OuterAPIController 
        List<subQuestion> subquestions = new List<subQuestion>();
        NLPControler nlp = new NLPControler();
        DataBaseController db = new DataBaseController();

        List<WorldObject> answer1 = new List<WorldObject>();
        List<WorldObject> answer2 = new List<WorldObject>();

        QuestionsAnswersControllers qnaCont = new QuestionsAnswersControllers();


        public Form1()
        {
            InitializeComponent();
        }

        private void addQuestion_BTN_Click(object sender, EventArgs e)
        {
            try
            {
                IQuestion question = new Question();
                question.QuestionText = question_TB.Text;
                question.SubCategory = subSubject_TB.Text;
                question.Category = subject_TB.Text;
                question.Flags = questionType_CB.Text == "שאלת מקור" ? "sorcePic" : null;
                if (questionType_CB.Text == "שאלת מקור")
                {
                    //loadPic
                }
                question.SubQuestion = new List<SubQuestion>();


                var entityList = new List<IentityBase>();
                foreach (var sq in subquestions)
                {
                    var subQuestion = new SubQuestion();
                    subQuestion = sq.getSubQuestion();
                    if (subQuestion == null) throw new Exception();
                    question.SubQuestion.Add(subQuestion);

                    entityList.AddRange(sq.updatedEntityList());
                }

                db.addUpdateQuestion((Question)question);


                nlp.updateEntities(entityList.Distinct());
            }catch(Exception ex)
            {
                MessageBox.Show("אין אפשרות להוסיף את השאלה, יש למלא את כל הסעיפים הנדרשים");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var newSQ = new subQuestion();
            subquestions.Add(newSQ);
            subQuestionsPanel.Controls.Add(newSQ);
        }

        private void questionType_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(questionType_CB.Text == "שאלת מקור")
            {
                loadPic.Visible = true;
            }
            else
            {
                loadPic.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

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


        }

        private void drawTree(List<WorldObject> content, TreeView tv)
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
            if (obj != null)
            {
                objectNode.Text = obj.ToString();
                foreach (var r in (obj as WorldObject).Relations)
                {
                    var rNode = drawObject(r as RelationObject);
                    var objec = drawObject(r.Objective as WorldObject);
                    rNode.Nodes.Add(objec);
                    objectNode.Nodes.Add(rNode);
                }

            }
            else
            {
                objectNode.Text = "null object";
            }
            return objectNode;
        }





        public virtual string getPhrase(Pkey key, string[] flags = null, string[] flagesNot = null, string textVar = null)
        {
            Logger.addLog("Bot: " + Enum.GetName(typeof(Pkey), key));

            if (flags == null) flags = new string[] { };
            if (flagesNot == null) flagesNot = new string[] { };

            var phrases = new DataBaseController().getBotPhrase(key, flags, flagesNot);
            string phraseRes = null;
            if (phrases.Length > 0)
            {

                var rundomInt = 0;
                phraseRes = phrases[rundomInt];

            }
            else
            {
                //   throw new botphraseException();
            }

            if (phraseRes != null)
            {
                return phraseRes;
            }
            else
            {
                return "";
            }

        }



        private void button4_Click(object sender, EventArgs e)
        {
            var inputText = systemAnswer_TB.Text;

            string log = "";
            var botResualt = new NLPControler().testAnalizer(inputText, out log, questionInput_TB.Text);
            drawTree(botResualt, answerTreeView);

            inputText = userAnswer_TB2.Text;
            var botResualt2 = new NLPControler().testAnalizer(inputText, out log, questionInput_TB.Text);
            drawTree(botResualt2, userAnswerTreeView);


            QAEngin qna = new QAEngin();
                string str;
                var subQuestion = new SubQuestion();
                subQuestion.questionText = questionInput_TB.Text;
                subQuestion.answerText = systemAnswer_TB.Text;
                subQuestion.flags = "need1";
                var match = qna.matchAnswers(subQuestion,
                    userAnswer_TB2.Text);
                var answerText = "Score:" + match.score + Environment.NewLine;
                foreach (var ent in match.missingEntitis)
                {
                    answerText += "Missing Entity:" + ent.entityValue + Environment.NewLine;

                }

                foreach (var ans in match.answersFeedbacks.Where(x => x.score < 75))
                {
                    answerText += "Missing Answer:" + ans.answer + Environment.NewLine;

                }

                var res = qnaCont.createFeedBack(match);
                foreach (var s in res)
                {
                    answerText += s + Environment.NewLine;
                }

                resualt_TB3.Text = answerText;
            
        }
    }
}


