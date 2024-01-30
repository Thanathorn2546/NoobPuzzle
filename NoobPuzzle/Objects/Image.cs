using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoobBubble.Objects
{
    public class Image : GameObject
    {
        public Rectangle Rectangle { get; }
        public Texture2D Texture { get; }

        public Image(Rectangle rectangle, Texture2D texture)
        {
            Rectangle = rectangle;
            Texture = texture;
        }

        public void LoadContent(ContentManager Content)
        {

        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }
    }
}
