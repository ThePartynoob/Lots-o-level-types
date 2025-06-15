using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace Lots_o__level_types
{
    internal class Structure_SteamShooter : StructureBuilder
    {
        [SerializeField]
        public SteamShooter prefab;
        public override void PostOpenCalcGenerate(LevelGenerator lg, Random rng)
        {
            base.PostOpenCalcGenerate(lg, rng);
            List<List<Cell>> halls = lg.Ec.FindHallways();
          
            for (int i = 0; i < halls.Count; i++)
            {
                
                halls[i].RemoveAll(x => x.shape != TileShapeMask.Single && x.shape != TileShapeMask.Corner);
             
            }
            halls.RemoveAll(x => x.Count == 0);
            int LaserFieldCount = rng.Next(parameters.minMax[0].x, parameters.minMax[0].z);
            int retries = 0;
            for (int i = 0; i < LaserFieldCount; i++)
            {
                if (halls.Count == 0)
                {
                    Debug.LogWarning("Cant find hall for the SteamShooter #" + i + ", Skipping!");
                    continue;
                }
                int chosenHallIndex = rng.Next(0, halls.Count);
                Cell chosenCell = halls[chosenHallIndex][rng.Next(0, halls[chosenHallIndex].Count)];
                halls.RemoveAt(chosenHallIndex);
               
                List<Direction> potentialDirections = Directions.ClosedDirectionsFromBin(chosenCell.ConstBin);
                Direction ChosenDirection = potentialDirections[rng.Next(0, potentialDirections.Count)];
                Place(chosenCell, ChosenDirection);
                
            }
        }

        void Place(Cell cellAt, Direction dir)
        {
            var _SteamShooterInstance = Instantiate<SteamShooter>(prefab);
            _SteamShooterInstance.transform.position = cellAt.FloorWorldPosition;
            _SteamShooterInstance.transform.rotation = dir.GetOpposite().ToRotation();
            _SteamShooterInstance.transform.position += new Vector3(0, 5, 0);
            _SteamShooterInstance.transform.position += -_SteamShooterInstance.transform.forward * 4.8f;
        } 
    }
}
