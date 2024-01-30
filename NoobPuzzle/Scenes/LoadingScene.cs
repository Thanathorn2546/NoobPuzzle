using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoobBubble.Objects;
using Microsoft.Xna.Framework.Input;

namespace NoobBubble.Scenes
{
    public class LoadingScene : GameScene
    {
        private ContentManager contentManager;

        private Image backgroundImage;

        private Texture2D backGround;
        private Texture2D foreGround;
     //   private Vector2 Position;
        private float progress = 0.0f;
        private float elapsedSeconds;

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            backgroundImage = new Image(new Rectangle(0, 0, Singleton.Instance.widthScreen, Singleton.Instance.heightScreen), contentManager.Load<Texture2D>("MenuScene/background"));
            backGround = contentManager.Load<Texture2D>("LoadingScene/stoke");
            foreGround = contentManager.Load<Texture2D>("LoadingScene/bar");
        }
        public void UnloadContent()
        {
            contentManager.Unload();
        }

        public void Update(GameTime gameTime)
        {
            elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            progress += elapsedSeconds / 1.0f; // Fill up over 5 seconds
            if (progress > 1.0f)
            {
                Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.PlayScene); // change scene
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            backgroundImage.Draw(spriteBatch);
            // Draw the background
            spriteBatch.Draw(backGround, new Vector2(50, Singleton.Instance.heightScreen - (Singleton.Instance.heightScreen / 6)), Color.White);

            // Draw the fill, scaled to the current progress value
            Rectangle fillRect = new Rectangle(0, 0, (int)(foreGround.Width * progress), foreGround.Height);
            spriteBatch.Draw(foreGround, new Vector2(50, Singleton.Instance.heightScreen - (Singleton.Instance.heightScreen / 6)), fillRect, Color.White);
        }
    }
}
