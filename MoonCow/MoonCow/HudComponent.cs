using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class HudComponent
    {
        public Hud hud { get; protected set; }
        public SpriteFont font { get; protected set; }
        public Game1 game { get; protected set; }
        public float wakeTime { get; protected set; }
        public float wakeThresh { get; protected set; }

        public HudComponent(Hud hud, SpriteFont font, Game1 game)
        {
            this.hud = hud;
            this.font = font;
            this.game = game;
        }

        public virtual void Update()
        {
            if(wakeTime < wakeThresh)
                wakeTime += Utilities.deltaTime;
        }

        public virtual void Draw(SpriteBatch sb) { }
    
        public virtual void Wake()
        {
            wakeTime = 0;
        }
    }
}
