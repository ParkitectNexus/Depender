namespace Depender.Types
{
    public class DecoMod : ModdedObject
    {
        public float HeightDelta;
        public float GridSubdivision;
        public bool SnapCenter;
        public bool BuildOnGrid;
        public string category;

        public override void Decorate()
        {
            Deco deco = Object.AddComponent<Deco>();
            deco.heightChangeDelta = HeightDelta;
            deco.defaultGridSubdivision = GridSubdivision;
            deco.buildOnGrid = BuildOnGrid;
            deco.defaultSnapToGridCenter = SnapCenter;
            deco.categoryTag = category;
            base.Decorate();
        }
    }
}
