using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Lots_o__level_types
{
    internal class Structure_BallonSpawner : StructureBuilder
    {
        int balloonsToSpawnHall = 75;
        int balloonsToSpawnRoom = 10;
        Balloon[] BalloonsSpawn;
        
        public override void Generate(LevelGenerator lg, System.Random rng)
        {
            base.Generate(lg, rng);
            
        }
        public override void OnGenerationFinished(LevelBuilder lb)
        {
            base.OnGenerationFinished(lb);
        
        
            
            var dis = Resources.FindObjectsOfTypeAll<Balloon>();
            BalloonsSpawn = [
                dis.First(x => x.name == "Balloon_Orange"),
                dis.First(x => x.name == "Balloon_Green"),
                dis.First(x => x.name == "Balloon_Blue"),
                dis.First(x => x.name == "Balloon_Purple")
                ];




            List<Cell> SafeCells = ec.mainHall.AllTilesNoGarbage(false,false);

            for (int i = 0; i < balloonsToSpawnHall; i++)
            {
                if (SafeCells.Count == 0)
                {
                    break;
                }
                var RandomSelectedBalloon = BalloonsSpawn[UnityEngine.Random.Range(0, BalloonsSpawn.Length)];

                var clone = UnityEngine.Object.Instantiate(RandomSelectedBalloon);
                if (i % 10 == 1)
                {

                    clone.gameObject.AddComponent<ItemBalloon>();
                    
                }
                clone.Initialize(ec.mainHall);
                Cell cell = SafeCells[UnityEngine.Random.Range(0, SafeCells.Count)];
                SafeCells.Remove(cell);

                clone.Entity.Teleport(cell.FloorWorldPosition);
                
            }

            foreach (var room in ec.rooms)
            {
                for (int i = 0; i < balloonsToSpawnRoom; i++)
                {

                    var RandomSelectedBalloon = BalloonsSpawn[UnityEngine.Random.Range(0, BalloonsSpawn.Length)];

                    var clone = UnityEngine.Object.Instantiate(RandomSelectedBalloon);
                    if (i % 10 == 1)
                    {

                        clone.gameObject.AddComponent<ItemBalloon>();

                    }

                    clone.Initialize(room);

                }
            }


        }

    }

}
