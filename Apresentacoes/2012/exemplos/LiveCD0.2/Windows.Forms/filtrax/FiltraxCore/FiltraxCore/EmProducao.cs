using System.Collections;

namespace FiltraxCore
{
	/// <summary>
	/// Summary description for EmProducao.
	/// Item Producao
	/// </summary>
	public class EmProducao
	{
		private Produto produto;
		private int quantidade;
		private int desconto;
		private string cor;
		private Prioridade prioridade;

		public EmProducao(Produto produto, int quantidade, Prioridade prioridade)
		{
			this.produto = produto;
			this.quantidade = quantidade;
		
			this.prioridade = prioridade;
		}


		public EmProducao(Produto produto, int quantidade)
		{
			this.produto = produto;
			this.quantidade = quantidade;
		}


		public EmProducao()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/*public Produto Produto
		{
			set
			{
				produto = value;
			}

			get
			{
				return produto;
			}
		}*/

		public string NomeProduto
		{
			
			get
			{
				return this.produto.Modelo;
			}
		}



		public int Quantidade
		{
			set
			{
				quantidade = value;
			}

			get
			{
				return quantidade;
			}
		}

		public int Desconto
		{
			set
			{
				desconto = value;
			}

			get
			{
				return desconto;
			}
		}

		public string Cor
		{
			set
			{
				cor = value;
			}

			get
			{
				return cor;
			}
		}

		public string  Prioridade
		{
			

			get
			{
				return prioridade.Descricao;
			}
		}

		
	}
}
