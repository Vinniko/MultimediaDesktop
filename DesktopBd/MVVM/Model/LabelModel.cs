using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DesktopBd.MVVM.Model
{
    public sealed class LabelModel
    {

        #region Constructors

        public LabelModel()
        {

        }

        /// <summary>
        /// Конструктор для создания none ярлыка
        /// </summary>
        /// <param name="typeFlag"></param>
        public LabelModel(Boolean typeFlag)
        {
            Title = String.Empty;
            ImagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\label_image_none.png");
            Path = String.Empty;
            TypeFlag = typeFlag;
        }

        /// <summary>
        /// Конструктор для создания обычного ярлыка
        /// </summary>
        /// <param name="task"></param>
        /// <param name="imagePath"></param>
        /// <param name="path"></param>
        /// <param name="typeFlag"></param>
        public LabelModel(String title, String imagePath, String path, Boolean typeFlag)
        {
            Title = title;
            ImagePath = imagePath;
            Path = path;
            TypeFlag = typeFlag;
        }

        #endregion



        #region Get/Set

        /// <summary>
        /// Название ярлыка
        /// </summary>
        public String Title
        {
            get => _title;
            set
            {
                _title = value;
            }
        } 
        /// <summary>
        /// Путь к картинке ярлыка
        /// </summary>
        public String ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;

            }
        }
        /// <summary>
        /// Путь ярлыка
        /// </summary>
        public String Path
        {
            get => _path;
            set
            {
                _path = value;
            }
        }

        /// <summary>
        /// Тип ярылка (none или обычный)
        /// </summary>
        public Boolean TypeFlag
        {
            get => _typeFlag;
            set
            {
                _typeFlag = value;
            }
        }
        /// <summary>
        /// Флаг картинки
        /// </summary>
        public Boolean ImageFlag
        {
            get => _imageFlag;
            set
            {
                _imageFlag = value;
            }
        }

        #endregion



        #region Fields

        private String _title;
        private String _imagePath;
        private String _path;

        private Boolean _typeFlag;
        private Boolean _imageFlag;
       
        #endregion

    }
}
