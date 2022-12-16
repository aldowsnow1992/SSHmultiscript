using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Renci;
using System.Threading;
using Renci.SshNet;

namespace angelito
{
    public partial class Form1 : Form
    {
        List<Server> L = new List<Server>();
        string logglobal;

        public Form1()
        {
            InitializeComponent();
        }

        private void btniniciar_Click(object sender, EventArgs e)
        {
            rtb.Clear();
            logglobal = "";
            foreach (var item in L)
            {
                //new Task(() => RunCommand(item.IPP,item.USER,item.CONTR,item.COMAN)).Start();
                RunCommand(item.IPP, item.USER, item.CONTR, item.COMAN);
                logglobal = logglobal + rtb.Text;
                GenerarTXT(rtb.Text, item.IPP);
                rtb.Clear();
            }
            MessageBox.Show("finalizado");
            rtb.Text = logglobal;
            
        }



        private void RunCommand(string H,string u,string p, string c)
        {
            var host = H;
            var username = u;
            var password = p;

            try
            {
                using (var client = new SshClient(host, username, password))
                {

                    client.Connect();
                    var cmd = client.CreateCommand(c);

                    var result = cmd.BeginExecute();

                    using (var reader = new StreamReader(
                                          cmd.OutputStream, Encoding.UTF8, true, 1024, true))
                    {
                        while (!result.IsCompleted || !reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            if (line != null)
                            {
                                rtb.Invoke(
                                    (MethodInvoker)(() =>
                                        rtb.AppendText(line + Environment.NewLine)));

                            }
                        }
                    }

                    cmd.EndExecute(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Inaccesible al Host usuario o contraseña incorrecto: " + ex);
                //throw;
            }
        }

        private void btnagregar_Click(object sender, EventArgs e)
        {
            Server s = new Server();
            s.IPP = textBox1.Text;
            s.USER = textBox2.Text;
            s.CONTR= textBox3.Text;
            s.COMAN = textBox4.Text;
            L.Add(s);
            IPs.Items.Clear();
            foreach (var item in L)
            {
                IPs.Items.Add(item.IPP);
            }

        }

        private void IPs_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show( "eliminado " + IPs.SelectedItem.ToString());

            var d = L.FirstOrDefault(x=> x.IPP== IPs.SelectedItem.ToString());
            L.Remove(d);
            IPs.Items.Clear();
            foreach (var item in L)
            {
                IPs.Items.Add(item.IPP);
            }

        }

        public void GenerarTXT(string dat,string h)
        {
            
            string texto = dat;

            using (StreamWriter mylogs = new StreamWriter(Path.Combine(Application.StartupPath, h+" "+DateTime.Now.Millisecond.ToString()+" " +" log.txt"), append: true))         //se crea el archivo
            {

                //se adiciona alguna información y la fecha

                mylogs.WriteLine(texto);

                mylogs.Close();


            }
        }

        private void rtb_TextChanged(object sender, EventArgs e)
        {

        }
    }//fin
    public class Server
    {
        public string IPP { get; set; }
        public string USER { get; set; }
        public string CONTR { get; set; }
        public string COMAN { get; set; }



       
    }

    public class info
        {
        public static string ipdata;
        public static string  userdata;
        public static string passdata;
        public static string data;
    }



}
