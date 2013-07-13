using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BarbieBear
{
    public class BackgroundScenery
    {
        public World World { get; set; }
        List<BackgroundBarbie> bgBarbies = new List<BackgroundBarbie>();

        public EmotionalState EmotionalState { get; set; }

        public static Vector2 MinPos = new Vector2(100, 200);
        public static Vector2 MaxPos = new Vector2(World.Width - 100, World.BaseLine - 50);

        public BackgroundScenery(World world)
        {
            this.World = world;
            this.EmotionalState = EmotionalState.Happy;

            //// bg barbies
            //for (int i = 0; i < MathExtra.RandomInt(8, 12); i++)
            //{
            //    Vector2 sceneryPos = MathExtra.RandomFloat()*new Vector2(World.Width, World.Height);

            //    sceneryPos = Vector2.Clamp(sceneryPos, new Vector2(100, 200),
            //                               new Vector2(World.Width - 100, World.BaseLine - 50));
            //    int bgLevel = (int) Math.Round(MathHelper.Lerp(3, 0, (sceneryPos.Y - 200)/(World.BaseLine - 50 - 200)));

            //    this.bgBarbies.Add(new BackgroundBarbie(this.World, sceneryPos, bgLevel));
            //}
        }

        public void Make(EmotionalState state)
        {
            this.EmotionalState = state;

            foreach (var backgroundBarby in bgBarbies)
            {
                backgroundBarby.EmotionalState = state;
            }
        }

        public void Update()
        {
            //foreach (var backgroundBarbie in this.bgBarbies)
            //{
            //    backgroundBarbie.Update();
            //}
        }

        public void Draw(SpriteBatch spr)
        {
            spr.Draw(TextureBin.Get("background"), Vector2.Zero, Color.White);
            //foreach (var backgroundBarbie in this.bgBarbies)
            //{
            //    backgroundBarbie.Draw(spr);
            //}
        }
    }
}