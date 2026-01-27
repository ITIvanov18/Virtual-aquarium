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

            this.Width = 1080;
            this.Height = 640;

            // взима байтовете на картинката
            byte[] imageBytes = (byte[])Properties.Resources.aquarium;

            // създава поток, който трябва да живее, докато приложението работи
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

            // генерира тип от 1 до 9 (1-8)
            int randomType = rnd.Next(1, 9);

            // --- умна логика за размера ---
            int randomSizeW;

            // проверяваме дали е акула (7) или риба меч (8)
            if (randomType == 7 || randomType == 8)
            {
                // големите риби: между 200 и 300 пиксела
                randomSizeW = rnd.Next(200, 301);
            }
            else
            {
                // малките риби: между 60 и 100 пиксела
                randomSizeW = rnd.Next(60, 101);
            }
            int randomSizeH = (int)(randomSizeW * 0.6);

            // създава новата риба
            Fish newFish = new Fish(
                0,
                rnd.Next(0, this.Height - 150),
                rnd.Next(5, 15),
                randomType
            );

            newFish.Width = randomSizeW;
            newFish.Height = randomSizeH;

            aquariumFish.Add(newFish);
        }

        private void запишиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // прозорец за запис
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Aquarium Files (*.xml)|*.xml"; // филтър само за XML
            saveDialog.Title = "Запази състоянието на аквариума";
            saveDialog.FileName = "MyAquarium.xml"; // име по подразбиране

            // показва прозореца и чака потребителят да цъкне "Save"
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                // използва името, което потребителят е избрал (saveDialog.FileName)
                XmlSerializer serializer = new XmlSerializer(typeof(List<Fish>));

                using (TextWriter writer = new StreamWriter(saveDialog.FileName))
                {
                    serializer.Serialize(writer, aquariumFish);
                }

                MessageBox.Show("Успешно запазване!", "Информация");
            }
        }

        private void заредиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // създава прозорец за отваряне
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Aquarium Files (*.xml)|*.xml";
            openDialog.Title = "Избери аквариум за зареждане";

            // ако потребителят избере файл и цъкне "Open"
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Fish>));

                    using (FileStream fs = new FileStream(openDialog.FileName, FileMode.Open))
                    {
                        aquariumFish = (List<Fish>)serializer.Deserialize(fs);
                    }

                    // прерисува веднага, за да се видят новите риби
                    Invalidate();
                    MessageBox.Show("Аквариумът е зареден успешно!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Грешка при зареждане: " + ex.Message);
                }
            }
        }
    }
}