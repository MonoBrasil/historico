CSC = mcs -g

BUILDDIR = $(top_srcdir)/build/bin

TARGET = $(BUILDDIR)/Test.LifeLets.Core.dll
WRAPPER = test

PROJECT_REFERENCES = -r:$(BUILDDIR)/LifeLets.Core.dll

CSFLAGS = -target:library

CSFILES = $(srcdir)/TestLife.cs

REFERENCES = \
-r:$(top_srcdir)/$(BAMBOO) \
-r:$(top_srcdir)/$(NUNIT) \
$(PROJECT_REFERENCES)
	
$(TARGET): $(CSFILES) $(srcdir)/Makefile
	mkdir -p $(BUILDDIR)
	$(CSC) -o $@ $(CSFLAGS) $(REFERENCES) $(CSFILES)
	cp -u $(top_srcdir)/$(BAMBOO) $(BUILDDIR)/
	cp -u $(top_srcdir)/$(NUNIT) $(BUILDDIR)/

lifeletslibdir = $(pkglibdir)
lifeletslib_DATA = $(TARGET)
	
EXTRA_DIST = \
	$(CSFILES)
	
CLEANFILES = \
	$(TARGET)

test: $(TARGET)
	$(NUNITCONSOLE) /xml=../../TestResult.xml $(TARGET)
