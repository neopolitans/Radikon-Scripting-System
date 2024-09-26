#if UNITY_EDITOR

namespace Radikon.ScriptingSystem.Editor
{
    using System.Text.RegularExpressions;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;

    [CustomEditor(typeof(ScriptingNode), true)]
    public class BaseNodeInspector : Editor
    {
        /// <summary>
        /// A value to change all the spacer sizings with.
        /// </summary>
        private static float imguiSpacerSize = 1.5f;

        /// <summary>
        /// The cached attribute for the inspector display name. <br/>
        /// Used if a custom display name should be used.
        /// </summary>
        private InspectorDisplayNameAttribute inspectorDisplayName;

        /// <summary>
        /// A list of fields and properties with conditional display attributes. <br/>
        /// </summary>
        Dictionary<string, ShowIfConditionAttribute> membersWithBoolDisplayConditions = new Dictionary<string, ShowIfConditionAttribute>();
        Dictionary<string, ShowIfMultiConditionAttribute> membersWithMultiBoolDisplayConditions = new Dictionary<string, ShowIfMultiConditionAttribute>();
        Dictionary<string, DontShowIfMultiConditionAttribute> membersWithMultiBoolDontDisplayConditions = new Dictionary<string, DontShowIfMultiConditionAttribute>();
        Dictionary<string, ShowIfEnumValueAttribute> membersWithEnumDisplayConditions = new Dictionary<string, ShowIfEnumValueAttribute>();
        
        // METHODS
        public void OnEnable()
        {
            FilterMemberInfoForAttributes();
        }

        // Cappuccino Editor Framework taught a lot more about Reflection than I expected to ever remember.
        /// <summary>
        /// Filter all member info for any fields or properties with conditional display attributes.
        /// </summary>
        private void FilterMemberInfoForAttributes()
        {
            // Clear the dictionaries if any residual data is still stored in it from the last time the editor was awake.
            membersWithBoolDisplayConditions.Clear();
            membersWithMultiBoolDisplayConditions.Clear();
            membersWithMultiBoolDontDisplayConditions.Clear();
            membersWithEnumDisplayConditions.Clear();

            // Get all members in the selected target.
            MemberInfo[] members = target.GetType().GetMembers();

            // If the member is a field or property, try get any relevant attributes from it.
            foreach (MemberInfo member in members)
            {
                if (member.MemberType == MemberTypes.Field || member.MemberType == MemberTypes.Property)
                {
                    // If there is a ShowIfConditionAttribute on the member, add it ot the list of members with bool display conditions.
                    ShowIfConditionAttribute singleConditionAttribute = member.GetCustomAttribute<ShowIfConditionAttribute>();
                    if (singleConditionAttribute != null)
                    {
                        membersWithBoolDisplayConditions.Add(member.Name, singleConditionAttribute);
                    }

                    // If there is a ShowIfMultiConditionAttribute on the member, add it ot the list of members with multiple bool display conditions.
                    ShowIfMultiConditionAttribute multiConditionAttribute = member.GetCustomAttribute<ShowIfMultiConditionAttribute>();
                    if (multiConditionAttribute != null)
                    {
                        membersWithMultiBoolDisplayConditions.Add(member.Name, multiConditionAttribute);
                    }

                    // If there is a ShowIfMultiConditionAttribute on the member, add it ot the list of members with multiple bool display conditions.
                    DontShowIfMultiConditionAttribute dontShowMultiConditionAttribute = member.GetCustomAttribute<DontShowIfMultiConditionAttribute>();
                    if (dontShowMultiConditionAttribute != null)
                    {
                        membersWithMultiBoolDontDisplayConditions.Add(member.Name, dontShowMultiConditionAttribute);
                    }

                    // If there is a ShowIfEnumValueAttribute on the member, add it ot the list of members with enum display conditions
                    ShowIfEnumValueAttribute showIfEnumAttribute = member.GetCustomAttribute<ShowIfEnumValueAttribute>();
                    if (showIfEnumAttribute != null)
                    {
                        membersWithEnumDisplayConditions.Add(member.Name, showIfEnumAttribute);
                    }
                }
            }
        }

