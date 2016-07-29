using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GpsGateProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Color[,] colorArray = new Color[500, 500];
        private int height;
        private int width;
        private Color backgroundColor = new Color()
        {
            R = 255,
            G = 255,
            B = 255
        };

        private static readonly Random Random = new Random();


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            height = colorArray.GetUpperBound(0) + 1;
            width = colorArray.GetUpperBound(1) + 1;

            // init colors
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    colorArray[y, x] = backgroundColor;
                }
            }

            printColorArray();
        }

        private void printColorArray()
        {
            // http://stackoverflow.com/questions/11928884/writing-a-2d-array-of-colors-to-a-wpf-image

            var pixelFormat = PixelFormats.Bgra32;
            var stride = width * 4; // bytes per row

            byte[] pixelData = new byte[height * stride];

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    var color = colorArray[y, x];


                    var index = (y * stride) + (x * 4);

                    pixelData[index] = color.B;
                    pixelData[index + 1] = color.G;
                    pixelData[index + 2] = color.R;
                    pixelData[index + 3] = color.A; // color.A;
                }
            }

            BitmapSource bitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null, pixelData, stride);


            // http://stackoverflow.com/questions/5338253/bitmapsource-to-bitmapimage
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            BitmapImage bImg = new BitmapImage();

            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(memoryStream);

            memoryStream.Position = 0;
            bImg.BeginInit();
            bImg.StreamSource = new MemoryStream(memoryStream.ToArray());
            bImg.EndInit();
            bImg.Freeze();

            imgImage.Source = bImg;
        }

        public Point p1 { get; set; } = new Point(-1, -1);
        public Point p2 { get; set; } = new Point(-1, -1);
        private async void imgImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Reset points if both have been added
            if (p2 != new Point(-1, -1))
            {
                p1 = new Point(-1, -1);
                p2 = new Point(-1, -1);
            }
            Point p = e.GetPosition((IInputElement)sender);

            if (p1 == new Point(-1, -1))
            {
                p1 = p;
            }
            else
            {
                p2 = p;

                List<Node> path = new List<Node>();
                // Calculate path
                path = await Task.Run(() =>
                {
                    try
                    {
                        return Dijkstra.Run(p1, p2, width, height);
                    }
                    catch (NoSuchPathException)
                    {
                        MessageBox.Show("No such path!");
                    }
                    catch (StartAlreadyPartOfPathException)
                    {
                        MessageBox.Show("Start point is already a part of a path!");
                    }
                    catch (EndAlreadyPartOfPathException)
                    {
                        MessageBox.Show("End point is already a part of a path!");
                    }
                    return new List<Node>();
                });
                // Draw path to colorArray
                setPathColors(path);
            }
            printColorArray();
        }

        private void setPathColors(List<Node> path)
        {
            Color pathColor = new Color()
            {
                R = Convert.ToByte(Random.Next(0, 150)),
                G = Convert.ToByte(Random.Next(0, 150)),
                B = Convert.ToByte(Random.Next(0, 150))
            };
            foreach (Node item in path)
            {
                if (colorArray[item.Y, item.X] != backgroundColor)
                {
                    throw new Exception("Pathes crossing!!!");
                }
                colorArray[item.Y, item.X] = pathColor;
            }
        }
    }
}
