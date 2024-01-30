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
    public class CreditScene : GameScene
    {
        private ContentManager contentManager;

        private Image backgroundImage;
        private Button backButton;

        private SpriteFont textFont;
        private string paragraph; // TODO: add a Text class that handle a text
        private string title;

        private Rectangle cursorRectangle;
        private Texture2D cursorTexture;
        private MouseState previousMouseState;

        public CreditScene() 
        {
            cursorRectangle = new Rectangle(0, 0, 100, 100);

            title = "CREDIT";
            paragraph = "64050037 Kheerinthon Temprada \n64050502 Nopparuj     Kanyasin \n64050481 Thanathorn   Krirkkawin \n64050502 Nopparuj     Kanyasin ";
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            backgroundImage = new Image(new Rectangle(0, 0, Singleton.Instance.widthScreen, Singleton.Instance.heightScreen), contentManager.Load<Texture2D>("MenuScene/background"));
            backButton = new Button(new Rectangle(10, 10, 100, 100), contentManager.Load<Texture2D>("MenuScene/back_button"));
            textFont = contentManager.Load<SpriteFont>("Font/Pixel");
            cursorTexture = contentManager.Load<Texture2D>("MenuScene/cursor");

            /* Subscribe Event */
            backButton.OnClicked += BackButtonAction;
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

            backButton.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            backgroundImage.Draw(spriteBatch);
            backButton.Draw(spriteBatch);

            Vector2 titlePosition = new Vector2((Singleton.Instance.widthScreen / 2) - (textFont.MeasureString(title).X / 2), 300);
            spriteBatch.DrawString(textFont, title, titlePosition, Color.OrangeRed);
            Vector2 paragraphPosition = new Vector2((Singleton.Instance.widthScreen / 2) - (textFont.MeasureString(paragraph).X / 2), (Singleton.Instance.heightScreen / 2) - (textFont.MeasureString(paragraph).Y / 2));
            spriteBatch.DrawString(textFont, paragraph, paragraphPosition, Color.OrangeRed);

            spriteBatch.Draw(cursorTexture, cursorRectangle, Color.White);
        }

        private void BackButtonAction()
        {
            // TODO: add feedback

            Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.MenuScene);
        }
    }
}