using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using DesktopBd.Service;

namespace DesktopBd.MVVM.Model
{
    public class DesktopModel
    {
        #region Get/Set
       
        /// <summary>
        /// Колличество ярлыков на данном рабочем столе
        /// </summary>
        public Int16 LabelCount
        {
            get => _labelCount;
            set
            {
                _labelCount = value;
            }
        }

        /// <summary>
        /// Путь к картинке рабочего стола
        /// </summary>
        public String WallpaperImage
        {
            get => _wallpaperImage;
            set
            {
                _wallpaperImage = value;
            }
        }

        #endregion

        #region Fields

        private Int16 _labelCount = 0;

        private String _wallpaperImage = String.Empty;

        #endregion

    }
}
