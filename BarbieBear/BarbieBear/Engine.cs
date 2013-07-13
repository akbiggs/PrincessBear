using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace BarbieBear
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Engine : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int Score = 0;
        public static bool CanRestart { get; set; }
        public static bool PlayingIntro { get; set; }

        public Color ScoreFontColor;
        public Color PrettyFontColor;
        public Color AngryFontColor;

        public bool ShowedAngryText = false;
        public float AngryFontScale = 5f;

        private World world;
        private SpriteFont prettyFont;
        private SpriteFont angryFont;
        private int fadeTimer;

        protected bool FadingAway { get; set; }

        public Engine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";  

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

            CanRestart = false;
            PlayingIntro = true;
            IsMouseVisible = true;

            this.ScoreFontColor = Color.Transparent;
            this.PrettyFontColor = Color.Transparent;
            this.AngryFontColor = Color.Transparent;

            this.FadingAway = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureBin.LoadContent(Content);
            this.world = new World(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            this.prettyFont = this.Content.Load<SpriteFont>("Fonts/PrettyFont");
            this.angryFont = this.Content.Load<SpriteFont>("Fonts/AngryFont");
            
            MediaPlayer.Play(Content.Load<Song>("Sounds/song"));
            MediaPlayer.IsRepeating = true;
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            Input.Update();

            world.Update();

            if (CanRestart && Input.ScreenTapped)
            {
                world = new World(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                Score = 0;
                CanRestart = false;
            }

            if (PlayingIntro && !FadingAway)
            {
                PrettyFontColor = PrettyFontColor.PushTowards(Color.Black, 2);
                if (Input.ScreenTapped)
                {
                    PrettyFontColor.A = 255;
                }

                if (PrettyFontColor.A == 255 && !ShowedAngryText)
                {
                    AngryFontColor.A = 255;
                    AngryFontScale = AngryFontScale.PushTowards(1f, 0.2f);
                    if (Math.Abs(this.AngryFontScale - 1f) < 0.001f)
                    {
                        this.fadeTimer++;
                        this.FadingAway = true;
                    }
                }
            }
            else if (PlayingIntro)
            {
                this.fadeTimer++;
                if (fadeTimer > 100)
                {
                    PrettyFontColor = PrettyFontColor.PushTowards(Color.Transparent, 5);
                    AngryFontColor = AngryFontColor.PushTowards(Color.Transparent, 5);

                    if (PrettyFontColor.A == 0)
                    {
                        PlayingIntro = false;
                        FadingAway = false;
                    }
                    ScoreFontColor = ScoreFontColor.PushTowards(Color.Black, 5);
                }
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.world.Draw(spriteBatch);

            spriteBatch.Begin();

            string princessString = "~ Barbie Princess ~";
            spriteBatch.DrawString(Content.Load<SpriteFont>("Fonts/ScoreFont"), "Score: " + Score, new Vector2(30), ScoreFontColor);

            spriteBatch.DrawString(this.prettyFont, princessString, 
                new Vector2(World.Width/2 - this.prettyFont.MeasureString(princessString).X/2, 100), 
                PrettyFontColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            string bearBrawling = "BEAR BRAWLIN";
            spriteBatch.DrawString(angryFont, bearBrawling, new Vector2(World.Width/2, 225), AngryFontColor,
                MathHelper.ToRadians(-10), angryFont.MeasureString(bearBrawling)/2, AngryFontScale, SpriteEffects.None, 0);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
