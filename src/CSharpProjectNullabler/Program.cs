using System;
using System.IO; // Directory.EnumerateFiles
using System.Collections.Generic; // IEnumerable<>
using System.Xml.Linq; // XDocument
using System.Xml; // XmlWriter 
using System.Text; // StringBuilder

namespace CSharpProjectNullabler
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string path in GetProjectFiles()) {
                var xml = CreateNullableXDocument(path);
                using (var writer = CreateOmitXmlDeclarationWriter(path)) { xml.Save(writer); }
            }
        }
        static IEnumerable<string> GetProjectFiles()
        {
            //return Directory.GetFiles(System.Environment.CurrentDirectory, "*.csproj");
            return Directory.EnumerateFiles(System.Environment.CurrentDirectory, "*.csproj");
            // return Directory.EnumerateFiles(System.Environment.CurrentDirectory, "*.csproj", SearchOption.AllDirectories);
        }
        static XmlWriter CreateOmitXmlDeclarationWriter(string path)
        {
            XmlWriterSettings xws = new XmlWriterSettings();  
            xws.OmitXmlDeclaration = true; // XML宣言を出力しない
            xws.Indent = true;  
            return XmlWriter.Create(path, xws);
        }
        static XDocument CreateNullableXDocument(string path)
        {
            XDocument xml = XDocument.Load(path);
            XElement project = xml.Element("Project");
            XElement propertyGroup = project.Element("PropertyGroup");
            XElement? nullable = propertyGroup.Element("Nullable");
            if (null == nullable) { propertyGroup.Add(new XElement("Nullable", "enable")); }
            return xml;
        }
    }
}
