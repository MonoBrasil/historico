MCATALOG_ORIGINAL_CSFILES =				\
	$(srcdir)/AssemblyInfo.cs 			\
	$(srcdir)/Exporter.cs 				\
	$(srcdir)/Catalog.cs 				\
	$(srcdir)/CatalogList.cs 			\
	$(srcdir)/Database.cs 				\
	$(srcdir)/Item.cs 				\
	$(srcdir)/ItemList.cs 				\
	$(srcdir)/Main.cs 				\
	$(srcdir)/ItemShelf.cs	 			\
	$(srcdir)/View.cs 				\
	$(srcdir)/Presentation.cs 			\
	$(srcdir)/Conf.cs 				\
	$(srcdir)/ImageSize.cs 				\
	$(srcdir)/ItemCollection.cs 			\
	$(srcdir)/RatingWidget.cs 			\
	$(srcdir)/BorrowerList.cs 			\
	$(srcdir)/Borrower.cs 				\
	$(srcdir)/TitleWidget.cs			\
	$(srcdir)/Order.cs				\
	$(srcdir)/search_engines/SearchResults.cs	\
	$(srcdir)/search_engines/SearchResultsAlbum.cs	\
	$(srcdir)/search_engines/SearchResultsBook.cs	\
	$(srcdir)/search_engines/SearchResultsFilm.cs	\
        $(srcdir)/search_engines/SearchEngine.cs        \
        $(srcdir)/search_engines/SearchEngineAttribute.cs \
        $(srcdir)/search_engines/AmazonSearchService.cs \
        $(srcdir)/search_engines/SearchEngineAmazon.cs  \
        $(srcdir)/search_engines/SearchEngineIMDB.cs  \
	$(srcdir)/dialogs/AddDialog.cs			\
	$(srcdir)/dialogs/AddBookDialog.cs 		\
	$(srcdir)/dialogs/AddFilmDialog.cs 		\
	$(srcdir)/dialogs/AddAlbumDialog.cs 		\
	$(srcdir)/dialogs/CatalogPropertiesDialog.cs 	\
	$(srcdir)/dialogs/SelectDialog.cs               \
	$(srcdir)/dialogs/ProgressDialog.cs 		\
	$(srcdir)/dialogs/LendItemDialog.cs 		\
	$(srcdir)/dialogs/ExportDialog.cs 		\
	$(srcdir)/dialogs/PreferencesDialog.cs

MCATALOG_CSFILES =					\
	$(MCATALOG_ORIGINAL_CSFILES)			\
	$(srcdir)/Defines.cs

ASSEMBLIES =					\
	/r:System.Data				\
	/r:System.Web.Services			\
	/r:Mono.Data.SqliteClient		\
	/r:Mono.Posix				\
	/pkg:gecko-sharp-2.0			\
	/pkg:glade-sharp-2.0			\
	/pkg:gconf-sharp-2.0			\
	/pkg:gtk-sharp-2.0			\
	/pkg:gnome-sharp-2.0			\
	/pkg:evolution-sharp

RESOURCE_FILES= $(top_srcdir)/resources/mainwindow.glade	\
	$(top_srcdir)/resources/themeselection.glade		\
	$(top_srcdir)/resources/dialogaddbook.glade		\
	$(top_srcdir)/resources/dialogaddfilm.glade		\
	$(top_srcdir)/resources/dialogaddalbum.glade		\
	$(top_srcdir)/resources/dialogexport.glade		\
	$(top_srcdir)/resources/title.glade			\
	$(top_srcdir)/resources/Database.sql			\
	$(top_srcdir)/resources/template_items_books.html	\
	$(top_srcdir)/resources/template_items_films.html	\
	$(top_srcdir)/resources/template_items_albums.html	\
	$(top_srcdir)/resources/films.png			\
	$(top_srcdir)/resources/books.png			\
	$(top_srcdir)/resources/albums.png			\
	$(top_srcdir)/resources/shelf.png			\
	$(top_srcdir)/resources/list.png			\
	$(top_srcdir)/resources/bigstar.png			\
	$(top_srcdir)/resources/littlestar.png			\
	$(top_srcdir)/resources/empty.png			\
	$(top_srcdir)/resources/dbupdates/0.0.5.sql		\
	$(top_srcdir)/resources/dbupdates/0.0.7.sql

RESOURCES = $(foreach res, $(RESOURCE_FILES), $(addprefix -resource:,$(res)),$(notdir $(res)))

mcatalog.exe: $(MCATALOG_CSFILES)
	$(MCS) -out:$@ $(CSFLAGS) $(MCATALOG_CSFILES) $(ASSEMBLIES) $(RESOURCES)

all: mcatalog.exe

mcataloglibdir = $(prefix)/lib/mcatalog

install-data-local: mcatalog.exe
	$(mkinstalldirs) $(DESTDIR)$(mcataloglibdir)
	$(INSTALL_DATA) mcatalog.exe $(DESTDIR)$(mcataloglibdir)/mcatalog.exe	

bin_SCRIPTS = mcatalog

Defines.cs: Defines.cs.in
	sed -e "s|\@version\@|$(VERSION)|" \
	-e "s|\@prefix\@|$(prefix)|" \
	< Defines.cs.in > Defines.cs

mcatalog: mcatalog.in
	sed -e "s|\@prefix\@|$(prefix)|" < $(srcdir)/mcatalog.in > mcatalog
	chmod +x mcatalog 

EXTRA_DIST =					\
	$(MCATALOG_ORIGINAL_CSFILES)		\
	Defines.cs.in				\
	mcatalog.in

CLEANFILES =					\
	mcatalog				\
	Defines.cs				\
	mcatalog.exe

DISTCLEANFILES = 				\
	mcatalog				\
	Defines.cs				\
	mcatalog.exe				\
	makefile
