using System;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// An attribute that allows Scripting Nodes to appear when relavent keywords are provided in the search bar
    /// </summary>
    /// <remarks>
    /// <see langword="Radikon.ScriptingSystem:"/> Only applies in the Node Browser, not the BaseNodeInspector.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class BrowserKeywordsAttribute : Attribute
    {
        /// <summary>
        /// The list of keywords associated with the 
        /// </summary>
        public string[] keywords { get; protected set; }

        /// <summary>
        /// [PROTECTED] - Base Constructor Not in Use
        /// </summary>
        protected BrowserKeywordsAttribute() { }

        /// <summary>
        /// Create a BrowserKeywordsAttribute with a list of applicable key words.
        /// </summary>
        /// <param name="keywords"></param>
        public BrowserKeywordsAttribute(params string[] keywords)
        {
            this.keywords = keywords;
        }
    }

}