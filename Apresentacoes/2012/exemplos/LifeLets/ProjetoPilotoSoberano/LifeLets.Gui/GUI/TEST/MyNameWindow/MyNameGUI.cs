// created on 29/11/2004 at 00:03
// project created on 28/11/2004 at 22:42

//Tem que colocar esses libs
using System;
using Gtk;
using Glade;

public class MyNameGUI
{
// dai vc precisa referencias os objetos que estao no XML
// no diretorio ProjetoPilotoSoberano/TOOLS que faz isso para vc ..
// basta executar mono GladeSync.exe nome.glade class.cs
// e deginir um region GladeSync que ele vai ler o XML e
// colocar todas as definicoes dos objecto de interface.. 
	#region GladeSync
	[Glade.Widget] Window MyNameWindow;
	[Glade.Widget] VBox vbox1;
	[Glade.Widget] Fixed fixed1;
	[Glade.Widget] Entry entry1;
	[Glade.Widget] Label label3;
	[Glade.Widget] HButtonBox hbuttonbox2;
	[Glade.Widget] Button buttonOK;
	[Glade.Widget] Alignment alignment2;
	[Glade.Widget] HBox hbox5;
	[Glade.Widget] Image image2;
	[Glade.Widget] Label label4;
	[Glade.Widget] Button buttonCancel;
	[Glade.Widget] Alignment alignment3;
	[Glade.Widget] HBox hbox6;
	[Glade.Widget] Image image3;
	[Glade.Widget] Label label5;
	[Glade.Widget] Statusbar statusbar1;
	#endregion

	private string name;  
	
	
//aqui vai a criacao da interface   
	public MyNameGUI (string myname) 
	{
		//interfave construction 
	 	Application.Init();
	 	// Carrega o XML 
	 	// Voce pode nao querer usar o  Glade, mas dai teria que instaciar tudo na mão
	 	//
	 	// Exemplo de código :  
	 	// 	 	Window window = new Window("Simple Application");
		//   	Label label = new Label("Name");
    	//		Entry entry = new Entry();
    	//		Button button = new Button("Hello!");
    	// 
    	// No código acima instaciamos e configuramos na mão objetos por objeto
    	// a vantagem no glade é que vc configura todos o objetos visualmente
    	// e vc não precisa executar para ver como vai ficar ai interface 
    	// isso economiza um bocado de codigo 
    	// so para essa tela teriamo que instanciar 14 objecto e configurar todos
    	// colocando seus estados e posicionamento na tela
    
		Glade.XML gxml = new Glade.XML (null, "mynamewindow.glade", "MyNameWindow", null);
		gxml.Autoconnect (this);
		
		// Aqui estou  atribuindo um valor no objecto visual edit
		this.name = myname;
		entry1.Text = myname;
		
		// É possivel criar os evento no glade tambem mas não descobri como
		// entao é necessario criar os delegate aqui no código 
		// óu seja, é precisa amarrar (delegar o evento) para um metodo da aplicacao
		// é o que estamos fazendo abaixo estamos delegando o eveto buttonCancel.Clicked
		// para  o método OnButtonCancel_Clicked
		// neste momento vc esta criando um EventHandle que vai jogar o processamento
		// para o metodo que vc esta definido entre os parentese (vai que cria isso)
		buttonCancel.Clicked +=  new EventHandler (OnButtonCancel_Clicked);
		buttonOK.Clicked +=  new EventHandler (OnButtonOK_Clicked);
		
	 	Application.Run();
	}

	// Aqui estamo criando os metodo que vao responder os eventos
	// este abaiso é o do evento do botao cancel. 
	// basta declara-lo com esse estrutura abaixo. 
	
	public  void OnButtonCancel_Clicked (object o, EventArgs args) 
	{
	    //Application.Quit ();
	    // Esse destroy eu descobri olhando o fonte do MonoDevelop
	    // que quiser olhar é so ir em www.monodevelop.com
	    // ou no link http://www.go-mono.com/archive/1.0.2/monodevelop-0.5.1.tar.gz
	    // para descompactar user tar -zxvf monodevelop-0.5.1.tar.gz
	    MyNameWindow.Destroy();
	    
	}
	
	public  void OnButtonOK_Clicked (object o, EventArgs args) 
	{
	    // TODO :write code here 
	    System.Console.WriteLine (entry1.Text);
 
	}
	
	

}


