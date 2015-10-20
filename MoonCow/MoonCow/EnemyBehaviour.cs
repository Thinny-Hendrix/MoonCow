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
            XPathNodeIterator iterator = nav.Select("//swarmer/pathing"); // Look at the swarmer tag?
            
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

            iterator = nav.Select("//swarmer/chasePlayer");
            while (iterator.MoveNext())
            {
                swarmerPlayerChase = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//swarmer/chargePlayer");
            while (iterator.MoveNext())
            {
                swarmerPlayerCharge = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//swarmer/attackPlayer");
            while (iterator.MoveNext())
            {
                swarmerPlayerAttack = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//swarmer/followPath");
            while (iterator.MoveNext())
            {
                swarmerFollowPath = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//swarmer/meleeAttack");
            while (iterator.MoveNext())
            {
                swarmerMelee = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//swarmer/rangedAttack");
            while (iterator.MoveNext())
            {
                swarmerRanged = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//swarmer/attackCore");
            while (iterator.MoveNext())
            {
                swarmerAttackCore = Boolean.Parse(iterator.Current.Value);
            }

            //Do the same for every other class




            iterator = nav.Select("//gunner/pathing");
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

            iterator = nav.Select("//gunner/chasePlayer");
            while (iterator.MoveNext())
            {
                gunnerPlayerChase = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//gunner/chargePlayer");
            while (iterator.MoveNext())
            {
                gunnerPlayerCharge = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//gunner/attackPlayer");
            while (iterator.MoveNext())
            {
                gunnerPlayerAttack = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//gunner/followPath");
            while (iterator.MoveNext())
            {
                gunnerFollowPath = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//gunner/meleeAttack");
            while (iterator.MoveNext())
            {
                gunnerMelee = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//gunner/rangedAttack");
            while (iterator.MoveNext())
            {
                gunnerRanged = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//gunner/attackCore");
            while (iterator.MoveNext())
            {
                gunnerAttackCore = Boolean.Parse(iterator.Current.Value);
            }


            iterator = nav.Select("//sneaker/pathing");
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

            iterator = nav.Select("//sneaker/chasePlayer");
            while (iterator.MoveNext())
            {
                sneakerPlayerChase = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//sneaker/chargePlayer");
            while (iterator.MoveNext())
            {
                sneakerPlayerCharge = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//sneaker/attackPlayer");
            while (iterator.MoveNext())
            {
                sneakerPlayerAttack = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//sneaker/followPath");
            while (iterator.MoveNext())
            {
                sneakerFollowPath = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//sneaker/meleeAttack");
            while (iterator.MoveNext())
            {
                sneakerMelee = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//sneaker/rangedAttack");
            while (iterator.MoveNext())
            {
                sneakerRanged = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//sneaker/attackCore");
            while (iterator.MoveNext())
            {
                sneakerAttackCore = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//heavy/pathing");
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

            iterator = nav.Select("//heavy/chasePlayer");
            while (iterator.MoveNext())
            {
                heavyPlayerChase = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//heavy/chargePlayer");
            while (iterator.MoveNext())
            {
                heavyPlayerCharge = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//heavy/attackPlayer");
            while (iterator.MoveNext())
            {
                heavyPlayerAttack = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//heavy/followPath");
            while (iterator.MoveNext())
            {
                heavyFollowPath = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//heavy/meleeAttack");
            while (iterator.MoveNext())
            {
                heavyMelee = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//heavy/rangedAttack");
            while (iterator.MoveNext())
            {
                heavyRanged = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//heavy/attackCore");
            while (iterator.MoveNext())
            {
                heavyAttackCore = Boolean.Parse(iterator.Current.Value);
            }



            //Sentry
            iterator = nav.Select("//sentry/pathing");
            if (iterator.Count > 0)
            {
                String behaviour = "";

                while (iterator.MoveNext())
                {
                    behaviour = iterator.Current.Value;
                }

                System.Diagnostics.Debug.WriteLine("Sentry behaviour  = " + behaviour);

                if (behaviour.Equals("AvoidTurretDamage"))
                {
                    sentryBehaviour = Behaviour.AvoidTurretDamage;
                }
                else
                {
                    if (behaviour.Equals("AvoidPlayerDamage"))
                    {
                        sentryBehaviour = Behaviour.AvoidPlayerDamage;
                    }
                    else
                    {
                        //shortest path
                        sentryBehaviour = Behaviour.ShortestPathFirst;
                    }
                }
            }
            else
            {
                Console.WriteLine("Sentry Behaviour Settings Error");
            }

            iterator = nav.Select("//sentry/chasePlayer");
            while (iterator.MoveNext())
            {
                sentryPlayerChase = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//sentry/chargePlayer");
            while (iterator.MoveNext())
            {
                sentryPlayerCharge = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//sentry/attackPlayer");
            while (iterator.MoveNext())
            {
                sentryPlayerAttack = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//sentry/followPath");
            while (iterator.MoveNext())
            {
                sentryFollowPath = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//sentry/meleeAttack");
            while (iterator.MoveNext())
            {
                sentryMelee = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//sentry/rangedAttack");
            while (iterator.MoveNext())
            {
                sentryRanged = Boolean.Parse(iterator.Current.Value);
            }

            iterator = nav.Select("//sentry/attackCore");
            while (iterator.MoveNext())
            {
                sentryAttackCore = Boolean.Parse(iterator.Current.Value);
            }

        }

    }
}
