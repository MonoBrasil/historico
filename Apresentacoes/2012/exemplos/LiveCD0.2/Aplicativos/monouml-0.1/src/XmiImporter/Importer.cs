/*
MonoUML.XmiImporter - Tool to convert from Argo and Poseidon files to MonoUML
Copyright (C) 2005 Rodolfo Campero

This file is part of MonoUML.

MonoUML is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

MonoUML is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MonoUML; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using ICSharpCode.SharpZipLib.Zip;

namespace MonoUML.XmiImporter
{
	public class Importer
	{
		public void Convert(string inputfile, string outputfile)
		{
			// loads the input file
			XmlDocument input = GetInputFile (inputfile);
			// generates the output
			XmlTextWriter output = outputfile==null
				? new XmlTextWriter(Console.Out)
				: new XmlTextWriter(
					outputfile, Encoding.GetEncoding("ISO-8859-15"));
			output.Formatting = Formatting.Indented;
			System.Security.Policy.Evidence evidence = outputfile==null
				? null : XmlSecureResolver.CreateEvidenceForUrl("file://" + outputfile); 
			XmlResolver res = new XmlUrlResolver ();
			XslTransform xslt = GetXslt (input, evidence, res);
			xslt.Transform(input, new XsltArgumentList (), output, res);
			output.WriteRaw(System.Environment.NewLine);
		}
		
		private static XslTransform GetXslt (
			XmlDocument input, 
			System.Security.Policy.Evidence evidence,
			XmlResolver res)
		{
			// loads the transformation stylesheet
			Assembly a = Assembly.GetExecutingAssembly();
			string stylesheet = null;
			string inputVersion = input.SelectNodes("/XMI/@xmi.version")[0].Value;
			switch(inputVersion)
			{
				case "1.0":	stylesheet = "fromXMI1_0.xslt"; break; 
				case "1.2":	stylesheet = "fromXMI1_2.xslt"; break;
				default:
					throw new ApplicationException ("XMI version " + inputVersion + " is not supported.");
			}
			Stream st = a.GetManifestResourceStream(stylesheet);
			XslTransform xslt = new XslTransform();
			xslt.Load(new XmlTextReader(new StreamReader(st)), res, evidence);
			return xslt;
		}

		public XmlDocument ImportXmi (string filename)
		{
			XmlDocument input = GetInputFile (filename);
			XmlResolver res = new XmlUrlResolver ();
			XslTransform xslt = GetXslt (input, null, res);
			XmlReader reader = xslt.Transform (input, new XsltArgumentList (), res);
			XmlDocument output = new XmlDocument ();
			output.Load (reader);
			return output;
		}

		public Stream ImportXmiStream (string filename)
		{
			XmlDocument input = GetInputFile (filename);
			XmlResolver res = new XmlUrlResolver ();
			XslTransform xslt = GetXslt (input, null, res);
			MemoryStream output = new MemoryStream ();
			xslt.Transform (input, new XsltArgumentList (), output, res);
			return output;
		}
		
		private static XmlDocument GetInputFile (string filename)
		{
			ZipFile file;
			ZipEntry entry;
			Stream stream;
			string ext = Path.GetExtension (filename).ToLower ();
			if (ext==".zargo" || ext==".zuml")
			{
				file = new ICSharpCode.SharpZipLib.Zip.ZipFile (filename);
				entry = null;
				foreach (ZipEntry e in file)
				{
					if (Path.GetExtension (e.Name) == ".xmi")
					{
						entry = e;
						break;
					}
				}
				stream = file.GetInputStream (entry);
			}
			else
			{
				stream = new FileStream (filename, FileMode.Open, FileAccess.Read);
			}
			XmlDocument xmlDoc = new XmlDocument ();
			xmlDoc.Load (stream);
			return xmlDoc;
		}
	}
}
