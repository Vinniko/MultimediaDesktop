using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopBd.MVVM.Model;
using DesktopBd.Service;
using DesktopBd.Repositories;
using DesktopBd.MVVM.View;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace DesktopBd.MVVM.ViewModel
{
    public class SettingViewModel : BaseVM
    {
        #region Constructors

        public SettingViewModel(UserModel user, Int32 desktopIndex)
        {
            User = user;
            SetValues(user, desktopIndex);
        }

        #endregion



        #region Commands

        /// <summary>
        /// Принятие настроек без закрытия окна
        /// </summary>
        public RelayCommand ApplySettings
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (LoginChangeFlag)
                    {
                        if (LoginTest(Login))
                        {
                            ApplySettingsFunc();
                            SaveFlag = true;
                        }
                    }
                    else
                    {
                        ApplySettingsFunc();
                        SaveFlag = true;
                    }
                });
            }
        }

        /// <summary>
        /// Сохранение настроек с закрытием окна
        /// </summary>
        public RelayCommand SaveSettings
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (!SaveFlag)
                    {
                        if (LoginChangeFlag)
                        {
                            if (LoginTest(Login))
                            {
                                ApplySettingsFunc();
                                ChangeFlag = false;
                                CloseFlag = false;
                                SaveFlag = false;
                                ViewManager.CloseView();
                            }
                        }
                        else
                        {
                            ApplySettingsFunc();
                            ChangeFlag = false;
                            CloseFlag = false;
                            SaveFlag = false;
                            ViewManager.CloseView();
                        }
                    }
                    else
                    {
                        ChangeFlag = false;
                        CloseFlag = false;
                        ViewManager.CloseView();
                    }
                });
            }
        }

        /// <summary>
        /// Отмена примененых натсроек с закрытием окна
        /// </summary>
        public RelayCommand CancelSettings
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    CancelSettingsFunc();
                    CloseFlag = false;
                    ChangeFlag = false;
                    ViewManager.CloseView();
                });
            }
        }

        /// <summary>
        /// Изменение строки логина
        /// </summary>
        public RelayCommand ChangeLogin
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    ChangeFlag = true;
                    LoginChangeFlag = true;
                    SaveFlag = false;
                });
            }
        }

        /// <summary>
        /// Открытие окна изменения пароля
        /// </summary>
        public RelayCommand OpenChangePasswordWindow
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    ViewManager.OpenNewWindowInShow(new PasswordChangeView { DataContext = CreatePasswordChangeViewModel() }, false);
                });
            }
        }

        /// <summary>
        /// Открытие диалогового окна настройки шрифтов
        /// </summary>
        public RelayCommand OpenFontDialogWindow
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    var fontDialog = new FontDialog();
                    fontDialog.ShowColor = true;
                    fontDialog.AllowScriptChange = false;
                    fontDialog.MaxSize = 14;
                    fontDialog.Font = new System.Drawing.Font(User.Font, FontSize);
                    fontDialog.Color = System.Drawing.Color.FromArgb(int.Parse(FontColor[0]), int.Parse(FontColor[1]), int.Parse(FontColor[2]), int.Parse(FontColor[3]));
                    if (fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        ChangeFlag = true;
                        FontChangeFlag = true;
                        Font = fontDialog.Font.Name;
                        FontSize = (int)fontDialog.Font.Size;
                        FontBold = fontDialog.Font.Bold;
                        FontItalic = fontDialog.Font.Italic;
                        FontUnderline = fontDialog.Font.Underline;
                        FontStrikeout = fontDialog.Font.Strikeout;
                        var tmpColor = new String[4] { fontDialog.Color.A.ToString(), fontDialog.Color.R.ToString(), fontDialog.Color.G.ToString(), fontDialog.Color.B.ToString() };
                        FontColor = tmpColor;
                    }  
                });
            }
        }

        /// <summary>
        /// Изменение индекса редактируемого рабочего стола
        /// </summary>
        public RelayCommand DesktopIndexChange
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    ChangeDesktopImageOnView();
                });
            }
        }

        /// <summary>
        /// Добавление картинки для выбранного рабочего стола
        /// </summary>
        public RelayCommand AddDesktopImage
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    var dialog = new OpenFileDialog();
                    dialog.Filter = "Графические файлы (*.jpeg/*.bmp/*.png/*.gif)|*.tiff;*.jpeg;*.bmp;*.png;*.gif;*.jpg";
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        DesktopImagePath = FileService.SaveImage(ImageService.ImageFile(dialog.FileName), User.ID, dialog.SafeFileName);
                        DesktopImagePathes[DesktopIndex] = DesktopImagePath;
                        DesktopChangeFlag = true;
                        ChangeFlag = true;
                    }
                });
            }
        }

        /// <summary>
        /// Сброс настроек до заводских
        /// </summary>
        public RelayCommand SetDefaultSettings
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    ChangeFlag = true;
                    FontChangeFlag = true;
                    DesktopChangeFlag = true;
                    for (int i = 0; i < DesktopImagePathes.Count(); i++)
                    {
                        DesktopImagePathes[i] = DefaultDesktopImagePath;
                    }
                    Font = DefaultFont;
                    FontColor = DefaultColor;
                    FontSize = DefaultFontSize;
                    FontBold = DefaultFontBold;
                    FontItalic = DefaultFontItalic;
                    FontUnderline = DefaultFontUnderline;
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
                    if (!ChangeFlag)
                    {
                        if (CloseFlag) ViewManager.CloseView();
                    }
                    else
                    {
                        CancelSettingsFunc();
                        if (CloseFlag) ViewManager.CloseView();
                    }
                });
            }
        }

        #endregion



        #region Main Logic

        /// <summary>
        /// Применить настройки
        /// </summary>
        public void ApplySettingsFunc()
        {
            if (DesktopChangeFlag)
            {
                for (int i = 0; i < DesktopImagePathes.Count; i++)
                {
                    DesktopService.GetDesktop((short)i).WallpaperImage = DesktopImagePathes[i];
                }
            }
            SettingsEvent(Login, OldLogin, LoginChangeFlag, Password, PasswordChangeFlag, DesktopImagePath, Font, FontSize, FontColor, FontBold, FontItalic, FontUnderline, FontChangeFlag);
        }

        /// <summary>
        /// Отменить настройки
        /// </summary>
        public void CancelSettingsFunc()
        {
            if (ChangeFlag)
            {
                if (DesktopChangeFlag)
                {
                    for (int i = 0; i < DesktopImagePathes.Count; i++)
                    {
                        DesktopService.GetDesktop((short)i).WallpaperImage = OldDesktopImagePathes[i];
                    }
                }
                SettingsCencelEvent(OldLogin, Login, LoginChangeFlag, OldPassword, PasswordChangeFlag, OldDesktopImagePath, OldFont, OldFontSize, OldFontColor, OldFontBold, OldFontItalic, OldFontUnderline, FontChangeFlag);
            }
        }

        #endregion



        #region Staff

        /// <summary>
        /// Создание копии старых данных
        /// </summary>
        /// <param name="user"></param>
        public void SetValues(UserModel user, Int32 desktopIndex)
        {
            OldLogin = User.Login;
            Login = user.Login;

            OldPassword = User.Password;
            Password = User.Password;

            OldFont = User.Font;
            Font = User.Font;

            OldFontSize = User.FontSize;
            FontSize = User.FontSize;

            OldFontBold = User.FontBold;
            FontBold = User.FontBold;

            OldFontItalic = User.FontItalic;
            FontItalic = User.FontItalic;

            OldFontUnderline = User.FontUnderline;
            FontUnderline = User.FontUnderline;

            OldFontStrikeout = User.FontStrikeout;
            FontStrikeout = User.FontStrikeout;

            OldFontColor = User.FontColor;
            FontColor = User.FontColor;

            DesktopIndexes = SetDesktops();
            DesktopIndex = desktopIndex;
            DesktopImagePath = DesktopImagePathes[DesktopIndex];
        }

        /// <summary>
        /// Проверка правильности введеного логина
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public Boolean LoginTest(String login)
        {
            var tmp_info = String.Empty;
            var tmp_flag = ValidationService.LoginRegistrationTest(login, out tmp_info);
            LoginInfo = tmp_info;
            return tmp_flag;
        }

        /// <summary>
        /// Создание вью модели окна изменения пароля
        /// </summary>
        /// <returns></returns>
        public PasswordChangeViewModel CreatePasswordChangeViewModel()
        {
            var passwordChangeViewModel = new PasswordChangeViewModel(User);
            passwordChangeViewModel.PasswordChangeComplete += PasswordChangeViewModel_PasswordChangeComplete;
            return passwordChangeViewModel;
        }

        /// <summary>
        /// Создание копии оригинальных рабочих столов
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Int32> SetDesktops()
        {
            var tmpCollection = new ObservableCollection<Int32>();
            var tmpDesktopCollection = new ObservableCollection<String>();
            var tmpArray = new String[User.DesktopCount];
            for (Int32 i = 0; i < User.DesktopCount; i++)
            {
                tmpCollection.Add(i + 1);
                tmpDesktopCollection.Add(DesktopService.GetDesktop((short)i).WallpaperImage);
                tmpArray[i] = DesktopService.GetDesktop((short)i).WallpaperImage;
            }
            DesktopImagePathes = tmpDesktopCollection;
            OldDesktopImagePathes = tmpArray;
            return tmpCollection;
        }

        /// <summary>
        /// Изменение картинки рабочего стола на текущем окне
        /// </summary>
        public void ChangeDesktopImageOnView()
        {
            DesktopImagePath = DesktopImagePathes[DesktopIndex];
        }

        #endregion



        #region Event

        public delegate void SettingsCompleatEventHandler(String login, String oldLogin, Boolean loginFlag,
            String password, Boolean passwordFlag,
            String desktopImagePath,
            String font, Int32 fontSize, String[] fontColor, Boolean fontBold, Boolean fontItalic, Boolean fontUnderline, Boolean fontFlag);
        public event SettingsCompleatEventHandler SettingsEvent;

        public delegate void SettingsCencelEventHandler(String login, String newLogin, Boolean loginFlag,
            String password, Boolean passwordFlag,
            String desktopImagePath,
            String font, Int32 fontSize, String[] fontColor, Boolean fontBold, Boolean fontItalic, Boolean fontUnderline, Boolean fontFlag);
        public event SettingsCencelEventHandler SettingsCencelEvent;

        private void PasswordChangeViewModel_PasswordChangeComplete(String password)
        {
            ChangeFlag = true;
            PasswordChangeFlag = true;
            SaveFlag = false;
            Password = password;
        }

        #endregion




        #region Get/Set

        public String Login
        {
            get => _login;
            set
            {
                _login = value;
                NotifyPropertyChanged(nameof(Login));
            }
        }
        public String LoginInfo
        {
            get => _loginInfo;
            set
            {
                _loginInfo = value;
                NotifyPropertyChanged(nameof(LoginInfo));
            }
        }
        public String Password
        {
            get => _password;
            set
            {
                _password = value;
            }
        }
        public String DesktopImagePath
        {
            get => _desktopImagePath;
            set
            {
                _desktopImagePath = value;
                NotifyPropertyChanged(nameof(DesktopImagePath));
                NotifyPropertyChanged(nameof(BImage));
            }
        }
        /// <summary>
        /// Шрифт пользователя
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

        public BitmapImage BImage
        {
            get => ImageService.ImageCreaterForDesktop(DesktopImagePath);
        }

        public String[] FontColor
        {
            get => _fontColor;
            set
            {
                _fontColor = value;
                NotifyPropertyChanged(nameof(FontColor));
                NotifyPropertyChanged(nameof(Color));
            }
        }
        public System.Windows.Media.Brush Color
        {
            get => new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(Byte.Parse(FontColor[0]), Byte.Parse(FontColor[1]), Byte.Parse(FontColor[2]), Byte.Parse(FontColor[3])));
        }

        /// <summary>
        /// Коллекция индексов рабочих столов
        /// </summary>
        public ObservableCollection<Int32> DesktopIndexes
        {
            get => _desktopIndexes;
            set
            {
                _desktopIndexes = value;
                NotifyPropertyChanged(nameof(DesktopIndexes));
            }
        }

        /// <summary>
        /// Коллекция путей картинок рабочих столов
        /// </summary>
        public ObservableCollection<String> DesktopImagePathes
        {
            get => _desktopImagePathes;
            set
            {
                _desktopImagePathes = value;
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
        /// Индекс текущего рабочего стола на окне
        /// </summary>
        public Int32 DesktopIndex
        {
            get => _desktopIndex;
            set
            {
                _desktopIndex = value;
            }
        }

        /// <summary>
        /// Флаг отвечающий за то Были ли внесены изменения или нет
        /// </summary>
        public Boolean ChangeFlag
        {
            get => _changeFlag;
            set
            {
                _changeFlag = value;
                NotifyPropertyChanged(nameof(ChangeFlag));

            }
        }
        public Boolean FontBold
        {
            get => _fontBold;
            set
            {
                _fontBold = value;
                NotifyPropertyChanged(nameof(FontBold));
                NotifyPropertyChanged(nameof(Bold));
            }
        }
        public System.Windows.FontWeight Bold
        {
            get => _fontBold ? System.Windows.FontWeights.ExtraBold : System.Windows.FontWeights.Normal;
        }
        public Boolean FontItalic
        {
            get => _fontItalic;
            set
            {
                _fontItalic = value;
                NotifyPropertyChanged(nameof(FontItalic));
                NotifyPropertyChanged(nameof(Italic));
            }
        }
        public System.Windows.FontStyle Italic
        {
            get => _fontItalic ? System.Windows.FontStyles.Oblique : System.Windows.FontStyles.Normal;
        }
        public Boolean FontUnderline
        {
            get => _fontUnderline;
            set
            {
                _fontUnderline = value;
                NotifyPropertyChanged(nameof(FontUnderline));
                NotifyPropertyChanged(nameof(Underline));
            }
        }
        public System.Windows.TextDecorationCollection Underline
        {
            get => (_fontUnderline ? System.Windows.TextDecorations.Underline : null);

        }
        public Boolean FontStrikeout
        {
            get => _fontStrikeout;
            set
            {
                _fontStrikeout = value;
                NotifyPropertyChanged(nameof(FontStrikeout));
                NotifyPropertyChanged(nameof(Strikeout));
            }
        }
        public System.Windows.TextDecorationCollection Strikeout
        {
            get => (_fontStrikeout ? System.Windows.TextDecorations.Strikethrough : System.Windows.TextDecorations.Baseline);

        }

        public UserModel User
        {
            get => _user;
            set
            {
                _user = value;
            }
        }

        #endregion



        #region Fields

        public String OldLogin { get; private set; }
        public String OldPassword { get; private set; }
        public String OldDesktopImagePath { get; private set; }
        public String OldFont { get; private set; }
        public String OldFontType { get; private set; }
        public String OldFontEdit { get; private set; }

        public const String DefaultDesktopImagePath = "";
        public const String DefaultFont = "Arial";
        public String[] DefaultColor { get; } = new String[4] { "255", "0", "0", "0" };
        public const Int32 DefaultFontSize = 12;
        public const Boolean DefaultFontBold = false;
        public const Boolean DefaultFontItalic = false;
        public const Boolean DefaultFontUnderline = false;

        public String[] OldFontColor { get;  private set; }
        public String[] OldDesktopImagePathes { get; private set; }

        private ObservableCollection<Int32> _desktopIndexes;

        private ObservableCollection<String> _desktopImagePathes;

        public Int32 OldFontSize { get; private set; }
        private Int32 _desktopIndex;

        private String _login;
        private String _loginInfo;
        private String _password;
        private String _desktopImagePath;
        private String _font;

        private String[] _fontColor;

        private Boolean _fontBold;
        private Boolean _fontItalic;
        private Boolean _fontUnderline;
        private Boolean _fontStrikeout;

        private Int32 _fontSize;

        private UserModel _user;

        public Boolean OldFontBold { get; private set; }
        public Boolean OldFontItalic { get; private set; }
        public Boolean OldFontUnderline { get; private set; }
        public Boolean OldFontStrikeout { get; private set; }
        public Boolean _changeFlag = false;
        /// <summary>
        /// Флаг отвечающий за способ закрытия окна
        /// </summary>
        public Boolean CloseFlag { get; private set; } = true;
        /// <summary>
        /// Флаг отвечающий за то: Были ли уже применены настройки или нет
        /// </summary>
        public Boolean SaveFlag { get; private set; } = false;
        /// <summary>
        /// Флаг отвечающий за то: Был ли измененен логин или нет
        /// </summary>
        public Boolean LoginChangeFlag { get; private set; } = false;
        /// <summary>
        /// Флаг отвечающий за то: Был ли изменен пароль или нет
        /// </summary>
        public Boolean PasswordChangeFlag { get; private set; } = false;
        /// <summary>
        /// Флаг отвечающий за то: Был ли изменен шрифт или нет.
        /// </summary>
        public Boolean FontChangeFlag { get; private set; } = false;

        public Boolean DesktopChangeFlag { get; private set; } = false;

        #endregion
    }
}
