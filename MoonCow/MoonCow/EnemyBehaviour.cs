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
    static class EnemyBehaviour
    {
        public enum Behaviour { ShortestPathFirst, AvoidTurretDamage, AvoidPlayerDamage };      // an enum value to store what the enemy does

        //variables to hold the behaviour enum
        public static Behaviour swarmerBehaviour;
        public static Behaviour gunnerBehaviour;
        public static Behaviour sneakerBehaviour;
        public static Behaviour heavyBehaviour;


        public static void load() //using xpath
        {
            XPathDocument file = new XPathDocument(@"Content/Settings/enemyBehaviours.xml");
            XPathNavigator nav = file.CreateNavigator();
            
            XPathNodeIterator iterator = nav.Select("//swarmer"); // Look at the swarmer tag?
            if (iterator.Count > 0)
            {
                string behaviour = "";
                while (iterator.MoveNext())
                {
                    //behaviour = whatever is in the tag value
                }

                System.Diagnostics.Debug.WriteLine("Swarmer behaviour  = " + behaviour); // read out what is ebing read from the xml file for debug purposes

                // From that string in the xml file set the right enum value
                if (behaviour.Equals("AvoidTurretDamage"))
                {
                    swarmerBehaviour = Behaviour.AvoidTurretDamage;
                }
                else
                {
                    if (behaviour.Equals("AvoidPlayerDamage"))
                    {
                        swarmerBehaviour = Behaviour.AvoidPlayerDamage;
                    }
                    else
                    {
                        //shortest path
                        swarmerBehaviour = Behaviour.ShortestPathFirst;
                    }
                }
            }
            else
            {
                Console.WriteLine("Swarmer Behaviour Settings Error");
            }




            //Do the same for every other class




            iterator = nav.Select("//gunner");
            if (iterator.Count > 0)
            {
                string behaviour = "";
                while (iterator.MoveNext())
                {
                    //char value = char.Parse(iterator.Current.Value);
                    //behaviour += value;
                }

                System.Diagnostics.Debug.WriteLine("Gunner behaviour  = " + behaviour);

                if (behaviour.Equals("AvoidTurretDamage"))
                {
                    gunnerBehaviour = Behaviour.AvoidTurretDamage;
                }
                else
                {
                    if (behaviour.Equals("AvoidPlayerDamage"))
                    {
                        gunnerBehaviour = Behaviour.AvoidPlayerDamage;
                    }
                    else
                    {
                        //shortest path
                        gunnerBehaviour = Behaviour.ShortestPathFirst;
                    }
                }
            }
            else
            {
                Console.WriteLine("Gunner Behaviour Settings Error");
            }

            iterator = nav.Select("//sneaker");
            if (iterator.Count > 0)
            {
                string behaviour = "";
                while (iterator.MoveNext())
                {
                    //char value = char.Parse(iterator.Current.Value);
                    //behaviour += value;
                }

                System.Diagnostics.Debug.WriteLine("Sneaker behaviour  = " + behaviour);

                if (behaviour.Equals("AvoidTurretDamage"))
                {
                    sneakerBehaviour = Behaviour.AvoidTurretDamage;
                }
                else
                {
                    if (behaviour.Equals("AvoidPlayerDamage"))
                    {
                        sneakerBehaviour = Behaviour.AvoidPlayerDamage;
                    }
                    else
                    {
                        //shortest path
                        sneakerBehaviour = Behaviour.ShortestPathFirst;
                    }
                }
            }
            else
            {
                Console.WriteLine("Sneaker Behaviour Settings Error");
            }

            iterator = nav.Select("//heavy");
            if (iterator.Count > 0)
            {
                string behaviour = "";
                while (iterator.MoveNext())
                {
                    //char value = char.Parse(iterator.Current.Value);
                    //behaviour += value;
                }

                System.Diagnostics.Debug.WriteLine("Heavy behaviour  = " + behaviour);

                if (behaviour.Equals("AvoidTurretDamage"))
                {
                    heavyBehaviour = Behaviour.AvoidTurretDamage;
                }
                else
                {
                    if (behaviour.Equals("AvoidPlayerDamage"))
                    {
                        heavyBehaviour = Behaviour.AvoidPlayerDamage;
                    }
                    else
                    {
                        //shortest path
                        heavyBehaviour = Behaviour.ShortestPathFirst;
                    }
                }
            }
            else
            {
                Console.WriteLine("Heavy Behaviour Settings Error");
            }

        }

    }
}
