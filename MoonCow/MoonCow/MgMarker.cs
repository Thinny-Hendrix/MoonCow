using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class MgMarker
    {
        Vector2 pos;
        CircleCollider col;
        Vector2 dir;
        enum Type{up, down, left, right}
        Type type;
        MgManager manager;
        CircleCollider goalCol;
        float spriteAngle;
        bool hitGoal;
        public bool passedGoal;
        public float distFromGoal;

        public MgMarker(MgManager manager, Vector2 pos, int type)
        {
            this.manager = manager;
            this.pos = pos;
            this.type = (Type)type;
            setDir();

            col = new CircleCollider(pos, 64);
        }

        void setDir()
        {
            switch((int)type)
            {
                default:
                    dir = new Vector2(0, -1);
                    goalCol = manager.upGoal;
                    spriteAngle = MathHelper.PiOver2;
                    break;
                case 1:
                    dir = new Vector2(0, 1);
                    goalCol = manager.downGoal;
                    spriteAngle = -MathHelper.PiOver2;
                    break;
                case 2:
                    dir = new Vector2(-1, 0);
                    goalCol = manager.leftGoal;
                    spriteAngle = 0;
                    break;
                case 3:
                    dir = new Vector2(1, 0);
                    goalCol = manager.rightGoal;
                    spriteAngle = MathHelper.Pi;
                    break;
            }
        }

        public void Update()
        {
            pos += dir * manager.speed * Utilities.deltaTime;
            col.Update(pos);
            distFromGoal = goalCol.distFrom(pos);
            if (!hitGoal)
            {
                if (goalCol.checkCircle(col))
                    hitGoal = true;
            }
            else
            {
                if (!goalCol.checkCircle(col))
                    passedGoal = true;
            }
            if (passedGoal && goalCol.distFrom(pos) > 20)
            {
                manager.miss();
                manager.mToDelete.Add(this);
            }
        }


        public void hit()
        {
            float dist = goalCol.distFrom(pos);
            if (dist < 200)
            {
                if (dist > 150)
                    manager.miss();
                else if (dist > 100)
                {
                    //okay
                    manager.particles.Add(new SpRing(pos, 1, manager));
                }
                else if (dist > 75)
                {
                    //good
                    manager.particles.Add(new SpRing(pos, 2, manager));
                }
                else if (dist > 40)
                {
                    //great
                    manager.particles.Add(new SpRing(pos, 3, manager));
                }
                else //dist <= 40
                {
                    //perfect
                    manager.particles.Add(new SpRing(goalCol.centre, manager.speed/500*7, manager));
                    manager.particles.Add(new SpRing(goalCol.centre, manager.speed / 500 * 14, manager));
                }
                manager.mToDelete.Add(this);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(manager.markerSprite, new Rectangle((int)pos.X, (int)pos.Y, manager.markerSprite.Bounds.Width, manager.markerSprite.Bounds.Height), null, Color.White, spriteAngle, new Vector2(77), SpriteEffects.None, 0);
            //sb.DrawString(manager.font, ""+distFromGoal, new Vector2((int)pos.X, (int)pos.Y), Color.White);
        }
    }
}
