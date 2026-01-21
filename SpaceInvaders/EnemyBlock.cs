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
        protected Size Size {  get; set; }        // Block size (width, height)
        protected Vector2D Position { get; set; } // Block position (top left corner)

        public EnemyBlock(HashSet<Spaceship> enemyShips, int baseWidth, Size size, Vector2D position)
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

                enemyShips.Add(new Spaceship(pos, 0.0, shipImage, nbLives));
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
        }

        public override void Update(Game gameInstance, double deltaT) {  }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            foreach (var ship in enemyShips)
                ship.Draw(gameInstance, graphics);
        }

        public override bool IsAlive()
        {
            return enemyShips.Any(ship => ship.IsAlive());
        }

        public override void Collision(Missile m) { } // TODO

    }
}
