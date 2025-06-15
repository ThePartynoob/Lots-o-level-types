using HarmonyLib;
using MTM101BaldAPI.Registers.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace Lots_o__level_types
{
    
    internal class ElectricalRoomFunction : RoomFunction
    {
        GameButtonBase PreButton;
        Cell[] PlacedButtons = [];
        Cell[] SafeButtonCells = [];
        public override void Initialize(RoomController room)
        {
            base.Initialize(room);
            PreButton = Resources.FindObjectsOfTypeAll<GameLever>().First(x => x.name == "GameLever" && x.gameObject.activeSelf);
        }

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

            foreach (var roomA in room.ec.rooms)
            {
                if (roomA == room) continue;
                foreach (Cell cell in SafeButtonCells)
                {
                    var controller = new GameObject("ControllerLight");
                    controller.transform.SetParent(roomA.transform, false);
                    var LCL = controller.AddComponent<LightControllerLever>();
                    LCL.Room = roomA;

                    var button = GameButton.BuildInArea(this.room.ec, cell.position, 2, controller, PreButton, new Random());
                    switch (roomA.category)
                    {
                        case RoomCategory.Class:
                            button.ChangeColor("Blue");
                            break;
                        case RoomCategory.Office:
                            button.ChangeColor("Yellow");
                            break;
                        case RoomCategory.Faculty:
                            button.ChangeColor("Orange");
                            break;
                        case RoomCategory.Special:
                            button.ChangeColor("White");
                            break;
                        default:
                            button.ChangeColor("Black");
                            break;
                            
                    }

                    button.Set(true);

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
