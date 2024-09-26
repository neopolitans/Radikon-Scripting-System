using System;
using UnityEngine;

namespace Radikon.Attributes
{
    /// <summary>
    /// A port of Schaik, J.'s (2022) Single-Flag Enum Selection attribute with slight changes. <br/>
    /// It can now <b>only</b> be applied on enum types.
    /// </summary>
    /// <remarks>
    /// Source: <see href="https://localjoost.github.io/Making-only-one-entry-of-a-Flag-Enum-selectable-in-the-Unity-Editor/"/>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class SingleFlagSelectionAttribute : PropertyAttribute
    {
        /// <summary>
        /// The internal enum type.
        /// </summary>
        public Type enumType {get; protected set;}

        /// <summary>
        /// Is this a valid type?
        /// </summary>
        public bool isValid => enumType.IsSubclassOf(typeof(Enum));

        public SingleFlagSelectionAttribute(Type enumType)
        {
            if (!enumType.IsSubclassOf(typeof(Enum)))
            {
                throw new Exception("SingleFlagSelection cannot be applied to non-enum type fields.");
            }
            else
            {
                this.enumType = enumType;
            }
        }
    }

}