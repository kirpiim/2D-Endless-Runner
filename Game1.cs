using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace _2DEndlessRunner
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D sky, ground, player, rock, rockSecond, bird, cactus;
        private Texture2D replayTexture, debugRectTexture, playerAnimationSheet;

        private Vector2 playerPosition, obstaclePosition;
        private string currentObstacle = "rock";
        private float playerVelocityY;
        private const float gravity = 0.5f;
        private const float jumpStrength = -18f;

        private int groundY;
        private int groundHeight = 120;

        private float playerScale = 0.33f;
        private float obstacleScale = 0.25f;
        private float birdScale = 0.2f;

        private float speed = 8f;
        private float speedIncreaseRate = 0.5f;
        private float speedTimer = 0f;

        private bool isGameOver = false;
        private bool isGameStarted = false;

        private Rectangle replayRect;
        private MouseState prevMouseState;

        private Random random;
        private SpriteFont pixelFont;
        private int score;
        private float scoreTimer;

        private float scoreFontScale = 2f;
        private Vector2 scorePosition = new Vector2(700, 20);
        private Vector2 replayScale = new Vector2(2.5f, 2.5f);

        private int frameWidth, frameHeight, currentFrame;
        private double animationTimer;
        private const double timePerFrame = 0.15;

        public Game1()
        {
            Content.RootDirectory = "Content";
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            random = new Random();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            sky = Content.Load<Texture2D>("sky");
            ground = Content.Load<Texture2D>("ground third");
            player = Content.Load<Texture2D>("player");
            playerAnimationSheet = Content.Load<Texture2D>("player_animation_second");
            rock = Content.Load<Texture2D>("rock");
            rockSecond = Content.Load<Texture2D>("rock second");
            bird = Content.Load<Texture2D>("bird");
            cactus = Content.Load<Texture2D>("cactus");
            replayTexture = Content.Load<Texture2D>("replay");
            pixelFont = Content.Load<SpriteFont>("gamefont");

            frameWidth = playerAnimationSheet.Width / 2;
            frameHeight = playerAnimationSheet.Height / 2;

            groundY = _graphics.PreferredBackBufferHeight - groundHeight;
            playerPosition = new Vector2(100, groundY - player.Height * playerScale);
            obstaclePosition = new Vector2(_graphics.PreferredBackBufferWidth, groundY);

            debugRectTexture = new Texture2D(GraphicsDevice, 1, 1);
            debugRectTexture.SetData(new[] { Color.White });

            SetReplayRect();
            SpawnObstacle();
        }

        private void SetReplayRect()
        {
            int replayWidth = (int)(replayTexture.Width * replayScale.X);
            int replayHeight = (int)(replayTexture.Height * replayScale.Y);
            int replayX = (_graphics.PreferredBackBufferWidth - replayWidth) / 2;
            int replayY = (_graphics.PreferredBackBufferHeight - replayHeight) / 2;
            replayRect = new Rectangle(replayX, replayY, replayWidth, replayHeight);
        }

        private void SpawnObstacle()
        {
            int rand = random.Next(0, 10);
            if (rand < 3) currentObstacle = "bird";
            else if (rand < 6) currentObstacle = "rock";
            else currentObstacle = "cactus";

            obstaclePosition.X = _graphics.PreferredBackBufferWidth + random.Next(100, 400);
            float scale = currentObstacle == "bird" ? birdScale : obstacleScale;
            float offsetY = (currentObstacle == "rock" || currentObstacle == "cactus") ? 3f : 0f;
            obstaclePosition.Y = currentObstacle switch
            {
                "bird" => groundY - bird.Height * birdScale - 130,
                "rock" => groundY - rockSecond.Height * scale + offsetY,
                _ => groundY - cactus.Height * scale + offsetY
            };
        }

        protected override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
            MouseState currentMouseState = Mouse.GetState();

            if (!isGameStarted)
            {
                if (kstate.GetPressedKeyCount() > 0 || currentMouseState.LeftButton == ButtonState.Pressed)
                    isGameStarted = true;

                base.Update(gameTime);
                return;
            }

            if (!isGameOver)
            {
                scoreTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (scoreTimer >= 0.05f) { score++; scoreTimer = 0; }

                speedTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (speedTimer >= 1f) { speed += speedIncreaseRate; speedTimer = 0; }

                bool isOnGround = playerPosition.Y >= groundY - player.Height * playerScale;
                bool jumpPressed = kstate.IsKeyDown(Keys.Space) || kstate.IsKeyDown(Keys.Up);
                bool downPressed = kstate.IsKeyDown(Keys.Down);

                if (jumpPressed && isOnGround)
                    playerVelocityY = jumpStrength;

                if (downPressed && !isOnGround && playerVelocityY < 0)
                    playerVelocityY = 2f;

                playerVelocityY += gravity;
                playerPosition.Y += playerVelocityY;

                if (playerPosition.Y >= groundY - player.Height * playerScale)
                {
                    playerPosition.Y = groundY - player.Height * playerScale;
                    playerVelocityY = 0;
                }

                animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (animationTimer >= timePerFrame)
                {
                    currentFrame = (currentFrame + 1) % 4;
                    animationTimer = 0;
                }

                obstaclePosition.X -= speed;
                if (obstaclePosition.X < -200) SpawnObstacle();

                Rectangle playerRect = GetShrinkedRect(playerPosition, player, playerScale, 0.8f);
                Rectangle obstacleRect = GetObstacleRect();

                if (playerRect.Intersects(obstacleRect)) isGameOver = true;
            }
            else if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                if (replayRect.Contains(currentMouseState.Position))
                {
                    isGameOver = false;
                    playerPosition = new Vector2(100, groundY - player.Height * playerScale);
                    obstaclePosition.X = _graphics.PreferredBackBufferWidth;
                    playerVelocityY = 0;
                    score = 0;
                    scoreTimer = 0;
                    speed = 8f;
                    speedTimer = 0f;
                    currentFrame = 0;
                    SpawnObstacle();
                }
            }

            prevMouseState = currentMouseState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(sky, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

            // draw ground
            float groundScale = groundHeight / (float)ground.Height;
            int groundTileWidth = (int)(ground.Width * groundScale);
            for (int x = 0; x < _graphics.PreferredBackBufferWidth; x += groundTileWidth)
            {
                _spriteBatch.Draw(
                    ground,
                    new Vector2(x, groundY),
                    null,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    new Vector2(groundScale, groundScale),
                    SpriteEffects.None,
                    0f
                );
            }

            // draw player
            if (playerPosition.Y < groundY - player.Height * playerScale)
                _spriteBatch.Draw(player, playerPosition, null, Color.White, 0f, Vector2.Zero, playerScale, SpriteEffects.None, 0f);
            else
            {
                int col = currentFrame % 2;
                int row = currentFrame / 2;
                Rectangle sourceRect = new Rectangle(col * frameWidth, row * frameHeight, frameWidth, frameHeight);
                float animationScale = (player.Height * playerScale) / frameHeight;
                Vector2 walkOffset = new Vector2(0, 12);
                _spriteBatch.Draw(playerAnimationSheet, playerPosition + walkOffset, sourceRect, Color.White, 0f, Vector2.Zero, animationScale, SpriteEffects.None, 0f);
            }

            // draw start screen
            if (!isGameStarted)
            {
                string title = "Pixel Cat Runner";
                string instructions = "Press SPACE or UP to Jump\nHold DOWN to Drop Faster\nClick or Press Any Key to Start";
                Vector2 titleSize = pixelFont.MeasureString(title) * 2.5f;
                Vector2 titlePos = new Vector2((_graphics.PreferredBackBufferWidth - titleSize.X) / 2, 100);
                _spriteBatch.DrawString(pixelFont, title, titlePos, Color.Black, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);

                Vector2 instrSize = pixelFont.MeasureString(instructions) * 2f;
                Vector2 instrPos = new Vector2((_graphics.PreferredBackBufferWidth - instrSize.X) / 2, 250);
                _spriteBatch.DrawString(pixelFont, instructions, instrPos, Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            }

            // draw obstacle
            Texture2D obstacleTexture = currentObstacle switch
            {
                "rock" => rockSecond,
                "bird" => bird,
                "cactus" => cactus,
                _ => rockSecond
            };

            float drawScale = currentObstacle == "bird" ? birdScale : obstacleScale;
            Vector2 drawOffset = (currentObstacle == "rock" || currentObstacle == "cactus") ? new Vector2(0, 3) : Vector2.Zero;
            _spriteBatch.Draw(obstacleTexture, obstaclePosition + drawOffset, null, Color.White, 0f, Vector2.Zero, drawScale, SpriteEffects.None, 0f);

            _spriteBatch.DrawString(pixelFont, score.ToString(), scorePosition, Color.Black, 0f, Vector2.Zero, scoreFontScale, SpriteEffects.None, 0f);

            if (isGameOver)
            {
                _spriteBatch.Draw(replayTexture, replayRect, Color.White);
                string finalScoreText = $"Score: {score}";
                Vector2 finalScoreSize = pixelFont.MeasureString(finalScoreText) * 1.5f;
                Vector2 finalScorePos = new Vector2(
                    replayRect.X + (replayRect.Width - finalScoreSize.X) / 2,
                    replayRect.Bottom + 20
                );
                _spriteBatch.DrawString(pixelFont, finalScoreText, finalScorePos, Color.Black, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private Rectangle GetShrinkedRect(Vector2 pos, Texture2D tex, float scale, float shrink)
        {
            int width = (int)(tex.Width * scale * shrink);
            int height = (int)(tex.Height * scale * shrink);
            return new Rectangle(
                (int)(pos.X + (tex.Width * scale - width) / 2),
                (int)(pos.Y + (tex.Height * scale - height) / 2),
                width,
                height
            );
        }

        private Rectangle GetObstacleRect()
        {
            Texture2D tex = currentObstacle switch
            {
                "rock" => rockSecond,
                "bird" => bird,
                "cactus" => cactus,
                _ => rockSecond
            };

            float shrink = currentObstacle == "rock" ? 0.4f : currentObstacle == "bird" ? 0.6f : 0.8f;
            float scale = currentObstacle == "bird" ? birdScale : obstacleScale;
            return GetShrinkedRect(obstaclePosition, tex, scale, shrink);
        }
    }
}
