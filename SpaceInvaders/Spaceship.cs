using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class Spaceship : SimpleObject
    {
       
       protected double speedPixelsPerSecond; // ship's movement speed
       public Missile missile; // ship's missile

       public Spaceship(Side side, Vector2D startPosition, double speed, Bitmap image, int initialLives) : base(side, startPosition, image, initialLives)
       {
           speedPixelsPerSecond = speed;
       }

        public override void Update(Game gameInstance, double deltaT) { } // do nothing by default

        public void Shoot(Game gameInstance)
        {
            double missileSpeed = (Side == Side.Enemy) ? -500 : 500;

            if (missile == null || !missile.IsAlive())
            {
                Bitmap missileImage = SpaceInvaders.Properties.Resources.shoot1;
                Vector2D missileStartPos = new Vector2D(
                    position.x + (picture.Width - missileImage.Width) / 2,
                    position.y - missileImage.Height
                );
                missile = new Missile(this.Side, missileStartPos, missileSpeed, SpaceInvaders.Properties.Resources.shoot1, 1);
                gameInstance.AddNewGameObject(missile);
            }
        }

        protected override void OnCollision(Missile m, int collisionCount)
        {
            int damage = Math.Min(m.lives, lives);
            m.lives -= damage;
            lives -= damage;
        }
    }
}