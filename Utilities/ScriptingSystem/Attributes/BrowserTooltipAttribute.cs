using System;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// An attribute that can be attached to <see cref="ScriptingNode"/> classes to provide a tooltip in the Node Browser.<br/>
    /// Tooltips will appear on
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class BrowserTooltipAttribute : Attribute
    {
        public string tooltip;

        /// <summary>
        /// [PROTECTED] Unused constructor to prevent incorrect usage.
        /// </summary>
        protected BrowserTooltipAttribute() { }

        /// <summary>
        /// Create a BrowserTooltip attribute to assign a tooltip to the provided Node.
        /// </summary>
        /// <param name="tooltip"></param>
        public BrowserTooltipAttribute(string tooltip)
        {
            this.tooltip = tooltip;
        }
    }

}