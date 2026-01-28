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
       protected double speedPixelsPerSecond; // ship's base movement speed
       public Missile missile; // primary ship missile
 

       public Spaceship(Side side, Vector2D startPosition, double speed, Bitmap image, int initialLives) : base(side, startPosition, image, initialLives)
       {
           speedPixelsPerSecond = speed;
       }

        public override void Update(Game gameInstance, double deltaT)
        {
            if (gameInstance == null) return;

            // Handle movement input (left/right)
            double currentSpeed = speedPixelsPerSecond;
            if (gameInstance.keyPressed.Contains(Keys.Left))
            {
                position.x -= currentSpeed * deltaT;
            }
            if (gameInstance.keyPressed.Contains(Keys.Right))
            {
                position.x += currentSpeed * deltaT;
            }

            // Keep ship in bounds
            position.x = Math.Max(0, Math.Min(position.x, gameInstance.gameSize.Width - (picture?.Width ?? 0)));

            // Fire
            if (gameInstance.keyPressed.Contains(Keys.Space))
            {
                Shoot(gameInstance);
                gameInstance.ReleaseKey(Keys.Space); // Prevent continuous shooting until re-pressed
            }
        }

        public void Shoot(Game gameInstance)
        {
            if (gameInstance == null) return;

            double missileSpeed = (this.ObjectSide == Side.Enemy) ? -500 : 500;

            Bitmap missileImage = SpaceInvaders.Properties.Resources.shoot1;
            if (missileImage == null || picture == null) return; // defensive

            // Primary missile: same logic as before
            if (missile == null || !missile.IsAlive())
            {
                Vector2D missileStartPos = new Vector2D(
                    position.x + (picture.Width - missileImage.Width) / 2.0,
                    (this.ObjectSide == Side.Enemy) ? position.y + picture.Height : position.y - missileImage.Height
                );
                missile = new Missile(this.ObjectSide, missileStartPos, missileSpeed, missileImage, 1);
                gameInstance.AddNewGameObject(missile);
            }
        }

        protected override void OnCollision(Missile m, int collisionCount)
        {
            if (m == null || collisionCount <= 0) return;

            // Normal damage behavior
            int damage = Math.Min(m.lives, Math.Min(lives, collisionCount));
            m.lives -= damage;
            lives -= damage;

            if (lives <= 0 && this.ObjectSide == Side.Enemy)
            {
                DropBonus(Game.game);
            }
        }

        private void DropBonus(Game gameInstance)
        {
            if (gameInstance == null) return;

            if (new Random().NextDouble() < 0.3) // 30% chance to drop a bonus
            {
                Bitmap bonusImage = SpaceInvaders.Properties.Resources.extraLife.png;
                if (bonusImage == null || picture == null) return;

                Vector2D bonusStartPos = new Vector2D(
                    position.x + (picture.Width - bonusImage.Width) / 2.0,
                    position.y + picture.Height
                );

                // choose random bonus
                var types = Enum.GetValues(typeof(BonusType));
                BonusType chosen = (BonusType)types.GetValue(new Random().Next(types.Length));

                Bonus bonus = new Bonus(bonusStartPos, bonusImage, chosen);
                gameInstance.AddNewGameObject(bonus);
            }
        }

       
        
    }
}