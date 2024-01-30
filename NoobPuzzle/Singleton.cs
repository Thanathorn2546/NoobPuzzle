using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NoobBubble.Manager;
using NoobBubble.Objects;
using System.Collections.Generic;

namespace NoobBubble
{
    /// <summary>
    ///
    /// <example>
    /// For example:
    /// <code>
    /// finalScore = Singleton.Instance.Score;
    /// </code>
    /// Result in accessing the Score variable in Singleton Instance.
    /// And this instance is the only one throughout the game cycle.
    /// </example>
    /// </summary>
    public class Singleton
    {
        // ? System-related
        public int heightScreen = 1080;
        public int widthScreen = 1920;
        public bool isExitGame = false;
        public double gameTicksInMilliSec = 87f;
        public double swapDelayInMilliSec = 213f;

        // ? Graphic Manager
        public GraphicsDeviceManager graphicsDeviceManager;

        // ? ContentManager
        public ContentManager contentManager;

        // ? SceneManager
        public SceneManager sceneManager = new SceneManager();


        // ? PlayScene
        public int ceilDroppingTickInSec = 40;
        public int SCORE = 0;
        public int MAIN_STAGE = 1; // ? The main stage number (1)-1
        public int SUB_STAGE = 1; // ? The sub stage number 1-(1)
        public Bubble _shootingBubble;
        public Dictionary<string, string> BUBBLE_COLOR_DIC = new Dictionary<string, string>(){
            {"B", "Blue"},
            {"C", "Cyan"},
            {"G", "Green"},
            {"R", "Red"},
            {"Y", "Yellow"},
            {"X", "Black"}
        };
        public Cannon _cannon;
        public bool isShooting = false;
        public int SUB_STAGE_AMOUNT = 3;
        public int CANNON_CARTRIDGE_SIZE = 4;
        public int CANNON_SPECIAL_CARTRIDGE_SIZE = 3;
        public int CARTRIDGE_BUBBLE_GRID_MARGIN = 41;
        public int BUBBLE_GRID_MARGIN = 78;
        public int BUBBLE_GAP = 1;
        public int BUBBLE_SPEED = 40;
        public int BOSS_HEALTH = 5;
        public int PLAYER_HEALTH = 3;
        public bool PLAYER_LOSE = false;
        public Vector2 BUBBLE_SIZE = new Vector2(9, 10);
        public Vector2 BUBBLE_START_POS = new Vector2(586, 0);
        public Vector2 GAME_SCREEN_SIZE = new Vector2(750,1080);
        public Vector2 GAME_SCREEN_POSITION = new Vector2(585, 0);
        public Vector2 HUD_LEFT_SCREEN_SIZE = new Vector2(585,1080);
        public Vector2 HUD_LEFT_SCREEN_POSITION = new Vector2(0, 0);
        public Vector2 HUD_RIGHT_SCREEN_SIZE = new Vector2(585,1080);
        public Vector2 HUD_RIGHT_SCREEN_POSITION = new Vector2(1335, 0);
        public float soundVolume = 0.5f;

        // ? Singleton Stuff
        private static Singleton instance;
        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }

        public int SUBSTAGE { get; internal set; }
    }
}
