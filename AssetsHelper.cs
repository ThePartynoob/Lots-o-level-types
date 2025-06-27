
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx.Bootstrap;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace Lots_o__level_types
{
    class AssetsHelper
    {
        
        public static Texture2D CreateTexture(int size, bool includeAlpha = false) =>
            CreateTexture(size, GenerateRandomColor(includeAlpha));
        public static Texture2D CreateTexture(int size, Color color) =>
            CreateTexture(size, size, color);
        public static Texture2D CreateTexture(int width, int height, bool includeAlpha = false)
        {
            return CreateTexture(width, height, GenerateRandomColor(includeAlpha));
        }
        
        
        public static Texture2D CreateTexture(int width, int height, Color color)
        {
            Texture2D texture2D = new Texture2D(width, height);
            for (int x = 0; x < texture2D.width; x++)
            {
                for (int y = 0; y < texture2D.height; y++)
                {
                    texture2D.SetPixel(x, y, color);
                }
            }
            texture2D.Apply();
            return texture2D;
        }
        
        public static Color GenerateRandomColor(bool includeAlpha = false) 
        {
            float r = Random.Range(0, 256) / 256f;
            float g = Random.Range(0, 256) / 256f;
            float b = Random.Range(0, 256) / 256f;
            float a = 1f;
            if (includeAlpha)
                a = Random.Range(0, 256) / 256f;
            return new Color(r, g, b, a);
        }
       
    }
}