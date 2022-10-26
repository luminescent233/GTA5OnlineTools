namespace GTA5OnlineTools.Features.SDK;

public static class Offsets
{
    public static class Mask
    {
        public const string WorldMask = "48 8B 05 ?? ?? ?? ?? 45 ?? ?? ?? ?? 48 8B 48 08 48 85 C9 74 07";
        public const string BlipMask = "4C 8D 05 ?? ?? ?? ?? 0F B7 C1";
        public const string GlobalMask = "4C 8D 05 ?? ?? ?? ?? 4D 8B 08 4D 85 C9 74 11";

        public const string ReplayInterfaceMask = "48 8D 0D ?? ?? ?? ?? 48 8B D7 E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 8A D8 E8";
        public const string NetworkPlayerMgrMask = "48 8B 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B C8 E8 ?? ?? ?? ?? 48 8B CF";

        public const string ViewPortMask = "48 8B 15 ?? ?? ?? ?? 48 8D 2D ?? ?? ?? ?? 48 8B CD";
        public const string CCameraMask = "48 8B 05 ?? ?? ?? ?? 4A 8B 1C F0";
        public const string AimingPedMask = "48 8B 0D ?? ?? ?? ?? 48 85 C9 74 0C 48 8D 15 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 89 1D";

        public const string WeatherMask = "48 83 EC ?? 8B 05 ?? ?? ?? ?? 8B 3D ?? ?? ?? ?? 49";

        public const string FriendListMask = "48 8B 0D ?? ?? ?? ?? 8B C6 48 8D 5C 24 70";
        public const string PickupDataMask = "48 8B 05 ?? ?? ?? ?? 48 8B 1C F8 8B";
        public const string UnkModelMask = "4C 8B 15 ?? ?? ?? ?? 49 8B 04 D2 44 39 40 08";

        public const string PlayerchatterNameMask = "48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 48 83 C4 28 E9 ?? ?? ?? ?? 48 83 EC 28 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 48 83 C4 28 E9 ?? ?? ?? ?? 48 83 EC 28 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 48 83 C4 28 E9 ?? ?? ?? ?? 48 83 EC 28 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 48 83 C4 28 E9 ?? ?? ?? ?? 48 83 EC 28 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 48 83 C4 28 E9 ?? ?? ?? ?? 48 83 EC 28 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 48 83 C4 28 E9 ?? ?? ?? ?? 48 83 EC 28 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 48 83 C4 28 E9 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E9 ?? ?? ?? ?? 48 83 EC 28 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ??";
        public const string PlayerExternalDisplayNameMask = "48 8D 0D ?? ?? ?? ?? 48 69 C0 B0 00 00 00 F6 84 08 ?? ?? ?? ?? 02 74 0C";

        public const string LocalScriptsMask = "48 8B 05 ?? ?? ?? ?? 8B CF 48 8B 0C C8 39 59 68";

        public const string UnkMask = "48 39 3D ?? ?? ?? ?? 75 2D";
    }

    ////////////////////////////////////////////////////////////////////

    public const int InitSession_State = 1574589;
    public const int InitSession_Cache = 1574589 + 2;
    public const int InitSession_Type = 1575015;

    ////////////////////////////////////////////////////////////////////

    // Vehicle Menus Globals
    public const int oVMCreate = 2725439;       // Create any vehicle.
    public const int oVMYCar = 2815059;         // Get my car.
    public const int oVGETIn = 2671449;         // Spawn into vehicle.
    public const int oVMSlots = 1585857;        // Get vehicle slots.

    // Some Player / Network times associated Globals
    public const int oNETTimeHelp = 2703735;
    public const int oPlayerIDHelp = 2689235;
    public const int oPlayerGA = 2703735;

    ////////////////////////////////////////////////////////////////////

    public const int SpawnPersonalVehicleIndex1 = oVMYCar + 965;
    public const int SpawnPersonalVehicleIndex2 = oVMYCar + 962;

    public static class Player
    {
        public static int[] Health = new int[] { 0x08, 0x280 };
        public static int[] MaxHealth = new int[] { 0x08, 0x284 };
        public static int[] Armor = new int[] { 0x08, 0x150C };

        public static int[] RunSpeed = new int[] { 0x08, 0x10A8, 0xCF0 };
        public static int[] SwimSpeed = new int[] { 0x08, 0x10A8, 0x170 };
        public static int[] StealthSpeed = new int[] { 0x08, 0x10A8, 0x18C };

