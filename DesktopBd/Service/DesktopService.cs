using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopBd.Repositories;
using DesktopBd.MVVM.Model;

namespace DesktopBd.Service
{
    public static class DesktopService
    {

        #region Main Logic

        /// <summary>
        /// Создает новый рабочий стол
        /// </summary>
        public static void CreateDesktop()
        {
            DesktopRepositori.AddDesktop(new DesktopModel());
        }

        /// <summary>
        /// Возвращает рабочий стол из репозитория по заданному индексу
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static DesktopModel GetDesktop(Int16 index)
        {
            return DesktopRepositori.GetDesktop(index);
        }

        /// <summary>
        /// Вызывает удаление последнего рабочего стола из репозитория
        /// </summary>
        public static void DeleteLastDesktop()
        {
            DesktopRepositori.DeleteLastDesktop();
        }

        #endregion

       

        #region Fields

        public static DesktopRepositorie DesktopRepositori { get; private set; } = new DesktopRepositorie();

        #endregion

    }
}
