using System;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// An attribute that can be attached to <see cref="ScriptingNode"/> classes to provide additional filtering categories in the Node Browser.<br/><br/>
    /// Supports camel-cased names, such as "TimedNodes", which will split the name accordingly in the NodeBrowser.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class NodeBrowserCategoryAttribute : Attribute
    {
        public string categoryName;

        /// <summary>
        /// [PROTECTED] Unused constructor to prevent incorrect usage.
        /// </summary>
        protected NodeBrowserCategoryAttribute() { }

        /// <summary>
        /// Create a NodeBrowserCategory Attribute to assign decorator data to the node's inspector panel. <br/>
        /// This exists so that developers have more freedom-of-expression.
        /// </summary>
        /// <param name="categoryName"></param>
        public NodeBrowserCategoryAttribute(string categoryName)
        {
            this.categoryName = categoryName;
        }
    }
}