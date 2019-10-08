using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopBd.Service;
using DesktopBd.Repositories;
using DesktopBd.MVVM.Model;
using System.IO;
using System.Drawing;
using System.Windows;

namespace DesktopBd.Service
{
    static class FileService
    {
        // TODO Перевсти все с файловой системы на базу данных с таблицами: Пользователи, Настройки, Рабочие столы, Ярлыки...

        #region Main Logic
            
        /// <summary>
            /// Запись аккаунтов в файл
            /// </summary>
        public static void WriteAccountsInFile()
        {
            if (!Directory.Exists(_accountsFolder))
            {
                Directory.CreateDirectory(_accountsFolder);
            }
            var tmp_stream = new FileStream(_accountPath, FileMode.Create);
            var writingAccount = new BinaryWriter(tmp_stream);
            writingAccount.Write(AccountRepositorie.GetAccounts().Count);
            for (int i = 0; i < AccountRepositorie.GetAccounts().Count; i++)
            {
                writingAccount.Write(AccountRepositorie.GetAccounts().Values.ElementAt(i).Login);
                writingAccount.Write(AccountRepositorie.GetAccounts().Values.ElementAt(i).Password);
            }
            writingAccount.Close();
        }
        /// <summary>
        /// Запоминание пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        public static void WriteRememberUserInFile(String login, String password)
        {
            if (!Directory.Exists(_accountsFolder))
            {
                Directory.CreateDirectory(_accountsFolder);
            }
            var tmp_stream = new FileStream(_rememberUserFilePath, FileMode.Create);
            var writingAccount = new BinaryWriter(tmp_stream);
            writingAccount.Write(login);
            writingAccount.Write(password);
            writingAccount.Close();
        }

