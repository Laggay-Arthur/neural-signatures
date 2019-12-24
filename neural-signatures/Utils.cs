using System.Drawing;
using System.Windows.Forms;


namespace neural_signatures
{
    class Utils
    {
        public static void ClearImage(PictureBox[] Box)
        { foreach (PictureBox pb in Box) pb.Image = new Bitmap(pb.Width, pb.Height); }

        public static int[,] ArrayFromBitmap(Bitmap image)
        {
            int[,] res = new int[image.Width, image.Height];

            for (int n = 0; n < res.GetLength(0); n++)
                for (int m = 0; m < res.GetLength(1); m++)
                {
                    int color = (image.GetPixel(n, m).R + image.GetPixel(n, m).G + image.GetPixel(n, m).B) / 3;
                    res[n, m] = color > 0 ? 1 : 0;
                }
            return res;
        }

        public static Bitmap BitmapFromArr(ref int[,] array)
        {
            Bitmap bitmap = new Bitmap(array.GetLength(0), array.GetLength(1));

            for (int x = 0; x < array.GetLength(0); x++)
                for (int y = 0; y < array.GetLength(1); y++)
                    bitmap.SetPixel(x, y, array[x, y] == 0 ? Color.White : Color.Black);

            return bitmap;
        }

        public static int[,] ImageToArray(Bitmap b, Point max)
        {// обрезает рисунок по краям и преобразовать в массив
            int x1 = 0, y1 = 0, x2 = max.X, y2 = max.Y;

            bool flag = true;

            for (int y = 0; y < b.Height && flag; y++)
                for (int x = 0; x < b.Width && flag; x++)
                    if (b.GetPixel(x, y).ToArgb() != 0) { y1 = y; flag = false; }

            flag = true;

            for (int y = b.Height - 1; y >= 0 && y2 == max.Y && flag; y--)
                for (int x = 0; x < b.Width && y2 == max.Y && flag; x++)
                    if (b.GetPixel(x, y).ToArgb() != 0) { y2 = y; flag = false; }

            flag = true;

            for (int x = 0; x < b.Width && flag; x++)
                for (int y = 0; y < b.Height && flag; y++)
                    if (b.GetPixel(x, y).ToArgb() != 0) { x1 = x; flag = false; }

            flag = true;

            for (int x = b.Width - 1; x >= 0 && x2 == max.X && flag; x--)
                for (int y = 0; y < b.Height && x2 == max.X && flag; y++)
                    if (b.GetPixel(x, y).ToArgb() != 0) { x2 = x; flag = false; }

            if (x1 == 0 && y1 == 0 && x2 == max.X && y2 == max.Y) return null;

            int size = x2 - x1 > y2 - y1 ? x2 - x1 + 1 : y2 - y1 + 1;
            int dx = y2 - y1 > x2 - x1 ? ((y2 - y1) - (x2 - x1)) / 2 : 0;
            int dy = y2 - y1 < x2 - x1 ? ((x2 - x1) - (y2 - y1)) / 2 : 0;

            int[,] res = new int[size, size];

            for (int x = 0; x < res.GetLength(0); x++)
                for (int y = 0; y < res.GetLength(1); y++)
                {
                    int pX = x + x1 - dx;
                    int pY = y + y1 - dy;

                    if (pX < 0 || pX >= max.X || pY < 0 || pY >= max.Y)
                        res[x, y] = 0;

                    else
                        res[x, y] = b.GetPixel(x + x1 - dx, y + y1 - dy).ToArgb() == 0 ? 0 : 1;
                }
            return res;
        }

        public static void Simplify(ref int[,] source, ref int[,] res)
        {// пересчитывает массив исходных пикселей в выходной массив
            for (int n = 0; n < res.GetLength(0); n++)
                for (int m = 0; m < res.GetLength(1); m++)
                    res[n, m] = 0;

            double pX = (double)res.GetLength(0) / source.GetLength(0);
            double pY = (double)res.GetLength(1) / source.GetLength(1);

            for (int n = 0; n < source.GetLength(0); n++)
                for (int m = 0; m < source.GetLength(1); m++)
                {
                    int posX = (int)(n * pX);
                    int posY = (int)(m * pY);

                    if (res[posX, posY] == 0) res[posX, posY] = source[n, m];
                }
        }

        public static Bitmap CutImage(Rectangle rect, ref Bitmap bmp)
        {        
            Bitmap newBmp = new Bitmap(60, 60); 

            for (int x = rect.X, i = 0; x < rect.Width; x++, i++)
                for (int y = rect.Y, g = 0; y < rect.Height; y++, g++)
                    newBmp.SetPixel(i, g, bmp.GetPixel(x, y));

            return newBmp;
        }
    }
}