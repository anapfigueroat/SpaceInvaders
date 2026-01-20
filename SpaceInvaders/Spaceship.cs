using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class Spaceship : GameObject
    {
       
       private double speedPixelsPerSecond; // Player movement speed
       public Vector2D position; // Player position
       public int lives; // Player lives
       public Bitmap picture; // Image depicting the spaceship
       public Missile missile; // Player's missile

       public Spaceship(Vector2D startPosition, double speed, Bitmap image, int initialLives)
       {
           position = startPosition;
           speedPixelsPerSecond = speed;
           picture = image;
           lives = initialLives;
        }


        public override void Update(Game gameInstance, double deltaT)
        {
            //// Example movement logic (left/right with arrow keys)
            if (gameInstance.keyPressed.Contains(Keys.Left))
            {
                position.x -= speedPixelsPerSecond * deltaT;
            }
            if (gameInstance.keyPressed.Contains(Keys.Right))
            {
                position.x += speedPixelsPerSecond * deltaT;
            }
            // Ensure the spaceship stays within game bounds
            position.x = Math.Max(0, Math.Min(position.x, gameInstance.gameSize.Width - picture.Width));

            if (gameInstance.keyPressed.Contains(Keys.Space))
            {
                Shoot(gameInstance);
                gameInstance.ReleaseKey(Keys.Space); // Prevent continuous shooting
            }
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            graphics.DrawImage(picture, (float)position.x, (float)position.y);
        }

        public override bool IsAlive()
        {
            return lives > 0;
        }

        public void Shoot(Game gameInstance)
        {
            if (missile == null || !missile.IsAlive())
            {
                Bitmap missileImage = SpaceInvaders.Properties.Resources.shoot1;
                Vector2D missileStartPos = new Vector2D(
                    position.x + (picture.Width - missileImage.Width) / 2,
                    position.y - missileImage.Height
                );
                missile = new Missile(missileStartPos, 500, SpaceInvaders.Properties.Resources.shoot1, 1);
                gameInstance.AddNewGameObject(missile);
            }
        }

    }
}