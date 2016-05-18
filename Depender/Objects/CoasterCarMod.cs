using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Depender.Types
{
    public class CoasterCarMod : ModdedObject
    {
        public GameObject FrontCarGO;
        public string CoasterName;
        public Vector3 closedAngleRetraints = new Vector3(0,0,0);

        public override void Decorate()
        {
            List<Attraction> attractions = AssetManager.Instance.getAttractionObjects().ToList();
            foreach (TrackedRide attraction in attractions.OfType<TrackedRide>())
            {
                if (attraction.getUnlocalizedName() == CoasterName)
                {
                    List<CoasterCarInstantiator> TypeList = attraction.carTypes.ToList();
                    CoasterCarInstantiator CarType = ScriptableObject.CreateInstance<CoasterCarInstantiator>();
                    AssetManager.Instance.registerObject(Object.AddComponent<Car>());
                    CarType.carGO = Object;
                    if (FrontCarGO != null)
                    {
                        AssetManager.Instance.registerObject(FrontCarGO.AddComponent<Car>());
                        CarType.frontCarGO = FrontCarGO;
                    }
                    CarType.displayName = Name;
                    CarType.name = Name;
                    AssetManager.Instance.registerObject(CarType);
                    TypeList.Add(CarType);

                    attraction.carTypes = TypeList.ToArray();
                }
            }
            RestraintRotationController controller = Object.AddComponent<RestraintRotationController>();
            controller.closedAngles = closedAngleRetraints;
            if (FrontCarGO != null)
            {
                controller = FrontCarGO.AddComponent<RestraintRotationController>();
                controller.closedAngles = closedAngleRetraints;
            }
            string shader;
            if (Recolorable)
            {
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
            if (FrontCarGO != null)
            {
                // Apply custom colors for FrontCarGO
                
                if (Recolorable)
                {
                    CustomColors cc = FrontCarGO.AddComponent<CustomColors>();
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
                        SetMaterial(FrontCarGO, material);

                        // edge case for fences
                        Fence fence = FrontCarGO.GetComponent<Fence>();

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
        }
    }
}

