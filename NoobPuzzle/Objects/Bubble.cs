using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NoobBubble.Objects
{
    public class Bubble : GameObject
   {
        private ContentManager contentManager;

        protected Dictionary<string,Texture2D> _bubbleColor;
        public string colorBubble;

        public Vector2 Position;
        public Vector2 Velocity;

        public Vector2 originalPos;
        public bool isActive;
        public bool isProcessed;
        public bool isShootable;
        public double radius;

        // ? Special Shooting Bubble
        public Bubble(double posX, double posY, int radius, string colorBubble, int speed, int dx, int dy, Vector2 originalPos, int skills)
        {
            Position.X = (float)posX;
            Position.Y = (float)posY;
            this.radius = radius;
            this.colorBubble = colorBubble;
            this.Velocity.X = dx;
            this.Velocity.Y = dy;
            this.originalPos = originalPos;
            this.isShootable = true;
            LoadContent(Singleton.Instance.contentManager);
        }

        // ? Normal Shooting Bubble
        public Bubble(double posX, double posY, int radius, string colorBubble, int speed, int dx, int dy, Vector2 originalPos)
        {
            Position.X = (float)posX;
            Position.Y = (float)posY;
            this.radius = radius;
            this.colorBubble = colorBubble;
            this.Velocity.X = dx;
            this.Velocity.Y = dy;
            this.originalPos = originalPos;
            this.isShootable = true;
            LoadContent(Singleton.Instance.contentManager);
        }

        // ? Dummy Bubble
        public Bubble(double posX, double posY, int radius)
        {
            Position.X = (float)posX;
            Position.Y = (float)posY;
            this.radius = radius;
            LoadContent(Singleton.Instance.contentManager);
        }

        // ? Bubble
        public Bubble(double posX, double posY, int radius, string colorBubble, bool isActive)
        {
            Position.X = (float)posX;
            Position.Y = (float)posY;
            this.radius = radius;
            this.colorBubble = colorBubble;
            this.isActive = isActive;
            LoadContent(Singleton.Instance.contentManager);
        }


        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

             _bubbleColor = new Dictionary<string, Texture2D>();
            foreach (string color in Singleton.Instance.BUBBLE_COLOR_DIC.Values)
            {
                string filePath = "PlayScene/Bubble" + color;
                _bubbleColor[color] = this.contentManager.Load<Texture2D>(filePath);
            }
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_bubbleColor[colorBubble], Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void DrawAmmo(SpriteBatch spriteBatch, Vector2 AmmoPosition)
        {
            spriteBatch.Draw(_bubbleColor[colorBubble], AmmoPosition, null, Color.White, 0f, Vector2.Zero, 41f/78f, SpriteEffects.None, 0f);
        }

        public void DrawShootingRest(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_bubbleColor[colorBubble], new Vector2(Position.X+200, Position.Y), null, Color.White, 0f, new Vector2((_bubbleColor[colorBubble].Width) / 2f, (_bubbleColor[colorBubble].Height) / 2f), 1f, SpriteEffects.None, 0f);
        }

        public void DrawShooting(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_bubbleColor[colorBubble], Position, null, Color.White, 0f, new Vector2((_bubbleColor[colorBubble].Width) / 2f, (_bubbleColor[colorBubble].Height) / 2f), 1f, SpriteEffects.None, 0f);
        }
    }
}