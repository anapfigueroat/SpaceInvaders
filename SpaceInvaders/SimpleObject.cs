using System.Drawing;
namespace SpaceInvaders
{
    abstract class SimpleObject : GameObject
    {
        public Vector2D position; // Missile position
        public int lives; // Missile lives
        public Bitmap picture; // Image depicting the missile

        public SimpleObject(Side side, Vector2D position, Bitmap picture, int lives) : 
            base(side)
        {
            this.position = position;
            this.picture = picture;
            this.lives = lives;
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            graphics.DrawImage(picture, (float)position.x, (float)position.y, picture.Width, picture.Height);        
        }

        public override bool IsAlive()
        {
            return lives > 0;
        }

        /// <summary>
        /// Handle collision with a missile
        /// </summary>
        public override void Collision(Missile m)
        {
            if (!TestContainingRectangles(m)) return;

            int collisionCount = CountPixelCollision(m);

            if (collisionCount > 0)
                OnCollision(m, collisionCount);
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

            // Object dimensions
            double oX = this.position.x;
            double oY = this.position.y;
            double oW = this.picture.Width;
            double oH = this.picture.Height;

            // Check if disjoint (standard AABB collision logic)
            if (mX > oX + oW || mX + mW < oX || mY > oY + oH || mY + mH < oY)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Precise check: pixel by pixel intersection
        /// </summary>
        private int CountPixelCollision(Missile m)
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
                        // Calculate corresponding position on the object image
                        int bPixelX = (int) System.Math.Round(m.position.x + i - this.position.x);
                        int bPixelY = (int) System.Math.Round(m.position.y + j - this.position.y);

                        // Check if valid coordinate on object
                        if (bPixelX >= 0 && bPixelX < this.picture.Width &&
                            bPixelY >= 0 && bPixelY < this.picture.Height)
                        {
                            // Check if object pixel is opaque (part of the bunker)
                            if (this.picture.GetPixel(bPixelX, bPixelY).A != 0)
                            {
                                // Collision confirmed: increment collision count
                                collisionCount++;
                            }
                        }
                    }
                }
            }
            return collisionCount;
        }

        protected abstract void OnCollision(Missile m, int numberOfPixelsInCollision);
    }
}
