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

namespace MrHaddQNAConsole
{
    public partial class Form1 : Form
    {
        //OuterAPIController 
        List<subQuestion> subquestions = new List<subQuestion>();
        NLPControler nlp = new NLPControler();
        DataBaseController db = new DataBaseController();
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
    }
}


