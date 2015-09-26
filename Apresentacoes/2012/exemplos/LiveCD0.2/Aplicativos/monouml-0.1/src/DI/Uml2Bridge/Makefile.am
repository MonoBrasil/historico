# MonoUML. CASE tool
# http://www.monouml.org
DESTDIR=

MONOUML_DLL_BRIDGE_SOURCES=\
	AssemblyInfo.cs \
	Uml2SemanticModelBridge.cs \
	Serialization/Serializer.cs

DLLS_REFERENCES= \
	$(MONOUML_DLL_DI) \
	$(EC_DLL_XMI2) \
	$(EC_DLL_UML2)

DLLS_REFERENCES_BUILD = $(addprefix -r:, $(DLLS_REFERENCES))

EXTRA_DIST=$(MONOUML_DLL_BRIDGE_SOURCES)

all: $(MONOUML_DLL_BRIDGE)
	@echo 'Done: MonUML.Uml2Bridge.dll'

$(MONOUML_DLL_BRIDGE): $(MONOUML_DLL_BRIDGE_SOURCES)
	$(MCS) -t:library $(DLLS_REFERENCES_BUILD) -out:$(MONOUML_DLL_BRIDGE) $(MONOUML_DLL_BRIDGE_SOURCES)

clean:
	rm -f $(MONOUML_DLL_BRIDGE)

install-exec-am: all
	mkdir -p $(DESTDIR)$(prefix)/$(PACKAGE)/; \
	$(INSTALL) -m 664 $(MONOUML_DLL_BRIDGE) $(DESTDIR)$(prefix)/$(PACKAGE)/;
