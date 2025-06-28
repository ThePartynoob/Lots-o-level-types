using System.Collections.Generic;
using UnityEngine;
using static Lots_o__level_types.CustomEnums;

namespace Lots_o__level_types
{
    public class PlantManager : MonoBehaviour
    {
        Dictionary<PlantTypes, WeightedPlantEffects[]> PlantEffectDict = new Dictionary<PlantTypes, WeightedPlantEffects[]>
        {
            { PlantTypes.Eggplant,  [new() {
                selection = PlantEffects.SpeedUp,
                weight = 80
            },
            new() {
                selection = PlantEffects.SpeedDown,
                weight = 65
            },
            new() {
                selection = PlantEffects.None,
                weight = 30
            },
            new() {
                selection = PlantEffects.Poke,
                weight = 20
            },
            new() {
                selection = PlantEffects.Block,
                weight = 15
            }] }
        };
        void Start()
        {
            
        }
    }
}