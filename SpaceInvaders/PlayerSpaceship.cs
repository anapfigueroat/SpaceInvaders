using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class PlayerSpaceship : Spaceship
    {
        public PlayerSpaceship(Vector2D startPosition, double speed, Bitmap image, int initialLives) 
            : base(Side.Ally, startPosition, speed, image, initialLives) { }

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
            // Method chaining  
            base.Draw(gameInstance, graphics);

            // Displays the amount of lives
            graphics.DrawString("Lives: " + lives, Game.defaultFont, Game.blackBrush, 10, 10);
        }
    }
}
