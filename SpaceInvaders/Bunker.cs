using System;
using System.Drawing;

namespace SpaceInvaders
{
    class Bunker : SimpleObject
    {
        public Bunker(Vector2D position) : base(position, SpaceInvaders.Properties.Resources.bunker, 100){} 
        public override void Update(Game gameInstance, double deltaT){}

        /// <summary>
        /// Handle collision with a missile
        /// </summary>
        public override void Collision(Missile m)
        {
            if (TestContainingRectangles(m))
            {
                HandlePixelCollision(m);
            }
        }

        /// <summary>
        /// Fast check: do the bounding boxes intersect?
        /// </summary>
        private bool TestContainingRectangles(Missile m)
        {
            // Missile dimensions
            double mX = m.position.x;
            double mY = m.position.y;
            double mW = m.picture.Width;
            double mH = m.picture.Height;

            // Bunker dimensions
            double bX = this.position.x;
            double bY = this.position.y;
            double bW = this.picture.Width;
            double bH = this.picture.Height;

            // Check if disjoint (standard AABB collision logic)
            if (mX > bX + bW || mX + mW < bX || mY > bY + bH || mY + mH < bY)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Precise check: pixel by pixel intersection
        /// </summary>
        private void HandlePixelCollision(Missile m)
        {
            int collisionCount = 0;

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
                                collisionCount++;
                            }
                        }
                    }
                }
            }

            // Reduce missile lives by the number of pixels hit
            if (collisionCount > 0)
            {
                m.lives -= collisionCount;
            }
        }

    }
}