using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoobBubble.Manager;

namespace NoobBubble
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Main()
        {
            // ? System-related variable and setting.
            _graphics = new GraphicsDeviceManager(this);
            Singleton.Instance.graphicsDeviceManager = _graphics;
            Content.RootDirectory = "Content";
            Singleton.Instance.contentManager = Content;
            IsMouseVisible = false; // ? Not showing mouse cursor.


            // ! NOTHING TO ADD HERE!
        }

        protected override void Initialize()
        {
            // ? Initialize system-related for the game, please store the value
            // ? in the Singleton class.
            _graphics.PreferredBackBufferWidth = Singleton.Instance.widthScreen;
            _graphics.PreferredBackBufferHeight = Singleton.Instance.heightScreen;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();


            // ! NOTHING TO ADD HERE!


            base.Initialize();
        }

        protected override void LoadContent()
        {
            // ? Load content by call the LoadContent in SceneManager, which each
            // ? scene has its own LoadContent code.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Singleton.Instance.sceneManager.LoadContent(Content);


            // ! NOTHING TO ADD HERE!
        }

        protected override void Update(GameTime gameTime)
        {
            if (Singleton.Instance.isExitGame)
                Exit();

            // ? System-related logic code here. e.g. isFullScreen, isExitGame
            Singleton.Instance.sceneManager.Update(gameTime);


            // ! NOTHING TO ADD HERE!


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            // ? Draw by call the Draw in SceneManager, which each
            // ? scene has its own Draw code.
            Singleton.Instance.sceneManager.Draw(_spriteBatch);


            // ! NOTHING TO ADD HERE!


            _spriteBatch.End();
            _graphics.BeginDraw();
            base.Draw(gameTime);
        }
    }
}
