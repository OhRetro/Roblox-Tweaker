using System;

namespace RobloxTweaker
{
    internal class MainVariables
    {
        //Roblox Tweaker Variables
        public static readonly string ROBLOX_VERSIONS_DIR = Environment.GetEnvironmentVariable("LOCALAPPDATA") + "\\Roblox\\Versions";
        public static string ROBLOX_VERSION_DIR = "Not Set";
        public static string ROBLOX_VERSION_DIR_TYPE = "Not Set";
        public const string SETTINGS_FILE = ".\\settings.txt";

        public static string ROBLOX_TEXTURE_DIR = "Not Set";
        public const string PATH_TO_TEXTURES_DIR = "\\PlatformContent\\pc\\textures";
        public static readonly string[] EXCEPTION_TEXTURES = { "sky", "brdfLUT.dds", "studs.dds", "wangIndex.dds" };

        public const string BACKUP_TEXTURE_FILE = ".\\textures_backup.zip";
    }
}
