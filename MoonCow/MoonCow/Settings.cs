using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public static class Settings
    {
        static String fileName;
        public static float difficulty;
        public static bool fullScreen;
        public static Vector2 resolution;
        public static float masterVolume;
        public static float musicVolume;
        public static float effectsVolume;
        public static bool firstPlay;

        public static void initialize()
        {
            fileName = "../../Content/Settings/settings.xml";
            read(fileName);
        }

        private static void read(String fileName) //using xpath
        {
            XPathDocument file = new XPathDocument(fileName);
            XPathNavigator nav = file.CreateNavigator();
            XPathNodeIterator iterator = nav.Select("//mastervolume");
            if (iterator.Count > 0)
            {
                while (iterator.MoveNext())
                {
                    masterVolume = float.Parse(iterator.Current.Value);
                }
            }
            else
            {
                Console.WriteLine("MasterVolume Error");
            }

            iterator = nav.Select("//musicvolume");
            if (iterator.Count > 0)
            {
                while (iterator.MoveNext())
                {
                    musicVolume = float.Parse(iterator.Current.Value);
                }
            }
            else
            {
                Console.WriteLine("MusicVolume Error");
            }

            iterator = nav.Select("//effectsVolume");
            if (iterator.Count > 0)
            {
                while (iterator.MoveNext())
                {
                    effectsVolume = float.Parse(iterator.Current.Value);
                }
            }
            else
            {
                Console.WriteLine("EffectsVolume Error");
            }

            iterator = nav.Select("//difficulty");
            if (iterator.Count > 0)
            {
                while (iterator.MoveNext())
                {
                    difficulty = float.Parse(iterator.Current.Value);
                }
            }
            else
            {
                Console.WriteLine("Difficulty Error");
            }

            iterator = nav.Select("//rwidth");
            if (iterator.Count > 0)
            {
                while (iterator.MoveNext())
                {
                    resolution.X = float.Parse(iterator.Current.Value);
                }
            }
            else
            {
                Console.WriteLine("Width Error");
            }

            iterator = nav.Select("//rheight");
            if (iterator.Count > 0)
            {
                while (iterator.MoveNext())
                {
                    resolution.Y = float.Parse(iterator.Current.Value);
                }
            }
            else
            {
                Console.WriteLine("Height Error");
            }

            iterator = nav.Select("//fullscreen");
            if (iterator.Count > 0)
            {
                while (iterator.MoveNext())
                {
                    fullScreen = bool.Parse(iterator.Current.Value);
                }
            }
            else
            {
                Console.WriteLine("Fullscreen Error");
            }

            iterator = nav.Select("//firstplay");
            if (iterator.Count > 0)
            {
                while (iterator.MoveNext())
                {
                    firstPlay = bool.Parse(iterator.Current.Value);
                }
            }
            else
            {
                Console.WriteLine("First Play Error");
            }

        }

        public static void updateSettings()
        {
            XmlWriter xmlWriter = XmlWriter.Create("../../Content/Settings/settings.xml");

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("settings");

            xmlWriter.WriteStartElement("mastervolume");
            xmlWriter.WriteString(masterVolume + "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("musicvolume");
            xmlWriter.WriteString(musicVolume + "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("effectsvolume");
            xmlWriter.WriteString(effectsVolume + "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("difficulty");
            xmlWriter.WriteString(difficulty + "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("fullscreen");
            xmlWriter.WriteString(fullScreen + "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("rwidth");
            xmlWriter.WriteString(resolution.X + "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("rheight");
            xmlWriter.WriteString(resolution.Y + "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("firstplay");
            xmlWriter.WriteString(firstPlay + "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }
    }
}
