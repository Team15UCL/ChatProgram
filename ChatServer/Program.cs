using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer;

internal class Program
{
    static void Main(string[] args)
    {
        ExecuteServer();
    }

    public static void ExecuteServer()
    {
        try
        {
            // Finder Ip-addressen til den lokale computer
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];

            // Vælger port vi vil bruge
            IPEndPoint localEndPoint = new(ipAddr, 65000);

            // Laver en socket ud fra vores ip og port
            Socket serverSocket = new(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                serverSocket.Bind(localEndPoint);

                serverSocket.Listen(10);

                Console.WriteLine("Waiting for connection...");

                Socket clientSocket = serverSocket.Accept();

                // buffer til at modtage
                byte[] bytes = new byte[1024];
                string data = null;

                int numByte = clientSocket.Receive(bytes);

                data += Encoding.ASCII.GetString(bytes, 0, numByte);

                Console.WriteLine($"Modtaget besked: {data}");

                byte[] message = Encoding.ASCII.GetBytes("Hej fra serveren");

                clientSocket.Send(message);

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
