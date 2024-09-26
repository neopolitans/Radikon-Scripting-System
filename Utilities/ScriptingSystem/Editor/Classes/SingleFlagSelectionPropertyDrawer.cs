using System;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

namespace Radikon.Attributes.Editor
{
    /// <summary>
    /// A port of Schaik, J.'s (2022) Single-Flag Enum Selection attribute drawer with slight changes. <br/>
    /// It can now <b>only</b> be applied on enum types.
    /// </summary>
    /// <remarks>
    /// Source: <see href="https://localjoost.github.io/Making-only-one-entry-of-a-Flag-Enum-selectable-in-the-Unity-Editor/"/>
    /// </remarks>
    [CustomPropertyDrawer(typeof(SingleFlagSelectionAttribute))]
    public class SingleFlagSelectionPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SingleFlagSelectionAttribute attrib = (SingleFlagSelectionAttribute)attribute;

            if (attrib.isValid)
            {
                // Converted from lists to System.Array as enums are fixed in their enum size.
                // They can't change without a full recompile of the scripting so this should be fine, and a bit more performant.
                // Less LINQ-behaviour too!

                Array arr = Enum.GetValues(attrib.enumType);
                GUIContent[] displayContent = new GUIContent[arr.Length];
                int[] intValues = new int[arr.Length];

                for (int i = 0; i < arr.Length; i++)
                {
                    var arrval = arr.GetValue(i);
                    displayContent[i] = new GUIContent(SplitCamelCase(arrval.ToString()));
                    intValues[i] = (int)arrval;
                }

                property.intValue = EditorGUI.IntPopup(position, label, property.intValue, displayContent, intValues);
            }
            else
            {
                return;
            }
        }

        // [COPIED FROM BASENODEINSEPCTOR]
        // From a very insightful post from 2005 (funnily enough). Source: https://weblogs.asp.net/jongalloway/426087
        // With the changes in processor technology & .NET, this should be inconsequential for only a few uses every inspector update frame.
        /// <summary>
        /// Split a string containing words visually distinguished with camel-casing while keeping acronyms intact. 
        /// </summary>
        /// <remarks>
        /// Thanks to Galloway, J. (2005) &amp; Heidt, J. (2007)
        /// </remarks>
        /// <param name="s"></param>
        /// <returns></returns>
        protected static string SplitCamelCase(string s) => Regex.Replace(s, "([A-Z][A-Z]*)", " $1", RegexOptions.Compiled).Trim();
    }

}