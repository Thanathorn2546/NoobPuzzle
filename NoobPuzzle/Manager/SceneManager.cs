using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoobBubble.Scenes; // ! Import every file from ./Scenes folders
using Microsoft.Xna.Framework.Content;

namespace NoobBubble.Manager
{
    public class SceneManager
    {
        private GameScene currentGameScene;
        public enum SceneName
        {
            // ? Each Scene files are referenced here.
            MenuScene,
            SettingScene,
            CreditScene,
            PlayScene,
            LoadingScene,
            EndStageScene,
            EndGameScene,
        }

        public SceneManager()
        {
            currentGameScene = new PlaceholderScene();
        }

        public void changeScene(SceneName sceneName)
        {
            switch (sceneName)
            {
                case SceneName.MenuScene:
                    currentGameScene = new MenuScene();
                    break;
                case SceneName.SettingScene:
                    currentGameScene = new SettingScene();
                    break;
                case SceneName.CreditScene:
                    currentGameScene = new CreditScene();
                    break;
                case SceneName.PlayScene:
                    currentGameScene = new PlayScene();
                    break;
                case SceneName.LoadingScene:
                    currentGameScene = new LoadingScene();
                    break;
                case SceneName.EndStageScene:
                    currentGameScene = new EndStageScene();
                    break;
                case SceneName.EndGameScene:
                    currentGameScene = new EndGameScene();
                    break;
            }
            LoadContent(Singleton.Instance.contentManager);
        }
        public void LoadContent(ContentManager Content)
        {
            currentGameScene.LoadContent(Content);
        }
        public void UnloadContent()
        {
            currentGameScene.UnloadContent();
        }
        public void Update(GameTime gameTime)
        {
            currentGameScene.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentGameScene.Draw(spriteBatch);
        }
    }
}