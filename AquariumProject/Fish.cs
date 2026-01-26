using System;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

/*
 * Име: Иван Иванов
 * Фак. No: F115436 
 * Клас: Fish
 * Описание: Класът описва обекта риба, нейните координати, 
 * вид (коя картинка да ползва) и логиката за движение
 */

namespace AquariumProject
{
    // класът трябва да е Public за да работи сериализацията
    [Serializable]
    public class Fish
    {
        // свойства за позиция и размер
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; } = 120;  // Стандартна ширина
        public int Height { get; set; } = 100; // Стандартна височина

        // скорост на движение по хоризонтала и вертикала
        public int SpeedX { get; set; }
        public int SpeedY { get; set; }

        // тип на рибата (1 = fish1, 2 = fish2)
        public int FishType { get; set; }

        // празен конструктор - задължителен за XML сериализацията
        public Fish() { }

        // основен конструктор за създаване на нова риба
        public Fish(int startX, int startY, int speedX, int type)
        {
            this.X = startX;
            this.Y = startY;
            this.SpeedX = speedX;
            this.SpeedY = 0; // засега само хоризонтално
            this.FishType = type;
        }

        // метод за изчисляване на новата позиция
        public void Move(int boundaryWidth)
        {
            // променя позицията
            X += SpeedX;

            // проверка за удар в стената (обръщане на посоката)
            // ако стигне десния край ИЛИ левия край
            if (X + Width >= boundaryWidth || X <= 0)
            {
                SpeedX = -SpeedX; // обръща знака на скоростта
            }
        }

        // помощен метод, който връща картинката според типа (не се сериализира)
        public Image GetImage()
        {
            // помощна функция за конвертиране от byte[] към Image
            Image ImageFromBytes(byte[] bytes)
            {
                using (var ms = new MemoryStream(bytes))
                {
                    return Image.FromStream(ms);
                }
            }

            switch (FishType)
            {
                case 1: return ImageFromBytes(Properties.Resources.fish1);
                case 2: return ImageFromBytes(Properties.Resources.fish2);
                case 3: return ImageFromBytes(Properties.Resources.fish3);
                case 4: return ImageFromBytes(Properties.Resources.fish4);
                case 5: return ImageFromBytes(Properties.Resources.pufferfish);
                case 6: return ImageFromBytes(Properties.Resources.seahorse);
                case 7: return ImageFromBytes(Properties.Resources.shark);
                case 8: return ImageFromBytes(Properties.Resources.swordfish);
                default: return ImageFromBytes(Properties.Resources.fish4);
            }
        }
    }
}