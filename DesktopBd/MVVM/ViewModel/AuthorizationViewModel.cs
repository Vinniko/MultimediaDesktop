using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopBd.MVVM.View;
using DesktopBd.Service;
using System.Windows;
using System.Windows.Controls;

namespace DesktopBd.MVVM.ViewModel
{
    class AuthorizationViewModel : BaseVM
    {
        #region Commands

        /// <summary>
        /// Авторизация
        /// </summary>
        public RelayCommand Authorization
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (LoginTest(Login) & PasswordTest(Password))
                    {
                        if (AuthorizationService.AcccountTest(Login, Password)) ViewManager.OpenNewWindowInShow(new DesktopView(AuthorizationService.Authorization(Login, Password)), true); 
                    }
                });
            }
        }

        /// <summary>
        /// Открытие окна регистрации
        /// </summary>
        public RelayCommand OpenRegistraitionWindow
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    ViewManager.OpenNewWindowInShowDialog(new RegistrationView());
                });
            }
        }

        /// <summary>
        /// Получение пароля
        /// </summary>
        public RelayCommand PasswordChange
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    var NewPasswordBox = obj as PasswordBox;
                    _passwordBox = obj as PasswordBox;
                    Password = NewPasswordBox.Password;
                });
            }
        }
        
        /// <summary>
        /// Установка флага "Запомнить меня"
        /// </summary>
        public RelayCommand CheckRememberUser
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    var NewPasswordBox = obj as PasswordBox;
                    _passwordBox = obj as PasswordBox;
                    String tmpLogin = String.Empty;
                    String tmpPassword = String.Empty;
                    Boolean tmpFlag = false;
                    FileService.ReadRememberUserInFile(out tmpLogin, out tmpPassword, out tmpFlag);
                    Login = tmpLogin;
                    Password = tmpPassword;
                    _passwordBox.Password = tmpPassword;
                    RememberUserFlag = tmpFlag;
                });
            }
        }

        public RelayCommand Close
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (_rememberUserFlag) RemeberUser(Login, Password);
                    else DeleteRememberUser();
                });
            }
        }

        #endregion



        #region Staff

        /// <summary>
        /// Проверка правильности введеного логина
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public Boolean LoginTest(String login)
        {
            var tmp_info = String.Empty;
            var tmp_flag = ValidationService.LoginAuthorizationTest(login, out tmp_info);
            LoginInfo = tmp_info;
            return tmp_flag;
        }
        /// <summary>
        /// Проверка правильности введенного пароля
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public Boolean PasswordTest(String password)
        {
            var tmp_info = String.Empty;
            var tmp_flag = ValidationService.PasswordTest(password, out tmp_info);
            PasswordInfo = tmp_info;
            return tmp_flag;
        }

        /// <summary>
        /// Запоминание пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        public void RemeberUser(String login, String password)
        {
            FileService.WriteRememberUserInFile(login, password);
        }
        /// <summary>
        /// Удаление фиксирвоанного пользователя
        /// </summary>
        public void DeleteRememberUser()
        {
            FileService.DeleteRememberUserInFile();
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
        public String Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyPropertyChanged(nameof(Password));
            }
        }
        /// <summary>
        /// Информация об ошибках в логине
        /// </summary>
        public String LoginInfo
        {
            get => _loginInfo;
            set
            {
                _loginInfo = value;
                NotifyPropertyChanged(nameof(LoginInfo));
            }
        }
        /// <summary>
        /// Информация об ошибках в пароле
        /// </summary>
        public String PasswordInfo
        {
            get => _passwordInfo
;
            set
            {
                _passwordInfo = value;
                NotifyPropertyChanged(nameof(PasswordInfo));
            }
        }
        
        /// <summary>
        /// Флаг показа пароля
        /// </summary>
        public Boolean PasswordShowFlag
        {
            get => _passwordShowFlag;
            set
            {
                _passwordShowFlag = value;
                NotifyPropertyChanged(nameof(PasswordShowFlag));
                _passwordBox.Password = _password;
            }
        }
        /// <summary>
        /// Флаг запоминания пользователя
        /// </summary>
        public Boolean RememberUserFlag
        {
            get => _rememberUserFlag;
            set
            {
                _rememberUserFlag = value;
                NotifyPropertyChanged(nameof(RememberUserFlag));
                if (_rememberUserFlag) RemeberUser(Login, Password);
                else DeleteRememberUser();
            }
        }

        #endregion



        #region Fields

        private String _login = String.Empty;
        private String _password = String.Empty;
        private String _loginInfo;
        private String _passwordInfo;
        
        private Boolean _passwordShowFlag = false;
        private Boolean _rememberUserFlag = false;

        private PasswordBox _passwordBox;

        #endregion
    }
}
