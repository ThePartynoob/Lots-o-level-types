using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.ObjectCreation;
using MTM101BaldAPI.Registers;
using UnityEngine;
using EditorCustomRooms;
using System.IO;
using MTM101BaldAPI.Reflection;
using BepInEx.Bootstrap;
namespace Lots_o__level_types
{
    [BepInDependency("mtm101.rulerp.baldiplus.leveltyped",BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin("Partynoob.lotsoleveltypes", "lots o' level types", "1.0.0.0")]
    public class BasePlugin : BaseUnityPlugin
    {
        public static string GUID;

        LaserFieldLogic LaserField;

        public static AssetManager AssetMan;
        /// <summary>
        /// This is the party bash style suggested by @0salt_grain0
        /// </summary>
        public static LevelType PartyBashType;

        /// <summary>
        /// This is the techy style suggested by @bsideskid
        /// </summary>
        public static LevelType TechyType;

        /// <summary>
        /// This is the shaft style suggested by @bsideskid
        /// </summary>
        public static LevelType ShaftType;

        public static LevelType GreenhouseType;

        private RoomAsset[] ButtonsroomLayout;

        private RoomAsset[] ElectricalRoomLayout;
        

        private ConfigEntry<bool>[] FloorTypeEnabled;

        public static string modPath;


        public static BasePlugin instance;

        IEnumerator PreLoad()
        {
            yield return 5;
            yield return "Loading party bash assets";
            PartyBashType = EnumExtensions.ExtendEnum<LevelType>("PartyBash");
            
            AssetMan.Add<Sprite>("spr_partybash_wall", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "partybash_hallwaywall.png"));
            AssetMan.Add<Sprite>("spr_partybash_floor", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "partybash_hallwayfloor.png"));
            AssetMan.Add<Sprite>("spr_partybash_ceiling", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "partybash_ceiling.png"));
            AssetMan.Add<Sprite>("spr_partybash_surpriseytp", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 50, "partybash_surpriseytp.png"));
            AssetMan.Add<Sprite>("spr_partybash_itemballon", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 30, "partybash_itemballon.png"));
            AssetMan.Add<Sprite>("spr_partybash_present", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 50, "Present.png"));
            AssetMan.Add("Aud_Pop", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, "Gen_Pop.wav"), "sfx_balloonpop", SoundType.Effect, Color.white, 0.4f));

            GameObject BalloonsSpawnerObject = new GameObject("BalloonSpawner");
            BalloonsSpawnerObject.ConvertToPrefab(true);
            Structure_BallonSpawner Balloon_spawner = BalloonsSpawnerObject.AddComponent<Structure_BallonSpawner>();
            AssetMan.Add<Structure_BallonSpawner>("BalloonSpawner", Balloon_spawner);

            AssetMan.Add<ItemObject>("itm_present",new ItemBuilder(this.Info)
                .SetAsInstantUse()
                .SetGeneratorCost(20)
                .SetItemComponent<Present>()
                .SetNameAndDescription("ITM_Present", "a present, can contain any item")
                .SetEnum("PresentItem")
                .SetSprites(AssetMan.Get<Sprite>("spr_partybash_present"), AssetMan.Get<Sprite>("spr_partybash_present"))
                .Build());

            AssetMan.Add<ItemObject>("itm_ytppresent", new ItemBuilder(this.Info)
                .SetAsInstantUse()
                .SetGeneratorCost(45)
                .SetItemComponent<YtpPresent>()
                .SetNameAndDescription("ITM_YtpPresent", "a present, can contain any ytp")
                .SetEnum("PresentItem")
                .SetSprites(AssetMan.Get<Sprite>("spr_partybash_surpriseytp"), AssetMan.Get<Sprite>("spr_partybash_surpriseytp"))
                .Build());
            yield return "Loading techy assets";
            TechyType = EnumExtensions.ExtendEnum<LevelType>("Techy");
            AssetMan.Add<Sprite>("spr_techy_floor", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "techy_floor.png"));
            AssetMan.Add<Sprite>("spr_techy_ceiling", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "techy_ceiling.png"));
            AssetMan.Add<Sprite>("spr_techy_wall", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "techy_wall.png"));
            AssetMan.Add<Sprite>("spr_elc_wall", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "electricalroomwall.png"));
            AssetMan.Add<Sprite>("spr_elc_ceil", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "electricalroomroofpng.png"));
            AssetMan.Add<Sprite>("spr_elc_doorA", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "ELETCRICALROOMCLOSDE.png"));
            AssetMan.Add<Sprite>("spr_elc_doorB", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "ELETCRICALROOMOPEN.png"));
            AssetMan.Add<Sprite>("spr_elc_floor", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "vent.png"));
            AssetMan.Add<Sprite>("spr_Belt", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "Belt.png"));
            AssetMan.Add<Sprite>("spr_redtex", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "redtex.png"));
            AssetMan.Add<Sprite>("Icon_lsrOn", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 14, "IconLaser_On.png"));
            
            AssetMan.Add<Sprite>("Icon_lsrOff", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 14, "IconLaser_Off.png"));
            AssetMan.Add<Sprite>("Map_Button", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 14, "MapBG_Buttons.png"));
            AssetMan.Add<Sprite>("Map_Electrical", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 14, "MapBG_Electrical.png"));

            AssetMan.Add<StandardDoorMats>("door_elec", ObjectCreators.CreateDoorDataObject("Electrical", AssetMan.Get<Sprite>("spr_elc_doorB").texture, AssetMan.Get<Sprite>("spr_elc_doorA").texture));


            AssetMan.Add("Aud_Laser", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, "laser.wav"), "sfx_lasertouch", SoundType.Effect, Color.white, 0.4f));
            AssetMan.Add("Aud_LaserOn", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, "Laser_Activate.wav"), "sfx_laseractivate", SoundType.Effect, Color.white, 0.4f));
            AssetMan.Add("Aud_LaserOff", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, "Laser_Deactivate.wav"), "sfx_laserdeactivate", SoundType.Effect, Color.white, 0.4f));
            ButtonsroomLayout = RoomFactory.CreateAssetsFromPath(Path.Combine(modPath, "ButtonroomLayouts.cbld"), 0, false, null, false, false, null, false, true).ToArray();
            
            foreach (var room in ButtonsroomLayout)
            {
                room.ceilTex = AssetMan.Get<Sprite>("spr_techy_ceiling").texture;
                room.florTex = AssetMan.Get<Sprite>("spr_techy_floor").texture;
                room.wallTex = AssetMan.Get<Sprite>("spr_techy_wall").texture;
                room.color = Color.blue;
                
                room.type = RoomType.Room;
                room.category = RoomCategory.Special;
                room.lightPre = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "HangingLight").transform;
                room.mapMaterial = ObjectCreators.CreateMapTileShader(AssetMan.Get<Sprite>("Map_Button").texture);
            }
            ButtonsroomLayout.AddFunctionToAllRoomAssets<SpecialRoomSwingingDoorsBuilder>();
            ButtonsroomLayout.AddFunctionToAllRoomAssets<ButtonroomFunction>();
            ElectricalRoomLayout = RoomFactory.CreateAssetsFromPath(Path.Combine(modPath, "ElectricalRoom.cbld"), 0, false, null, false, false, null, false, false).ToArray();
            foreach (var room in ElectricalRoomLayout)
            {
                room.ceilTex = AssetMan.Get<Sprite>("spr_elc_ceil").texture;
                room.florTex = AssetMan.Get<Sprite>("spr_elc_floor").texture;
                room.wallTex = AssetMan.Get<Sprite>("spr_elc_wall").texture;
                
                room.standardLightCells.RemoveAll(x => x.x % 5 == 0 && x.z % 5 == 0);
                room.color = Color.magenta;
                room.type = RoomType.Room;
                room.category = RoomCategory.Null;
                room.mapMaterial = ObjectCreators.CreateMapTileShader(AssetMan.Get<Sprite>("Map_Electrical").texture);
                room.doorMats = AssetMan.Get<StandardDoorMats>("door_elec");
            }
            ElectricalRoomLayout.AddFunctionToAllRoomAssets<ElectricalRoomFunction>();


            Material BaseMaterial = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "TileBase" && x.GetInstanceID() >= 0);
            Material TopLaserMaterial = new Material(BaseMaterial)
            {
                name = "TopLaser",
                mainTexture = AssetMan.Get<Sprite>("spr_Belt").texture
            };
            Material LaserMaterial = new Material(BaseMaterial)
            {
                name = "Laser",
                mainTexture = AssetMan.Get<Sprite>("spr_redtex").texture
            };


            var laserModel1 = AssetLoader.ModelFromMod(this, "lazer-1.obj");
            var laserModel2 = AssetLoader.ModelFromMod(this, "lazeractual-1.obj");
            GameObject LaserFieldPrefab = new GameObject("Prefab_LaserField");
            laserModel1.transform.SetParent(LaserFieldPrefab.transform,false);
            laserModel2.transform.SetParent(LaserFieldPrefab.transform,false);
            for (int i = 0; i < laserModel1.transform.childCount; i++)
            {

                    laserModel1.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = TopLaserMaterial;
                
            }
            for (int i = 0; i < laserModel2.transform.childCount; i++)
            {
                laserModel2.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = LaserMaterial;
            }
            LaserFieldPrefab.ConvertToPrefab(true);
            LaserField = LaserFieldPrefab.AddComponent<LaserFieldLogic>();
            var collider = LaserFieldPrefab.AddComponent<BoxCollider>();
            collider.size = new Vector3(10, 10, 10f);
            collider.isTrigger = true;
            LaserField.AudMan = LaserFieldPrefab.AddComponent<PropagatedAudioManager>();
            var objLFS = new GameObject("LaserFieldStructure");
            objLFS.ConvertToPrefab(true);
            LaserFieldStructure laserFieldStructure = objLFS.AddComponent<LaserFieldStructure>();
            laserFieldStructure.Prefab = LaserField;
            AssetMan.Add<LaserFieldStructure>("Structure_LaserField", laserFieldStructure);
            yield return "Loading shafts assets";
            ShaftType = EnumExtensions.ExtendEnum<LevelType>("Shaft");
            AssetMan.Add<Sprite>("spr_shafts_floor", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "Shafts_Floor.png"));
            AssetMan.Add<Sprite>("spr_shafts_ceiling", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "Shafts_Roof.png"));
            AssetMan.Add<Sprite>("spr_shafts_wall", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "Shafts_Wall.png"));
            AssetMan.Add<Sprite>("spr_shafts_light", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 50, "Shafts_Light.png"));
            AssetMan.Add<Sprite>("spr_bigcloud", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 32, "Cloud_Sprite_Large.png"));
            AssetMan.Add<Sprite>("spr_ShaftVent", AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "ShaftVent.png"));
            AssetMan.Add<WindowObject>("Window_Shafts", ObjectCreators.CreateWindowObject("ShaftsWindow",
                AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "ShaftsWindow.png").texture,
                AssetLoader.SpriteFromMod(this, Vector2.one / 2, 10, "ShaftsWindow_Broken.png").texture));
            AssetMan.Add("Aud_VentLoop", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, "VentNoise_Loop.wav"), "sfx_ventLoop", SoundType.Effect, Color.white));
            AssetMan.Add("Aud_VentShoot", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, "VentNoise_Shoot.wav"), "sfx_ventShoot", SoundType.Effect, Color.white));
            AssetMan.Add("Aud_SteamLeak", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, "ValveLeak.wav"), "sfx_steamleak", SoundType.Effect, Color.white, 0.01f));



            GameObject ParticlePrefabCloud = new GameObject("ParticleCloudPrefab");
            ParticlePrefabCloud.layer = LayerMask.NameToLayer("Billboard");
            var spriteRenderer = ParticlePrefabCloud.AddComponent<SpriteRenderer>();
            spriteRenderer.material = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "SpriteStandard_Billboard");
            spriteRenderer.sprite = AssetMan.Get<Sprite>("spr_bigcloud");
            spriteRenderer.color = Color.red;
            ParticlePrefabCloud.ConvertToPrefab(true);


            GameObject SteamPrefabObject = new GameObject("SteamBlockagePrefab");
            SteamPrefabObject.ConvertToPrefab(true);
            SteamBlockage SteamBlockageComp = SteamPrefabObject.AddComponent<SteamBlockage>();
            var colliderSteam = SteamPrefabObject.AddComponent<BoxCollider>();
            colliderSteam.size = new Vector3(10, 10, 2.5f);
            SteamBlockageComp.prefab = ParticlePrefabCloud;
            SteamBlockageComp.AudMan = SteamPrefabObject.AddComponent<PropagatedAudioManager>();
            SteamBlockageComp.AudMan.ReflectionSetVariable("minDistance", 25);
            SteamBlockageComp.AudMan.ReflectionSetVariable("maxDistance", 150);

            GameObject SteamBlockage = new GameObject("SteamBlockageStructure");
            SteamBlockage.ConvertToPrefab(true);
            Structure_SteamBlockage SteamBlockageStructure = SteamBlockage.AddComponent<Structure_SteamBlockage>();
            SteamBlockageStructure.prefab = SteamBlockageComp;

            AssetMan.Add<Structure_SteamBlockage>("Structure_SB", SteamBlockageStructure);

            GameObject LightFlickerStructure = new GameObject("LightFlickerStructure");
            LightFlickerStructure.ConvertToPrefab(true);
            Structure_LightFlickerManager LightFlickerStruc = SteamBlockage.AddComponent<Structure_LightFlickerManager>();
            AssetMan.Add<Structure_LightFlickerManager>("LFM", LightFlickerStruc);
            // Creating SteamSteam Prefab
            var SteamSteamPrefab = new EntityBuilder()
                .SetLayer("CollidableEntities")
                .SetBaseRadius(0.5f)
                .AddTrigger(0.5f)
                
                .SetHeight(10)
                .AddDefaultRenderBaseFunction(AssetMan.Get<Sprite>("spr_bigcloud"))
                .Build();
            SteamSteamPrefab.gameObject.AddComponent<SteamSteam>();
            
            var LightPrefab = new GameObject("LightShafts");
            var BaseLightRenderer = new GameObject("BaseRenderer");
            var lightRenderer = BaseLightRenderer.AddComponent<SpriteRenderer>();
            BaseLightRenderer.transform.SetParent(LightPrefab.transform, false);
            lightRenderer.gameObject.layer = LayerMask.NameToLayer("Billboard");
            
            LightPrefab.ConvertToPrefab(true);
            lightRenderer.transform.localPosition += new Vector3(0, 8.5f, 0);
            lightRenderer.material = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "SpriteStandard_Billboard");
            lightRenderer.sprite = AssetMan.Get<Sprite>("spr_shafts_light");
            AssetMan.Add<Transform>("light_Shafts", LightPrefab.transform);

            // creating SteamShooterBox and material
            Material ShaftVent = new Material(BaseMaterial)
            {
                name = "ShaftVent",
                mainTexture = AssetMan.Get<Sprite>("spr_ShaftVent").texture
            };

            GameObject SteamShooterBox =GameObject.CreatePrimitive(PrimitiveType.Cube);
            SteamShooterBox.transform.localScale = new Vector3(10f, 10f, 1f);
            SteamShooterBox.GetComponent<Renderer>().material = ShaftVent;
            SteamShooterBox.ConvertToPrefab(true);
            var SteamShooter = SteamShooterBox.AddComponent<SteamShooter>();
            SteamShooter.prefab = SteamSteamPrefab;
            var audManSteam = SteamShooterBox.AddComponent<PropagatedAudioManager>();
            audManSteam.ReflectionSetVariable("minDistance", 25);
            audManSteam.ReflectionSetVariable("maxDistance", 200);
            GameObject StructureSteamShooterObj = new GameObject("SteamShooterStructure");
            StructureSteamShooterObj.ConvertToPrefab(true);
            Structure_SteamShooter structure_SteamShooter = StructureSteamShooterObj.AddComponent<Structure_SteamShooter>();
            structure_SteamShooter.prefab = SteamShooter;
            AssetMan.Add<Structure_SteamShooter>("SS", structure_SteamShooter);
            yield return "loading Greenhouse level type assets";


            yield return "Adding level typed Support, if no level typed then this will be skipped";
            if (Chainloader.PluginInfos.ContainsKey("mtm101.rulerp.baldiplus.leveltyped"))
            {
                LeveltypedAdder.Add();
            }


        }
        ///<summary>
        /// Turns the specified level object into a party bash variant
        /// </summary>
        /// <param name="toModify">Level object to modify</param>
        /// <param name="levelId"></param>
        /// <returns></returns>
        #region Partybash floor type
        public void ModifyIntoPartybash(LevelObject toModify, int levelId)
        {
            toModify.hallCeilingTexs = [new WeightedTexture2D() {
                selection = AssetMan.Get<Sprite>("spr_partybash_ceiling").texture,
                weight = 99
            }];
            toModify.hallWallTexs = [new WeightedTexture2D() {
                selection = AssetMan.Get<Sprite>("spr_partybash_wall").texture,
                weight = 99
            }];
            toModify.hallFloorTexs = [new WeightedTexture2D() {
                selection = AssetMan.Get<Sprite>("spr_partybash_floor").texture,
                weight = 99
            }];
            toModify.forcedStructures = toModify.forcedStructures.AddRangeToArray(new StructureWithParameters[] {
                 new StructureWithParameters() {
                    parameters = new() {
                        minMax = new IntVector2[] {
                            new(1,1)
                        }

                    },
                    prefab = AssetMan.Get<Structure_BallonSpawner>("BalloonSpawner"),


                },
                new StructureWithParameters()
                {
                    parameters = new()
                    {
                        minMax = [new IntVector2(4, 20),
                        new IntVector2(4,10)],
                        chance = [0.45f],
                    },
                    prefab = Resources.FindObjectsOfTypeAll<Structure_Lockers>().First()

                },
                new StructureWithParameters()
                {
                    parameters = new()
                    {
                        minMax = [new IntVector2(40, 50)],
                        chance = [0.45f],
                        prefab = [new() {
                            selection = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Plant_Offset"),
                            weight = 99
                        }]
                    },
                    prefab = Resources.FindObjectsOfTypeAll<Structure_EnvironmentObjectPlacer>().First()

                },
                new StructureWithParameters()
            {
                parameters = new()
                {
                    minMax = [new IntVector2(5,0)],
                    chance = [0.25f],
                    prefab = new WeightedGameObject[] {
                        new WeightedGameObject() {
                            selection = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Door_Swinging"),
                            weight = 99
                        }
                    }
                },
                prefab = Resources.FindObjectsOfTypeAll<Structure_HallDoor>().First(x => x.name == "SwingingDoorConstructor")

            }
            });
            toModify.potentialItems = [
                new WeightedItemObject() {
                    selection = AssetMan.Get<ItemObject>("itm_present"),
                    weight = 50
                },
                new WeightedItemObject() {
                    selection = AssetMan.Get<ItemObject>("itm_ytppresent"),
                    weight = 30
                }
                ];

            toModify.maxEvents = 1;
            toModify.minEvents = 1;
            toModify.randomEvents = [
                new WeightedRandomEvent() {
                    selection = Resources.FindObjectsOfTypeAll<PartyEvent>().First(),
                    weight = 99
                }
                ];
            toModify.forcedItems = [];


        }
        #endregion
        ///<summary>
        /// Turns the specified level object into a Techy variant
        /// </summary>
        /// <param name="toModify">Level object to modify</param>
        /// <param name="levelId"></param>
        /// <returns></returns>
        #region Techy floor type
        public void ModifyIntoTechy(LevelObject toModify, int levelId)
        {
            toModify.hallCeilingTexs = [new WeightedTexture2D() {
                selection = AssetMan.Get<Sprite>("spr_techy_ceiling").texture,
                weight = 99
            }];
            toModify.hallWallTexs = [new WeightedTexture2D() {
                selection = AssetMan.Get<Sprite>("spr_techy_wall").texture,
                weight = 99
            }];
            toModify.hallFloorTexs = [new WeightedTexture2D() {
                selection = AssetMan.Get<Sprite>("spr_techy_floor").texture,
                weight = 99
            }];

            toModify.maxSpecialRooms = 1;
            toModify.minSpecialRooms = 1;
            toModify.potentialSpecialRooms = [
                new WeightedRoomAsset() {
                    selection = ButtonsroomLayout[0],
                    weight = 100
                }
                ];
            toModify.maxSize += new IntVector2(13, 10);
            toModify.minSize += new IntVector2(13, 10);

            toModify.forcedStructures = toModify.forcedStructures.AddRangeToArray(new StructureWithParameters[] {
                new StructureWithParameters() {
                    parameters = new StructureParameters() {
                        minMax = new IntVector2[] { new IntVector2(5,12) },
                        prefab = new WeightedGameObject[] {
                            new WeightedGameObject() {
                                selection = LaserField.gameObject,
                                weight = 99
                            }
                        }
                    },
                    prefab = AssetMan.Get<LaserFieldStructure>("Structure_LaserField")

                },
                new StructureWithParameters()
                {
                    parameters = new()
                    {
                        minMax = [new IntVector2(40, 50)],
                        chance = [0.45f],
                        prefab = [new() {
                            selection = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Plant_Offset"),
                            weight = 99
                        }]
                    },
                    prefab = Resources.FindObjectsOfTypeAll<Structure_EnvironmentObjectPlacer>().First()

                },
                new StructureWithParameters() {
                    parameters = new StructureParameters() {
                        minMax = new IntVector2[] { new IntVector2(2,4), new IntVector2(0,12) }
                    },
                    prefab = Resources.FindObjectsOfTypeAll<Structure_Rotohalls>().First()

                },
                new StructureWithParameters()
                {
                    parameters = new()
                    {
                        minMax = [new IntVector2(4, 20),
                        new IntVector2(4,10)],
                        chance = [0.45f],
                    },
                    prefab = Resources.FindObjectsOfTypeAll<Structure_Lockers>().First()

                },
                new StructureWithParameters()
            {
                parameters = new()
                {
                    minMax = [new IntVector2(5,0)],
                    chance = [0.25f],
                    prefab = new WeightedGameObject[] {
                        new WeightedGameObject() {
                            selection = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Door_Swinging"),
                            weight = 99
                        }
                    }
                },
                prefab = Resources.FindObjectsOfTypeAll<Structure_HallDoor>().First(x => x.name == "SwingingDoorConstructor")

            }
            });

            RoomGroup ElectricalRoomGroup = new RoomGroup();
            ElectricalRoomGroup.floorTexture = [new WeightedTexture2D() {
                selection = AssetMan.Get<Sprite>("spr_elc_floor").texture,
                weight = 999}];
            ElectricalRoomGroup.wallTexture = [new WeightedTexture2D() {
                selection = AssetMan.Get<Sprite>("spr_elc_wall").texture,
                weight = 999}];
            ElectricalRoomGroup.ceilingTexture = [new WeightedTexture2D() {
                selection = AssetMan.Get<Sprite>("spr_elc_ceil").texture,
                weight = 999}];
            ElectricalRoomGroup.potentialRooms = [
                new WeightedRoomAsset() {
                    selection = ElectricalRoomLayout[0],
                    weight = 100
                },
                new WeightedRoomAsset() {
                    selection = ElectricalRoomLayout[1],
                    weight = 80
                },
                new WeightedRoomAsset() {
                    selection = ElectricalRoomLayout[2],
                    weight = 40
                },
                new WeightedRoomAsset() {
                    selection = ElectricalRoomLayout[3],
                    weight = 45
                }
                ];
            ElectricalRoomGroup.maxRooms = 1;
            ElectricalRoomGroup.minRooms = 1;
            ElectricalRoomGroup.light = toModify.hallLights;
            ElectricalRoomGroup.name = "ElectricalRoomGroup";
            toModify.roomGroup = toModify.roomGroup.AddToArray(ElectricalRoomGroup);
            foreach (var room in toModify.roomGroup)
            {
                room.light = [new WeightedTransform() {
                    selection = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "HangingLight").transform,
                    weight = 9999
                }];
                
            }
            toModify.hallLights = [new WeightedTransform() {
                    selection = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "HangingLight").transform,
                    weight = 9999
                }];
        }
        #endregion
        #region Shafts floor type
        public void ModifyIntoShafts(LevelObject toModify, int levelId)
        {
            toModify.hallCeilingTexs = [new WeightedTexture2D() {
                selection = AssetMan.Get<Sprite>("spr_shafts_ceiling").texture,
                weight = 99
            }];
            toModify.hallWallTexs = [new WeightedTexture2D() {
                selection = AssetMan.Get<Sprite>("spr_shafts_wall").texture,
                weight = 99
            }];
            toModify.hallFloorTexs = [new WeightedTexture2D() {
                selection = AssetMan.Get<Sprite>("spr_shafts_floor").texture,
                weight = 99
            }];


            toModify.maxSize += new IntVector2(14, 20);
            toModify.minSize += new IntVector2(14, 20);
            toModify.minPlots *= 8;
            toModify.maxPlots *= 8;
            toModify.maxReplacementHalls *= 12;
            toModify.minReplacementHalls *= 12;
            toModify.bridgeTurnChance += 500;
            toModify.forcedStructures = toModify.forcedStructures.AddRangeToArray(new StructureWithParameters[] {
                new StructureWithParameters()
                {
                    parameters = new()
                    {
                        minMax = [new IntVector2(3, 6)],
                    },
                    prefab = AssetMan.Get<Structure_SteamBlockage>("Structure_SB")

                },
                new StructureWithParameters()
                {
                    parameters = new()
                    {
                        minMax = [new IntVector2(40, 50)],
                        chance = [0.45f],
                        prefab = [new() {
                            selection = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Plant_Offset"),
                            weight = 99
                        }]
                    },
                    prefab = Resources.FindObjectsOfTypeAll<Structure_EnvironmentObjectPlacer>().First()

                },
                new StructureWithParameters()
                {
                    parameters = new()
                    {
                        minMax = [new IntVector2(3, 6)],
                    },
                    prefab = AssetMan.Get<Structure_LightFlickerManager>("LFM")

                },
                new StructureWithParameters()
                {
                    parameters = new()
                    {
                        minMax = [new IntVector2(2, 4)],
                    },
                    prefab = AssetMan.Get<Structure_SteamShooter>("SS")

                },
                new StructureWithParameters()
                {
                    parameters = new()
                    {
                        minMax = [new IntVector2(4, 20),
                        new IntVector2(4,10)],
                        chance = [0.45f],
                    },
                    prefab = Resources.FindObjectsOfTypeAll<Structure_Lockers>().First()

                },
                new StructureWithParameters()
            {
                parameters = new()
                {
                    minMax = [new IntVector2(5,0)],
                    chance = [0.25f],
                    prefab = new WeightedGameObject[] {
                        new WeightedGameObject() {
                            selection = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Door_Swinging"),
                            weight = 99
                        }
                    }
                },
                prefab = Resources.FindObjectsOfTypeAll<Structure_HallDoor>().First(x => x.name == "SwingingDoorConstructor")

            }
            });

            foreach (var room in toModify.roomGroup)
            {
                room.light = [new WeightedTransform() {
                    selection = AssetMan.Get<Transform>("light_Shafts"),
                    weight = 9999
                }];


            }
            toModify.hallLights = [new WeightedTransform() {
                    selection = AssetMan.Get<Transform>("light_Shafts"),
                    weight = 9999
                }];

        }
        #endregion
        public bool shouldGenerateFloorType(string levelName, int LevelId, SceneObject scene, string TypeName)
        {
            if (levelName != "F4" && levelName != "F5" 
                && Config.Bind<bool>("Enables the floor type " + TypeName + " disabling it will make it not spawn on F4 and F5",TypeName,true).Value) return false;
            return true;
        }
        #region floor type creators
        void PartyBashTypeCreator(string levelName, int levelId, SceneObject scene)
        {
            if (!shouldGenerateFloorType(levelName, levelId, scene, "PartyBash")) return;
            CustomLevelObject[] supportedObjects = scene.GetCustomLevelObjects();
            CustomLevelObject factorylevel = supportedObjects.First(x => x.type == LevelType.Factory);
            if (factorylevel == null) return;
            CustomLevelObject PartyBashClone = factorylevel.MakeClone();
            PartyBashClone.type = PartyBashType;
            PartyBashClone.name = "Partybash";
            List<StructureWithParameters> structures = PartyBashClone.forcedStructures.ToList();
            structures.RemoveAll(x => x.prefab is Structure_Rotohalls);
            structures.RemoveAll(x => x.prefab is Structure_ConveyorBelt);
            structures.RemoveAll(x => x.prefab.name == "LockdownDoorConstructor");
            structures.RemoveAll(x => x.prefab is Structure_LevelBox);
            structures.Add(new StructureWithParameters()
            {
                parameters = new()
                {
                    minMax = [new IntVector2(5,0)],
                    chance = [0.25f]
                },
                prefab = Resources.FindObjectsOfTypeAll<Structure_HallDoor>().First(x => x.name == "SwingingDoorConstructor")

            });
            
            PartyBashClone.forcedStructures = structures.ToArray();


            ModifyIntoPartybash(PartyBashClone, levelId);
            scene.randomizedLevelObject = scene.randomizedLevelObject.AddToArray(new WeightedLevelObject()
            {
                selection = PartyBashClone,
                weight = 100
            });



        }

        void TechyTypeCreator(string levelName, int levelId, SceneObject scene)
        {
            if (!shouldGenerateFloorType(levelName, levelId, scene, "Techy")) return;
            CustomLevelObject[] supportedObjects = scene.GetCustomLevelObjects();
            CustomLevelObject factorylevel = supportedObjects.First(x => x.type == LevelType.Factory);
            if (factorylevel == null) return;
            CustomLevelObject TechyClone = factorylevel.MakeClone();
            TechyClone.type = TechyType;
            TechyClone.name = "TechyClone";
            List<StructureWithParameters> structures = TechyClone.forcedStructures.ToList();
            structures.RemoveAll(x => x.prefab is Structure_Rotohalls);
            structures.RemoveAll(x => x.prefab is Structure_ConveyorBelt);
            structures.RemoveAll(x => x.prefab.name == "LockdownDoorConstructor");
            structures.RemoveAll(x => x.prefab is Structure_LevelBox);
             structures.Add(new StructureWithParameters()
            {
                parameters = new()
                {
                    minMax = [new IntVector2(5,0)],
                    chance = [0.25f]
                },
                prefab = Resources.FindObjectsOfTypeAll<Structure_HallDoor>().First(x => x.name == "SwingingDoorConstructor")

            });
            TechyClone.forcedStructures = structures.ToArray();
            ModifyIntoTechy(TechyClone, levelId);
            scene.randomizedLevelObject = scene.randomizedLevelObject.AddToArray(new WeightedLevelObject()
            {
                selection = TechyClone,
                weight = 100
            });



        }

        void ShaftsTypeCreator(string levelName, int levelId, SceneObject scene)
        {
            if (!shouldGenerateFloorType(levelName, levelId, scene, "Shafts")) return;
            CustomLevelObject[] supportedObjects = scene.GetCustomLevelObjects();
            CustomLevelObject factorylevel = supportedObjects.First(x => x.type == LevelType.Factory);
            if (factorylevel == null) return;
            CustomLevelObject ShaftsClone = factorylevel.MakeClone();
            ShaftsClone.type = ShaftType;
            ShaftsClone.name = "ShaftClone";
            List<StructureWithParameters> structures = ShaftsClone.forcedStructures.ToList();
            structures.RemoveAll(x => x.prefab is Structure_Rotohalls);
            structures.RemoveAll(x => x.prefab is Structure_ConveyorBelt);
            structures.RemoveAll(x => x.prefab.name == "LockdownDoorConstructor");
            structures.RemoveAll(x => x.prefab is Structure_LevelBox);
           
            ShaftsClone.forcedStructures = structures.ToArray();
            
            ModifyIntoShafts(ShaftsClone, levelId);
            scene.randomizedLevelObject = scene.randomizedLevelObject.AddToArray(new WeightedLevelObject()
            {
                selection = ShaftsClone,
                weight = 100
            });



        }
        void GreenhouseTypeCreator(string levelName, int levelId, SceneObject scene)
        {
            if (!shouldGenerateFloorType(levelName, levelId, scene, "Shafts")) return;
            CustomLevelObject[] supportedObjects = scene.GetCustomLevelObjects();
            CustomLevelObject factorylevel = supportedObjects.First(x => x.type == LevelType.Factory);
            if (factorylevel == null) return;
            CustomLevelObject ShaftsClone = factorylevel.MakeClone();
            ShaftsClone.type = ShaftType;
            ShaftsClone.name = "ShaftClone";
            List<StructureWithParameters> structures = ShaftsClone.forcedStructures.ToList();
            structures.RemoveAll(x => x.prefab is Structure_Rotohalls);
            structures.RemoveAll(x => x.prefab is Structure_ConveyorBelt);
            structures.RemoveAll(x => x.prefab.name == "LockdownDoorConstructor");
            structures.RemoveAll(x => x.prefab is Structure_LevelBox);
           
            ShaftsClone.forcedStructures = structures.ToArray();
            
            ModifyIntoShafts(ShaftsClone, levelId);
            scene.randomizedLevelObject = scene.randomizedLevelObject.AddToArray(new WeightedLevelObject()
            {
                selection = ShaftsClone,
                weight = 100
            });



        }
        #endregion

        void LevelTypeCreatorHandler(string levelName, int levelId, SceneObject scene)
        {
            PartyBashTypeCreator(levelName, levelId, scene);
            TechyTypeCreator(levelName, levelId, scene);
            ShaftsTypeCreator(levelName, levelId, scene);
        }
        void genModification(string levelName, int levelId, SceneObject scene)
        {

            foreach (var toModify in scene.GetCustomLevelObjects())
            {
                toModify.potentialSpecialRooms =
                [
                new WeightedRoomAsset()
                {
                    selection = ButtonsroomLayout[0],
                    weight = 100
                },
                new WeightedRoomAsset()
                {
                    selection = ButtonsroomLayout[1],
                    weight = 50
                },
                new WeightedRoomAsset()
                {
                    selection = ButtonsroomLayout[2],
                    weight = 15
                }
                ];
            }
        }
        

        void Awake()
        {
            instance = this;
            modPath = AssetLoader.GetModPath(this);
            AssetMan = new AssetManager();
            GUID = this.Info.Metadata.GUID;
            LoadingEvents.RegisterOnAssetsLoaded(this.Info, PreLoad(),false);
            MTM101BaldAPI.AssetTools.AssetLoader.LocalizationFromMod(this);
            GeneratorManagement.Register(this, GenerationModType.Preparation, LevelTypeCreatorHandler);
            // Debug purposes
            //GeneratorManagement.Register(this, GenerationModType.Override, genModification);
            Harmony h = new Harmony(GUID);
            h.PatchAll();
    }
}
        }
    