        public static int[] GodMode = new int[] { 0x08, 0x189 };
        public static int[] Wanted = new int[] { 0x08, 0x10A8, 0x888 };
        public static int[] Invisibility = new int[] { 0x08, 0x2C };            // on 0x01 off 0x27

        public static int[] Seatbelt = new int[] { 0x08, 0x143C };
        public static int[] NoRagdoll = new int[] { 0x08, 0x10B8 };
        public static int[] NoCollision = new int[] { 0x08, 0x30, 0x10, 0x20, 0x70, 0x00, 0x2C };
    }

    public static int[] SpecialAmmo = new int[] { 0x08, 0x10A8, 0x218 };
    public static int[] NeverWanted = new int[] { 0x08, 0x10A8, 0x73C };
    public static int[] WantedCanChange = new int[] { 0x08, 0x10A8, 0x73C };
    public static int[] NPCIgnore = new int[] { 0x08, 0x10A8, 0x872 };          // 00 04 C3 C7

    public static int[] RpMultiplier = new int[] { 0x10 };
    public static int[] ApMultiplier = new int[] { 0x3188 };

    public static int[] OnlineListPlayerName = new int[] { 0x08, 0x10A8, 0xA4 };

    public static int[] RID = new int[] { 0x08, 0x10A8, 0x90 };

    public static class Vehicle
    {
        public static int[] GodMode = new int[] { 0x08, 0xD10, 0x189 };
        public static int[] CurPassenger = new int[] { 0x08, 0xD10, 0xC62 };
        public static int[] Invisibility = new int[] { 0x08, 0xD10, 0x2C };            // on 0x01 off 0x27

        public static int[] Health = new int[] { 0x08, 0xD10, 0x280 };

        //public static int[] VehicleDrawHandler = new int[] { 0x08, 0xD10, 0x48 };
        //public static int[] Vehicle_EMS = new int[] { 0x08, 0xD10, 0x48, 0x3D6 };
        //public static int[] Vehicle_Brakes = new int[] { 0x08, 0xD10, 0x48, 0x3D7 };
        //public static int[] Vehicle_Transmission = new int[] { 0x08, 0xD10, 0x48, 0x3D8 };
        //public static int[] Vehicle_Horn = new int[] { 0x08, 0xD10, 0x48, 0x3D9 };
        //public static int[] Vehicle_Suspension = new int[] { 0x08, 0xD10, 0x48, 0x3DA };
        //public static int[] Vehicle_Armor = new int[] { 0x08, 0xD10, 0x48, 0x3DB };
        //public static int[] Vehicle_Turbo = new int[] { 0x08, 0xD10, 0x48, 0x3DD };
        //public static int[] Vehicle_Windows = new int[] { 0x08, 0xD10, 0x48, 0x3FF };

        public static int[] Extras = new int[] { 0x08, 0xD10, 0x20, 0x58B };
        public static int[] Parachute = new int[] { 0x08, 0xD10, 0x20, 0x58C };
    }

    public static class Weapon
    {
        public static int[] AmmoInfo = new int[] { 0x08, 0x10B8, 0x20, 0x60 };
        public static int[] AmmoModifier = new int[] { 0x08, 0x10D0, 0x78 };

        public static int[] NoSpread = new int[] { 0x8, 0x10B8, 0x20, 0x7C };
        public static int[] NoRecoil = new int[] { 0x8, 0x10B8, 0x20, 0x2F4 };

        public static int[] ImpactType = new int[] { 0x08, 0x10B8, 0x20, 0x20 };
        public static int[] ImpactExplosion = new int[] { 0x08, 0x10B8, 0x20, 0x24 };

        public static int[] Range = new int[] { 0x08, 0x10B8, 0x20, 0x28C };
        public static int[] LockRange = new int[] { 0x08, 0x10B8, 0x20, 0x288 };

        public static int[] ReloadMult = new int[] { 0x08, 0x10B8, 0x20, 0x134 };
        public static int[] ReloadVehicleMult = new int[] { 0x08, 0x10B8, 0x20, 0x130 };

    }

    ////////////////////////////////////////////////////////////////////

    public static int[] PlayerVisualX = new int[] { 0x08, 0x90 };
    public static int[] PlayerVisualY = new int[] { 0x08, 0x94 };
    public static int[] PlayerVisualZ = new int[] { 0x08, 0x98 };

    public static int[] PlayerPositionX = new int[] { 0x08, 0x30, 0x50 };
    public static int[] PlayerPositionY = new int[] { 0x08, 0x30, 0x54 };
    public static int[] PlayerPositionZ = new int[] { 0x08, 0x30, 0x58 };

