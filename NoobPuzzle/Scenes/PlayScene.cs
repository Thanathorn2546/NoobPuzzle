using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoobBubble.Manager;
using NoobBubble.Objects;

namespace NoobBubble.Scenes
{
    public class PlayScene : GameScene
    {
        // ? Manager
        private ContentManager contentManager;
        private BubbleManager NoobBubble;

        // ? Texture2D
        private Texture2D _gameBackgroundPlaceholder;
        private Texture2D _gameScreenBackground;
        private Texture2D _gameScreenWater;
        private Texture2D _leftHUDBackground;
        private Texture2D _rightHUDBackground;

        // ? Scene Objects
        private Cannon _cannon;

        public PlayScene()
        {
            // ? Initiate Bubble Manager
            NoobBubble = new BubbleManager();

            // ? Initiate Objects
            _cannon = new Cannon();
            Singleton.Instance._cannon = _cannon;
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            // ? Shapes
            _gameBackgroundPlaceholder = new Texture2D(Singleton.Instance.graphicsDeviceManager.GraphicsDevice, 1, 1);
            Color[] data = new Color[1 * 1];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            _gameBackgroundPlaceholder.SetData(data);

            // ? Textures
            _leftHUDBackground = this.contentManager.Load<Texture2D>("PlayScene/LeftHUDBackground");
            _rightHUDBackground = this.contentManager.Load<Texture2D>("PlayScene/RightHUDBackground");
            _gameScreenBackground = this.contentManager.Load<Texture2D>("PlayScene/PlaySceneGameBackground");
            _gameScreenWater = this.contentManager.Load<Texture2D>("PlayScene/Water");

            // ? Load Objects Content
            NoobBubble.LoadContent(Content);
            _cannon.LoadContent(Content);
        }

        public void UnloadContent()
        {
            contentManager.Unload();
            NoobBubble.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            KeyboardState keyboardState = Keyboard.GetState();

            // ? Exit game with Escape
            if (keyboardState.IsKeyDown(Keys.Escape))
                Singleton.Instance.isExitGame = true;

            // ? Update Objects
            _cannon.Update(gameTime);
            NoobBubble.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Add your drawing code here

            // ? Draw Game Background
            spriteBatch.Draw(_gameScreenBackground, Singleton.Instance.GAME_SCREEN_POSITION, null, Color.White);
            spriteBatch.Draw(_gameScreenWater, new Vector2(Singleton.Instance.GAME_SCREEN_POSITION.X, 992), null, Color.White);

      
            // ? Draw Objects
            _cannon.Draw(spriteBatch);
            NoobBubble.Draw(spriteBatch);

            // ? Draw HUD Background
            spriteBatch.Draw(_leftHUDBackground, Singleton.Instance.HUD_LEFT_SCREEN_POSITION, null, Color.White);
            spriteBatch.Draw(_rightHUDBackground, Singleton.Instance.HUD_RIGHT_SCREEN_POSITION, null, Color.White);
        }
    }
}
