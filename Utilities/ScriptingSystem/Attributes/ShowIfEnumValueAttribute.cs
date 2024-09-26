using System;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// An attribute for whether to show a property or field in the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ShowIfEnumValueAttribute : Attribute
    {
        /// <summary>
        /// The name of the other field or property (of the given type)
        /// </summary>
        public string propertyName;

        /// <summary>
        /// The maximum integer value.
        /// </summary>
        public int enumValueAsInt;

        /// <summary>
        /// Priavte Constructor - Not In Use.
        /// </summary>
        protected ShowIfEnumValueAttribute() { }

        /// <summary>
        /// Create a Show If Condition Attribute for if another property in the node is true or false.
        /// </summary>
        public ShowIfEnumValueAttribute(string propertyName, int enumValueAsInt)
        {
            this.propertyName = propertyName;
            this.enumValueAsInt = enumValueAsInt;
        }
    }
}