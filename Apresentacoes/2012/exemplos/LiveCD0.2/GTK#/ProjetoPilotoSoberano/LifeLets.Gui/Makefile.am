CSC = mcs -g

BUILDDIR = $(top_srcdir)/build/bin

TARGET = $(BUILDDIR)/LifeLetsGTK.exe
WRAPPER = lifelets_gtk

PROJECT_REFERENCES = -r:$(BUILDDIR)/LifeLets.Core.dll

CSFLAGS = -target:exe

CSFILES = \
$(srcdir)/Main.cs \
$(srcdir)/AssemblyInfo.cs \
$(srcdir)/MyNickname.cs \
$(srcdir)/LLMainWindow.cs \
$(srcdir)/SovereChat.cs \
$(srcdir)/MainLLGTK.cs \
$(srcdir)/LLChatWindow.cs \
$(srcdir)/LLChangeName.cs \
$(srcdir)/LLAddContactWindow.cs \
$(srcdir)/LLTrustWindow.cs \
$(srcdir)/LLNavigateWindow.cs \
$(srcdir)/LLCreditsWindow.cs

PKG_REFERENCES = \
glade-sharp-2.0 \
gnome-sharp-2.0

PKG_REFERENCES_BUILD = $(addprefix -pkg:, $(PKG_REFERENCES))

REFERENCES = \
-r:$(top_srcdir)/$(BAMBOO) \
$(PROJECT_REFERENCES)

RESOURCES = \
$(srcdir)/resources/gui.glade \
$(srcdir)/resources/LLChatWindow.glade \
$(srcdir)/resources/llwindowaddcontact.glade \
$(srcdir)/resources/LLChangeName.glade \
$(srcdir)/resources/llTrustWindow.glade \
$(srcdir)/resources/LLNavigation.glade \
$(srcdir)/resources/LLCreditsWindow.glade \
$(srcdir)/resources/logoLifeLets.jpg

RESOURCES_BUILD = $(foreach res,$(RESOURCES), $(addprefix -resource:,$(res)),$(notdir $(res)))

$(TARGET): $(CSFILES) $(RESOURCES) $(srcdir)/Makefile
	mkdir -p $(BUILDDIR)
	$(CSC) -o $@ $(CSFLAGS) $(REFERENCES) $(PKG_REFERENCES_BUILD) $(RESOURCES_BUILD) $(CSFILES)
	cp -u $(top_srcdir)/$(BAMBOO) $(BUILDDIR)/

lifeletslibdir = $(pkglibdir)
lifeletslib_DATA = $(TARGET)
	
EXTRA_DIST = \
	$(CSFILES) \
	$(RESOURCES)

CLEANFILES = \
	$(TARGET)
	
run: $(TARGET)
	mono --debug $(TARGET)
