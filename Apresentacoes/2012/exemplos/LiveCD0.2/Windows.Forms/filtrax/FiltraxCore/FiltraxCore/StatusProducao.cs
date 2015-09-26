using System;

namespace FiltraxCore
{
	/// <summary>
	/// Summary description for StatusProducao.
	/// </summary>
	public class StatusProducao
	{

		private DateTime dataInicio;
		private DateTime dataFim;
		private bool pedidoLiberado;
	
		public StatusProducao()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public DateTime DataInicio
		{
			set
			{
				dataInicio = value;
			}

			get
			{
				return dataInicio;
			}
		}

		public DateTime DataFim
		{
			set
			{
				dataFim = value;
			}

			get
			{
				return dataFim;
			}
		}

		public bool PedidoLiberado
		{
			set
			{
				pedidoLiberado = value;
			}

			get
			{
				return pedidoLiberado;
			}
		}
	}
}
