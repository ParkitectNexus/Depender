namespace Depender.Types
{
    public class SeatingMod : ModdedObject
    {
        public bool hasBackRest;
        public override void Decorate()
        {
            Object.AddComponent<Seating>().hasBackRest = hasBackRest;
            base.Decorate();
        }
    }
}

