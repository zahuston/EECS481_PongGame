using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace EECS481_HW2
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont gameFont;
        Texture2D pong1, pong2, ball;
        Rectangle pong1Pos, pong2Pos, ballPos;
        Rectangle screenDimensions;
        int p1Score, p2Score;
        Vector2 ballVelocity;
        Vector2 screenSize;

        public enum collisionType
        {
            NoCollision = 0,
            TopCollision,
            BottomCollision,
            LeftCollision,
            RightCollision
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Sets the initial/resting positions of the ball and the two sliders
            pong1Pos = new Rectangle(10, 200, 30, 70);
            pong2Pos = new Rectangle(760, 200, 30, 70);
            ballPos = new Rectangle(380, 220, 30, 36);
            ballVelocity.X = 3;
            ballVelocity.Y = 3;
            screenSize = new Vector2(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
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
            pong1 = Content.Load<Texture2D>("WhiteBox");
            pong2 = Content.Load<Texture2D>("WhiteBox");
            ball = Content.Load<Texture2D>("PongBall");
            gameFont = Content.Load<SpriteFont>("SpriteFont1");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
            // TODO: Unload any non ContentManager content here
        }

        protected bool handlePossibleCollisions(Rectangle check, int xChange, int yChange, bool shouldHandleDirection)
        {
            collisionType collision = collisionType.NoCollision;
            if (check.Y + yChange < 0)
            {
                collision = collisionType.TopCollision;
            }
            else if (check.Y + check.Height + yChange > screenSize.Y)
            {
                collision = collisionType.BottomCollision;
            }
            else if (check.X + xChange < 0)
            {
                collision = collisionType.LeftCollision;
                if (shouldHandleDirection) p1Score++;
            }
            else if (shouldHandleDirection && ballPos.Intersects(pong1Pos))
            {
                collision = collisionType.LeftCollision;
            }
            else if (check.X + check.Width + xChange > screenSize.X)
            {
                if (shouldHandleDirection) p2Score++;
                collision = collisionType.RightCollision;
            }
            else if (shouldHandleDirection && ballPos.Intersects(pong2Pos))
            {
                collision = collisionType.RightCollision;
            }
            if (collision != collisionType.NoCollision)
            {
                if (shouldHandleDirection)
                {
                    redirectBall(collision);
                }
                return true;
            }
            return false;
        }

        protected void redirectBall(collisionType collision)
        {
            if (collision == collisionType.TopCollision)
            {
                ballVelocity.Y *= -1;
            }
            else if (collision == collisionType.LeftCollision)
            {
                ballVelocity.X *= -1;
            }
            else if (collision == collisionType.BottomCollision)
            {
                ballVelocity.Y *= -1;
            }
            else
            {
                ballVelocity.X *= -1;
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            // Allows the game to exit
            int moveSpeed = 2;
            if (keyState.IsKeyDown(Keys.Escape))
                this.Exit();

            if (keyState.IsKeyDown(Keys.A) && !handlePossibleCollisions(pong1Pos, 0, -moveSpeed, false)) //Move right pong down
            {
                pong1Pos.Y -= moveSpeed;
            }
            else if (keyState.IsKeyDown(Keys.Z) && !handlePossibleCollisions(pong1Pos, 0, moveSpeed, false)) //Move left pong down
            {
                pong1Pos.Y += moveSpeed;
            }

            if (keyState.IsKeyDown(Keys.K) && !handlePossibleCollisions(pong2Pos, 0, -moveSpeed, false))
            {
                pong2Pos.Y -= moveSpeed;
            }
            else if (keyState.IsKeyDown(Keys.M) && !handlePossibleCollisions(pong2Pos, 0, moveSpeed, false))
            {
                pong2Pos.Y += moveSpeed;
            }
            if (handlePossibleCollisions(ballPos, (moveSpeed / 2) * (int)ballVelocity.X, (moveSpeed / 2) * (int)ballVelocity.Y, true)) ;


            ballPos.X += (moveSpeed / 2) * (int)ballVelocity.X;
            ballPos.Y += (moveSpeed / 2) * (int)ballVelocity.Y;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            // TODO: Add your drawing code here
            DrawMovingElements(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected void DrawMovingElements(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pong1, pong1Pos, Color.White);
            spriteBatch.Draw(pong2, pong2Pos, Color.White);
            spriteBatch.Draw(ball, ballPos, Color.White);
            spriteBatch.DrawString(gameFont, "Player1 Score : " + p1Score.ToString(), new Vector2(20, 20), Color.Black);
            spriteBatch.DrawString(gameFont, "Player2 Score : " + p2Score.ToString(), new Vector2(500, 20), Color.Black);
        }
    }


}
