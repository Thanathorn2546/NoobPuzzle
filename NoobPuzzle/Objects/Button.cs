using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace NoobBubble.Objects
{
    public class Button : GameObject
    {
        public Rectangle Rectangle { get; }
        public Texture2D Texture { get; }
        public Color Color { get; private set; }

        public delegate void ButtonClickedEventHandler();
        public event ButtonClickedEventHandler OnClicked;

        public Button(Rectangle rectangle, Texture2D texture)
        {
            Rectangle = rectangle;
            Texture = texture;
            Color = Color.White;
        }

        public void LoadContent(ContentManager Content)
        {

        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            /* Click & Hover Handle */

            MouseState mouseState = Mouse.GetState();

            if (Rectangle.Contains(mouseState.Position))
            {
                Color = Color.Gray;
            }
            else
            {
                Color = Color.White;
            }

            if (mouseState.LeftButton == ButtonState.Pressed && Rectangle.Contains(mouseState.Position))
            {
                OnClicked?.Invoke();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color);
        }
    }
}