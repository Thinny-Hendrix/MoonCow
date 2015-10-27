using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public static class TextureManager
    {
        //stores a whole bunch of loaded textures that many things use, prevents needing to load it individually each time

        public static Effect alphaMap;

        public static Texture2D particle1;
        public static Texture2D particle2;
        public static Texture2D particle3;
        public static Texture2D glowStreak1;
        public static Texture2D glowStreak2;
        public static Texture2D glowS_0;
        public static Texture2D glowS_1;
        public static Texture2D glowS_2;
        public static Texture2D glowS_3;
        public static Texture2D glowC_0;
        public static Texture2D glowC_1;
        public static Texture2D glowC_2;
        public static Texture2D glowC_3;
        public static Texture2D spinGlow;
        public static Texture2D spinGlow_d;

        public static Texture2D vortLines;
        public static Texture2D vortBack;
        public static Texture2D vortSpir;



        public static Texture2D particle1small;
        public static Texture2D particle2small;
        public static Texture2D particle3small;

        public static Texture2D whiteBurst;
        public static Texture2D gibGlow;
        public static Texture2D spark1;

        public static Texture2D smoke1;
        public static Texture2D boostFlame;
        public static Texture2D shotBlip;
        public static Texture2D smallDot;

        public static Texture2D station1;


        public static Model square;
        public static Model flameSquare;
        public static Model dirSquare;

        //station window
        public static RenderTarget2D baseWindow;
        public static Texture2D windowLines;
        public static Texture2D warnLines;
        public static Texture2D warnSign2;
        public static SpriteBatch spriteBatch;
        static Vector2 linespos;
        static Color windowIdle1;

        //forcefield
        public static Texture2D forceLines;
        public static Texture2D forceVing;
        public static Texture2D drillHolo;

        //asteroid field
        public static Texture2D ast1;
        public static Texture2D skyBox2;
        public static Texture2D astCloud1;
        public static Texture2D astCloud2;

        //weapon model textures
        public static Texture2D bombTex1;

        //Turret model textures
        public static Texture2D turBase;
        public static Texture2D elecTurTex;
        public static Texture2D turLogoIn;
        public static Texture2D turLogoOut;

        //minigame
        public static Texture2D mgLines;
        public static Texture2D rbowRing;
        public static Texture2D mgPulse;
        public static Texture2D mgGrid;
        public static Texture2D mgBlue;
        public static Texture2D mgPink;
        public static Texture2D mgMarkWhite;
        public static Texture2D mgBack;
        public static Texture2D artefact;
        public static Texture2D bpCloud;
        public static Texture2D panel;


        //weapon effects
        public static Texture2D bombRing;
        public static Texture2D bombBlip;
        public static Texture2D bombRipple;
        public static Texture2D bombCenter;
        public static Texture2D bombRing2;
        public static Texture2D bombCenter2;
        public static Texture2D bombBlip2;

        public static Texture2D drawLine0;
        public static Texture2D drawLine1;
        public static Texture2D drawLine2;
        public static Texture2D drawLine3;
        public static Texture2D drawLine4;
        public static Texture2D drillMask;
        public static Texture2D spinL;
        public static Texture2D spinM;
        public static Texture2D spinS;
        public static Texture2D spinL128;
        public static Texture2D spinM128;
        public static Texture2D spinS128;

        //weapon icons
        public static Texture2D icoPew;
        public static Texture2D icoBomb;
        public static Texture2D icoAst;
        public static Texture2D icoWave;
        public static Texture2D icoDrill;
        public static Texture2D icoGat;
        public static Texture2D icoPyr;
        public static Texture2D icoEle;
        public static Texture2D icoX;
        public static Texture2D icoMon;
        public static Texture2D infinity;


        //electricity
        public static Texture2D elecL1;
        public static Texture2D elecL2;
        public static Texture2D elecL3;
        public static Texture2D elecL4;
        public static Texture2D elecE1;
        public static Texture2D elecE2;
        public static Texture2D elecE3;
        public static Texture2D elecE4;
        public static Texture2D elecRound64;
        public static Texture2D elecRound;
        public static Texture2D elecTrail;
        public static Texture2D elecTrail64;

        //fire
        public static Texture2D pyroFlame;

        //money gibs
        public static Texture2D gib1_0;
        public static Texture2D gib1_1;
        public static Texture2D gib1_2;
        public static Texture2D gib1_3;
        public static Texture2D gib1_s;
        public static Texture2D gibOre1;
        public static Texture2D polyGib;
        public static Texture2D chip;

        //enemies
        public static Texture2D swarmerTex;
        public static Texture2D sneTex;
        public static Texture2D gunTex;
        public static Texture2D hevTex;

        //sentry
        public static Texture2D sentryBod;
        public static Texture2D sEye0;
        public static Texture2D sEye1;
        public static Texture2D sEye2;
        public static Texture2D sEye3;
        public static Texture2D sEye4;
        public static Texture2D sEye5;
        public static Texture2D sEye6;

        //ship
        public static Texture2D ship;
        public static Texture2D screenPulse;



        public static Texture2D pureWhite;

        public static Texture2D railMask;
        public static Texture2D railHolo;
        public static RenderTarget2D railTarg;
        public static Vector2 holoPos;

        public static void initialize(Game game)
        {
            alphaMap = game.Content.Load<Effect>(@"Effects/AlphaMap");

            //station window
            pureWhite = new Texture2D(game.GraphicsDevice, 1, 1);
            pureWhite.SetData(new Color[] { Color.White });
            baseWindow = new RenderTarget2D(game.GraphicsDevice, 384, 640);
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            linespos = new Vector2(0, -16);
            windowIdle1 = new Color(0, 200, 255);


            //loading textures
            particle1 = game.Content.Load<Texture2D>(@"Models/Effects/tex1");
            particle2 = game.Content.Load<Texture2D>(@"Models/Effects/tex2");
            particle3 = game.Content.Load<Texture2D>(@"Models/Effects/tex3");
            glowStreak1 = game.Content.Load<Texture2D>(@"Models/Effects/glowStreak");
            glowStreak2 = game.Content.Load<Texture2D>(@"Models/Effects/glowStreak2");
            glowS_0 = game.Content.Load<Texture2D>(@"Models/Effects/Enemy/glowStreak-0");
            glowS_1 = game.Content.Load<Texture2D>(@"Models/Effects/Enemy/glowStreak-1");
            glowS_2 = game.Content.Load<Texture2D>(@"Models/Effects/Enemy/glowStreak-2");
            glowS_3 = game.Content.Load<Texture2D>(@"Models/Effects/Enemy/glowStreak-3");
            glowC_0 = game.Content.Load<Texture2D>(@"Models/Effects/Enemy/glowCent_0");
            glowC_1 = game.Content.Load<Texture2D>(@"Models/Effects/Enemy/glowCent_1");
            glowC_2 = game.Content.Load<Texture2D>(@"Models/Effects/Enemy/glowCent_2");
            glowC_3 = game.Content.Load<Texture2D>(@"Models/Effects/Enemy/glowCent_3");
            spinGlow = game.Content.Load<Texture2D>(@"Models/Effects/Enemy/spinGlow256");
            spinGlow_d = game.Content.Load<Texture2D>(@"Models/Effects/Enemy/spinGlow_d");

            vortLines = game.Content.Load<Texture2D>(@"Models/Enemies/SpawnPoint/vortLines");
            vortSpir = game.Content.Load<Texture2D>(@"Models/Enemies/SpawnPoint/vortFull");
            vortBack = game.Content.Load<Texture2D>(@"Models/Enemies/SpawnPoint/vortBack");

            particle1small = game.Content.Load<Texture2D>(@"Models/Effects/tex1-64");
            particle2small = game.Content.Load<Texture2D>(@"Models/Effects/tex2-64");
            particle3small = game.Content.Load<Texture2D>(@"Models/Effects/tex3-64");

            gibGlow = game.Content.Load<Texture2D>(@"Models/Effects/gibglow");
            whiteBurst = game.Content.Load<Texture2D>(@"Models/Effects/burstWhite");
            spark1 = game.Content.Load<Texture2D>(@"Models/Effects/spark1");
            smoke1 = game.Content.Load<Texture2D>(@"Models/Effects/smoke1");
            boostFlame = game.Content.Load<Texture2D>(@"Models/Effects/boostFlame");
            smallDot = game.Content.Load<Texture2D>(@"Models/Effects/smallDot");
            shotBlip = game.Content.Load<Texture2D>(@"Models/Effects/shotBlip");

            station1 = game.Content.Load<Texture2D>(@"Models/StationTiles/station5");

            windowLines = game.Content.Load<Texture2D>(@"Models/StationTiles/Window/screenlines");
            warnLines = game.Content.Load<Texture2D>(@"Models/StationTiles/Window/windowWarnLines");
            warnSign2 = game.Content.Load<Texture2D>(@"Models/StationTiles/Window/windowWarnSign");

            //forcefield
            forceLines = game.Content.Load<Texture2D>(@"Models/Base/forceLines");
            forceVing = game.Content.Load<Texture2D>(@"Models/Base/forceVing");
            drillHolo = game.Content.Load<Texture2D>(@"Models/Base/drillHolo");

            //ast field
            skyBox2 = game.Content.Load<Texture2D>(@"Models/Misc/Skybox/skybox6");
            ast1 = game.Content.Load<Texture2D>(@"Models/AstField/ast1tex_0");
            astCloud1 = game.Content.Load<Texture2D>(@"Models/AstField/astCloud");
            astCloud2 = game.Content.Load<Texture2D>(@"Models/AstField/astCloud2");


            //weapon model textures
            bombTex1 = game.Content.Load<Texture2D>(@"Models/Weapons/bombt");

            //turret model textures
            turBase = game.Content.Load<Texture2D>(@"Models/Turrets/turrBase_0");
            elecTurTex = game.Content.Load<Texture2D>(@"Models/Turrets/elecTurt_0");
            turLogoIn = game.Content.Load<Texture2D>(@"Models/Turrets/tLogIn");
            turLogoOut = game.Content.Load<Texture2D>(@"Models/Turrets/tLogOut");

            //minigame
            mgLines = game.Content.Load<Texture2D>(@"Minigame/mgRimLines");
            rbowRing = game.Content.Load<Texture2D>(@"Minigame/rBowRing");
            mgPulse = game.Content.Load<Texture2D>(@"Minigame/pulse");
            mgGrid = game.Content.Load<Texture2D>(@"Minigame/grid");
            mgBlue = game.Content.Load<Texture2D>(@"Minigame/bluePoly");
            mgPink = game.Content.Load<Texture2D>(@"Minigame/pinkPoly");
            mgMarkWhite = game.Content.Load<Texture2D>(@"Minigame/mgMarkerW");
            mgBack = game.Content.Load<Texture2D>(@"Minigame/mgBack");
            artefact = game.Content.Load<Texture2D>(@"Minigame/polyt");
            bpCloud = game.Content.Load<Texture2D>(@"Minigame/bpCloud");
            panel = game.Content.Load<Texture2D>(@"Minigame/panelt");

            //weapon effects
            bombRing = game.Content.Load<Texture2D>(@"Models/Weapons/sploring");
            bombBlip = game.Content.Load<Texture2D>(@"Models/Weapons/sploblip");
            bombRipple = game.Content.Load<Texture2D>(@"Models/Weapons/bombRipple");
            bombCenter = game.Content.Load<Texture2D>(@"Models/Weapons/bombcenter");
            bombRing2 = game.Content.Load<Texture2D>(@"Models/Weapons/bombRing");
            bombCenter2 = game.Content.Load<Texture2D>(@"Models/Weapons/bombCenter2");
            bombBlip2 = game.Content.Load<Texture2D>(@"Models/Weapons/bombBlip2");


            spinL = game.Content.Load<Texture2D>(@"Models/Weapons/spinL");
            spinM = game.Content.Load<Texture2D>(@"Models/Weapons/spinM");
            spinS = game.Content.Load<Texture2D>(@"Models/Weapons/spinS");
            spinL128 = game.Content.Load<Texture2D>(@"Models/Weapons/spinL128");
            spinM128 = game.Content.Load<Texture2D>(@"Models/Weapons/spinM128");
            spinS128 = game.Content.Load<Texture2D>(@"Models/Weapons/spinS128");

            drawLine0 = game.Content.Load<Texture2D>(@"Models/Weapons/Drill/drawLine0");
            drawLine1 = game.Content.Load<Texture2D>(@"Models/Weapons/Drill/drawLine1");
            drawLine2 = game.Content.Load<Texture2D>(@"Models/Weapons/Drill/drawLine2");
            drawLine3 = game.Content.Load<Texture2D>(@"Models/Weapons/Drill/drawLine3");
            drawLine4 = game.Content.Load<Texture2D>(@"Models/Weapons/Drill/drawLine4");
            drillMask = game.Content.Load<Texture2D>(@"Models/Weapons/Drill/drillMask");



            //weapon icons
            icoPew = game.Content.Load<Texture2D>(@"Hud/QuickSelect/icoPew");
            icoBomb = game.Content.Load<Texture2D>(@"Hud/QuickSelect/icoBomb");
            icoAst = game.Content.Load<Texture2D>(@"Hud/QuickSelect/icoAst");
            icoWave = game.Content.Load<Texture2D>(@"Hud/QuickSelect/icoWave");
            icoDrill = game.Content.Load<Texture2D>(@"Hud/QuickSelect/icoDrill");

            icoEle = game.Content.Load<Texture2D>(@"Hud/QuickSelect/icoEle");
            icoPyr = game.Content.Load<Texture2D>(@"Hud/QuickSelect/icoPyr");
            icoX = game.Content.Load<Texture2D>(@"Hud/QuickSelect/icoX");
            icoMon = game.Content.Load<Texture2D>(@"Hud/QuickSelect/icoMon");

            infinity = game.Content.Load<Texture2D>(@"Hud/inf");


            //electricity textures
            elecL1 = game.Content.Load<Texture2D>(@"Models/Effects/Electro/elecL1");
            elecL2 = game.Content.Load<Texture2D>(@"Models/Effects/Electro/elecL2");
            elecL3 = game.Content.Load<Texture2D>(@"Models/Effects/Electro/elecL3");
            elecL4 = game.Content.Load<Texture2D>(@"Models/Effects/Electro/elecL4");
            elecE1 = game.Content.Load<Texture2D>(@"Models/Effects/Electro/elecE1");
            elecE2 = game.Content.Load<Texture2D>(@"Models/Effects/Electro/elecE2");
            elecE3 = game.Content.Load<Texture2D>(@"Models/Effects/Electro/elecE3");
            elecE4 = game.Content.Load<Texture2D>(@"Models/Effects/Electro/elecE4");
            elecRound = game.Content.Load<Texture2D>(@"Models/Effects/Electro/elecRound");
            elecRound64 = game.Content.Load<Texture2D>(@"Models/Effects/Electro/elecRound64");
            elecTrail = game.Content.Load<Texture2D>(@"Models/Effects/Electro/elecTrail");
            elecTrail64 = game.Content.Load<Texture2D>(@"Models/Effects/Electro/elecTrail64");

            //fire
            pyroFlame = game.Content.Load<Texture2D>(@"Models/Effects/Pyro/fire1");

            //money gibs
            gib1_0 = game.Content.Load<Texture2D>(@"Models/MoneyGibs/gib1-0");
            gib1_1 = game.Content.Load<Texture2D>(@"Models/MoneyGibs/gib1-1");
            gib1_2 = game.Content.Load<Texture2D>(@"Models/MoneyGibs/gib1-2");
            gib1_3 = game.Content.Load<Texture2D>(@"Models/MoneyGibs/gib1-3");
            gib1_s = game.Content.Load<Texture2D>(@"Models/MoneyGibs/gib1-s");
            chip = game.Content.Load<Texture2D>(@"Minigame/chipt_0");
            polyGib = game.Content.Load<Texture2D>(@"Models/MoneyGibs/polygibt_0");
            gibOre1 = game.Content.Load<Texture2D>(@"Models/MoneyGibs/oreGold");


            //enemies
            swarmerTex = game.Content.Load<Texture2D>(@"Models/Enemies/Swarmer/swat_0");
            sneTex = game.Content.Load<Texture2D>(@"Models/Enemies/Sneaker/sneakt_0");
            gunTex = game.Content.Load<Texture2D>(@"Models/Enemies/Gunner/gunt_0");
            hevTex = game.Content.Load<Texture2D>(@"Models/Enemies/Heavy/hev1t_0");


            //sentry
            sentryBod = game.Content.Load<Texture2D>(@"Models/Enemies/Sentry/sentryt");
            sEye0 = game.Content.Load<Texture2D>(@"Models/Enemies/Sentry/sentEyes0");
            sEye1 = game.Content.Load<Texture2D>(@"Models/Enemies/Sentry/sentEyes1");
            sEye2 = game.Content.Load<Texture2D>(@"Models/Enemies/Sentry/sentEyes2");
            sEye3 = game.Content.Load<Texture2D>(@"Models/Enemies/Sentry/sentEyes3");
            sEye4 = game.Content.Load<Texture2D>(@"Models/Enemies/Sentry/sentEyes4");
            sEye5 = game.Content.Load<Texture2D>(@"Models/Enemies/Sentry/sentEyes5");
            sEye6 = game.Content.Load<Texture2D>(@"Models/Enemies/Sentry/sentEyes6");

            //ship
            ship = game.Content.Load<Texture2D>(@"Models/Ship/shipDt");
            screenPulse = game.Content.Load<Texture2D>(@"Models/Ship/screenpulse2");



            square = game.Content.Load<Model>(@"Models/Misc/square");
            flameSquare = game.Content.Load<Model>(@"Models/Effects/flameSquare");
            dirSquare = game.Content.Load<Model>(@"Models/Misc/dirSquare");

            railMask = game.Content.Load<Texture2D>(@"Models/Rails/railMask2");
            railHolo = game.Content.Load<Texture2D>(@"Models/Rails/railholo");
            railTarg = new RenderTarget2D(game.GraphicsDevice, 512, 512);
        }

        public static void Update(Game1 game)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                linespos.Y -= Utilities.deltaTime * 32;
                if (linespos.Y < -32)
                    linespos.Y += 16;
                game.GraphicsDevice.SetRenderTarget(baseWindow);

                spriteBatch.Begin();
                if (game.enemyManager.enemies.Count() != 0)
                {
                    spriteBatch.Draw(pureWhite, new Rectangle(0, 0, 384, 640), Color.Red * 0.6f);
                    spriteBatch.Draw(windowLines, linespos, Color.Red * 0.5f);
                    spriteBatch.Draw(warnLines, Vector2.Zero, Color.Red);
                    spriteBatch.Draw(warnSign2, Vector2.Zero, Color.White);
                    spriteBatch.Draw(warnSign2, Vector2.Zero, Color.White);

                }
                else
                {
                    spriteBatch.Draw(pureWhite, new Rectangle(0, 0, 384, 640), Color.Aqua * 0.4f);
                    spriteBatch.Draw(windowLines, linespos, windowIdle1);
                }
                spriteBatch.End();

                holoPos.Y -= Utilities.deltaTime * 32;
                if(holoPos.Y <= 0)
                {
                    holoPos.Y += 512;
                }
                game.GraphicsDevice.SetRenderTarget(railTarg);
                spriteBatch.Begin();
                spriteBatch.Draw(railHolo, holoPos, Color.White);
                spriteBatch.Draw(railHolo, holoPos + new Vector2(0,-512), Color.White);
                spriteBatch.Draw(railMask, Vector2.Zero, Color.White);
                spriteBatch.End();
                game.GraphicsDevice.SetRenderTarget(null);
            }


        }

    }
}
