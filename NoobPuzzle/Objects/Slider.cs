using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.ComponentModel.DataAnnotations;

namespace NoobBubble.Objects
{
    public class Slider : GameObject
    {
        public Rectangle Rectangle { get; }
        public Texture2D Texture { get; }
        public Color Color { get; private set; }

        [Range(0, 1)]
        public float Value { get; private set; }

        private bool isDragging;

        public delegate void SliderChangedEventHandler();
        public event SliderChangedEventHandler OnChanged;

        public Slider(Rectangle rectangle, Texture2D texture)
        {
            Rectangle = rectangle;
            Texture = texture;
            Color = Color.White;
            Value = 0.5f; // Default value
        }

        public void LoadContent(ContentManager Content)
        {

        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            /* Mouse Drag Handle */

            MouseState mouseState = Mouse.GetState();
            Point mousePosition = new Point(mouseState.X, mouseState.Y);

            if (Rectangle.Contains(mousePosition))
            {
                if (mouseState.LeftButton == ButtonState.Pressed && !isDragging)
                {
                    isDragging = true;
                }
            }

            if (mouseState.LeftButton == ButtonState.Released && isDragging)
            {
                isDragging = false;
            }

            if (isDragging)
            {
                float mouseX = mousePosition.X - Rectangle.X;
                Value = MathHelper.Clamp(mouseX / Rectangle.Width, 0f, 1f);
                OnChanged?.Invoke();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color); // Draw a slider background rect
            spriteBatch.Draw(Texture, new Rectangle(Rectangle.X, Rectangle.Y, (int)(Rectangle.Width * Value), Rectangle.Height), new Color(0, 0, 0, 100)); // Draw a slider value rect
        }
    }
}
