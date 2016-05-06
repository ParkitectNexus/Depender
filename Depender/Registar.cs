using Depender.Types;
using System.Collections.Generic;
using UnityEngine;

namespace Depender
{
    public class Registar
    {
        private static List<BuildableObject> _customObjects = new List<BuildableObject>();
        public static List<Product> _customProducts = new List<Product>();

        public static GameObject _hider;

        public enum PathTypes { Normal, Queue, Employee }


        public static GameObject Register(ModdedObject ModdedObject)
        {
            HPDebug.Log("Registering: " + ModdedObject.Name + " = Type: " + ModdedObject.GetType());
            try
            {
                GameObject asset = ModdedObject.Object;

                if (asset == null)
                {
                    asset = new GameObject();
                }
                ModdedObject.Decorate();
                Object.DontDestroyOnLoad(asset);
                if (asset.GetComponent<BuildableObject>())
                {
                    BuildableObject buildableObject = asset.GetComponent<BuildableObject>();
                    buildableObject.dontSerialize = true;
                    buildableObject.isPreview = true;
                    _customObjects.Add(buildableObject);
                    AssetManager.Instance.registerObject(buildableObject);
                }
                asset.transform.SetParent(_hider.transform);
                _hider.SetActive(false);
                HPDebug.Log("Succelfully Registered: " + ModdedObject.Name);
                return asset;
            }

            catch (System.Exception e)
            {

                HPDebug.LogError(e.Message);
                return null;
            }
        }

        public enum PathType { Normal, Queue, Employee }
        public static void RegisterPath(Texture PathTexture, string identifier, PathType Type)
        {

            PathStyle c = AssetManager.Instance.pathStyles.getPathStyle("concrete");

            PathStyle ps = new PathStyle();

            ps.handRailGO = c.handRailGO;
            ps.handRailRampGO = c.handRailRampGO;
            Material Mat = GameObject.Instantiate(c.material);
            Mat.mainTexture = PathTexture;
            ps.material = Mat;
            ps.platformTileMapper = AssetManager.Instance.platformTileMapper;
            ps.identifier = identifier;
            ps.spawnSound = c.spawnSound;
            ps.despawnSoundEvent = c.despawnSoundEvent;
            ps.spawnLastSound = c.spawnLastSound;
            ps.spawnTilesOnPlatforms = true;
            switch (Type)
            {
                case PathType.Normal:
                    AssetManager.Instance.pathStyles.registerPathStyle(ps);
                    break;
                case PathType.Queue:
                    AssetManager.Instance.queueStyles.registerPathStyle(ps);
                    break;
                case PathType.Employee:
                    AssetManager.Instance.employeePathStyles.registerPathStyle(ps);
                    break;
                default:
                    break;
            }
        }
        public static void UnRegister()
        {
            foreach (var item in _customObjects)
            {
                AssetManager.Instance.unregisterObject(item);

            }
            _customObjects.Clear();
            foreach (var item in _customProducts)
            {
                AssetManager.Instance.unregisterObject(item);
            }
            _customProducts.Clear();
        }
    }
}

