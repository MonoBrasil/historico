CSC = mcs -g

BUILDDIR = $(top_srcdir)/build/bin

TARGET = $(BUILDDIR)/LifeLets.Core.dll
WRAPPER = lifelets_core

CSFLAGS = -target:library           

CSFILES = \
$(srcdir)/Contact.cs \
$(srcdir)/File.cs \
$(srcdir)/Life.cs \
$(srcdir)/PeerNetwork.cs \
$(srcdir)/Photo.cs \
$(srcdir)/NetworkServer.cs \
$(srcdir)/Network.cs \
$(srcdir)/NameService.cs \
$(srcdir)/NetworkBridge.cs

REFERENCES = \
-r:$(top_srcdir)/$(BAMBOO) \
-r:System.Drawing.dll
	
$(TARGET): $(CSFILES) $(srcdir)/Makefile
	mkdir -p $(BUILDDIR)
	$(CSC) -o $@ $(CSFLAGS) $(REFERENCES) $(CSFILES)
	cp -u $(top_srcdir)/$(BAMBOO) $(BUILDDIR)/

lifeletslibdir = $(pkglibdir)
lifeletslib_DATA = $(TARGET)
	
EXTRA_DIST = \
	$(CSFILES)

CLEANFILES = \
	$(TARGET)
