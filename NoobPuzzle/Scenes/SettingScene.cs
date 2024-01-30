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
using Microsoft.Xna.Framework.Media;

namespace NoobBubble.Scenes
{
    public class SettingScene : GameScene
    {
        private ContentManager contentManager;
        private Button backButton;
        private Image backgroundImage;

        private SpriteFont textFont;
        private string headerText;
        private string soundText;

        private Rectangle cursorRectangle;
        private Texture2D cursorTexture;
        private MouseState previousMouseState;

        private Slider soundSlider;

        public SettingScene() {
            cursorRectangle = new Rectangle(0, 0, 100, 100);
            headerText = "Setting";
            soundText = "Sound Volume";
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            backgroundImage = new Image(new Rectangle(0, 0, Singleton.Instance.widthScreen, Singleton.Instance.heightScreen), contentManager.Load<Texture2D>("MenuScene/background"));
            backButton = new Button(new Rectangle(10, 10, 100, 100), contentManager.Load<Texture2D>("MenuScene/back_button"));
            soundSlider = new Slider(new Rectangle((Singleton.Instance.widthScreen / 2) - (656 / 2), 500, 656, 100), contentManager.Load<Texture2D>("Slider/sliderbar"));
            textFont = contentManager.Load<SpriteFont>("Font/Pixel");
            cursorTexture = contentManager.Load<Texture2D>("MenuScene/cursor");

            /* Subscribe Event */
            soundSlider.OnChanged += UpdateSoundVolume;
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
            soundSlider.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            backgroundImage.Draw(spriteBatch);
            backButton.Draw(spriteBatch);

            Vector2 headerTextPosition = new Vector2((Singleton.Instance.widthScreen / 2) - (textFont.MeasureString(headerText).X / 2), 300);
            spriteBatch.DrawString(textFont, headerText, headerTextPosition, Color.Black);
            Vector2 soundTextPosition = new Vector2((Singleton.Instance.widthScreen / 2) - (textFont.MeasureString(soundText).X / 2), 450);
            spriteBatch.DrawString(textFont, soundText, soundTextPosition, Color.Black);
            spriteBatch.Draw(cursorTexture, cursorRectangle, Color.White);

            soundSlider.Draw(spriteBatch);
        }

        private void BackButtonAction()
        {
            // TODO: add feedback
            Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.MenuScene);
        }

        private void UpdateSoundVolume()
        {
            float soundVolume = soundSlider.Value;

            Singleton.Instance.soundVolume = soundVolume;
            MediaPlayer.Volume = Singleton.Instance.soundVolume;
        }
    }
}
