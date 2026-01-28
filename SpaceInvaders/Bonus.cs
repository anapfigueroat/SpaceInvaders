using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public enum BonusType
    {
        ExtraLife
    }

    class Bonus : SimpleObject
    {
        private BonusType bonusType;
        private double fallSpeedPixelsPerSecond = 100; // Speed at which the bonus falls

        public Bonus(Vector2D startPosition, Bitmap image, BonusType type) : base(Side.Neutral, startPosition, image, 1)
        {
            this.bonusType = type;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            // Move the bonus downwards
            position.y += fallSpeedPixelsPerSecond * deltaT; // Move down at 100 pixels per second
            // If the bonus goes off-screen, it is no longer alive
            if (position.y > gameInstance.gameSize.Height)
            {
                lives = 0;
            }
        }

        // Called when a missile intersects this object (pixel-collision count provided).
        protected override void OnCollision(Missile m, int collisionCount)
        {
            if (m == null) return;
            if (collisionCount <= 0) return;

            // Consume the bonus and damage the missile.
            try
            {
                // Reduce missile life (one unit per collision)
                m.lives = Math.Max(0, m.lives - 1);

                // Destroy the bonus so it will be removed on next update
                this.lives = 0;

                // If missile belongs to player, apply extra life
                if (m.ObjectSide == Side.Ally)
                {
                    ApplyBonus(Game.game);
                }
            }
            catch
            {
                // swallow unexpected error
            }
        }

        private void ApplyBonus(Game gameInstance)
        {
            if (gameInstance == null) return;

            // Find the player's spaceship (assumes player is a Spaceship in gameObjects)
            Spaceship playerShip = gameInstance.gameObjects
                .OfType<Spaceship>()
                .FirstOrDefault(s => s.ObjectSide == Side.Ally);

            if (playerShip == null) return;

            // Only bonus: Extra life
            playerShip.lives += 1;
        }
    }
}