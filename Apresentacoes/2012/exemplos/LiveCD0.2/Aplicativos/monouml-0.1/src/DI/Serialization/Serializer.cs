/*
MonoUML.DI - A library for handling Diagram Interchange elements
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
using System.Collections;
using Xmi2 = ExpertCoder.Xmi2;
using DI = MonoUML.DI;
using System.Xml;
using System;

namespace MonoUML.DI.Serialization
{
	public class Serializer: Xmi2.XmiSerializerBase
	{
		protected override string DotNetNs
		{
			get { return "MonoUML.DI"; }
		}

		protected override string XmiNs
		{
			get { return URI; }
		}

		protected override void CompleteCurrentElement(string currentNs, string currentType, object current)
		{
			if(object.ReferenceEquals(currentNs, _diNs))
			{
				CompleteElementInNs(currentType, current);
			}
		}

		private void CompleteElementInNs(string currentType, object current)
		{
			if(object.ReferenceEquals(currentType, _bezierPointType))
			{
				Complete_BezierPoint((DI.BezierPoint)current);
			}
			else if(object.ReferenceEquals(currentType, _diagramType))
			{
				Complete_Diagram((DI.Diagram)current);
			}
			else if(object.ReferenceEquals(currentType, _diagramLinkType))
			{
				Complete_DiagramLink((DI.DiagramLink)current);
			}
			else if(object.ReferenceEquals(currentType, _dimensionType))
			{
				Complete_Dimension((DI.Dimension)current);
			}
			else if(object.ReferenceEquals(currentType, _ellipseType))
			{
				Complete_Ellipse((DI.Ellipse)current);
			}
			else if(object.ReferenceEquals(currentType, _graphConnectorType))
			{
				Complete_GraphConnector((DI.GraphConnector)current);
			}
			else if(object.ReferenceEquals(currentType, _graphEdgeType))
			{
				Complete_GraphEdge((DI.GraphEdge)current);
			}
			else if(object.ReferenceEquals(currentType, _graphNodeType))
			{
				Complete_GraphNode((DI.GraphNode)current);
			}
			else if(object.ReferenceEquals(currentType, _imageType))
			{
				Complete_Image((DI.Image)current);
			}
			else if(object.ReferenceEquals(currentType, _pointType))
			{
				Complete_Point((DI.Point)current);
			}
			else if(object.ReferenceEquals(currentType, _polylineType))
			{
				Complete_Polyline((DI.Polyline)current);
			}
			else if(object.ReferenceEquals(currentType, _propertyType))
			{
				Complete_Property((DI.Property)current);
			}
			else if(object.ReferenceEquals(currentType, _referenceType))
			{
				Complete_Reference((DI.Reference)current);
			}
			else if(object.ReferenceEquals(currentType, _simpleSemanticModelElementType))
			{
				Complete_SimpleSemanticModelElement((DI.SimpleSemanticModelElement)current);
			}
			else if(object.ReferenceEquals(currentType, _textElementType))
			{
				Complete_TextElement((DI.TextElement)current);
			}
		}
		
		private void Complete_BezierPoint(DI.BezierPoint current)
		{
			string attrNs;
			string prop;
			// attributes
			while(base.XmlReader.MoveToNextAttribute())
			{
				attrNs = base.XmlReader.NamespaceURI;
				prop = base.XmlReader.LocalName;
				if(object.ReferenceEquals(attrNs, String.Empty)
					|| object.ReferenceEquals(attrNs, _diNs))
				{
					if(!Set_BezierPointAttribute(current, prop))
					Set_PointAttribute(current, prop);
				}
			}
			if(base.MoveToFirstChild())
			{
				do {} while(base.MoveToNextSibling());
			}
		}
		
		private void Complete_Diagram(DI.Diagram current)
		{
			string attrNs;
			string prop;
			// attributes
			while(base.XmlReader.MoveToNextAttribute())
			{
				attrNs = base.XmlReader.NamespaceURI;
				prop = base.XmlReader.LocalName;
				if(object.ReferenceEquals(attrNs, String.Empty)
					|| object.ReferenceEquals(attrNs, _diNs))
				{
					if(!Set_DiagramAttribute(current, prop))
					Set_DiagramElementAttribute(current, prop);
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
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						// check composite elements
						if(!CompleteCompElemsIn_Diagram(prop))
						if(!CompleteCompElemsIn_GraphNode(prop))
						if(!CompleteCompElemsIn_GraphElement(prop))
						CompleteCompElemsIn_DiagramElement(prop);
					}
				} while(base.MoveToNextSibling());
			}
		}

		private void Complete_DiagramLink(DI.DiagramLink current)
		{
			string attrNs;
			string prop;
			// attributes
			while(base.XmlReader.MoveToNextAttribute())
			{
				attrNs = base.XmlReader.NamespaceURI;
				prop = base.XmlReader.LocalName;
				if(object.ReferenceEquals(attrNs, String.Empty)
					|| object.ReferenceEquals(attrNs, _diNs))
				{
					Set_DiagramLinkAttribute(current, prop);
				}
			}
			if(base.MoveToFirstChild())
			{
				do {} while(base.MoveToNextSibling());
			}
		}
		
		private void Complete_Ellipse(DI.Ellipse current)
		{
			string attrNs;
			string prop;
			// attributes
			while(base.XmlReader.MoveToNextAttribute())
			{
				attrNs = base.XmlReader.NamespaceURI;
				prop = base.XmlReader.LocalName;
				if(object.ReferenceEquals(attrNs, String.Empty)
					|| object.ReferenceEquals(attrNs, _diNs))
				{
					if(!Set_EllipseAttribute(current, prop))
					Set_DiagramElementAttribute(current, prop);
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
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						// check composite elements
						if(!CompleteCompElemsIn_Ellipse(prop))
						CompleteCompElemsIn_DiagramElement(prop);
					}
				} while(base.MoveToNextSibling());
			}
		}

		private void Complete_Dimension(DI.Dimension current)
		{
			string attrNs;
			string prop;
			// attributes
			while(base.XmlReader.MoveToNextAttribute())
			{
				attrNs = base.XmlReader.NamespaceURI;
				prop = base.XmlReader.LocalName;
				if(object.ReferenceEquals(attrNs, String.Empty)
					|| object.ReferenceEquals(attrNs, _diNs))
				{
					Set_DimensionAttribute(current, prop);
				}
			}
			// properties as elements
			if(base.MoveToFirstChild())
			{
				do
				{
					attrNs = base.XmlReader.NamespaceURI;
					prop = base.XmlReader.NameTable.Get(base.XmlReader.LocalName);
					if(object.ReferenceEquals(attrNs, String.Empty)
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						// check attributes
						Set_DimensionAttribute(current, prop);
					}
				} while(base.MoveToNextSibling());
			}
		}

		private void Complete_GraphConnector(DI.GraphConnector current)
		{
			string attrNs;
			string prop;
			// properties as attributes
			while(base.XmlReader.MoveToNextAttribute())
			{
				attrNs = base.XmlReader.NamespaceURI;
				prop = base.XmlReader.LocalName;
				if(object.ReferenceEquals(attrNs, String.Empty)
					|| object.ReferenceEquals(attrNs, _diNs))
				{
					Set_GraphConnectorAttribute(current, prop);
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
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						// check composite elements
						CompleteCompElemsIn_GraphConnector(prop);
					}
				} while(base.MoveToNextSibling());
			}
		}

		private void Complete_GraphEdge(DI.GraphEdge current)
		{
			string attrNs;
			string prop;
			// properties as attributes
			while(base.XmlReader.MoveToNextAttribute())
			{
				attrNs = base.XmlReader.NamespaceURI;
				prop = base.XmlReader.LocalName;
				if(object.ReferenceEquals(attrNs, String.Empty)
					|| object.ReferenceEquals(attrNs, _diNs))
				{
					if(!Set_GraphEdgeAttribute(current, prop))
					Set_DiagramElementAttribute(current, prop);
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
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						if(!Set_GraphEdgeAttribute(current, prop))
						// check composite elements
						if(!CompleteCompElemsIn_GraphEdge(prop))
						if(!CompleteCompElemsIn_GraphElement(prop))
						CompleteCompElemsIn_DiagramElement(prop);
					}
				} while(base.MoveToNextSibling());
			}
		}
		
		private void Complete_GraphNode(DI.GraphNode current)
		{
			string attrNs;
			string prop;
			// properties as attributes
			while(base.XmlReader.MoveToNextAttribute())
			{
				attrNs = base.XmlReader.NamespaceURI;
				prop = base.XmlReader.LocalName;
				if(object.ReferenceEquals(attrNs, String.Empty)
					|| object.ReferenceEquals(attrNs, _diNs))
				{
					Set_DiagramElementAttribute(current, prop);
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
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						// check composite elements
						if(!CompleteCompElemsIn_GraphNode(prop))
						if(!CompleteCompElemsIn_GraphElement(prop))
						CompleteCompElemsIn_DiagramElement(prop);
					}
				} while(base.MoveToNextSibling());
			}
		}

		private void Complete_Image(DI.Image current)
		{
			string attrNs;
			string prop;
			// properties as attributes
			while(base.XmlReader.MoveToNextAttribute())
			{
				attrNs = base.XmlReader.NamespaceURI;
				prop = base.XmlReader.LocalName;
				if(object.ReferenceEquals(attrNs, String.Empty)
					|| object.ReferenceEquals(attrNs, _diNs))
				{
					if(!Set_ImageAttribute(current, prop))
					Set_DiagramElementAttribute(current, prop);
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
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						// check composite elements
						CompleteCompElemsIn_DiagramElement(prop);
					}
				} while(base.MoveToNextSibling());
			}
		}

		private void Complete_Point(DI.Point current)
		{
			string attrNs;
			string prop;
			// attributes
			while(base.XmlReader.MoveToNextAttribute())
			{
				attrNs = base.XmlReader.NamespaceURI;
				prop = base.XmlReader.LocalName;
				if(object.ReferenceEquals(attrNs, String.Empty)
					|| object.ReferenceEquals(attrNs, _diNs))
				{
					Set_PointAttribute(current, prop);
				}
			}
			// properties as elements
			if(base.MoveToFirstChild())
			{
				do
				{
					attrNs = base.XmlReader.NamespaceURI;
					prop = base.XmlReader.NameTable.Get(base.XmlReader.LocalName);
					if(object.ReferenceEquals(attrNs, String.Empty)
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						// check attributes
						Set_PointAttribute(current, prop);
					}
				} while(base.MoveToNextSibling());
			}
		}

		private void Complete_Polyline(DI.Polyline current)
		{
			string attrNs;
			string prop;
			// attributes
			while(base.XmlReader.MoveToNextAttribute())
			{
				attrNs = base.XmlReader.NamespaceURI;
				prop = base.XmlReader.LocalName;
				if(object.ReferenceEquals(attrNs, String.Empty)
					|| object.ReferenceEquals(attrNs, _diNs))
				{
					if(!Set_PolylineAttribute(current, prop))
					Set_DiagramElementAttribute(current, prop);
				}
			}
		}

		private void Complete_Property(DI.Property current)
		{
			if(base.MoveToFirstChild())
			{
				do {} while(base.MoveToNextSibling());
			}
		}

		private void Complete_Reference(DI.Reference current)
		{
			string attrNs;
			string prop;
			// properties as attributes
			while(base.XmlReader.MoveToNextAttribute())
			{
				attrNs = base.XmlReader.NamespaceURI;
				prop = base.XmlReader.LocalName;
				if(object.ReferenceEquals(attrNs, String.Empty)
					|| object.ReferenceEquals(attrNs, _diNs))
				{
					Set_ReferenceAttribute(current, prop);
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
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						// check composite elements
						Set_ReferenceAttribute(current, prop);
					}
				} while(base.MoveToNextSibling());
			}
		}

		private void Complete_SimpleSemanticModelElement(DI.SimpleSemanticModelElement current)
		{
			string attrNs;
			string prop;
			// properties as attributes
			while(base.XmlReader.MoveToNextAttribute())
			{
				attrNs = base.XmlReader.NamespaceURI;
				prop = base.XmlReader.LocalName;
				if(object.ReferenceEquals(attrNs, String.Empty)
					|| object.ReferenceEquals(attrNs, _diNs))
				{
					if(!Set_SimpleSemanticModelElementAttribute(current, prop))
					Set_SemanticModelBridgeAttribute(current, prop);
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
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						// check composite elements
						if(!Set_SimpleSemanticModelElementAttribute(current, prop))
						Set_SemanticModelBridgeAttribute(current, prop);
					}
				} while(base.MoveToNextSibling());
			}
		}

		private void Complete_TextElement(DI.TextElement current)
		{
			string attrNs;
			string prop;
			// properties as attributes
			while(base.XmlReader.MoveToNextAttribute())
			{
				attrNs = base.XmlReader.NamespaceURI;
				prop = base.XmlReader.LocalName;
				if(object.ReferenceEquals(attrNs, String.Empty)
					|| object.ReferenceEquals(attrNs, _diNs))
				{
					if(!Set_TextElementAttribute(current, prop))
					Set_DiagramElementAttribute(current, prop);
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
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						// check composite elements
						CompleteCompElemsIn_DiagramElement(prop);
					}
				} while(base.MoveToNextSibling());
			}
		}
		
		// SetNNN_CCCAttribute methods are for simple attributes and references
		// (NNN = Namespace, CCC = Diagram)
		
		private bool Set_BezierPointAttribute(DI.BezierPoint current, string prop)
		{
			return false;
		}
		
		private bool Set_DiagramAttribute(DI.Diagram current, string prop)
		{
			if(object.ReferenceEquals(prop, _name))
			{
				current.Name = base.StringValue;
				return true;
			}
			else if(object.ReferenceEquals(prop, _zoom))
			{
				current.Zoom = base.DoubleValue;
				return true;
			}
			else if(object.ReferenceEquals(prop, _diagramLink))
			{
				foreach(object element in base.ObjectArrayValue)
				{
					if(!current.DiagramLink.Contains(element)) current.DiagramLink.Add(element);
				}
				return true;
			}
			return false;
		}
		
		private bool Set_DiagramLinkAttribute(DI.DiagramLink current, string prop)
		{
			if(object.ReferenceEquals(prop, _diagram))
			{
				current.Diagram = (DI.Diagram)base.IdMapper.GetObject(base.StringValue);
				return true;
			}
			else if(object.ReferenceEquals(prop, _zoom))
			{
				current.Zoom = base.DoubleValue;
				return true;
			}
			return false;
		}
		
		private bool Set_DiagramElementAttribute(DI.DiagramElement current, string prop)
		{
			if(object.ReferenceEquals(prop, _isVisible))
			{
				current.IsVisible = base.BooleanValue;
				return true;
			}
			else if(object.ReferenceEquals(prop, _reference))
			{
				foreach(object element in base.ObjectArrayValue)
				{
					if(!current.Reference.Contains(element)) current.Reference.Add(element);
				}
				return true;
			}
			else if(object.ReferenceEquals(prop, _property))
			{
				foreach(object element in base.ObjectArrayValue)
				{
					if(!current.Property.Contains((DI.Property)element)) current.Property.Add((DI.Property)element);
				}
				return true;
			}
			return false;
		}

		private bool Set_EllipseAttribute(DI.Ellipse current, string prop)
		{
			if(object.ReferenceEquals(prop, _endAngle))
			{
				current.EndAngle = base.DoubleValue;
				return true;
			}
			else if(object.ReferenceEquals(prop, _radiusX))
			{
				current.RadiusX = base.DoubleValue;
				return true;
			}
			else if(object.ReferenceEquals(prop, _radiusX))
			{
				current.RadiusY = base.DoubleValue;
				return true;
			}
			else if(object.ReferenceEquals(prop, _rotation))
			{
				current.Rotation = base.DoubleValue;
				return true;
			}
			else if(object.ReferenceEquals(prop, _startAngle))
			{
				current.StartAngle = base.DoubleValue;
				return true;
			}
			return false;
		}

		private bool Set_DimensionAttribute(DI.Dimension current, string prop)
		{
			if(object.ReferenceEquals(prop, _height))
			{
				current.Height = base.DoubleValue;
				return true;
			}
			else if(object.ReferenceEquals(prop, _width))
			{
				current.Width = base.DoubleValue;
				return true;
			}
			return false;
		}

		private bool Set_GraphConnectorAttribute(DI.GraphConnector current, string prop)
		{
			if(object.ReferenceEquals(prop, _graphEdge))
			{
				foreach(object element in base.ObjectArrayValue)
				{
					if(!current.GraphEdge.Contains(element)) current.GraphEdge.Add(element);
				}
				return true;
			}
			return false;
		}
		
		private bool Set_GraphEdgeAttribute(DI.GraphEdge current, string prop)
		{
			if(object.ReferenceEquals(prop, _anchor))
			{
				foreach(GraphConnector graphConnector in base.ObjectArrayValue)
				{
					if(!current.Anchor.Contains(graphConnector)) current.Anchor.Add(graphConnector);
					if(!graphConnector.GraphEdge.Contains(current)) graphConnector.GraphEdge.Add(current);
				}
				return true;
			}
			return false;
		}

		private bool Set_ImageAttribute(DI.Image current, string prop)
		{
			if(object.ReferenceEquals(prop, _mimeType))
			{
				current.MimeType = base.StringValue;
				return true;
			}
			else if(object.ReferenceEquals(prop, _uri))
			{
				current.Uri = base.StringValue;
				return true;
			}
			return false;
		}

		private bool Set_PointAttribute(DI.Point current, string prop)
		{
			if(object.ReferenceEquals(prop, _x))
			{
				current.X = base.DoubleValue;
				return true;
			}
			else if(object.ReferenceEquals(prop, _y))
			{
				current.Y = base.DoubleValue;
				return true;
			}
			return false;
		}

		private bool Set_PolylineAttribute(DI.Polyline current, string prop)
		{
			if(object.ReferenceEquals(prop, _closed))
			{
				current.Closed = base.BooleanValue;
				return true;
			}
			return false;
		}

		private bool Set_PropertyAttribute(DI.Property current, string prop)
		{
			if(object.ReferenceEquals(prop, _key))
			{
				current.Key = base.StringValue;
				return true;
			}
			else if(object.ReferenceEquals(prop, _value))
			{
				current.Value = base.StringValue;
				return true;
			}
			return false;
		}
		
		private bool Set_ReferenceAttribute(DI.Reference current, string prop)
		{
			if(object.ReferenceEquals(prop, _isIndividualRepresentation))
			{
				current.IsIndividualRepresentation = base.BooleanValue;
				return true;
			}
			return false;
		}

		private bool Set_SemanticModelBridgeAttribute(DI.SemanticModelBridge current, string prop)
		{
			if(object.ReferenceEquals(prop, _presentation))
			{
				current.Presentation = base.StringValue;
				return true;
			}
			return false;
		}

		private bool Set_SimpleSemanticModelElementAttribute(DI.SimpleSemanticModelElement current, string prop)
		{
			if(object.ReferenceEquals(prop, _typeInfo))
			{
				current.TypeInfo = base.StringValue;
				return true;
			}
			return false;
		}

		private bool Set_TextElementAttribute(DI.TextElement current, string prop)
		{
			if(object.ReferenceEquals(prop, _text))
			{
				current.Text = base.StringValue;
				return true;
			}
			return false;
		}
		
		private bool CompleteCompElemsIn_Diagram(string prop)
		{
			if(object.ReferenceEquals(prop, _namespace))
			{
				base.IdentifyAndCompleteElement(_diNs, _semanticModelBridgeType);
				return true;
			}
			else if(object.ReferenceEquals(prop, _viewport))
			{
				base.IdentifyAndCompleteElement(_diNs, _pointType);
				return true;
			}
			return false;
		}
		
		private bool CompleteCompElemsIn_DiagramElement(string prop)
		{
			if(object.ReferenceEquals(prop, _property))
			{
				base.IdentifyAndCompleteElement(_diNs, _propertyType);
				return true;
			}
			return false;
		}

		private bool CompleteCompElemsIn_Ellipse(string prop)
		{
			if(object.ReferenceEquals(prop, _center))
			{
				base.IdentifyAndCompleteElement(_diNs, _pointType);
				return true;
			}
			return false;
		}

		private bool CompleteCompElemsIn_GraphConnector(string prop)
		{
			if(object.ReferenceEquals(prop, _position))
			{
				base.IdentifyAndCompleteElement(_diNs, _pointType);
				return true;
			}
			return false;
		}
		
		private bool CompleteCompElemsIn_GraphEdge(string prop)
		{
			if(object.ReferenceEquals(prop, _waypoints))
			{
				base.IdentifyAndCompleteElement(_diNs, _pointType);
				return true;
			}
			return false;
		}

		private bool CompleteCompElemsIn_GraphElement(string prop)
		{
			if(object.ReferenceEquals(prop, _anchorage))
			{
				base.IdentifyAndCompleteElement(_diNs, _graphConnectorType);
				return true;
			}
			else if(object.ReferenceEquals(prop, _contained))
			{
				base.IdentifyAndCompleteElement(_diNs, _diagramElementType);
				return true;
			}
			else if(object.ReferenceEquals(prop, _link))
			{
				base.IdentifyAndCompleteElement(_diNs, _diagramLinkType);
				return true;
			}
			else if(object.ReferenceEquals(prop, _semanticModel))
			{
				base.IdentifyAndCompleteElement(_diNs, _semanticModelBridgeType);
				return true;
			}
			else if(object.ReferenceEquals(prop, _position))
			{
				base.IdentifyAndCompleteElement(_diNs, _pointType);
				return true;
			}
			return false;
		}

		private bool CompleteCompElemsIn_GraphNode(string prop)
		{
			if(object.ReferenceEquals(prop, _size))
			{
				base.IdentifyAndCompleteElement(_diNs, _dimensionType);
				return true;
			}
			return false;
		}

		protected override void DeclareNamespaces()
		{
			base.XmlWriter.WriteAttributeString("xmlns", "di", null, _diNs);
			return;
		}

		protected override object DeserializeCurrentElement(string nsUri, string type)
		{
			return DeserializeElementInNs(type);
		}

		private object DeserializeElementInNs(string xmiType)
		{
			// composition is handled in the Deserialize<Namespace>_<elementType> methods
			if(object.ReferenceEquals(xmiType, _bezierPointType)) { return Deserialize_BezierPoint(); }
			else if(object.ReferenceEquals(xmiType, _diagramType)) { return Deserialize_Diagram(); }
			else if(object.ReferenceEquals(xmiType, _diagramLinkType)) { return Deserialize_DiagramLink(); }
			else if(object.ReferenceEquals(xmiType, _dimensionType)) { return Deserialize_Dimension(); }
			else if(object.ReferenceEquals(xmiType, _ellipseType)) { return Deserialize_Ellipse(); }
			else if(object.ReferenceEquals(xmiType, _graphConnectorType)) { return Deserialize_GraphConnector(); }
			else if(object.ReferenceEquals(xmiType, _graphEdgeType)) { return Deserialize_GraphEdge(); }
			else if(object.ReferenceEquals(xmiType, _graphNodeType)) { return Deserialize_GraphNode(); }
			else if(object.ReferenceEquals(xmiType, _imageType)) { return Deserialize_Image(); }
			else if(object.ReferenceEquals(xmiType, _pointType)) { return Deserialize_Point(); }
			else if(object.ReferenceEquals(xmiType, _polylineType)) { return Deserialize_Polyline(); }
			else if(object.ReferenceEquals(xmiType, _propertyType)) { return Deserialize_Property(); }
			else if(object.ReferenceEquals(xmiType, _referenceType)) { return Deserialize_Reference(); }
			else if(object.ReferenceEquals(xmiType, _simpleSemanticModelElementType)) { return Deserialize_SimpleSemanticModelElement(); }
			else if(object.ReferenceEquals(xmiType, _textElementType)) { return Deserialize_TextElement(); }
			else { return null; }
		}

		private object Deserialize_BezierPoint()
		{
			string attrNs, prop;
			DI.BezierPoint deserialized = new DI.BezierPoint();
			if(base.MoveToFirstChild())
			{
				do
				{
					attrNs = base.XmlReader.NamespaceURI;
					prop = base.XmlReader.NameTable.Get(base.XmlReader.LocalName);
					if(object.ReferenceEquals(attrNs, String.Empty)
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						if(!DeserCompElemsIn_BezierPoint(deserialized, prop))
						base.WarnUnknownElement(prop);
					}
				} while(base.MoveToNextSibling());
			}
			return deserialized;
		}

		private object Deserialize_Diagram()
		{
			string attrNs, prop;
			DI.Diagram deserialized = new DI.Diagram();
			if(base.MoveToFirstChild())
			{
				do
				{
					attrNs = base.XmlReader.NamespaceURI;
					prop = base.XmlReader.NameTable.Get(base.XmlReader.LocalName);
					if(object.ReferenceEquals(attrNs, String.Empty)
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						if(!DeserCompElemsIn_Diagram(deserialized, prop))
						if(!DeserCompElemsIn_GraphNode(deserialized, prop))
						if(!DeserCompElemsIn_GraphElement(deserialized, prop))
						if(!DeserCompElemsIn_DiagramElement(deserialized, prop))
						base.WarnUnknownElement(prop);
					}
				} while(base.MoveToNextSibling());
			}
			return deserialized;
		}

		private object Deserialize_DiagramLink()
		{
			string attrNs, prop;
			DI.DiagramLink deserialized = new DI.DiagramLink();
			if(base.MoveToFirstChild())
			{
				do
				{
					attrNs = base.XmlReader.NamespaceURI;
					prop = base.XmlReader.NameTable.Get(base.XmlReader.LocalName);
					if(object.ReferenceEquals(attrNs, String.Empty)
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						if(!DeserCompElemsIn_DiagramLink(deserialized, prop))
						base.WarnUnknownElement(prop);
					}
				} while(base.MoveToNextSibling());
			}
			return deserialized;
		}

		private object Deserialize_Ellipse()
		{
			string attrNs, prop;
			DI.Ellipse deserialized = new DI.Ellipse();
			if(base.MoveToFirstChild())
			{
				do
				{
					attrNs = base.XmlReader.NamespaceURI;
					prop = base.XmlReader.NameTable.Get(base.XmlReader.LocalName);
					if(object.ReferenceEquals(attrNs, String.Empty)
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						if(!DeserCompElemsIn_Ellipse(deserialized, prop))
						if(!DeserCompElemsIn_DiagramElement(deserialized, prop))
						base.WarnUnknownElement(prop);
					}
				} while(base.MoveToNextSibling());
			}
			return deserialized;
		}

		private object Deserialize_Dimension()
		{
			DI.Dimension deserialized = new DI.Dimension();
			SkipAllAndWornAsUnknown();
			return deserialized;
		}

		private object Deserialize_GraphConnector()
		{
			string attrNs, prop;
			DI.GraphConnector deserialized = new DI.GraphConnector();
			if(base.MoveToFirstChild())
			{
				do
				{
					attrNs = base.XmlReader.NamespaceURI;
					prop = base.XmlReader.NameTable.Get(base.XmlReader.LocalName);
					if(object.ReferenceEquals(attrNs, String.Empty)
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						if(!DeserCompElemsIn_GraphConnector(deserialized, prop))
						base.WarnUnknownElement(prop);
					}
				} while(base.MoveToNextSibling());
			}
			return deserialized;
		}

		private object Deserialize_GraphEdge()
		{
			string attrNs, prop;
			DI.GraphEdge deserialized = new DI.GraphEdge();
			if(base.MoveToFirstChild())
			{
				do
				{
					attrNs = base.XmlReader.NamespaceURI;
					prop = base.XmlReader.NameTable.Get(base.XmlReader.LocalName);
					if(object.ReferenceEquals(attrNs, String.Empty)
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						if(!DeserCompElemsIn_GraphEdge(deserialized, prop))
						if(!DeserCompElemsIn_GraphElement(deserialized, prop))
						if(!DeserCompElemsIn_DiagramElement(deserialized, prop))
						base.WarnUnknownElement(prop);
					}
				} while(base.MoveToNextSibling());
			}
			return deserialized;
		}

		private object Deserialize_GraphElement()
		{
			string attrNs, prop;
			DI.GraphElement deserialized = new DI.GraphElement();
			if(base.MoveToFirstChild())
			{
				do
				{
					attrNs = base.XmlReader.NamespaceURI;
					prop = base.XmlReader.NameTable.Get(base.XmlReader.LocalName);
					if(object.ReferenceEquals(attrNs, String.Empty)
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						if(!DeserCompElemsIn_GraphElement(deserialized, prop))
						if(!DeserCompElemsIn_DiagramElement(deserialized, prop))
						base.WarnUnknownElement(prop);
					}
				} while(base.MoveToNextSibling());
			}
			return deserialized;
		}

		private object Deserialize_GraphNode()
		{
			string attrNs, prop;
			DI.GraphNode deserialized = new DI.GraphNode();
			if(base.MoveToFirstChild())
			{
				do
				{
					attrNs = base.XmlReader.NamespaceURI;
					prop = base.XmlReader.NameTable.Get(base.XmlReader.LocalName);
					if(object.ReferenceEquals(attrNs, String.Empty)
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						if(!DeserCompElemsIn_GraphNode(deserialized, prop))
						if(!DeserCompElemsIn_GraphElement(deserialized, prop))
						if(!DeserCompElemsIn_DiagramElement(deserialized, prop))
						base.WarnUnknownElement(prop);
					}
				} while(base.MoveToNextSibling());
			}
			return deserialized;
		}

		private object Deserialize_Image()
		{
			string attrNs, prop;
			DI.Image deserialized = new DI.Image();
			if(base.MoveToFirstChild())
			{
				do
				{
					attrNs = base.XmlReader.NamespaceURI;
					prop = base.XmlReader.NameTable.Get(base.XmlReader.LocalName);
					if(object.ReferenceEquals(attrNs, String.Empty)
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						if(!DeserCompElemsIn_DiagramElement(deserialized, prop))
						base.WarnUnknownElement(prop);
					}
				} while(base.MoveToNextSibling());
			}
			return deserialized;
		}

		private object Deserialize_Point()
		{
			DI.Point deserialized = new DI.Point();
			SkipAllAndWornAsUnknown();
			return deserialized;
		}

		private object Deserialize_Polyline()
		{
			string attrNs, prop;
			DI.Polyline deserialized = new DI.Polyline();
			if(base.MoveToFirstChild())
			{
				do
				{
					attrNs = base.XmlReader.NamespaceURI;
					prop = base.XmlReader.NameTable.Get(base.XmlReader.LocalName);
					if(object.ReferenceEquals(attrNs, String.Empty)
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						if(!DeserCompElemsIn_Polyline(deserialized, prop))
						if(!DeserCompElemsIn_DiagramElement(deserialized, prop))
						base.WarnUnknownElement(prop);
					}
				} while(base.MoveToNextSibling());
			}
			return deserialized;
		}

		private object Deserialize_Property()
		{
			string attrNs, prop;
			DI.Property deserialized = new DI.Property();
			// properties as attributes
			while(base.XmlReader.MoveToNextAttribute())
			{
				attrNs = base.XmlReader.NamespaceURI;
				prop = base.XmlReader.LocalName;
				if(object.ReferenceEquals(attrNs, String.Empty)
					|| object.ReferenceEquals(attrNs, _diNs))
				{
					Set_PropertyAttribute(deserialized, prop);
				}
			}
			if(base.MoveToFirstChild())
			{
				do
				{
					attrNs = base.XmlReader.NamespaceURI;
					prop = base.XmlReader.NameTable.Get(base.XmlReader.LocalName);
					if(object.ReferenceEquals(attrNs, String.Empty)
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						// check composite elements
						Set_PropertyAttribute(deserialized, prop);
					}
				} while(base.MoveToNextSibling());
			}
			return deserialized;
		}

		private object Deserialize_Reference()
		{
			DI.Reference deserialized = new DI.Reference();
			SkipAllAndWornAsUnknown();
			return deserialized;
		}

		private object Deserialize_SimpleSemanticModelElement()
		{
			DI.SimpleSemanticModelElement deserialized = new DI.SimpleSemanticModelElement();
			SkipAllAndWornAsUnknown();
			return deserialized;
		}

		private object Deserialize_TextElement()
		{
			DI.TextElement deserialized = new DI.TextElement();
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
						|| object.ReferenceEquals(attrNs, _diNs))
					{
						base.WarnUnknownElement(prop);
					}
				} while(base.MoveToNextSibling());
			}
		}

		private bool DeserCompElemsIn_BezierPoint(DI.BezierPoint current, string prop)
		{
			if(object.ReferenceEquals(prop, _controls))
			{
				DI.Point control = (DI.Point)IdentifyAndDeserializeElement(_diNs, _pointType); 
				current.Controls.Add(control);
				return true;
			}
			return false;
		}
		
		private bool DeserCompElemsIn_Diagram(DI.Diagram current, string prop)
		{
			if(object.ReferenceEquals(prop, _namespace))
			{
				DI.SemanticModelBridge @namespace = (DI.SemanticModelBridge)IdentifyAndDeserializeElement(_diNs, _semanticModelBridgeType);
				current.Namespace = @namespace;
				@namespace.Diagram = current;
				return true;
			}
			else if(object.ReferenceEquals(prop, _viewport))
			{
				DI.Point viewport = (DI.Point)IdentifyAndDeserializeElement(_diNs, _pointType);
				current.SetViewport(viewport);
				return true;
			}
			return false;
		}

		private bool DeserCompElemsIn_DiagramElement(DI.DiagramElement current, string prop)
		{
			if(object.ReferenceEquals(prop, _property))
			{
				DI.Property property = (DI.Property)IdentifyAndDeserializeElement(_diNs, _propertyType);
				current.Property.Add(property);
				return true;
			}
			return false;
		}

		private bool DeserCompElemsIn_DiagramLink(DI.DiagramLink current, string prop)
		{
			return false;
		}
		
		private bool DeserCompElemsIn_Ellipse(DI.Ellipse current, string prop)
		{
			if(object.ReferenceEquals(prop, _center))
			{
				DI.Point center = (DI.Point)IdentifyAndDeserializeElement(_diNs, _pointType);
				current.Center = center;
				return true;
			}
			return false;
		}

		private bool DeserCompElemsIn_GraphConnector(DI.GraphConnector current, string prop)
		{
			if(object.ReferenceEquals(prop, _position))
			{
				current.SetPosition((DI.Point)IdentifyAndDeserializeElement(_diNs, _pointType));
				return true;
			}
			return false;
		}

		private bool DeserCompElemsIn_GraphEdge(DI.GraphEdge current, string prop)
		{
			if(object.ReferenceEquals(prop, _waypoints))
			{
				current.Waypoints.Add(IdentifyAndDeserializeElement(_diNs, _pointType));
				return true;
			}
			return false;
		}

		private bool DeserCompElemsIn_GraphElement(DI.GraphElement current, string prop)
		{
			if(object.ReferenceEquals(prop, _anchorage))
			{
				DI.GraphConnector anchorage = (DI.GraphConnector)IdentifyAndDeserializeElement(_diNs, _graphConnectorType);
				current.Anchorage.Add(anchorage);
				anchorage.GraphElement = current;
				return true;
			}
			else if(object.ReferenceEquals(prop, _contained))
			{
				DI.DiagramElement contained = (DI.DiagramElement)IdentifyAndDeserializeElement(_diNs, _diagramElementType);
				current.Contained.Add(contained);
				contained.Container = current;
				return true;
			}
			else if(object.ReferenceEquals(prop, _link))
			{
				DI.DiagramLink link = (DI.DiagramLink)IdentifyAndDeserializeElement(_diNs, _diagramLinkType);
				current.Link.Add(link);
				link.GraphElement = current;
				return true;
			}
			else if(object.ReferenceEquals(prop, _position))
			{
				DI.Point position = (DI.Point)IdentifyAndDeserializeElement(_diNs, _pointType);
				current.SetPosition(position);
				return true;
			}
			else if(object.ReferenceEquals(prop, _semanticModel))
			{
				DI.SemanticModelBridge semanticModel = (DI.SemanticModelBridge)IdentifyAndDeserializeElement(_diNs, _semanticModelBridgeType);
				current.SemanticModel = semanticModel;
				semanticModel.GraphElement = current;
				return true;
			}
			return false;
		}

		private bool DeserCompElemsIn_GraphNode(DI.GraphNode current, string prop)
		{
			if(object.ReferenceEquals(prop, _size))
			{
				DI.Dimension dimension = (DI.Dimension)IdentifyAndDeserializeElement(_diNs, _dimensionType);
				current.SetSize(dimension); 
				return true;
			}
			return false;
		}

		private bool DeserCompElemsIn_Polyline(DI.Polyline current, string prop)
		{
			if(object.ReferenceEquals(prop, _waypoints))
			{
				current.Waypoints.Add(IdentifyAndDeserializeElement(_diNs, _pointType));
				return true;
			}
			return false;
		}

		protected override void InitCompletion()
		{
			InitDeserialization();
			XmlNameTable nt = base.NT;
			// declare here only primitive and reference properties
			_anchor = nt.Add("anchor");
			_closed = nt.Add("closed");
			_container = nt.Add("container");
			_diagram = nt.Add("diagram");
			_diagramLink = nt.Add("diagramLink");
			_endAngle = nt.Add("endAngle");
			_graphEdge = nt.Add("graphEdge");
			_graphElement = nt.Add("graphElement");
			_height = nt.Add("height");
			_isIndividualRepresentation = nt.Add("isIndividualRepresentation");
			_isVisible = nt.Add("isVisible");
			_mimeType = nt.Add("mimeType");
			_name = nt.Add("name");
			_presentation = nt.Add("presentation");
			_rotation = nt.Add("rotation");
			_radiusX = nt.Add("radiusX");
			_radiusY = nt.Add("radiusY");
			_reference = nt.Add("reference");
			_referenced = nt.Add("referenced");
			_size = nt.Add("size");
			_startAngle = nt.Add("startAngle");
			_text = nt.Add("text");
			_typeInfo = nt.Add("typeInfo");
			_uri = nt.Add("uri");
			_width = nt.Add("width");
			_x = nt.Add("x");
			_y = nt.Add("y");
			_zoom = nt.Add("zoom");
		}

		protected override void InitDeserialization()
		{
			XmlNameTable nt = base.NT;
			// declare here xmi namespaces, types and composite properties
			_anchorage = nt.Add("anchorage");
			_bezierPointType = nt.Add("BezierPoint");
			_center = nt.Add("center");
			_contained = nt.Add("contained");
			_controls = nt.Add("controls");
			_diagramElementType = nt.Add("DiagramElement");
			_diagramLinkType = nt.Add("DiagramLink");
			_diagramType = nt.Add("Diagram");
			_dimensionType = nt.Add("Dimension");
			_diNs = nt.Add(URI);
			_diPrefix = nt.Add("di");
			_ellipseType = nt.Add("Ellipse");
			_graphConnectorType = nt.Add("GraphConnector");
			_graphEdgeType = nt.Add("GraphEdge");
			_graphElementType = nt.Add("GraphElement");
			_graphNodeType = nt.Add("GraphNode");
			_graphicPrimitiveType = nt.Add("GraphicPrimitive");
			_imageType = nt.Add("Image");
			_key = nt.Add("key");
			_leafElementType = nt.Add("LeafElement");
			_link = nt.Add("link");
			_namespace = nt.Add("namespace");
			_pointType = nt.Add("Point");
			_polylineType = nt.Add("Polyline");
			_position = nt.Add("position");
			_property = nt.Add("property");
			_propertyType = nt.Add("Property");
			_referenceType = nt.Add("Reference");
			_semanticModel = nt.Add("semanticModel");
			_semanticModelBridgeType = nt.Add("SemanticModelBridge");
			_simpleSemanticModelElementType = nt.Add("SimpleSemanticModelElement");
			_size = nt.Add("size");
			_textElementType = nt.Add("TextElement");
			_value = nt.Add("value");
			_viewport = nt.Add("viewport");
			_waypoints = nt.Add("waypoints");
		}

		protected override void InitSerialization()
		{
			InitCompletion();
			XmlNameTable nt = base.NT;
			// declare here xmi namespaces and implementation names
			_bezierPointType = nt.Add("BezierPoint");
			_diagramElementType = nt.Add("DiagramElement");
			_diagramLinkType = nt.Add("DiagramLink");
			_diagramType = nt.Add("Diagram");
			_dimensionType = nt.Add("Dimension");
			_diNs = nt.Add(URI);
			_diPrefix = nt.Add("di");
			_ellipseType = nt.Add("Ellipse");
			_graphConnectorType = nt.Add("GraphConnector");
			_graphEdgeType = nt.Add("GraphEdge");
			_graphElementType = nt.Add("GraphElement");
			_graphNodeType = nt.Add("GraphNode");
			_graphicPrimitiveType = nt.Add("GraphicPrimitive");
			_imageType = nt.Add("Image");
			_implMonoUmlDINs = nt.Add("MonoUML.DI");
			_leafElementType = nt.Add("LeafElement");
			_pointType = nt.Add("Point");
			_polylineType = nt.Add("Polyline");
			_propertyType = nt.Add("Property");
			_referenceType = nt.Add("Reference");
			_semanticModelBridgeType = nt.Add("SemanticModelBridge");
			_simpleSemanticModelElementType = nt.Add("SimpleSemanticModelElement");
			_textElementType = nt.Add("TextElement");
		}
		
		protected override string GetXmlNs(string implNs)
		{
			if(object.ReferenceEquals(implNs, _implMonoUmlDINs))
			{
				return _diNs;
			}
			else
			{
				throw new ApplicationException("Unknown namespace: " + implNs);
			}
		}
		
		protected override string GetXmlPrefix(string implNs)
		{
			if(object.ReferenceEquals(implNs, _implMonoUmlDINs))
			{
				return _diPrefix;
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
			if(!object.ReferenceEquals(implNs, _implMonoUmlDINs))
			{
				throw new ApplicationException("Unknown namespace: " + implNs);
			}
			if(object.ReferenceEquals(implType, _bezierPointType))
			{
				DI.BezierPoint bezierPoint = (DI.BezierPoint)current;
				SerializeAttributesIn_Point(bezierPoint);
				//BezierPoint has no attributes
				//Point has no compositions
				SerializeCompositionsIn_BezierPoint(bezierPoint);
			}
			else if(object.ReferenceEquals(implType, _diagramType))
			{
				DI.Diagram diagram = (DI.Diagram)current;
				SerializeAttributesIn_DiagramElement(diagram);
				//GraphNode has no attributes
				//GraphElement has no attributes
				SerializeAttributesIn_Diagram(diagram);
				SerializeCompositionsIn_DiagramElement(diagram);
				SerializeCompositionsIn_GraphNode(diagram);
				SerializeCompositionsIn_GraphElement(diagram);
				SerializeCompositionsIn_Diagram(diagram);
			}
			else if(object.ReferenceEquals(implType, _diagramLinkType))
			{
				DI.DiagramLink diagramLink = (DI.DiagramLink)current;
				SerializeAttributesIn_DiagramLink(diagramLink);
				SerializeCompositionsIn_DiagramLink(diagramLink);
			}
			else if(object.ReferenceEquals(implType, _ellipseType))
			{
				DI.Ellipse ellipse = (DI.Ellipse)current;
				SerializeAttributesIn_DiagramElement(ellipse);
				SerializeAttributesIn_Ellipse(ellipse);
				SerializeCompositionsIn_DiagramElement(ellipse);
				SerializeCompositionsIn_Ellipse(ellipse);
			}
			else if(object.ReferenceEquals(implType, _dimensionType))
			{
				DI.Dimension dimension = (DI.Dimension)current;
				SerializeAttributesIn_Dimension(dimension);
			}
			else if(object.ReferenceEquals(implType, _graphConnectorType))
			{
				DI.GraphConnector graphConnector = (DI.GraphConnector)current;
				SerializeAttributesIn_GraphConnector(graphConnector);
				SerializeCompositionsIn_GraphConnector(graphConnector);
			}
			else if(object.ReferenceEquals(implType, _graphEdgeType))
			{
				DI.GraphEdge graphEdge = (DI.GraphEdge)current;
				SerializeAttributesIn_DiagramElement(graphEdge);
				SerializeAttributesIn_GraphEdge(graphEdge);
				SerializeCompositionsIn_DiagramElement(graphEdge);
				SerializeCompositionsIn_GraphElement(graphEdge);
				SerializeCompositionsIn_GraphEdge(graphEdge);
			}
			else if(object.ReferenceEquals(implType, _graphNodeType))
			{
				DI.GraphNode graphNode = (DI.GraphNode)current;
				SerializeAttributesIn_DiagramElement(graphNode);
				//GraphElement has no attributes
				//GraphNode has no attributes
				SerializeCompositionsIn_DiagramElement(graphNode);
				SerializeCompositionsIn_GraphElement(graphNode);
				SerializeCompositionsIn_GraphNode(graphNode);
			}
			else if(object.ReferenceEquals(implType, _imageType))
			{
				DI.Image image = (DI.Image)current;
				SerializeAttributesIn_DiagramElement(image);
				SerializeAttributesIn_Image(image);
				SerializeCompositionsIn_DiagramElement(image);
				//Image has no compositions
			}
			else if(object.ReferenceEquals(implType, _pointType))
			{
				DI.Point point = (DI.Point)current;
				SerializeAttributesIn_Point(point);
				//Point has no compositions
			}
			else if(object.ReferenceEquals(implType, _polylineType))
			{
				DI.Polyline polyline = (DI.Polyline)current;
				SerializeAttributesIn_DiagramElement(polyline);
				SerializeAttributesIn_Polyline(polyline);
				SerializeCompositionsIn_DiagramElement(polyline);
				SerializeCompositionsIn_Polyline(polyline);
			}
			else if(object.ReferenceEquals(implType, _propertyType))
			{
				DI.Property property = (DI.Property)current;
				SerializeAttributesIn_Property(property);
				//Property has no compositions
			}
			else if(object.ReferenceEquals(implType, _referenceType))
			{
				DI.Reference reference = (DI.Reference)current;
				SerializeAttributesIn_Reference(reference);
				//Reference has no compositions
			}
			else if(object.ReferenceEquals(implType, _simpleSemanticModelElementType))
			{
				DI.SimpleSemanticModelElement ssmElement = (DI.SimpleSemanticModelElement)current;
				SerializeAttributesIn_SemanticModelBridge(ssmElement);
				SerializeAttributesIn_SimpleSemanticModelElement(ssmElement);
				//SimpleSemanticModelElement has no compositions
			}
			else if(object.ReferenceEquals(implType, _textElementType))
			{
				DI.TextElement textElement = (DI.TextElement)current;
				SerializeAttributesIn_DiagramElement(textElement);
				SerializeAttributesIn_TextElement(textElement);
				SerializeCompositionsIn_DiagramElement(textElement);
				//TextElement has no compositions
			}
			else
			{
				throw new ApplicationException("Unknown object type: " + current.ToString());
			}
			return;
		}

		// ------------------------ serialize attributes

		private void SerializeAttributesIn_Diagram(DI.Diagram current)
		{
			base.WritePropertyValue(_name, current.Name);
			base.WritePropertyValue(_zoom, current.Zoom, 1D);
			base.WritePropertyValue(_diagramLink, ConcatenateIds(current.DiagramLink));
		}

		private void SerializeAttributesIn_DiagramElement(DI.DiagramElement current)
		{
			base.WritePropertyValue(_isVisible, current.IsVisible);
		}
		
		private void SerializeAttributesIn_DiagramLink(DI.DiagramLink current)
		{
			base.WritePropertyValue(_zoom, current.Zoom);
		}

		private void SerializeAttributesIn_Dimension(DI.Dimension current)
		{
			base.WritePropertyValue(_height, current.Height);
			base.WritePropertyValue(_width, current.Width);
		}

		private void SerializeAttributesIn_Ellipse(DI.Ellipse current)
		{
			base.WritePropertyValue(_endAngle, current.EndAngle);
			base.WritePropertyValue(_radiusX, current.RadiusX);
			base.WritePropertyValue(_radiusY, current.RadiusY);
			base.WritePropertyValue(_rotation, current.Rotation);
			base.WritePropertyValue(_startAngle, current.StartAngle);
		}

		private void SerializeAttributesIn_GraphConnector(DI.GraphConnector current)
		{
			base.WritePropertyValue(_graphEdge, ConcatenateIds(current.GraphEdge));
		}
		
		private void SerializeAttributesIn_GraphEdge(DI.GraphEdge current)
		{
			base.WritePropertyValue(_anchor, ConcatenateIds(current.Anchor));
		}

		private void SerializeAttributesIn_Image(DI.Image current)
		{
			base.WritePropertyValue(_mimeType, current.MimeType);
			base.WritePropertyValue(_uri, current.Uri);
		}
		
		private void SerializeAttributesIn_Point(DI.Point current)
		{
			base.WritePropertyValue(_x, current.X);
			base.WritePropertyValue(_y, current.Y);
		}
		
		private void SerializeAttributesIn_Polyline(DI.Polyline current)
		{
			base.WritePropertyValue(_closed, current.Closed);
		}

		private void SerializeAttributesIn_Property(DI.Property current)
		{
			base.WritePropertyValue(_key, current.Key);
			base.WritePropertyValue(_value, current.Value);
		}

		private void SerializeAttributesIn_Reference(DI.Reference current)
		{
			base.WritePropertyValue(_isIndividualRepresentation, current.IsIndividualRepresentation);
			base.WritePropertyValue(_referenced, base.GetIdAndEnqueue(current.Referenced));
		}

		private void SerializeAttributesIn_SemanticModelBridge(DI.SemanticModelBridge current)
		{
			base.WritePropertyValue(_diagram, base.GetIdAndEnqueue(current.Diagram));
			base.WritePropertyValue(_graphElement, base.GetIdAndEnqueue(current.GraphElement));
			base.WritePropertyValue(_presentation, current.Presentation);
		}

		private void SerializeAttributesIn_SimpleSemanticModelElement(DI.SimpleSemanticModelElement current)
		{
			base.WritePropertyValue(_typeInfo, current.TypeInfo);
		}

		private void SerializeAttributesIn_TextElement(DI.TextElement current)
		{
			base.WritePropertyValue(_text, current.Text);
		}

		// ------------------------ serialize compositions

		private void SerializeCompositionsIn_BezierPoint(DI.BezierPoint current)
		{
			SerializeCollection(current.Controls, _controls, _pointType);
		}
		
		private void SerializeCompositionsIn_Diagram(DI.Diagram current)
		{
			Serialize(current.Namespace, false, _namespace, _semanticModelBridgeType);
			Serialize(current.Viewport, false, _viewport, _pointType, false);
		}
		
		private void SerializeCompositionsIn_DiagramElement(DI.DiagramElement current)
		{
			SerializeCollection(current.Property, _property, _propertyType, false);
		}

		private void SerializeCompositionsIn_DiagramLink(DI.DiagramLink current)
		{
			Serialize(current.Viewport, false, _viewport, _pointType, false);
		}

		private void SerializeCompositionsIn_Ellipse(DI.Ellipse current)
		{
			Serialize(current.Center, false, _center, _pointType, false);
		}

		private void SerializeCompositionsIn_GraphConnector(DI.GraphConnector current)
		{
			Serialize(current.Position, false, _position, _pointType, false);
		}
		
		private void SerializeCompositionsIn_GraphEdge(DI.GraphEdge current)
		{
			SerializeCollection(current.Waypoints, _waypoints, _pointType, false);
		}
		
		private void SerializeCompositionsIn_GraphElement(DI.GraphElement current)
		{
			SerializeCollection(current.Anchorage, _anchorage, _graphConnectorType);
			SerializeCollection(current.Contained, _contained, _diagramElementType);
			SerializeCollection(current.Link, _link, _diagramLinkType);
			Serialize(current.SemanticModel, false, _semanticModel, _semanticModelBridgeType);
			if(current.Position.X != 0 || current.Position.Y != 0)
			{
				Serialize(current.Position, false, _position, _pointType, false);
			}
		}

		private void SerializeCompositionsIn_GraphNode(DI.GraphNode current)
		{
			Serialize(current.Size, false, _size, _dimensionType, false);
		}
		
		private void SerializeCompositionsIn_Polyline(DI.Polyline current)
		{
			SerializeCollection(current.Waypoints, _waypoints, _pointType);
		}
		
		private string _anchor;
		private string _anchorage;
		private string _bezierPointType;
		private string _center;
		private string _closed;
		private string _contained;
		private string _container;
		private string _controls;
		private string _diagram;
		private string _diagramElementType;
		private string _diagramLink;
		private string _diagramLinkType;
		private string _diagramType;
		private string _dimensionType;
		private string _diNs;
		private string _diPrefix;
		private string _ellipseType;
		private string _endAngle;
		private string _graphConnectorType;
		private string _graphEdge;
		private string _graphEdgeType;
		private string _graphElement;
		private string _graphElementType;
		private string _graphNodeType;
		private string _graphicPrimitiveType;
		private string _height;
		private string _imageType;
		private string _implMonoUmlDINs;
		private string _isIndividualRepresentation;
		private string _isVisible;
		private string _key;
		private string _leafElementType;
		private string _link;
		private string _mimeType;
		private string _name;
		private string _namespace;
		private string _pointType;
		private string _polylineType;
		private string _position;
		private string _presentation;
		private string _property;
		private string _propertyType;
		private string _rotation;
		private string _radiusX;
		private string _radiusY;
		private string _reference;
		private string _referenced;
		private string _referenceType;
		private string _semanticModel;
		private string _semanticModelBridgeType;
		private string _simpleSemanticModelElementType;
		private string _size;
		private string _startAngle;
		private string _text;
		private string _textElementType;
		private string _typeInfo;
		private string _uri;
		private string _value;
		private string _viewport;
		private string _waypoints;
		private string _width;
		private string _x;
		private string _y;
		private string _zoom;
		private const string URI = "org.omg.xmi.namespace.UML"; 
	}
}
