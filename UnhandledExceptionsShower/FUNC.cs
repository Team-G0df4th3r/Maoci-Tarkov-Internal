using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFT;
using UnityEngine;

namespace UnhandledException
{
    class FUNC
    {
        public static bool isInScreenRestricted(Vector3 V)
        {
            if (V.x > 0.01f &&
                V.y > 0.01f &&
                V.x < Cons.ScreenWidth.Full &&
                V.y < Cons.ScreenHeight.Full &&
                V.z > 0.01f)
                return true;
            return false;
        }
        public static bool isInScreenYZ(Vector3 V)
        {
            // properly display snap lines :)
            if (V.y > 0.01f &&
                V.y < (Cons.ScreenHeight.Full - 5f) &&
                V.z > 0.01f)
                return true;
            return false;
        }
        public static Vector3 W2S(Vector3 V) { return Camera.main.WorldToScreenPoint(V); } 
        public class Bones {

            public enum BodyPart
            {
                Pelvis = 14,
                LThigh1 = 15,
                LThigh2 = 16,
                LCalf = 17,
                LFoot = 18,
                LToe = 19,
                RThigh1 = 20,
                RThigh2 = 21,
                RCalf = 22,
                RFoot = 23,
                RToe = 24,
                Bear_Feet = 25,
                USEC_Feet = 26,
                BEAR_feet_1 = 27,
                Spine1 = 29,
                Gear1 = 30,
                Gear2 = 31,
                Gear3 = 32,
                Gear4 = 33,
                Gear4_1 = 34,
                Gear5 = 35,
                Spine2 = 36,
                Spine3 = 37,
                Ribcage = 66,
                LCollarbone = 89,
                LUpperarm = 90,
                LForearm1 = 91,
                LForearm2 = 92,
                LForearm3 = 93,
                LPalm = 94,
                RUpperarm = 111,
                RForearm1 = 112,
                RForearm2 = 113,
                RForearm3 = 114,
                RPalm = 115,
                Neck = 132,
                Head = 133
            }
            public static Vector3 SkeletonBonePos(Diz.Skinning.Skeleton sko, int id)
            {
                return sko.Bones.ElementAt(id).Value.position;
            }
            public static string SkeletonBoneName(Diz.Skinning.Skeleton sko, int id)
            {
                //most likely used for dumping
                return sko.Bones.ElementAt(id).Key.ToString();
            }
            public static Vector3 GetBonePosByID(Player p, int id)
            {
                Vector3 result;
                if (p == null)
                    return Vector3.zero;
                try
                {
                    result = SkeletonBonePos(p.PlayerBones.AnimatedTransform.Original.gameObject.GetComponent<PlayerBody>().SkeletonRootJoint, id);
                }
                catch (Exception)
                {
                    result = Vector3.zero;
                }
                return result;
            }
        }
        public class Update {
            #region [HOTKEYS]
            public static void Hotkeys()
            {
                #region Draw non sensitive data
                if (Input.GetKeyUp(KeyCode.Keypad5))
                {
                    Cons.Bools.Draw_Crosshair = !Cons.Bools.Draw_Crosshair;
                }
                if (Input.GetKeyUp(KeyCode.Keypad2))
                {
                    Cons.Bools.DisplayPlayerInfo = !Cons.Bools.DisplayPlayerInfo;
                }
                if (Input.GetKeyUp(KeyCode.Home))
                {
                    Cons.Bools.Display_HelpInfo = !Cons.Bools.Display_HelpInfo;
                }
                if (Input.GetKeyDown(KeyCode.Insert))
                {
                    Cons.Bools.Display_HUDGui = !Cons.Bools.Display_HUDGui;
                }
                #endregion

                #region Draw sensitive data - no errors allowed here
                if (Input.GetKeyUp(KeyCode.Keypad0))
                {
                    Cons.Bools.Draw_ESP = !Cons.Bools.Draw_ESP;
                }
                if (Input.GetKeyUp(KeyCode.Keypad1))
                {
                    Cons.Bools.Draw_Corpses = !Cons.Bools.Draw_Corpses;
                }
                if (Input.GetKeyUp(KeyCode.Keypad7))
                {
                    Cons.Bools.Draw_Loot = !Cons.Bools.Draw_Loot;
                }
                if (Input.GetKeyUp(KeyCode.Keypad4))
                {
                    Cons.Bools.Draw_Grenades = !Cons.Bools.Draw_Grenades;
                }
                if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    Cons.Bools.Recoil_Reducer = !Cons.Bools.Recoil_Reducer;
                }
                if (Input.GetKeyDown(KeyCode.Keypad9))
                {
                    Cons.Bools.Spawn_FullBright = !Cons.Bools.Spawn_FullBright;
                }
                if (Input.GetKeyDown(KeyCode.Mouse3)) // You can change it or create a GUI for change it in game
                {
                    Cons.Bools.AimingAtNikita = !Cons.Bools.AimingAtNikita;
                }
                if (Cons.Bools.IKnowWhatImDoing)
                {
                    if (Input.GetKeyDown(KeyCode.F10))
                    {
                        if (Input.GetKeyDown(KeyCode.F10) && Cons.Main._localPlayer != null)
                        {
                            // we can add an prevention to too fast teleporting adding like once per second etc.
                            //if (Time.time >= _secTime) {
                            //_secTime = Time.time + 1f;
                            Cons.Main._localPlayer.Transform.position = Cons.Main._localPlayer.Transform.position + Camera.main.transform.forward * Cons.Distances.Teleport;
                            //}
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.F11))
                    {
                        // simple FOV changer
                        GClass436.SetFov(120f, 1f);
                        GClass436.ApplyFoV(120, 100, 120);
                    }
                }
                #endregion
                /* unused yet
                if (Input.GetKeyDown(KeyCode.Keypad9))
                {
                    Debug.unityLogger.logEnabled = !Debug.unityLogger.logEnabled;
                }
                */
            }
            #endregion
            #region [BUTTONS]
            public static void Buttons()
            {
                if (Cons.Buttons.Ma0c1)
                {
                    //Cons.Bools.Recoil_Reducer = true;
                    Cons.Bools.Draw_ESP = true;
                    Cons.Bools.Draw_Corpses = true;
                    Cons.Bools.Draw_Grenades = true;
                    Cons.Bools.Draw_Crosshair = true;
                    Cons.Bools.Spawn_FullBright = true;
                    Cons.Bools.AimingAtNikita = true;
                    Cons.Bools.Aim_Smoothing = true;
                    Cons.Bools.SnapLines = true;
                    Cons.Bools.ShowBones = true;
                    Cons.Bools.NoVisorScreen = true;
                    Cons.Buttons.Ma0c1 = false;
                }
                if (Cons.Buttons.Niger)
                {
                    // not used yet :)
                    Cons.Buttons.Niger = false;
                }
            }
            #endregion
            #region [FULL.BRIGHT]
            public static void FullBright()
            {
                if (Cons.Main._localPlayer != null)
                {
                    if (Cons.Bools.Spawn_FullBright)
                    {
                        Vector3 playerPos = Cons.Main._localPlayer.Transform.position;
                        playerPos.y += 1f;

                        Cons.FullBright.lightGameObject.transform.position = playerPos;
                        Cons.FullBright.FullBrightLight.range = 1000f;
                        Cons.FullBright.FullBrightLight.intensity = 0.4f;
                    }
                    else
                    {
                        GameObject.Destroy(Cons.FullBright.FullBrightLight);
                        Cons.FullBright.lightCalled = false;
                    }
                }
            }
            #endregion
            #region [REC.REDUCER]
            public static void RecoilReducer()
            {
                // old method of reducing recoil - still working btw ... not bannable tho
                if (Cons.Main._localPlayer != null)
                    if (Cons.Bools.Recoil_Reducer)
                    {
                        if (Cons.Main._localPlayer.ProceduralWeaponAnimation.Shootingg.Intensity != 0.5f)
                            Cons.Main._localPlayer.ProceduralWeaponAnimation.Shootingg.Intensity = 0.5f;
                    }
                    else if (Cons.Main._localPlayer.ProceduralWeaponAnimation.Shootingg.Intensity != 1.0f)
                    {
                        Cons.Main._localPlayer.ProceduralWeaponAnimation.Shootingg.Intensity = 1.0f;
                    }
            }
            #endregion
            #region [FUNCTION] - LOD Controller // TODO Not working - need to find work around
            /*public LODGroup group;
            private void SetLODToLow()
            {
                if (Bools.LOD_Controll)
                {
                    group.ForceLOD(6);
                }
                else 
                {
                    group.ForceLOD(0);
                }
            }*/
            #endregion
        }
        public class OnGUI {
            public static void FullBright()
            {
                if (!Cons.FullBright.lightCalled)
                {
                    Cons.FullBright.lightGameObject = new GameObject("Fullbright");
                    Cons.FullBright.FullBrightLight = Cons.FullBright.lightGameObject.AddComponent<Light>();
                    Cons.FullBright.FullBrightLight.color = new Color(1f, 0.839f, 0.66f, 1f);
                    Cons.FullBright.lightCalled = true;
                }
            }
        }


    }
}
