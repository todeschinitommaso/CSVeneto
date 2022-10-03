using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CSVeneto
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

        private void searchbutton_Click(object sender, EventArgs e)
        {
            string cerca = textBox1.Text.ToUpper();
            namebox.Text = (Search(path, cerca));
        }

        public string path = "veneto_verona.csv";

        static string Search(string filename, string name)
        {
            string line;
            var f = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite); //accesso al file binario 
            BinaryReader reader = new BinaryReader(f);
            BinaryWriter writer = new BinaryWriter(f);

            int totrecord = Convert.ToInt32(f.Length); //byte totali
            totrecord /= 528; //numero record

            string result;

            int lung = Convert.ToInt32(f.Length);
            int i = 0, j = totrecord - 1, m, pos = -1;

            do //ricerca dicotomica
            {
                m = (i + j) / 2;
                f.Seek(m * 528, SeekOrigin.Begin);
                line = Encoding.ASCII.GetString(reader.ReadBytes(528));
                result = FromString(line, 0);

                if (myCompare(result, name) == 0)
                {
                    pos = m;
                }

                else if (myCompare(result, name) == -1)
                {
                    i = m + 1;
                }

                else
                {
                    j = m - 1;
                }


            } while (i <= j && pos == -1);

            if (pos == -1)
            {
                throw new Exception("Comune non trovato");
            }

            string end = FromString(line, 7);
            f.Close();
            return end;

        }

        static int myCompare(string stringa1, string stringa2)
        {
            if (stringa1 == stringa2) //0=uguali 1=stringa prima -1=stringa dopo
            {
                return 0;
            }

            char[] c1 = stringa1.ToCharArray();
            char[] c2 = stringa2.ToCharArray();
            int l = c1.Length;

            if (c2.Length < l) //l = lunghezza minore
            {
                l = c2.Length;
            }

            for (int i = 0; i < l; i++)
            {
                if (c1[i] < c2[i])
                {
                    return -1;
                }

                if (c1[i] > c2[i])
                {
                    return 1;
                }
            }

            return 0;
        }
        public static string FromString(string Stringa, int pos, string sep = ";") //separa i campi
        {
            string[] ris = Stringa.Split(';');
            return ris[pos];

        }

    }
}
