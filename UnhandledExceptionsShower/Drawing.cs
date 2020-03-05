using UnityEngine;

namespace UnhandledException
{
    public static class Drawing
    {
        public class Special {
            public static void DrawPoint(float axis_x, float axis_y, float size, Color pixel_color) {
                Drawing.P(
                    new Vector2(axis_x,axis_y),
                    pixel_color,
                    size
                );
            }
            public static void DrawText(string text, float axis_x, float axis_y, Vector2 size, GUIStyle styleOfText, Color front_color) {
                Drawing.DrawShadow(
                    new Rect(axis_x, axis_y, size.x,size.y),
                    new GUIContent(text),
                    styleOfText,
                    front_color,
                    Constants.Colors.Black,
                    new Vector2(1f, 1f)
                );
            }
        }
        public static Texture2D lineTex;
        #region Crosshair
        public static void Crosshair()
        {
            Vector3 AimAtPoint = FUNC.W2S(Cons.AimPoint);
            P(new Vector2(AimAtPoint.x - 2f, Screen.height - AimAtPoint.y - 1f), new Color(0f, 0f, 0f), 4f);
            P(new Vector2(AimAtPoint.x - 1f, Screen.height - AimAtPoint.y - 1f), new Color(0.30f, 0.88f, 0.2196f), 2f);
        }
        public static void Crosshair2d()
        {
            Vector3 AimAtPoint = FUNC.W2S(Cons.AimPoint);
            P(new Vector2(Screen.width / 2f - 2f, Screen.height / 2f - 1f), new Color(0f, 0f, 0f), 4f);
            P(new Vector2(Screen.width / 2f - 1f, Screen.height / 2f - 1f), new Color(0.30f, 0.88f, 0.2196f), 2f);
        }
        #endregion
        #region DrawPixel
        public static void P(Vector2 Position, Color color, float thickness)
        {
            // Generate a single pixel texture if it doesn't exist
            if (!lineTex) { lineTex = new Texture2D(1, 1); }

            float yOffset = Mathf.Ceil(thickness / 2f);
            // Store current GUI color, so we can switch it back later,
            // and set the GUI color to the color parameter
            Color savedColor = GUI.color;
            GUI.color = color;

            GUI.DrawTexture(new Rect(Position.x, Position.y - (float)yOffset, thickness, thickness), lineTex);

            // We're done.  Restore the GUI color to whatever they were before.
            GUI.color = savedColor;
        }
        #endregion
        #region Drawing Shadowed Text
        public static void Text(Rect rect, string content) {
            Text(rect, content, Constants.Colors.White);
        }
        public static void Text(Rect rect, string content, Color txtColor)
        {
            DrawShadow(rect, new GUIContent(content), new GUIStyle(), txtColor, new Color(0f, 0f, 0f, 1f), new Vector2(1f, 1f));
        }
        public static void DrawShadow(Rect rect, GUIContent content, GUIStyle style, Color txtColor, Color shadowColor, Vector2 direction)
        {
            GUIStyle backupStyle = style;
            style.normal.textColor = shadowColor;
            rect.x += direction.x;
            rect.y += direction.y;
            GUI.Label(rect, content, style);
            style.normal.textColor = txtColor;
            rect.x -= direction.x;
            rect.y -= direction.y;
            GUI.Label(rect, content, style);
            style = backupStyle;
        }

        #endregion
        #region DrawLine - new with overloads

