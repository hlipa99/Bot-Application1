using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Xml;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Helpers;

using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace wikipedia
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           

            try
            {

                
             
            }
            catch (Exception ex)
            {
                richTextBox1.Text = ex.ToString();
            }
        }


        private void buttonBible_Click(object sender, EventArgs e)
        {

        }

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            Controler co = new Controler();
            co.StartHistoryRendom("היסטוריה");
        }
    }
}
