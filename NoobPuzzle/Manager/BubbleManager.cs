using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoobBubble.Objects;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;

namespace NoobBubble.Manager
{
    public class BubbleManager
    {
        // ? Manager
        private ContentManager contentManager;

        // ? Texture2D
        private Texture2D _cartridgeNormalBackground;
        private Texture2D _cartridgeSpecialBackground;
        private Texture2D[] _specialBubbleTextures;
        private Texture2D _bossBackground;
        private Texture2D[] _bubbleExplosion;
        private Texture2D _bossSprite;

        // ? Font
        protected SpriteFont mainFont;

        // ? Scene Objects
        private List<Bubble> listBubble;
        public string[] BubbleColorList;

        // ? Sound
        private SoundEffect _explosion;
        private SoundEffectInstance _explosionInstance;

        // ? Objects
        private Bubble[] _normalBubbles;
        private Bubble[] _specialBubbles;

        private List<List<string[]>> stageList = new List<List<string[]>>();
        private List<string[]> stage1_1;
        private List<string[]> stage1_2;
        private List<string[]> stage1_3;

        public void InitializeStages()
        {
            stage1_1 = GenerateRandomBubbleArrangement();
            stage1_2 = GenerateRandomBubbleArrangement();
            stage1_3 = GenerateRandomBubbleArrangement();

            stageList.Add(stage1_1);
            stageList.Add(stage1_2);
            stageList.Add(stage1_3);
        }
        public Dictionary<string, int> shortToIndex = new Dictionary<string, int>(){
            {"Blue", 0},
            {"Cyan", 1},
            {"Green", 2},
            {"Red", 3},
            {"Yellow", 4},
            {"Black", 5}
        };
        private int[] amountBubbleColor = { 0, 0, 0, 0, 0 };
        private List<Vector2> listEffectPosition = new List<Vector2>();

        public bool isUsingSpecialAmmo;
        public int specialAmmoIndex;
        public bool loadNewAmmo;
        private bool autoPass;
        private int currentAnimateIndex;

        private int haveMargin = 1;
        private double lastPressTime;

        public BubbleManager()
        {
            listBubble = new List<Bubble>();
            BubbleColorList = new List<string>(Singleton.Instance.BUBBLE_COLOR_DIC.Values).ToArray();
            InitializeStages();
            _normalBubbles = new Bubble[(int)Singleton.Instance.CANNON_CARTRIDGE_SIZE];
            _specialBubbles = new Bubble[(int)Singleton.Instance.CANNON_CARTRIDGE_SIZE];
            _specialBubbleTextures = new Texture2D[3];
            _bubbleExplosion = new Texture2D[7];
            Singleton.Instance.isShooting = false;
            lastPressTime = 0f;
            autoPass = false;
            GenerateBubbles();
            InitiateBubble();
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            // ? Bubbles
            foreach (Bubble bubble in listBubble)
            {
                bubble.LoadContent(Content);
            }

            // ? Font
            mainFont = contentManager.Load<SpriteFont>("Font/Pixel");

            _cartridgeNormalBackground = this.contentManager.Load<Texture2D>("PlayScene/CartridgeNormal");
            _cartridgeSpecialBackground = this.contentManager.Load<Texture2D>("PlayScene/CartridgeSpecial");
            for (int i = 0; i < 3; i++)
            {
                _specialBubbleTextures[i] = this.contentManager.Load<Texture2D>("PlayScene/SpecialBubble"+(i+1));
            }
            for (int i = 0; i < 7; i++)
            {
                _bubbleExplosion[i] = this.contentManager.Load<Texture2D>("PlayScene/BubbleExplosion"+(i+1));
            }

            // ? Boss
            //_bossBackground = this.contentManager.Load<Texture2D>("PlayScene/BossBackground");
            //_bossSprite = this.contentManager.Load<Texture2D>("PlayScene/Boss");

            // ? Song
            _explosion = Content.Load<SoundEffect>("PlayScene/Explosion");
            _explosionInstance = _explosion.CreateInstance();
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.P))
            {
                autoPass = true;
            }


