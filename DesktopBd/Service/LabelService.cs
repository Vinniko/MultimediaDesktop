using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopBd.Repositories;
using DesktopBd.MVVM.Model;

namespace DesktopBd.Service
{
    public static class LabelService
    {

        #region Main Logic

        /// <summary>
        /// Создание ярлыка
        /// </summary>
        public static void CreateLabel()
        {
            LabelRepositori.AddLabel(new LabelModel());
        }
        /// <summary>
        /// Создание none ярлыка
        /// </summary>
        /// <param name="typeFlag"></param>
        public static void CreateLabel(Boolean typeFlag)
        {
            LabelRepositori.AddLabel(new LabelModel(typeFlag));
        }
        /// <summary>
        /// Изменение none ярлыка на нормальный ярлык
        /// </summary>
        /// <param name="task"></param>
        /// <param name="imagePath"></param>
        /// <param name="path"></param>
        /// <param name="typeFlag"></param>
        /// <param name="index"></param>
        public static void CreateLabel(String task, String imagePath, String path, Boolean typeFlag, Int32 index)
        {
            LabelRepositori.InsertLabel(new LabelModel(task, imagePath, path, typeFlag), index);
        }
        /// <summary>
        /// Добавление ярлыка
        /// </summary>
        /// <param name="task"></param>
        /// <param name="imagePath"></param>
        /// <param name="path"></param>
        /// <param name="typeFlag"></param>
        public static void AddLabel(String task, String imagePath, String path, Boolean typeFlag)
        {
            LabelRepositori.AddLabel(new LabelModel(task, imagePath, path, typeFlag));
        }

        /// <summary>
        /// Получение ярлыка из репозитория по index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static LabelModel GetLabel(Int32 index)
        {
            return LabelRepositori.GetLabel(index);
        }
        /// <summary>
        /// Получение массива ярылков в заданном промежутке с началом в заданном index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<LabelModel> GetLabelsRange(Int32 index, Int32 count)
        {
            return LabelRepositori.GetLabels(index * LabelMaxCount, count);
        }

        /// <summary>
        /// Получение общего колличества ярлыков
        /// </summary>
        /// <returns></returns>
        public static Int32 GetLabelsCount()
        {
            return LabelRepositori.GetLabelsCount();
        }

        /// <summary>
        /// Проверка превышения колличества ярлыков на рабочем столе
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static Boolean LabelCountTest(Int32 count)
        {
            if (count < LabelMaxCount) return true;
            else return false;
        }

        /// <summary>
        /// Удаление ярлыка из репозитория по заданному индексу
        /// </summary>
        /// <param name="label"></param>
        public static void DeleteLabel(LabelModel label)
        {
            if (LabelRepositori.DeleteLabel(label)) SettingsEvent();
        }

        #endregion



        #region Events

        public delegate void DeleteLabelEventHandler();
        public static event DeleteLabelEventHandler SettingsEvent;

        #endregion



        #region Fields

        public static LabelRepositorie LabelRepositori { get; private set; } = new LabelRepositorie();

        public const Int16 LabelMaxCount = 126;

        #endregion

    }
}
