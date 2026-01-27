using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace SpaceInvaders
{
    /// <summary>
    /// This class represents the entire game, it implements the singleton pattern
    /// </summary>
    /// 
    enum GameState
    {
        Play, 
        Pause,
        Won,
        Lost
    }
    class Game
    {

        #region GameObjects management

        private Spaceship playerShip;

        private EnemyBlock enemies;

        private GameState state;

        /// <summary>
        /// Set of all game objects currently in the game
        /// </summary>
        public HashSet<GameObject> gameObjects = new HashSet<GameObject>();

        /// <summary>
        /// Set of new game objects scheduled for addition to the game
        /// </summary>
        private HashSet<GameObject> pendingNewGameObjects = new HashSet<GameObject>();

        /// <summary>
        /// Schedule a new object for addition in the game.
        /// The new object will be added at the beginning of the next update loop
        /// </summary>
        /// <param name="gameObject">object to add</param>
        public void AddNewGameObject(GameObject gameObject)
        {
            pendingNewGameObjects.Add(gameObject);
        }
        #endregion

        #region game technical elements
        /// <summary>
        /// Size of the game area
        /// </summary>
        public Size gameSize { get; }

        /// <summary>
        /// State of the keyboard
        /// </summary>
        public HashSet<Keys> keyPressed = new HashSet<Keys>();

        #endregion

        #region static fields (helpers)

        /// <summary>
        /// Singleton for easy access
        /// </summary>
        public static Game game { get; private set; }

        /// <summary>
        /// A shared black brush
        /// </summary>
        public static Brush blackBrush = new SolidBrush(Color.Black);

        /// <summary>
        /// A shared simple font
        /// </summary>
        public static Font defaultFont = new Font("Times New Roman", 24, FontStyle.Bold, GraphicsUnit.Pixel);
        #endregion


        #region constructors
        /// <summary>
        /// Singleton constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        /// 
        /// <returns></returns>
        public static Game CreateGame(Size gameSize)
        {
            if (game == null)
                game = new Game(gameSize);
            return game;
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        private Game(Size gameSize)
        {
            this.gameSize = gameSize;
            Init();
        }

        private void Init(){

            gameObjects.Clear();

            this.state = GameState.Play;

            // create player ship
            Bitmap image = SpaceInvaders.Properties.Resources.ship3;

            Vector2D startPosition = new Vector2D(
                (gameSize.Width - image.Width) / 2,
                gameSize.Height - image.Height - 10
            );

            playerShip = new PlayerSpaceship(startPosition, 300, image, 3);

            gameObjects.Add(playerShip);

            // create bunkers

            Bitmap bunkerImage = SpaceInvaders.Properties.Resources.bunker;
            double yPosition = gameSize.Height - bunkerImage.Height - 100;
            for (int i = 1; i <= 3; i++)
            {
                double xPosition = (gameSize.Width / 4.0) * i - (bunkerImage.Width / 2.0);
                
                Bunker bunker = new Bunker(new Vector2D(xPosition, yPosition));
                gameObjects.Add(bunker);
            }

            // create enemy block
            enemies = new EnemyBlock(
                new HashSet<Spaceship>(),
                (int)gameSize.Width - 2 * 20 - 200, // -200 so the block could move
                new Size(0, 0),
                new Vector2D(20, 20)
            );

            Bitmap enemyImage1 = SpaceInvaders.Properties.Resources.ship2;
            Bitmap enemyImage2 = SpaceInvaders.Properties.Resources.ship4;
            Bitmap enemyImage3 = SpaceInvaders.Properties.Resources.ship7;

            enemies.AddLine(nbShips: 6, nbLives: 3, shipImage: enemyImage1);
            enemies.AddLine(nbShips: 4, nbLives: 2, shipImage: enemyImage2);
            enemies.AddLine(nbShips: 9, nbLives: 1, shipImage: enemyImage3);

            gameObjects.Add(enemies);
        }

        #endregion

        #region methods

        /// <summary>
        /// Force a given key to be ignored in following updates until the user
        /// explicitily retype it or the system autofires it again.
        /// </summary>
        /// <param name="key">key to ignore</param>
        public void ReleaseKey(Keys key)
        {
            keyPressed.Remove(key);
        }


        /// <summary>
        /// Draw the whole game
        /// </summary>
        /// <param name="g">Graphics to draw in</param>
        public void Draw(Graphics g)
        {
            foreach (GameObject gameObject in gameObjects)
                gameObject.Draw(this, g); 

            if (state == GameState.Pause)
            {
                g.DrawString("PAUSE", defaultFont, blackBrush, gameSize.Width / 2 - 30, gameSize.Height / 2);
            }
            else if (state == GameState.Won)
            {
                g.DrawString("YOU WON!", defaultFont, blackBrush, gameSize.Width / 2 - 60, gameSize.Height / 2);
                g.DrawString("Press Space", defaultFont, blackBrush, gameSize.Width / 2 - 60, gameSize.Height / 2 + 30);
            }
            else if (state == GameState.Lost)
            {
                g.DrawString("GAME OVER", defaultFont, blackBrush, gameSize.Width / 2 - 70, gameSize.Height / 2);
                g.DrawString("Press Space", defaultFont, blackBrush, gameSize.Width / 2 - 70, gameSize.Height / 2 + 30);
            }
        }

        /// <summary>
        /// Update game
        /// </summary>
        public void Update(double deltaT)
        {

            if (keyPressed.Contains(Keys.P))
            {
                if (state == GameState.Play) state = GameState.Pause;
                else if (state == GameState.Pause) state = GameState.Play;
                ReleaseKey(Keys.P);
            }

            if ((state == GameState.Won || state == GameState.Lost) && keyPressed.Contains(Keys.Space))
            {
                //game restarts when Init() is called
                Init();
                ReleaseKey(Keys.Space);
                return;
            }

            if (state == GameState.Play)
            {
                // Update objects
                gameObjects.UnionWith(pendingNewGameObjects);
                pendingNewGameObjects.Clear();

                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Update(this, deltaT);
                }

                gameObjects.RemoveWhere(gameObject => !gameObject.IsAlive());

                
                // Defeat: Player died
                if (!playerShip.IsAlive())
                {
                    state = GameState.Lost;
                }
                // Victory: All enemies destroyed
                else if (!enemies.IsAlive())
                {
                    state = GameState.Won;
                }
                // Defeat: Enemies reached player level
                else if (enemies.Position.y + enemies.Size.Height > playerShip.position.y)
                {
                    playerShip.lives = 0; // Kill player
                    state = GameState.Lost;
                }
            }
        }
        #endregion
    }
}
