using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCharactersFilms.Constants
{
    public class Constraints
    {
        public enum StatusUser { Active, Blocked, Pending };

        public const int MaxLengthNames = 30;
        public const int MaxLengthSurnames = 30;
        public const int MaxLengthEmail = 60;
        public const int MaxLengthRole = 10;
        public const int MaxLengthPassword = 30;
        public const int MaxLengthPasswordEncrypted = 500;
        public const int MinLengthPassword = 8;
        public const int MaxLengthBiography = 500;
        public const int MaxLengthTitle = 30;
        public const int MaxLengthPathFile = 100;
        public const int MaxLengthToken = 500;

        public const int MinQualification = 1;
        public const int MaxQualification = 5;


        public const int MinYearsOld = 1;
        public const int MaxYearsOld = 100;

        public const int MinWeight = 1;
        public const int MaxWeight = 200;
        public const string OrderASC = "ASC";
        public const string OrderDESC = "DESC";

        public const int MaxLengthFile = 2097152;
        public const string Jpg = "jpg";
        public const string Jpge = "jpeg";
        public const string Png = "png";
    }
}
