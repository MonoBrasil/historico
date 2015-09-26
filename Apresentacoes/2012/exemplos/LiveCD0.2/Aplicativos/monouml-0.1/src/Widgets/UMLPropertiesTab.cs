/*
MonoUML.Widgets - A library for representing the Widget elements
Copyright (C) 2004  Mario Carrión
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
using Gtk;
using System;
using System.Collections;
using UML = ExpertCoder.Uml2; 
using MonoUML.I18n;

namespace MonoUML.Widgets
{
	// The UML Properties Tab goes inside monouml_notebook_properties
	public class UMLPropertiesTab : Gtk.VBox, IBroadcaster, IView
	{
		public UMLPropertiesTab() : base(false, 2)
		{
			// Model Element type
			_metvType = new ModelElementTypeViewer(this);
			base.PackStart(_metvType, false, false, 2);
			// "NamedElement.Name" property
			_ssvName = new SingleStringViewer(this, GettextCatalog.GetString ("Name:"), "Name");
			base.PackStart(_ssvName, false, false, 2);
			// "TypedElement.Type" property
			_sovTEType = new TypedElementTypeViewer(this);
			base.PackStart(_sovTEType, false, false, 2);
			// "InterfaceRealization.Contract" property
			_sovImplContract = new InterfaceRealizationContractViewer(this);
			base.PackStart(_sovImplContract, false, false, 2);
			// "Generalization.General" property
			_sovGeneral = new GeneralizationGeneralViewer(this);
			base.PackStart(_sovGeneral, false, false, 2);
			// "NamedElement.Visibility" property
			_nevNEVisibility = new NamedElementVisibilityViewer(this);
			base.PackStart(_nevNEVisibility, false, false, 2);
			// "Parameter.Direction" property
			_pdKindViewer = new ParameterDirectionKindViewer(this);
			base.PackStart(_pdKindViewer, false, false, 2);
			// "Property.Aggregation" property
			_aKindViewer = new AggregationKindViewer(this);
			base.PackStart(_aKindViewer, false, false, 2);
			// **** boolean properties: Classifier.IsAbstract, IsLeaf, IsStatic,
			// IsReadOnly
			HBox boolProps = new HBox();
			_sbvClassifierIsAbstract = new SingleBooleanViewer(this, GettextCatalog.GetString ("Abstract:"), "IsAbstract"); 
			boolProps.PackStart(_sbvClassifierIsAbstract, false, false, 2);
			_sbvIsDerived = new SingleBooleanViewer(this, GettextCatalog.GetString ("Derived:"), "IsDerived");
			boolProps.PackStart(_sbvIsDerived, false, false, 2);
			_sbvIsLeaf = new SingleBooleanViewer(this, GettextCatalog.GetString ("Leaf:"), "IsLeaf");
			boolProps.PackStart(_sbvIsLeaf, false, false, 2);
			_sbvIsQuery = new SingleBooleanViewer(this, GettextCatalog.GetString ("Query:"), "IsQuery");
			boolProps.PackStart(_sbvIsQuery, false, false, 2);
			_sbvIsReadOnly = new SingleBooleanViewer(this, GettextCatalog.GetString ("Read only:"), "IsReadOnly");
			boolProps.PackStart(_sbvIsReadOnly, false, false, 2);
			_sbvIsStatic = new SingleBooleanViewer(this, GettextCatalog.GetString ("Static:"), "IsStatic");
			boolProps.PackStart(_sbvIsStatic, false, false, 2);
			base.PackStart(boolProps, false, false, 0);
			// MultiplicityElement properties
			_multiplicityElement = new MultiplicityElementViewer(this);
			base.PackStart(_multiplicityElement, false, false, 0);
			// "Body" property
			_ssvBody = new SingleStringViewer(this, "Body:", "Body");
			base.PackStart(_ssvBody, false, false, 2);
			// "Addition" property
			_sobAddition = new AdditionViewer(this);
			base.PackStart(_sobAddition, false, false, 2);
			// "ExtendedCase" property
			_sobExtendedCase = new ExtendedCaseViewer(this);
			base.PackStart(_sobExtendedCase, false, false, 2);
			// "Extend.Condition" property
			_sobExtendCondition = new ExtendConditionViewer(this);
			base.PackStart(_sobExtendCondition, false, false, 2);
			// "ExtensionLocation" property
			_movExtensionLocation = new ExtensionLocationViewer(this);
			base.PackStart(_movExtensionLocation, false, false, 2);
			// "ExtensionPoint" property
			_movExtensionPoint = new OwnedElementsViewer(
				this, GettextCatalog.GetString ("Extension points:"), "ExtensionPoint", "UseCase", "ExtensionPoint");
			base.PackStart(_movExtensionPoint, false, false, 2);
			// "Extend" and "Include" properties
			HBox extInc = new HBox();
			_movExtend = new ExtendViewer(this);
			extInc.PackStart(_movExtend, true, true, 2);
			_movInclude = new IncludeViewer(this);
			extInc.PackStart(_movInclude, true, true, 2);
			base.PackStart(extInc, false, false, 0);
			// "AnnotatedElement" property
			_movAnnotatedElement = new AnnotatedElementViewer(this);
			base.PackStart(_movAnnotatedElement, false, false, 2);
			// "Literal" property
			_movLiterals = new OwnedElementsViewer(
				this, GettextCatalog.GetString ("Owned literals:"), "OwnedLiteral", "Enumeration", "EnumerationLiteral");
			base.PackStart(_movLiterals, false, false, 2);
			// "OwnedParameter" property
			_movOwnedParameter = new OwnedElementsViewer(
				this, GettextCatalog.GetString ("Owned parameters:"), "OwnedParameter", "Operation", "Parameter");
			base.PackStart(_movOwnedParameter, false, false, 2);
			// "NestedPackage" and "OwnedType" properties
			HBox nesown = new HBox();
			_movNestedPackage = new OwnedElementsViewer(
				this, GettextCatalog.GetString ("Nested packages:"), "NestedPackage", "NestingPackage", "Package");
			nesown.PackStart(_movNestedPackage, true, true, 2);
			_movOwnedType = new OwnedTypeViewer(this);
			nesown.PackStart(_movOwnedType, true, true, 2);
			base.PackStart(nesown, false, false, 0);
			// "OwnedAttribute" and "OwnedOperation" properties
			HBox attope = new HBox();
			_movOwnedAttribute = new OwnedAttributesViewer(this);
			attope.PackStart(_movOwnedAttribute, true, true, 2);
			_movOwnedEnd = new OwnedAttributesViewer(this, GettextCatalog.GetString ("Owned ends:"));
			attope.PackStart(_movOwnedEnd, true, true, 2);
			_movMemberEnd = new MixedElementsViewer(
				this, GettextCatalog.GetString ("Member ends:"), "MemberEnd", "Association", "Property");
			attope.PackStart(_movMemberEnd, true, true, 2);
			_movOwnedOperation = new OwnedOperationsViewer(this);
			attope.PackStart(_movOwnedOperation, true, true, 2);
			base.PackStart(attope, false, false, 0);
			// "Classifier.Generalization" and "BehavioredClassifier.InterfaceRealization" properties
			HBox genint = new HBox();
			_movGeneralizations = new GeneralizationsViewer(this);
			genint.PackStart(_movGeneralizations, true, true, 2);
			_movInterfaceRealizations = new OwnedElementsViewer(
				this, GettextCatalog.GetString ("Interface Realizations:"), "InterfaceRealization", "ImplementingClassifier", "InterfaceRealization");
			genint.PackStart(_movInterfaceRealizations, true, true, 2);
			base.PackStart(genint, false, false, 0);
			// "PackageImport" and "ElementImport" properties
			HBox peImports = new HBox();
			_movPackageImports = new OwnedElementsViewer(
				this, GettextCatalog.GetString ("Package imports:"), "PackageImport", "ImportingNamespace", "PackageImport");
			peImports.PackStart(_movPackageImports, true, true, 2);
			_movElementImports = new OwnedElementsViewer(
				this, GettextCatalog.GetString ("Element imports:"), "ElementImport", "ImportingNamespace", "ElementImport");
			peImports.PackStart(_movElementImports, true, true, 2);
			base.PackStart(peImports, false, false, 0);
			// "OwnedUseCase" property
			_movOwnedUseCase = new OwnedElementsViewer(
				this, GettextCatalog.GetString ("Owned use cases:"), "OwnedUseCase", null, "UseCase");
			base.PackStart(_movOwnedUseCase, false, false, 2);
			// Element properties
			_movComments = new CommentsViewer(this);
			base.PackStart(_movComments, false, false, 2);
			// FIXME: Check why this doesn't work (all the widgets are shown) 
			HideAllProperties();
		}

		public UMLPropertiesTab(IntPtr raw) : base(raw) {}
		
		#region IView implementation
		void IView.Clear()
		{
			ShowPropertiesFor(null);
		}
		
		void IView.SelectElement(object selectedElement)
		{
			ShowPropertiesFor(GetUmlElement(selectedElement));
		}

		void IView.UpdateElement(object modifiedElement)
		{
			//System.Console.WriteLine ("AUIQ->"+GetUmlElement(modifiedElement));
			ShowPropertiesFor(GetUmlElement(modifiedElement));
		}

		void IView.SetBroadcaster(IBroadcaster hub)
		{
			_hub = hub;
		}

		void IView.SetModel(System.Collections.IList modelElements)
		{
			HideAllProperties();
		}
		#endregion
		
		#region IBroadcaster implementation
		void IBroadcaster.BroadcastElementChange(object modifiedElement)
		{
			_hub.BroadcastElementChange(modifiedElement);
		}

		void IBroadcaster.BroadcastElementSelection(object element)
		{
			_hub.BroadcastElementSelection(element);
		}
		#endregion

		private void EnableAssociationProperties()
		{
			UML.Association assoc = (UML.Association)_currentElement;
			_sbvIsDerived.Visible = true;
			_sbvIsDerived.ShowPropertyValueFor(assoc);
			if(!(_currentElement is UML.Extension))
			{
				_movOwnedEnd.Visible = true;
				_movOwnedEnd.ShowOwnedAttributesFor(assoc);
			}
			_movMemberEnd.Visible = true;
			_movMemberEnd.ShowMixedElementsFor(assoc);
		}
		
		private void EnableBehavioredClassifierProperties()
		{
			UML.BehavioredClassifier behavioredClassifier = 
				(UML.BehavioredClassifier)_currentElement;
			_movInterfaceRealizations.Visible = true;
			_movInterfaceRealizations.ShowOwnedElementsFor(behavioredClassifier);
		}

		private void EnableClassProperties()
		{
			_movOwnedAttribute.Visible = true;
			_movOwnedAttribute.ShowOwnedAttributesFor((UML.Classifier)_currentElement);
			_movOwnedOperation.Visible = true;
			_movOwnedOperation.ShowOwnedOperationsFor((UML.Classifier)_currentElement);
		}

		private void EnableClassifierProperties()
		{
			UML.Classifier classifier = (UML.Classifier)_currentElement;
			_movGeneralizations.Visible = true;
			_movGeneralizations.ShowGeneralizationsFor(classifier);
			_movOwnedUseCase.Visible = true;
			_movOwnedUseCase.ShowOwnedElementsFor(classifier);
			_sbvClassifierIsAbstract.Visible = true;
			_sbvClassifierIsAbstract.ShowPropertyValueFor(classifier);
		}

		private void EnableCommentProperties()
		{
			UML.Comment comment = (UML.Comment)_currentElement;
			_ssvBody.Visible = true;
			_ssvBody.ShowPropertyValueFor(comment);
			_movAnnotatedElement.Visible = true;
			_movAnnotatedElement.ShowAnnotatedElementsFor(comment);
		}

		private void EnableDataTypeProperties()
		{
			_movOwnedAttribute.Visible = true;
			_movOwnedAttribute.ShowOwnedAttributesFor((UML.Classifier)_currentElement);
			_movOwnedOperation.Visible = true;
			_movOwnedOperation.ShowOwnedOperationsFor((UML.Classifier)_currentElement);
		}

		private void EnableEnumerationProperties()
		{
			_movLiterals.Visible = true;
			_movLiterals.ShowOwnedElementsFor(_currentElement);
		}

		private void EnableExtendProperties()
		{
			UML.Extend extend = (UML.Extend)_currentElement;
			_sobExtendedCase.Visible = true;
			_sobExtendedCase.ShowExtendedCaseFor(extend);
			_sobExtendCondition.Visible = true;
			_sobExtendCondition.ShowConditionFor(extend);
			_movExtensionLocation.Visible = true;
			_movExtensionLocation.ShowExtensionLocationFor(extend);
		}

		private void EnableElementProperties()
		{
			_metvType.Visible = true;
			_metvType.ShowElementTypeFor(_currentElement);
			_movComments.Visible = true;
			_movComments.ShowCommentsFor(_currentElement);
		}
		
		private void EnableFeatureProperties()
		{
			_sbvIsStatic.Visible = true;
			_sbvIsStatic.ShowPropertyValueFor((UML.Feature)_currentElement);
		}
		
		private void EnableGeneralizationProperties()
		{
			_sovGeneral.Visible = true;
			_sovGeneral.ShowGeneralFor((UML.Generalization)_currentElement);
		}
		
		private void EnableInterfaceRealizationProperties()
		{
			_sovImplContract.Visible = true;
			_sovImplContract.ShowContractFor((UML.InterfaceRealization)_currentElement);
		}

		private void EnableIncludeProperties()
		{
			_sobAddition.Visible = true;
			_sobAddition.ShowAdditionFor((UML.Include)_currentElement);
		}

		private void EnableInterfaceProperties()
		{
			_movOwnedAttribute.Visible = true;
			_movOwnedAttribute.ShowOwnedAttributesFor((UML.Classifier)_currentElement);
			_movOwnedOperation.Visible = true;
			_movOwnedOperation.ShowOwnedOperationsFor((UML.Classifier)_currentElement);
		}

		private void EnableMultiplicityElementProperties()
		{
			_multiplicityElement.Visible = true;
			_multiplicityElement.ShowPropertiesFor((UML.MultiplicityElement)_currentElement);
		}

		private void EnableNamedElementProperties()
		{
			UML.NamedElement namedElement = (UML.NamedElement)_currentElement; 
			_ssvName.Visible = true;
			_ssvName.ShowPropertyValueFor(namedElement);
			_nevNEVisibility.Visible = true;
			_nevNEVisibility.ShowVisibilityFor(namedElement);
		}
		
		private void EnableNamespaceProperties()
		{
			UML.Namespace ns = (UML.Namespace)_currentElement; 
			_movElementImports.Visible = true;
			_movElementImports.ShowOwnedElementsFor(ns);
			_movPackageImports.Visible = true;
			_movPackageImports.ShowOwnedElementsFor(ns);
		}
		
		private void EnableOperationProperties()
		{
			UML.Operation op = (UML.Operation)_currentElement;
			_sbvIsQuery.Visible = true;
			_sbvIsQuery.ShowPropertyValueFor(op);
			_movOwnedParameter.Visible = true;
			_movOwnedParameter.ShowOwnedElementsFor(op);
		}

		private void EnablePackageProperties()
		{
			UML.Package package = (UML.Package)_currentElement;
			_movNestedPackage.Visible = true;
			_movNestedPackage.ShowOwnedElementsFor(package);
			_movOwnedType.Visible = true;
			_movOwnedType.ShowOwnedTypeFor(package);
		}

		private void EnableParameterProperties()
		{
			UML.Parameter parameter = (UML.Parameter)_currentElement;
			_pdKindViewer.Visible = true;
			_pdKindViewer.ShowDirectionFor(parameter);
		}
		
		private void EnablePropertyProperties()
		{
			UML.Property property = (UML.Property)_currentElement;
			_aKindViewer.Visible = true;
			_aKindViewer.ShowAggregationFor(property);
		}
		
		private void EnableRedefinableElementProperties()
		{
			UML.RedefinableElement redefElem
				= (UML.RedefinableElement)_currentElement;
			_sbvIsLeaf.Visible = true;
			_sbvIsLeaf.ShowPropertyValueFor(redefElem);
		}

		private void EnableStructuralFeatureProperties()
		{
			_sbvIsReadOnly.Visible = true;
			_sbvIsReadOnly.ShowPropertyValueFor((UML.StructuralFeature)_currentElement);
		}

		private void EnableTypedElementProperties()
		{
			UML.TypedElement typedElement
				= (UML.TypedElement)_currentElement;
			_sovTEType.Visible = true;
			_sovTEType.ShowTypeFor(typedElement);
		}

		private void EnableUseCaseProperties()
		{
			UML.UseCase useCase = (UML.UseCase)_currentElement;
			_movExtend.Visible = true;
			_movExtend.ShowExtendFor(useCase);
			_movExtensionPoint.Visible = true;
			_movExtensionPoint.ShowOwnedElementsFor(useCase);
			_movInclude.Visible = true;
			_movInclude.ShowIncludeFor(useCase);
		}

		private UML.Element GetUmlElement(object element)
		{
			UML.Element umlElement = element as UML.Element;
			MonoUML.DI.GraphElement graphElement;
			if(umlElement == null) 
			{
				if( (graphElement = element as MonoUML.DI.GraphElement) != null)
				{
					umlElement = Helper.GetSemanticElement(graphElement);
				}
			}
			return umlElement;
		}
		
		private void HideAllProperties()
		{
			_aKindViewer.Hide();
			_metvType.Hide();
			_multiplicityElement.Hide();
			_movAnnotatedElement.Hide();
			_movComments.Hide();
			_movElementImports.Hide();
			_movExtend.Hide();
			_movExtensionLocation.Hide();
			_movExtensionPoint.Hide();
			_movGeneralizations.Hide();
			_movInclude.Hide();
			_movInterfaceRealizations.Hide();
			_movLiterals.Hide();
			_movOwnedParameter.Hide();
			_movMemberEnd.Hide();
			_movNestedPackage.Hide();
			_movOwnedAttribute.Hide();
			_movOwnedEnd.Hide();
			_movOwnedOperation.Hide();
			_movOwnedType.Hide();
			_movOwnedUseCase.Hide();
			_movPackageImports.Hide();
			_nevNEVisibility.Hide();
			_pdKindViewer.Hide();
			_sbvClassifierIsAbstract.Hide();
			_sbvIsDerived.Hide();
			_sbvIsLeaf.Hide();
			_sbvIsQuery.Hide();
			_sbvIsReadOnly.Hide();
			_sbvIsStatic.Hide();
			_sobAddition.Hide();
			_sobExtendCondition.Hide();
			_sobExtendedCase.Hide();
			_sovGeneral.Hide();
			_sovImplContract.Hide();
			_sovTEType.Hide();
			_ssvBody.Hide();
			_ssvName.Hide();
		}

		private void ShowPropertiesFor(UML.Element element)
		{
			_currentElement = element;
			HideAllProperties();
			if (element == null)
			{
				return;
			}
			EnableElementProperties();
			if(element is UML.Association)
			{
				EnableAssociationProperties();
			}
			if(element is UML.BehavioredClassifier)
			{
				EnableBehavioredClassifierProperties();
			}
			if(element is UML.Classifier)
			{
				EnableClassifierProperties();
			}
			// this comes after Classifier because it may hide some viewers
			// (those corresponding to redefined attributes).
			if(element is UML.Class)
			{
				EnableClassProperties();
			}
			if(element is UML.Comment)
			{
				EnableCommentProperties();
			}
			if(element is UML.DataType)
			{
				EnableDataTypeProperties();
			}
			if(element is UML.Enumeration)
			{
				EnableEnumerationProperties();
			}
			if(element is UML.Extend)
			{
				EnableExtendProperties();
			}
			if(element is UML.Feature)
			{
				EnableFeatureProperties();
			}
			if(element is UML.Generalization)
			{
				EnableGeneralizationProperties();
			}
			if(element is UML.InterfaceRealization)
			{
				EnableInterfaceRealizationProperties();
			}
			if(element is UML.Include)
			{
				EnableIncludeProperties();
			}
			if(element is UML.Interface)
			{
				EnableInterfaceProperties();
			}
			if(element is UML.MultiplicityElement)
			{
				EnableMultiplicityElementProperties();
			}
			if(element is UML.NamedElement)
			{
				EnableNamedElementProperties();
			}
			if(element is UML.Namespace)
			{
				EnableNamespaceProperties();
			}
			if(element is UML.Operation)
			{
				EnableOperationProperties();
			}
			if(element is UML.Package)
			{
				EnablePackageProperties();
			}
			if(element is UML.Parameter)
			{
				EnableParameterProperties();
			}
			if(element is UML.Property)
			{
				EnablePropertyProperties();
			}
			if(element is UML.RedefinableElement)
			{
				EnableRedefinableElementProperties();
			}
			if(element is UML.StructuralFeature)
			{
				EnableStructuralFeatureProperties();
			}
			if(element is UML.TypedElement)
			{
				EnableTypedElementProperties();
			}
			if(element is UML.UseCase)
			{
				EnableUseCaseProperties();
			}
		}

		private UML.Element _currentElement;
		private IBroadcaster _hub;
		private AggregationKindViewer _aKindViewer;
		private ModelElementTypeViewer _metvType;
		private MultiplicityElementViewer _multiplicityElement;
		private AnnotatedElementViewer _movAnnotatedElement;
		private ExtendViewer _movExtend;
		private ExtensionLocationViewer _movExtensionLocation;
		private OwnedElementsViewer _movExtensionPoint;
		private IncludeViewer _movInclude;
		private CommentsViewer _movComments;
		private OwnedElementsViewer _movElementImports;
		private GeneralizationsViewer _movGeneralizations;
		private OwnedElementsViewer _movInterfaceRealizations;
		private OwnedElementsViewer _movLiterals;
		private MixedElementsViewer _movMemberEnd;
		private OwnedElementsViewer _movNestedPackage;
		private OwnedAttributesViewer _movOwnedAttribute;
		private OwnedAttributesViewer _movOwnedEnd;
		private OwnedElementsViewer _movOwnedParameter;
		private OwnedOperationsViewer _movOwnedOperation;
		private OwnedTypeViewer _movOwnedType;
		private OwnedElementsViewer _movOwnedUseCase;
		private OwnedElementsViewer _movPackageImports;
		private NamedElementVisibilityViewer _nevNEVisibility;
		private ParameterDirectionKindViewer _pdKindViewer;
		private SingleBooleanViewer _sbvClassifierIsAbstract;
		private SingleBooleanViewer _sbvIsDerived;
		private SingleBooleanViewer _sbvIsLeaf;
		private SingleBooleanViewer _sbvIsQuery;
		private SingleBooleanViewer _sbvIsReadOnly;
		private SingleBooleanViewer _sbvIsStatic;
		private ExtendConditionViewer _sobExtendCondition;
		private AdditionViewer _sobAddition;
		private ExtendedCaseViewer _sobExtendedCase;
		private InterfaceRealizationContractViewer _sovImplContract;
		private GeneralizationGeneralViewer _sovGeneral;
		private TypedElementTypeViewer _sovTEType;
		private SingleStringViewer _ssvBody;
		private SingleStringViewer _ssvName;
	}
}
