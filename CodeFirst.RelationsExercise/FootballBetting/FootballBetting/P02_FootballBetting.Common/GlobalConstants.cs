using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Common
{
    public class GlobalConstants
    {
        // Team
        public const int TeamNameMaxLength = 95;
        public const int UrlMaxLength = 2048;
        public const int InitialsMaxLength = 4;

        // Color
        public const int ColorNameMaxLength = 29;

        // Town
        public const int TownNameMaxLength = 90;

        // Country
        public const int CountryNameMaxLength = 60;

        // PLayer
        public const int PlayerNameMaxLength = 99;

        // Position
        public const int PositionNameMaxLength = 35;

        // User
        public const int UserNameMaxLength = 88;
        public const int UsersNamesMaxLength = PlayerNameMaxLength; // 99
        public const int PassworMaxLength = 288;
        public const int EmailMaxLength = 355;
    }
}
