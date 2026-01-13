using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    class Spaceship : GameObject
    {
       
       private double speedPixelsPerSecond; // Player movement speed
       public Vector2D position; // Player position
       public int lives; // Player lives
       public Bitmap picture; // Image depicting the spaceship

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
            //if (gameInstance.IsKeyPressed(Keys.Left))
            //{
            //    position.x -= speedPixelsPerSecond * deltaT;
            //}
            //if (gameInstance.IsKeyPressed(Keys.Right))
            //{
            //    position.x += speedPixelsPerSecond * deltaT;
            //}
            //// Ensure the spaceship stays within game bounds
            //position.x = Math.Max(0, Math.Min(position.x, gameInstance.GameSize.Width - picture.Width));
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            graphics.DrawImage(picture, (float)position.x, (float)position.y);
        }

        public override bool IsAlive()
        {
            return lives > 0;
        }

    }
}