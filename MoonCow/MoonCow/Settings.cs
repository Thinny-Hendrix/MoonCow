using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;

namespace MoonCow
{
    public static class Settings
    {
        public static float difficulty;
        public static bool fullScreen;
        public static Vector2 resolution;
        public static float masterVolume;
        public static float musicVolume;
        public static float effectsVolume;
        public static bool firstPlay;


        private void updateSettings()
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
