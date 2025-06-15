using MTM101BaldAPI.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Lots_o__level_types
{
   
    internal class SteamSteam : MonoBehaviour, IEntityTrigger
    {
        private EnvironmentController ec;

        [SerializeField]
        private Entity entity;

        [SerializeField]
        private MovementModifier moveMod;

        [SerializeField]
        private float speed = 15;

        [SerializeField]
        private float lifetime = 8.5f;

        private bool launching = true;

        private List<ActivityModifier> activityMods = new List<ActivityModifier>();

        void Start()
        {
            ec = Singleton<BaseGameManager>.Instance.Ec;
            //gameObject.AddComponent<BoxCollider>().size = new Vector3(10, 10, 0.5f);
            //gameObject.GetComponent<BoxCollider>().isTrigger = true;
            // Ensure Entity component exists
            if (entity == null)
            {
                entity = gameObject.GetComponent<Entity>();
                if (entity == null)
                {
                    
                }
            }

            // Initialize the Entity
            
            entity.Initialize(ec, transform.position);

            entity.gameObject.transform.Find("RenderBase").Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            entity.gameObject.transform.Find("RenderBase").Find("Sprite").gameObject.transform.localScale /= 1.85f;
            moveMod.priority = 1;
        }

        void Update()
        {
            if (ec == null || entity == null) return;
            if (lifetime < 7.5f)
            {
                Vector3 movement = transform.forward * speed * ec.EnvironmentTimeScale;
                moveMod.movementAddend = entity.ExternalActivity.Addend + movement;
                entity.UpdateInternalMovement(movement);
            } else
            {
                entity.gameObject.transform.Find("RenderBase").Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(entity.gameObject.transform.Find("RenderBase").Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color, Color.red, 0.05f);
            }
            lifetime -= Time.deltaTime * ec.EnvironmentTimeScale;
            if (lifetime < 1.25f)
            {
                entity.gameObject.transform.Find("RenderBase").Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(entity.gameObject.transform.Find("RenderBase").Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color, Color.clear, 0.025f);
            }
            if (lifetime <= 0f)
            {
                foreach (ActivityModifier activityMod in activityMods)
                {
                    activityMod.moveMods.Remove(moveMod);
                }

                Destroy(gameObject);
            }
        }

        public void EntityTriggerEnter(Collider other)
        {
            Entity otherEntity = other.GetComponent<Entity>();
            if (otherEntity != null)
            {
                otherEntity.ExternalActivity.moveMods.Add(moveMod);
                activityMods.Add(otherEntity.ExternalActivity);
            }
        }

        public void EntityTriggerStay(Collider other)
        {
            // Optional: add logic if needed
        }

        public void EntityTriggerExit(Collider other)
        {
            Entity otherEntity = other.GetComponent<Entity>();


            if (otherEntity != null)
            {
                otherEntity.ExternalActivity.moveMods.Remove(moveMod);
                activityMods.Remove(otherEntity.ExternalActivity);
            }
        }
    }
}
