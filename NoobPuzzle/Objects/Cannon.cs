using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoobBubble.Manager;

namespace NoobBubble.Objects
{
    public class Cannon : GameObject
    {
        private ContentManager contentManager;


        // ? Texture2D
        protected Texture2D _cannon;
        protected Texture2D _baseCannon;
        protected Texture2D _cannonHolder;
        protected Texture2D _placeholderTexture;

        // ? Font
        protected SpriteFont mainFont;

        // ? Sound
        private SoundEffect _shootingEffect;
        private SoundEffectInstance _shootingEffectInstance;

        // ? Variables
        public Vector2 Position;
        public Vector2 cannonSize;
        public float rotateOrigin;
        public int cursorSize;

        public Bubble currentBubble;
        public float minimumDegree;
        public float maximumDegree;
        public float shootingDegree;
        public int shootingDirection; // -1 Left, 1 Right
        public string[] BubbleColorList;

        public Cannon()
        {
            cursorSize = 1;
            cannonSize = new Vector2(128, 149);
            float posX = (Singleton.Instance.GAME_SCREEN_SIZE.X / 2f) + Singleton.Instance.GAME_SCREEN_POSITION.X;
            float posY = 870;
            rotateOrigin = 73 / 100f;
            Position = new Vector2(posX, posY);

            shootingDegree = 0;
            minimumDegree = (float)DegreeToRadian(-60);
            maximumDegree = (float)DegreeToRadian(60);
            shootingDirection = 0;

            BubbleColorList = new List<string>(Singleton.Instance.BUBBLE_COLOR_DIC.Values).ToArray();
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            // ? Shapes
            _placeholderTexture = new Texture2D(Singleton.Instance.graphicsDeviceManager.GraphicsDevice, 1, 1);
            Color[] data = new Color[1 * 1];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            _placeholderTexture.SetData(data);

            // ? Textures
            _baseCannon = this.contentManager.Load<Texture2D>("PlayScene/BaseCannon");
            _cannonHolder = this.contentManager.Load<Texture2D>("PlayScene/CannonHolder");
            _cannon = this.contentManager.Load<Texture2D>("PlayScene/Cannon");

            // ? Font
            mainFont = contentManager.Load<SpriteFont>("Font/Pixel");

            _shootingEffect = Content.Load<SoundEffect>("PlayScene/ShootingEffect");
            _shootingEffectInstance = _shootingEffect.CreateInstance();
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();

            // Calculate the angle based on mouse position
            Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);
            Vector2 direction = mousePosition - Position;

            // Calculate the angle
            shootingDegree = MathHelper.ToRadians(0.1f) * direction.X;

            // Ensure smooth movement to the right and left
            shootingDegree = MathHelper.WrapAngle((float)shootingDegree);

            // Minimum/Maximum Degree
            if (shootingDegree < minimumDegree)
            {
                shootingDegree = minimumDegree;
            }
            else if (shootingDegree > maximumDegree)
            {
                shootingDegree = maximumDegree;
            }

            // Check for shooting
            if (currentMouseState.LeftButton == ButtonState.Pressed &&
                !Singleton.Instance.isShooting &&
                Singleton.Instance._shootingBubble.Velocity.X.Equals(0) &&
                Singleton.Instance._shootingBubble.Velocity.Y.Equals(0))
            {
                _shootingEffectInstance.Play();

                // Corrected shooting logic
                Singleton.Instance._shootingBubble.Velocity.X = (float)(System.Math.Sin(shootingDegree) * Singleton.Instance.BUBBLE_SPEED);
                Singleton.Instance._shootingBubble.Velocity.Y = (float)(-System.Math.Cos(shootingDegree) * Singleton.Instance.BUBBLE_SPEED);
                Singleton.Instance.isShooting = true;
            }
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            // ?  Cannon and Aiming Line
            CannonUI(spriteBatch);
            GuidingLine(spriteBatch);
        }

        public double DegreeToRadian(double degree)
        {
            return (degree * MathHelper.Pi) / 180;
        }

        public int RandomNumber(int start, int range)
        {
            System.Random rnd = new System.Random();
            return rnd.Next(range) + start;
        }

        public void CannonUI(SpriteBatch spriteBatch)
        {

            // ? Load cannon with sprite
            spriteBatch.Draw(_cannon, Position, null, Color.White, (float)shootingDegree, new Vector2(_cannon.Width / 2f, _cannon.Height * rotateOrigin), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(_baseCannon, new Vector2(816, 935), null, Color.White);
            spriteBatch.Draw(_cannonHolder, new Vector2((Singleton.Instance.GAME_SCREEN_SIZE.X / 2) + Singleton.Instance.GAME_SCREEN_POSITION.X - _cannonHolder.Width / 2, 864), null, Color.White);
        }

        public void GuidingLine(SpriteBatch spriteBatch)
        {
            double newX = System.Math.Sin(shootingDegree) * (cannonSize.Y * rotateOrigin) + Position.X;
            double newY = -System.Math.Cos(shootingDegree) * (cannonSize.Y * rotateOrigin) + Position.Y;

            // ? First initial cursor line
            double borderToCannonDistance = Singleton.Instance.GAME_SCREEN_SIZE.X + Singleton.Instance.GAME_SCREEN_POSITION.X - Position.X;

            // ! No margin offset value
            double cursorShowLength = borderToCannonDistance / System.Math.Sin(System.Math.Abs(shootingDegree)) - (cannonSize.Y * rotateOrigin);

            // ? Upper border
            // ! 0.369 ~= 21 in Degree
            if (shootingDegree > -0.369 && shootingDegree < 0.369)
            {
                if (shootingDegree.Equals(0))
                {
                    cursorShowLength = (Singleton.Instance.GAME_SCREEN_POSITION.Y + Position.Y);
                }
                else
                {
                    cursorShowLength = newY / System.Math.Cos(System.Math.Abs(shootingDegree)) + (cannonSize.Y * rotateOrigin);
                }
            }

            // ? Draw First Cursor
            spriteBatch.Draw(_placeholderTexture, new Vector2((float)newX, (float)newY), null, Color.Red, (float)shootingDegree, new Vector2(1 / 2f, 1f), new Vector2(cursorSize, (float)cursorShowLength), SpriteEffects.None, 0f);
        }
    }
}