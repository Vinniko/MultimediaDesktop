using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopBd.MVVM.Model;
using DesktopBd.Service;
using DesktopBd.MVVM.View;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace DesktopBd.MVVM.ViewModel
{
    public sealed class LabelViewModel : BaseVM
    {
        #region Constructors

        public LabelViewModel()
        {


        }

        /// <summary>
        /// Конструктор создания обычного ярлыка
        /// </summary>
        /// <param name="labelModel"></param>
        public LabelViewModel(LabelModel labelModel, String Login,
            String font, Int32 fontSize, Boolean fontBold, Boolean fontItalic, Boolean fontUnderline, Boolean fontStrikeout, String[] fontColor)
        {
            Font = font;
            FontSize = fontSize;
            FontBold = fontBold;
            FontItalic = fontItalic;
            FontUnderline = fontUnderline;
            FontStrikeout = fontStrikeout;
            FontColor = fontColor;
            Model = labelModel;
            ContextMenuOpacity = 100;
            ToolTipText = String.Concat("Путь:" + LabelPath);
        }

        /// <summary>
        /// Конструктор создания none ярлыка
        /// </summary>
        /// <param name="labelModel"></param>
        /// <param name="createLabelViewModel"></param>
        public LabelViewModel(LabelModel labelModel, CreateLabelViewModel createLabelViewModel)
        {
            Model = labelModel;
            CreateLabelViewModel = createLabelViewModel;
        }

        #endregion



        #region Commands

        /// <summary>
        /// Нажатие на ярлык левой кнопкой мыши
        /// </summary>
        public RelayCommand Click
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (!TypeFlag)
                    {
                        ViewManager.OpenNewWindowInShow(new CreateLabelView() { DataContext = CreateLabelViewModel }, false);
                    }
                    else System.Diagnostics.Process.Start(LabelPath);
                });
            }
        }

        /// <summary>
        /// Изменение ярлыка
        /// </summary>
        public RelayCommand Edit
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    ViewManager.OpenNewWindowInShow(new ChangeLabelView() { DataContext = ChangeLabelViewModel() }, false);
                });
            }
        }

        /// <summary>
        /// Удаление ярлыка
        /// </summary>
        public RelayCommand Remove
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    var result = MessageBox.Show("Вы точно хотите удалить ярлык?", "Внимание!", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.OK);
                    if (result == System.Windows.MessageBoxResult.OK)
                    {
                        LabelService.DeleteLabel(Model);
                        FileService.DeleteFile(ImagePath);
                    }
                });
                
            }
        }

        #endregion



        #region Staf

        /// <summary>
        /// Создание Вью-модели окна изменения ярлыка
        /// </summary>
        /// <returns></returns>
        public ChangeLabelViewModel ChangeLabelViewModel()
        {
            var changeLabelViewModel = new ChangeLabelViewModel(Login, LabelPath, Task, ImagePath);
            changeLabelViewModel.LabelChangeEvent += ChangeLabelViewModel_LabelChangeEvent; 
            return changeLabelViewModel;
        }

        #endregion



        #region Event

        private void ChangeLabelViewModel_LabelChangeEvent(string labelPath, string task, string imagePath)
        {
            LabelPath = labelPath;
            Task = task;
            ImagePath = imagePath;
        }

        #endregion



        #region Get/Set

        public String Task
        {
            get => Model.Title;
            set
            {
                Model.Title = value;
                NotifyPropertyChanged(nameof(Task));
            }
        }
        public String ImagePath
        {
            get => Model.ImagePath;
            set
            {
                Model.ImagePath = value;
                NotifyPropertyChanged(nameof(ImagePath));
                NotifyPropertyChanged(nameof(BImage));
            }
        }
        public String LabelPath
        {
            get => Model.Path;
            set
            {
                Model.Path = value;
                NotifyPropertyChanged(nameof(LabelPath));
            }
        }
        /// <summary>
        /// Текст подсказки при наведении
        /// </summary>
        public String ToolTipText
        {
            get => _toolTipText;
            set
            {
                _toolTipText = value;
                NotifyPropertyChanged(nameof(ToolTipText));
            }
        }
        /// <summary>
        /// Шрифт ярлыка
        /// </summary>
        public String Font
        {
            get => _font;
            set
            {
                _font = value;
                NotifyPropertyChanged(nameof(Font));
            }
        }

        public String[] FontColor
        {
            get => _fontColor;
            set
            {
                _fontColor = value;
                NotifyPropertyChanged(nameof(FontColor));
            }
        }
        public System.Windows.Media.Brush Color
        {
            get => new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(Byte.Parse(FontColor[0]), Byte.Parse(FontColor[1]), Byte.Parse(FontColor[2]), Byte.Parse(FontColor[3])));
            set
            {
                NotifyPropertyChanged(nameof(FontColor));
            }
        }

        public Int32 FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                NotifyPropertyChanged(nameof(FontSize));
            }
        }

        /// <summary>
        /// Флаг отвечающий за тип ярлыка
        /// </summary>
        public Boolean TypeFlag
        {
            get => Model.TypeFlag;
            set
            {
                Model.TypeFlag = value;
                NotifyPropertyChanged(nameof(TypeFlag));
            }
        }
        public Boolean ImageFlag
        {
            get => Model.ImageFlag;
            set
            {
                Model.ImageFlag = value;
                NotifyPropertyChanged(nameof(ImageFlag));
            }
        }
        public Boolean FontBold
        {
            get => _fontBold;
            set
            {
                _fontBold = value;
                NotifyPropertyChanged(nameof(FontBold));
            }
        }
        public System.Windows.FontWeight Bold
        {
            get => _fontBold ? System.Windows.FontWeights.ExtraBold : System.Windows.FontWeights.Normal;
            set
            {
                NotifyPropertyChanged(nameof(FontBold));
            }
        }
        public Boolean FontItalic
        {
            get => _fontItalic;
            set
            {
                _fontItalic = value;
                NotifyPropertyChanged(nameof(FontItalic));
            }
        }
        public System.Windows.FontStyle Italic
        {
            get => _fontItalic ? System.Windows.FontStyles.Oblique : System.Windows.FontStyles.Normal;
            set
            {
                NotifyPropertyChanged(nameof(FontItalic));
            }
        }
        public Boolean FontUnderline
        {
            get => _fontUnderline;
            set
            {
                _fontUnderline = value;
                NotifyPropertyChanged(nameof(FontUnderline));
            }
        }
        public System.Windows.TextDecorationCollection Underline
        {
            get => (_fontUnderline ? System.Windows.TextDecorations.Underline : null);
            set
            {
                NotifyPropertyChanged(nameof(FontUnderline));
            }

        }
        public Boolean FontStrikeout
        {
            get => _fontStrikeout;
            set
            {
                _fontStrikeout = value;
                NotifyPropertyChanged(nameof(FontStrikeout));
            }
        }

        public BitmapImage BImage
        {
            get => ImageService.LoadLabelImage(ImagePath);
            set
            {
                NotifyPropertyChanged(nameof(ImagePath));
            }
        }

        public Double ContextMenuOpacity
        {
            get => _contextMenuOpacity;
            set
            {
                _contextMenuOpacity = value;
                NotifyPropertyChanged(nameof(ContextMenuOpacity));
            }
        }

        #endregion



        #region Fields

        public LabelModel Model { get; private set; }

        public CreateLabelViewModel CreateLabelViewModel { get; private set; }

        private String _toolTipText = "Добавить ярлык";
        private String _font;
        public String Login { get; private set; }

        private String[] _fontColor;

        private Boolean _fontBold;
        private Boolean _fontItalic;
        private Boolean _fontUnderline;
        private Boolean _fontStrikeout;

        private Int32 _fontSize;

        private Double _contextMenuOpacity = 0;

        #endregion
    }
}
