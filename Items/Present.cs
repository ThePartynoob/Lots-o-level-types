using MTM101BaldAPI.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Lots_o__level_types
{
    internal class Present : Item
    {
        [SerializeField]
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
        public override bool Use(PlayerManager pm)
        {
            var selected = WeightedItemObject.ControlledRandomSelection(weightedItems, new System.Random());
            pm.itm.AddItem(selected);
            pm.itm.UpdateItems();
            Destroy(gameObject);
            return false;


        }
    }

    internal class YtpPresent : Item
    {
        [SerializeField]
        internal WeightedInt[] WeightedAmounts = [new WeightedInt() {
            selection = 25,
            weight = 100
        },
        new WeightedInt() {
            selection = 50,
            weight = 50
        },
        new WeightedInt() {
            selection = 100,
            weight = 10
        }];
        public override bool Use(PlayerManager pm)
        {
            var selected = WeightedInt.ControlledRandomSelection(WeightedAmounts, new System.Random());
            var ytpsound1 = Resources.FindObjectsOfTypeAll<SoundObject>().First(x => x.name == "YTPPickup_0");
            var ytpsound2 = Resources.FindObjectsOfTypeAll<SoundObject>().First(x => x.name == "YTPPickup_1");
            var ytpsound3 = Resources.FindObjectsOfTypeAll<SoundObject>().First(x => x.name == "YTPPickup_2");
            Singleton<CoreGameManager>.Instance.AddPoints(selected, 0, true);
            switch (selected)
            {
                case 25:
                    Singleton<CoreGameManager>.Instance.audMan.PlaySingle(ytpsound1);
                    break;
                case 50:
                    Singleton<CoreGameManager>.Instance.audMan.PlaySingle(ytpsound2);
                    break;
                case 100:
                    Singleton<CoreGameManager>.Instance.audMan.PlaySingle(ytpsound3);
                    break;
            }
            pm.itm.UpdateItems();
            Destroy(gameObject);
            return false;


        }
    }
}
