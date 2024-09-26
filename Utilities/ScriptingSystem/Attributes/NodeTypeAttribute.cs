using System;
using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// An attribute that can be attached to <see cref="ScriptingNode"/> classes to describe additional decorative data in the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class NodeTypeAttribute : Attribute
    {
        /// <summary>
        /// The category that the node is classed as.
        /// </summary>
        public string nodeCategory { get; protected set; }

        /// <summary>
        /// The icon directory (in Unity Editor) that the node will use for it's icon display. 
        /// </summary>
        /// <remarks>
        /// <see langword="Radikon.ScriptingSystem"/>: Node Icons should be found in a Resources folder. <br/> 
        /// This should be a path that can be used by <see cref="Resources.Load{T}(string)"/>.
        /// </remarks>
        public string nodeIconDirectory { get; protected set; }

        /// <summary>
        /// [PROTECTED] Unused constructor to prevent incorrect usage.
        /// </summary>
        protected NodeTypeAttribute() { }

        /// <summary>
        /// Create a NodeType Attribute to assign decorator data to the node's inspector panel.
        /// </summary>
        /// <param name="nodeCategory"></param>
        /// <param name="nodeIconDirectory"></param>
        public NodeTypeAttribute(string nodeCategory, string nodeIconDirectory)
        {
            this.nodeCategory = nodeCategory;
            this.nodeIconDirectory = nodeIconDirectory;
        }
    }

}