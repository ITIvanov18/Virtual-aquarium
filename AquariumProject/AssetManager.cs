using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

/*
 * Име: Иван Тенев Иванов
 * Фак. No: F115436
 * Клас: AssetManager
 * Описание: Този клас отговаря единствено за управлението на графичните ресурси.
 * Той реализира "Singleton" или "Static" модел за достъп до кешираните изображения,
 * разделяйки логиката на визуализацията от логиката на формата.
 */

namespace AquariumProject
{
    public static class AssetManager
    {
        // ОПТИМИЗАЦИЯ (Image Caching)
        // ползвам dictionaries за кеширане на изображенията
        // това предотвратява тежките операции(resize/rotate) във всеки кадър
        private static Dictionary<int, Image> cachedFishRight = new Dictionary<int, Image>();
        private static Dictionary<int, Image> cachedFishLeft = new Dictionary<int, Image>();

        // флаг дали вече са заредени картинките
        private static bool isLoaded = false;

        // ОПТИМИЗАЦИЯ: Обхожда всички видове риби, изчислява техните пропорции,
        // преоразмерява ги и ги запазва в кеша (за ляво и дясно движение)
        public static void LoadResources()
        {
            if (isLoaded) return; // зарежда само веднъж

            for (int i = 1; i <= 8; i++)
            {
                byte[] rawBytes = GetBytesByType(i);

                using (MemoryStream ms = new MemoryStream(rawBytes))
                {
                    using (Image original = Image.FromStream(ms))
                    {
                        // изчислява на оригиналното съотношение (Aspect Ratio)
                        double ratio = (double)original.Height / original.Width;

                        // смалява базовия размер до 350px ширина за оптимизация,
                        // НО височината се смята автоматично според пропорцията
                        int newWidth = 350;
                        int newHeight = (int)(newWidth * ratio);

                        // създава на оптимизирано копие (гледащо надясно)
                        Image resizedImg = ResizeImage(original, newWidth, newHeight);
                        cachedFishRight.Add(i, resizedImg);

                        // създава на огледално копие (гледащо наляво)
                        Image flippedImg = (Image)resizedImg.Clone();
                        flippedImg.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        cachedFishLeft.Add(i, flippedImg);
                    }
                }
            }
            isLoaded = true;
        }

        // помощен метод за безопасно взимане на картинка според типа и посоката
        public static Image GetFishImage(int type, bool movingRight)
        {
            if (!isLoaded) LoadResources();

            if (movingRight)
            {
                if (cachedFishRight.ContainsKey(type)) return cachedFishRight[type];
            }
            else
            {
                if (cachedFishLeft.ContainsKey(type)) return cachedFishLeft[type];
            }

            // Fallback (ако нещо липсва)
            return null;
        }

        // помощен метод, който връща ресурсните данни (byte array) според избрания тип риба
        private static byte[] GetBytesByType(int type)
        {
            switch (type)
            {
                case 1: return Properties.Resources.fish1;
                case 2: return Properties.Resources.fish2;
                case 3: return Properties.Resources.fish3;
                case 4: return Properties.Resources.fish4;
                case 5: return Properties.Resources.pufferfish;
                case 6: return Properties.Resources.seahorse;
                case 7: return Properties.Resources.shark;
                case 8: return Properties.Resources.swordfish;
                default: return Properties.Resources.fish1;
            }
        }

        // помощен метод за висококачествено оразмеряване (без пикселизация)
        private static Image ResizeImage(Image imgToResize, int width, int height)
        {
            Bitmap b = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, 0, 0, width, height);
            }
            return b;
        }
    }
}