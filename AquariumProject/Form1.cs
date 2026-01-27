using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Windows.Forms.AxHost;

/*
 * Име: Иван Тенев Иванов
 * Фак. No: F115436
 * Описание: Основната форма на приложението. Управлява логиката на визуализацията, 
 * рендерирането на графиката и взаимодействието с потребителя
 */

namespace AquariumProject
{
    public partial class Form1 : Form
    {
        // списък, съхраняващ всички инстанции на риби в аквариума
        List<Fish> aquariumFish = new List<Fish>();

        // ОПТИМИЗАЦИЯ (Image Caching)
        // ползвам dictionaries за кеширане на изображенията
        // това предотвратява тежките операции(resize/rotate) във всеки кадър
        private Dictionary<int, Image> cachedFishRight = new Dictionary<int, Image>();
        private Dictionary<int, Image> cachedFishLeft = new Dictionary<int, Image>();

        // променливи за изчисляване на FPS
        private int frames = 0;
        private double fps = 0;
        private DateTime lastFpsTime = DateTime.Now;
        private Font infoFont = new Font("Consolas", 12, FontStyle.Bold); // шрифт за брояча

        // Конструктор на формата. Инициализира компонентите, включва
        // двойното буфериране и стартира процеса по зареждане на ресурсите
        public Form1()
        {
            InitializeComponent();

            // включва DoubleBuffered за по-гладка анимация без трептене (flickering)
            this.DoubleBuffered = true;

            this.Width = 1080;
            this.Height = 640;

            // зарежда на ресурсите (фон и риби)
            LoadBackground();
            PreloadFishImages();
        }

        // зарежда фоновото изображение MemoryStream, за да избегне GDI+ грешки
        private void LoadBackground()
        {
            // взима байтовете на картинката от ресурсите
            byte[] imageBytes = (byte[])Properties.Resources.aquarium;

            // създава поток в паметта, който остава отворен докато приложението работи
            MemoryStream ms = new MemoryStream(imageBytes);

            this.BackgroundImage = Image.FromStream(ms);
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }



        // ОПТИМИЗАЦИЯ: Обхожда всички видове риби, изчислява техните пропорции,
        // преоразмерява ги и ги запазва в кеша (за ляво и дясно движение)
        private void PreloadFishImages()
        {
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
        }

        // таймер, който се изпълнява на всеки интервал (tick) и обновява 
        // логическите координати на всички обекти
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            foreach (var fish in aquariumFish)
            {
                fish.Move(this.ClientSize.Width);
            }
            // предизвиква прерисуване на формата (извиква OnPaint)
            Invalidate();
        }

        /// Метод за изчертаване на графиката. Използва кеширани изображения за максимална производителност
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // рисуване на рибите
            foreach (var fish in aquariumFish)
            {
                Image imgToDraw;
                if (fish.SpeedX > 0) 
                    imgToDraw = cachedFishRight[fish.FishType];
                else 
                    imgToDraw = cachedFishLeft[fish.FishType];

                e.Graphics.DrawImage(imgToDraw, fish.X, fish.Y, fish.Width, fish.Height);
            }

            // изчисляване и визуализация на FPS 
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

        // помощен метод, който връща ресурсните данни (byte array) според избрания тип риба
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

        /// обработчик на събитието за добавяне на нова риба от менюто
        private void добавиРибкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();

            int randomType = rnd.Next(1, 9);   // генерира тип от 1 до 8

            // --- умна логика за размера ---
            // взима картинката от кеша, за да използва реалните ѝ пропорции
            Image baseImage = cachedFishRight[randomType];
            double ratio = (double)baseImage.Height / baseImage.Width;

            // определя ширината на случаен принцип (хищниците са по-големи)
            int randomWidth;
            if (randomType == 7 || randomType == 8) 
                randomWidth = rnd.Next(220, 350);
            else 
                randomWidth = rnd.Next(80, 140);

            // височината се изчислява автоматично спрямо пропорцията
            int randomHeight = (int)(randomWidth * ratio);

            Fish newFish = new Fish(
                0, 
                rnd.Next(0, this.Height - 150), 
                rnd.Next(5, 15),  // скорост
                randomType        // тип риба
                );
            newFish.Width = randomWidth;
            newFish.Height = randomHeight;

            aquariumFish.Add(newFish);
        }

        // сериализира текущия списък с риби в XML файл
        private void запишиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Aquarium Files (*.xml)|*.xml"; // филтър само за XML
            saveDialog.Title = "Запази състоянието на аквариума";
            saveDialog.FileName = "MyAquarium.xml"; // име по подразбиране

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Fish>));
                using (TextWriter writer = new StreamWriter(saveDialog.FileName))
                {
                    serializer.Serialize(writer, aquariumFish);
                }
                MessageBox.Show("Успешно запазване!", "Информация");
            }
        }

        // десериализира списък с риби от XML файл
        private void заредиToolStripMenuItem_Click(object sender, EventArgs e)
        {
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

                    // обновява веднага, за да се видят новите риби
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