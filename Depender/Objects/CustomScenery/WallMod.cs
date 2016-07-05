namespace Depender.Types
{
    public class WallMod : ModdedObject
    {
        public string category;

        public override void Decorate()
        {
            Wall deco = Object.AddComponent<Wall>();
            deco.blockedSides = Wall.BlockedSide.Back;
            deco.buildOnGrid = true;
            deco.defaultSnapToGridCenter = true;
            deco.categoryTag = category;
            base.Decorate();
        }
    }
}
