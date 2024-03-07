using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatClient;

internal class Program
{
    static void Main(string[] args)
    {
        ExecuteClient();
    }

    static void ExecuteClient()
    {
        try
        {
            // Finder Ip-addressen til den lokale computer
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];

            // Vælger port vi vil bruge
            IPEndPoint localEndPoint = new(ipAddr, 65000);

            // Laver en socket ud fra vores ip og port
            Socket clientSocket = new(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                clientSocket.Connect(localEndPoint);

                Console.WriteLine($"Socket connected to: {clientSocket.RemoteEndPoint}");

                byte[] messageSent = Encoding.ASCII.GetBytes("Hej fra klienten");
                int byteSent = clientSocket.Send(messageSent);

                byte[] messageReceived = new byte[1024];

                int byteReceived = clientSocket.Receive(messageReceived);

                Console.WriteLine($"Besked fra server: {Encoding.ASCII.GetString(messageReceived)}");

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine($"ANexception: {ane}");
            }
            catch (SocketException se)
            {
                Console.WriteLine($"SException: {se}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

    }
}
