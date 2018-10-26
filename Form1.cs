using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Tumultu_x
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileInfo f;
            bool res;
            try
            {
                res = GUI.OpenFile(out f);
                goButton.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            fPathNameBox.Text = res ? f.FullName : "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GUI.GoButton(int.TryParse(samplesTextBox.Text, out int x) ? x : 256, fPathNameBox.Text, out StringBuilder sb, ref chart1);
            string s1 = "", s2 = "", s3 = "";
            GUI.HashUpdate(fPathNameBox.Text, ref s1, ref s2, ref s3);
            md5.Text = s1; sha1.Text = s2; sha256.Text = s3;
            chart1.DataBind();
            entResBox.Text = sb.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
