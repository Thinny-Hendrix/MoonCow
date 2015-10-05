using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class MgMessage
    {
        public Vector2 pos;
        string text;
        float alpha;
        float time;
        List<MgMessage> toDelete;
        public MgMessage(string text, Vector2 pos, List<MgMessage> toDelete)
        {
            this.pos = pos;
            this.text = text;
            this.toDelete = toDelete;
            alpha = 1;
        }

        public void Update()
        {
            time += Utilities.deltaTime;
            if (time > 1)
                toDelete.Add(this);
            else
            {
                if (time > 0.5f)
                    alpha = MathHelper.Lerp(1, 0, (time - 0.5f) * 2);
            }
        }

        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            sb.DrawString(font, text, pos, Color.White * alpha, 0,
                new Vector2(font.MeasureString(text).X/2, font.MeasureString(text).Y / 2), 30.0f / 40, SpriteEffects.None, 0);
        }
    }
}
