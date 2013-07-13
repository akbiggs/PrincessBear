using System;
using Microsoft.Xna.Framework;

namespace BarbieBear
{
    public class Bear : GameObject
    {
        public float MoveSpeed;
        public AttackDirection AttackDirection { get; set; }
        public Vector2 MoveDirection { get; set; }
        public bool Dead { get; set; }
        public override bool ShouldRemove
        {
            get { return this.Dead && (this.Size.X > 300 || this.Size.X < 10); }
        }

        public Bear(World world, AttackDirection direction) : base(world, Vector2.Zero, new Vector2(0.5f), TextureBin.Pixel)
        {
            this.AttackDirection = direction;
            this.Dead = false;

            this.CurAnimation = new AnimationSet("running", TextureBin.Get("bear"), 12, 235, 277, 2, 4, true, 0);

            this.Scale = 0.5f;
            this.Position -= new Vector2((Texture.Width*Scale)/2, Texture.Height*Scale);

            this.MoveSpeed = 5;

            switch (direction)
            {
                case AttackDirection.Left: this.MoveDirection = new Vector2(1, 0);
                    this.Position = new Vector2(-Size.X - 150, World.BaseLine - Size.Y - 140);
                    this.FacingDirection = FacingDirection.Right;
                    this.Size = new Vector2(1);
                    break;
                case AttackDirection.UpperLeft: this.MoveDirection = new Vector2(0.5f, 0.5f);
                    this.Position = new Vector2(-Size.X - 10, World.BaseLine - World.Height/2 - Size.Y*3 - 300);
                    this.MoveSpeed = 8;
                    this.SizeDelta = 0.005f;
                    this.FacingDirection = FacingDirection.Right;
                    break;
                case AttackDirection.Top: this.MoveDirection = new Vector2(0, 1);
                    this.Position = new Vector2(World.Width/2 - Size.X/2 - 100, -80);
                    this.SizeDelta = 0.008f;
                    this.MoveSpeed = 3.5f;
                    break;
                case AttackDirection.UpperRight: this.MoveDirection = new Vector2(-0.5f, 0.5f);
                    this.Position = new Vector2(World.Width + Size.X + 10, World.BaseLine - World.Height/2 - Size.Y*3 - 380);
                    this.SizeDelta = 0.005f;
                    this.MoveSpeed = 8;
                    break;
                case AttackDirection.Right: this.MoveDirection = new Vector2(-1, 0);
                    this.Position = new Vector2(World.Width + Size.X + 50, World.BaseLine - Size.Y - 140);
                    this.Size = new Vector2(1);
                    break;
                case AttackDirection.None: 
                    throw new InvalidOperationException("BAD DIRECTION");
                    break;
            }

            int speedBoostIncrease = (int) MathHelper.Clamp((int) Engine.Score/2000, 0, 6);
            MoveSpeed += speedBoostIncrease;
        }

        public override void Update()
        {
            this.Velocity = this.MoveDirection*this.MoveSpeed;
            base.Update();
        }

        public void Die()
        {
            if (!this.Dead)
            {
                TextureBin.PlaySound("punch" + (MathExtra.RandomInt(4) + 1));

                this.MoveDirection = -this.MoveDirection +
                                     new Vector2(MathHelper.Clamp(MathExtra.RandomFloat(), 0.2f, 0.8f) *
                                                 (MathExtra.RandomBool() ? -1 : 1));
                if (Vector2.Distance(this.MoveDirection, Vector2.Zero) < 0.01)
                {
                    this.MoveDirection = new Vector2(0.5f);
                }
                this.MoveSpeed = MathExtra.RandomInt(10, 20);
                //this.Color = Color.Red;
                Engine.Score += 50;
                this.RotationDelta = MathHelper.PiOver4/8;
                this.Origin = new Vector2(50f, 50f);
                //this.Position += Size/2;
                this.SizeDelta = MathExtra.RandomFloat()*0.0005f*MathExtra.RandomSign();
                this.Dead = true;

                var particle = new Particle(World, this.Position + new Vector2(50), new Vector2(1), TextureBin.Get("Pixel"),
                    Vector2.Zero, 20);
                World.Add(particle);

                particle.CurAnimation = new AnimationSet("sparkle", TextureBin.Get("sparkle"), 5, 163, 162, 2, -1, false, 0);
            }
        }
    }
}