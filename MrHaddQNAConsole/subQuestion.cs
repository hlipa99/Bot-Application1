using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLP.Controllers;
using Model.dataBase;
using Model.Models;

namespace MrHaddQNAConsole
{
    public partial class subQuestion : UserControl
    {
        NLPControler nlp = new NLPControler();


        public subQuestion()
        {
            InitializeComponent();
        }

        private void SQAddEntiti_BTN_Click(object sender, EventArgs e)
        {
            var text = subQuestion_Answers_TB.SelectedText;
            
            if (text == null || text == "")
            {
                MessageBox.Show("סמן את היישות המבוקשת בטקסט");
            }
            else
            {
                var entity = nlp.findMatchingEntities(text);
                int i = 0;
                foreach (var ent in entity)
                {
                    if(ent is multyEntity)
                    {
                        var entMu = ent as multyEntity;
                        var syn = entMu.singleValue != null && entMu.singleValue != "" ? entMu.parts + entMu.singleValue + ";" : entMu.parts;
                        entitiesDGV.Rows.Add(i,entMu.entityValue, entMu.entityType, syn);
                    }
                    else
                    {
                        var entR = ent as entity;
                        entitiesDGV.Rows.Add(i,entR.entityValue, entR.entityType, entR.entitySynonimus);
                    }
                    i++;
                }
            }
        }

        internal SubQuestion getSubQuestion()
        {
            SubQuestion sq = new SubQuestion();
            sq.answerText = subQuestion_Answers_TB.Text.ToString(); ;
            if (SQ_NeededPicker_UDP.Text == null || SQ_NeededPicker_UDP.Text == "" ||
                int.Parse(SQ_NeededPicker_UDP.Text) > sq.answerText.Split('|').Where(t=>t.Trim().Length > 0).Count())
            {
                MessageBox.Show("מס' התשובות הנדרשות גדול ממספר התשובות הקיימות");
                return null;
            }
            sq.flags = "need" + SQ_NeededPicker_UDP.Text;
            sq.questionText = SQquestionTB.Text.ToString(); ;

            return sq;

        }

        public List<IentityBase> updatedEntityList()
        {
            List<IentityBase> entList = new List<IentityBase>() ;
            for (int i = 0; i < entitiesDGV.Rows.Count; i++)
            {
                if (entitiesDGV.Rows[i].Cells[1].Value != null)
                {
                    var value = entitiesDGV.Rows[i].Cells[1].Value.ToString();
                    var type = entitiesDGV.Rows[i].Cells[2].Value.ToString();
                    var syn = entitiesDGV.Rows[i].Cells[3].Value.ToString();

                    if (syn.Contains("#"))
                    {
                        var entS = new multyEntity();
                        entS.entityValue = value;
                        entS.entityType = type;
                        entS.parts = concat(syn.Split(';').Where(me => me.Count() > 0 && me.Split('#').Count() > 1));
                        entS.singleValue = syn.Split(';').Where(me => me.Count() > 0 && me.Split('#').Count() == 1).FirstOrDefault();

                    }
                    else
                    {
                        var entS = new entity();
                        entS.entityValue = value;
                        entS.entityType = type;
                        entS.entitySynonimus = syn;
                        entList.Add(entS);
                    }
                }
            }

            return entList;
        }

        private string concat(IEnumerable<string> list)
        {
            var res = ";";
            foreach(var s in list)
            {
                res += s + ";";
            }
            return res;
        }

    }
}
