using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class Missile : SimpleObject 
    {
        public double speed; // Missile speed

        public Missile(Vector2D startPosition, double speed, Bitmap image, int initialLives) : base(startPosition, image, initialLives)
        {
            this.speed = speed;
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

            foreach (GameObject gameObject in gameInstance.gameObjects)
            {
                if (lives <= 0) break;
                if (ReferenceEquals(gameObject, this)) continue;
                gameObject.Collision(this);
            }
        }

        protected override void OnCollision(Missile m, int collisionCount)
        {
            m.lives = 0;
            lives = 0;
        }
    }
}