        // - GUI Draw Methods

        public override void OnInspectorGUI()
        {
            NodeStyles.ConstructStyles();

            GUILayout.Space(8f);
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(NodeStyles.darkStyle);
            
                // Topbar
                GUILayout.BeginHorizontal();
                    GUILayout.Space(imguiSpacerSize);
                    TopBar();
                    GUILayout.Space(imguiSpacerSize);
                GUILayout.EndHorizontal();

                // Properties Bar
                GUILayout.BeginHorizontal();
                    GUILayout.Space(imguiSpacerSize);
                    GUILayout.BeginVertical(NodeStyles.stormGreyStyle);
                        DrawNodeProperties();
                        GUILayout.Space(8f);
                    GUILayout.EndVertical();
                    GUILayout.Space(imguiSpacerSize);
                GUILayout.EndHorizontal();

                GUILayout.Space(imguiSpacerSize);

                // Cap Bar
                CapBar();

                GUILayout.Space(imguiSpacerSize);
            GUILayout.EndVertical();
            GUILayout.Space(8f);
            GUILayout.EndHorizontal();

            GUILayout.Space(8f);
        }

        // - - Inspector GUI Drawing Components

        /// <summary>
        /// Draw the Scripting Node's top bar.
        /// </summary>
        private void TopBar()
        {
            string nodeName;

            // Try Get the Inspector Display Name attribute but only if it currently doesn't exist.
                    // TODO: INVESTIGATE PERFORMANCE - Might be performance intensive in editor.
            inspectorDisplayName ??= GetAttribute<InspectorDisplayNameAttribute>(target.GetType());

            if (inspectorDisplayName != null)
            {
                nodeName = inspectorDisplayName.displayName;
            }
            else
            {
                GetTypeData(target.GetType().ToString(), out nodeName, out string nodeNamespace);
                nodeName = SplitCamelCase(nodeName);
            }

            GetNodeDecoratorData(target, out string nodeCategory, out Texture2D nodeIcon);

            // Draw Topbar Icon
            GUILayout.BeginVertical(GUILayout.MaxWidth(36f), GUILayout.MaxHeight(36f));
                GUILayout.Space(imguiSpacerSize);
                GUILayout.Label(new GUIContent(nodeIcon), NodeStyles.plainStyle, GUILayout.Width(36f), GUILayout.Height(36f));
            GUILayout.EndVertical();

            // Draw Topbar Text
            GUILayout.BeginVertical();
                GUILayout.Space(imguiSpacerSize);

                // DRAW BASE CLASS NAME
                GUILayout.BeginVertical(NodeStyles.plainStyleB, GUILayout.Height(16f));
                    GUILayout.Label(nodeCategory, NodeStyles.plainStyleB);
                GUILayout.EndVertical();

                // DRAW OBJECT CLASS NAME
                GUILayout.BeginVertical(NodeStyles.plainStyle, GUILayout.Height(20f));
                    GUILayout.Label(nodeName, NodeStyles.plainStyle);
                GUILayout.EndVertical();
                GUILayout.Space(imguiSpacerSize);

            GUILayout.EndVertical();

        }

