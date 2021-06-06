/************************************************************
 * Sprite Class                                             *
 * based on Sprite class from Learning XNA, by Aaron Reed   *
 * modifications made by Mark McCarthy                      *
 ************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace CorporateEspionage
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Sprite
    {
        Texture2D textureImage;
        protected Vector2 position;
        protected Point frameSize;
        Point currentFrame;
        Point sheetSize;
        float scale;
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame;
        protected Vector2 facing;                       //direction sprite is facing
        const int defaultMillisecondsPerFrame = 350;

        public Vector2 Position
        {
            get { return position; }
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame,
            Point sheetSize, Vector2 facing, float scale)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.facing = facing;
            this.scale = scale;

            millisecondsPerFrame = defaultMillisecondsPerFrame;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize()
        {
            // TODO: Add your initialization code here

        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //update animation frame -- two-frame animation in each cardinal direction
            //facing down = row 0
            //facing up = row 1
            //facing left = row 2
            //facing right = row 3
            if(facing.Y == 1)
                currentFrame.Y = 0;
            else if (facing.Y == -1)
                currentFrame.Y = 1;
            else if(facing.X == -1)
                currentFrame.Y = 2;
            else
                currentFrame.Y = 3;

            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                currentFrame.X++;
                if (currentFrame.X >= sheetSize.X)
                    currentFrame.X = 0;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Draw the sprite
            spriteBatch.Draw(textureImage, position,
                new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }
}