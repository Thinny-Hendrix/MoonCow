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
        public static Behaviour sentryBehaviour;

        //variables to hold player chase bool
        public static bool swarmerPlayerChase;
        public static bool gunnerPlayerChase;
        public static bool sneakerPlayerChase;
        public static bool heavyPlayerChase;
        public static bool sentryPlayerChase;

        //variables to holf the player charge bool
        public static bool swarmerPlayerCharge;
        public static bool gunnerPlayerCharge;
        public static bool sneakerPlayerCharge;
        public static bool heavyPlayerCharge;
        public static bool sentryPlayerCharge;

        //variables to hold player damage bool
        public static bool swarmerPlayerAttack;
        public static bool gunnerPlayerAttack;
        public static bool sneakerPlayerAttack;
        public static bool heavyPlayerAttack;
        public static bool sentryPlayerAttack;

        //variables to hold pathing follow bool
        public static bool swarmerFollowPath;
        public static bool gunnerFollowPath;
        public static bool sneakerFollowPath;
        public static bool heavyFollowPath;
        public static bool sentryFollowPath;

        //variables to hold core attack bool
        public static bool swarmerAttackCore;
        public static bool gunnerAttackCore;
        public static bool sneakerAttackCore;
        public static bool heavyAttackCore;
        public static bool sentryAttackCore;

        //variables to hold ranged attack bool
        public static bool swarmerRanged;
        public static bool gunnerRanged;
        public static bool sneakerRanged;
        public static bool heavyRanged;
        public static bool sentryRanged;

        //variables to hold melee attack bool
        public static bool swarmerMelee;
        public static bool gunnerMelee;
        public static bool sneakerMelee;
        public static bool heavyMelee;
        public static bool sentryMelee;


        public static void load() //using xpath
        {
            XPathDocument file = new XPathDocument(@"Content/Settings/enemyBehaviours.xml");
            XPathNavigator nav = file.CreateNavigator(); 
            XPathNodeIterator iterator = nav.Select("//swarmer"); // Look at the swarmer tag?
            
            if (iterator.Count > 0)
            {
                String behaviour = "";
                while (iterator.MoveNext())
                {
                    behaviour = iterator.Current.Value;
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
                String behaviour = "";

                while (iterator.MoveNext())
                {
                    behaviour = iterator.Current.Value;
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
                String behaviour = "";

                while (iterator.MoveNext())
                {
                    behaviour = iterator.Current.Value;
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
                String behaviour = "";

                while (iterator.MoveNext())
                {
                    behaviour = iterator.Current.Value;
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


            // This fucntions is going to be so fucking huge it makes me cry
            // placeholder default hardcoded values

            swarmerBehaviour = Behaviour.ShortestPathFirst;
            gunnerBehaviour = Behaviour.AvoidTurretDamage;
            sneakerBehaviour = Behaviour.AvoidPlayerDamage;
            heavyBehaviour = Behaviour.ShortestPathFirst;
            sentryBehaviour = Behaviour.ShortestPathFirst;

            swarmerPlayerChase = true;
            swarmerPlayerCharge = false;
            swarmerPlayerAttack = true;
            swarmerFollowPath = true;
            swarmerMelee = true;
            swarmerRanged = false;
            swarmerAttackCore = true;

            gunnerAttackCore = true;
            gunnerFollowPath = true;
            gunnerMelee = true;
            gunnerPlayerAttack = true;
            gunnerPlayerChase = false;
            gunnerPlayerCharge = false;
            gunnerRanged = true;

            sneakerAttackCore = true;
            sneakerFollowPath = true;
            sneakerMelee = true;
            sneakerPlayerAttack = true;
            sneakerPlayerChase = false;
            sneakerPlayerCharge = true;
            sneakerRanged = false;

            heavyAttackCore = true;
            heavyFollowPath = true;
            heavyMelee = true;
            heavyPlayerAttack = true;
            heavyPlayerChase = false;
            heavyPlayerCharge = false;
            heavyRanged = false;

            sentryAttackCore = false;
            sentryFollowPath = false;
            sentryMelee = false;
            sentryPlayerAttack = true;
            sentryPlayerChase = false;
            sentryPlayerCharge = false;
            sentryRanged = true;
        }

    }
}
