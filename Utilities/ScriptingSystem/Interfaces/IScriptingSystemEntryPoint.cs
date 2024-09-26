using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> An interface that guarantees the provided node has an entry point for the level.
    /// </summary>
    public interface IScriptingSystemEntryPoint
    {
        /// <summary>
        /// The level setup method, which calls all required actions at the start of the level. <br/>
        /// Such actions can be Player and UI Spawning, Cutsecne Playing or similar.
        /// </summary>
        public void SetupLevel();
    }
}