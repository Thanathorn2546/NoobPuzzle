using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NoobBubble.Scenes
{
    internal interface GameScene
    {
        public void LoadContent(ContentManager Content);
        public void UnloadContent();
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);
    }
}
