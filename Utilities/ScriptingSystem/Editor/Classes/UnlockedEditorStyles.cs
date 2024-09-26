using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

namespace Radikon.Editor
{
    using UnityEditor;

    // NOTICE: I wanted to properly decorate a custom search field because IMGUI.Controls.SearchField does not properly work.
    //         This class exists to enhance functionality of projects using UNITY ENGINE and meet the DESIGN GUIDELINES set
    //         by UNITY TECHNOLOGIES with their EDITOR FOUNDATIONS DESIGN SYSTEM page. This is otherwise not possible as some
    //         GUIStyle objects are still marked as internal to the UnityEditor.CoreModule assembly.

    //         NOT INTENDED FOR PUBLIC USE

    /// <summary>
    /// A class containing directly ported styles that are otherwise marked as internal in UnityEditor.EditorStyles. <br/>
    /// Please open up this class further, Unity. Having to use your engine's C# reference isn't fun.
    /// </summary>
    public static class UnlockedEditorStyles
    {
        // Styles
        // - PUBLIC
        /// <summary>
        /// The style for the search field cancel button, if there's a search term.
        /// </summary>
        public static GUIStyle SearchFieldCancelButton
        {
            get
            {
                return m_searchFieldCancelButton ??= GetStyle("SearchCancelButton");
            }
        }

        /// <summary>
        /// The style for the search field cancel button, if empty.
        /// </summary>
        public static GUIStyle SearchFieldCancelButtonEmpty
        {
            get
            {
                return m_searchFieldCancelButtonEmpty ??= GetStyle("SearchCancelButtonEmpty");
            }
        }

        // - PRIVATE
        private static GUIStyle m_searchFieldCancelButtonEmpty;
        private static GUIStyle m_searchFieldCancelButton;

        // Methods
        /// <summary>
        /// Get the GUIStyle by the given name.
        /// </summary>
        /// <param name="styleName"></param>
        /// <returns></returns>
        public static GUIStyle GetStyle(string styleName)
        {
            // Try find the given style in the current GUI skin, otherwise use the EditorGUIUtility.
            GUIStyle guiStyle = GUI.skin.FindStyle(styleName) ?? EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle(styleName);

            if (guiStyle == null)
            {
                guiStyle = new GUIStyle();
                guiStyle.name = "StyleNotFondError";
                Debug.LogWarning("[UNLOCKEDEDITORSTYLES] Missing built-in guistyle " + styleName);
            }

            return guiStyle;
        }
    }

}

#endif