using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace textAdEngineDemo_client
{
	class main
	{
		private static IPAddress serverIP;
		private static int port;
		private static Socket server;
		private static byte[] data;
		private static IPEndPoint serverEndPoint;
		
		public static void Main(string[] args)
		{
			data = new byte[1024];  //to prevent from crashing use the same buffersize like the server
			
			if(args.Length == 0 || args[0].Trim() == "")
			{
				Console.WriteLine("Server IP: ");
				serverIP = IPAddress.Parse(Console.ReadLine());
			}
			else
				serverIP = IPAddress.Parse(args[0]);
			
			if(args.Length <= 1 || args[1].Trim() == "")
			{
				port = 4;
			}
			else
				port = Convert.ToInt32(Console.ReadLine());
			
			serverEndPoint = new IPEndPoint(serverIP, port);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            data = new byte[1024];
            
           	Thread listener = new Thread(Listen);
	        listener.Priority = ThreadPriority.Lowest;
	        listener.Start();
		}
		
		private static void Listen()
        {
            var recv = server.Receive(data);
            string stringData = Encoding.ASCII.GetString(data, 0, recv);
            Console.WriteLine(stringData);

            while (server.Connected)
            {
                data = new byte[1024];
                recv = server.Receive(data);
                stringData = Encoding.ASCII.GetString(data, 0, recv);
                Console.WriteLine("Received from Server: " + stringData);
            }
            Console.WriteLine("Disconnecting from server...");
            server.Close();
        }
	}
}