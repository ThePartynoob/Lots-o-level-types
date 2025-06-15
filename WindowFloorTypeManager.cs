using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MTM101BaldAPI.Reflection;
namespace Lots_o__level_types
{
    [HarmonyPatch]
    static internal class WindowFloorTypeManager
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Window),"Initialize")]
        static void WindowPatchInit(Window __instance)
        {
            if (Singleton<BaseGameManager>.Instance.GetType() != typeof(MainGameManager)) return;
            var lvltype = Singleton<BaseGameManager>.Instance.levelObject.type;
            if (lvltype == BasePlugin.ShaftType)
            {
                __instance.ReflectionSetVariable("windowObject", BasePlugin.AssetMan.Get<WindowObject>("Window_Shafts"));
                __instance.UpdateTextures();
            }

        }
    }
}
