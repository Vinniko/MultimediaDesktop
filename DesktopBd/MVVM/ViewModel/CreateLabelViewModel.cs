using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopBd.Service;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using DesktopBd.MVVM.Model;

namespace DesktopBd.MVVM.ViewModel 
{
    public class CreateLabelViewModel : BaseVM
    {
        #region Constructors

        /// <summary>
        /// Передача логина пользователя
        /// </summary>
        /// <param name="userLogin"></param>
        public CreateLabelViewModel(String userLogin)
        {
            UserLogin = userLogin;
        }

        #endregion



        #region Commands

        /// <summary>
        /// Добавление ярлыка ярлыка из папки в окно
        /// </summary>
        public RelayCommand AddLabel
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    DialogCreater(GetDialogType(FileType));
                });
            }
        }

        /// <summary>
        /// Добавление новой картинки для ярлыка
        /// </summary>
        public RelayCommand AddImage
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    LabelImage =  SetPersonalImage();
                });
            }
        }

        /// <summary>
        /// Создание ярлыка на рабочем столе
        /// </summary>
        public RelayCommand CreateLabel
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    LabelCreateEvent(LabelPath, Task, FileService.SaveImage(Image.FromFile(ImagePath), UserLogin, Task));
                    CloseFlag = false;
                    ViewManager.CloseView();
                });
            }
        }

        /// <summary>
        /// Закрытие окна
        /// </summary>
        public RelayCommand CloseWindow
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if(CloseFlag) ViewManager.CloseView();
                });
            }
        }

        #endregion



        #region Events

        public delegate void LabelCreateCompleatEventHandler(String labelPath, String task, String imagePath);
        public event LabelCreateCompleatEventHandler LabelCreateEvent;

        #endregion



        #region Main Logic

        /// <summary>
        /// Создание диалогового окна определенного типа
        /// </summary>
        /// <param name="type"></param>
        public void DialogCreater(String type)
        {
            if (type != null)
            {
                var dialog = new OpenFileDialog();
                dialog.Filter = type;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LabelPath = dialog.FileName;
                    LabelPathInfo = String.Concat(InfoTask + LabelPath);
                    PathViewToolTipFlag = true;
                    ImagePath = FileService.SaveTmpImage(ImageService.ImageFromFile(dialog.FileName), UserLogin);
                    LabelImage = ImageService.LoadLabelImage(ImagePath);
                    Task = dialog.SafeFileName;
                    ImageFlag = false;
                    CreateFlag = true;
                }
            }
            else
            {
                var Dialog = new FolderBrowserDialog();
                if (Dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LabelPath = Dialog.SelectedPath;
                    LabelPathInfo = String.Concat(InfoTask + LabelPath);
                    PathViewToolTipFlag = true;
                    ImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\label_image_01.png");
                    LabelImage = ImageService.LoadLabelImage(ImagePath);
                    Task = new DirectoryInfo(Dialog.SelectedPath).Name;
                    ImageFlag = true;
                    CreateFlag = true;
                }
            }
        }

        /// <summary>
        /// Выбор типа создаваемого диалогового окна
        /// </summary>
        /// <param name="selectedType"></param>
        /// <returns></returns>
        public String GetDialogType(Int16 selectedType)
        {
            String tmp_type = String.Empty;
            switch (selectedType)
            {
                case 0:
                    tmp_type = "Файлы All|*.*";
                    return tmp_type;
                case 1:
                    tmp_type = "Текстовые файлы (*.docx/*.txt/*.pdf)|*.txt;*.doc;*.docx;*.pdf;*.xml;*.rtf";
                    return tmp_type;
                case 2:
                    return null;
                case 3:
                    tmp_type = "Звуковые файлы (*.mp3/*.wma/*.wav)|*.mp3;*.wma;*.wav";
                    return tmp_type;
                case 4:
                    tmp_type = "Графические файлы (*.jpeg/*.bmp/*.png/*.gif)|*.tiff;*.jpeg;*.bmp;*.png;*.gif;*.jpg";
                    return tmp_type;
                case 5:
                    tmp_type = "Файлы мультимедиа (*.mp4/*.wav/*.avi/*.mov)|*.mp4;*.wav;*.avi;*.mov;";
                    return tmp_type;
                default: return null;
            }
        }

        /// <summary>
        /// Установка собственного изображения
        /// </summary>
        /// <returns></returns>
        public BitmapImage SetPersonalImage()
        {
            string type = "Графические файлы (*.jpeg/*.bmp/*.png/*.gif)|*.tiff;*.jpeg;*.bmp;*.png;*.gif;*.jpg";
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = type;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImagePath = dialog.FileName;
                ImageFlag = true;
                return ImageService.ImageCreaterForLabel(dialog.FileName);
            }
            else return LabelImage;
        }

        #endregion

        

        #region Get/Set

        public String LabelPath
        {
            get => _labelPath;
            set
            {
                _labelPath = value;
                NotifyPropertyChanged(nameof(LabelPath));
            }
        }
        public String Task
        {
            get => _task;
            set
            {
                _task = value;
                NotifyPropertyChanged(nameof(Task));
            }
        }
        /// <summary>
        /// Информация о пути к файлу выводимая в ToolTip
        /// </summary>
        public String LabelPathInfo
        {
            get => _labelPathInfo;
            set
            {
                _labelPathInfo = value;
                NotifyPropertyChanged(nameof(LabelPathInfo));
            }
        }
        public String ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                NotifyPropertyChanged(nameof(ImagePath));
            }
        }

        /// <summary>
        /// Значение типа файла из ComboBox
        /// </summary>
        public Int16 FileType
        {
            get => _fileType;
            set
            {
                _fileType = value;
            }
        }

        /// <summary>
        /// Флаг типа ToolTip для Path View
        /// </summary>
        public Boolean PathViewToolTipFlag
        {
            get => _pathViewToolTipFlag;
            set
            {
                _pathViewToolTipFlag = value;
                NotifyPropertyChanged(nameof(PathViewToolTipFlag));
            }
        }
        /// <summary>
        /// Флаг устанавливаемой картинки оригинальная/новая
        /// </summary>
        public Boolean ImageFlag
        {
            get => _imageFlag;
            set
            {
                _imageFlag = value;
                NotifyPropertyChanged(nameof(ImageFlag));
            }
        }
        /// <summary>
        /// Флаг разрешающий собственную настройку ярлыка
        /// </summary>
        public Boolean SettingsFlag
        {
            get => _settingsFlag;
            set
            {
                _settingsFlag = value;
                NotifyPropertyChanged(nameof(SettingsFlag));
            }
        }
        /// <summary>
        /// Флаг разрешающий создание нового ярлыка
        /// </summary>
        public Boolean CreateFlag
        {
            get => _createFlag;
            set
            {
                _createFlag = value;
                NotifyPropertyChanged(nameof(CreateFlag));
            }
        }

        public BitmapImage LabelImage
        {
            get => _image;
            set
            {
                _image = value;
                NotifyPropertyChanged(nameof(LabelImage));
            }
        }

        #endregion

       

        #region Fields

        private String _labelPath;
        private String _labelPathInfo;
        private String _task;
        private String _imagePath;
        public String InfoTask { get; private set; } = "Выбранный путь: ";
        public String UserLogin { get; private set; }

        private Boolean _imageFlag;
        private Boolean _pathViewToolTipFlag = false;
        private Boolean _settingsFlag = false;
        public Boolean CloseFlag { get; private set; } = true;
        private Boolean _createFlag = false;

        private BitmapImage _image;

        private Int16 _fileType = 0;

        #endregion

    }
}
