using Microsoft.Xna.Framework;

namespace BarbieBear
{
    public class BackgroundBarbie : GameObject
    {
        public EmotionalState EmotionalState = EmotionalState.Happy;
        public int PanicTimer { get; set; }

        public BackgroundBarbie(World world, Vector2 position, int bgLevel) : base(world, position, new Vector2(32, 32), TextureBin.Pixel)
        {
            this.Color = Color.Yellow;

            switch (bgLevel)
            {
                case 0: this.Size = new Vector2(48, 48);
                    break;
                case 1: this.Size = new Vector2(32, 32);
                    break;
                case 2: this.Size = new Vector2(16, 16);
                    break;
                case 3: this.Size = new Vector2(8, 8);
                    break;
            }
        }

        public override void Update()
        {
            switch (this.EmotionalState)
            {
                case EmotionalState.Happy:
                    this.HappyUpdate();
                    break;
                case EmotionalState.Sad:
                    this.SadUpdate();
                    break;
                case EmotionalState.Panicked:
                    this.PanickedUpdate();
                    break;
            }

            base.Update();

            this.Position = Vector2.Clamp(this.Position, BackgroundScenery.MinPos, BackgroundScenery.MaxPos);
        }

        private void PanickedUpdate()
        {
            Color = Color.Red;

            if (this.ChangePanicDirection)
            {
                this.Velocity = new Vector2(MathExtra.RandomSign(), 0)*5;
                this.PanicTimer = MathExtra.RandomInt(20, 35);
            }
            else
            {
                this.PanicTimer--;
            }
        }

        protected bool ChangePanicDirection
        {
            get { return this.PanicTimer == 0; }
        }

        private void SadUpdate()
        {
            this.Velocity = Vector2.Zero;
            this.Color = Color.Blue;
        }

        private void HappyUpdate()
        {
            this.Color = Color.Yellow;
        }
    }

    public enum EmotionalState { Sad, Happy, Panicked }
}