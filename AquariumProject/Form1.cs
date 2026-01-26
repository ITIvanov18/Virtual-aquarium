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

        private void добавиРибкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int randomType = rnd.Next(1, 9);
            int randomSizeW = rnd.Next(60, 120);
            int randomSizeH = (int)(randomSizeW * 0.7);

            Fish newFish = new Fish(
                0,
                rnd.Next(0, this.Height - 150),
                rnd.Next(5, 20),
                randomType
            );

            newFish.Width = randomSizeW;
            newFish.Height = randomSizeH;

            aquariumFish.Add(newFish);
        }

        private void запишиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Fish>));
            using (TextWriter writer = new StreamWriter("aquarium.xml"))
            {
                serializer.Serialize(writer, aquariumFish);
            }
            MessageBox.Show("Аквариумът е запазен!");
        }

        private void заредиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists("aquarium.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Fish>));
                using (FileStream fs = new FileStream("aquarium.xml", FileMode.Open))
                {
                    aquariumFish = (List<Fish>)serializer.Deserialize(fs);
                }
                Invalidate();
            }
        }
    }
}