    public static int[] PlayerSin = new int[] { 0x08, 0x30, 0x20 };
    public static int[] PlayerCos = new int[] { 0x08, 0x30, 0x30 };

    public static int[] PlayerProof = new int[] { 0x08, 0x188 };

    public static int[] InVehicle = new int[] { 0x08, 0xE52 };       // 1代表在载具，0代表不在

    public static int[] VehicleMaxSpeed = new int[] { 0x08, 0xD10, 0x8CC };
    public static int[] VehicleGear = new int[] { 0xFD4 };
    public static int[] VehicleRPM = new int[] { 0xE50 };

    // 角色在载具中，坐标偏移多了个pCVehicle = 0xD10
    public static int[] VehicleVisualX = new int[] { 0x08, 0xD10, 0x90 };
    public static int[] VehicleVisualY = new int[] { 0x08, 0xD10, 0x94 };
    public static int[] VehicleVisualZ = new int[] { 0x08, 0xD10, 0x98 };

    public static int[] VehiclePositionX = new int[] { 0x08, 0xD10, 0x30, 0x50 };
    public static int[] VehiclePositionY = new int[] { 0x08, 0xD10, 0x30, 0x54 };
    public static int[] VehiclePositionZ = new int[] { 0x08, 0xD10, 0x30, 0x58 };

    ////////////////////////////////////////////////////////////////////

    public static int[] ViewAngles_FPP_CosYaw = new int[] { 0x0, 0x3c0, 0x40 };
    public static int[] ViewAngles_FPP_SinYaw = new int[] { 0x0, 0x3c0, 0x44 };
    public static int[] ViewAngles_FPP_SinPitch = new int[] { 0x0, 0x3c0, 0x48 };

    public static int[] Crosshair_Location_X = new int[] { 0x0, 0x3c0, 0x60 };
    public static int[] Crosshair_Location_Y = new int[] { 0x0, 0x3c0, 0x64 };
    public static int[] Crosshair_Location_Z = new int[] { 0x0, 0x3c0, 0x68 };

    public static int[] ViewAngles_TPS_CosYaw = new int[] { 0x0, 0x3c0, 0x3D0 };
    public static int[] ViewAngles_TPS_SinYaw = new int[] { 0x0, 0x3c0, 0x3D4 };
    public static int[] ViewAngles_TPS_SinPitch = new int[] { 0x0, 0x3c0, 0x3D8 };

    public static int[] Crosshair_TPP_Location_X = new int[] { 0x0, 0x3c0, 0x3F0 };
    public static int[] Crosshair_TPP_Location_Y = new int[] { 0x0, 0x3c0, 0x3F4 };
    public static int[] Crosshair_TPP_Location_Z = new int[] { 0x0, 0x3c0, 0x3F8 };

    public static int[] Fov_Zoom = new int[] { 0x0, 0x3c0, 0x10, 0x30 };

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // CPedFactory (WorldPTR)
    public const int CPed = 0x08;

    // CPed Offsets
    public const int CPed_EntityType = 0x2B;                // int 156:Player 152:Other
    public const int CPed_Invisible = 0x2C;                 // byte 0x01:on 0x24:off
    public const int CPed_CNavigation = 0x30;
    public const int CPed_VisualX = 0x90;                   // float, vector3
    public const int CPed_VisualY = 0x94;
    public const int CPed_VisualZ = 0x98;
    public const int CPed_Proof = 0x188;
    public const int CPed_God = 0x189;                      // int8 0:false 1:true
    public const int CPed_Hostility = 0x18C;
    public const int CPed_Health = 0x280;                   // float
    public const int CPed_HealthMax = 0x284;
    public const int CPed_CVehicle = 0xD10;
    public const int CPed_InVehicle = 0xE32;                // int 0:false 1:true
    public const int CPed_Ragdoll = 0x1098;                 // int 32:false 1/0:true
    public const int CPed_CPlayerInfo = 0x10A8;
    public const int CPed_CPedInventory = 0x10B0;
    public const int CPed_CPedWeaponManager = 0x10B8;
    public const int CPed_Seatbelt = 0x143C;                // byte 55:false 56:true
    public const int CPed_Armor = 0x150C;                   // float, 50:Online 100:Story Mode

