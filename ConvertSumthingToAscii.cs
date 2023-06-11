using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FromImageToAscii
{
    class ConvertSumthingToAscii
    {
        //Переменная, определяющая максимальное кол-во символов в ширину
        public static int WIDTH_SIZE = 300;
        //Переменная, редактирующая отношение сторон
        protected static double WIDTH_OFF_SET = 2;
        //Массив, представляющий из себя символы, которыми будет кодироваться изобржаение
        protected static char[] _asciiTable = { '@', '#', '$', '%', '?', '*', '+', ':', ',', '.' };

        //Метод, изменяющий размер битмапа в соответсвии с переменными WIDTH_SIZE и WIDTH_OFF_SET;
        protected static Bitmap ResizeBitmap(Bitmap bitmap)
        {
                var newHeight = bitmap.Height / WIDTH_OFF_SET * WIDTH_SIZE / bitmap.Width;

                if (bitmap.Width > WIDTH_SIZE || bitmap.Height > newHeight)
                    bitmap = new Bitmap(bitmap, new System.Drawing.Size(WIDTH_SIZE, (int)newHeight));

                return bitmap;
        }
        //Преобразование битмапа в зубчатый массив чаров
        protected static char[][] ToCharArray(Bitmap bitmap)
        {
            var result = new char[bitmap.Height][];

            for (int y = 0; y < bitmap.Height; y++)
            {
                result[y] = new char[bitmap.Width];
                for (int x = 0; x < bitmap.Width; x++)
                {
                    //Получаем индекс символа, относитлеьного его яркости, в массиве символов
                    int mapIndex = (int)Map(bitmap.GetPixel(x, y).R, 0, 255, 0, _asciiTable.Length - 1);
                    
                    result[y][x] = _asciiTable[mapIndex];
                }
            }

            return result;
        }
        //Метод получения индекса массива, относительно его яркости
        protected static float Map(float valueToMap, float start1, float stop1, float start2, float stop2)
        {
            return ((valueToMap - start1) / (stop1 - start1)) * (stop2 - start2) + start2;
        }

        //Преобразование зубчатого массива в строку(кадр) по средством StringBuilder  
        protected static string GetStr(char[][] myArry)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var row in myArry)
            {
                foreach (var symbol in row)
                {
                    stringBuilder.Append(symbol.ToString());
                }
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }
    }
}
