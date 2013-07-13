using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace BarbieBear
{
    public class Barbie : GameObject
    {
        private static float LEFT_LOWER = -120;
        private static float LEFT_UPPER = -80;

        private static float UPPERLEFT_LOWER = LEFT_UPPER;
        private static float UPPERLEFT_UPPER = -30;

        private static float TOP_LOWER = UPPERLEFT_UPPER;
        private static float TOP_UPPER = 30;

        private static float UPPERRIGHT_LOWER = TOP_UPPER;
        private static float UPPERRIGHT_UPPER = 80;

        private static float RIGHT_LOWER = UPPERRIGHT_UPPER;
        private static float RIGHT_UPPER = 120;

        public AttackDirection CurAttack;

        public int AttackSoundTimer { get; set; }
        public bool CanPlayAttackSound { get { return AttackSoundTimer == 0; } }

        public bool Dead { get; set; }

        public Barbie(World world, Vector2 position) : base(world, position, new Vector2(0.75f), TextureBin.Get("barbie"))
        {
            this.Scale = 0.75f;
            this.Position -= new Vector2((Texture.Width*Scale)/2,
                                         200);
            //Texture.Height*Scale);
        }

        public override void Update()
        {
            if (Input.ScreenTapped && !Engine.PlayingIntro)
                this.LaunchAttack(Input.TapPosition);

            if (!CanPlayAttackSound)
                AttackSoundTimer--;

            base.Update();
        }

        private void LaunchAttack(Vector2 tapPosition)
        {
            float angle = MathHelper.ToDegrees((tapPosition - Center).ToAngle());

            if (LEFT_LOWER <= angle && angle <= LEFT_UPPER) LaunchAttack(AttackDirection.Left);
            else if (UPPERLEFT_LOWER <= angle && angle <= UPPERLEFT_UPPER) LaunchAttack(AttackDirection.UpperLeft);
            else if (TOP_LOWER <= angle && angle <= TOP_UPPER) LaunchAttack(AttackDirection.Top);
            else if (UPPERRIGHT_LOWER <= angle && angle <= UPPERRIGHT_UPPER) LaunchAttack(AttackDirection.UpperRight);
            else if (RIGHT_LOWER <= angle && angle <= RIGHT_UPPER) LaunchAttack(AttackDirection.Right);
        }

        private void LaunchAttack(AttackDirection direction)
        {
            string directionString;
            switch (direction)
            {
                case AttackDirection.Left:
                    directionString = "left";
                    break;
                case AttackDirection.UpperLeft:
                    directionString = "upleft";
                    break;
                case AttackDirection.Top:
                    directionString = "up";
                    break;
                case AttackDirection.UpperRight:
                    directionString = "upright";
                    break;
                case AttackDirection.Right:
                    directionString = "right";
                    break;
                default:
                    directionString = "left";
                    break;
            }

            CurAttack = direction;
            this.Texture = TextureBin.Get("barbie_" + directionString + "punch");

            World.Timers.Remove("attack");
            World.AddTimer(new Timer("attack", 30, () =>
                {
                    CurAttack = AttackDirection.None;
                    this.Texture = TextureBin.Get("barbie");
                }));

            if (CanPlayAttackSound)
            {
                TextureBin.PlaySound("barbie" + (MathExtra.RandomInt(0, 5) + 1));
            }
        }

        public void Die(Bear bear)
        {
            this.Origin = new Vector2(0.5f);
            this.Velocity = bear.MoveDirection * 10;
            this.Dead = true;
        }
    }
}