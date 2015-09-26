# MonoUML. CASE tool
# http://www.monouml.org
DESTDIR=

MONOUML_DLL_PREF_SOURCES=\
	AssemblyInfo.cs \
	Core.cs

EXTRA_DIST=$(MONOUML_DLL_PREF_SOURCES)

all: $(MONOUML_DLL_PREF)
	@echo 'Done: MonoUML.Preferences.dll'

$(MONOUML_DLL_PREF): $(MONOUML_DLL_PREF_SOURCES)
	$(MCS) -t:library $(MONOUML_LIBS) -out:$(MONOUML_DLL_PREF) $(MONOUML_DLL_PREF_SOURCES) -resource:../GUI/Glade/gui.glade,gui.glade

clean:
	rm -f $(MONOUML_DLL_PREF)

install-exec: all
	mkdir -p $(DESTDIR)$(prefix)/$(PACKAGE)/; \
	$(INSTALL) -m 664 $(MONOUML_DLL_PREF) $(DESTDIR)$(prefix)/$(PACKAGE)/;
