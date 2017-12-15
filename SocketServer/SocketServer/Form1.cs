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
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketServer
{
    class message
    {
        protected string _name = "REQ_PB";
        protected bool _clicked = false;

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        public bool clicked
        {
            get { return _clicked;  }
            set { _clicked = value; }
        }
        

    }

    public partial class Form1 : Form
    {

        IPEndPoint ip;
        Socket socket;
        Socket client;
        IPEndPoint clientep;

        message mess = new message();


        public Form1()
        {
            InitializeComponent();

            new Thread(() =>
            {
                ip = new IPEndPoint(IPAddress.Any, 8080); //Any IPAddress that connects to the server on any port
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //Initialize a new Socket

                socket.Bind(ip); //Bind to the client's IP
                socket.Listen(3); //Listen for maximum 10 connections
                listBox1.Items.Add("Waiting.....");
                client = socket.Accept();
                clientep = (IPEndPoint)client.RemoteEndPoint;
                listBox1.Items.Add(clientep.Address);

            }).Start();

        }

        



        


        private void textBox1_TextChanged(object sender, EventArgs e)
        {


        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            new Thread(() =>
            {
                
                mess.name = textBox1.Text; //This is the data we will respond with
                byte[] data = new byte[8000];
                data = Encoding.ASCII.GetBytes(mess.name); //Encode the data
                client.Send(data, data.Length, SocketFlags.None); //Send the data to the client
                int dataReceived = client.Receive(data);
                string stringData = Encoding.ASCII.GetString(data, 0, dataReceived); //Decode the data received
                listBox1.Items.Add(stringData); //Write the data on the screen
        

            }).Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            client.Close(); //Close Client
            socket.Close(); //Close socket
            listBox1.Items.Add("Server STOP");
        }
    }
}
