using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> A demo node which provides basic debugging.
    /// </summary>
    [System.Serializable]
    [NodeType("Start Node", "ScriptingSystemIcons/SpawnActorIcon")]
    [NodeBrowserCategory("DebugTools")]
    [BrowserKeywords("Level", "Start", "Begin", "Entry")]
    [BrowserTooltip("Provides the scene with a simple start node for Radikon Scripting System to use.\n\nIf you wish to have project-specific code run on a level opening, you should create your own start node.\n\nSee the README.pdf on how to do this.")]
    public class LevelStartNode : ScriptingNode, IScriptingSystemEntryPoint
    {
        // SetupLevel is called when the Scripting System first finds the starting node
        // of a level. Once this is called, then the starting point's Execute Node is called.
        public void SetupLevel() 
        {
            #if UNITY_EDITOR
            Debug.Log("Level Setup");
            #endif
        }

        // In our case, Execute immediately skips over to the next node since this is acting
        // as our level's entry point.
        // You may want to do some checks or initial gameplay logic here instead.
        public override void Execute() => Next();
    }
}