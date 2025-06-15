using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Lots_o__level_types
{
    internal class LaserFieldStructure : StructureBuilder
    {

        [SerializeField]
        public LaserFieldLogic Prefab;

        public override void PostOpenCalcGenerate(LevelGenerator lg, System.Random rng)
        {
            base.PostOpenCalcGenerate(lg, rng);
            List<List<Cell>> halls = lg.Ec.FindHallways();
            List<Cell> ForbiddenCells;
            var useless = true;
            lg.Ec.FindPath(lg.Ec.CellFromPosition(lg.Ec.spawnPoint), lg.Ec.CellFromPosition(lg.specialRooms[0].doors[0].position), PathType.Nav, out ForbiddenCells, out useless);
            for (int i = 0; i < halls.Count; i++)
            {
                halls[i].RemoveAll(x => Directions.OpenDirectionsFromBin(x.ConstBin).Count > 2);
                halls[i].RemoveAll(x => x.shape != TileShapeMask.Straight);
                halls[i].RemoveAll(x => !x.HasAllFreeWall);
                foreach (var CellComp in ForbiddenCells)
                {
                    halls[i].RemoveAll(x => x.position == CellComp.position);
                }
            }
            halls.RemoveAll(x => x.Count == 0);
            int LaserFieldCount = rng.Next(parameters.minMax[0].x, parameters.minMax[0].z);
            int retries = 0;
            for (int i = 0; i < LaserFieldCount; i++)
            {
                if(halls.Count == 0)
                {
                    Debug.LogWarning("Cant find hall for the laserfield #" + i + ", Skipping!");
                }
                int chosenHallIndex = rng.Next(0, halls.Count);
                Cell chosenCell = halls[chosenHallIndex][rng.Next(0, halls[chosenHallIndex].Count)];
                halls.RemoveAt(chosenHallIndex);
                List<Direction> potentialDirections = Directions.OpenDirectionsFromBin(chosenCell.ConstBin);
                Direction ChosenDirection = potentialDirections[rng.Next(0, potentialDirections.Count)];
                
                if (chosenCell.AllCoverageFitsInDirection(CellCoverage.East | CellCoverage.West, ChosenDirection))
                {
                    Place(chosenCell, ChosenDirection);
                } else
                {
                    retries++;
                    
                    if (retries >= 15)
                    {
                        Debug.LogWarning("Cant place laserfield #" + i + ", Skipping!");
                        continue;
                    }
                    i--;
                    Debug.LogWarning("Cant place laserfield #" + i + ", retrying!");
                }
            }
        }

        public void Place(Cell cellAt, Direction dir)
        {
            Debug.Log(Prefab);
            LaserFieldLogic LaserField = GameObject.Instantiate<LaserFieldLogic>(Prefab, cellAt.room.objectObject.transform);
            LaserField.transform.position = cellAt.FloorWorldPosition;
            LaserField.transform.rotation = dir.ToRotation();
            LaserField.CurrentCell = cellAt;
            LaserField.StructureDirection = dir;
            LaserField.IsEnabled = false || UnityEngine.Random.Range(0, 1) == 0;
            

        }





    }
}
