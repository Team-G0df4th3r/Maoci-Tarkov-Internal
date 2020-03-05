using EFT;
using EFT.Interactive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnhandledException
{
    class Doors
    {
        public class Draw {
            public static void Doors()
            {
                /*if (Cons.Main._doors != null)
                    return;
                var LabelSize = new GUIStyle { fontSize = 12 };
                float deltaDistance = 25f;
                float devLabel = 1f;
                var e = Cons.Main._doors.GetEnumerator();
                while (e.MoveNext())
                {
                    try {
                        var item = e.Current;
                        if (item != null)
                        {
                            if (FUNC.isInScreenRestricted(FUNC.W2S(item.transform.position)))
                            { // do not display out of bounds items
                                float distance = FastMath.FD(Camera.main.transform.position, item.transform.position);
                                if (distance < 1000f)
                                {
                                    Vector3 itemPosition = FUNC.W2S(item.transform.position);
                                    float[] boxSize = new float[2] { 3f, 1.5f };
                                    int FontSize = 12;
                                    FastMath.DistSizer(distance, ref FontSize, ref deltaDistance, ref devLabel);
                                    LabelSize.fontSize = FontSize;
                                    LabelSize.normal.textColor = new Color(.7f, .7f, .7f, .8f);
                                    string Text = $"<<door>>";
                                    Vector2 sizeOfText = GUI.skin.GetStyle(Text).CalcSize(new GUIContent(Text));
                                    Drawing.Special.DrawPoint(
                                        itemPosition.x - boxSize[1],
                                        (float)(Screen.height - itemPosition.y) - boxSize[1],
                                        boxSize[0],
                                        Color.yellow
                                    );
                                    Drawing.Special.DrawText(
                                        Text,
                                        itemPosition.x - sizeOfText.x / 2f,
                                        (float)(Screen.height - itemPosition.y) - deltaDistance - 1,
                                        sizeOfText,
                                        LabelSize,
                                        Color.yellow
                                    );
                                }
                            }
                        }
                    }
                    catch (NullReferenceException ex)
                    {
                        ErrorHandler.Catch("Doors", ex);
                    }
                }*/
            }
        }
        public class Update
        {
            private static float timestamp = 0;
            private static float pertick = 1;
            public static void Doors()
            {
                /*if (timestamp < Time.time)
                {
                    try
                    {
                        WorldInteractiveObject[] temp = UnityEngine.Object.FindObjectsOfType<WorldInteractiveObject>();
                        for (int i = 0; i < temp.Length; i++)
                        {
                            Cons.Main._doors.Add(temp[i]);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        ErrorHandler.Catch("Get_Doors", e);
                    }
                    timestamp = Time.time + pertick;
                }*/
            }
        }
    }
}
