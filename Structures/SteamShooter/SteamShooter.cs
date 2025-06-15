using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Lots_o__level_types
{
    internal class SteamShooter : MonoBehaviour
    {
        [SerializeField]
        public Entity prefab;
        float interval = 10;
        float timeleft = 0;
        PropagatedAudioManager AudMan;
        void Start()
        {
            AudMan = gameObject.GetComponent<PropagatedAudioManager>();
            AudMan.QueueAudio(BasePlugin.AssetMan.Get<SoundObject>("Aud_VentLoop"));
            AudMan.SetLoop(true);
        }
        void Update()
        {
            timeleft += Time.deltaTime;
            if (timeleft > interval)
            {
                timeleft = 0;
                var _instance = Instantiate<Entity>(prefab, gameObject.transform);
                _instance.gameObject.transform.localPosition = Vector3.zero;
                _instance.gameObject.transform.rotation = gameObject.transform.rotation;
                AudMan.PlaySingle(BasePlugin.AssetMan.Get<SoundObject>("Aud_VentShoot"));
            }
        }
    }
}
