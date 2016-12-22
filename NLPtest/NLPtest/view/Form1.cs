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
        public Form1()
        {
            InitializeComponent();
            HebDictionary.loadWords();

        }

  //      public BotControler bot = new BotControler();

        private void Form1_Load(object sender, EventArgs e)
        {
            input_TB.Select();
            BotControler.initialize();

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
            var botResualt = BotControler.sendMessage(inputText);
            foreach (var line in botResualt)
            {
                text_TB.AppendText(line + Environment.NewLine);
            }



        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void input_TB_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
