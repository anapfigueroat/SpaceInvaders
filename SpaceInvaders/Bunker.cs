namespace SpaceInvaders
{
    class Bunker : SimpleObject
    {
        public Bunker(Vector2D position) : base(position, SpaceInvaders.Properties.Resources.bunker, 100){} 
        public override void Update(Game gameInstance, double deltaT){}
    }
}