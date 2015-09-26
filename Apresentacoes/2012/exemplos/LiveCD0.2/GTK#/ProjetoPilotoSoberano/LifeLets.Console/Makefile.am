CSC = mcs -g

BUILDDIR = $(top_srcdir)/build/bin

TARGET = $(BUILDDIR)/LifeLetsConsole.exe
WRAPPER = lifelets_console

PROJECT_REFERENCES = -r:$(BUILDDIR)/LifeLets.Core.dll

CSFLAGS = -target:exe

CSFILES =                               \
$(srcdir)/Main.cs \
$(srcdir)/AssemblyInfo.cs \
$(srcdir)/Console.cs \
$(srcdir)/ConsoleNovo.cs \
$(srcdir)/ConsoleOptions.cs \
$(srcdir)/ContactsConsole.cs \
$(srcdir)/MenuOption.cs \
$(srcdir)/Navigator.cs \
$(srcdir)/SovereChat.cs

REFERENCES = \
-r:$(top_srcdir)/$(BAMBOO) \
$(PROJECT_REFERENCES)

$(TARGET): $(CSFILES) $(srcdir)/Makefile
	$(CSC) -o $@ $(CSFLAGS) $(REFERENCES) $(CSFILES)
	cp -u $(top_srcdir)/$(BAMBOO) $(BUILDDIR)/

lifeletslibdir = $(pkglibdir)
lifeletslib_DATA = $(TARGET)

EXTRA_DIST = \
	$(CSFILES)
	
CLEANFILES = \
	$(TARGET)