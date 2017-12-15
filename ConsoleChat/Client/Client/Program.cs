using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            client();
        }

        public static void client()
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);

            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                server.Connect(ip); //Connect to the server
            }
            catch (SocketException ex)
            {
                //Console.WriteLine(ex.Message);
                Console.WriteLine("Server not found");
                Thread.Sleep(2000);
                client();
                //return;
            }

            Console.WriteLine("Connected to server ");

            while (true)
            {

                try
                {
                    byte[] data = new byte[8000];
                    string input;
                    int receivedDataLength = server.Receive(data); //Wait for the data
                    string stringData = Encoding.ASCII.GetString(data, 0, receivedDataLength); //Decode the data received
                    Console.WriteLine(stringData); //Write the data on the screen

                    if (stringData == "REQ_STOP")
                    {
                        input = "ALL STOP";
        
                        server.Send(Encoding.ASCII.GetBytes(input));

                  
                        break;
                    }

                    if (stringData == "REQ_SPY")
                    {
                        input = "SPY";
                        Thread.Sleep(5000);
                        server.Send(Encoding.ASCII.GetBytes(input));
                    }

                    if (stringData == "REQ_PB")
                    {
                        input = "PB";
                        Thread.Sleep(1000);
                        server.Send(Encoding.ASCII.GetBytes(input));
                    }

                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }

            }

            Console.WriteLine("STOP");

            server.Shutdown(SocketShutdown.Both);
            server.Close();

            //server.Shutdown(SocketShutdown.Both);
            //server.Close();
        }
    }
}