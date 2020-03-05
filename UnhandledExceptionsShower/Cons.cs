using EFT;
using EFT.Interactive;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnhandledException
{
    public class Cons
    {
        public class Aim {
            public static float distanceToScan = 200f;
            public static float AAN_FOV = 3f;
            public static Player lastTargeted;
        }
        public class Distances
        {
            public static float Teleport = 1f;
            public static float Aim = 200f;
            public static float Players = 1000f;
            public static float Loot = 1000f;
            public static float Crates = 1000f;
            public static float Exfils = 1000f;
            public static float Corpses = 200f;
            public static float Grenade = 100f;
        }
        public class LocalPlayer {
            #region Packets
            public static void func1051() {
                //GClass1051 gclass;
                //gclass.
            }
            #endregion
            #region Skills and HP

            #endregion
            #region Group
            private static string Group = "";
            public static string GetGroup() {
                return Group;
            }
            public static void SetGroup(string localPlayerGroup)
            {
                Group = localPlayerGroup;
            }
            public static void SetGroup(Player localPlayer)
            {
                Group = localPlayer.Profile.Info.GroupId;
            }
            public static bool isInYourGroup(Player p)
            {
                return Group == p.Profile.Info.GroupId && Group != "0" && Group != "" && Group != null;
            }
            public static bool isInYourGroup(string groupId)
            {
                return Group == groupId && Group != "0" && Group != "" && Group != null;
            }
            #endregion
            #region Weapon
            public class Weapon {
                public static int RecoilIntensity; // described in %
                public static Vector3 SavedCameraSpringCurrent = Vector3.zero;
                public static Vector3 SavedHandsSpringCurrent = Vector3.zero;
                public static void SetRecoil() {
                    RecoilIntensity = (int)(Main._localPlayer.ProceduralWeaponAnimation.Shootingg.Intensity * 100);
                }
                public static void NoRecoil() {
                    if (Main._localPlayer != null)
                    {
                        /*
                        Main._localPlayer.ProceduralWeaponAnimation.HandsContainer.CameraRotation.InputIntensity = 0f;
                        Main._localPlayer.ProceduralWeaponAnimation.HandsContainer.CameraRotation.AccelerationMax = 0f;
                        Main._localPlayer.ProceduralWeaponAnimation.HandsContainer.HandsRotation.InputIntensity = 0f;
                        Main._localPlayer.ProceduralWeaponAnimation.HandsContainer.HandsRotation.AccelerationMax = 0f;
                        */
                        /*
                        Main._localPlayer.ProceduralWeaponAnimation.HandsContainer.CameraRotation.AccelerationMax = .01f;
                        Main._localPlayer.ProceduralWeaponAnimation.HandsContainer.CameraRotation.Damping = 0f;
                        Main._localPlayer.ProceduralWeaponAnimation.HandsContainer.CameraRotation.Softness = .01f;
                        Main._localPlayer.ProceduralWeaponAnimation.HandsContainer.CameraRotation.Max = new Vector3(0.01f, 0.01f, 0.01f);
                        Main._localPlayer.ProceduralWeaponAnimation.HandsContainer.CameraRotation.Min = new Vector3(0.01f, 0.01f, 0.01f);
                        Main._localPlayer.ProceduralWeaponAnimation.HandsContainer.HandsRotation.AccelerationMax = .01f;
                        Main._localPlayer.ProceduralWeaponAnimation.HandsContainer.HandsRotation.Damping = 0f;
                        Main._localPlayer.ProceduralWeaponAnimation.HandsContainer.HandsRotation.Softness = .01f;
                        Main._localPlayer.ProceduralWeaponAnimation.HandsContainer.HandsRotation.Max = new Vector3(0.01f, 0.01f, 0.01f);
                        Main._localPlayer.ProceduralWeaponAnimation.HandsContainer.HandsRotation.Min = new Vector3(0.01f, 0.01f, 0.01f);
                        Main._localPlayer.ProceduralWeaponAnimation.HandsContainer.SwaySpring._dampingRatio = .0001f;
                        Main._localPlayer.ProceduralWeaponAnimation.HandsContainer.Recoil.Damping = 0f;
                        */
                        Main._localPlayer.ProceduralWeaponAnimation.Shootingg.Intensity = 0f; // no recoil
                    }
                }

                public static string CurrentAmmo;
                public static string MaxAmmo;
                public static void UpdateAmmo()
                {
                    try
                    {
                        if (Main._localPlayer.Weapon != null)
                        {
                            CurrentAmmo = Main._localPlayer.Weapon.GetCurrentMagazineCount().ToString();
                            MaxAmmo = Main._localPlayer.Weapon.GetMaxMagazineCount().ToString();
                        }
                        else 
                        {
                            CurrentAmmo = "";
                            MaxAmmo = "";
                        }
                    }
                    catch (Exception) 
                    {
                        CurrentAmmo = "";
                        MaxAmmo = "";
                    }
                    
                }
            }
            #endregion
            #region Status - Health and other
            public class Status {
                public static string Energy;
                public static string Hydration;
                public class Health {
                    public static string Common;
                    public static string Head;
                    public static string Chest;
                    public static string LeftArm;
                    public static string RightArm;
                    public static string LeftLeg;
                    public static string RightLeg;
                    public static string Stomach;
                }
                private static string GetHealthEndStatus(EFT.HealthSystem.EBodyPart Part) {
                    int curr = (int)Main._localPlayer.HealthController.GetBodyPartHealth(Part).Current;
                    if (curr == 0)
                        return "n/a";
                    return curr.ToString() + "/" + ((int)Main._localPlayer.HealthController.GetBodyPartHealth(Part).Maximum).ToString();
                }
                private static string GetVitalEndStatus(string type) {
                    switch (type) {
                        case "Energy":
                            int curr_e = (int)Main._localPlayer.HealthController.Energy.Current;
                            if (curr_e == 0) {
                                return "No Energy!!";
                            }
                            return curr_e.ToString() + "/" + ((int)Main._localPlayer.HealthController.Energy.Maximum).ToString();
                        case "Hydration":
                            int curr_h = (int)Main._localPlayer.HealthController.Hydration.Current;
                            if (curr_h == 0)
                            {
                                return "Dehydration!!";
                            }
                            return curr_h.ToString() + "/" + ((int)Main._localPlayer.HealthController.Hydration.Maximum).ToString();
                        default:
                            return "";
                    }
                }
                public static void UpdateStatus()
                {
                    Energy = GetVitalEndStatus("Energy");
                    Hydration = GetVitalEndStatus("Hydration");
                    Health.Chest = GetHealthEndStatus(EFT.HealthSystem.EBodyPart.Chest);
                    Health.Common = GetHealthEndStatus(EFT.HealthSystem.EBodyPart.Common);
                    Health.Head = GetHealthEndStatus(EFT.HealthSystem.EBodyPart.Head);
                    Health.LeftArm = GetHealthEndStatus(EFT.HealthSystem.EBodyPart.LeftArm);
                    Health.LeftLeg = GetHealthEndStatus(EFT.HealthSystem.EBodyPart.LeftLeg);
                    Health.RightArm = GetHealthEndStatus(EFT.HealthSystem.EBodyPart.RightArm);
                    Health.RightLeg = GetHealthEndStatus(EFT.HealthSystem.EBodyPart.RightLeg);
                    Health.Stomach = GetHealthEndStatus(EFT.HealthSystem.EBodyPart.Stomach);
                }
            }
            #endregion
        }
        public class Main
        {
            public class G_Scene
            {
                public static Scene Game_Scene;
                private static string GetSceneName()
                {
                    return Game_Scene.name;
                }
                public static bool isActiveAndLoaded()
                {
                    return Game_Scene.isLoaded;
                }
                public static bool isInMatch()
                {
                    return GetSceneName() != "EnvironmentUIScene" &&
                            GetSceneName() != "MenuUIScene" &&
                            GetSceneName() != "CommonUIScene" &&
                            GetSceneName() != "MainScene" &&
                            GetSceneName() != "";
                }
                public static void SaveScene()
                {
                    Game_Scene = SceneManager.GetActiveScene();
                }
            }
            public static List<Player> _players;
            //public static List<WorldInteractiveObject> _doors;
            public static List<Throwable> _grenades;
            public static List<LootItem> _corpses;
            public static List<LootItem> _lootItems;
            public static List<ExfiltrationPoint> _exfils;
            public static Dictionary<GInterface163, EFT.GameWorld.GStruct65>.Enumerator _containers;

            public static Player _localPlayer;
            public static List<Player> tPlayer;
            //public static List<LootItem> tDoor;
            public static List<Throwable> tGrenades;
            public static List<LootItem> tCorpses;
            public static List<LootItem> tItems;
            public static List<ExfiltrationPoint> tExfils;

            public static GameObject GameObjectHolder;
            public static GameWorld _GameWorld = null;
            //public static List<LootItem> tContainers;
            public static void Clear()
            {
                _players = null;
                _grenades = null;
                _corpses = null;
                _lootItems = null;
                _exfils = null;
                //_containers = null;
                _localPlayer = null;
                tPlayer = null;
                tGrenades = null;
                tCorpses = null;
                tItems = null;
                tExfils = null;
                //tContainers = null;
            }
        }
        public class Buttons {
            public static bool Ma0c1 = false;
            public static bool Niger = false;
        }
        public class Bools
        {
            public static bool Draw_ESP = false;
            public static bool Draw_Doors = false; 
            public static bool Draw_Corpses = false;
            public static bool Draw_Grenades = false;
            public static bool Draw_Loot = false;
            public static bool Draw_Exfil = false;
            public static bool Draw_Containers = false;
            public static bool Draw_Crosshair = false;
            public static bool Draw_Crosshair2d = false;
            public static bool Display_HelpInfo = false;
            public static bool Switch_Colors = false;
            public static bool DisplayPlayerInfo = false;
            public static bool Spawn_FullBright = false;
            public static bool LOD_Controll = false;
            public static bool AimingAtNikita = false;
            public static bool Display_HUDGui = false;
            public static bool Recoil_Reducer = false;
            public static bool Aim_Smoothing = true;
            public static bool StreamerMode = false;
            public static bool SnapLines = false;
            public static bool ShowBones = false;
            public static bool IKnowWhatImDoing = false;
            public static bool ChangeSessionID = false;
            public static bool IamStrumer = false;
            public static bool NoVisorScreen = false;

            public static void SetToOff()
            {
                /*Draw_ESP = false;
                Draw_Corpses = false;
                Draw_Grenades = false;
                Draw_Loot = false;
                Draw_Crosshair = false;
                Display_HelpInfo = false;
                Switch_Colors = false;
                DisplayPlayerInfo = false;
                Spawn_FullBright = false;
                LOD_Controll = false;
                AimingAtNikita = false;
                Display_HUDGui = false;
                Recoil_Reducer = false;*/
            }
        }
        public class FullBright
        {
            public static GameObject lightGameObject;
            public static Light FullBrightLight;
            public static bool _LightEnabled = true;
            public static bool _LightCreated;
            public static bool lightCalled;
        }
        public class AliveCount
        {
            public static int All = 0;
            public static int dist_0_25 = 0;
            public static int dist_25_50 = 0;
            public static int dist_50_100 = 0;
            public static int dist_100_250 = 0;
            public static int dist_250_1000 = 0;
            public static void Reset()
            {
                All = 0;
                dist_0_25 = 0;
                dist_25_50 = 0;
                dist_50_100 = 0;
                dist_100_250 = 0;
                dist_250_1000 = 0;
            }
        }
        public static string LootSearcher = ""; // variable used for searches loop
        public static Vector3 AimPoint = Vector3.zero;
        public class ScreenWidth {
            public static int Full = Screen.width;
            public static int Half = (int)(Full / 2);
        }
        public class ScreenHeight {
            public static int Full = Screen.height;
            public static int Half = (int)(Full / 2);
        }
        public static string MyDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string[] HelpMenuTexts = new string[10] {
                "Help Menu: (Turn off/on 'Home' key)",
                "'Num 0' - E.S.P - Players",
                "'Num 1' - E.S.P - Corpses",
                "'Num 2' - PlayerInfo - Health, Alive Objects, etc.",
                "'Num 3' - Recoil 50%/100%",
                "'Num 4' - E.S.P - Grenades",
                "'Num 5' - Crosshair",
                "'Num 7' - E.S.P - Loot (kinda laggy) * dont use it all the time",
                "'Num 9' - Full Bright",
                "'Insert' - GUI Menu"
            };
    }
}
