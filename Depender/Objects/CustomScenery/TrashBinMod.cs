namespace Depender.Types
{
    public class TrashBinMod : ModdedObject
    {

        public override void Decorate()
        {
            Object.AddComponent<TrashBin>();
            base.Decorate();
        }
    }
}

