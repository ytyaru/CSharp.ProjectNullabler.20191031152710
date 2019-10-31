using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
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
            Console.WriteLine($"xml: {xml}");
            Console.WriteLine($"<Project>: {xml.Element("Project")}");

            XElement project = xml.Element("Project");
            XElement propertyGroup = project.Element("PropertyGroup");
            XElement? nullable = propertyGroup.Element("Nullable");
            if (null == nullable) { propertyGroup.Add(new XElement("Nullable", "enable")); }
            Console.WriteLine($"propertyGroup: {propertyGroup}");
            // XML宣言が追記されてしまう！ <?xml version="1.0" encoding="utf-8"?>
//            xml.Declaration = null; // 効果なし
//            xml.Save(path);
            return xml;
        }
    }
}
