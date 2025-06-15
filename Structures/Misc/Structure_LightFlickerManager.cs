using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Lots_o__level_types
{
    internal class Structure_LightFlickerManager : StructureBuilder
    {
        public override void OnGenerationFinished(LevelBuilder lb)
        {
            base.OnGenerationFinished(lb);
            var flickerManager = new GameObject("LightFlickerManager");
            flickerManager.transform.SetParent(ec.transform);
            flickerManager.AddComponent<LightFlickerManager>().Ec = ec;
        }
    }

    internal class LightFlickerManager : MonoBehaviour
    {
        [SerializeField]
        public EnvironmentController Ec;

        float updateevery = 7f;
        float timeleft = 7f;
        void Update()
        {
            timeleft -= Time.deltaTime;
            if (timeleft < 0 )
            {
                timeleft = updateevery;
                foreach (var light in Ec.lights)
                {
                    light.SetLight(UnityEngine.Random.Range(0, 2) == 0);
                }
            }
        }
    }
}