            if (listBubble.FindAll(bubble => bubble.isActive).Count == 0 || autoPass)
            {
                Singleton.Instance.SUB_STAGE += 1;
                Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.EndStageScene);
            }


            double nowPressTime = (gameTime.TotalGameTime.Ticks / System.TimeSpan.TicksPerMillisecond);

            Animate(gameTime);

            // ? Ceil Dropping
            if (Singleton.Instance.SUB_STAGE == 2 && nowPressTime - lastPressTime > Singleton.Instance.gameTicksInMilliSec * 220)
            {
                CeilDropping(2);

                lastPressTime = nowPressTime;
            }
            else if (Singleton.Instance.SUB_STAGE == 3 && nowPressTime - lastPressTime > Singleton.Instance.gameTicksInMilliSec * 160)
            {
                CeilDropping(3);
                lastPressTime = nowPressTime;
            }

            Singleton.Instance._shootingBubble.Position.X += Singleton.Instance._shootingBubble.Velocity.X;
            Singleton.Instance._shootingBubble.Position.Y += Singleton.Instance._shootingBubble.Velocity.Y;

            // ? Bounce the Bubble against the Border.
            if (Singleton.Instance._shootingBubble.Position.X - Singleton.Instance.BUBBLE_GRID_MARGIN / 2 < Singleton.Instance.BUBBLE_START_POS.X)
            { // ? Left Border
                Singleton.Instance._shootingBubble.Position.X = Singleton.Instance.BUBBLE_START_POS.X + Singleton.Instance.BUBBLE_GRID_MARGIN / 2;
                Singleton.Instance._shootingBubble.Velocity.X *= -1;
            }
            else if (Singleton.Instance._shootingBubble.Position.X + Singleton.Instance.BUBBLE_GRID_MARGIN / 2 > Singleton.Instance.GAME_SCREEN_POSITION.X + Singleton.Instance.GAME_SCREEN_SIZE.X)
            { // ? Right Border
                Singleton.Instance._shootingBubble.Position.X = (Singleton.Instance.GAME_SCREEN_POSITION.X + Singleton.Instance.GAME_SCREEN_SIZE.X) - Singleton.Instance.BUBBLE_GRID_MARGIN / 2;
                Singleton.Instance._shootingBubble.Velocity.X *= -1;
            }

            // ? Top Border
            if (Singleton.Instance._shootingBubble.Position.Y - Singleton.Instance.BUBBLE_GRID_MARGIN / 2 < Singleton.Instance.BUBBLE_START_POS.Y)
            {
                Bubble closestBubble = FindClosestBubble(Singleton.Instance._shootingBubble);
                HandleCollision(closestBubble);
            }

            // ? Collide with other Bubble
            for (int i = 0; i < listBubble.Count; i++)
            {
                Bubble bubble = listBubble[i];
                if (bubble.isActive && CheckCollides(Singleton.Instance._shootingBubble, bubble))
                {
                    Bubble closestBubble = FindClosestBubble(Singleton.Instance._shootingBubble);
                    if (closestBubble == null)
                    {
                        // ? Game Over!
                        Singleton.Instance.PLAYER_LOSE = true;
                        Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.EndStageScene);
                    }

                    if (closestBubble != null)
                    {
                        HandleCollision(closestBubble);
                    }
                }
            }

            // ? Bubble Past the Border
            List<Bubble> pastBorderBubble = listBubble.FindAll(bubble => (int)System.Math.Floor(bubble.Position.Y / 75) > Singleton.Instance.BUBBLE_SIZE.Y - 1 && bubble.isActive);
            if (pastBorderBubble.Count > 0)
            {
                // ? Game Over!
                Singleton.Instance.PLAYER_LOSE = true;
                Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.EndStageScene);
            }

            Singleton.Instance.isShooting = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bubble bubble in listBubble)
            {
                if (bubble.isActive)
                {
                    bubble.Draw(spriteBatch);
                }
                //else bubble.Draw(spriteBatch);
            }

            // ? Shooting Bubble
            Singleton.Instance._shootingBubble.DrawShooting(spriteBatch);

            // ? Cartridge Ammo UI
            CartridgeNormalAmmoUI(spriteBatch);

            // ? Boss Drawing
            //if (Singleton.Instance.SUB_STAGE == 3)
            //{
            //    spriteBatch.Draw(_bossBackground, new Vector2(Singleton.Instance.GAME_SCREEN_POSITION.X, 0), null, Color.White);
            //    spriteBatch.Draw(_bossSprite, new Vector2(860, 5), null, Color.White);
            //}

            foreach (Vector2 effectPosition in listEffectPosition)
            {
                spriteBatch.Draw(_bubbleExplosion[currentAnimateIndex], effectPosition, null, Color.White);
            }

            if (currentAnimateIndex == _bubbleExplosion.Length - 1)
            {
                currentAnimateIndex = 0;
                listEffectPosition.Clear();
            }
        }

        private List<string[]> GenerateRandomBubbleArrangement()
        {
            List<string[]> arrangement = new List<string[]>();

            // First 5 rows
            for (int i = 0; i < 5; i++)
            {
                string[] row = new string[11];

                // Randomly shuffle the colors R, G, Y, B
                List<string> colors = new List<string> { "R", "G", "Y", "B" };
                Shuffle(colors);

                for (int j = 0; j < 4; j++)
                {
                    row[j] = colors[j];
                }

                // Randomly shuffle the colors R, G, Y, B for the remaining bubbles
                Shuffle(colors);

                for (int j = 4; j < 8; j++)
                {
                    row[j] = colors[j - 4];
                }

                Shuffle(colors);

                for (int j = 8; j < 11; j++)
                {
                    row[j] = colors[j - 8];
                }

                arrangement.Add(row);
            }

            // Generate the rest of the rows as empty
            for (int i = 5; i < 11; i++)
            {
                string[] row = new string[11];

                for (int j = 0; j < 11; j++)
                {
                    row[j] = "X";
                }

                arrangement.Add(row);
            }

            return arrangement;
        }

        private void Shuffle<T>(List<T> list)
        {
            Random random = new Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public void CartridgeNormalAmmoUI(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_cartridgeNormalBackground, new Vector2(603, 953), null, Color.White);
            for (int i = Singleton.Instance.CANNON_CARTRIDGE_SIZE - 1; i > 0; i--)
            {
                // ? The order is 3 2 1 0
                _normalBubbles[i].DrawAmmo(spriteBatch, new Vector2(736 - (55 * (i - 1)), 981));
            }
        }

        public void Animate(GameTime gameTime)
        {
            int animationSpeed = 200; // Change this value to adjust the animation speed
            int numFrames = _bubbleExplosion.Length;
            int totalAnimationTime = animationSpeed * numFrames;

            int animationTime = (int)gameTime.TotalGameTime.TotalMilliseconds % totalAnimationTime;

            int frameIndex = animationTime / animationSpeed;

            currentAnimateIndex = frameIndex % numFrames;
        }

        public double DegreeToRadian(double degree)
        {
            return (degree * MathHelper.Pi) / 180;
        }

        public Vector2 RotatePoint(double posX, double posY, double angle)
        {
            double sin = System.Math.Sin(angle);
            double cos = System.Math.Cos(angle);

            posX = (float)((posX * cos) - (posY * sin));
            posY = (float)((posX * sin) + (posY * cos));
            return new Vector2((float)posX, (float)posY);
        }

        public int RandomNumber(int start, int range)
        {
            System.Random rnd = new System.Random();
            return rnd.Next(range) + start;
        }

        public double CalculateDistance(Bubble shootingBubble, Bubble bubbleOnGrid)
        {
            double distanceX = shootingBubble.Position.X - bubbleOnGrid.Position.X;
            double distanceY = shootingBubble.Position.Y - bubbleOnGrid.Position.Y;
            return System.Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY));
        }

        public bool CheckCollides(Bubble shootingBubble, Bubble bubbleOnGrid)
        {
            return CalculateDistance(shootingBubble, bubbleOnGrid) < shootingBubble.radius + bubbleOnGrid.radius;
        }

        public Bubble FindClosestBubble(Bubble shootingBubble, bool activeState = false)
        {
            List<Bubble> closestBubbles = listBubble.FindAll(bubble => bubble.isActive == activeState && CheckCollides(shootingBubble, bubble));

            if (closestBubbles.Count == 0)
            {
                return null;
            }

            List<KeyValuePair<double, Bubble>> closestBubblesWithDistance = new List<KeyValuePair<double, Bubble>>();
            closestBubbles.ForEach(bubbleOnGrid =>
            {
                closestBubblesWithDistance.Add(new KeyValuePair<double, Bubble>(CalculateDistance(shootingBubble, bubbleOnGrid), bubbleOnGrid));
            });
            closestBubblesWithDistance.Sort((x, y) => x.Key.CompareTo(y.Key));

            return closestBubblesWithDistance[0].Value;
        }

        public List<Bubble> FindNeighborBubbles(Bubble bubbleOnGrid)
        {
            List<Bubble> neighbors = new List<Bubble>();

            Vector2[] directions = {
              RotatePoint(Singleton.Instance.BUBBLE_GRID_MARGIN, 0, 0), // right
              RotatePoint(Singleton.Instance.BUBBLE_GRID_MARGIN, 0, DegreeToRadian(60)), // up-right
              RotatePoint(Singleton.Instance.BUBBLE_GRID_MARGIN, 0, DegreeToRadian(120)), // up-left
              RotatePoint(Singleton.Instance.BUBBLE_GRID_MARGIN, 0, DegreeToRadian(180)), // left
              RotatePoint(Singleton.Instance.BUBBLE_GRID_MARGIN, 0, DegreeToRadian(240)), // down-left
              RotatePoint(Singleton.Instance.BUBBLE_GRID_MARGIN, 0, DegreeToRadian(300)) // down-right
            };

            for (int i = 0; i < directions.Length; i++)
            {
                Vector2 direction = directions[i];

                double posX = bubbleOnGrid.Position.X + direction.X;
                double posY = bubbleOnGrid.Position.Y + direction.Y;
                Bubble newBubble = new Bubble(posX, posY, (int)bubbleOnGrid.radius);
                Bubble neighbor = FindClosestBubble(newBubble, true);
                if (neighbor != null && neighbor != bubbleOnGrid && !neighbors.Contains(neighbor))
                {
                    neighbors.Add(neighbor);
                }
            }
            return neighbors;
        }

        public void RemoveMatch(Bubble targetBubble)
        {
            List<Bubble> matchedBubbles = new List<Bubble>();
            matchedBubbles.Add(targetBubble);

            listBubble.ForEach(bubble => bubble.isProcessed = false);
            targetBubble.isProcessed = true;

            List<Bubble> neighbors = FindNeighborBubbles(targetBubble);
            for (int i = 0; i < neighbors.Count; i++)
            {
                Bubble neighbor = neighbors[i];

                if (!neighbor.isProcessed)
                {
                    neighbor.isProcessed = true;

                    if (neighbor.colorBubble.Equals(targetBubble.colorBubble))
                    {
                        matchedBubbles.Add(neighbor);
                        neighbors = Enumerable.Concat(neighbors, FindNeighborBubbles(neighbor)).ToList();
                    }
                }
            }

            if (matchedBubbles.Count >= 3)
            {
                _explosionInstance.Play();
                matchedBubbles.ForEach(bubble =>
                {
                    bubble.isActive = false;
                    amountBubbleColor[shortToIndex[bubble.colorBubble]]--;
                    listEffectPosition.Add(bubble.Position);
                });
            }
        }

        public void DropFloatingBubbles()
        {
            List<Bubble> activeBubbles = listBubble.FindAll(bubble => bubble.isActive);
            activeBubbles.ForEach(bubble => bubble.isProcessed = false);

            List<Bubble> neighbors = activeBubbles.FindAll(bubble => bubble.Position.Y - Singleton.Instance.BUBBLE_GRID_MARGIN <= Singleton.Instance.BUBBLE_START_POS.Y);

            // process all bubbles that form a chain with the ceiling bubbles
            for (int i = 0; i < neighbors.Count; i++)
            {
                Bubble neighbor = neighbors[i];

                if (!neighbor.isProcessed)
                {
                    neighbor.isProcessed = true;
                    neighbors = Enumerable.Concat(neighbors, FindNeighborBubbles(neighbor)).ToList();
                }
            }

            // any bubble that is not isProcessed doesn't touch the ceiling
            activeBubbles.FindAll(bubble => !bubble.isProcessed).ForEach(bubble =>
            {
                bubble.isActive = false;
                amountBubbleColor[shortToIndex[bubble.colorBubble]]--;
                listEffectPosition.Add(bubble.Position);
            });
        }

        public int stageIndexToListIndex()
        {
            int listIndex = ((Singleton.Instance.MAIN_STAGE - 1) * Singleton.Instance.SUB_STAGE_AMOUNT) + (Singleton.Instance.SUB_STAGE - 1);
            return listIndex;
        }

        public void GenerateBubbles()
        {
            List<string[]> currentStage = stageList[stageIndexToListIndex()];
            for (int row = 0; row < Singleton.Instance.BUBBLE_SIZE.Y; row++)
            {
                for (int col = 0; col < Singleton.Instance.BUBBLE_SIZE.X; col++)
                {
                    string color = currentStage[row][col];
                    if (color == "X")
                    {
                        CreateBubble(col * Singleton.Instance.BUBBLE_GRID_MARGIN, row * Singleton.Instance.BUBBLE_GRID_MARGIN, Singleton.Instance.BUBBLE_COLOR_DIC[color]);
                    }
                    else
                    {
                        CreateBubble(col * Singleton.Instance.BUBBLE_GRID_MARGIN, row * Singleton.Instance.BUBBLE_GRID_MARGIN, Singleton.Instance.BUBBLE_COLOR_DIC[color]);
                        amountBubbleColor[shortToIndex[Singleton.Instance.BUBBLE_COLOR_DIC[color]]]++;
                    }
                }
                haveMargin = (haveMargin + 1) % 2;
            }
        }

        public void CreateBubble(double x, double y, string color)
        {
            int row = (int)System.Math.Floor(y / Singleton.Instance.BUBBLE_GRID_MARGIN);
            int col = (int)System.Math.Floor(x / Singleton.Instance.BUBBLE_GRID_MARGIN);

            int startX;
            startX = (haveMargin % 2 == 0) ? 0 : (int)(Singleton.Instance.BUBBLE_GRID_MARGIN / 2);
            int center = Singleton.Instance.BUBBLE_GRID_MARGIN / 2;

            double posX = Singleton.Instance.BUBBLE_START_POS.X + ((Singleton.Instance.BUBBLE_GRID_MARGIN + Singleton.Instance.BUBBLE_GAP) * col) + startX;
            double posY = Singleton.Instance.BUBBLE_START_POS.Y + ((Singleton.Instance.BUBBLE_GRID_MARGIN + Singleton.Instance.BUBBLE_GAP - 4) * row);

            bool isActive = (color != "Black") ? true : false;
            listBubble.Add(new Bubble(posX, posY, Singleton.Instance.BUBBLE_GRID_MARGIN / 2, color, isActive));
        }

        public void CeilDropping(int substage)
        {
            MoveBubble();
            if (Singleton.Instance.SUBSTAGE == substage)
                GenerateBubblesAt(3, 1);
            else
                GenerateBubblesWithRange(1);
        }

        public void MoveBubble()
        {
            listBubble.ForEach(bubble =>
            {
                bubble.Position.Y += Singleton.Instance.BUBBLE_GRID_MARGIN;
            });
        }

        public void GenerateBubblesWithRange(int amountOfRow)
        {
            List<string[]> currentStage = stageList[stageIndexToListIndex()];
            for (int row = 0; row < amountOfRow; row++)
            {
                haveMargin = (haveMargin + 1) % 2;
                for (int col = 0; col < Singleton.Instance.BUBBLE_SIZE.X; col++)
                {
                    string color = currentStage[row][col];
                    if (color == "X")
                    {
                        CreateBubble(col * Singleton.Instance.BUBBLE_GRID_MARGIN, row * Singleton.Instance.BUBBLE_GRID_MARGIN, Singleton.Instance.BUBBLE_COLOR_DIC[color]);
                    }
                    else
                    {
                        CreateBubble(col * Singleton.Instance.BUBBLE_GRID_MARGIN, row * Singleton.Instance.BUBBLE_GRID_MARGIN, Singleton.Instance.BUBBLE_COLOR_DIC[color]);
                        amountBubbleColor[shortToIndex[Singleton.Instance.BUBBLE_COLOR_DIC[color]]]++;
                    }
                }
                listBubble.RemoveAll(bubble => (int)System.Math.Floor(bubble.Position.Y / 75) >= Singleton.Instance.BUBBLE_SIZE.Y && !bubble.isActive);
            }
        }

        public void GenerateBubblesAt(int startRow, int amountOfRow)
        {
            List<string[]> currentStage = stageList[stageIndexToListIndex()];
            for (int row = startRow; row < startRow+amountOfRow; row++)
            {
                for (int col = 0; col < Singleton.Instance.BUBBLE_SIZE.X; col++)
                {
                    string color = currentStage[row][col];
                    if (color == "X")
                    {
                        CreateBubble(col * Singleton.Instance.BUBBLE_GRID_MARGIN, row * Singleton.Instance.BUBBLE_GRID_MARGIN, Singleton.Instance.BUBBLE_COLOR_DIC[color]);
                    }
                    else
                    {
                        CreateBubble(col * Singleton.Instance.BUBBLE_GRID_MARGIN, row * Singleton.Instance.BUBBLE_GRID_MARGIN, Singleton.Instance.BUBBLE_COLOR_DIC[color]);
                        amountBubbleColor[shortToIndex[Singleton.Instance.BUBBLE_COLOR_DIC[color]]]++;
                    }
                }
                haveMargin = (haveMargin + 1) % 2;
                listBubble.RemoveAll(bubble => (int)System.Math.Floor(bubble.Position.Y / 75) >= Singleton.Instance.BUBBLE_SIZE.Y && !bubble.isActive);
            }
        }

        public void InitiateBubble()
        {
            float posX = (Singleton.Instance.GAME_SCREEN_SIZE.X / 2f) + Singleton.Instance.GAME_SCREEN_POSITION.X;
            float posY = 870;

            // ? Only Random from the pool of Color
            List<int> viableColor = new List<int>();
            for (int i = 0; i < amountBubbleColor.Length; i++)
            {
                if (amountBubbleColor[i] != 0)
                {
                    viableColor.Add(i);
                }
            }

            // ? Generate 4 Bubble, 3 for Cartridge, 1 for Cannon
            for (int i = 0; i < Singleton.Instance.CANNON_CARTRIDGE_SIZE; i++)
            {
                _normalBubbles[i] = new Bubble(posX, posY, Singleton.Instance.BUBBLE_GRID_MARGIN / 2, BubbleColorList[viableColor[RandomNumber(0, viableColor.Count)]], Singleton.Instance.BUBBLE_SPEED, 0, 0, new Vector2(posX, posY));
            }

            Singleton.Instance._shootingBubble = _normalBubbles[0];
        }

        // ? Cannon
        public void GetNewBubble()
        {
            Singleton.Instance._shootingBubble.Position.X = Singleton.Instance._shootingBubble.originalPos.X;
            Singleton.Instance._shootingBubble.Position.Y = Singleton.Instance._shootingBubble.originalPos.Y;
            Singleton.Instance._shootingBubble.Velocity.X = Singleton.Instance._shootingBubble.Velocity.Y = 0;

            // ? Only Random from the pool of Color
            List<int> viableColor = new List<int>();
            for (int i = 0; i < amountBubbleColor.Length; i++)
            {
                if (amountBubbleColor[i] != 0)
                {
                    viableColor.Add(i);
                }
            }

            for (int i = 0; i < Singleton.Instance.CANNON_CARTRIDGE_SIZE - 1; i++)
            {
                _normalBubbles[i] = _normalBubbles[i + 1];
            }

            float posX = (Singleton.Instance.GAME_SCREEN_SIZE.X / 2f) + Singleton.Instance.GAME_SCREEN_POSITION.X;
            float posY = 870;
            _normalBubbles[Singleton.Instance.CANNON_CARTRIDGE_SIZE - 1] = new Bubble(posX, posY, Singleton.Instance.BUBBLE_GRID_MARGIN / 2, BubbleColorList[viableColor[RandomNumber(0, viableColor.Count)]], Singleton.Instance.BUBBLE_SPEED, 0, 0, new Vector2(posX, posY));

            Singleton.Instance._shootingBubble.colorBubble = _normalBubbles[0].colorBubble;
        }

        public void HandleCollision(Bubble bubbleOnGrid)
        {
            bubbleOnGrid.colorBubble = Singleton.Instance._shootingBubble.colorBubble;
            bubbleOnGrid.isActive = true;
            amountBubbleColor[shortToIndex[bubbleOnGrid.colorBubble]]++;
            GetNewBubble();
            RemoveMatch(bubbleOnGrid);
            if (Singleton.Instance.SUB_STAGE != 3)
                DropFloatingBubbles();
        }
    }
}
