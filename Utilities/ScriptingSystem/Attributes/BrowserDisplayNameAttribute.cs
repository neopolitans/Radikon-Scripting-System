using System;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// An attribute that allows a custom display name for a scripting node prefab to be shown. <br/>
    /// The name will be automatically split using Regular Expressions. (e.g. TestNode -> Test Node)
    /// </summary>
    /// <remarks>
    /// <see langword="Radikon.ScriptingSystem:"/> Only applies in the Node Browser, not the BaseNodeInspector.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class BrowserDisplayNameAttribute : Attribute
    {
        /// <summary>
        /// The display name for the node in the Node Browser.
        /// </summary>
        public string displayName { get; protected set; }

        /// <summary>
        /// Whether to filter out the node suffix in the display name.
        /// </summary>
        public bool removeNodeSuffix { get; protected set; }

        /// <summary>
        /// [PROTECTED] - Base Constructor Not in Use
        /// </summary>
        protected BrowserDisplayNameAttribute() { }

        public BrowserDisplayNameAttribute(string displayName, bool removeNodeSuffix = true)
        {
            this.displayName = displayName;
            this.removeNodeSuffix = removeNodeSuffix;
        } 
    }

}