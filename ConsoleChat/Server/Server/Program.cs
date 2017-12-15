using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            server();
        }

        public static void server()
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 8080); //Any IPAddress that connects to the server on any port
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //Initialize a new Socket

            socket.Bind(ip); //Bind to the client's IP
            socket.Listen(3); //Listen for maximum 10 connections
            Console.WriteLine("Waiting for a client...");
            Socket client = socket.Accept();
            IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
            Console.WriteLine("Connected with {0} at port {1}", clientep.Address, clientep.Port);

            try
            {

                while (true)
                {
                    string input = Console.ReadLine(); //This is the data we will respond with
                    if (input == "exit")
                        break;
                    byte[] data = new byte[1024];
                    data = Encoding.ASCII.GetBytes(input); //Encode the data
                    client.Send(data, data.Length, SocketFlags.None); //Send the data to the client
                    int dataReceived = client.Receive(data);
                    string stringData = Encoding.ASCII.GetString(data, 0, dataReceived); //Decode the data received
                    Console.WriteLine(stringData); //Write the data on the screen
                }


                Console.WriteLine("Disconnected from {0}", clientep.Address);
                client.Close(); //Close Client
                socket.Close(); //Close socket


            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }


            //Console.WriteLine("Disconnected from {0}", clientep.Address);
            //client.Close(); //Close Client
            //socket.Close(); //Close socket
        }
    }
}