        public static void DrawLine(Rect rect) { DrawLine(rect, GUI.contentColor, 1.0f); }
        public static void DrawLine(Rect rect, Color color) { DrawLine(rect, color, 1.0f); }
        public static void DrawLine(Rect rect, float width) { DrawLine(rect, GUI.contentColor, width); }
        public static void DrawLine(Rect rect, Color color, float width) { DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.x + rect.width, rect.y + rect.height), color, width); }
        public static void DrawLine(Vector2 pointA, Vector2 pointB) { DrawLine(pointA, pointB, GUI.contentColor, 1.0f); }
        public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color) { DrawLine(pointA, pointB, color, 1.0f); }
        public static void DrawLine(Vector2 pointA, Vector2 pointB, float width) { DrawLine(pointA, pointB, GUI.contentColor, width); }
        public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
        {
            // Save the current GUI matrix, since we're going to make changes to it.
            Matrix4x4 matrix = GUI.matrix;

            // Generate a single pixel texture if it doesn't exist
            if (!lineTex) { lineTex = new Texture2D(1, 1); }

            // Store current GUI color, so we can switch it back later,
            // and set the GUI color to the color parameter
            Color savedColor = GUI.color;
            GUI.color = color;

            // Determine the angle of the line.
            float angle = Vector3.Angle(pointB - pointA, Vector2.right);

            // Vector3.Angle always returns a positive number.
            // If pointB is above pointA, then angle needs to be negative.
            if (pointA.y > pointB.y) { angle = -angle; }

            // Use ScaleAroundPivot to adjust the size of the line.
            // We could do this when we draw the texture, but by scaling it here we can use
            //  non-integer values for the width and length (such as sub 1 pixel widths).
            // Note that the pivot point is at +.5 from pointA.y, this is so that the width of the line
            //  is centered on the origin at pointA.
            GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));

            // Set the rotation for the line.
            //  The angle was calculated with pointA as the origin.
            GUIUtility.RotateAroundPivot(angle, pointA);

            // Finally, draw the actual line.
            // We're really only drawing a 1x1 texture from pointA.
            // The matrix operations done with ScaleAroundPivot and RotateAroundPivot will make this
            //  render with the proper width, length, and angle.
            GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1, 1), lineTex);

            // We're done.  Restore the GUI matrix and GUI color to whatever they were before.
            GUI.matrix = matrix;
            GUI.color = savedColor;
        }
        #endregion
        #region DrawBox
        public static void DrawBox(float x, float y, float w, float h, Color color)
        {
            DrawLine(new Vector2(x, y), new Vector2(x + w, y), color);
            DrawLine(new Vector2(x, y), new Vector2(x, y + h), color);
            DrawLine(new Vector2(x + w, y), new Vector2(x + w, y + h), color);
            DrawLine(new Vector2(x, y + h), new Vector2(x + w, y + h), color);
        }
        #endregion
        #region DrawCircle
        public static void Circle(int X, int Y, float radius)
        {
            //DrawBox(X - (radius / 2), Y - (radius / 2), radius, radius, Color.yellow);
        }
        #endregion
        #region ActionThings
        public static void CheckBox(ref bool variable, string name, int row = 1, int column = 0)
        {
            variable = GUI.Toggle(
                new Rect(
                    Constants.Locations.initialInputSizes.x + column * (Constants.Locations.initialInputSizes.x + Constants.Locations.boxSize.box_200),
                    Constants.Locations.initialInputSizes.y + (Constants.Locations.initialInputSizes.y * row),
                    Constants.Locations.boxSize.box_200,
                    Constants.Locations.boxSize.box_20
                ),
                variable,
                name
            );
        }
        public static void Button(ref bool variable, string name, int row = 1, int column = 0)
        {
            variable = GUI.Button(
                new Rect(
                    Constants.Locations.initialInputSizes.x + column * (Constants.Locations.initialInputSizes.x + Constants.Locations.boxSize.box_200) +25f,
                    Constants.Locations.initialInputSizes.y + (Constants.Locations.initialInputSizes.y * row),
                    Constants.Locations.boxSize.box_50,
                    Constants.Locations.boxSize.box_20
                ),
                name
            );
        }
        public static void Label(string name, int row = 1, int column = 0, float width = 0f, float height = 0f)
        {
            if (height == 0f)
                height = Constants.Locations.boxSize.box_20;
            if (width == 0f)
                width = Constants.Locations.boxSize.box_200;
            GUI.Label(
                new Rect(
                    Constants.Locations.initialInputSizes.x + column * (Constants.Locations.initialInputSizes.x + Constants.Locations.boxSize.box_200),
                    Constants.Locations.initialInputSizes.y + (Constants.Locations.initialInputSizes.y * row),
                    width,
                    height
                ),
                name
            );
        }
        public static void HorizontalSlider(ref float variable, float minimum = 1f, float maximum = 1000f, int row = 1, int column = 0)
        {
            variable = (int)GUI.HorizontalSlider(
                new Rect(
                    Constants.Locations.initialInputSizes.x + column * (Constants.Locations.initialInputSizes.x + Constants.Locations.boxSize.box_200),
                    Constants.Locations.initialInputSizes.y + (Constants.Locations.initialInputSizes.y * row),
                    Constants.Locations.boxSize.box_200,
                    Constants.Locations.boxSize.box_20
                ),
                variable,
                minimum,
                maximum
            );
        }
        public static void TextField(ref string variable, int row = 1, int column = 0)
        {
            variable = GUI.TextField(
                new Rect(
                    Constants.Locations.initialInputSizes.x + column * (Constants.Locations.initialInputSizes.x + Constants.Locations.boxSize.box_200), // this should give 0 and only input if not column was specified
                    Constants.Locations.initialInputSizes.y + (Constants.Locations.initialInputSizes.y * row),
                    Constants.Locations.boxSize.box_200,
                    Constants.Locations.boxSize.box_20
                ),
                variable
            );
        }
        /*
            GUI.Label(
            new Rect(initial.x, initial.y * 1, 
            Constants.Locations.boxSize.box_100, Constants.Locations.boxSize.box_20)
            , "FOV:" + Cons.Aim.AAN_FOV.ToString());

         
         */

        #endregion
    }
}
