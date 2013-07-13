using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BarbieBear
{
    /// <summary>
    /// Collection of frames into an animation.
    /// </summary>
    public class AnimationSet
    {
        int animationTimer = 0;
        int animationFrame = 0;
        public int CurFrameNumber
        {
            get { return this.animationFrame; }
        }

        public readonly String Name;
        Texture2D texture;
        public readonly int NumFrames;
        private int startFrame;
        int width;
        public readonly int FrameDuration;
        private int framesPerRow;

        private bool shouldLoop;
        private int height;

        /// <summary>
        /// Make a new animation set.
        /// </summary>
        /// <param name="name">The name of the animation.</param>
        /// <param name="texture">The texture from which the animation will be extracted.</param>
        /// <param name="frames">The number of frames in the animation.</param>
        /// <param name="frameWidth">The width of each frame.</param>
        /// <param name="frameDuration">How long each frame should last for.</param>
        /// <param name="framesPerRow">How many frames per row there are.</param>
        /// <param name="shouldLoop">Whether or not the animation should loop.</param>
        /// <param name="startFrame">The starting frame of the animation.</param>
        public AnimationSet(String name, Texture2D texture, int frames, int frameWidth, int frameHeight, int frameDuration, 
            int framesPerRow, bool shouldLoop, int startFrame)
        {
            this.Name = name;
            this.texture = texture;
            this.NumFrames = frames;
            this.startFrame = startFrame;
            this.width = frameWidth;
            this.height = frameHeight;
            this.FrameDuration = frameDuration;
            this.shouldLoop = shouldLoop;

            if (framesPerRow == -1)
            {
                this.framesPerRow = frames;
            }
            else
            {
                this.framesPerRow = framesPerRow;
            }
        }

        /// <summary>
        /// Update the animation.
        /// </summary>
        public void Update()
        {
            if (this.shouldLoop || !this.IsDonePlaying())
            {
                this.animationTimer++;
                if (this.animationTimer > this.FrameDuration)
                {
                    this.animationTimer = 0;
                    this.animationFrame++;
                }

                if (this.animationFrame == this.NumFrames)
                {
                    this.animationFrame = 0;
                }
            }
        }

        public void Reset()
        {
            this.animationTimer = 0;
            this.animationFrame = 0;
        }

        /// <summary>
        /// Whether or not the name of this set is the
        /// given name.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <returns>True if they are the same name, false otherwise.</returns>
        public bool IsCalled(String name)
        {
            return this.Name == name;
        }

        /// <summary>
        /// The texture from which the animation is extracted.
        /// </summary>
        /// <returns>The texture of the animation.</returns>
        public Texture2D GetTexture()
        {
            return this.texture;
        }

        /// <summary>
        /// Returns a rectangle corresponding to the given frame.
        /// </summary>
        /// <param name="frame">The frame to get the rectangle from.</param>
        /// <returns>A rectangle of the same size as the frame.</returns>
        public Rectangle GetFrameRect(int frame)
        {
            int frameIndex = frame + this.startFrame;
            return new Rectangle((frameIndex % this.framesPerRow) * this.width, (frameIndex / this.framesPerRow) * this.height,
                this.width, this.height);
        }

        /// <summary>
        /// Return a rectangle corresponding to the animation's current frame.
        /// </summary>
        /// <returns>A rectangle of the same size as the frame.</returns>
        public Rectangle GetFrameRect()
        {
            return this.GetFrameRect(this.animationFrame);
        }

        /// <summary>
        /// Returns whether or not the animation is done playing.
        /// </summary>
        /// <returns></returns>
        public bool IsDonePlaying()
        {
            return this.animationTimer > this.FrameDuration - 1 && this.animationFrame == this.NumFrames - 1;
        }
    }
}
