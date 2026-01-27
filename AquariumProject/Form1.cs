using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

/*
 * Име: Иван Тенев Иванов
 * Фак. No: F115436
 * Описание: Основната форма на приложението, която управлява визуализацията, 
 * анимацията и потребителското взаимодействие.
 */

namespace AquariumProject
{
    public partial class Form1 : Form
    {
        // списък с всички активни риби в аквариума
        List<Fish> aquariumFish = new List<Fish>();

        // ОПТИМИЗАЦИЯ: dictionaries за кеширане на изображенията
        // това предотвратява създаването на нови Bitmap обекти при всяко прерисуване
        private Dictionary<int, Image> cachedFishRight = new Dictionary<int, Image>();
        private Dictionary<int, Image> cachedFishLeft = new Dictionary<int, Image>();

        // ПРОМЕНЛИВИ ЗА FPS и ИНФО
        private int frames = 0;
        private double fps = 0;
        private DateTime lastFpsTime = DateTime.Now;
        private Font infoFont = new Font("Consolas", 12, FontStyle.Bold); // шрифт за брояча

        // конструктор на формата, който нициализира компонентите и зарежда ресурсите
        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            this.Width = 1080;
            this.Height = 640;

            // зарежда на ресурсите (Фон и Риби)
            LoadBackground();
            PreloadFishImages();
        }

        // зарежда фоновото изображение по безопасен начин, за да избегне GDI+ грешки
        private void LoadBackground()
        {
            // взима байтовете на картинката от ресурсите
            byte[] imageBytes = (byte[])Properties.Resources.aquarium;

            // създава поток в паметта, който остава отворен докато приложението работи
            MemoryStream ms = new MemoryStream(imageBytes);

            this.BackgroundImage = Image.FromStream(ms);
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }



        // ОПТИМИЗАЦИЯ: предварително зарежда и обработва всички видове риби
        // създава версии за ляво и дясно движение, за да не го прави в OnPaint
        private void PreloadFishImages()
        {
            for (int i = 1; i <= 8; i++)
            {
                byte[] rawBytes = GetBytesByType(i);

                using (MemoryStream ms = new MemoryStream(rawBytes))
                {
                    using (Image original = Image.FromStream(ms))
                    {
                        // ИЗЧИСЛЯВАНЕ НА РЕАЛНИТЕ ПРОПОРЦИИ
                        // взима оригиналното съотношение (напр. 600/800 = 0.75)
                        double ratio = (double)original.Height / original.Width;

                        // смалява базовия размер до 350px ширина за оптимизация,
                        // НО височината се смята автоматично според пропорцията!
                        int newWidth = 350;
                        int newHeight = (int)(newWidth * ratio);

                        // създава качествено смалено копие
                        Image resizedImg = ResizeImage(original, newWidth, newHeight);

                        // запазва се в кеша (надясно)
                        cachedFishRight.Add(i, resizedImg);

                        // създава се обърната версия (няляво)
                        Image flippedImg = (Image)resizedImg.Clone();
                        flippedImg.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        cachedFishLeft.Add(i, flippedImg);
                    }
                }
            }
        }

        // таймер, който отговаря за обновяването на логиката (координатите)
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            // премества всяка риба според нейната скорост
            foreach (var fish in aquariumFish)
            {
                fish.Move(this.ClientSize.Width);
            }
            // заявка, че формата трябва да се прерисува
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // рисуване на рибите
            foreach (var fish in aquariumFish)
            {
                Image imgToDraw;
                if (fish.SpeedX > 0) imgToDraw = cachedFishRight[fish.FishType];
                else imgToDraw = cachedFishLeft[fish.FishType];

                e.Graphics.DrawImage(imgToDraw, fish.X, fish.Y, fish.Width, fish.Height);
            }

            // логика за FPS брояча
            frames++;
            double secondsElapsed = (DateTime.Now - lastFpsTime).TotalSeconds;
            if (secondsElapsed >= 1)
            {
                fps = frames / secondsElapsed;
                frames = 0;
                lastFpsTime = DateTime.Now;
            }

            // рисуване на текста
            string infoText = $"FPS: {fps:F0} | Риби: {aquariumFish.Count}";
            e.Graphics.DrawString(infoText, infoFont, Brushes.Black, 12, 32); // сянка
            e.Graphics.DrawString(infoText, infoFont, Brushes.White, 10, 30); // текст
        }

        // помощен метод за висококачествено оразмеряване (без пикселизация)
        private Image ResizeImage(Image imgToResize, int width, int height)
        {
            Bitmap b = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, 0, 0, width, height);
            }
            return b;
        }

        // помощен метод, който връща суровите данни (byte[]) от ресурсите
        private byte[] GetBytesByType(int type)
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

        private void добавиРибкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();

            // генерира тип от 1 до 9 (1-8)
            int randomType = rnd.Next(1, 9);

            // --- умна логика за размера ---
            // взима картинката от кеша, за да види истинската ѝ пропорция
            Image baseImage = cachedFishRight[randomType];
            double ratio = (double)baseImage.Height / baseImage.Width;

            int randomWidth;
            if (randomType == 7 || randomType == 8) randomWidth = rnd.Next(220, 350);
            else randomWidth = rnd.Next(80, 140);

            // смята височината правилно
            int randomHeight = (int)(randomWidth * ratio);

            Fish newFish = new Fish(0, rnd.Next(0, this.Height - 150), rnd.Next(5, 20), randomType);
            newFish.Width = randomWidth;
            newFish.Height = randomHeight;

            aquariumFish.Add(newFish);

            aquariumFish.Add(newFish);
        }

        private void запишиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // прозорец за запис
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Aquarium Files (*.xml)|*.xml"; // филтър само за XML
            saveDialog.Title = "Запази състоянието на аквариума";
            saveDialog.FileName = "MyAquarium.xml"; // име по подразбиране

            // показва прозорца и чака потребителят да цъкне "Save"
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