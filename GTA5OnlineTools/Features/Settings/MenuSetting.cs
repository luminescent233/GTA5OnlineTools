using GTA5OnlineTools.Features.Core;

namespace GTA5OnlineTools.Features.Settings;

public static class MenuSetting
{
    public static class Overlay
    {
        public static bool VSync = true;
        public static int FPS = 300;

        public static bool ESP_2DBox = true;
        public static bool ESP_3DBox = false;
        public static bool ESP_2DLine = true;
        public static bool ESP_Bone = false;
        public static bool ESP_2DHealthBar = true;
        public static bool ESP_HealthText = false;
        public static bool ESP_NameText = false;
        public static bool ESP_Player = true;
        public static bool ESP_NPC = true;
        public static bool ESP_Crosshair = true;

        public static bool AimBot_Enabled = false;
        public static int AimBot_BoneIndex = 0;
        public static float AimBot_Fov = 250.0f;
        public static WinVK AimBot_Key = WinVK.CONTROL;

        public static bool NoTOPMostHide = false;
    }
}
