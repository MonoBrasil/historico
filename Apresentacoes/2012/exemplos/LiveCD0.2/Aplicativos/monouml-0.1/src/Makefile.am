# MonoUML. CASE tool
# http://www.monouml.org
DESTDIR=

SUBDIRS=\
	Preferences \
	I18n \
	IconLibrary \
	DI \
	XmiImporter \
	ReverseEngineering \
	Widgets \
	Generators \
	.

MONOUML_SOURCES=\
	MainWindow.cs \
	CodeGeneration/Wizard.cs \
	GUI/MonoUML.cs \
	GUI/SelectProjectDialog.cs

MONOUML_RESOURCES=\
	GUI/Glade/gui.glade
	
EXTRA_DIST=README $(MONOUML_SOURCES) $(MONOUML_RESOURCES) ../bin/README

MONOUML_RESOURCES_BUILD=$(foreach res,$(MONOUML_RESOURCES), $(addprefix -resource:,$(res)),$(notdir $(res)))
MONOUML_DLL_REFERENCES_BUILD=$(addprefix -r:, $(MONOUML_DLL_REFERENCES))
EC_DLL_REFERENCES_BUILD=$(addprefix -r:, $(EC_DLL_REFERENCES))

all-am: $(EC_DLL_TEMPLATES) $(EC_DLL_PACKAGING) $(EC_DLL_CSHARPGEN) $(EC_DLL_UML2CSHARP) $(EC_DLL_UMLNAVIG) $(EC_DLL_ES) $(DLL_DIR)/monouml.exe
	@echo 'Done: MonoUML'

$(DLL_DIR)/monouml.exe: $(MONOUML_SOURCES) $(MONOUML_RESOURCES)
	$(MCS) -out:$(DLL_DIR)/monouml.exe -t:winexe $(MONOUML_RESOURCES_BUILD) $(MONOUML_LIBS) $(MONOUML_DLL_REFERENCES_BUILD) -r:$(MONOUML_DLL_WIDGETS) -r:$(MONOUML_DLL_PREF) -r:$(MONOUML_DLL_IL) $(EC_DLL_REFERENCES_BUILD) $(MONOUML_SOURCES)

clean-am:
	rm -f $(DLL_DIR)/monouml.exe

install-exec-am: all
	mkdir -p $(DESTDIR)$(prefix)/$(PACKAGE); \
	$(INSTALL) $(DLL_DIR)/monouml.exe $(DESTDIR)$(prefix)/$(PACKAGE)/; \
	echo '#!$(SHELL)' >> .monouml.tmp; \
	echo 'mono $(prefix)/$(PACKAGE)/monouml.exe' >> .monouml.tmp; \
	mkdir -p $(DESTDIR)/usr/bin/; \
	$(INSTALL) .monouml.tmp $(DESTDIR)/usr/bin/monouml; \
	rm .monouml.tmp; \
	mkdir -p $(DESTDIR)$(prefix)/$(PACKAGE)/generators/; \
	$(INSTALL) -m 664 $(EC_DLL_ES) $(DESTDIR)$(prefix)/$(PACKAGE)/generators/; \
	$(INSTALL) -m 664 $(EC_DLL_PACKAGING) $(DESTDIR)$(prefix)/$(PACKAGE)/generators/; \
	$(INSTALL) -m 664 $(EC_DLL_TEMPLATES) $(DESTDIR)$(prefix)/$(PACKAGE)/generators/; \
	$(INSTALL) -m 664 $(EC_DLL_CSHARPGEN) $(DESTDIR)$(prefix)/$(PACKAGE)/generators/; \
	$(INSTALL) -m 664 $(EC_DLL_UML2CSHARP) $(DESTDIR)$(prefix)/$(PACKAGE)/generators/; \
	$(INSTALL) -m 664 $(EC_DLL_UMLNAVIG) $(DESTDIR)$(prefix)/$(PACKAGE)/generators/;
	
install-data-am: install-exec-am
	mkdir -p $(DESTDIR)$(prefix)/$(PACKAGE)/samples; \
	$(INSTALL) -m 664 $(top_builddir)/samples/*.xmi $(DESTDIR)$(prefix)/$(PACKAGE)/samples; \
	$(INSTALL) -m 664 $(top_builddir)/AUTHORS $(DESTDIR)$(prefix)/$(PACKAGE)/; \
	$(INSTALL) -m 664 $(top_builddir)/GPL.txt $(DESTDIR)$(prefix)/$(PACKAGE)/; \
	$(INSTALL) -m 664 $(top_builddir)/LGPL.txt $(DESTDIR)$(prefix)/$(PACKAGE)/; \
	$(INSTALL) -m 664 $(EC_DLL_XMI2) $(DESTDIR)$(prefix)/$(PACKAGE)/; \
	$(INSTALL) -m 664 $(EC_DLL_UML2) $(DESTDIR)$(prefix)/$(PACKAGE)/; \
	$(INSTALL) -m 664 $(EC_DLL_MOF2) $(DESTDIR)$(prefix)/$(PACKAGE)/; \
	$(INSTALL) -m 664 $(EC_DLL_ES) $(DESTDIR)$(prefix)/$(PACKAGE)/;

uninstall-am:
	rm -fr $(DESTDIR)$(prefix)/$(PACKAGE)/;
	rm -f $(DESTDIR)/usr/bin/monouml;
