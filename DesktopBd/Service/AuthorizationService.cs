using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopBd.MVVM.Model;
using DesktopBd.Repositories;


namespace DesktopBd.Service
{
    static class AuthorizationService
    {
        #region Main Logic

        /// <summary>
        /// Возращает логичское значиение: Существует ли аккаунт
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Boolean AcccountTest(String login, String password)
        {
            if (AccountRepositorie.GetAccount(login).Password == password) return true;
            else return false;
        }

        /// <summary>
        /// Возвращает пользователя по введенному логину и паролю
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static UserModel Authorization(String login, String password)
        {
            return AccountRepositorie.GetAccount(login);
        }

        #endregion
    }
}
