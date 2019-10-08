using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopBd.Service;

namespace DesktopBd.MVVM.Model
{
    public class UserModel
    {
        #region Constructors

        public UserModel(String login, String password)
        {
            Login = login;
            Password = password;
        }

        #endregion



        #region Get/Set
        
        /// <summary>
        /// Колличество рабочих столов User
        /// </summary>
        public Int16 DesktopCount
        {
            get => _desktopsCount;
            set
            {
                _desktopsCount = value;
            }
        }

        public String Login
        {
            get => _login;
            set
            {
                _login = value;
                ID = String.Concat(FileNameStringFirst, value, FileNameStringSecond);
            }
        }
        public String Password
        {
            get => _password;
            set
            {
                _password = value;
            }
        }
        /// <summary>
        /// Наименование шрифта пользователя
        /// </summary>
        public String Font
        {
            get => _font;
            set
            {
                _font = value;
            }
        }

        public String[] FontColor
        {
            get => _fontColor;
            set
            {
                _fontColor = value;
            }
        }

        public Int32 FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
            }
        }

        public Boolean FontBold
        {
            get => _fontBold;
            set
            {
                _fontBold = value;
            }
        }
        public Boolean FontItalic
        {
            get => _fontItalic;
            set
            {
                _fontItalic = value;
            }
        }
        public Boolean FontUnderline
        {
            get => _fontUnderline;
            set
            {
                _fontUnderline = value;
            }
        }
        public Boolean FontStrikeout
        {
            get => _fontStrikeout;
            set
            {
                _fontStrikeout = value;
            }
        }

        #endregion



        #region Fields

        /// <summary>
        /// ID пользователя для создания файла
        /// </summary>
        public String ID { get; private set; }
        private String _login;
        private String _password;

        /// <summary>
        /// Шрифт используемый для ярлыков
        /// </summary>
        private String _font = "Arial";
        private String[] _fontColor = { "255", "0", "0", "0" };

        private Int32 _fontSize = 12;

        private Boolean _fontBold = false;
        private Boolean _fontItalic = false;
        private Boolean _fontUnderline = false;
        private Boolean _fontStrikeout = false;

        public String FileNameStringFirst { get; private set; } = "LOCAL_ACCOUNT_SETTINGS_";
        public String FileNameStringSecond { get; private set; } = "_DESKTOP";

        private Int16 _desktopsCount = 0;
        
        #endregion
    }
}
