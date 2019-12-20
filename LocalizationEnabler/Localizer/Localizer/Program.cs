using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using Localizer.Model;

namespace Localizer
{
    class Program
    {
        static List<MetaData> meta = new List<MetaData>();

        static void Maina(string[] args)
        {
            //Console.WriteLine("AVASVASVASV \v ASASASASASAS");
            //Console.ReadKey();
            //string lol = "manoj";
            //string line = "ABsbc {0}{0}{1}sajacsjcbajscbasjc {0}{0}{1}asvasvasvasvsav";
            //line ="Of the equipment identified as Air Handling Units (AHUs), if the Mixed Air Damper and Mixed Air Temperature objects are found, or the Outdoor Air Damper object value shows variable modulation between 0% and 100%, then the device is categorized as “AHU Without 100% Outdoor Air”. {0}{0}{1}If neither Mixed Air objects are found and the Outdoor Air Damper object value shows 2-stage operation (0% or 100%), then the device is categorized as “AHU With 100% Outdoor Air”. {0}{0}{1}If none of the objects identified above are found based on the search criteria (configured using the options under the Preference menu), the device is categorized as “Undetermined”";
            //line = string.Format(lol+Environment.NewLine+string.Format(line, Environment.NewLine,"\t"));
            //Console.WriteLine(line);
            //Console.ReadKey();
            //var splitLines = line.Split('\n');
            //Console.WriteLine(splitLines[0].Trim()+Environment.NewLine+splitLines[1].Trim());
            //Console.ReadKey();
            string path = @"C: \Users\csircao\Desktop\PVT 2.0\Utility\Localizer\Localizer\Resource\";
            string[] resourceTypes = { "Label", "Master", "Messages" };
            //UpdateWeblate(path, resourceTypes);
            CreateWeblate(path, resourceTypes);
            //var rd = CreateResourceDictionary();
            //SplitResourceDictionary(rd, GetKeyMapping());
            //Console.ReadKey();
        }
        static Dictionary<string, string> CreateResourceDictionary()
        {
            Dictionary<string, string> resourceDictionary = new Dictionary<string, string>();
            string[] lines = File.ReadAllLines(@"C:\Users\csircao\Desktop\PVT 2.0\Utility\Localizer\Localizer\Resource\PVT_en_US.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                string[] splitString = lines[i].Split('=');
                string key = splitString[0];
                string value = splitString[1];
                while (i + 1 < lines.Length && (lines[i + 1] == "" || lines[i + 1][0] != '0'))
                {
                    i++;
                    value += '\n' + lines[i];
                }
                resourceDictionary.Add(key, value);
            }
            return resourceDictionary;
        }
        static void SplitResourceDictionary(Dictionary<string, string> resourceDictionary, Dictionary<string, KeyFile> keyMapping)
        {
            Dictionary<string, string> labelDictionary = new Dictionary<string, string>();
            Dictionary<string, string> masterDictionary = new Dictionary<string, string>();
            Dictionary<string, string> messagesDictionary = new Dictionary<string, string>();
            foreach (var map in keyMapping)
            {
                var data = resourceDictionary.First(x => x.Key == map.Key);
                switch (map.Value.File)
                {
                    case "Label":
                        labelDictionary.Add(map.Value.Key, data.Value);
                        break;
                    case "Master":
                        masterDictionary.Add(map.Value.Key, data.Value);
                        break;
                    case "Messages":
                        messagesDictionary.Add(map.Value.Key, data.Value);
                        break;
                }
            }

        }

