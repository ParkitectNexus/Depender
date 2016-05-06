
using UnityEngine;
namespace Depender.Types
{
    public class FenceMod : ModdedObject
    {
        public GameObject FenceFlat;
        public GameObject FencePost;

        public override void Decorate()
        {
            Object.AddComponent<Fence>();

            if (FenceFlat)
            {
                Object.GetComponent<Fence>().flatGO = FenceFlat;
            }

            if (FencePost)
            {
                Object.GetComponent<Fence>().postGO = FencePost;
            }

            Object.GetComponent<Fence>().hasMidPosts = false;
            base.Decorate();
        }
    }
}

