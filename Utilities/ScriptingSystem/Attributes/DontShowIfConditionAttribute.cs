using System;
using System.Collections.Generic;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// An attribute for whether to not show a property or field in the inspector if other property values meet certain conditions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DontShowIfMultiConditionAttribute : Attribute
    {
        /// <summary>
        /// A list of property names (as bools) and flag conditions that need to be met for this member to be shown.
        /// </summary>        
        public List<(string propertyName, bool condition)> propertyConditionsList = new List<(string propertyName, bool condition)>();

        /// <summary>
        /// Priavte Constructor - Not In Use.
        /// </summary>
        protected DontShowIfMultiConditionAttribute() { }

        // Yes I know this is bad. But what's worse is there's no better way. C#9.0 restricts what we can do with attributes.
        // So, I made it so that up to 8 properties could be checked at maximum. 2 properties being the minimum.

        /// <summary>
        /// Create a Show If Condition Attribute for if two properties in the node are true or false.
        /// </summary>
        public DontShowIfMultiConditionAttribute(
            string propertyName1, bool condition1,
            string propertyName2, bool condition2)
        {
            propertyConditionsList.Add((propertyName1, condition1));
            propertyConditionsList.Add((propertyName2, condition2));
        }

        /// <summary>
        /// Create a Show If Condition Attribute for if three properties in the node are true or false.
        /// </summary>
        public DontShowIfMultiConditionAttribute(
            string propertyName1, bool condition1,
            string propertyName2, bool condition2,
            string propertyName3, bool condition3
            )
        {
            propertyConditionsList.Add((propertyName1, condition1));
            propertyConditionsList.Add((propertyName2, condition2));
            propertyConditionsList.Add((propertyName3, condition3));
        }

        /// <summary>
        /// Create a Show If Condition Attribute for if four properties in the node are true or false.
        /// </summary>
        public DontShowIfMultiConditionAttribute(
            string propertyName1, bool condition1,
            string propertyName2, bool condition2,
            string propertyName3, bool condition3,
            string propertyName4, bool condition4
            )
        {
            propertyConditionsList.Add((propertyName1, condition1));
            propertyConditionsList.Add((propertyName2, condition2));
            propertyConditionsList.Add((propertyName3, condition3));
            propertyConditionsList.Add((propertyName4, condition4));
        }

        /// <summary>
        /// Create a Show If Condition Attribute for if five properties in the node are true or false.
        /// </summary>
        public DontShowIfMultiConditionAttribute(
            string propertyName1, bool condition1,
            string propertyName2, bool condition2,
            string propertyName3, bool condition3,
            string propertyName4, bool condition4,
            string propertyName5, bool condition5
            )
        {
            propertyConditionsList.Add((propertyName1, condition1));
            propertyConditionsList.Add((propertyName2, condition2));
            propertyConditionsList.Add((propertyName3, condition3));
            propertyConditionsList.Add((propertyName4, condition4));
            propertyConditionsList.Add((propertyName5, condition5));
        }

        /// <summary>
        /// Create a Show If Condition Attribute for if six properties in the node are true or false.
        /// </summary>
        public DontShowIfMultiConditionAttribute(
            string propertyName1, bool condition1,
            string propertyName2, bool condition2,
            string propertyName3, bool condition3,
            string propertyName4, bool condition4,
            string propertyName5, bool condition5,
            string propertyName6, bool condition6
            )
        {
            propertyConditionsList.Add((propertyName1, condition1));
            propertyConditionsList.Add((propertyName2, condition2));
            propertyConditionsList.Add((propertyName3, condition3));
            propertyConditionsList.Add((propertyName4, condition4));
            propertyConditionsList.Add((propertyName5, condition5));
            propertyConditionsList.Add((propertyName6, condition6));
        }

        /// <summary>
        /// Create a Show If Condition Attribute for if seven properties in the node are true or false.
        /// </summary>
        public DontShowIfMultiConditionAttribute(
            string propertyName1, bool condition1,
            string propertyName2, bool condition2,
            string propertyName3, bool condition3,
            string propertyName4, bool condition4,
            string propertyName5, bool condition5,
            string propertyName6, bool condition6,
            string propertyName7, bool condition7
            )
        {
            propertyConditionsList.Add((propertyName1, condition1));
            propertyConditionsList.Add((propertyName2, condition2));
            propertyConditionsList.Add((propertyName3, condition3));
            propertyConditionsList.Add((propertyName4, condition4));
            propertyConditionsList.Add((propertyName5, condition5));
            propertyConditionsList.Add((propertyName6, condition6));
            propertyConditionsList.Add((propertyName7, condition7));
        }

        /// <summary>
        /// Create a Show If Condition Attribute for if eight properties in the node are true or false.
        /// </summary>
        public DontShowIfMultiConditionAttribute(
            string propertyName1, bool condition1,
            string propertyName2, bool condition2,
            string propertyName3, bool condition3,
            string propertyName4, bool condition4,
            string propertyName5, bool condition5,
            string propertyName6, bool condition6,
            string propertyName7, bool condition7,
            string propertyName8, bool condition8
            )
        {
            propertyConditionsList.Add((propertyName1, condition1));
            propertyConditionsList.Add((propertyName2, condition2));
            propertyConditionsList.Add((propertyName3, condition3));
            propertyConditionsList.Add((propertyName4, condition4));
            propertyConditionsList.Add((propertyName5, condition5));
            propertyConditionsList.Add((propertyName6, condition6));
            propertyConditionsList.Add((propertyName7, condition7));
            propertyConditionsList.Add((propertyName8, condition8));
        }
    }
}