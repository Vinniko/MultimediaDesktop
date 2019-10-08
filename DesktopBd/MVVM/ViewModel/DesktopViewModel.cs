using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopBd.MVVM.Model;
using DesktopBd.Service;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using DesktopBd.MVVM.View;
using DesktopBd.Repositories;
using System.Windows.Media.Imaging;

namespace DesktopBd.MVVM.ViewModel
{
    public class DesktopViewModel : BaseVM
    {
        #region Constructor

        public DesktopViewModel(UserModel user)
        {
            User = user;
            LabelService.SettingsEvent += LabelService_SettingsEvent1; 
        }

        #endregion



        #region Commands

        /// <summary>
        /// Перемещение окна
        /// </summary>
        public RelayCommand MoveWindow
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    ViewManager.GetView().DragMove();

                });
            }
        }

        /// <summary>
        /// Сворачивание окна
        /// </summary>
        public RelayCommand MinimizedWindow
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    ViewManager.GetView().WindowState = System.Windows.WindowState.Minimized;
                });
            }
        }

        /// <summary>
        /// Начало работы с рабочим столом
        /// </summary>
        public RelayCommand StartWork
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (FileService.CheckFileExist(User.ID))
                    {
                        FileService.ReadInFile(User);
                        LoadLabels();
                        CreateNoneLabel();
                        LoadDesktopImage();
                    }
                    else
                    {
                        DesktopService.CreateDesktop();
                        User.DesktopCount++;
                        CreateNoneLabel();
                        LoadDesktopImage();
                    }
                });
            }
        }

        /// <summary>
        /// Открытие окна создания ярлыка
        /// </summary>
        public RelayCommand CreateLabelWindowOpen
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    ViewManager.OpenNewWindowInShow(new CreateLabelView() { DataContext = CreateViewModel() }, false);
                });
            }
        }

        /// <summary>
        /// Открытие окна персоналных настроек
        /// </summary>
        public RelayCommand SettingsWindowOpen
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    ViewManager.OpenNewWindowInShow(new SettingView() { DataContext = CreateSettingsViewModel() }, false);
                });
            }
        }

        /// <summary>
        /// Переход на предыдущий рабочий стол
        /// </summary>
        public RelayCommand GoLeft
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    ChangeButtons(--DesktopIndex, false);
                });
            }
        }

        /// <summary>
        /// Переход на следующий рабочий стол
        /// </summary>
        public RelayCommand GoRight
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    ChangeButtons(++DesktopIndex, true);
                });
            }
        }

        /// <summary>
        /// Открытие файла помощи
        /// </summary>
        public RelayCommand OpenHelp
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    try
                    {
                        System.Diagnostics.Process.Start(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"HELP.txt"));

                    }
                    catch
                    {
                        MessageBox.Show("Неудалось открыть файл или программу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }

        /// <summary>
        /// Выход из аккаунта
        /// </summary>
        public RelayCommand Exit
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    CloseFlag = false;
                    ViewManager.OpenNewWindowInShow(new AuthorizationView(), true);
                });
            }
        }

        /// <summary>
        /// Удаление аккаунта
        /// </summary>
        public RelayCommand AccountDelete
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    var result = MessageBox.Show("Вы точно хотите удалить аккаунт?", "Внимание!",  MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.OK);
                    if (result == System.Windows.MessageBoxResult.OK)
                    {
                        CloseFlag = false;
                        AccountRepositorie.GetAccounts().Remove(User.Login);
                        FileService.DeleteSettingFile(User.ID);
                        FileService.WriteAccountsInFile();
                        DeleteFlag = true;
                        ViewManager.OpenNewWindowInShow(new AuthorizationView(), true);
                    }
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
                    if (CloseFlag) ViewManager.CloseView();
                });
            }
        }

        /// <summary>
        /// После закрытия окна
        /// </summary>
        public RelayCommand ClosedWindow
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if(!DeleteFlag) FileService.WriteLabelsToFile(User);
                    ClearAll();
                });
            }
        }

        #endregion



        #region Main Logic

        /// <summary>
        /// Загрузка ярлыков на текущий рабочий стол
        /// </summary>
        public void LoadLabels()
        {
            Labels.Clear();
            var tmpList = LabelService.GetLabelsRange(DesktopIndex, DesktopService.GetDesktop(DesktopIndex).LabelCount);
            foreach (LabelModel labelModel in tmpList)
            {
                Labels.Add(new LabelViewModel(labelModel, User.Login, User.Font, User.FontSize, User.FontBold, User.FontItalic, User.FontUnderline, User.FontStrikeout, User.FontColor));
            }
        }

        /// <summary>
        /// Загрузка нулевого ярлыка на текущий рабочий стол
        /// </summary>
        public void LoadNoneLabel()
        {
            Labels.Add(new LabelViewModel(LabelService.GetLabel(LabelService.GetLabelsCount() - 1), CreateViewModel()));
        }

        /// <summary>
        /// Создание нового ярлыка
        /// </summary>
        /// <param name="labelPath"></param>
        /// <param name="task"></param>
        /// <param name="imagePath"></param>
        public void CreateLabel(String labelPath, String task, String imagePath)
        {
            LabelService.CreateLabel(task, imagePath, labelPath, true, LabelService.GetLabelsCount() - 1);
            Labels[DesktopService.GetDesktop(DesktopIndex).LabelCount] = new LabelViewModel(LabelService.GetLabel(LabelService.GetLabelsCount() - 1), User.Login,
                User.Font, User.FontSize, User.FontBold, User.FontItalic, User.FontUnderline, User.FontStrikeout, User.FontColor);
            DesktopService.GetDesktop(DesktopIndex).LabelCount++;
        }

        /// <summary>
        /// Создание нулевого ярлыка
        /// </summary>
        public void CreateNoneLabel()
        {
            if (LabelService.LabelCountTest(DesktopService.GetDesktop(DesktopIndex).LabelCount))
            {
                LabelService.CreateLabel(false);
                Labels.Add(new LabelViewModel(LabelService.GetLabel(LabelService.GetLabelsCount() - 1), CreateViewModel()));
            }
            else
            {
                if (DesktopIndex == User.DesktopCount - 1)
                {
                    DesktopService.CreateDesktop();
                    User.DesktopCount++;
                }
                DesktopIndex++;
                LoadLabels();
                CreateNoneLabel();
                LeftButtonFlag = true;
            }
        }

        #endregion



        #region Staff

        /// <summary>
        /// Создание Вью-модели окна создания ярлыка
        /// </summary>
        /// <returns></returns>
        public CreateLabelViewModel CreateViewModel()
        {
            var createLabelViewModel = new CreateLabelViewModel(User.Login);
            createLabelViewModel.LabelCreateEvent += CreateLabelViewModel_LabelCreateEvent;
            return createLabelViewModel;
        }

        /// <summary>
        /// Создание Вью-модели окна настроек
        /// </summary>
        /// <returns></returns>
        public SettingViewModel CreateSettingsViewModel()
        {
            var settingsViewModel = new SettingViewModel(User, DesktopIndex);
            settingsViewModel.SettingsEvent += SettingsViewModel_SettingsEvent1; ; ;
            settingsViewModel.SettingsCencelEvent += SettingsViewModel_SettingsCencelEvent; ; ;
            return settingsViewModel;
        }

        /// <summary>
        /// Изменение кнопок при переключении
        /// </summary>
        /// <param name="index"></param>
        /// <param name="flag"></param>
        public void ChangeButtons(Int16 index, Boolean flag)
        {
            if (flag)
            {
                if (index == User.DesktopCount - 1)
                {
                    RightButtonFlag = false;
                    LeftButtonFlag = true;
                }
                else LeftButtonFlag = true;
                if (DesktopService.GetDesktop(DesktopIndex).LabelCount == 0)
                {
                    Labels.Clear();
                    LoadNoneLabel();
                    LoadDesktopImage();
                }
                else
                {
                    LoadLabels();
                    LoadNoneLabel();
                    LoadDesktopImage();
                }
            }
            else
            {
                if (index == 0)
                {
                    LeftButtonFlag = false;
                    RightButtonFlag = true;
                }
                else RightButtonFlag = true;
                LoadLabels();
                LoadDesktopImage();
            }
        }

        /// <summary>
        /// Загрузка картинки рабочего стола
        /// </summary>
        public void LoadDesktopImage()
        {
            NotifyPropertyChanged(nameof(DesktopImagePath));
            NotifyPropertyChanged(nameof(BImage));
        }

        /// <summary>
        /// Очистка всех репозиториев
        /// </summary>
        public void ClearAll()
        {
            LabelService.LabelRepositori.Labels.Clear();
            DesktopService.DesktopRepositori.Desktops.Clear();
        }

        #endregion



        #region Event

        private void CreateLabelViewModel_LabelCreateEvent(String labelPath, String task, String imagePath)
        {
            if (DesktopIndex != User.DesktopCount - 1)
            {
                DesktopIndex = User.DesktopCount;
                DesktopIndex--;
                RightButtonFlag = false;
                LeftButtonFlag = true;
                LoadLabels();
                LoadNoneLabel();
            }
            LoadDesktopImage();
            CreateLabel(labelPath, task, imagePath);
            CreateNoneLabel();
        }

        private void SettingsViewModel_SettingsCencelEvent(string oldLogin, string newLogin, bool loginFlag,
            string password, bool passwordFlag,
            string desktopImagePath,
            string font, int fontSize, string[] fontColor, bool fontBold, bool fontItalic, bool fontUnderline, bool fontFlag)
        {
            if (loginFlag)
            {
                Login = oldLogin;
                AccountRepositorie.AddAccount(User);
                AccountRepositorie.GetAccounts().Remove(newLogin);
                FileService.DeleteSettingFile(newLogin);
            }
            if (passwordFlag)
            {
                User.Password = password;
            }
            if (fontFlag)
            {
                User.Font = font;
            }
            if (fontFlag)
            {
                User.Font = font;
                User.FontSize = fontSize;
                User.FontBold = fontBold;
                User.FontColor = fontColor;
                User.FontItalic = fontItalic;
                User.FontUnderline = fontUnderline;
            }
            FileService.WriteAccountsInFile();
            AccountRepositorie.GetAccounts().Clear();
            FileService.ReadAccountsInFile();
            FileService.WriteLabelsToFile(User);
            LoadDesktopImage();
            LoadLabels();
            LoadNoneLabel();
        }

        private void SettingsViewModel_SettingsEvent1(string login, string oldLogin, bool loginFlag,
            string password, bool passwordFlag,
            string desktopImagePath,
            string font, int fontSize, string[] fontColor, bool fontBold, bool fontItalic, bool fontUnderline, bool fontFlag)
        {
            if (loginFlag)
            {
                Login = login;
                AccountRepositorie.AddAccount(User);
                AccountRepositorie.GetAccounts().Remove(oldLogin);
                FileService.DeleteSettingFile(oldLogin);
            }
            if (passwordFlag)
            {
                User.Password = password;
            }
            if (fontFlag)
            {
                User.Font = font;
                User.FontSize = fontSize;
                User.FontBold = fontBold;
                User.FontColor = fontColor;
                User.FontItalic = fontItalic;
                User.FontUnderline = fontUnderline;
            }
            FileService.WriteAccountsInFile();
            AccountRepositorie.GetAccounts().Clear();
            FileService.ReadAccountsInFile();
            FileService.WriteLabelsToFile(User);
            LoadDesktopImage();
            LoadLabels();
            LoadNoneLabel();
        }
        
        private void LabelService_SettingsEvent1()
        {
            DesktopService.GetDesktop((short)(User.DesktopCount - 1)).LabelCount--;
            if (DesktopService.DesktopRepositori.GetDesktop((short)(User.DesktopCount - 1)).LabelCount > 0)
            {
                if (DesktopIndex == User.DesktopCount - 1)
                {
                    LoadLabels();
                    LoadNoneLabel();
                }
                else LoadLabels();
            }
            else if (DesktopService.DesktopRepositori.GetDesktop((short)(User.DesktopCount - 1)).LabelCount == 0)
            {
                if (DesktopIndex == User.DesktopCount - 1)
                {
                    LoadLabels();
                    LoadNoneLabel();
                }
                else LoadLabels();
            }
            else
            {
                DesktopService.DeleteLastDesktop();
                User.DesktopCount--;
                DesktopService.GetDesktop((short)(User.DesktopCount - 1)).LabelCount--;
                if (DesktopIndex > User.DesktopCount - 1)
                {
                    DesktopIndex--;
                    RightButtonFlag = false;
                    LoadLabels();
                    LoadNoneLabel();
                }
                else if(DesktopIndex == User.DesktopCount - 1)
                {
                    RightButtonFlag = false;
                    LoadLabels();
                    LoadNoneLabel();
                }
                else LoadLabels();
            }
        }

        #endregion



        #region Get/Set

        /// <summary>
        /// Флаг твечающий за отображение правой кнопки
        /// </summary>
        public Boolean RightButtonFlag
        {
            get => _rightButtonFlag;
            set
            {
                _rightButtonFlag = value;
                NotifyPropertyChanged(nameof(RightButtonFlag));
            }
        }
        /// <summary>
        /// Флаг твечающий за отображение плевой кнопки
        /// </summary>
        public Boolean LeftButtonFlag
        {
            get => _leftButtonFlag;
            set
            {
                _leftButtonFlag = value;
                NotifyPropertyChanged(nameof(LeftButtonFlag));
            }
        }

        public String Login
        {
            get => User.Login;
            set
            {
                User.Login = value;
                NotifyPropertyChanged(nameof(Login));
            }
        }
        public String DesktopImagePath
        {
            get => DesktopService.GetDesktop(DesktopIndex).WallpaperImage;
            set
            {
                NotifyPropertyChanged(nameof(DesktopImagePath));
                NotifyPropertyChanged(nameof(BImage));
            }
        }

        public BitmapImage BImage
        {
            get => ImageService.ImageCreaterForDesktop(DesktopImagePath);
        }

        #endregion



        #region Fields

        public UserModel User { get; private set; }

        public ObservableCollection<LabelViewModel> Labels { get; private set; } = new ObservableCollection<LabelViewModel>();

        private Boolean _rightButtonFlag = false;
        private Boolean _leftButtonFlag = false;
        /// <summary>
        /// Флаг отвечающий за тип закрытия окна
        /// </summary>
        public Boolean CloseFlag { get; private set; } = true;
        /// <summary>
        /// Флаг отвечающий за то был ли удален аккаунт или нет
        /// </summary>
        public Boolean DeleteFlag { get; private set; } = false;

        /// <summary>
        /// Индекс текущего рабочего стола
        /// </summary>
        public Int16 DesktopIndex { get; private set; } = 0;

        #endregion
    }
}
