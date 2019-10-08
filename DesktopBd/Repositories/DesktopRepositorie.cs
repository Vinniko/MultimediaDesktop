using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopBd.MVVM.Model;

namespace DesktopBd.Repositories
{
    public class DesktopRepositorie
    {

        #region Main Logic

        /// <summary>
        /// Добавляет рабочий стол в репозиторий
        /// </summary>
        /// <param name="desktop"></param>
        public void AddDesktop(DesktopModel desktop)
        {
            Desktops.Add(desktop);
        }

        /// <summary>
        /// Возвращает рабочий стол из репозитория по индексу
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DesktopModel GetDesktop(Int16 index)
        {
            return Desktops.ElementAt(index);
        }

        /// <summary>
        /// Удаляет последний рабочий стол из репозитория
        /// </summary>
        public void DeleteLastDesktop()
        {
            Desktops.RemoveAt(Desktops.Count - 1);
        }

        #endregion



        #region Fields

        public List<DesktopModel> Desktops { get; private set; } = new List<DesktopModel>();

        #endregion

    }
}
