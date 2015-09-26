SUBDIRS = templates images resources src po

@INTLTOOL_DESKTOP_RULE@

desktop_in_files = mcatalog.desktop.in
desktop_files = $(desktop_in_files:.desktop.in=.desktop)

desktopdir = $(datadir)/applications
desktop_DATA = $(desktop_files)

applicationsdir = $(datadir)/application-registry
applications_DATA = mcatalog.applications

schemasdir = $(GCONF_SCHEMA_FILE_DIR)
schemas_in_files = mcatalog.schemas.in
schemas_DATA = $(schemas_in_files:.schemas.in=.schemas)

@INTLTOOL_SCHEMAS_RULE@

if GCONF_SCHEMAS_INSTALL
install-data-local:
	if test -z "$(DESTDIR)" ; then \
	  for p in $(schemas_DATA) ; do \
	    GCONF_CONFIG_SOURCE=$(GCONF_SCHEMA_CONFIG_SOURCE) $(GCONFTOOL) --makefile-install-rule $(srcdir)/$$p ; \
	  done \
	fi
else
install-data-local:
endif

INTLTOOL =                      \
        intltool-extract.in     \
        intltool-merge.in       \
        intltool-update.in

EXTRA_DIST = \
        $(desktop_in_files)     \
        $(desktop_files)        \
        $(schemas_in_files)	\
        $(schemas_DATA)		\
	$(applications_DATA)	\
	$(INTLTOOL)

DISTCLEANFILES = \
        intltool-extract        \
        intltool-merge          \
        intltool-update         \
        $(desktop_files)        \
        $(schemas_DATA)

