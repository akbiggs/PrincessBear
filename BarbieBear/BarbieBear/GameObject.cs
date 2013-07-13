using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BarbieBear
{
    public class GameObject
    {
        public World World { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Size { get; set; }
        public float SizeDelta { get; set; }
        public Vector2 Velocity { get; set; }
        public Color Color { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }

        public float Scale { get; set; }
        public float RotationDelta { get; set; }

        public FacingDirection FacingDirection { get; set; }

        public Vector2 Center { get { return Position + (new Vector2(Texture.Width, Texture.Height)*Scale)/2; } }

        public virtual bool ShouldRemove { get { return false; } }

        public List<AnimationSet> Animations { get; set; }
        public AnimationSet CurAnimation { get; set; }

        public GameObject(World world, Vector2 position, Vector2 size, Texture2D texture)
        {
            this.World = world;
            this.Position = position;
            this.Size = size;
            this.Texture = texture;
            this.RotationDelta = 0;
            this.Origin = Vector2.Zero;

            this.Scale = 1;
            this.Color = Color.White;

            this.FacingDirection = FacingDirection.Left;

            this.Animations = new List<AnimationSet>();
            this.CurAnimation = null;
        }

        public virtual void Update()
        {
            if (!this.ShouldRemove)
            {
                if (this.CurAnimation != null)
                    this.CurAnimation.Update();
                this.Position += this.Velocity;
                this.Rotation += this.RotationDelta;
                this.Size += new Vector2(this.SizeDelta);
            }
            else
            {
                World.Remove(this);
            }
        }

        public virtual void Draw(SpriteBatch spr)
        {
            Texture2D texture = CurAnimation == null ? Texture : CurAnimation.GetTexture();
            Rectangle? sourceRect = CurAnimation == null ? (Rectangle?) null : CurAnimation.GetFrameRect();

            spr.Draw(texture, this.Position,
                     sourceRect, this.Color, this.Rotation, this.Origin, this.Size, 
                     this.FacingDirection == FacingDirection.Left ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }

    }

    public enum FacingDirection
    {
        Left, Right
    }
}