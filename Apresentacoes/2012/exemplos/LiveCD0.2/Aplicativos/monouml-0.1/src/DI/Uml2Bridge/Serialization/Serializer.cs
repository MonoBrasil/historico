/*
MonoUML.DI.Uml2Bridge.Uml2Bridge
Copyright (C) 2004  Rodolfo Campero

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
using Xmi2 = ExpertCoder.Xmi2;
using Uml2 = ExpertCoder.Uml2;
using Uml2Bridge = MonoUML.DI.Uml2Bridge;
using System.Xml;
using System;

namespace MonoUML.DI.Uml2Bridge.Serialization
{
	public class Serializer: Xmi2.XmiSerializerBase
	{
		protected override string DotNetNs
		{
			get { return "MonoUML.DI.Uml2Bridge"; }
		}

		protected override string XmiNs
		{
			get { return URI; }
		}

		protected override void CompleteCurrentElement(string currentNs, string currentType, object current)
		{
			if(object.ReferenceEquals(currentNs, _uml2bridgeNs))
			{
				CompleteElementInNs(currentType, current);
			}
		}

		private void CompleteElementInNs(string currentType, object current)
		{
			if(object.ReferenceEquals(currentType, _uml2SemanticModelBridgeType))
			{
				Complete_Uml2SemanticModelBridge((Uml2Bridge.Uml2SemanticModelBridge)current);
			}
		}
		
		private void Complete_Uml2SemanticModelBridge(Uml2Bridge.Uml2SemanticModelBridge current)
		{
			string attrNs;
			string prop;
			// attributes
			while(base.XmlReader.MoveToNextAttribute())
			{
				attrNs = base.XmlReader.NamespaceURI;
				prop = base.XmlReader.LocalName;
				if(object.ReferenceEquals(attrNs, String.Empty)
					|| object.ReferenceEquals(attrNs, _uml2bridgeNs))
				{
					Set_Uml2SemanticModelBridgeAttribute(current, prop);
				}
			}
			// properties as elements and composite elements
			if(base.MoveToFirstChild())
			{
				do
				{
					attrNs = base.XmlReader.NamespaceURI;
					prop = base.XmlReader.NameTable.Get(base.XmlReader.LocalName);
					if(object.ReferenceEquals(attrNs, String.Empty)
						|| object.ReferenceEquals(attrNs, _uml2bridgeNs))
					{
						Set_Uml2SemanticModelBridgeAttribute(current, prop);
					}
				} while(base.MoveToNextSibling());
			}
		}

		// SetNNN_CCCAttribute methods are for simple attributes and references
		// (NNN = Namespace, CCC = Diagram)
		
		private bool Set_Uml2SemanticModelBridgeAttribute(Uml2Bridge.Uml2SemanticModelBridge current, string prop)
		{
			if(object.ReferenceEquals(prop, _diagram))
			{
				current.Diagram = (DI.Diagram)base.IdMapper.GetObject(base.StringValue);;
				return true;
			}
			else if(object.ReferenceEquals(prop, _element))
			{
				current.Element = (Uml2.Element)base.IdMapper.GetObject(base.StringValue);;
				return true;
			}
			else if(object.ReferenceEquals(prop, _graphElement))
			{
				current.GraphElement = (DI.GraphElement)base.IdMapper.GetObject(base.StringValue);;
				return true;
			}
			else if(object.ReferenceEquals(prop, _presentation))
			{
				current.Presentation = base.StringValue;
				return true;
			}
			return false;
		}

		protected override void DeclareNamespaces()
		{
			base.XmlWriter.WriteAttributeString("xmlns", _uml2bridgePrefix, null, _uml2bridgeNs);
			return;
		}

		protected override object DeserializeCurrentElement(string nsUri, string type)
		{
			return DeserializeElementInNs(type);
		}

		private object DeserializeElementInNs(string xmiType)
		{
			// composition is handled in the Deserialize<Namespace>_<elementType> methods
			if(object.ReferenceEquals(xmiType, _uml2SemanticModelBridgeType)) { return Deserialize_Uml2SemanticModelBridge(); }
			else { return null; }
		}

		private object Deserialize_Uml2SemanticModelBridge()
		{
			Uml2Bridge.Uml2SemanticModelBridge deserialized = new Uml2Bridge.Uml2SemanticModelBridge();
			SkipAllAndWornAsUnknown();
			return deserialized;
		}

		private void SkipAllAndWornAsUnknown()
		{
			string attrNs, prop;
			if(base.MoveToFirstChild())
			{
				do
				{
					attrNs = base.XmlReader.NamespaceURI;
					prop = base.XmlReader.NameTable.Get(base.XmlReader.LocalName);
					if(object.ReferenceEquals(attrNs, String.Empty)
						|| object.ReferenceEquals(attrNs, _implUml2BridgeNs))
					{
						base.WarnUnknownElement(prop);
					}
				} while(base.MoveToNextSibling());
			}
		}

		protected override void InitCompletion()
		{
			InitDeserialization();
			XmlNameTable nt = base.NT;
			// declare here only primitive and reference properties
			_diagram = nt.Add("diagram");
			_element = nt.Add("element");
			_graphElement = nt.Add("graphElement");
			_presentation = nt.Add("presentation");
		}

		protected override void InitDeserialization()
		{
			XmlNameTable nt = base.NT;
			// declare here xmi namespaces, types and composite properties
			_uml2bridgeNs = nt.Add(URI);
			_uml2bridgePrefix = nt.Add("uml2bridge");
			_uml2SemanticModelBridgeType = nt.Add("Uml2SemanticModelBridge");
		}

		protected override void InitSerialization()
		{
			InitCompletion();
			XmlNameTable nt = base.NT;
			// declare here xmi namespaces and implementation names
			_implUml2BridgeNs = nt.Add("MonoUML.DI.Uml2Bridge");
			_uml2bridgeNs = nt.Add(URI);
			_uml2bridgePrefix = nt.Add("uml2bridge");
			_uml2SemanticModelBridgeType = nt.Add("Uml2SemanticModelBridge");
		}
		
		protected override string GetXmlNs(string implNs)
		{
			if(object.ReferenceEquals(implNs, _implUml2BridgeNs))
			{
				return _uml2bridgeNs;
			}
			else
			{
				throw new ApplicationException("Unknown namespace: " + implNs);
			}
		}
		
		protected override string GetXmlPrefix(string implNs)
		{
			if(object.ReferenceEquals(implNs, _implUml2BridgeNs))
			{
				return _uml2bridgePrefix;
			}
			else
			{
				throw new ApplicationException("Unknown namespace: " + implNs);
			}
		}

		protected override string GetXsiType(string implNs, string implType)
		{
			return implType;
		}

		protected override void SerializeObject(object current, string implNs, string implType)
		{
			if(!object.ReferenceEquals(implNs, _implUml2BridgeNs))
			{
				throw new ApplicationException("Unknown namespace: " + implNs);
			}
			if(object.ReferenceEquals(implType, _uml2SemanticModelBridgeType))
			{
				Uml2Bridge.Uml2SemanticModelBridge uml2bridge = (Uml2Bridge.Uml2SemanticModelBridge)current;
				SerializeAttributesIn_Uml2SemanticModelBridge(uml2bridge);
			}
			else
			{
				throw new ApplicationException("Unknown object type: " + current.ToString());
			}
			return;
		}

		// ------------------------ serialize attributes

		private void SerializeAttributesIn_Uml2SemanticModelBridge(Uml2Bridge.Uml2SemanticModelBridge current)
		{
			base.WritePropertyValue(_diagram, base.GetIdAndEnqueue(current.Diagram));
			base.WritePropertyValue(_element, base.GetIdAndEnqueue(current.Element));
			base.WritePropertyValue(_graphElement, base.GetIdAndEnqueue(current.GraphElement));
			base.WritePropertyValue(_presentation, current.Presentation);
		}
		
		private string _diagram;
		private string _element;
		private string _graphElement;
		private string _implUml2BridgeNs;
		private string _presentation;
		private string _uml2bridgeNs;
		private string _uml2bridgePrefix;
		private string _uml2SemanticModelBridgeType;
		private const string URI = "http://monouml.sourceforge.net/uml2bridge"; 
	}
}
