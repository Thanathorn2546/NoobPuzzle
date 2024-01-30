using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using NoobBubble.Objects;

namespace NoobBubble.Scenes
{
    public class EndStageScene : GameScene
    {
        int Score = Singleton.Instance.SCORE;

        bool Lose = Singleton.Instance.PLAYER_LOSE;

        private ContentManager contentManager;

        private Button playButton;
        private Button nextButton;
        private Button homeButton;

        private Image backgroundImage;
        private Image starfishImage;

        private SpriteFont textFont;
        private string paragraph;
        private string score;

        private Rectangle cursorRectangle;
        private Texture2D cursorTexture;
        private MouseState previousMouseState;

        public EndStageScene()
        {
            cursorRectangle = new Rectangle(0, 0, 100, 100);

            paragraph = "Let Do This";
            score = "Continue";
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            backgroundImage = new Image(new Rectangle(0, 0, Singleton.Instance.widthScreen, Singleton.Instance.heightScreen), contentManager.Load<Texture2D>("EndStageScene/background1"));

            playButton = new Button(new Rectangle((Singleton.Instance.widthScreen / 2) -160, 700, 75, 75), contentManager.Load<Texture2D>("EndStageScene/PAG"));
            nextButton = new Button(new Rectangle((Singleton.Instance.widthScreen / 2) -40, 700, 200, 75), contentManager.Load<Texture2D>("EndStageScene/next"));
            homeButton = new Button(new Rectangle((Singleton.Instance.widthScreen / 2) - 40, 700, 200, 75), contentManager.Load<Texture2D>("EndStageScene/home"));
            textFont = contentManager.Load<SpriteFont>("Font/Pixel");
            cursorTexture = contentManager.Load<Texture2D>("MenuScene/cursor");

            /* Subscribe Event */
            playButton.OnClicked += ReplayButtonAction;
            nextButton.OnClicked += NextButtonAction;
            homeButton.OnClicked += HomeButtonAction;
        }

        public void UnloadContent()
        {
            contentManager.Unload();
        }

        public void Update(GameTime gameTime)
        {
            /* Cursor Handle */
            MouseState currentMouseState = Mouse.GetState();
            cursorRectangle.X = currentMouseState.X;
            cursorRectangle.Y = currentMouseState.Y;
            previousMouseState = currentMouseState;

            // TODO: update as list
            if (Lose == false){
                playButton.Update(gameTime);
                nextButton.Update(gameTime);
            }
            else {
                playButton.Update(gameTime);
                homeButton.Update(gameTime);
                Singleton.Instance.PLAYER_LOSE = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            backgroundImage.Draw(spriteBatch);

            Texture2D starfishTexture;
            if (int.TryParse(score.Replace(",", ""), out int scoreValue))
            {
                if (scoreValue < 5000)
                    starfishTexture = contentManager.Load<Texture2D>("EndStageScene/starfish0");
                else if (scoreValue < 8000)
                    starfishTexture = contentManager.Load<Texture2D>("EndStageScene/starfish1");
                else if (scoreValue < 10000)
                    starfishTexture = contentManager.Load<Texture2D>("EndStageScene/starfish2");
                else
                    starfishTexture = contentManager.Load<Texture2D>("EndStageScene/starfish3");
            }
            else
            {
                starfishTexture = contentManager.Load<Texture2D>("EndStageScene/starfish2");
            }

            starfishImage = new Image(new Rectangle((Singleton.Instance.widthScreen / 2) - 250, 180, 500, 250), starfishTexture);
            starfishImage.Draw(spriteBatch);

            if (Lose == false){
                playButton.Draw(spriteBatch);
                nextButton.Draw(spriteBatch);
            }
            else {
                playButton.Draw(spriteBatch);
                homeButton.Draw(spriteBatch);
            }


            Vector2 paragraphPosition = new Vector2((Singleton.Instance.widthScreen / 2) - (textFont.MeasureString(paragraph).X / 2), (Singleton.Instance.heightScreen / 2) - (textFont.MeasureString(paragraph).Y / 2));
            spriteBatch.DrawString(textFont, paragraph, paragraphPosition, Color.White);

            Vector2 scorePosition = new Vector2((Singleton.Instance.widthScreen / 2) - (textFont.MeasureString(score).X / 2), (Singleton.Instance.heightScreen / 2) + 50);
            spriteBatch.DrawString(textFont, score, scorePosition, Color.White);

            spriteBatch.Draw(cursorTexture, new Vector2(cursorRectangle.X, cursorRectangle.Y), null, Color.White, 0f, new Vector2(cursorTexture.Width/2f, cursorTexture.Height/2f), new Vector2(10/36f,10/34f), SpriteEffects.None, 0f);
        }

        private void ReplayButtonAction()
        {
            // TODO: add feedback
            Singleton.Instance.SUB_STAGE -= 1;
            if (Singleton.Instance.SUB_STAGE < 0)
                Singleton.Instance.SUB_STAGE = 1;
            Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.PlayScene);
        }

        private void NextButtonAction()
        {
            // TODO: add feedback
            if (Singleton.Instance.SUB_STAGE <= 3)
            {
                Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.LoadingScene);
            }


            if (Singleton.Instance.SUB_STAGE == 4)
            {
                Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.EndGameScene);
            }
        }

        private void HomeButtonAction()
        {
            // TODO: add feedback
            Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.MenuScene);
        }
    }
}
