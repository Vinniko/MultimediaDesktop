using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopBd.MVVM.Model;
using DesktopBd.Service;
using System.Windows.Controls;

namespace DesktopBd.MVVM.ViewModel
{
    public class PasswordChangeViewModel : BaseVM
    {
        #region Constructors

        /// <summary>
        /// Передача пользователя на ВьюМодель
        /// </summary>
        /// <param name="user"></param>
        public PasswordChangeViewModel(UserModel user)
        {
            User = user;
        }

        #endregion



        #region Commands

        /// <summary>
        /// Изменение строки для первого парлоя
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
        /// Изменение строки для второго пароля
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
        /// Принятие измнений пароля
        /// </summary>
        public RelayCommand ApllyChanges
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (PasswordTest(FirstPassword, SecondPassword))
                    {
                        PasswordChangeComplete(FirstPassword);
                        CloseFlag = false;
                        ViewManager.CloseView();
                    }
                });
            }
        }

        /// <summary>
        /// Отмена изменений
        /// </summary>
        public RelayCommand Cencel
        {
            get
            {
                return new RelayCommand(obj =>
                {
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
                    if (CloseFlag) ViewManager.CloseView();
                });
            }
        }

        #endregion



        #region Main Logic

        /// <summary>
        /// Тестирование паролей на валидность
        /// </summary>
        /// <param name="firstPassword"></param>
        /// <param name="secondPassword"></param>
        /// <returns></returns>
        public Boolean PasswordTest(String firstPassword, String secondPassword)
        {
            String tmpInfo = String.Empty;
            if (ValidationService.PasswordTest(firstPassword, secondPassword, out tmpInfo))
            {
                if (FirstPassword != User.Password) return true;
                else
                {
                    PasswordInfo = ErorInfo_BadPassword_OldPassword;
                    return false;
                }
            }
            else
            {
                PasswordInfo = tmpInfo;
                return false;
            }
        }

        #endregion



        #region Events

        public delegate void PasswordChangeCompleteEventHandler(String password);
        public event PasswordChangeCompleteEventHandler PasswordChangeComplete;

        #endregion



        #region Get/Set

        public String PasswordInfo
        {
            get => _passwordInfo;
            set
            {
                _passwordInfo = value;
                NotifyPropertyChanged(nameof(PasswordInfo));
            }
        }

        #endregion



        #region Fields

        public String FirstPassword { get; private set; } = String.Empty;
        public String SecondPassword { get; private set; } = String.Empty;
        private String _passwordInfo;
        public String ErorInfo_BadPassword_OldPassword = "Совпадает со старым";

        public Boolean CloseFlag { get; private set; } = true;

        public UserModel User { get; private set; }

        #endregion

    }
}