        /// <summary>
        /// Сохранение временного изображения для работы с ним
        /// </summary>
        /// <param name="image"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static String SaveTmpImage(Image image, String id)
        {
            if (!Directory.Exists(_usersLabelsImagesFolder))
            {
                Directory.CreateDirectory(_usersLabelsImagesFolder);
            }
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Images\" + id + "_images")))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Images\" + id + "_images"));
            }
            String tmp_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Images\" + id + @"_images\" + "tmp.png");
            if (!File.Exists(tmp_path))
            {
                image.Save(tmp_path, System.Drawing.Imaging.ImageFormat.Png);
                image.Dispose();
            }
            else
            {
                image.Save(tmp_path, System.Drawing.Imaging.ImageFormat.Png);
                image.Dispose();
            }
            return tmp_path;
        }
        /// <summary>
        /// Сохранение изображения
        /// </summary>
        /// <param name="image"></param>
        /// <param name="id"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        public static String SaveImage(Image image, String id, String task)
        {
            try
            {
                if (!Directory.Exists(_usersLabelsImagesFolder))
                {
                    Directory.CreateDirectory(_usersLabelsImagesFolder);
                }
                if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Images\" + id + "_images")))
                {
                    Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Images\" + id + "_images"));
                }
                String tmp_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Images\" + id + @"_images\" + task + ".png");
                Boolean tmp_flag = true;
                Int32 tmp_count = 0;
                while (tmp_flag)
                {
                    if (!File.Exists(tmp_path))
                    {
                        image.Save(tmp_path, System.Drawing.Imaging.ImageFormat.Png);
                        image.Dispose();
                        tmp_flag = false;
                    }
                    else
                    {
                        tmp_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Images\" + id + @"_images\" + String.Concat(tmp_count.ToString(), "_", task) + ".png");
                        tmp_count++;
                    }
                }
                return tmp_path;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
            
        }

        /// <summary>
        /// Чтение аккаунтов из файла в репозиторий
        /// </summary>
        public static void ReadAccountsInFile()
        {
            if (File.Exists(_accountPath))
            {
                var tmp_stream = new FileStream(_accountPath, FileMode.Open);
                var readingAccount = new BinaryReader(tmp_stream);
                string tmp_login = string.Empty;
                string tmp_password = string.Empty;
                int tmp_count = readingAccount.ReadInt32();
                for (int i = 0; i < tmp_count; i++)
                {
                    tmp_login = readingAccount.ReadString();
                    tmp_password = readingAccount.ReadString();
                    RegistrationService.CreateNewAccount(tmp_login, tmp_password);
                }
                readingAccount.Close();
            }
        }
        /// <summary>
        /// Чтение пользователя, которого запомнили в прошлый раз
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="flag"></param>
        public static void ReadRememberUserInFile(out String login, out String password, out Boolean flag)
        {
            if (File.Exists(_rememberUserFilePath))
            {
                var tmp_stream = new FileStream(_rememberUserFilePath, FileMode.Open);
                var readingAccount = new BinaryReader(tmp_stream);
                login = readingAccount.ReadString();
                password = readingAccount.ReadString();
                readingAccount.Close();
                flag = true;
            }
            else
            {
                login = String.Empty;
                password = String.Empty;
                flag = false;
            }
        }

        /// <summary>
        /// Удаление запомененного пользователя
        /// </summary>
        public static void DeleteRememberUserInFile()
        {
            File.Delete(_rememberUserFilePath);
        }

        /// <summary>
        /// Проверка существования файла в папке настроек
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Boolean CheckFileExist(String name)
        {
            String tmp_path = CreateFilePath(_usersSettingsFolder, name);
            return File.Exists(tmp_path);
        }

        /// <summary>
        /// Запись ярлыков в файл
        /// </summary>
        /// <param name="user"></param>
        public static void WriteLabelsToFile(UserModel user)
        {
            if (!Directory.Exists(_usersSettingsFolder))
            {
                Directory.CreateDirectory(_usersSettingsFolder);
            }
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Settings\" + user.ID + ".dat");
            var tmpStream = new FileStream(path, FileMode.Create);
            var writingSettings = new BinaryWriter(tmpStream);

            writingSettings.Write(user.DesktopCount);
            for (short i = 0; i < user.DesktopCount; i++)
            {
                writingSettings.Write(DesktopService.GetDesktop(i).LabelCount);
                writingSettings.Write(DesktopService.GetDesktop(i).WallpaperImage);
            }
            writingSettings.Write(user.Font);
            writingSettings.Write(user.FontSize);
            writingSettings.Write(user.FontColor.Count());
            for (int i = 0; i < user.FontColor.Count(); i++)
                writingSettings.Write(user.FontColor[i]);
            writingSettings.Write(user.FontBold);
            writingSettings.Write(user.FontItalic);
            writingSettings.Write(user.FontUnderline);
            writingSettings.Write(LabelService.GetLabelsCount() - 1);
            for (int i = 0; i < LabelService.GetLabelsCount() - 1; i++)
            {
                writingSettings.Write(LabelService.GetLabel(i).Title);
                writingSettings.Write(LabelService.GetLabel(i).Path);
                writingSettings.Write(LabelService.GetLabel(i).ImagePath);
            }
            writingSettings.Close();
        }

        /// <summary>
        /// Чтение ярлыков из файла
        /// </summary>
        /// <param name="user"></param>
        public static void ReadInFile(UserModel user)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Settings\" + user.ID + ".dat");
            FileStream tmpStream = new FileStream(path, FileMode.Open);
            BinaryReader readingSettings = new BinaryReader(tmpStream);

            user.DesktopCount = readingSettings.ReadInt16();
            for (short i = 0; i < user.DesktopCount; i++)
            {
                DesktopService.CreateDesktop();
            DesktopService.GetDesktop(i).LabelCount = readingSettings.ReadInt16();
                DesktopService.GetDesktop(i).WallpaperImage = readingSettings.ReadString();
            }

            user.Font= readingSettings.ReadString();
            user.FontSize = readingSettings.ReadInt32();
            int tmpColorsArrayCount = readingSettings.ReadInt32();
            for (int i = 0; i < tmpColorsArrayCount; i++)
                user.FontColor[i] = readingSettings.ReadString();
            user.FontBold = readingSettings.ReadBoolean();
            user.FontItalic = readingSettings.ReadBoolean();
            user.FontUnderline = readingSettings.ReadBoolean();

            int tmpCount = readingSettings.ReadInt32();
            for (int i = 0; i < tmpCount; i++)
            {
                String tmpTask = readingSettings.ReadString();
                String tmpPath = readingSettings.ReadString();
                String tmpImagePath = readingSettings.ReadString();
                LabelService.AddLabel(tmpTask, tmpImagePath, tmpPath, true);
            }
            readingSettings.Close();
        }

        /// <summary>
        /// Удаление файла настроек пользователя
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteSettingFile(String id)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Settings\" + id + ".dat");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Удаление файла по заданному пути
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFile(String path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        #endregion



        #region Staff

        public static String CreateFilePath(String filePath, String fileName)
        {
            return String.Concat(filePath, @"\", fileName, @".dat");
        }

        #endregion



        #region Fields

        static String _accountsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Accounts");
        static String _accountPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Accounts\accountList.dat");
        static String _rememberUserFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Accounts\rememberUserData.dat");
        static String _usersSettingsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Settings");
        static String _usersLabelsImagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Images");

        #endregion
    }
}
