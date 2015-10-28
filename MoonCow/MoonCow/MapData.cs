using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace MoonCow
{
    class MapData
    {
        int id;
        String name;
        String creator;
        int width;
        int length;
        public int[,] map;
        String fileName;

        public MapData(String fileName)
	    {
            this.fileName = fileName;
            read(fileName);
	    }

        public MapData(int id, String name, String creator, int width, int length, int[,] map)
        {
            this.id = id;
            this.name = name;
            this.creator = creator;
            this.width = width;
            this.length = length;
            this.map = map;
            fileName = @"Content/MapXml/" + name + ".xml";
        }

        private void read(String fileName) //using xpath
        {
            List<int> nodes = new List<int>();

            XPathDocument file = new XPathDocument(fileName);
            XPathNavigator nav = file.CreateNavigator();
            XPathNodeIterator iterator = nav.Select("//tiles/tile");

            if (iterator.Count > 0)
            {
                while (iterator.MoveNext())
                {
                    nodes.Add(Int32.Parse(iterator.Current.Value));
                }
            }
            else
            {
                Console.WriteLine("Map does not contain any tiles");
            }

            iterator = nav.Select("//data/width");
            if (iterator.Count > 0)
            {
                while (iterator.MoveNext())
                {
                    width = Int32.Parse(iterator.Current.Value);
                }
            }
            else
            {
                Console.WriteLine("Width Error");
            }

            iterator = nav.Select("//data/ID");
            if (iterator.Count > 0)
            {
                while (iterator.MoveNext())
                {
                    id = Int32.Parse(iterator.Current.Value);
                }
            }
            else
            {
                Console.WriteLine("ID Error");
            }

            iterator = nav.Select("//data/name");
            if (iterator.Count > 0)
            {
                while (iterator.MoveNext())
                {
                    name = iterator.Current.Value;
                }
            }
            else
            {
                Console.WriteLine("Name Error");
            }

            iterator = nav.Select("//data/creator");
            if (iterator.Count > 0)
            {
                while (iterator.MoveNext())
                {
                    creator = iterator.Current.Value;
                }
            }
            else
            {
                Console.WriteLine("Creator Error");
            }
          
            iterator = nav.Select("//data/length");
            if (iterator.Count > 0)
            {
                while (iterator.MoveNext())
                {
                    length = Int32.Parse(iterator.Current.Value);
                }
            }
            else
            {
                Console.WriteLine("Length Error");
            }

            map = convertTo2D(width, length, nodes);
        }

        private int[,] convertTo2D(int width, int length, List<int> nodes)
        {
            int[,] map = new int[width, length];
            int count = 0;

            for (int i = 0; i < length; i++)
            {
                for (int q = 0; q < width; q++)
                {
                    map[q, i] = nodes[count];
                    count++;
                }
            }
            return map;
        }

        public void writeMap()
        {
            XmlWriter xmlWriter = XmlWriter.Create(fileName);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("map");

            xmlWriter.WriteStartElement("data");
            xmlWriter.WriteStartElement("id");
            xmlWriter.WriteString(id + "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("name");
            xmlWriter.WriteString(name + "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("creator");
            xmlWriter.WriteString(creator + "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("width");
            xmlWriter.WriteString(width + "");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("length");
            xmlWriter.WriteString(length + "");
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("tiles");

            for (int i = 0; i < length; i++)
            {
                for (int q = 0; q < width; q++)
                {
                    xmlWriter.WriteStartElement("tile");
                    xmlWriter.WriteString(map[i, q] + "");
                    xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }

        public int[,] getNodes()
        {
            return map;
        }

        public int getId()
        {
            return id;
        }

        public String getName()
        {
            return name;
        }

        public String getCreator()
        {
            return creator;
        }

        public int getWidth()
        {
            return width;
        }

        public int getLength()
        {
            return length;
        }
    }
}
