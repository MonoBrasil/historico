using System.Collections;

namespace FiltraxCore
{
	/// <summary>
	/// Summary description for Produto.
	/// </summary>
	public class Produto
	{
		private int id;
		private string nome;
		private string modelo;
		private string cor;
        private float valor;
		private string observacao;

		public Produto(int id,  string modelo)
		{
			this.id = id;
			this.modelo = modelo;
		}

		public Produto()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public string Nome
		{
			set
			{
				nome = value;
			}

			get
			{
				return nome;
			}
		}

		public string Modelo
		{
			set
			{
				modelo = value;
			}

			get
			{
				return modelo;
			}
		}

		public int Id
		{
			set
			{
				id = value;
			}

			get
			{
				return id;
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

		public float Valor
		{
			set
			{
				valor = value;
			}

			get
			{
				return valor;
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

		public static Produto[] Listar()
		{
			ArrayList l = new ArrayList();
			l.Add(new Produto(1,"Modelo 1"));
			l.Add(new Produto(2,"Modelo 2"));
			l.Add(new Produto(3,"Modelo 3"));
			l.Add(new Produto(4,"Modelo 4"));
			Produto[] p = new Produto[l.Count];
			l.CopyTo(p);
			return p;
		}
	}
}
