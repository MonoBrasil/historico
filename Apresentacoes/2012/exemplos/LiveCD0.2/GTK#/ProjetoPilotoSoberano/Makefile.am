##Process this file with automake to create Makefile.in 

SUBDIRS = LifeLets.Core LifeLets.Console LifeLets.Gui Test

test: all
	cd Test && make test

EXTRA_DIST = \
	ChangeLog \
	autogen.sh \
	TOOLS/GladeSync.cs \
	lib/Bamboo.Prevalence.dll \
	lib/nunit.framework.dll
	
#        lifelets.spec.in          \
#        lifelets.spec

#DISTCLEANFILES =                        \
#       intltool-extract                \
#       intltool-merge                  \
#       intltool-update                 \
#       po/.intltool-merge-cache
