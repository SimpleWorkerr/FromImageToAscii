using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace FromImageToAscii
{
    internal class ConvertImg : ConvertSumthingToAscii
    {
        //Преобразование объекта Bitmap в строку
        public static Task<string> Convert()
        {
                Task<string> result = new Task<string>(() =>
                {

                    var OpenFileDialog = new OpenFileDialog()
                    {
                        Filter = "Images | *.bmp; *.png; *.jpg; *.JPEG"
                    };
                    OpenFileDialog.ShowDialog();

                    Bitmap bitmap = new Bitmap(OpenFileDialog.FileName);
                    bitmap = ResizeBitmap(bitmap);
                    bitmap.ToGrayScale();

                    return GetStr(ToCharArray(bitmap));

                });

                result.Start();

                return result;
            
        }

    }
}
