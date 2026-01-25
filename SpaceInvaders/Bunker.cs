using System;
using System.Drawing;

namespace SpaceInvaders
{
    class Bunker : SimpleObject
    {
        public Bunker(Vector2D position) : base(position, SpaceInvaders.Properties.Resources.bunker, 100) { }
        public override void Update(Game gameInstance, double deltaT) { }

        protected override void OnCollision(Missile m, int collisionCount)
        {
            m.lives -= collisionCount;
            ErasePixels(m);
        }

        private void ErasePixels(Missile m)
        {
            // Iterate through missile pixels
            for (int i = 0; i < m.picture.Width; i++)
            {
                for (int j = 0; j < m.picture.Height; j++)
                {
                    // Check if missile pixel is not transparent
                    if (m.picture.GetPixel(i, j).A != 0)
                    {
                        // Calculate corresponding position on the bunker image
                        int bPixelX = (int)(m.position.x + i - this.position.x);
                        int bPixelY = (int)(m.position.y + j - this.position.y);

                        // Check if valid coordinate on bunker
                        if (bPixelX >= 0 && bPixelX < this.picture.Width &&
                            bPixelY >= 0 && bPixelY < this.picture.Height)
                        {
                            // Check if bunker pixel is opaque (part of the bunker)
                            if (this.picture.GetPixel(bPixelX, bPixelY).A != 0)
                            {
                                // Collision confirmed: erase bunker pixel
                                this.picture.SetPixel(bPixelX, bPixelY, Color.Transparent);
                            }
                        }
                    }
                }
            }
        }
    }
}