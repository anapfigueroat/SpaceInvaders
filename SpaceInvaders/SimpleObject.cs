using System.Drawing;
namespace SpaceInvaders
{
    abstract class SimpleObject : GameObject
    {
        public Vector2D position; // Missile position
        public int lives; // Missile lives
        public Bitmap picture; // Image depicting the missile

        public SimpleObject(Vector2D position, Bitmap picture, int lives)
        {
            this.position = position;
            this.picture = picture;
            this.lives = lives;
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            graphics.DrawImage(picture, (float)position.x, (float)position.y);
        }

        public override bool IsAlive()
        {
            return lives > 0;
        }

        public override void Collision(Missile m)
        {
            
        }
    }
}
