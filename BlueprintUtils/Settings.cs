using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueprintUtils
{
    public class Settings
    {
        public static bool ClearOwner { get; set; }         //Удалять теги Owner и BuiltBy?
        public static bool CreateMultiGrid { get; set; }    //Создавать чертежи объединенных объектов?
        public static bool RemoveDeformation { get; set; }  //Удалять деформации объектов?
        public static bool RemoveAI { get; set; }           //Удалять автоматическое поведение?
        public static bool ExtractProjectorBP { get; set; } //Извлекать чертежи из проектора?

        static Settings()
        {
            SetDefault();
        }

        public static void SetDefault()
        {
            ClearOwner = false;
            CreateMultiGrid = true;
            RemoveDeformation = true;
            RemoveAI = true;
            ExtractProjectorBP = true;
        }
    }
}
