using System;
using System.Collections;
using System.Windows.Forms;
using FiltraxCore;

namespace FiltraxGUI
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;

		private Pedido pedido;
		private ArrayList pedidosNovos;
		private System.Windows.Forms.TabPage tpPedidos;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.DataGrid dgPedidos;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.TextBox tbObs;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ComboBox cbPrioridade;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button bAdd;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbCor;
		private System.Windows.Forms.TextBox teQuantidade;
		private System.Windows.Forms.Label lQuantidade;
		private System.Windows.Forms.ComboBox cbProduto;
		private System.Windows.Forms.DataGrid dgProdutos;
		private System.Windows.Forms.DateTimePicker dtPicker;
		private System.Windows.Forms.ComboBox cbCliente;
		private System.Windows.Forms.TextBox tbNumero;
		private System.Windows.Forms.Label Lcliente;
		private System.Windows.Forms.Label NPedido;
		private System.Windows.Forms.Label data;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox cbVendedor;
		private System.Windows.Forms.TextBox tbDesconto;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.Label lClienteNome;
		private System.Windows.Forms.TextBox tbClienteNome;
		private System.Windows.Forms.Button bNovoCliente;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button button6;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//this.pedido = new Pedido();
			this.pedidosNovos = new ArrayList();
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.panel2 = new System.Windows.Forms.Panel();
			this.button5 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.tbObs = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.cbVendedor = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.cbPrioridade = new System.Windows.Forms.ComboBox();
			this.tbDesconto = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.bAdd = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.cbCor = new System.Windows.Forms.ComboBox();
			this.teQuantidade = new System.Windows.Forms.TextBox();
			this.lQuantidade = new System.Windows.Forms.Label();
			this.cbProduto = new System.Windows.Forms.ComboBox();
			this.dgProdutos = new System.Windows.Forms.DataGrid();
			this.dtPicker = new System.Windows.Forms.DateTimePicker();
			this.cbCliente = new System.Windows.Forms.ComboBox();
			this.tbNumero = new System.Windows.Forms.TextBox();
			this.Lcliente = new System.Windows.Forms.Label();
			this.NPedido = new System.Windows.Forms.Label();
			this.data = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tpPedidos = new System.Windows.Forms.TabPage();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.dgPedidos = new System.Windows.Forms.DataGrid();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.bNovoCliente = new System.Windows.Forms.Button();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.tbClienteNome = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.lClienteNome = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.button6 = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgProdutos)).BeginInit();
			this.tabPage2.SuspendLayout();
			this.tpPedidos.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgPedidos)).BeginInit();
			this.tabPage3.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.button3);
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(152, 472);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Filtrax";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(16, 104);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(128, 24);
			this.button3.TabIndex = 2;
			this.button3.Text = "Ver Grade";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(16, 64);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(128, 24);
			this.button2.TabIndex = 1;
			this.button2.Text = "Ver Grade";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(16, 32);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(128, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Novo Pedido";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tpPedidos);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Location = new System.Drawing.Point(168, 16);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(520, 464);
			this.tabControl1.TabIndex = 1;
			this.tabControl1.Visible = false;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.tabPage1.Controls.Add(this.panel2);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(512, 438);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Novo Pedido";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.button5);
			this.panel2.Controls.Add(this.button4);
			this.panel2.Controls.Add(this.tbObs);
			this.panel2.Controls.Add(this.label6);
			this.panel2.Controls.Add(this.groupBox4);
			this.panel2.Controls.Add(this.groupBox3);
			this.panel2.Controls.Add(this.dtPicker);
			this.panel2.Controls.Add(this.cbCliente);
			this.panel2.Controls.Add(this.tbNumero);
			this.panel2.Controls.Add(this.Lcliente);
			this.panel2.Controls.Add(this.NPedido);
			this.panel2.Controls.Add(this.data);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(512, 438);
			this.panel2.TabIndex = 0;
			this.panel2.Visible = false;
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(304, 404);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(96, 23);
			this.button5.TabIndex = 34;
			this.button5.Text = "Cancela";
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(400, 404);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(96, 23);
			this.button4.TabIndex = 33;
			this.button4.Text = "Finalizar Pedido";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// tbObs
			// 
			this.tbObs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tbObs.Location = new System.Drawing.Point(112, 60);
			this.tbObs.Multiline = true;
			this.tbObs.Name = "tbObs";
			this.tbObs.Size = new System.Drawing.Size(392, 48);
			this.tbObs.TabIndex = 32;
			this.tbObs.Text = "";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 60);
			this.label6.Name = "label6";
			this.label6.TabIndex = 31;
			this.label6.Text = "Observação";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.cbVendedor);
			this.groupBox4.Controls.Add(this.label7);
			this.groupBox4.Controls.Add(this.textBox2);
			this.groupBox4.Controls.Add(this.label5);
			this.groupBox4.Controls.Add(this.comboBox2);
			this.groupBox4.Controls.Add(this.label4);
			this.groupBox4.Location = new System.Drawing.Point(8, 316);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(496, 80);
			this.groupBox4.TabIndex = 30;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Condições Pagamento";
			// 
			// cbVendedor
			// 
			this.cbVendedor.Location = new System.Drawing.Point(352, 24);
			this.cbVendedor.Name = "cbVendedor";
			this.cbVendedor.Size = new System.Drawing.Size(121, 21);
			this.cbVendedor.TabIndex = 5;
			this.cbVendedor.Text = "Selecione o Vendedor";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(280, 24);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(64, 16);
			this.label7.TabIndex = 4;
			this.label7.Text = "Vendedor";
			// 
			// textBox2
			// 
			this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox2.Location = new System.Drawing.Point(120, 48);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(48, 20);
			this.textBox2.TabIndex = 3;
			this.textBox2.Text = "";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 48);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(96, 16);
			this.label5.TabIndex = 2;
			this.label5.Text = "Parcelamento";
			// 
			// comboBox2
			// 
			this.comboBox2.Location = new System.Drawing.Point(120, 24);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(96, 21);
			this.comboBox2.TabIndex = 1;
			this.comboBox2.Text = "comboBox2";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(120, 16);
			this.label4.TabIndex = 0;
			this.label4.Text = "Forma de Pagamento";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.cbPrioridade);
			this.groupBox3.Controls.Add(this.tbDesconto);
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.Controls.Add(this.bAdd);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Controls.Add(this.cbCor);
			this.groupBox3.Controls.Add(this.teQuantidade);
			this.groupBox3.Controls.Add(this.lQuantidade);
			this.groupBox3.Controls.Add(this.cbProduto);
			this.groupBox3.Controls.Add(this.dgProdutos);
			this.groupBox3.Location = new System.Drawing.Point(8, 108);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(496, 192);
			this.groupBox3.TabIndex = 29;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Produto";
			// 
			// cbPrioridade
			// 
			this.cbPrioridade.Location = new System.Drawing.Point(360, 24);
			this.cbPrioridade.Name = "cbPrioridade";
			this.cbPrioridade.Size = new System.Drawing.Size(121, 21);
			this.cbPrioridade.TabIndex = 26;
			this.cbPrioridade.Text = "Prioridade";
			// 
			// tbDesconto
			// 
			this.tbDesconto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tbDesconto.Location = new System.Drawing.Point(304, 48);
			this.tbDesconto.Name = "tbDesconto";
			this.tbDesconto.Size = new System.Drawing.Size(40, 20);
			this.tbDesconto.TabIndex = 25;
			this.tbDesconto.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(240, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 16);
			this.label3.TabIndex = 24;
			this.label3.Text = "Desconto";
			// 
			// bAdd
			// 
			this.bAdd.Location = new System.Drawing.Point(360, 48);
			this.bAdd.Name = "bAdd";
			this.bAdd.Size = new System.Drawing.Size(120, 24);
			this.bAdd.TabIndex = 23;
			this.bAdd.Text = "Adiciona";
			this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 23);
			this.label2.TabIndex = 22;
			this.label2.Text = "Modelo";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 23);
			this.label1.TabIndex = 21;
			this.label1.Text = "Cor";
			// 
			// cbCor
			// 
			this.cbCor.Location = new System.Drawing.Point(64, 48);
			this.cbCor.Name = "cbCor";
			this.cbCor.Size = new System.Drawing.Size(168, 21);
			this.cbCor.TabIndex = 20;
			this.cbCor.Text = "Selecione a Cor";
			// 
			// teQuantidade
			// 
			this.teQuantidade.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.teQuantidade.Location = new System.Drawing.Point(304, 24);
			this.teQuantidade.Name = "teQuantidade";
			this.teQuantidade.Size = new System.Drawing.Size(40, 20);
			this.teQuantidade.TabIndex = 19;
			this.teQuantidade.Text = "";
			// 
			// lQuantidade
			// 
			this.lQuantidade.Location = new System.Drawing.Point(240, 24);
			this.lQuantidade.Name = "lQuantidade";
			this.lQuantidade.Size = new System.Drawing.Size(100, 16);
			this.lQuantidade.TabIndex = 18;
			this.lQuantidade.Text = "Quantidade";
			// 
			// cbProduto
			// 
			this.cbProduto.Location = new System.Drawing.Point(64, 24);
			this.cbProduto.Name = "cbProduto";
			this.cbProduto.Size = new System.Drawing.Size(168, 21);
			this.cbProduto.TabIndex = 17;
			this.cbProduto.Text = "Selecione o Modelo";
			// 
			// dgProdutos
			// 
			this.dgProdutos.BackgroundColor = System.Drawing.Color.MintCream;
			this.dgProdutos.DataMember = "";
			this.dgProdutos.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgProdutos.Location = new System.Drawing.Point(16, 72);
			this.dgProdutos.Name = "dgProdutos";
			this.dgProdutos.Size = new System.Drawing.Size(464, 112);
			this.dgProdutos.TabIndex = 14;
			// 
			// dtPicker
			// 
			this.dtPicker.Location = new System.Drawing.Point(264, 12);
			this.dtPicker.Name = "dtPicker";
			this.dtPicker.Size = new System.Drawing.Size(240, 20);
			this.dtPicker.TabIndex = 28;
			// 
			// cbCliente
			// 
			this.cbCliente.Location = new System.Drawing.Point(112, 36);
			this.cbCliente.Name = "cbCliente";
			this.cbCliente.Size = new System.Drawing.Size(392, 21);
			this.cbCliente.TabIndex = 27;
			this.cbCliente.Text = "Selecione o cliente";
			// 
			// tbNumero
			// 
			this.tbNumero.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tbNumero.Location = new System.Drawing.Point(112, 12);
			this.tbNumero.Name = "tbNumero";
			this.tbNumero.TabIndex = 26;
			this.tbNumero.Text = "";
			// 
			// Lcliente
			// 
			this.Lcliente.Location = new System.Drawing.Point(8, 36);
			this.Lcliente.Name = "Lcliente";
			this.Lcliente.Size = new System.Drawing.Size(100, 16);
			this.Lcliente.TabIndex = 25;
			this.Lcliente.Text = "Cliente";
			// 
			// NPedido
			// 
			this.NPedido.Location = new System.Drawing.Point(8, 12);
			this.NPedido.Name = "NPedido";
			this.NPedido.Size = new System.Drawing.Size(96, 16);
			this.NPedido.TabIndex = 24;
			this.NPedido.Text = "N Pedido";
			// 
			// data
			// 
			this.data.Location = new System.Drawing.Point(224, 12);
			this.data.Name = "data";
			this.data.Size = new System.Drawing.Size(40, 16);
			this.data.TabIndex = 23;
			this.data.Text = "Data";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.panel1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(512, 438);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Grade";
			// 
			// panel1
			// 
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(512, 438);
			this.panel1.TabIndex = 0;
			// 
			// tpPedidos
			// 
			this.tpPedidos.Controls.Add(this.groupBox2);
			this.tpPedidos.Location = new System.Drawing.Point(4, 22);
			this.tpPedidos.Name = "tpPedidos";
			this.tpPedidos.Size = new System.Drawing.Size(512, 438);
			this.tpPedidos.TabIndex = 2;
			this.tpPedidos.Text = "Pedido";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.dgPedidos);
			this.groupBox2.Location = new System.Drawing.Point(8, 8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(504, 176);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Lista de Pedidos";
			// 
			// dgPedidos
			// 
			this.dgPedidos.DataMember = "";
			this.dgPedidos.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgPedidos.Location = new System.Drawing.Point(8, 16);
			this.dgPedidos.Name = "dgPedidos";
			this.dgPedidos.Size = new System.Drawing.Size(488, 152);
			this.dgPedidos.TabIndex = 0;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.button6);
			this.tabPage3.Controls.Add(this.panel3);
			this.tabPage3.Controls.Add(this.bNovoCliente);
			this.tabPage3.Controls.Add(this.textBox4);
			this.tabPage3.Controls.Add(this.textBox3);
			this.tabPage3.Controls.Add(this.tbClienteNome);
			this.tabPage3.Controls.Add(this.label10);
			this.tabPage3.Controls.Add(this.label9);
			this.tabPage3.Controls.Add(this.lClienteNome);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(512, 438);
			this.tabPage3.TabIndex = 3;
			this.tabPage3.Text = "tabPage3";
			// 
			// bNovoCliente
			// 
			this.bNovoCliente.Location = new System.Drawing.Point(176, 128);
			this.bNovoCliente.Name = "bNovoCliente";
			this.bNovoCliente.TabIndex = 6;
			this.bNovoCliente.Text = "Novo Cliente";
			this.bNovoCliente.Click += new System.EventHandler(this.bNovoCliente_Click);
			// 
			// textBox4
			// 
			this.textBox4.Location = new System.Drawing.Point(152, 96);
			this.textBox4.Name = "textBox4";
			this.textBox4.TabIndex = 5;
			this.textBox4.Text = "textBox4";
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(152, 64);
			this.textBox3.Name = "textBox3";
			this.textBox3.TabIndex = 4;
			this.textBox3.Text = "textBox3";
			// 
			// tbClienteNome
			// 
			this.tbClienteNome.Location = new System.Drawing.Point(152, 32);
			this.tbClienteNome.Name = "tbClienteNome";
			this.tbClienteNome.TabIndex = 3;
			this.tbClienteNome.Text = "textBox1";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(32, 96);
			this.label10.Name = "label10";
			this.label10.TabIndex = 2;
			this.label10.Text = "label10";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(32, 64);
			this.label9.Name = "label9";
			this.label9.TabIndex = 1;
			this.label9.Text = "label9";
			// 
			// lClienteNome
			// 
			this.lClienteNome.Location = new System.Drawing.Point(48, 32);
			this.lClienteNome.Name = "lClienteNome";
			this.lClienteNome.TabIndex = 0;
			this.lClienteNome.Text = "Nome";
			// 
			// panel3
			// 
			this.panel3.Location = new System.Drawing.Point(32, 168);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(424, 100);
			this.panel3.TabIndex = 7;
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(336, 136);
			this.button6.Name = "button6";
			this.button6.TabIndex = 8;
			this.button6.Text = "button6";
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(696, 493);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.groupBox1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.groupBox1.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgProdutos)).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.tpPedidos.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgPedidos)).EndInit();
			this.tabPage3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			// Popula Clientes
			cbCliente.DataSource = Cliente.Listar();
			cbCliente.DisplayMember = "Nome";

			//Popula Produtos
			cbProduto.DataSource = Produto.Listar();
			cbProduto.DisplayMember = "Modelo";
			cbProduto.ValueMember = "Id";
			

			//Popula Prioridade
			cbPrioridade.DataSource = Prioridade.Listar();
			cbPrioridade.DisplayMember = "Descricao";
			cbPrioridade.ValueMember = "Id";

			
			this.pedido = new Pedido();
			this.pedidosNovos.Add(this.pedido);

			tbNumero.Text = this.pedido.Numero.ToString();
			tabControl1.Visible = true;
			panel2.Enabled = true;
			panel2.Visible = true;

		}

		private void bAdd_Click(object sender, System.EventArgs e)
		{
			Produto a = (Produto) cbProduto.SelectedItem;
			int q = int.Parse(teQuantidade.Text);
			int d = int.Parse(tbDesconto.Text);
			Prioridade pr = (Prioridade) cbPrioridade.SelectedItem;
			this.pedido.adicionaProduto(a,q,d,pr);

			//atualizar grid de produtos
			dgProdutos.DataSource = this.pedido.listaProdutos();

			
			
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			tpPedidos.Show();

		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			FormMain f = new FormMain();
			f.Show();
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			//Coloca o clinete no pedido
			this.pedido.Cliente = (Cliente) cbCliente.SelectedItem;
			this.pedido.Observacao = tbObs.Text;
			this.pedido.Data = dtPicker.Value;

			//Atualizar gride de pedidos
			Pedido[] listaPedido = new Pedido[this.pedidosNovos.Count];
			this.pedidosNovos.CopyTo(listaPedido);
			dgPedidos.DataSource = listaPedido;
			//tabControl1.Enabled = false;
			panel2.Enabled = false;
		}

		private void bNovoCliente_Click(object sender, System.EventArgs e)
		{
			Cliente c = new Cliente();
			c.Nome = tbClienteNome.Text;
			c.inserir();

		}

		private void button6_Click(object sender, System.EventArgs e)
		{
			Cliente c = new Cliente();
			panel3.DataBindings.Add("Nome",c, "Nome");
		}

		
	
		
		

		
	}
}
