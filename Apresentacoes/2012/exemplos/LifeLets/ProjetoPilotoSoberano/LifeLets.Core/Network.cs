// created on 7/12/2004 at 12:10

using System;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace LifeLets.Lib 
{
	public class Network
	{
	
		private int port;
		
		public Network(int port)
		{
			this.port = port;
		}
	
		public string CallService(string serviceName, string data, string ip)
		{
			TcpClient client = new TcpClient();
			try
			{
				client.Connect(ip, port);
				NetworkStream stream = client.GetStream();
																			
				BinaryWriter writer1 = new BinaryWriter(stream);
				writer1.Write(serviceName);
				writer1.Write(data);
				
				return new BinaryReader(stream).ReadString();
			}
			
			finally
			{
				client.Close();
			}
		}
	}
}
