using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstaHack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string getURL()
        {
            List<string> stuff = new List<string>();
            string user = textBox1.Text.Trim();
            string url = "https://www.instagram.com/" + user + "/";
            string code;
            string imgurl = "";

            using (WebClient c = new WebClient())
            {
                code = c.DownloadString(url);
            }

            HtmlAgilityPack.HtmlDocument maal = new HtmlAgilityPack.HtmlDocument();
            maal.LoadHtml(code);
            foreach (HtmlAgilityPack.HtmlNode node in maal.DocumentNode.SelectNodes("//script[@type='text/javascript']"))
            {
                stuff.Add(node.InnerHtml);
            }

            var reg = new Regex("\"https.*?\"");
            var matches = reg.Matches(stuff[4]);

            imgurl = matches[0].Value;
            if (imgurl.Length < 130)
            {
                return imgurl;
            }
            else
            {
                return "invalid";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string imgurl = "invalid";

            while (imgurl == "invalid")
            {
                imgurl = getURL();
            }

            imgurl = imgurl.Replace("\"", string.Empty);
            imgurl = imgurl.Remove(58, 8);

            pictureBox1.Load(imgurl);
            WebClient gg = new WebClient();
            string loc = textBox1.Text + ".jpg";
            gg.DownloadFile(imgurl, loc);
        }
    }
}
