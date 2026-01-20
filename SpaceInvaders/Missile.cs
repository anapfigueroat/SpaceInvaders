using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class Missile : GameObject 
    {
        public Vector2D position; // Missile position
        public double speed; // Missile speed
        public int lives; // Missile lives
        public Bitmap picture; // Image depicting the missile

        public Missile(Vector2D startPosition, double speed, Bitmap image, int initialLives)
        {
            position = startPosition;
            this.speed = speed;
            picture = image;
            lives = initialLives;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            // Move the missile upwards
            position.y -= speed * deltaT;
            // If the missile goes off the top of the screen, it loses a life
            if (position.y + picture.Height < 0)
            {
                lives = 0;
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

    }
}