using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using VirtualAssistant.Models;

namespace VirtualAssistant
{
    public static class Utilities
    {
        public static string GetTemplatePath()
        {
            return System.IO.Path.Combine(GetCurrentDirectory(), "Templates");
        }

        public static string GetTemplateFilePath(string fileName)
        {
            return System.IO.Path.Combine(GetTemplatePath(), fileName + ".tmpl");
        }

        public static string GetCurrentDirectory()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        public static bool FileExists(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                FileInfo fi = new FileInfo(filePath);
                return fi.Exists;
            }
            else
            {
                throw new ArgumentNullException(nameof(filePath));
            }
        }

        public static List<CommandItem> LoadCommands()
        {
            if (File.Exists(Path.Combine(Utilities.GetCurrentDirectory(), "Commands.xml")))
            {
                return DeserializeFromFile<List<CommandItem>>(Path.Combine(Utilities.GetCurrentDirectory(), "Commands.xml"));
            }
            else
            {
                List<CommandItem> commands = new List<CommandItem>();
                SerializeToFile<List<CommandItem>>(Path.Combine(Utilities.GetCurrentDirectory(), "Commands.xml"), commands);
                return commands;
            }
        }

        public static void SaveCommands(List<CommandItem> commands)
        {
            SerializeToFile<List<CommandItem>>(Path.Combine(Utilities.GetCurrentDirectory(), "Commands.xml"), commands);
        }

        public static ApplicationConfiguation LoadApplicationConfig()
        {
            if (File.Exists(Path.Combine(Utilities.GetCurrentDirectory(), "ApplicationConfiguation.xml")))
            {
                return DeserializeFromFile<ApplicationConfiguation>(Path.Combine(Utilities.GetCurrentDirectory(), "ApplicationConfiguation.xml"));
            }
            else
            {
                ApplicationConfiguation config = new ApplicationConfiguation { AssistantName = "Jarvis", Suffix = "Sir", UseNameInResponse = true, UseSuffix = true };
                SerializeToFile<ApplicationConfiguation>(Path.Combine(Utilities.GetCurrentDirectory(), "ApplicationConfiguation.xml"), config);
                return config;
            }
        }

        // Serialize to file
        /// <summary>
        /// Takes a serializable object and saves it to an xml file.
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="path">The path including the file name.</param>
        /// <param name="obj">The object to serialize.</param>
        public static void SerializeToFile<T>(string path, T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextWriter textWriter = new StreamWriter(path))
            {
                serializer.Serialize(textWriter, obj);
                textWriter.Close();
            }
        }

        /// <summary>
        /// Deserializes an object from a file.
        /// </summary>
        /// <param name="path">The path including the file name.</param>
        /// <returns>Object of type T</returns>
        public static T DeserializeFromFile<T>(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                XmlReader xmlReader = new XmlTextReader(stream);
                return (T)xmlSerializer.Deserialize(xmlReader);
            }
        }
    }
}
