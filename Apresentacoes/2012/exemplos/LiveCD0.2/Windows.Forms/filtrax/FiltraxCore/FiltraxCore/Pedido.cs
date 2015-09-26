using System;
using System.Collections;

namespace FiltraxCore
{
	/// <summary>
	/// Summary description for Pedido.
	/// </summary>
	public class Pedido
	{
		private int numero;
		private DateTime data;
		private Cliente cliente;
		private ArrayList produtosEmProducao;
		private Usuario vendedor;
		private StatusProducao status;
		private int notaFiscal;
		private bool imprimeNota;
		private string observacao;

		private static int id;




		public Pedido()
		{
			this.produtosEmProducao = new ArrayList();
			this.numero = id++;
			//
			// TODO: Add constructor logic here
			//
		}

		public int Numero
		{
			set
			{
				numero = value;
			}

			get
			{
				return numero;
			}
		}

		public DateTime Data
		{
			set
			{
				data = value;
			}

			get
			{
				return data;
			}
		}

		public Cliente Cliente
		{
			set
			{
				cliente = value;
			}

			get
			{
				return cliente;
			}
		}

		public ArrayList Produtos
		{
			set
			{
				produtosEmProducao = value;
			}

			get
			{
				return produtosEmProducao;
			}
		}

		public Usuario Vendedor
		{
			set
			{
				vendedor = value;
			}

			get
			{
				return vendedor;
			}
		}

		public StatusProducao Status
		{
			set
			{
				status = value;
			}

			get
			{
				return status;
			}
		}

		public int NotaFiscal
		{
			set
			{
				notaFiscal = value;
			}

			get
			{
				return notaFiscal;
			}
		}

		public bool ImprimeNota
		{
			set
			{
				imprimeNota = value;
			}

			get
			{
				return imprimeNota;
			}
		}

		public string Observacao
		{
			set
			{
				observacao = value;
			}

			get
			{
				return observacao;
			}
		}

		public void adicionaProduto(Produto p , int q, int desconto, Prioridade pr)
		{
			EmProducao e = new EmProducao(p,q,pr);
			e.Desconto = desconto;
			this.produtosEmProducao.Add(e);
		}

		public EmProducao[] listaProdutos()
		{
			EmProducao[] p = new EmProducao[this.produtosEmProducao.Count];
			this.produtosEmProducao.CopyTo(p);
			return p;

		}
	}
}
