using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DesktopBd.MVVM.View;
using DesktopBd.MVVM.ViewModel;

namespace DesktopBd.Service
{
    static class ViewManager
    {
        #region Main Logic

        /// <summary>
        /// Открывает новое окно, закрывая предыдущее.
        /// </summary>
        /// <param name="window"></param>
        public static void OpenNewWindowInShowDialog(Window window)
        {
            Push(window, true);
            ViewStack.ElementAt(0).ShowDialog();
        }

        /// <summary>
        /// В зависимости от флага, либо закрывает предыдущее окно, 
        /// либо делает видимым новое, но не затрагивает старое окно.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="flag"></param>
        public static void OpenNewWindowInShow(Window window, bool flag)
        {
            Push(window, flag);
            ViewStack.ElementAt(0).ShowDialog();
        }

        /// <summary>
        /// Закрывает окно, с которым ведется работа.
        /// </summary>
        public static void CloseView()
        {
            Close(ViewStack.ElementAt(0));
            Pop();
        }
        
        /// <summary>
        /// Возвращает экземпляр текущего окна.
        /// </summary>
        /// <returns></returns>
        public static Window GetView()
        {
            return ViewStack.ElementAt(0);
        }

        #endregion



        #region Staff

        /// <summary>
        /// Добавляет окно в стек окон. В случае положительного флага, вызывает удаление предыдущего окна.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="flag"></param>
        private static void Push(Window window, bool flag)
        {
            if (flag)
            {
                if (ViewStack.Count > 0)
                {
                    Close(ViewStack.ElementAt(0));
                    Pop();
                }
            }
            ViewStack.Push(window);
        }

        /// <summary>
        /// Удаление предыдущего окна из стека.
        /// </summary>
        public static void Pop()
        {
            ViewStack.Pop();
        }

        /// <summary>
        /// Скрытие окна.
        /// </summary>
        /// <param name="window"></param>
        private static void HideWindow(Window window)
        {
            window.Hide();
        }

        /// <summary>
        /// Закрытие главного окна и программы.
        /// </summary>
        /// <param name="window"></param>
        private static void Close(Window window)
        {
            window.Close();
        }

        #endregion



        #region Fields

        static Stack<Window> ViewStack = new Stack<Window>();

        #endregion

    }
}
