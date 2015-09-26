// created on 8/12/2004 at 15:11

using LifeLets.Lib;

namespace LifeLets.Core
{
	public class NameService : NetworkBridge
	{
	
		public NameService (object life) : base (life)
		{
			this.serviceName = "NameService";
			this.port = 2004;
		}
	
		public override string respondTo(string data)
		{
			return ((Life)tipoOperacao).Name;			
		}
	}
}