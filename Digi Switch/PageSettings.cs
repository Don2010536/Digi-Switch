using System.IO;

namespace Digi_Switch
{
    class PageSettings
    {
        public static string primary        = "#FF171D29";
        public static string secondary      = "#FF1D2231";
        public static string textAndBorders = "#FFFFFFFF";
        public static string highlight      = "#FF71A1F9";
        public static string good           = "#FF53915A";
        public static string medium         = "#FFCFA031";
        public static string bad            = "#FFFF3924";

        public static void Load()
        {
            if (File.Exists(DirLocations.settingsPath))
            {
                try
                {
                    primary        = File.ReadAllLines(DirLocations.settingsPath)[1];
                    secondary      = File.ReadAllLines(DirLocations.settingsPath)[2];
                    textAndBorders = File.ReadAllLines(DirLocations.settingsPath)[3];
                    highlight      = File.ReadAllLines(DirLocations.settingsPath)[4];
                    good           = File.ReadAllLines(DirLocations.settingsPath)[5];
                    medium         = File.ReadAllLines(DirLocations.settingsPath)[6];
                    bad            = File.ReadAllLines(DirLocations.settingsPath)[7];
                } catch { }
                Save();
            }
        }

        public static void Save()
        {
            File.WriteAllText(DirLocations.settingsPath, $"{DataManager.topMost}\n{primary.ToUpper()}\n{secondary.ToUpper()}\n{textAndBorders.ToUpper()}\n{highlight.ToUpper()}\n{good.ToUpper()}\n{medium.ToUpper()}\n{bad.ToUpper()}");
        }

        public static void Reset()
        {
            File.WriteAllText(DirLocations.settingsPath, $"{DataManager.topMost}\n#FF171D29\n#FF1D2231\n#FFFFFFFF\n#FF71A1F9\n#FF53915A\n#FFCFA031\n#FFFF3924");
        }
    }
}
