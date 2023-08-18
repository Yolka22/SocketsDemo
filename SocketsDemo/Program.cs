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

            Dictionary<string, string> zodiacForecastMap = new Dictionary<string, string>
        {
            { "Aries", "Good fortune ahead" },
            { "Taurus", "Luck is on your side" },
            { "Gemini", "Stay cautious today" },
            { "Cancer", "Positive vibes coming your way" },
            { "Leo", "An exciting opportunity awaits" },
            { "Virgo", "Things might get challenging" },
            { "Libra", "Balance will be key today" },
            { "Scorpio", "Trust your instincts" },
            { "Sagittarius", "Adventure is calling" },
            { "Capricorn", "Hard work will pay off" },
            { "Aquarius", "Unexpected changes ahead" },
            { "Pisces", "Creativity will flourish" }
        };

            int port = 9001;
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(ip, port);

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            try
            {
                listener.Bind(endPoint);
                listener.Listen(100);
                     

                while (true)
                {

                    ForegroundColor = ConsoleColor.Cyan;
                    WriteLine("\n> Wait for requests from clients ...");
                    ResetColor();

                    Socket acceptor = listener.Accept();

                    byte[] message_buffer = new byte[4096];
                    int message_bytes_count = acceptor.Receive(message_buffer);


                    string user_message = Encoding.UTF8.GetString(message_buffer, 0, message_bytes_count);
                    WriteLine($"{user_message} {DateTime.Now}");

                    string response ="default";

                    if (zodiacForecastMap.ContainsKey(user_message))
                    {
                        string forecast = zodiacForecastMap[user_message];
                        response = $"Forecast for {user_message}: {forecast}";
                    }
                    else
                    {
                        response = "Sign not found :("; 
                    }


                    byte[] response_buffer = Encoding.UTF8.GetBytes(response);
                    acceptor.Send(response_buffer);


                    acceptor.Shutdown(SocketShutdown.Both);
                    acceptor.Close();


                    if (user_message == "server/stop")
                    {
                        break;
                    }
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