        static void UpdateWeblate(string path, string[] resourceTypes)
        {
            // Dictionary<string, KeyFile> keyMap = GetKeyMapping();
            Dictionary<string, KeyFile> updatedKeyMap = new Dictionary<string, KeyFile>();
            var resourceDictionary = CreateResourceDictionary();
            var lastElement = resourceDictionary.Last();
            double counter = Convert.ToDouble(lastElement.Key) + 0.00001;
            using (StreamWriter w = File.AppendText(@"C:\Users\csircao\Desktop\PVT 2.0\Utility\Localizer\Localizer\Resource\PVT_en_US.txt"))
            {
                foreach (var type in resourceTypes)
                {
                    var xmlStr = File.ReadAllText(path + type + ".xml");
                    var str = XElement.Parse(xmlStr);
                    var result = str.Elements("data");
                    foreach (var item in result)
                    {
                        string value = item.Element("value").Value.Trim();
                        if (resourceDictionary.ContainsValue(value))
                            continue;
                        string weblateIndex = $"{counter:00000.00000}";
                        updatedKeyMap.Add(weblateIndex, new KeyFile() { File = type, Key = item.Attribute("name").Value });
                        w.WriteLine(weblateIndex + "=" + value);
                        counter += 0.00001;
                    }
                }
            }
            UpdateKeyMapping(updatedKeyMap);
        }
        static void CreateWeblate(string path, string[] resourceTypes)
        {
            Dictionary<string, KeyFile> keyMap = new Dictionary<string, KeyFile>();
            double counter = 0.00001;
            using (StreamWriter writetext = new StreamWriter("PVT_en_US.txt"))
            {
                foreach (var type in resourceTypes)
                {
                    MetaData metaData = new MetaData();
                    metaData.ResourceName = type;
                    var xmlStr = File.ReadAllText(path + type + ".xml");
                    var str = XElement.Parse(xmlStr);
                    var result = str.Elements("data");
                    metaData.StartKey = $"{counter:00000.00000}";
                    foreach (var item in result)
                    {
                        string value = item.Element("value").Value.Trim();
                        if (string.IsNullOrWhiteSpace(value))
                            continue;    
                        string weblateIndex = $"{counter:00000.00000}";
                        keyMap.Add(weblateIndex, new KeyFile() { File = type, Key = item.Attribute("name").Value });
                        writetext.WriteLine(weblateIndex + "=" + value);
                        counter += 0.00001;
                    }
                    metaData.EndKey = $"{counter - 0.00001:00000.00000}";
                    meta.Add(metaData);
                }
            }
            //CreateMetaData(meta);
            CreateKeyMapping(keyMap);
        }
        //static void CreateMetaData(List<MetaData> meta)
        //{

        //    using (StreamWriter writetext = new StreamWriter("PVT_en_US-MetaData.txt"))
        //    {
        //        foreach (var data in meta)
        //        {
        //            writetext.WriteLine(data.ResourceName + " -> " + data.StartKey + " to " + data.EndKey);

        //            Console.WriteLine(data.ResourceName + " -> " + data.StartKey + " to " + data.EndKey);
        //        }
        //    }
        //}
        static void CreateKeyMapping(Dictionary<string, KeyFile> keyMap)
        {
            using (StreamWriter file = new StreamWriter("mapping.txt"))
                foreach (var entry in keyMap)
                    file.WriteLine("{0}:{1}", entry.Key, entry.Value.Key + "-" + entry.Value.File);
        }
        static void UpdateKeyMapping(Dictionary<string, KeyFile> keyMap)
        {
            var existingKeyMap = GetKeyMapping();
                foreach (var entry in keyMap)
                {
                    KeyValuePair<string, KeyFile> currentEntry;
                    if (existingKeyMap.Any(x => x.Value.Key == entry.Value.Key))
                    {
                        currentEntry = existingKeyMap.First(x => x.Value.Key == entry.Value.Key);
                        existingKeyMap.Remove(currentEntry.Key);
                        existingKeyMap.Add(entry.Key,entry.Value);
                        continue;
                    }
                    currentEntry = entry;
                    existingKeyMap.Add(key:currentEntry.Key,value:entry.Value);
                    //file.WriteLine("{0}:{1}", entry.Key, entry.Value.Key + "-" + entry.Value.File);
                }

            File.Delete(@"C:\Users\csircao\Desktop\PVT 2.0\Utility\Localizer\Localizer\Resource\mapping.txt");
            using (StreamWriter file = new StreamWriter(@"C:\Users\csircao\Desktop\PVT 2.0\Utility\Localizer\Localizer\Resource\mapping.txt"))
                foreach (var entry in existingKeyMap)
                    file.WriteLine("{0}:{1}", entry.Key, entry.Value.Key + "-" + entry.Value.File);
        }
        static Dictionary<string, KeyFile> GetKeyMapping()
        {
            Dictionary<string, KeyFile> keyMap = new Dictionary<string, KeyFile>();
            string[] lines = File.ReadAllLines(@"C:\Users\csircao\Desktop\PVT 2.0\Utility\Localizer\Localizer\Resource\mapping.txt");
            //00000.00005:AHUCO2SetPnt - Label
            for (int i = 0; i < lines.Length; i++)
            {
                string[] splitString = lines[i].Split(':');
                string key = splitString[0];
                string value = splitString[1];
                string[] splitValue = value.Split('-');
                keyMap.Add(key, new KeyFile() { File = splitValue[1].Trim(), Key = splitValue[0].Trim() });
            }
            return keyMap;
        }
    }
}
