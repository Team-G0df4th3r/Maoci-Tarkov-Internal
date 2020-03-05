using EFT;
using System;
using UnityEngine;

namespace UnhandledException
{
    class Raycast
    {
        /* I added overloaded function to make sure we do not overcalc this shit :)
         *
         * */
        private static RaycastHit raycastHit;
        private static int mask = 1 << 12 | 1 << 18;
        public static Vector3 GetHandsPos()
        {
            if (Cons.Main._localPlayer == null)
            {
                return Vector3.zero;
            }
            return Cons.Main._localPlayer.Fireport.position - Cons.Main._localPlayer.Fireport.up * 1f; //fireport 
        }
        public static Vector3 GetShootPos()
        {
            if (Cons.Main._localPlayer == null)
            {
                return Vector3.zero;
            }
            Player.FirearmController firearmController = Cons.Main._localPlayer.HandsController as Player.FirearmController;
            if (firearmController == null)
            {
                return Vector3.zero;
            }
            return firearmController.Fireport.position + Camera.main.transform.forward * 1f;
        }
        public static Vector3 BarrelRaycast() {
            if (Cons.Main._localPlayer.Fireport == null)
                return Vector3.zero;
            Physics.Linecast(Cons.Main._localPlayer.Fireport.position, Cons.Main._localPlayer.Fireport.position - Cons.Main._localPlayer.Fireport.up * Mathf.Infinity, out raycastHit, mask);
            return raycastHit.point;
        }
        #region IsVisible - Properly done with mask
        private static bool IsVisible(GameObject obj, Vector3 Position)
        {
            if (Cons.Main._localPlayer == null)
                return false;
            RaycastHit raycastHit;
            return Physics.Linecast(GetShootPos(), Position, out raycastHit, mask) && raycastHit.collider && raycastHit.collider.gameObject.transform.root.gameObject == obj.transform.root.gameObject;
        }
        #endregion
        private static bool CastLine(Vector3 start, Vector3 end, GameObject obj) {
            return Physics.Linecast(start, end, out raycastHit, mask) && raycastHit.collider && raycastHit.collider.gameObject.transform.root.gameObject == obj.transform.root.gameObject;
        }  
        #region BodyRaycastCheck() - 5 overloads
        public static bool BodyRaycastCheck(GameObject obj, Vector3 Vector_1, Vector3 Vector_2, Vector3 Vector_3, Vector3 Vector_4, Vector3 Vector_5)
        {
            var HandsPos = GetShootPos();
            if (Vector_1 != null)
                if (CastLine(HandsPos, Vector_1, obj))
                {
                    return true;
                }
            if (Vector_2 != null)
                if (CastLine(HandsPos, Vector_2, obj))
                {
                    return true;
                }
            if (Vector_3 != null)
                if (CastLine(HandsPos, Vector_3, obj))
                {
                    return true;
                }
            if (Vector_4 != null)
                if (CastLine(HandsPos, Vector_4, obj))
                {
                    return true;
                }
            if (Vector_5 != null)
                if (CastLine(HandsPos, Vector_5, obj))
                {
                    return true;
                }
            return false;
        }
        public static bool BodyRaycastCheck(GameObject obj, Vector3 Vector_1, Vector3 Vector_2, Vector3 Vector_3, Vector3 Vector_4)
        {
            var HandsPos = GetShootPos();
            if (Vector_1 != null)
                if (CastLine(HandsPos, Vector_1, obj))
                {
                    return true;
                }
            if (Vector_2 != null)
                if (CastLine(HandsPos, Vector_2, obj))
                {
                    return true;
                }
            if (Vector_3 != null)
                if (CastLine(HandsPos, Vector_3, obj))
                {
                    return true;
                }
            if (Vector_4 != null)
                if (CastLine(HandsPos, Vector_4, obj))
                {
                    return true;
                }
            return false;
        }
        public static bool BodyRaycastCheck(GameObject obj, Vector3 Vector_1, Vector3 Vector_2, Vector3 Vector_3)
        {
            var HandsPos = GetShootPos();
            if (Vector_1 != null)
                if (CastLine(HandsPos, Vector_1, obj))
                {
                    return true;
                }
            if (Vector_2 != null)
                if (CastLine(HandsPos, Vector_2, obj))
                {
                    return true;
                }
            if (Vector_3 != null)
                if (CastLine(HandsPos, Vector_3, obj))
                {
                    return true;
                }
            return false;
        }
        public static bool BodyRaycastCheck(GameObject obj, Vector3 Vector_1, Vector3 Vector_2)
        {
            var HandsPos = GetShootPos();
            if (Vector_1 != null)
                if (CastLine(HandsPos, Vector_1, obj))
                {
                    return true;
                }
            if (Vector_2 != null)
                if (CastLine(HandsPos, Vector_2, obj))
                {
                    return true;
                }
            return false;
        }
        public static bool BodyRaycastCheck(GameObject obj, Vector3 Vector_1)
        {
            if (Vector_1 != null)
                if (CastLine(GetShootPos(), Vector_1, obj))
                {
                    return true;
                }
            return false;
        }
        #endregion
    }
}
