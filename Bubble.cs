using System;
using System.Drawing;

/*
 * Име: Иван Тенев Иванов
 * Фак. No: F115436
 * Клас: Bubble
 * Описание: Модел на балонче във виртуалния аквариум.
 * Управлява неговите координати, размер, прозрачност и синусоидално движение нагоре.
 */

namespace AquariumProject
{
    public class Bubble
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Size { get; set; }
        public float Speed { get; set; }

        // полупрозрачен бял цвят
        private Brush bubbleBrush = new SolidBrush(Color.FromArgb(80, 255, 255, 255));
        private Pen bubblePen = new Pen(Color.FromArgb(150, 255, 255, 255), 1);

        private static Random rnd = new Random();

        public Bubble(int screenWidth, int screenHeight)
        {
            Reset(screenWidth, screenHeight);
            // за да не тръгват всички от дъното в началото, 
            // се разпръскват по целия екран първия път
            Y = rnd.Next(0, screenHeight);
        }

        public void Move(int screenHeight, int screenWidth)
        {
            Y -= Speed; // движение нагоре

            // леко клатушкане наляво-надясно за реализъм
            X += (float)(Math.Sin(Y * 0.05) * 0.5);

            // ако балончето излезе отгоре, го връщаме долу
            if (Y + Size < 0)
            {
                Reset(screenWidth, screenHeight);
            }
        }

        public void Draw(Graphics g)
        {
            // плътен кръг (пълнеж)
            g.FillEllipse(bubbleBrush, X, Y, Size, Size);
            // контур
            g.DrawEllipse(bubblePen, X, Y, Size, Size);

            // малък отблясък за 3D ефект
            g.FillEllipse(Brushes.White, X + (Size * 0.2f), Y + (Size * 0.2f), Size / 3, Size / 3);
        }

        // ресетва балончето долу с нови случайни параметри
        private void Reset(int w, int h)
        {
            X = rnd.Next(0, w);
            Y = h + rnd.Next(10, 100); // малко под екрана
            Size = rnd.Next(5, 20);    // случаен размер (5px до 20px)
            Speed = (float)(rnd.NextDouble() * 2 + 1); // случайна скорост (1 до 3)
        }
    }
}