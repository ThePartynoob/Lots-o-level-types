using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace Lots_o__level_types
{
    internal class ButtonroomFunction : RoomFunction
    {
        GameButtonBase PreButton;

        Cell[] PlacedButtons = [];
        Cell[] SafeButtonCells = [];
        public override void AfterAllRoomsPlaced(LevelBuilder builder, Random rng)
        {
            
           
        }
        
        public override void Initialize(RoomController room)
        {
            PreButton = Resources.FindObjectsOfTypeAll<GameLever>().First(x => x.name == "GameLever" && x.gameObject.activeSelf);
            base.Initialize(room);
            
        }
        Direction SelectedDir;
        public override void OnGenerationFinished()
        {
            base.OnGenerationFinished();
            Debug.Log("Should work");
            foreach (Cell cell in room.cells)
            {
                bool safe = false;
                foreach (var dir in cell.AllWallDirections)
                {
                    if (cell.HasWallInDirection(dir)) safe = true;
                }
                if (cell.AllWallDirections.Count > 0 &&
                    cell.doorDirs.Count == 0 &&
                    safe)
                {

                    
                    SafeButtonCells = SafeButtonCells.AddToArray(cell);
                    Debug.Log("Safe button pos: " + cell.position.ToString());
                }
                else
                {
                    Debug.Log("Button cant be placed at " + cell.position.ToString());
                }
            }
            MonoBehaviour[] Structures = [];
            foreach (MonoBehaviour mb in FindObjectsOfType<MonoBehaviour>())
            {
                if (mb is LaserFieldLogic)
                {
                    Structures = Structures.AddToArray((MonoBehaviour)mb);
                    Debug.Log("found structure: " + mb.gameObject.name);
                    
                }
            }
            foreach (var Structure in Structures)
            {
                foreach (Cell cell in SafeButtonCells)
                {
                    
                    
                    var button = GameButton.BuildInArea(this.room.ec, cell.position, 2, Structure.gameObject, PreButton, new Random());
                    button.SetPowered(((LaserFieldLogic)Structure).IsEnabled);
                    
                    var e = SafeButtonCells.ToList();
                    e.Remove(cell);
                    SafeButtonCells = e.ToArray();
                        Debug.Log("Placed Button at " + cell.position.ToString());
                    break;
                }
            }
        }
    }
}
