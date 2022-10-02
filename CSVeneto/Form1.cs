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
            namebox.Text = (Ricerca(filename, cerca));
        }

        public string filename = "veneto_verona.csv";

        static string Ricerca(string filename, string nomecercato)
        {
            string line;
            var f = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite); //accesso al file binario 
            BinaryReader reader = new BinaryReader(f);
            BinaryWriter writer = new BinaryWriter(f);

            int righetot = Convert.ToInt32(f.Length); //byte totali
            int lunghezzariga = 528;
            righetot /= 528; //numero record

            string result;

            int lung = Convert.ToInt32(f.Length);
            int i = 0, j = righetot - 1, m, pos = -1;

            do //ricerca dicotomica
            {
                m = (i + j) / 2;
                f.Seek(m * lunghezzariga, SeekOrigin.Begin);
                line = Encoding.ASCII.GetString(reader.ReadBytes(lunghezzariga));
                result = FromString(line, 0);

                if (myCompare(result, nomecercato) == 0)
                {
                    pos = m;
                }

                else if (myCompare(result, nomecercato) == -1)
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
                throw new Exception("campo non trovato");
            }
               
            string fine = FromString(line, 7);
            f.Close();
            return fine;

        }

        static int myCompare(string stringa1, string stringa2)
        {
            if (stringa1 == stringa2) //0=uguali 1=stringa prima -1=stringa dopo
            {
                return 0;
            }

            char[] char1 = stringa1.ToCharArray();
            char[] char2 = stringa2.ToCharArray();
            int l = char1.Length;

            if (char2.Length < l) //l = lunghezza minore
            {
                l = char2.Length;
            }

            for (int i = 0; i < l; i++)
            {
                if (char1[i] < char2[i])
                {
                    return -1;
                }

                if (char1[i] > char2[i])
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
