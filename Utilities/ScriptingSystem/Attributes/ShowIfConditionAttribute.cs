using System;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// An attribute for whether to show a property or field in the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ShowIfConditionAttribute : Attribute
    {
        /// <summary>
        /// The name of the other field or property that needs to be true or false.
        /// </summary>
        public string propertyName;

        /// <summary>
        /// The flag condition (true or false) that needs to be met for this member to be shown.
        /// </summary>
        public bool condition;

        /// <summary>
        /// Priavte Constructor - Not In Use.
        /// </summary>
        protected ShowIfConditionAttribute() { }

        /// <summary>
        /// Create a Show If Condition Attribute for if another property in the node is true or false.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="condition"></param>
        public ShowIfConditionAttribute(string propertyName, bool condition)
        {
            this.propertyName = propertyName;
            this.condition = condition;
        }
    }
}