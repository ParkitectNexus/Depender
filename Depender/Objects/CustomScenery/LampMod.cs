namespace Depender.Types
{
    public class LampMod : ModdedObject
    {
        public override void Decorate()
        {
            Object.AddComponent<PathAttachment>();
            base.Decorate();
        }
    }
}
