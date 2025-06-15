using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace Lots_o__level_types
{
    internal class SteamBlockage : MonoBehaviour, IButtonReceiver
    {
        [SerializeField]
        public GameObject prefab;
        [SerializeField]
        public Door door;

        bool IsEnabled = true;

        Vector3 SizeParticlesSpawn;
        float TimeBeforeNextParticleSpawn = 0;
        int particles = 0;
        int particlesLimit = 25;
        [SerializeField]
        public PropagatedAudioManager AudMan;
        void Start()
        {
            SizeParticlesSpawn = new Vector3(10, 10, 0.1f);
            
            
            
            
        }

        void Update()
        {
            if (IsEnabled)
            {
                door.Lock(true);
                door.Block(true);
                door.Shut();
                TimeBeforeNextParticleSpawn -= Time.deltaTime;
                if (TimeBeforeNextParticleSpawn < 0 && particles < particlesLimit)
                {
                    TimeBeforeNextParticleSpawn = UnityEngine.Random.Range(0.2f, 0.5f);
                    StartCoroutine(summonParticle());
                }
            }
            gameObject.GetComponent<BoxCollider>().enabled = IsEnabled;
        }

        IEnumerator summonParticle()
        {
            var timeleft = 12f;
            particles++;
            var particle = Instantiate(prefab);
            particle.transform.SetParent(gameObject.transform);
            var rng = new Random();
            particle.transform.localPosition = new Vector3(rng.Next(0,11), rng.Next(0, 11), rng.Next(-2,2));
            var dir = new Vector3(rng.Next(0, 11), rng.Next(0, 11), 0).normalized;
            while (timeleft > 0)
            {
                yield return null;
                timeleft -= Time.deltaTime;
                particle.transform.localPosition += (dir * 0.05f) * Time.deltaTime;
            }
            Destroy(particle);
            particles--;
        }

        
        public void ButtonPressed(bool val)
        {
            IsEnabled = val;

            door.Unlock();
            door.Block(false);
            if (val)
            {
                AudMan.QueueAudio(BasePlugin.AssetMan.Get<SoundObject>("Aud_SteamLeak"));
                AudMan.SetLoop(true);
            } else
            {
                AudMan.FlushQueue(true);
            }
        }

        public void ConnectButton(GameButtonBase button)
        {
            button.Set(true);
        }
    }
}
