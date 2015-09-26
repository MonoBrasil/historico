/*
MonoUML.Widgets - A library for representing the Widget elements
Copyright (C) 2004  Rodolfo Campero
Copyright (C) 2005  Mario Carrión

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
using System;
using Gtk;
using MonoUML.I18n;

namespace MonoUML.Widgets
{
	public class TypeKindChooserDialog : ChooserDialog
	{

		public TypeKindChooserDialog () : base (GettextCatalog.GetString ("Choose a type"))
		{
			Gtk.HBox hbox = new Gtk.HBox();
			base.VBox.Add (hbox);
			Gtk.VBox vbox;
			// box 1
			vbox = new Gtk.VBox ();
			hbox.Add (vbox);
			AddButton (vbox, GettextCatalog.GetString ("Activity"), SetActivity);
			_selection = "Activity";
			AddButton (vbox, GettextCatalog.GetString ("Actor"), SetActor);
			AddButton (vbox, GettextCatalog.GetString ("Artifact"), SetArtifact);
			AddButton (vbox, GettextCatalog.GetString ("Association"), SetAssociation);
			AddButton (vbox, GettextCatalog.GetString ("AssociationClass"), SetAssociationClass);
			AddButton (vbox, GettextCatalog.GetString ("Class"), SetClass);
			AddButton (vbox, GettextCatalog.GetString ("Collaboration"), SetCollaboration);
			vbox.Show ();
			// box 2
			vbox = new Gtk.VBox();
			hbox.Add (vbox);
			AddButton (vbox, GettextCatalog.GetString ("CommunicationPath"), SetCommunicationPath);
			AddButton (vbox, GettextCatalog.GetString ("Component"), SetComponent);
			AddButton (vbox, GettextCatalog.GetString ("DataType"), SetDataType);
			AddButton (vbox, GettextCatalog.GetString ("DeploymentSpecification"), SetDeploymentSpecification);
			AddButton (vbox, GettextCatalog.GetString ("Device"), SetDevice);
			AddButton (vbox, GettextCatalog.GetString ("Enumeration"), SetEnumeration);
			AddButton (vbox, GettextCatalog.GetString ("ExecutionEnvironment"), SetExecutionEnvironment);
			vbox.Show ();
			// box 3
			vbox = new Gtk.VBox ();
			hbox.Add (vbox);
			AddButton (vbox, GettextCatalog.GetString ("Extension"), SetExtension);
			AddButton (vbox, GettextCatalog.GetString ("InformationItem"), SetInformationItem);
			AddButton (vbox, GettextCatalog.GetString ("Interaction"), SetInteraction);
			AddButton (vbox, GettextCatalog.GetString ("Interface"), SetInterface);
			AddButton (vbox, GettextCatalog.GetString ("Node"), SetNode);
			AddButton (vbox, GettextCatalog.GetString ("PrimitiveType"), SetPrimitiveType);
			AddButton (vbox, GettextCatalog.GetString ("ProtocolStateMachine"), SetProtocolStateMachine);
			vbox.Show ();
			// box 4
			vbox = new Gtk.VBox ();
			hbox.Add (vbox);
			AddButton (vbox, GettextCatalog.GetString ("Signal"), SetSignal);
			AddButton (vbox, GettextCatalog.GetString ("StateMachine"), SetStateMachine);
			AddButton (vbox, GettextCatalog.GetString ("Stereotype"), SetStereotype);
			AddButton (vbox, GettextCatalog.GetString ("UseCase"), SetUseCase);
			vbox.Show ();
			hbox.Show ();
		}

		private void SetActivity (object sender, EventArgs args)
		{
			_selection = "Activity"; //No i18n
		}
		
		private void SetActor (object sender, EventArgs args)
		{
			_selection = "Actor"; //No i18n
		}

		private void SetArtifact (object sender, EventArgs args)
		{
			_selection = "Artifact"; //No i18n
		}
		
		private void SetAssociation (object sender, EventArgs args)
		{
			_selection = "Association"; //No i18n
		}
		
		private void SetAssociationClass (object sender, EventArgs args)
		{
			_selection = "AssociationClass"; //No i18n
		}
		
		private void SetClass (object sender, EventArgs args)
		{
			_selection = "Class"; //No i18n
		}
		
		private void SetCollaboration (object sender, EventArgs args)
		{
			_selection = "Collaboration"; //No i18n
		}
		
		private void SetCommunicationPath (object sender, EventArgs args)
		{
			_selection = "CommunicationPath"; //No i18n
		}
		
		private void SetComponent (object sender, EventArgs args)
		{
			_selection = "Component"; //No i18n
		}
		
		private void SetDataType (object sender, EventArgs args)
		{
			_selection = "DataType"; //No i18n
		}
		
		private void SetDeploymentSpecification (object sender, EventArgs args)
		{
			_selection = "DeploymentSpecification"; //No i18n
		}
		
		private void SetDevice (object sender, EventArgs args)
		{
			_selection = "Device"; //No i18n
		}

		private void SetEnumeration (object sender, EventArgs args)
		{
			_selection = "Enumeration"; //No i18n
		}

		private void SetExecutionEnvironment (object sender, EventArgs args)
		{
			_selection = "ExecutionEnvironment"; //No i18n
		}

		private void SetExtension (object sender, EventArgs args)
		{
			_selection = "Extension"; //No i18n
		}

		private void SetInformationItem (object sender, EventArgs args)
		{
			_selection = "InformationItem"; //No i18n
		}
		
		private void SetInteraction (object sender, EventArgs args)
		{
			_selection = "Interaction"; //No i18n
		}
		
		private void SetInterface (object sender, EventArgs args)
		{
			_selection = "Interface"; //No i18n
		}
		
		private void SetNode (object sender, EventArgs args)
		{
			_selection = "Node"; //No i18n
		}
		
		private void SetPrimitiveType (object sender, EventArgs args)
		{
			_selection = "PrimitiveType"; //No i18n
		}
		
		private void SetProtocolStateMachine (object sender, EventArgs args)
		{
			_selection = "ProtocolStateMachine"; //No i18n
		}
		
		private void SetSignal (object sender, EventArgs args)
		{
			_selection = "Signal"; //No i18n
		}
		
		private void SetStateMachine (object sender, EventArgs args)
		{
			_selection = "StateMachine"; //No i18n
		}
		
		private void SetStereotype (object sender, EventArgs args)
		{
			_selection = "Stereotype"; //No i18n
		}
		
		private void SetUseCase (object sender, EventArgs args)
		{
			_selection = "UseCase"; //No i18n
		}
	}
}
