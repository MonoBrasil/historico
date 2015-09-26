# MonoUML. CASE tool
# http://www.monouml.org
DESTDIR=

REVENG_SOURCES=\
	AssemblyClass.cs \
	AssemblyImporter.cs \
	AssemblyInterface.cs \
	AssemblyOperation.cs \
	AssemblyType.cs \
	AssemblyConstructor.cs \
	AssemblyHelper.cs \
	AssemblyInfo.cs \
	AssemblyMethod.cs \
	AssemblyPackage.cs \
	AssemblyDataStructure.cs \
	AssemblyDataType.cs \
	AssemblyField.cs \
	AssemblyObject.cs \
	AssemblyEnumeration.cs \
	AssemblyStruct.cs \
	Wizard.cs

EC_DLL_REFERENCES_BUILD=$(addprefix -r:, $(EC_DLL_REFERENCES))

EXTRA_DIST=$(REVENG_SOURCES)

all: $(MONOUML_DLL_REVENG)
	@echo 'Done: Reverse Engineering libraries'

clean:
	rm -f $(MONOUML_DLL_REVENG) 

$(MONOUML_DLL_REVENG) : $(REVENG_SOURCES)
	$(MCS) -t:library $(MONOUML_LIBS) $(EC_DLL_REFERENCES_BUILD) -out:$(MONOUML_DLL_REVENG) $(REVENG_SOURCES) -r:$(MONOUML_DLL_IL),$(MONOUML_DLL_I18N)

main:
	$(MCS) -t:winexe $(MONOUML_LIBS) $(EC_DLL_REFERENCES_BUILD) -out:$(DLL_DIR)/reveng.exe $(REVENG_SOURCES) -r:$(MONOUML_DLL_IL),$(MONOUML_DLL_I18N) Main.cs

install-exec-am: all
	mkdir -p $(DESTDIR)$(prefix)/$(PACKAGE)/; \
	$(INSTALL) -m 664 $(MONOUML_DLL_REVENG) $(DESTDIR)$(prefix)/$(PACKAGE)/;
