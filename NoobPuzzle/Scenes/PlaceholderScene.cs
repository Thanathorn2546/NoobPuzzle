using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NoobBubble.Scenes
{
    public class PlaceholderScene : GameScene
    {
        private ContentManager contentManager;
        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);
        }
        public void UnloadContent()
        {
            contentManager.Unload();
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            // ? Change scene
            // ! Change from the "MenuScene" to whatever scene your want.
            // ! See the enum for scene name in "SceneManager" file.
            Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.MenuScene);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Add your drawing code here
        }
    }
}
