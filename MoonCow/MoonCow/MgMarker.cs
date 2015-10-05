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
        public Vector2 pos;
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
            if (passedGoal && goalCol.distFrom(pos) > 100)
            {
                manager.miss();
                manager.mToDelete.Add(this);
                manager.addParticle(new SpDot(pos, manager.speed / 500, manager.screen.pToDelete), 1);
            }
        }


        public void hit()
        {
            float dist = goalCol.distFrom(pos);
            if (dist < 200)
            {
                if (dist > 150)
                {
                    manager.miss();
                    manager.addParticle(new SpDot(pos, manager.speed / 500, manager.screen.pToDelete), 1);
                }
                else if (dist > 100)
                {
                    //okay
                    manager.addParticle(new SpRing(pos, manager.speed / 500, manager.screen.pToDelete));
                    manager.addParticle(new SpDot(pos, manager.speed / 500, manager.screen.pToDelete), 1);
                    manager.hit(5);
                    manager.addMessage("okay");
                }
                else if (dist > 75)
                {
                    //good
                    manager.addParticle(new SpRing(pos, manager.speed / 500 * 2, manager.screen.pToDelete));
                    manager.addParticle(new SpDot(pos, manager.speed / 500, manager.screen.pToDelete), 1);
                    manager.hit(10);
                    manager.addMessage("good");
                }
                else if (dist > 40)
                {
                    //great
                    manager.addParticle(new SpRing(pos, manager.speed / 500 * 3, manager.screen.pToDelete));
                    manager.addParticle(new SpDot(pos, manager.speed / 500, manager.screen.pToDelete), 1);
                    manager.hit(15);
                    manager.addMessage("great");
                }
                else //dist <= 40
                {
                    //perfect
                    manager.addParticle(new SpRing(goalCol.centre, manager.speed / 500 * 7, manager.screen.pToDelete, 1));
                    manager.addParticle(new SpRing(goalCol.centre, manager.speed / 500 * 14, manager.screen.pToDelete, 1));
                    manager.addParticle(new SpDot(goalCol.centre, manager.speed / 500, manager.screen.pToDelete), 1);
                    manager.hit(25);
                    manager.addMessage("perfect!");
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
