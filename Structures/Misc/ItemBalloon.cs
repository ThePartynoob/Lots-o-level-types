using MTM101BaldAPI.Registers;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.XR;

namespace Lots_o__level_types
{
    internal class ItemBalloon : MonoBehaviour, IClickable<int>
    {

        
        internal WeightedItemObject[] weightedItems = [new WeightedItemObject() {
                    selection = ItemMetaStorage.Instance.FindByEnum(Items.Quarter).value,
                    weight = 200
                },
                new WeightedItemObject() {
                    selection = ItemMetaStorage.Instance.FindByEnum(Items.ZestyBar).value,
                    weight = 140
                },
                new WeightedItemObject() {
                    selection = ItemMetaStorage.Instance.FindByEnum(Items.DietBsoda).value,
                    weight = 130
                },
                new WeightedItemObject() {
                    selection = ItemMetaStorage.Instance.FindByEnum(Items.Scissors).value,
                    weight = 130
                },
                new WeightedItemObject() {
                    selection = ItemMetaStorage.Instance.FindByEnum(Items.DetentionKey).value,
                    weight = 100
                },
                new WeightedItemObject() {
                    selection = ItemMetaStorage.Instance.FindByEnum(Items.Nametag).value,
                    weight = 100
                },
                new WeightedItemObject() {
                    selection = ItemMetaStorage.Instance.FindByEnum(Items.Wd40).value,
                    weight = 85
                },
                new WeightedItemObject() {
                    selection = ItemMetaStorage.Instance.FindByEnum(Items.Boots).value,
                    weight = 85
                },
                new WeightedItemObject() {
                    selection = ItemMetaStorage.Instance.FindByEnum(Items.DoorLock).value,
                    weight = 60
                },
                new WeightedItemObject() {
                    selection = ItemMetaStorage.Instance.FindByEnum(Items.Tape).value,
                    weight = 60
                },
                new WeightedItemObject() {
                    selection = ItemMetaStorage.Instance.FindByEnum(Items.Apple).value,
                    weight = 30
                },
                new WeightedItemObject() {
                    selection = ItemMetaStorage.Instance.FindByEnum(Items.Teleporter).value,
                    weight = 10
                },
                new WeightedItemObject() {
                    selection = ItemMetaStorage.Instance.FindByEnum(Items.GrapplingHook).value,
                    weight = 40
                }];
        void Start()
        {
            gameObject.GetComponent<Transform>().Find("RendererBase").Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite
                = BasePlugin.AssetMan.Get<Sprite>("spr_partybash_itemballon");
            
        }

        void Update()
        {
            gameObject.layer = LayerMask.NameToLayer("ClickableEntities");
        }
        public bool ClickableHidden()
        {
            return false;
        }

        public bool ClickableRequiresNormalHeight()
        {
            return true;
        }

        public void ClickableSighted(int player)
        {
            
        }

        public void ClickableUnsighted(int player)
        {
            
        }

        public void Clicked(int player)
        {
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(BasePlugin.AssetMan.Get<SoundObject>("Aud_Pop"));
            Singleton<BaseGameManager>.Instance.Ec.MakeNoise(Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position, 10);
            Singleton<CoreGameManager>.Instance.AddPoints(10, 0, true);
            var selected = WeightedItemObject.ControlledRandomSelection(weightedItems, new System.Random());
            Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.AddItem(selected);
            Destroy(gameObject);
        }
    }
}