    // CPed CNavigation Offsets
    public const int CPed_CNavigation_RightX = 0x20;        // float, vector3
    public const int CPed_CNavigation_RightY = 0x24;
    public const int CPed_CNavigation_RightZ = 0x28;
    public const int CPed_CNavigation_ForwardX = 0x30;      // float, vector3
    public const int CPed_CNavigation_ForwardY = 0x34;
    public const int CPed_CNavigation_ForwardZ = 0x38;
    public const int CPed_CNavigation_UpX = 0x40;           // float, vector3
    public const int CPed_CNavigation_UpY = 0x44;
    public const int CPed_CNavigation_UpZ = 0x48;
    public const int CPed_CNavigation_PositionX = 0x50;     // float, vector3
    public const int CPed_CNavigation_PositionY = 0x54;
    public const int CPed_CNavigation_PositionZ = 0x58;

    // CPed CVehicle Offsets
    public const int CPed_CVehicle_CModelInfo = 0x20;
    public const int CPed_CVehicle_Invisible = 0x2C;        // byte 0x01:on 0x27:off
    public const int CPed_CVehicle_CNavigation = 0x30;
    public const int CPed_CVehicle_VisualX = 0x90;          // float, vector3
    public const int CPed_CVehicle_VisualY = 0x94;
    public const int CPed_CVehicle_VisualZ = 0x98;
    public const int CPed_CVehicle_State = 0xD8;            // int 0:Player 1:NPC 2:Unused 3:Destroyed
    public const int CPed_CVehicle_God = 0x189;             // int8 0:false 1:true
    public const int CPed_CVehicle_Health = 0x280;          // float
    public const int CPed_CVehicle_HealthMax = 0x284;
    public const int CPed_CVehicle_Passenger = 0xC42;       // byte 载具座位人数

    // CPed CVehicle CModelInfo Offsets
    public const int CPed_CVehicle_CModelInfo_ModelHash = 0x18;         // int
    public const int CPed_CVehicle_CModelInfo_CamDist = 0x38;           // float
    public const int CPed_CVehicle_CModelInfo_Name = 0x298;             // string [10]
    public const int CPed_CVehicle_CModelInfo_Maker = 0x2A4;            // string [10]
    public const int CPed_CVehicle_CModelInfo_Extras = 0x58B;           // short
    public const int CPed_CVehicle_CModelInfo_Parachute = 0x58C;

    // CPed CVehicle CNavigation Offsets
    public const int CPed_CVehicle_CNavigation_PositionX = 0x50;        // float, vector3
    public const int CPed_CVehicle_CNavigation_PositionY = 0x54;
    public const int CPed_CVehicle_CNavigation_PositionZ = 0x58;

    // CPed CPlayerInfo Offsets
    public const int CPed_CPlayerInfo_Name = 0xA4;                  // string[20]
    public const int CPed_CPlayerInfo_SwimSpeed = 0x170;            // float
    public const int CPed_CPlayerInfo_FrameFlags = 0x219;
    public const int CPed_CPlayerInfo_WalkSpeed = 0x18C;
    public const int CPed_CPlayerInfo_WantedCanChange = 0x73C;      // float
    public const int CPed_CPlayerInfo_WantedLevel = 0x888;          // int8
    public const int CPed_CPlayerInfo_RunSpeed = 0xCF0;             // float
    public const int CPed_CPlayerInfo_Stamina = 0xCF4;
    public const int CPed_CPlayerInfo_StaminaRegen = 0xCF8;

    // CPed CPedInventory Offsets
    public const int CPed_CPedInventory_AmmoModifier = 0x78;

    // CPed CPedWeaponManager Offsets
    public const int CPed_CPedWeaponManager_CWeaponInfo = 0x20;

    // CPed CPedWeaponManager CWeaponInfo Offsets
    public const int CPed_CPedWeaponManager_CWeaponInfo_ImpactType = 0x20;          // byte
    public const int CPed_CPedWeaponManager_CWeaponInfo_ImpactExplosion = 0x24;
    public const int CPed_CPedWeaponManager_CWeaponInfo_CAmmoInfo = 0x60;
    public const int CPed_CPedWeaponManager_CWeaponInfo_Spread = 0x7C;              // float
    public const int CPed_CPedWeaponManager_CWeaponInfo_ReloadMult = 0x134;
    public const int CPed_CPedWeaponManager_CWeaponInfo_LockRange = 0x288;
    public const int CPed_CPedWeaponManager_CWeaponInfo_Range = 0x28C;
    public const int CPed_CPedWeaponManager_CWeaponInfo_Recoil = 0x2F4;

}
