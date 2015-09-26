// created on 8/12/2004 at 15:14

using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

using LifeLets.Lib;

namespace LifeLets.Core
{

	public abstract class NetworkBridge
	{
		protected string serviceName;		
		protected int port;
		protected object tipoOperacao;
		
		public NetworkBridge(object tipoOperacao)
		{
			this.tipoOperacao = tipoOperacao;		
		}
				
		public abstract string respondTo(string data);
		
		public virtual string CallService(string data, string ip)
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
		
		public static void MainListener(Life life, int port)
		{
			TcpListener listener;

			listener = new TcpListener(port);
			listener.Start();
			
			while(true)
			{								
				Socket connection = listener.AcceptSocket();
				
				NetworkStream socketstream = new NetworkStream( connection );
				
				BinaryReader reader = new BinaryReader(socketstream);
																																												
				string serviceName = reader.ReadString();

/*
				TODO: Pesquisar como instanciar um objeto a partir de objeto Type;
						Isso eliminará os "cases";			
				
				NetworkBridge servico = new System.Type.GetType(serviceName))();  
				
				servico.respondTo("");				

*/				string response = "";																																																	
				string serviceData = reader.ReadString();
				
				switch (serviceName)
				{
					case "NameService":
						{
							NameService service = new NameService(life);
							response = service.respondTo(serviceData);
							break;
						}
											
					default:{break;}
				}
				
				new BinaryWriter(socketstream).Write(response);
				
			}
		}
	}
}