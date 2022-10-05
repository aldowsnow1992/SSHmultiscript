using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Renci;
using Renci.SshNet;

namespace angelito
{
    public partial class Form1 : Form
    {
        List<Server> L = new List<Server>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btniniciar_Click(object sender, EventArgs e)
        {
            new Task(() => RunCommand()).Start();
        }
        public void testejecucion()
        {
            using (var client = new SshClient("hostnameOrIp", "username", "password"))
            {

            }
        }




        private void RunCommand()
        {
            var host = "192.168.0.110";
            var username = "ubuntu";
            var password = "M64z1992";

            using (var client = new SshClient(host, username, password))
            {
                client.Connect();
                var cmd = client.CreateCommand("whoami; ifconfig; whoami");

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

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

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
