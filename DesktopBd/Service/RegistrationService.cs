using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopBd.MVVM.Model;
using DesktopBd.Repositories;

namespace DesktopBd.Service
{
    static class RegistrationService
    {
        #region Main Logic

        public static UserModel CreateNewAccount(String login, String password)
        {
            var user = new UserModel(login, password);
            AccountRepositorie.AddAccount(user);
            return user;
        }

        #endregion
    }
}
