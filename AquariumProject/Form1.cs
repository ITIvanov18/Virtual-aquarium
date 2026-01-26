using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace AquariumProject
{
    public partial class Form1 : Form
    {
        List<Fish> aquariumFish = new List<Fish>();

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            // взима байтовете на картинката
            byte[] imageBytes = (byte[])Properties.Resources.aquarium;

            // създава поток, но НЕ слагаме 'using', за да не го затворим!
            // този поток трябва да живее, докато приложението работи
            MemoryStream ms = new MemoryStream(imageBytes);

            this.BackgroundImage = Image.FromStream(ms);
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            foreach (var fish in aquariumFish)
            {
                fish.Move(this.ClientSize.Width);
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            foreach (var fish in aquariumFish)
            {
                Image originalImg = fish.GetImage();

                Image imgToDraw = originalImg;
                bool isRotated = false; // флаг дали е създадена нова картинка

                if (fish.SpeedX < 0)
                {
                    imgToDraw = new Bitmap(originalImg);
                    imgToDraw.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    isRotated = true;
                }

                e.Graphics.DrawImage(imgToDraw, fish.X, fish.Y, fish.Width, fish.Height);

                // чисти само ако е създадена нова (обърната) картинка
                if (isRotated)
                {
                    imgToDraw.Dispose();
                }
            }
        }
    }
}