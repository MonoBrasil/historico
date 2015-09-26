# MonoUML. CASE tool
# http://www.monouml.org
SUBDIRS= src po

EXTRA_DIST= m4/Makefile.in config.rpath mkinstalldirs AUTHORS NEWS README THANKS TODO GPL.txt LGPL.txt

run:
	@if test -a $(DLL_DIR)/monouml.exe; then mono $(DLL_DIR)/monouml.exe; else echo 'You must run make first'; fi
