# MonoUML. CASE tool
# http://www.monouml.org
DESTDIR=

MONOUML_SOURCES=\
	AssemblyInfo.cs \
	GettextCatalog.cs

EXTRA_DIST=$(MONOUML_SOURCES)

all-am: $(MONOUML_DLL_I18N)
	@echo 'Done: MonoUML.I18n.dll'

$(MONOUML_DLL_I18N): $(MONOUML_SOURCES)
	$(MCS) -out:$(MONOUML_DLL_I18N) -t:library $(MONOUML_SOURCES) -r:Mono.Posix.dll

clean-am:
	rm -f $(MONOUML_DLL_I18N)

install-exec-am: all-am
	mkdir -p $(DESTDIR)$(prefix)/$(PACKAGE)/; \
	$(INSTALL) -m 664 $(MONOUML_DLL_I18N) $(DESTDIR)$(prefix)/$(PACKAGE)/;
