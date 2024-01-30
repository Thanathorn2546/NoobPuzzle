using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoobBubble.Objects;

namespace NoobBubble.Scenes
{
    public class EndGameScene : GameScene
    {
        private ContentManager contentManager;

        private Image backgroundImage;
        private Image CreditImage;

        private SpriteFont textFont;

        private Rectangle cursorRectangle;
        private Texture2D cursorTexture;
        private MouseState previousMouseState;

        private float creditPosition = 0f;
        private float creditScrollSpeed = 1.0f;

        public EndGameScene()
        {
            cursorRectangle = new Rectangle(0, 0, 100, 100);
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            backgroundImage = new Image(new Rectangle(0, 0, Singleton.Instance.widthScreen, Singleton.Instance.heightScreen), contentManager.Load<Texture2D>("EndStageScene/background1"));
            CreditImage = new Image(new Rectangle((Singleton.Instance.widthScreen / 2) - 400, 100, 900, 2000), contentManager.Load<Texture2D>("EndStageScene/endCredit"));

            textFont = contentManager.Load<SpriteFont>("Font/Pixel");
            cursorTexture = contentManager.Load<Texture2D>("MenuScene/cursor");
        }

        public void UnloadContent()
        {
            contentManager.Unload();
        }

        public void Update(GameTime gameTime)
        {
            /* Cursor Handle */
            MouseState currentMouseState = Mouse.GetState();
            cursorRectangle.X = currentMouseState.X;
            cursorRectangle.Y = currentMouseState.Y;
            previousMouseState = currentMouseState;

            // Scroll the end credits up
            creditPosition -= creditScrollSpeed;

            // If the end credits have scrolled off the screen, reset the position to the top
            if (creditPosition > CreditImage.Rectangle.Height)
            {
                creditPosition = 0;
            }


            KeyboardState keyboardState = Keyboard.GetState();

            // ? Exit game with Escape
            if (keyboardState.IsKeyDown(Keys.Escape))
                Singleton.Instance.isExitGame = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            backgroundImage.Draw(spriteBatch);

            // Draw the end credits with the current credit position
            Rectangle creditRectangle = new Rectangle(
                CreditImage.Rectangle.X,
                (int)creditPosition,
                CreditImage.Rectangle.Width,
                CreditImage.Rectangle.Height - (int)creditPosition
            );

            spriteBatch.Draw(CreditImage.Texture, creditRectangle, Color.White);

            spriteBatch.Draw(cursorTexture, new Vector2(cursorRectangle.X, cursorRectangle.Y), null, Color.White, 0f, new Vector2(cursorTexture.Width/2f, cursorTexture.Height/2f), new Vector2(10/36f,10/34f), SpriteEffects.None, 0f);
        }
    }
}