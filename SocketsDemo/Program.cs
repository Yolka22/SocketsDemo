using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using static System.Console;
using System.Runtime.Remoting.Messaging;

namespace SocketsDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1 net connection params
            int port = 9001;
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(ip, port);

            // 2 Listen socket 
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            try
            {
                // 3 turn listen socket on
                listener.Bind(endPoint);
                listener.Listen(100);
                     
                // 4 data transfer with clients
                while (true)
                {
                    //4.1 -> create aceept soket
                    ForegroundColor = ConsoleColor.Cyan;
                    WriteLine("\n> Wait for requests from clients ...");
                    ResetColor();

                    Socket acceptor = listener.Accept();

                    //4.2 -> taking input from client
                    byte[] message_buffer = new byte[4096];
                    int message_bytes_count = acceptor.Receive(message_buffer);

                    //4.3 -> decoding and show message
                    string message = Encoding.UTF8.GetString(message_buffer, 0, message_bytes_count);

                    ForegroundColor = ConsoleColor.Magenta;
                    WriteLine($"{DateTime.Now} -> {message}");
                    ResetColor();

                    // 4.4 -> processing request
                    string response = "your request data is ..smth..";

                    // 4.5 -> send response
                    byte[] response_buffer = Encoding.UTF8.GetBytes(response);
                    acceptor.Send(response_buffer);

                    // 4.6 -> closing connection with client
                    acceptor.Shutdown(SocketShutdown.Both);
                }
            }
            catch (Exception ex) 
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine($"\n> Server Error {ex.Message}");
                ResetColor();
            }
            finally
            {
                listener.Close();
                ForegroundColor = ConsoleColor.Green;
                WriteLine("\n> Server Stopped");
                ResetColor();
            }
        }
    }
}
