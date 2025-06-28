using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using PlusLevelLoader;
using MTM101BaldAPI;
using BaldiLevelEditor;
using HarmonyLib;
using PlusLevelFormat;
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

        public static void AddObjectToEditor(this GameObject obj)
        {
            PlusLevelLoaderPlugin.Instance.prefabAliases.Add(obj.name, obj);
            BasePlugin.AssetMan.Add($"editorPrefab_{obj.name}", obj);
            obj.ConvertToPrefab(true);
        }

        public static Sprite ResizeSprite(this Sprite sprite, int newWidth, int newHeight, float pixelsPerUnit = float.NaN, Vector2? center = null)
        {
            if (center == null)
                center = sprite.pivot;
            if (float.IsNaN(pixelsPerUnit))
                pixelsPerUnit = sprite.pixelsPerUnit;
            if (center == sprite.pivot && pixelsPerUnit == sprite.pixelsPerUnit && sprite.texture.width == newWidth && sprite.texture.height == newHeight)
                return sprite;
            if (float.IsNaN(pixelsPerUnit))
                pixelsPerUnit = sprite.pixelsPerUnit;
            Vector2 vector2;
            if (center == null)
                vector2 = Vector2.one / 2f;
            else
                vector2 = new Vector2(center.Value.x, center.Value.y);
            return Sprite.Create(sprite.texture.ResizeTexture(newWidth, newHeight), new Rect(0, 0, newWidth, newHeight), vector2, pixelsPerUnit);
        }
        public static Texture2D ResizeTexture(this Texture2D sourceTexture, int newWidth, int newHeight)
        {
            if (sourceTexture == null)
                return AssetsHelper.CreateTexture(newWidth, newHeight, Color.clear);
            if (sourceTexture.width == newWidth && sourceTexture.height == newHeight)
                return sourceTexture;
            Texture2D reference = sourceTexture.CopyTexture();
            Texture2D resizedTexture = new Texture2D(newWidth, newHeight, sourceTexture.format, false);
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    float xRatio = (float)x / newWidth;
                    float yRatio = (float)y / newHeight;
                    Color color = reference.GetPixelBilinear(xRatio, yRatio);
                    resizedTexture.SetPixel(x, y, color);
                }
            }
            resizedTexture.Apply();
            return resizedTexture;
        }
        public static Texture2D CopyTexture(this Texture2D original)
        {
            Texture2D copy = new Texture2D(original.width, original.height, original.format, false);
            if (original.isReadable)
            {
                copy.SetPixels(original.GetPixels());
                return copy;
            }
            RenderTexture rt = new RenderTexture(original.width, original.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(original, rt);
            RenderTexture.active = rt;
            copy.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            copy.Apply();
            RenderTexture.active = null;
            rt.Release();
            return copy;
        }
    }


}
