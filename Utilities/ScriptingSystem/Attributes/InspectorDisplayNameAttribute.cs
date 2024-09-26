using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// An attribute that enables a custom display name to be assigned to any scripting node. <br/>
    /// Only works the Unity Inspector for Node objects.
    /// </summary>
    /// <remarks>
    /// <see langword="Radikon.ScriptingSystem:"/> Only useful if node names aren't correctly split. E.G. "Set UIWidget Node" instead of "Set UI Widget Node". 
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class InspectorDisplayNameAttribute : System.Attribute
    {
        /// <summary>
        /// The display name for the node in the Unity Inspector.
        /// </summary>
        public string displayName;

        /// <summary>
        /// Priavte Constructor - Not In Use.
        /// </summary>
        protected InspectorDisplayNameAttribute() { }

        /// <summary>
        /// Create an Inspector Display Name attribute.
        /// </summary>
        public InspectorDisplayNameAttribute(string displayName) => this.displayName = displayName;
    }
}