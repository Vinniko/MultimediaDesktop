using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopBd.MVVM.Model;

namespace DesktopBd.Repositories
{
   static class AccountRepositorie
    {
        #region Main Logic

        /// <summary>
        /// Добавить новый аккаунт в репозиторий
        /// </summary>
        /// <param name="userModel"></param>
        public static void AddAccount(UserModel userModel)
        {
            Accounts.Add(userModel.Login, userModel);
        }

        /// <summary>
        /// Возвращает аккаунт пользователя из репозитория
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public static UserModel GetAccount(String login)
        {
            return Accounts[login];
        }

        /// <summary>
        /// Возвращает коллекцию аккаунтов из репозитория
        /// </summary>
        /// <returns></returns>
        public static Dictionary<String, UserModel> GetAccounts()
        {
            return Accounts;
        }

        /// <summary>
        /// Возвращает значение: Существует ли аккаунт в репозитории 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public static Boolean AccountExist(String login)
        {
            return Accounts.ContainsKey(login);
        }

        #endregion



        #region Fields

        public static Dictionary<string, UserModel> Accounts = new Dictionary<string, UserModel>();

        #endregion
    }
}