        /// <summary>
        /// Draw the properties of the scripting node.
        /// </summary>
        protected virtual void DrawNodeProperties()
        {
            serializedObject.Update();
            SerializedProperty iterator = serializedObject.GetIterator();

            iterator.NextVisible(true);
            do
            {
                if (iterator.name == "m_Script" && iterator.objectReferenceValue != null) continue;

                // Try find the property in either of the display condition dictioanries
                // and try determine if the property meet the provided display conditions to be shown.

                // otherwise, the display condition will show by default.

                if (membersWithBoolDisplayConditions.ContainsKey(iterator.name))
                {
                    if (MeetsBoolDisplayConditions(iterator)) EditorGUILayout.PropertyField(iterator);
                }
                else if (membersWithMultiBoolDisplayConditions.ContainsKey(iterator.name))
                {
                    if (MeetsMultiBoolDisplayConditions(iterator)) EditorGUILayout.PropertyField(iterator);
                }
                else if (membersWithMultiBoolDontDisplayConditions.ContainsKey(iterator.name))
                {
                    if (MeetsConditionsToAvoidDontDisplay(iterator)) EditorGUILayout.PropertyField(iterator);
                }
                else if (membersWithEnumDisplayConditions.ContainsKey(iterator.name))
                {
                    if (MeetsEnumDisplayConditions(iterator)) EditorGUILayout.PropertyField(iterator);
                }
                else
                {
                    EditorGUILayout.PropertyField(iterator);
                }
            }
            while (iterator.NextVisible(false));

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Draw the cap bar at the end of the scripting node view.
        /// </summary>
        private void CapBar()
        {
            GUILayout.BeginHorizontal();
                GUILayout.Space(imguiSpacerSize);
                GUILayout.BeginVertical(NodeStyles.plainStyle);
                    GUILayout.Space(12f);
                GUILayout.EndVertical();
                GUILayout.Space(imguiSpacerSize);
            GUILayout.EndHorizontal();
        }

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

        // Helper Methods

        /// <summary>
        /// Does the provided serialized property meet the conditions to be shown? <br/>
        /// Variant that parses for ShowIfConditionAttribute and tries to see if the bool values match.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected bool MeetsBoolDisplayConditions(SerializedProperty property)
        {
            // Get the attribute and try find the SerializedProperty referenced by attribute.propertyName.
            ShowIfConditionAttribute attrib = membersWithBoolDisplayConditions[property.name];
            SerializedProperty other = serializedObject.FindProperty(attrib.propertyName);

            // If that property is a bool type and it's value matches the condition show it. Otherwise, don't.
            // However, if a property doesn't exist or isn't a bool type, show the property as a fail-safe.
            if (other != null && other.type == "bool")
            {
                if (other.boolValue == attrib.condition) return true;
                else return false;
            }
            else return true;
        }

        /// <summary>
        /// Does the provided serialized property meet the conditions to be shown? <br/>
        /// Variant that parses for ShowIfMultiConditionAttribute and tries to see if all other properties' values match up with the required bool values.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected bool MeetsMultiBoolDisplayConditions(SerializedProperty property)
        {
            // Get the attribute and try find the SerializedProperty referenced by attribute.propertyName.
            ShowIfMultiConditionAttribute attrib = membersWithMultiBoolDisplayConditions[property.name];
            bool allConditionsMet = true;

            // For each property value, find out if all conditional property values are true.
            foreach ((string propertyName, bool condition) propertyValue in attrib.propertyConditionsList)
            {
                SerializedProperty other = serializedObject.FindProperty(propertyValue.propertyName);

                // If that property is a bool type and it's value matches the condition show it. Otherwise, don't.
                // However, if a property doesn't exist or isn't a bool type, show the property as a fail-safe.
                if (other != null && other.type == "bool")
                {
                    // Only set allConditionsMet as true if the previous value and this condition are both true.
                    // Otherwise, it stays false.
                    if (other.boolValue == propertyValue.condition) allConditionsMet = allConditionsMet && true;
                    else allConditionsMet = false;
                }
            }

            return allConditionsMet;
        }

        /// <summary>
        /// Does the provided serialized property meet the conditions to be shown? <br/>
        /// Variant that parses for DontShowIfMultiConditionAttribute and tries to see if all other properties' values match up with the required bool values. <br/>
        /// This is an inversion of ShowIfMultiConditionAttribute.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected bool MeetsConditionsToAvoidDontDisplay(SerializedProperty property)
        {
            // Get the attribute and try find the SerializedProperty referenced by attribute.propertyName.
            DontShowIfMultiConditionAttribute attrib = membersWithMultiBoolDontDisplayConditions[property.name];
            bool allConditionsMet = true;

            // For each property value, find out if all conditional property values are true.
            foreach ((string propertyName, bool condition) propertyValue in attrib.propertyConditionsList)
            {
                SerializedProperty other = serializedObject.FindProperty(propertyValue.propertyName);

                // If that property is a bool type and it's value matches the condition show it. Otherwise, don't.
                // However, if a property doesn't exist or isn't a bool type, show the property as a fail-safe.
                if (other != null && other.type == "bool")
                {
                    // Only set allConditionsMet as true if the previous value and this condition are both true.
                    // Otherwise, it stays false.
                    if (other.boolValue == propertyValue.condition) allConditionsMet = allConditionsMet && true;
                    else allConditionsMet = false;
                }
            }

            // Invert depending on the result obtained.
            return !allConditionsMet;
        }

        /// <summary>
        /// Does the provided serialized property meet the conditions to be shown? <br/>
        /// Variant that parses for ShowIfEnumValueAttribute and tries to see if enum values match.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected bool MeetsEnumDisplayConditions(SerializedProperty property)
        {
            // get the attribute and try find the SerializedProperty referenced by attribute.propertyName.
            ShowIfEnumValueAttribute attrib = membersWithEnumDisplayConditions[property.name];
            SerializedProperty other = serializedObject.FindProperty(attrib.propertyName);

            // If that property is a enum type and it's value matches the condition show it. Otherwise, don't.
            // However, if a property doesn't exist or isn't an enum type, show the property as a fail-safe.
            if (other != null && (other.type.ToLower() == "enum"))
            {
                if (other.intValue == attrib.enumValueAsInt) return true;
                else return false;
            }
            else return true;
        }

        /// <summary>
        /// Using Attribute.
        /// </summary>
        /// <param name="name">The resulting name.</param>
        /// <param name="icon">The resulting icon.</param>
        protected static void GetNodeDecoratorData(Object unityObj, out string name, out Texture2D icon)
        {
            // Default assignments to satisfy return conditions.
            name = "Invalid Type";
            icon = Resources.Load<Texture2D>("ScriptingSystemIcons/InvalidNodeIcon");

            // Attempt to find the attached Attribute Data to the class.
            if (unityObj.GetType().IsSubclassOf(typeof(ScriptingNode)))
            {
                ScriptingNode node = (ScriptingNode)unityObj;
                NodeTypeAttribute nodeType = (NodeTypeAttribute)System.Attribute.GetCustomAttribute(unityObj.GetType(), typeof(NodeTypeAttribute));

                if (nodeType != null)
                {
                    name = nodeType.nodeCategory;
                    icon = Resources.Load<Texture2D>(nodeType.nodeIconDirectory);
                }
                else
                {   
                    // This is a fallback.
                    name = "Custom Node";
                    icon = Resources.Load<Texture2D>($"ScriptingSystemIcons/CustomNodeIcon");
                }
            }
        }

        /// <summary>
        /// Get the type data from a string that contains the full path of an object.
        /// </summary>
        /// <param name="objectFullType">The full path of the object type.</param>
        /// <param name="objectType">The name of the type itself, isolated from the full path.</param>
        /// <param name="objectNamespace">The namespace the type is in, isolated from the full path.</param>
        /// <returns></returns>
        protected static void GetTypeData(string objectFullType, out string objectType, out string objectNamespace)
        {
            string[] typeData = objectFullType.Split('.');

            objectType = typeData[typeData.Length - 1];

            if (typeData.Length <= 1) objectNamespace = "Global";
            else objectNamespace = objectFullType.Substring(0, objectFullType.Length - (typeData[typeData.Length - 1].Length + 1));
        }

        // Utilities
        /// <summary>
        /// Get a list of the attribue type attached to a given object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static T[] GetAttributeList<T>(System.Type objectType) where T : System.Attribute
        {
            return (T[])System.Attribute.GetCustomAttributes(objectType, typeof(T));
        }

        /// <summary>
        /// Get the first available attribute type attached to a given object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(System.Type objectType) where T : System.Attribute
        {
            return (T)System.Attribute.GetCustomAttribute(objectType, typeof(T));
        }
    }
}
#endif