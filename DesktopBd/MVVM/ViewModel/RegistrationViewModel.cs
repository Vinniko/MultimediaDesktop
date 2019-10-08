using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DesktopBd.MVVM.View;
using DesktopBd.Service;
using System.Windows.Controls;
using System.Windows.Media;

namespace DesktopBd.MVVM.ViewModel
{
    class RegistrationViewModel : BaseVM
    {
        #region Commands

        /// <summary>
        /// Регистрация пользователя в системе
        /// </summary>
        public RelayCommand Registration
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (LoginTest(Login) && PasswordTest(FirstPassword, SecondPassword))
                    {
                        ViewManager.OpenNewWindowInShow(new DesktopView(RegistrationService.CreateNewAccount(Login, FirstPassword)), true);
                    }
                    else SetColor();
                });
            }
        }

        /// <summary>
        /// Изменение строки первого пароля
        /// </summary>
        public RelayCommand FirstPasswordChange
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    var NewPasswordBox = obj as PasswordBox;
                    FirstPassword = NewPasswordBox.Password;
                });
            }
        }

        /// <summary>
        /// Изменение строки второго пароля
        /// </summary>
        public RelayCommand SecondPasswordChange
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    var NewPasswordBox = obj as PasswordBox;
                    SecondPassword = NewPasswordBox.Password;
                });
            }
        }

        /// <summary>
        /// Сохранение аккаунтов
        /// </summary>
        public RelayCommand SaveFiles
        {
            get
            {
                return new RelayCommand(obj =>
                {
                   FileService.WriteAccountsInFile(); // TODO Перевести на базы данных
                });
            }
        }

        /// <summary>
        /// Выход из окна регистрации на окно авторизации
        /// </summary>
        public RelayCommand Exit
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    ViewManager.OpenNewWindowInShowDialog(new AuthorizationView());
                });
            }
        }

        #endregion



        #region Staff

        /// <summary>
        /// Проверка логина на валидность
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
        /// Проверка паролей на валидность
        /// </summary>
        /// <param name="firstPassword"></param>
        /// <param name="secondPassword"></param>
        /// <returns></returns>
        public Boolean PasswordTest(String firstPassword, String secondPassword)
        {
            var tmp_info = String.Empty;
            var tmp_flag = ValidationService.PasswordTest(firstPassword, secondPassword, out tmp_info);
            PasswordInfo = tmp_info;
            return tmp_flag;
        }

        /// <summary>
        /// Установка флага цвета на красный
        /// </summary>
        public void SetColor()
        {
            ColorFlag = true;
        }

        #endregion



        #region Get/Set

        public String Login
        {
            get => _login;
            set
            {
                _login = value;
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
        public String PasswordInfo
        {
            get => _pusswordInfo;
            set
            {
                _pusswordInfo = value;
                NotifyPropertyChanged(nameof(PasswordInfo));
            }
        }

        public Boolean ColorFlag
        {
            get => _colorFlag;
            set
            {
                _colorFlag = value;
                NotifyPropertyChanged(nameof(ColorFlag));
            }
        }

        #endregion



        #region Fields

        private String _login = String.Empty;
        public String FirstPassword { get; private set; } = String.Empty;
        public String SecondPassword { get; private set; } = String.Empty;

        private String _loginInfo;
        private String _pusswordInfo;

        private Boolean _colorFlag = false;

        #endregion
    }
}
