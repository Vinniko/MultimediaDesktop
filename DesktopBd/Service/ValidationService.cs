using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopBd.Repositories;

namespace DesktopBd.Service
{
    static class ValidationService
    {

        #region Main Logic

        /// <summary>
        /// Валидация логина при регистрации
        /// </summary>
        /// <param name="login"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static Boolean LoginRegistrationTest(String login, out String info)
        {
            if (login.Length == 0)
            {
                info = ErorInfo_BadLogin_LoginNotExist;
                return false;
            }
            else if (AccountRepositorie.AccountExist(login))
            {
                info = ErorInfo_BadLogin_LoginHaveExistInBd;
                return false;
            }
            else if (login.Contains(" "))
            {
                info = ErorInfo_BadLogin_BadSymbol;
                return false;
            }
            else
            {
                info = null;
                return true;
            }
        }
        /// <summary>
        /// Валидация логина при авторизации
        /// </summary>
        /// <param name="login"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static Boolean LoginAuthorizationTest(String login, out String info)
        {
            if (login.Length == 0)
            {
                info = ErorInfo_BadLogin_LoginNotExist;
                return false;
            }
            else if (!AccountRepositorie.AccountExist(login))
            {
                info = ErorInfo_BadLogin_LoginHaveNotExistInBd;
                return false;
            }
            else if (login.Contains(" "))
            {
                info = ErorInfo_BadLogin_BadSymbol;
                return false;
            }
            else
            {
                info = null;
                return true;
            }
        }
        /// <summary>
        /// Валидация пароля при авторизации
        /// </summary>
        /// <param name="password"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static Boolean PasswordTest(String password, out String info)
        {
            if (password.Contains(" "))
            {
                info = ErorInfo_BadPassword_BadSymbol;
                return false;
            }
            else if (password.Length < MinimalPasswordLength)
            {
                info = ErorInfo_BadPassword_SmallPasswrod;
                return false;
            }
            else
            {
                info = null;
                return true;
            }
        }
        /// <summary>
        /// Валидация паролей при регистрации
        /// </summary>
        /// <param name="firstPassword"></param>
        /// <param name="secondPassword"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static Boolean PasswordTest(String firstPassword, String secondPassword, out String info)
        {
            if (firstPassword != secondPassword)
            {
                info = ErorInfo_BadPassword_NotEqual;
                return false;
            }
            else if (firstPassword.Contains(" "))
            {
                info = ErorInfo_BadPassword_BadSymbol;
                return false;
            }
            else if (firstPassword.Length < MinimalPasswordLength)
            {
                info = ErorInfo_BadPassword_SmallPasswrod;
                return false;
            }
            else
            {
                info = null;
                return true;
            }
        }

        #endregion



        #region Fields 

        public static Int16 MinimalPasswordLength = 4;

        public static String ErorInfo_BadLogin_LoginNotExist = "Логин не введён";
        public static String ErorInfo_BadLogin_LoginHaveExistInBd = "Такой логин уже существует";
        public static String ErorInfo_BadLogin_LoginHaveNotExistInBd = "Такой логин не существует. Зарегистрируйтесь";
        public static String ErorInfo_BadLogin_BadSymbol = "Недопустимые символы";
        public static String ErorInfo_BadPassword_NotEqual = "Пароли не равны";
        public static String ErorInfo_BadPassword_BadSymbol = "Недопустимые символы";
        public static String ErorInfo_BadPassword_SmallPasswrod = "Маленький пароль";

        #endregion
    }
}
