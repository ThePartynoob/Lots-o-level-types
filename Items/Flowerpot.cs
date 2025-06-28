using System.Collections;
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
        GameObject Pot;
        public override bool Use(PlayerManager pm)
        {
            Pot = Instantiate(BasePlugin.AssetMan.Get<GameObject>("pottedPlant"));
            Pot.transform.position = pm.gameObject.transform.position;
            Pot.AddComponent<FallObject>();
            return true;

        }


    }
    class FallObject : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(Fall());
        }

        IEnumerator Fall()
        {
            var y = 5f;
            var velocity = 1.5f;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);
            while (y > 1)
            {
                yield return new WaitForEndOfFrame();
                y += velocity * Time.deltaTime;
                velocity -= 0.75f * Time.deltaTime;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);

            }
            var sproutC = Instantiate(BasePlugin.AssetMan.Get<GameObject>("sprout"), gameObject.transform.position, gameObject.transform.rotation);
            sproutC.transform.position = new Vector3(sproutC.transform.position.x, 1.8f, sproutC.transform.position.z);
            sproutC.tag = "LOLT_Sprout";

            Destroy(gameObject);
        }
    }

    class ITM_WateringCan : Item
    {
        public override bool Use(PlayerManager pm)
        {
            if (Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out var hit, pm.pc.reach, pm.pc.ClickLayers))
            {
                if (hit.collider.gameObject.GetComponent<CustomTag>().customTag == "LOLT_Sprout")
                {
                    Singleton<CoreGameManager>.Instance.audMan.PlaySingle(BasePlugin.AssetMan.Get<SoundObject>("snd_WateringCan"));
                    Destroy(hit.collider.gameObject);
                    return true;
                }
                

            }
            Object.Destroy(base.gameObject);
                return false;
            
        }
    }

}