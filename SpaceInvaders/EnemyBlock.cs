using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    class EnemyBlock : GameObject
    {
        private HashSet<Spaceship> enemyShips;    // ships in the block
        private int baseWidth;                    // block width
        public Size Size {  get; set; }        // Block size (width, height)
        public Vector2D Position { get; set; } // Block position (top left corner)
        private int direction = 1;                // 1 = right, -1 = left
        private double speed = 50.0;              // starting speed of the enemy block
        private const double speedCoef = 1.1;     // speed change coefficient 
        private const int drop = 25;              // Shift the block downards

        public EnemyBlock(HashSet<Spaceship> enemyShips, int baseWidth, Size size, Vector2D position) : base(Side.Enemy)
        {
            this.enemyShips = enemyShips;
            this.baseWidth = baseWidth;
            Size = size;
            Position = position;
        }

        public void AddLine(int nbShips, int nbLives, Bitmap shipImage)
        {
            if (nbShips <= 0) return;

            double offset = (enemyShips.Count == 0 ? 0 : 15); // add offset to rows except the 1st one

            for (int i = 0; i < nbShips; i++)
            {
                Vector2D pos = new Vector2D(
                    Position.x + (i + 0.5) * (baseWidth/nbShips) - shipImage.Width / 2,
                    Position.y + Size.Height + offset
                );

                enemyShips.Add(new Spaceship(this.Side, pos, 0.0, shipImage, nbLives));
            }

            UpdateSize();
        }

        public void UpdateSize()
        {
            // If no ships are alive — size 0
            var alive = enemyShips.Where(s => s.IsAlive());
            if (!alive.Any())
            {
                Size = new Size(0, 0);
                return;
            }

            double left = alive.Min(s => s.position.x);
            double top = alive.Min(s => s.position.y);
            double right = alive.Max(s => s.position.x + s.picture.Width);
            double bottom = alive.Max(s => s.position.y + s.picture.Height);

            Size = new Size((int)Math.Ceiling(right - left),
                            (int)Math.Ceiling(bottom - top));

            Position = new Vector2D(left, top);
        }

        public override void Update(Game gameInstance, double deltaT) {

            double dx = direction * speed * deltaT;

            foreach (var spaceShip in enemyShips)
                spaceShip.position.x += dx;

            UpdateSize();

            bool reachedLeft = (Position.x <= 0) && (direction == -1);
            bool reachedRight = (Position.x + Size.Width >= gameInstance.gameSize.Width) && (direction == 1);

            if (reachedLeft || reachedRight)
            {
                foreach (var spaceShip in enemyShips)
                    spaceShip.position.y += drop;

                UpdateSize();

                direction *= -1;
                speed *= speedCoef;
            }
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            foreach (var ship in enemyShips)
            {
                if (ship.lives > 0)
                    ship.Draw(gameInstance, graphics);
            }
        }

        public override bool IsAlive()
        {
            return enemyShips.Any(ship => ship.IsAlive());
        }

        public override void Collision(Missile m)
        {
            double mX = m.position.x;
            double mY = m.position.y;
            double mW = m.picture.Width;
            double mH = m.picture.Height;

            foreach (var spaceShip in enemyShips)
            {
                if (!spaceShip.IsAlive()) continue;

                double sX = spaceShip.position.x;
                double sY = spaceShip.position.y;
                double sW = spaceShip.picture.Width;
                double sH = spaceShip.picture.Height;

                if (mX > sX + sW || mX + mW < sX || mY > sY + sH || mY + mH < sY) continue;  
                
                spaceShip.Collision(m);

                if (!m.IsAlive()) { break; }
            }
        }
    }
}
