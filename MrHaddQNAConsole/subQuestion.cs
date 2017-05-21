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
                foreach (var ent in entity)
                {
                    if(ent is multyEntity)
                    {
                        var entMu = ent as multyEntity;
                        var syn = entMu.singleValue != null && entMu.singleValue != "" ? entMu.parts + entMu.singleValue + ";" : entMu.parts;
                        entitiesDGV.Rows.Add(entMu.entityValue, entMu.entityType, syn);
                    }
                    else
                    {
                        var entR = ent as entity;
                        entitiesDGV.Rows.Add(entR.entityValue, entR.entityType, entR.entitySynonimus);
                    }
                }
            }
        }
    }
}
