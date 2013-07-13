using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Linq.Expressions;

namespace BarbieBear
{
    public class World
    {
        public BufferedList<Barbie> Barbies { get; set; }
        public BufferedList<Bear> Bears { get; set; }
        public BufferedList<Particle> Particles { get; set; }

        public static int Width { get; set; }
        public static int Height { get; set; }

        public BackgroundScenery bgScenery { get; set; }

        public Dictionary<string, Timer> Timers { get; set; }
        public List<string> timersToRemove = new List<string>();
        private SpriteFont prettyFont;

        public static int BaseLine { get { return Height - 100; } }

        public World(int width, int height)
        {
            Width = width;
            Height = height;

            this.Barbies = new BufferedList<Barbie> { new Barbie(this, new Vector2(Width/2, BaseLine))};

            this.Bears = new BufferedList<Bear>();
                
            this.Particles = new BufferedList<Particle>();
            this.Timers = new Dictionary<string, Timer>();

            this.bgScenery = new BackgroundScenery(this);
            this.prettyFont = TextureBin.GetFont("PrettyFont");
        }

        public static AttackDirection RandomAttackDirection()
        {
            switch (MathExtra.RandomInt(5))
            {
                case 0: return AttackDirection.Left;
                case 1: return AttackDirection.UpperLeft;
                case 2: return AttackDirection.Top;
                case 3: return AttackDirection.UpperRight;
                case 4: return AttackDirection.Right;
                default:
                    return AttackDirection.Left;
            }
        }

        public void Update()
        {
            bgScenery.Update();
            foreach (var timer in Timers)
            {
                timer.Value.Update();
                if (timer.Value.Finished)
                {
                    RemoveTimer(timer.Value.Name);
                }
            }

            foreach (var timer in timersToRemove)
            {
                Timers.Remove(timer);
            }
            timersToRemove.Clear();

            if (!Timers.ContainsKey("spawnEnemies") && !Engine.PlayingIntro)
            {
                this.AddTimer(new Timer("spawnEnemies", MathExtra.RandomInt(50, 150), () => this.SpawnWave()));
            }

            foreach (var bear in this.Bears)
            {
                bool oldDead = bear.Dead;
                bear.Update();
                if (oldDead != bear.Dead)
                {
                    OnBearDead(bear);
                }
            }

            foreach (var barbie in this.Barbies)
            {
                barbie.Update();
            }

            foreach (var particle in Particles)
            {
                particle.Update();
            }

            this.Bears.ApplyBuffers();
            this.Barbies.ApplyBuffers();
            this.Particles.ApplyBuffers();
        }

        private void OnBearDead(Bear bear)
        {
            if (!this.Bears.Where((myBear) => { return myBear.Dead; }).Any())
            {
                this.bgScenery.Make(EmotionalState.Happy);
            }
        }

        private void RemoveTimer(string name)
        {
            timersToRemove.Add(name);
        }

        private void SpawnWave()
        {
            TextureBin.PlaySound("bear");
            
            for (int i = 0; i < MathExtra.RandomInt(3, 5); i++)
            {
                Bears.BufferAdd(new Bear(this, RandomAttackDirection()));
                if (bgScenery.EmotionalState != EmotionalState.Panicked && bgScenery.EmotionalState != EmotionalState.Sad)
                {
                    bgScenery.Make(EmotionalState.Panicked);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            bgScenery.Draw(spriteBatch);

            foreach (var barbie in this.Barbies)
            {
                barbie.Draw(spriteBatch);
            }

            foreach (var bear in this.Bears)
            {
                bear.Draw(spriteBatch);
                Barbie mainBarbie = Barbies[0];
                if (Vector2.DistanceSquared(bear.Position + new Vector2(50, 50), mainBarbie.Position + new Vector2(65)) <= 30000 &&
                    mainBarbie.CurAttack == bear.AttackDirection)
                {
                    bear.Die();
                }
                else if (Vector2.DistanceSquared(bear.Position + new Vector2(50, 50), mainBarbie.Position + new Vector2(65)) <= 10000 && !bear.Dead && !mainBarbie.Dead)

                {
                    mainBarbie.Die(bear);
                    this.bgScenery.Make(EmotionalState.Sad);

                    this.AddTimer(new Timer("allowRestart", 20, () =>
                        {
                            Engine.CanRestart = true;
                        }));
                }
            }


            foreach (var particle in Particles)
            {
                particle.Draw(spriteBatch);
            }

            if (Barbies[0] == null || Barbies[0].Dead)
            {
                string youLost = "~ You lost ~";
                spriteBatch.DrawString(this.prettyFont, youLost, new Vector2(Width/2 - prettyFont.MeasureString(youLost).X/2, 100),
                    Color.Black);

                string tap = "~ Tap to try again ~";
                spriteBatch.DrawString(this.prettyFont, tap, new Vector2(Width/2 - prettyFont.MeasureString(tap).X/2, 200), Color.Black);
            }

            spriteBatch.End();
        }

        public Timer AddTimer(Timer timer)
        {
            if (this.Timers.ContainsKey(timer.Name))
            {
                timer.Name = timer.Name + this.Timers.Count + 1;
            }

            this.Timers.Add(timer.Name, timer);

            return timer;
        }

        public void Add(GameObject gameObject)
        {
            if (gameObject is Bear)
            {
                this.Bears.BufferAdd((Bear)gameObject);
            }
            else if (gameObject is Barbie)
            {
                this.Barbies.BufferAdd((Barbie)gameObject);
            }
            else if (gameObject is Particle)
            {
                this.Particles.BufferAdd((Particle)gameObject);
            }
        }
        public void Remove(GameObject gameObject)
        {
            if (gameObject is Bear)
            {
                this.Bears.BufferRemove((Bear)gameObject);
            }
            else if (gameObject is Barbie)
            {
                this.Barbies.BufferRemove((Barbie)gameObject);
            }
            else if (gameObject is Particle)
            {
                this.Particles.BufferRemove((Particle)gameObject);
            }
        }
    }

}