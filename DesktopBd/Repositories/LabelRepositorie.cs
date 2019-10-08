using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopBd.MVVM.Model;

namespace DesktopBd.Repositories
{
    public sealed class LabelRepositorie
    {

        #region Main Logic

        /// <summary>
        /// Добавляет новый ярлык в репозиторий
        /// </summary>
        /// <param name="label"></param>
        public void AddLabel(LabelModel label)
        {
            Labels.Add(label);
        }

        /// <summary>
        /// Изменяет старый ярлык в репозитории
        /// </summary>
        /// <param name="label"></param>
        /// <param name="index"></param>
        public void InsertLabel(LabelModel label, Int32 index)
        {
            Labels[index] = label;
        }

        /// <summary>
        /// Возвращает ярлык из репозитория по индексу
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public LabelModel GetLabel(Int32 index)
        {
            return Labels.ElementAt(index);
        }

        /// <summary>
        /// Возвращает коллекцию ярлыков определенного промежутка
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<LabelModel> GetLabels(Int32 startIndex, Int32 count)
        {
            return Labels.GetRange(startIndex, count);
        }

        /// <summary>
        /// Возвращает полное колличество ярлыков
        /// </summary>
        /// <returns></returns>
        public Int32 GetLabelsCount()
        {
            return Labels.Count;
        }

        /// <summary>
        /// Удаляет ярлык поданный в качестве парамметра
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public bool DeleteLabel(LabelModel label)
        {
            if (Labels.Contains(label))
            {
                Labels.Remove(label);
                return true;
            }
            else return false;
        }

        #endregion



        #region Fields

        public List<LabelModel> Labels { get; private set; } = new List<LabelModel>();

        #endregion

    }
}
