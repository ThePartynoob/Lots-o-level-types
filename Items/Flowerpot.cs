using UnityEngine;


namespace Lots_o__level_types
{
    class FlowerPot : MonoBehaviour, IClickable<int>
    {
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
            Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.AddItem(BasePlugin.AssetMan.Get<ItemObject>("itm_flowerpot"));
            Destroy(gameObject);
        }
    }

    class ITM_FlowerPot : Item
    {
        public override bool Use(PlayerManager pm)
        {

            return base.Use(pm);
        }
    }

}