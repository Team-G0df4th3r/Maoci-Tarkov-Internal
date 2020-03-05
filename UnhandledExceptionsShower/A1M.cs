using System;
using UnityEngine;
using EFT;

namespace UnhandledException
{
    class A1M
    {
        public class Draw { // yes its used in draw bo be more accurate
            public static void Aimbot()
            {
                try
                {
                    Vector3 AimAtGuy = Vector3.zero;
                    float distanceOfTarget = 9999f;
                    foreach (Player player in Cons.Main._players)
                    {
                        if (player != null && player != Cons.Main._localPlayer && player.HealthController != null)
                        {
                            if (!Cons.LocalPlayer.isInYourGroup(player))
                            {
                                Vector3 vector = FUNC.Bones.GetBonePosByID(player, (int)FUNC.Bones.BodyPart.Head);
                                float dist = FastMath.FD(Camera.main.transform.position, player.Transform.position);
                                if (dist > Cons.Distances.Aim)
                                    continue;
                                if (vector != Vector3.zero && Calculations.CalcInFov(vector) <= Cons.Aim.AAN_FOV/* && IsVisible(player.gameObject, getBonePos(player))*/)
                                {
                                    if (distanceOfTarget > dist)
                                    {
                                        distanceOfTarget = dist;

                                        // bulletspeed is in meters/second like distance is on meters
                                        float travelTime = dist / Cons.Main._localPlayer.Weapon.CurrentAmmoTemplate.InitialSpeed;
                                        vector.x += (player.Velocity.x * travelTime);
                                        vector.y += (player.Velocity.y * travelTime);

                                        AimAtGuy = vector;
                                    }
                                }
                            }
                        }
                    }
                    if (AimAtGuy != Vector3.zero)
                    {
                        Calculations.AimAtPos(AimAtGuy);
                        Drawing.DrawBox(FUNC.W2S(AimAtGuy).x - 5f, FUNC.W2S(AimAtGuy).y - 5f, 10f, 10f, Color.blue);
                    }
                }
                catch (Exception e)
                {
                    ErrorHandler.Catch("Draw_AiBo", e);
                }
            }
        }
        public class Calculations {
            public static float CalcInFov(Vector3 Position)
            {
                Vector3 position = Camera.main.transform.position;
                Vector3 forward = Camera.main.transform.forward;
                Vector3 normalized = (Position - position).normalized;
                return Mathf.Acos(Mathf.Clamp(Vector3.Dot(forward, normalized), -1f, 1f)) * 57.29578f;
            }
            public static void AimAtPos(Vector3 pos)
            {
                Vector2 rotation = Cons.Main._localPlayer.MovementContext.Rotation;
                Vector3 b = Raycast.GetHandsPos();
                Vector3 eulerAngles = Quaternion.LookRotation((pos - b).normalized).eulerAngles;
                if (eulerAngles.x > 180f)
                {
                    eulerAngles.x -= 360f;
                }
                Cons.Main._localPlayer.MovementContext.Rotation = new Vector2(eulerAngles.y, eulerAngles.x);
            }
        }
    }
}
