using System;
using System.Collections.Generic;
using System.Text;
using LevelTyped;
using MTM101BaldAPI;
using UnityEngine;
namespace Lots_o__level_types
{
    static public class LeveltypedAdder
    {
        public static void Add()
        {
            LevelTyped.LevelTypedPlugin.Instance.AddExtraGenerator(new LevelTypedPartybash());
            LevelTyped.LevelTypedPlugin.Instance.AddExtraGenerator(new LevelTypedTechy());
            LevelTyped.LevelTypedPlugin.Instance.AddExtraGenerator(new LevelTypedShafts());
            
        }
    }


    internal class LevelTypedPartybash : LevelTypedGenerator
    {
        public override LevelType levelTypeToBaseOff => LevelType.Factory;

        public override LevelType myLevelType => BasePlugin.PartyBashType;

        public override string levelObjectName => "Partybash";

        public override void ApplyChanges(string levelName, int levelId, CustomLevelObject obj)
        {
            obj.type = BasePlugin.PartyBashType;
            obj.forcedStructures = [];


            BasePlugin.instance.ModifyIntoPartybash(obj, levelId);
        }

        public override bool ShouldGenerate(string levelName, int levelId, SceneObject sceneObject)
        {
            return !BasePlugin.instance.shouldGenerateFloorType(levelName, levelId, sceneObject, "PartyBash");
        }

        public override int GetWeight(int defaultWeight)
        {
            return Mathf.CeilToInt(base.GetWeight(defaultWeight) * 1.1f);
        }
    }
    internal class LevelTypedTechy : LevelTypedGenerator
    {
        public override LevelType levelTypeToBaseOff => LevelType.Factory;

        public override LevelType myLevelType => BasePlugin.TechyType;

        public override string levelObjectName => "Techy";

        public override void ApplyChanges(string levelName, int levelId, CustomLevelObject obj)
        {
            obj.type = BasePlugin.TechyType;
            obj.name = "Techy";
            obj.forcedStructures = [];


            BasePlugin.instance.ModifyIntoTechy(obj, levelId);
        }

        public override bool ShouldGenerate(string levelName, int levelId, SceneObject sceneObject)
        {
            return !BasePlugin.instance.shouldGenerateFloorType(levelName, levelId, sceneObject, "Techy");
        }

        public override int GetWeight(int defaultWeight)
        {
            return Mathf.CeilToInt(base.GetWeight(defaultWeight) * 1.1f);
        }
    }
    internal class LevelTypedShafts : LevelTypedGenerator
    {
        public override LevelType levelTypeToBaseOff => LevelType.Factory;

        public override LevelType myLevelType => BasePlugin.ShaftType;

        public override string levelObjectName => "Shafts";

        public override void ApplyChanges(string levelName, int levelId, CustomLevelObject obj)
        {
            obj.type = BasePlugin.ShaftType;
            obj.name = "Shafts";
            obj.forcedStructures = [];


            BasePlugin.instance.ModifyIntoShafts(obj, levelId);
        }

        public override bool ShouldGenerate(string levelName, int levelId, SceneObject sceneObject)
        {
            return !BasePlugin.instance.shouldGenerateFloorType(levelName, levelId, sceneObject, "Shafts");
        }

        public override int GetWeight(int defaultWeight)
        {
            return Mathf.CeilToInt(base.GetWeight(defaultWeight) * 1.1f);
        }
    }
    
}
