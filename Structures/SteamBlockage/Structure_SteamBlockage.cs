using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static PineDebug.PineDebugManager;
using Random = System.Random;

namespace Lots_o__level_types
{
    internal class Structure_SteamBlockage : StructureBuilder
    {
        [SerializeField]
        public SteamBlockage prefab;

        public List<RoomController> PossibleRooms;

        public GameButtonBase ButtonPre;


        public override void Initialize(EnvironmentController ec, StructureParameters parameters)
        {
            base.Initialize(ec, parameters);
            ButtonPre = Resources.FindObjectsOfTypeAll<GameValve>().First(x => x.name == "GameValve" && x.gameObject.activeSelf);
        }

        public override void PostOpenCalcGenerate(LevelGenerator lg, Random rng)
        { 
            base.PostOpenCalcGenerate(lg, rng);
            PossibleRooms = new List<RoomController>();
            PossibleRooms.AddRange(lg.Ec.rooms.Where(x => x.category == RoomCategory.Class || x.category == RoomCategory.Faculty));

            for (int i = 0; i < rng.Next(parameters.minMax[0].x, parameters.minMax[0].z); i++)
            {
                RoomController RoomBlocker = PossibleRooms[rng.Next(0, PossibleRooms.Count)];
                PossibleRooms.Remove(RoomBlocker);


                foreach (var DoorBlocker in RoomBlocker.doors)
                {
                    PlaceBlocker(prefab, DoorBlocker.position, DoorBlocker.direction, 5,-5, DoorBlocker);
                    
                }


            }
        }



        void PlaceBlocker(SteamBlockage Prefab, IntVector2 pos, Direction direction, float offsetforward, float offsetside, Door door)
        {

            if (Prefab == null)
            {
                Debug.LogError("SteamBlockage prefab is null.");
                return;
            }

            var cellBlocker = door.bTile;
            if (cellBlocker == null || cellBlocker.ObjectBase == null)
            {
                Debug.LogWarning($"[SteamBlockage] Skipped: No valid cell at {pos} or ObjectBase is null.");
                return;
            }

            SteamBlockage NewSteamBlock = Instantiate<SteamBlockage>(Prefab,cellBlocker.ObjectBase);
            NewSteamBlock.gameObject.transform.position = door.gameObject.transform.position;
            NewSteamBlock.gameObject.transform.rotation = direction.ToRotation();
            NewSteamBlock.gameObject.transform.position += NewSteamBlock.gameObject.transform.forward * offsetforward;
            NewSteamBlock.gameObject.transform.position += NewSteamBlock.gameObject.transform.right * offsetside;
            NewSteamBlock.door = door;
            GameButton.BuildInArea(ec, cellBlocker.position, 16, NewSteamBlock.gameObject, ButtonPre, new Random());
        }


    }
}
