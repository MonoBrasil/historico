# MonoUML. CASE tool
# http://www.monouml.org
DESTDIR=

MONOUML_DLL_GENERATOR_SQL_SOURCES=\
	SerializeClassRule.cs \
	ProcessDeserializedElementsRule.cs \
	ProcessClassRule.cs \
	ProcessAttributeRule.cs \
	NavigatePackageRule.cs \
	NavigateClassRule.cs \
	Expert.cs \
	AssemblyInfo.cs \
	TemplateTree/Table.cs \
	TemplateTree/PrimaryKey.cs \
	TemplateTree/Column.cs
	
MONOUML_RESOURCES=\
	TemplateTree/Templates/Table.txt \
	TemplateTree/Templates/PrimaryKey.txt \
	TemplateTree/Templates/Column.txt
	
EXTRA_DIST=$(MONOUML_DLL_GENERATOR_SQL_SOURCES) $(MONOUML_RESOURCES)

MONOUML_RESOURCES_BUILD=$(foreach res,$(MONOUML_RESOURCES), $(addprefix -resource:,$(res)),$(notdir $(res)))

all-am: $(MONOUML_DLL_GENERATOR_SQL)
	@echo 'Done: MonoUML.Generators.SqlGeneration.dll'

$(MONOUML_DLL_GENERATOR_SQL): $(MONOUML_DLL_GENERATOR_SQL_SOURCES)
	$(MCS) -t:library -r:$(EC_DLL_TEMPLATES) -r:$(EC_DLL_ES) -r:$(EC_DLL_UML2) -out:$(MONOUML_DLL_GENERATOR_SQL) $(MONOUML_RESOURCES_BUILD) $(MONOUML_DLL_GENERATOR_SQL_SOURCES)

clean-am:
	rm -f $(MONOUML_DLL_GENERATOR_SQL)

install-exec-am: all
	mkdir -p $(DESTDIR)$(prefix)/$(PACKAGE)/generators/; \
	$(INSTALL) -m 664 $(MONOUML_DLL_GENERATOR_SQL) $(DESTDIR)$(prefix)/$(PACKAGE)/generators/;
