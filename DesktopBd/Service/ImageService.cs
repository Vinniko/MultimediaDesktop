using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using System.Drawing;
using System.Windows;
using System.IO;

namespace DesktopBd.Service
{
    public static class ImageService
    {

        #region Main Logic

        /// <summary>
        /// Вытягивает из ярлыка его изображение
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Image ImageFromFile(string path)
        {
            var icon = System.Drawing.Icon.ExtractAssociatedIcon(path);
            Image image = icon.ToBitmap();
            return image;
        }

        /// <summary>
        /// Вытягивает картинку из графического файла
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Image ImageFile(string path)
        {
            Image image;
            image = Image.FromFile(path);
            return image;
        }

        /// <summary>
        /// Загружает картинку для байндинга.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BitmapImage LoadLabelImage(String path)
        {

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new FileStream(path, FileMode.Open, FileAccess.Read);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.StreamSource.Dispose();
            return bitmapImage;
        }

        /// <summary>
        /// Создание картинки для ярлыка
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BitmapImage ImageCreaterForLabel(string path)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new FileStream(path, FileMode.Open, FileAccess.Read);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.DecodePixelWidth = 333;
            bitmapImage.EndInit();
            return bitmapImage;
        }
        
        /// <summary>
        /// Создание картинки для рабочего стола
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BitmapImage ImageCreaterForDesktop(string path)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new FileStream(path, FileMode.Open, FileAccess.Read);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.DecodePixelWidth = 1280;
            bitmapImage.EndInit();
            return bitmapImage;
        }

        #endregion

    }
}
