using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using static System.Console;
using System.Runtime.Remoting.Messaging;

namespace ClientSocket
{
    internal class Program
    {
        static void Main(string[] args)
        {


            // 0 net connection params
            int port = 9001;
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(ip, port);


            try
            {
                string ans = "n";
                do
                {

                    Write("\n> Type your Zodiak Sign: ");
                    string message = ReadLine();
                    byte[] message_buffer = Encoding.UTF8.GetBytes(message);

                    Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    client.Connect(endPoint);
                    client.Send(message_buffer);

                    byte[] server_message_buffer = new byte[4096];
                    int server_message_bytes_count = client.Receive(server_message_buffer);

                    string response = Encoding.UTF8.GetString(server_message_buffer, 0, server_message_bytes_count);

                    ForegroundColor = ConsoleColor.Magenta;
                    WriteLine($"Server Response -> {response}");
                    ResetColor();

                    client.Shutdown(SocketShutdown.Both);
                    client.Close();

                    if (message == "server/stop")
                    {
                        break;
                    }
                    do
                    {
                        Write("\n> Continue (y/n)? - ");
                        ans = ReadLine();
                    } while (ans != "y" && ans!="n");
                    
                } while (ans == "y");
            }
            catch (Exception ex)
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine($"\n> Client Error {ex.Message}");
                ResetColor();
            }
            finally
            {
                ForegroundColor = ConsoleColor.Green;
                WriteLine("\n> Client Stopped");
                ResetColor();
            }
        }
    }
}
