# MonoUML. CASE tool
# http://www.monouml.org
DESTDIR=

SUBDIRS=\
	. \
	Uml2Bridge

MONOUML_DLL_DI_SOURCES=\
	AssemblyInfo.cs \
	BezierPoint.cs \
	Diagram.cs \
	DiagramElement.cs \
	DiagramLink.cs \
	Dimension.cs \
	Ellipse.cs \
	GraphConnector.cs \
	GraphEdge.cs \
	GraphElement.cs \
	GraphNode.cs \
	GraphicPrimitive.cs \
	IPoint.cs \
	ISemanticBridge.cs \
	Image.cs \
	LeafElement.cs \
	Point.cs \
	Polyline.cs \
	Property.cs \
	PropertyMap.cs \
	Reference.cs \
	SemanticModelBridge.cs \
	SimpleSemanticModelElement.cs \
	StandardProperty.cs \
	TextElement.cs \
	Serialization/Serializer.cs
	
EXTRA_DIST=$(MONOUML_DLL_DI_SOURCES)

all-am: $(MONOUML_DLL_DI)
	@echo 'Done: MonUML.DI.dll'

$(MONOUML_DLL_DI): $(MONOUML_DLL_DI_SOURCES)
	$(MCS) -t:library -r:$(EC_DLL_XMI2) -out:$(MONOUML_DLL_DI) $(MONOUML_DLL_DI_SOURCES)

clean-am:
	rm -f $(MONOUML_DLL_DI)

install-exec-am: all
	$(INSTALL) -m 664 $(MONOUML_DLL_DI) $(DESTDIR)$(prefix)/$(PACKAGE)/;
