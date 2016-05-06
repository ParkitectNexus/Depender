using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace Depender.Types.Shops
{
    [Serializable]
    public class ShopMod : ModdedObject
    {
        [SerializeField]
        public List<ProductMod> products = new List<ProductMod>();
        public Vector2 scrollPos2;
        public Product selected;

        public override void Decorate()
        {
            CustomShop shop = Object.AddComponent<CustomShop>();
            List<GameObject> productsGO = new List<GameObject>();
            foreach (ProductMod P in products)
            {
                Debug.Log(P.GetType().Name);
                productsGO.Add(P.Decorate());
            }
            shop.productGOs = productsGO.ToArray();
            base.Decorate();

        }

    }
    [Serializable]
    public class ProductMod
    {
        [SerializeField]
        public string Name = "New Product";
        [SerializeField]
        public GameObject GO = new GameObject();
        [SerializeField]
        public int price = 10;
        public enum hand { Left, Right }
        [SerializeField]
        public hand Hand = hand.Left;
        [SerializeField]
        public List<ingredient> ingredients = new List<ingredient>();
        ingredient selected;
        private Vector2 scrollPos;
        public virtual GameObject Decorate()
        {

            Product P = GO.GetComponent<Product>();

            P.defaultPrice = price;


            switch (Hand)
            {
                case hand.Left:
                    P.handSide = global::Hand.Side.LEFT;
                    break;
                case hand.Right:
                    P.handSide = global::Hand.Side.RIGHT;
                    break;
            }

            List<Ingredient> ingredientsMod = new List<Ingredient>();
            foreach (ingredient I in ingredients)
            {
                var resource = ScriptableObject.CreateInstance<Resource>();
                resource.name = I.Name;
                resource.setDisplayName(I.Name);
                resource.costs = I.price;
                resource.getResourceSettings().percentage = 1f;
                List<ConsumableEffect> consumableEffects = new List<ConsumableEffect>();
                foreach (effect E in I.effects)
                {
                    var consumableEffect = new ConsumableEffect();
                    switch (E.Type)
                    {
                        case effect.Types.hunger:
                            consumableEffect.affectedStat = ConsumableEffect.AffectedStat.HUNGER;
                            break;
                        case effect.Types.thirst:
                            consumableEffect.affectedStat = ConsumableEffect.AffectedStat.THIRST;
                            break;
                        case effect.Types.happiness:
                            consumableEffect.affectedStat = ConsumableEffect.AffectedStat.HAPPINESS;
                            break;
                        case effect.Types.tiredness:
                            consumableEffect.affectedStat = ConsumableEffect.AffectedStat.TIREDNESS;
                            break;
                        case effect.Types.sugarboost:
                            consumableEffect.affectedStat = ConsumableEffect.AffectedStat.SUGARBOOST;
                            break;
                    }
                    consumableEffect.amount = E.amount;
                    consumableEffects.Add(consumableEffect);
                }
                resource.effects = consumableEffects.ToArray();
                Ingredient ingredient = new Ingredient();
                ingredient.defaultAmount = I.amount;
                ingredient.tweakable = I.tweakable;
                AssetManager.Instance.registerObject(resource);
                ingredient.resource = resource;
                ingredientsMod.Add(ingredient);
            }
            P.ingredients = ingredientsMod.ToArray();

            return GO;
        }


    }

    [Serializable]
    public class wearable : ProductMod
    {

        public enum bodylocation { head, face, back }
        [SerializeField]
        public bodylocation BodyLocation = bodylocation.head;
        public override GameObject Decorate()
        {
            WearableProduct WP = GO.AddComponent<WerableProductInstance>();

            BindingFlags flags = BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic;
            typeof(Product).GetField("displayName", flags).SetValue(WP, Name);

            AssetManager.Instance.registerObject(WP);
            switch (BodyLocation)
            {
                case bodylocation.head:
                    WP.bodyLocation = WearableProduct.BodyLocation.HEAD;
                    break;
                case bodylocation.face:
                    WP.bodyLocation = WearableProduct.BodyLocation.FACE;
                    break;
                case bodylocation.back:
                    WP.bodyLocation = WearableProduct.BodyLocation.BACK;
                    break;
                default:
                    break;
            }
            return base.Decorate();
        }

    }
    [Serializable]
    public class consumable : ProductMod
    {
        public enum consumeanimation { generic, drink_straw, lick, with_hands }
        [SerializeField]
        public consumeanimation ConsumeAnimation;
        public enum temprature { none, cold, hot }
        [SerializeField]
        public temprature Temprature;
        [SerializeField]
        public int portions;
        public override GameObject Decorate()
        {
            ConsumableProduct CP = GO.AddComponent<ConsumableProductInstance>();

            BindingFlags flags = BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic;
            typeof(Product).GetField("displayName", flags).SetValue(CP, Name);

            AssetManager.Instance.registerObject(CP);
            switch (ConsumeAnimation)
            {
                case consumeanimation.generic:
                    CP.consumeAnimation = ConsumableProduct.ConsumeAnimation.GENERIC;
                    break;
                case consumeanimation.drink_straw:
                    CP.consumeAnimation = ConsumableProduct.ConsumeAnimation.DRINK_STRAW;
                    break;
                case consumeanimation.lick:
                    CP.consumeAnimation = ConsumableProduct.ConsumeAnimation.LICK;
                    break;
                case consumeanimation.with_hands:
                    CP.consumeAnimation = ConsumableProduct.ConsumeAnimation.WITH_HANDS;
                    break;
                default:
                    break;
            }
            switch (Temprature)
            {
                case temprature.none:
                    CP.temperaturePreference = ConsumableProduct.TemperaturePreference.NONE;
                    break;
                case temprature.cold:
                    CP.temperaturePreference = ConsumableProduct.TemperaturePreference.COLD;
                    break;
                case temprature.hot:
                    CP.temperaturePreference = ConsumableProduct.TemperaturePreference.HOT;
                    break;
                default:
                    break;
            }
            CP.portions = portions;
            return base.Decorate();
        }
    }
    [Serializable]
    public class ongoing : ProductMod
    {
        [SerializeField]
        public int duration;
        public override GameObject Decorate()
        {
            OngoingEffectProductInstance WP = GO.AddComponent<OngoingEffectProductInstance>();

            BindingFlags flags = BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic;
            typeof(Product).GetField("displayName", flags).SetValue(WP, Name);

            AssetManager.Instance.registerObject(WP);
            WP.duration = duration;
            return base.Decorate();
        }
    }
    [Serializable]
    public class effect
    {
        [SerializeField]
        public enum Types { hunger, thirst, happiness, tiredness, sugarboost }
        [SerializeField]
        public Types Type = Types.hunger;
        [SerializeField]
        public float amount;
    }
    [Serializable]
    public class ingredient
    {

        [SerializeField]
        public string Name = "New Ingredient";
        [SerializeField]
        public int price = 1;
        [SerializeField]
        public float amount = 1;
        [SerializeField]
        public bool tweakable = true;
        [SerializeField]
        public List<effect> effects = new List<effect>();
    }
}