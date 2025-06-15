using System;
using System.Collections.Generic;
using System.Text;

namespace Lots_o__level_types
{
    internal static class Extensions
    {
        private static RoomFunction Func;
        /// <summary>
        /// Adds a <see cref="RoomFunction"/> to every RoomAssets
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="RoomAssets"></param>
        public static void AddFunctionToAllRoomAssets<T>(this RoomAsset[] RoomAssets) where T : RoomFunction
        {
            foreach (RoomAsset room in RoomAssets)
            {
                if (room.roomFunctionContainer.gameObject.GetComponent<T>() == null)
                {
                    Func = room.roomFunctionContainer.gameObject.AddComponent<T>();
                }
                else continue;
                room.roomFunctionContainer.AddFunction(Func);
            }
        }
    }
}
