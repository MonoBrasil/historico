#                                               -*- Autoconf -*-
# Process this file with autoconf to produce a configure script.

AC_PREREQ(2.59)
AC_INIT(LifeLets, 0.8, computacaosoberana@yahoogroups.com)
AM_INIT_AUTOMAKE(@PACKAGE_NAME@, @PACKAGE_VERSION@)

# Checks for programs.

AC_PROG_MAKE_SET
AC_PROG_INSTALL 
AC_PROG_LN_S
AC_PROG_RANLIB

# Check for mcs

AC_PATH_PROG(MCS, mcs)
if test "x$MCS" = "x" ; then
        AC_MSG_ERROR([Can't find "mcs" in your PATH])
fi

# Check for nunit-console

AC_PATH_PROG(NUNITCONSOLE, nunit-console)
if test "x$NUNITCONSOLE" = "x" ; then
        AC_MSG_ERROR([Can't find "nunit-console" in your PATH])
fi

# Check for $(top_srcdir)/build/bin

if test ! -d "./build" ; then
        mkdir build
fi

if test ! -d "./build/bin" ; then
        mkdir build/bin
fi

# Checks for libraries.

AC_PATH_PROG(NUNIT, nunit.framework.dll, no, ./lib)
if test "x$NUNIT" = "xno"; then
	AC_MSG_ERROR([You need to install nunit])
fi

AC_PATH_PROG(BAMBOO, Bamboo.Prevalence.dll, no, ./lib)
if test "x$BAMBOO" = "xno"; then
	AC_MSG_ERROR([You need to install bamboo])
fi

# Find pkg-config

AC_PATH_PROG(PKG_CONFIG, pkg-config, no)
if test "x$PKG_CONFIG" = "xno"; then
	AC_MSG_ERROR([You need to install pkg-config])
fi

# Gtk# is needed by LifeLets

PKG_CHECK_MODULES(LIFELETS, mono)
AC_SUBST(MONO_LIBS)
PKG_CHECK_MODULES(GTK, gtk-sharp glade-sharp)
AC_SUBST(GTK_LIBS)
AC_DEFINE_UNQUOTED(BAMBOO_LIBS, -r:$BAMBOO)
AC_DEFINE_UNQUOTED(NUNIT_LIBS, -r:$NUNIT)


# Checks for header files.

# Checks for typedefs, structures, and compiler characteristics.

# Checks for library functions.

AC_CONFIG_FILES([
	Makefile
	Test/Makefile
	LifeLets.Core/Makefile
	LifeLets.Console/Makefile
	LifeLets.Gui/Makefile])
AC_OUTPUT
