# MonoUML. CASE tool
# http://www.monouml.org
DESTDIR=

MONOUML_SOURCES=\
	UML2/delegates.cs \
	UML2/UMLWidget.cs \
	UML2/UMLUseCase.cs \
	UML2/UMLUseCaseDiagram.cs \
	UML2/UMLTriangularEnd.cs \
	UML2/UMLStereotypedEdge.cs \
	UML2/UMLSelector.cs \
	UML2/UMLOpenArrow.cs \
	UML2/UMLObjectDiagram.cs \
	UML2/UMLNode.cs \
	UML2/UMLNamedEdge.cs \
	UML2/UMLInstanceSpecification.cs \
	UML2/UMLInterfaceRealization.cs \
	UML2/UMLGeneralization.cs \
	UML2/UMLEntry.cs \
	UML2/UMLElement.cs \
	UML2/UMLEditableField.cs \
	UML2/UMLEdgeEnd.cs \
	UML2/UMLEdge.cs \
	UML2/UMLDiamond.cs \
	UML2/UMLDiagramType.cs \
	UML2/UMLDiagram.cs \
	UML2/UMLControlPointGroup.cs \
	UML2/UMLControlPoint.cs \
	UML2/UMLClassifier.cs \
	UML2/UMLClassDiagram.cs \
	UML2/UMLClass.cs \
	UML2/UMLCanvas.cs \
	UML2/UMLBox.cs \
	UML2/UMLAssociation.cs \
	UML2/UMLActor.cs \
	UML2/Helper.cs \
	UML2/DeleteRepresentationAction.cs \
	UML2/CreateUseCaseAction.cs \
	UML2/CreateNodeAction.cs \
	UML2/CreateInstanceSpecificationAction.cs \
	UML2/CreateIncludeAction.cs \
	UML2/CreateExtendAction.cs \
	UML2/CreateEdgeAction.cs \
	UML2/CreateClassAction.cs \
	UML2/CreateAssociationAction.cs \
	UML2/CreateActorAction.cs \
	UML2/ActionBase.cs \
	UMLPropertiesTab.cs \
	TypedElementTypeViewer.cs \
	TypeKindChooserDialog.cs \
	Tree.cs \
	SingleStringViewer.cs \
	SingleObjectViewer.cs \
	SingleBooleanViewer.cs \
	RootLevelElementChooserDialog.cs \
	ParameterDirectionKindViewer.cs \
	OwnedTypeViewer.cs \
	OwnedOperationsViewer.cs \
	OwnedElementsViewer.cs \
	OwnedAttributesViewer.cs \
	NoteBookLabel.cs \
	NoteBook.cs \
	NamedElementVisibilityViewer.cs \
	MultiplicityElementViewer.cs \
	MultipleObjectViewer.cs \
	MultiChooserDialog.cs \
	ModelElementTypeViewer.cs \
	MixedElementsViewer.cs \
	InterfaceRealizationContractViewer.cs \
	IncludeViewer.cs \
	IView.cs \
	IBroadcaster.cs \
	Hub.cs \
	Helper.cs \
	GeneralizationsViewer.cs \
	GeneralizationGeneralViewer.cs \
	ExtensionLocationViewer.cs \
	ExtendedCaseViewer.cs \
	ExtendViewer.cs \
	ExtendConditionViewer.cs \
	EventQueue.cs \
	ElementChooserDialog.cs \
	DiagramTypeChooserDialog.cs \
	CommentsViewer.cs \
	CanvasArea.cs \
	AnnotatedElementViewer.cs \
	AggregationKindViewer.cs \
	AdditionViewer.cs \
	Toolbar/ToolbarBase.cs \
	Toolbar/ToolbarUseCase.cs \
	Toolbar/ToolbarClass.cs \
	ChooserDialog.cs

EXTRA_DIST=$(MONOUML_SOURCES) $(MONOUML_RESOURCES)

MONOUML_DLL_REFERENCES_BUILD=$(addprefix -r:, $(MONOUML_DLL_REFERENCES))
EC_DLL_REFERENCES_BUILD=$(addprefix -r:, $(EC_DLL_REFERENCES))

all-am: $(MONOUML_DLL_WIDGETS)
	@echo 'Done: MonoUML.Widgets.dll'

$(MONOUML_DLL_WIDGETS): $(MONOUML_SOURCES) $(MONOUML_RESOURCES)
	$(MCS) -out:$(DLL_DIR)/MonoUML.Widgets.dll -t:library $(MONOUML_SOURCES) $(MONOUML_LIBS) $(MONOUML_DLL_REFERENCES_BUILD) $(EC_DLL_REFERENCES_BUILD) -r:$(MONOUML_DLL_IL),$(MONOUML_DLL_I18N)

clean-am:
	rm -f $(MONOUML_DLL_WIDGETS)

install-exec-am: all-am
	mkdir -p $(DESTDIR)$(prefix)/$(PACKAGE)/; \
	$(INSTALL) -m 664 $(MONOUML_DLL_WIDGETS) $(DESTDIR)$(prefix)/$(PACKAGE)/;
