
using System.Collections.Generic;
using UnityEngine;

namespace Depender.Types
{
    public class ModdedObject
    {
        // General
        public GameObject Object;
        public string Name;
        public float Price;
        public List<BoundingBox> BoundingBoxes = new List<BoundingBox>();

        // Recolor options
        public bool Recolorable;
        public Color[] Colors;
        public string Shader;

        public virtual void Decorate()
        {
            Debug.Log("Decorating: " + Name);
            BuildableObject BO = Object.GetComponent<BuildableObject>();
            BO.price = Price;
            BO.setDisplayName(Name);
            BO.dontSerialize = true;
            BO.isPreview = true;
            string shader;
            if (Recolorable)
            {
                Debug.Log("Recolorable! : " + Shader);
                CustomColors cc = Object.AddComponent<CustomColors>();
                cc.setColors(Colors);
                shader = "CustomColors" + Shader;
            }
            else
            {
                shader = Shader;
            }
            foreach (Material material in AssetManager.Instance.objectMaterials)
            {
                if (material.name == shader)
                {
                    SetMaterial(Object, material);

                    // edge case for fences
                    Fence fence = Object.GetComponent<Fence>();

                    if (fence != null)
                    {
                        if (fence.flatGO != null)
                        {
                            SetMaterial(fence.flatGO, material);
                        }

                        if (fence.postGO != null)
                        {
                            SetMaterial(fence.postGO, material);
                        }
                    }

                    break;
                }
            }
        }
        public void SetMaterial(GameObject go, Material material)
        {
            // Go through all child objects and recolor		
            Renderer[] renderCollection;
            renderCollection = go.GetComponentsInChildren<Renderer>();

            foreach (Renderer render in renderCollection)
            {
                render.sharedMaterial = material;
            }
        }
    }
}



