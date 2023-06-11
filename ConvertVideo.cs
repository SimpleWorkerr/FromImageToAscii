using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;
using Microsoft.Win32;
using OpenCvSharp;

namespace FromImageToAscii
{
    internal class ConvertVideo : ConvertSumthingToAscii
    {

        //Преобразование видео-файла в массив строк(кадров)
        public static Task<string[]> Convert()
        {
            Task<string[]> convertVideo = new Task<string[]>(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog()
                {
                    Filter = "Videos | *.mp4"
                };

                openFileDialog.ShowDialog();

                
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                Dictionary<int, Bitmap> map = GetBitmapDicFromVideo(openFileDialog.FileName);

                MessageBox.Show($"GetBitmapDicFromVideo: {stopWatch.ElapsedMilliseconds}");
                stopWatch.Reset();
                stopWatch.Start();
                

                string[] result = new string[map.Count];

                //Запускаем обработку словаря через Parallel.For так-как это ускоряет обработку
                Parallel.For(0, map.Count, (int i) =>
                {
                    map[i] = ResizeBitmap(map[i]);
                    map[i].ToGrayScale();

                    result[i] = GetStr(ToCharArray(map[i]));
                });

                MessageBox.Show($"Convert: {stopWatch.ElapsedMilliseconds}");
                stopWatch.Stop();

                return result;
            });

            convertVideo.Start();

            return convertVideo;
        }
        //Преобразование тип Mat в тип Bitmap
        private static Bitmap MatToBitmap(Mat mat)
        {
            using (var ms = mat.ToMemoryStream())
            {
                return (Bitmap)System.Drawing.Image.FromStream(ms);
            }
        }
        //Конвертация объекта типа videoCapture в словарь с ключом типа int и значением Bitmap
        private static Dictionary<int, Bitmap> GetBitmapDicFromVideo(string path)
        {
            var videoFile = path;
            var capture = new VideoCapture(videoFile);
            var image = new Mat();
            var dic_image = new Dictionary<int, Bitmap>();

            var i = 0;
            
            while (capture.IsOpened())
            {
                capture.Read(image);

                if (image.Empty())
                    break;

                dic_image[i] = MatToBitmap(image);

                i++;
            }
            return dic_image;
        }
    }
}
