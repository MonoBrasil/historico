# MonoUML. CASE tool
# http://www.monouml.org
DESTDIR=

MONOUML_DLL_IL_SOURCES=\
	AssemblyInfo.cs \
	PixbufLoader.cs

MONOUML_RESOURCES=\
	Pixmaps/usecase_tree.png \
	Pixmaps/usecasediagram_tree.png \
	Pixmaps/unknown_tree.png \
	Pixmaps/snap2grid_tbar.png \
	Pixmaps/property_tree.png \
	Pixmaps/profile_tree.png \
	Pixmaps/primitivetype_tree.png \
	Pixmaps/parameter_tree.png \
	Pixmaps/package_tree.png \
	Pixmaps/operation_tree.png \
	Pixmaps/model_tree.png \
	Pixmaps/main_icon.png \
	Pixmaps/interface_tree.png \
	Pixmaps/interfacerealization_tree.png \
	Pixmaps/include_tree.png \
	Pixmaps/grid_tbar.png \
	Pixmaps/generalization_tree.png \
	Pixmaps/extensionpoint_tree.png \
	Pixmaps/extend_tree.png \
	Pixmaps/enumeration_tree.png \
	Pixmaps/enumerationliteral_tree.png \
	Pixmaps/datatype_tree.png \
	Pixmaps/comment_tree.png \
	Pixmaps/class_tree.png \
	Pixmaps/classdiagram_tree.png \
	Pixmaps/association_tree.png \
	Pixmaps/associationend_tree.png \
	Pixmaps/actor_tree.png \
	Pixmaps/about.png \
	Pixmaps/actor_dnd.png \
	Pixmaps/unknown_dnd.png \
	Pixmaps/usecase_dnd.png \
	Pixmaps/no_dnd.png

MONOUML_RESOURCES_BUILD=$(foreach res,$(MONOUML_RESOURCES), $(addprefix -resource:,$(res)),$(notdir $(res)))

EXTRA_DIST=$(MONOUML_DLL_IL_SOURCES) $(MONOUML_RESOURCES)

all-am: $(MONOUML_DLL_IL)
	@echo 'Done: MonoUML.IconLibrary.dll'

$(MONOUML_DLL_IL): $(MONOUML_DLL_IL_SOURCES)
	$(MCS) -t:library $(MONOUML_LIBS) -out:$(MONOUML_DLL_IL) $(MONOUML_DLL_IL_SOURCES) $(MONOUML_RESOURCES_BUILD)

clean-am:
	rm -f $(MONOUML_DLL_IL)

install-exec-am: all-am
	mkdir -p $(DESTDIR)$(prefix)/$(PACKAGE)/; \
	$(INSTALL) -m 664 $(MONOUML_DLL_IL) $(DESTDIR)$(prefix)/$(PACKAGE)/;
