ifndef TARGET
	TARGET=./bin/Debug
else
	TARGET=./bin/$(TARGET)
endif

MCS=mcs
ifndef (RELEASE)
	MCSFLAGS=-debug --stacktrace
endif

LIBS=-lib:/usr/lib/mono/1.0 -lib:/usr/lib/mono/gtk-sharp -r:System.Drawing


GMA_DRAWING_IMAGEINFO_DLL=$(TARGET)/gma.Drawing.ImageInfo.dll
GMA_DRAWING_IMAGEINFO_PDB=$(TARGET)/gma.Drawing.ImageInfo.pdb
GMA_DRAWING_IMAGEINFO_SRC=./AssemblyInfo.cs \
	./Constants.cs \
	./PropertyTag.cs \
	./Fraction.cs \
	./Info.cs
GMA_DRAWING_IMAGEINFO_RES=

$(GMA_DRAWING_IMAGEINFO_DLL): $(GMA_DRAWING_IMAGEINFO_SRC) 
	-mkdir -p $(TARGET)
	$(MCS) $(MCSFLAGS) $(LIBS)  -target:library -out:$(GMA_DRAWING_IMAGEINFO_DLL) $(GMA_DRAWING_IMAGEINFO_RES) $(GMA_DRAWING_IMAGEINFO_SRC)


# common targets

all:	$(GMA_DRAWING_IMAGEINFO_DLL)

clean:
	-rm -f "$(GMA_DRAWING_IMAGEINFO_DLL)" 2> /dev/null
	-rm -f "$(GMA_DRAWING_IMAGEINFO_PDB)" 2> /dev/null


# project names as targets

gma.Drawing.ImageInfo: $(GMA_DRAWING_IMAGEINFO_DLL)

