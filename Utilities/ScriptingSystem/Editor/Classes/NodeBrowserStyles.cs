using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Radikon.ScriptingSystem.Editor
{
    public static class NodeBrowserStyles
    {
        /// <summary>
        /// A base style of 56,56,56.
        /// </summary>
        public static GUIStyle plainStyle;

        /// <summary>
        /// A style of 41,41,41.
        /// </summary>
        public static GUIStyle darkStyle;

        /// <summary>
        /// A style of 65, 65, 65.
        /// </summary>
        public static GUIStyle stormGreyStyle;

        /// <summary>
        /// A style of 56, 56, 56 with smaller, darker font.
        /// </summary>
        public static GUIStyle plainStyleB;

        /// <summary>
        /// Style for the idle state of a button.
        /// </summary>
        public static GUIStyle buttonNormal;

        /// <summary>
        /// Style for the idle state of a button that represents a selected object.
        /// </summary>
        public static GUIStyle buttonSelected;

        /// <summary>
        /// Style for the Search Bar Header
        /// </summary>
        public static GUIStyle searchbarStyle;

        /// <summary>
        /// Style for buttons in the Search Bar Header.
        /// </summary>
        public static GUIStyle searchbarButtonStyle;

        /// <summary>
        /// Style for the filter header.
        /// </summary>
        public static GUIStyle filterHeaderText;

        /// <summary>
        /// Style for the message that appears when no applicable nodes have been found.
        /// </summary>
        public static GUIStyle nodeSearcherNoNodesStyle;

        /// <summary>
        /// Style for a button in the node search grid.
        /// </summary>
        public static GUIStyle nodeSearcherGridButton;

        /// <summary>
        /// Style for a label in the node search grid.
        /// </summary>
        public static GUIStyle nodeSearcherGridLabel;

        /// <summary>
        /// Create all the styles for ConnectionServiceMonitor.
        /// </summary>
        /// <remarks>
        /// <see langword="Radikon.SignallingSystem.Editor"/>: Used during the creation of ConnectionServiceMonitor Styles.
        /// </remarks>
        public static void ConstructStyles()
        {
            plainStyle = CreateGUIStyle(TextAnchor.UpperLeft, FontStyle.Normal, 14, TextColor(192, 192, 192, 255), CreateTex(72, 72, 72, 255));
            plainStyleB = CreateGUIStyle(TextAnchor.LowerLeft, FontStyle.Normal, 11, TextColor(128, 128, 128, 255), CreateTex(72, 72, 72, 255));

            darkStyle = CreateGUIStyle(TextAnchor.MiddleLeft, FontStyle.Normal, 12, TextColor(192, 192, 192, 255), CreateTex(41, 41, 41, 255));
            stormGreyStyle = CreateGUIStyle(TextAnchor.MiddleLeft, FontStyle.Normal, 12, TextColor(192, 192, 192, 255), CreateTex(62, 62, 62, 255));

            searchbarStyle = CreateGUIStyle(TextAnchor.LowerLeft, FontStyle.Normal, 11, Color.white, CreateTex(60, 60, 60, 255));
            filterHeaderText = CreateGUIStyle(TextAnchor.MiddleLeft, FontStyle.Bold, 12, Color.white);

            searchbarButtonStyle = CreateGUIStyle(TextAnchor.MiddleLeft, FontStyle.Normal, 12,
                TextColor(192, 192, 192, 255), CreateTex(60, 60, 60, 255),
                Color.white, CreateTex(70, 70, 70, 255),
                Color.white, CreateTex(90, 90, 90, 255));

            buttonNormal = CreateGUIStyle(TextAnchor.MiddleLeft, FontStyle.Normal, 12,
                TextColor(192, 192, 192, 255), CreateTex(45, 45, 45, 255),
                Color.white, CreateTex(51, 51, 51, 255),
                Color.white, CreateTex(69, 122, 169, 255));

            buttonSelected = CreateGUIStyle(TextAnchor.MiddleLeft, FontStyle.Bold, 12,
                TextColor(224, 224, 224, 255), CreateTex(38, 80, 117, 255),
                Color.white, CreateTex(53, 106, 153, 255),
                Color.white, CreateTex(69, 122, 169, 255));

            nodeSearcherGridButton = CreateGUIStyle(TextAnchor.MiddleCenter, FontStyle.Normal, 12,
                TextColor(192, 192, 192, 255), CreateTex(51, 51, 51, 255),
                Color.white, CreateTex(61, 61, 61, 255),
                Color.white, CreateTex(81, 81, 81, 255));

            nodeSearcherNoNodesStyle = CreateGUIStyle(TextAnchor.MiddleCenter, FontStyle.Normal, 11, TextColor(128, 128, 128, 255), CreateTex(51, 51, 51, 255));
            nodeSearcherGridLabel = CreateGUIStyle(TextAnchor.MiddleCenter, FontStyle.Normal, 11, TextColor(128, 128, 128, 255), CreateTex(51, 51, 51, 255));
        }

        /// <summary>
        /// Create a basic GUIStyle with a normal text color 
        /// </summary>
        /// <returns></returns>
        private static GUIStyle CreateGUIStyle(Color normalTextColor, Texture2D normalBackground)
        {
            GUIStyle style = new GUIStyle();

            style.normal.textColor = normalTextColor;
            style.normal.background = normalBackground;

            return style;
        }

        /// <summary>
        /// Create a basic GUIStyle for text display
        /// </summary>
        /// <returns></returns>
        private static GUIStyle CreateGUIStyle(TextAnchor textAnchor, FontStyle fontStyle, int fontSize, Color textColor)
        {
            GUIStyle style = new GUIStyle();

            style.alignment = textAnchor;
            style.fontStyle = fontStyle;
            style.fontSize = fontSize;
            style.normal.textColor = textColor;

            return style;
        }

        /// <summary>
        /// Create a basic GUIStyle with a normal text color 
        /// </summary>
        /// <param name="normalTextColor"></param>
        /// <param name="texture"></param>
        /// <returns></returns>
        private static GUIStyle CreateGUIStyle(TextAnchor textAnchor, FontStyle fontStyle, int fontSize, Color normalTextColor, Texture2D normalBackground)
        {
            GUIStyle style = new GUIStyle();

            style.alignment = textAnchor;
            style.fontStyle = fontStyle;
            style.fontSize = fontSize;
            style.normal.textColor = normalTextColor;
            style.normal.background = normalBackground;

            return style;
        }

        /// <summary>
        /// Create a basic GUIStyle with multiple states.
        /// </summary>
        /// <param name="normalTextColor"></param>
        /// <param name="texture"></param>
        /// <returns></returns>
        private static GUIStyle CreateGUIStyle(TextAnchor textAnchor, FontStyle fontStyle, int fontSize, 
            Color normalTextColor, Texture2D normalBackground,
            Color hoverTextColor, Texture2D hoverBackground,
            Color activeTextColor, Texture2D activeBackground)
        {
            GUIStyle style = new GUIStyle();

            style.alignment = textAnchor;
            style.fontStyle = fontStyle;
            style.fontSize = fontSize;
            style.normal.textColor = normalTextColor;
            style.normal.background = normalBackground;
            style.hover.textColor = hoverTextColor;
            style.hover.background = hoverBackground;
            style.active.textColor = activeTextColor;
            style.active.background = activeBackground;

            return style;
        }

        /// <summary>
        /// Create a 1 by 1 pixel texture for a GUIStyle.
        /// </summary>
        /// <remarks>
        /// <see langword="Radikon.SignallingSystem.Editor"/>: Used during the creation of ConnectionServiceMonitor Styles.
        /// </remarks>
        /// <param name="r">Red Color Channel.</param>
        /// <param name="g">Green Color Channel.</param>
        /// <param name="b">Blue Color Channel</param>
        /// <param name="a">Alpha Transparency Channel</param>
        /// <returns><see cref="Texture2D"/></returns>
        private static Texture2D CreateTex(byte r, byte g, byte b, byte a)
        {
            Texture2D result = new Texture2D(1, 1);
            result.SetPixel(0, 0, new Color32(r, g, b, a));
            result.Apply();
            return result;
        }

        /// <summary>
        /// Shorthand for creating a Color32.
        /// </summary>
        /// <remarks>
        /// <see langword="Radikon.SignallingSystem.Editor"/>: Used during the creation of ConnectionServiceMonitor Styles.
        /// </remarks>
        /// <param name="r">Red Color Channel.</param>
        /// <param name="g">Green Color Channel.</param>
        /// <param name="b">Blue Color Channel</param>
        /// <param name="a">Alpha Transparency Channel</param>
        /// <returns></returns>
        private static Color32 TextColor(byte r, byte g, byte b, byte a) => new Color32(r, g, b, a);
    }


}
#endif