using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using Localizer.Model;


namespace Localizer
{
    class ResourceCreator
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing resource file creation.....");
            string[] resourceTypes = {"Label", "Master", "Messages"};
            var rd = CreateResourceDictionary();
            var dictionaryList = SplitResourceDictionary(rd, GetKeyMapping());
            CreateResourceFile(dictionaryList[0], resourceTypes[0]);
            Console.WriteLine(resourceTypes[0] + " file generated.....");
            CreateResourceFile(dictionaryList[1], resourceTypes[1]);
            Console.WriteLine(resourceTypes[1] + " file generated.....");
            CreateResourceFile(dictionaryList[2], resourceTypes[2]);
            Console.WriteLine(resourceTypes[2] + " file generated.....");
            Console.WriteLine("Resource files succesfully generated... Press any key to exit.");
            Console.ReadKey();
        }

        static Dictionary<string, string> CreateResourceDictionary()
        {
            Dictionary<string, string> resourceDictionary = new Dictionary<string, string>();
            string[] lines = File.ReadAllLines(@"C:\Users\csircao\Desktop\PVT 2.0\Utility\Localizer\Localizer\Resource\PVT_zn_hans.txt");
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
        static List<Dictionary<string, string>> SplitResourceDictionary(Dictionary<string, string> resourceDictionary, Dictionary<string, KeyFile> keyMapping)
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
            List<Dictionary<string, string>> dictionarySplits =
                new List<Dictionary<string, string>> { labelDictionary, masterDictionary, messagesDictionary };
            return dictionarySplits;

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

        static void CreateResourceFile(Dictionary<string, string> resourceDict,string name)
        {
            using (ResXResourceWriter resx = new ResXResourceWriter(@"C:\"+name+".resx"))
            {
                foreach (var resource in resourceDict)
                {
                    resx.AddResource(resource.Key,resource.Value);
                }
            }
        }

    }
}
