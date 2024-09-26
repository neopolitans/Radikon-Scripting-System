using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> An abstract scripting node that requires a condition to be true before passing control to the next node.
    /// </summary>
    [Obsolete("ConditionalScriptingNode is not recommended for inheritance.")]
    public abstract class ConditionalScriptingNode : ScriptingNode
    {
        public override void Execute()
        {
        }

        public override void Next()
        {
            if (Condition())
            {
                base.Next();
            }
        }

        /// <summary>
        /// The condition that needs to be met for the node to pass off control safely. <br/>
        /// This could be any type of condition, so long as the result is a boolean.
        /// </summary>
        /// <returns></returns>
        public virtual bool Condition() => true;

    }

}