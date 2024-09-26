using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> A demo node which provides basic debugging.
    /// </summary>
    [System.Serializable]
    [NodeType("Debug Node", "ScriptingSystemIcons/DebugNodeIcon")]
    [NodeBrowserCategory("DebugTools")]
    [BrowserKeywords("Log", "Message", "Debug", "Console", "Debugging")]
    [BrowserTooltip("Display a message in Unity's Developer Console. (Editor Only)")]
    public class ConsoleMessageNode : ScriptingNode
    {
        [Header("Debug Node Settings")]
        [Tooltip("The message that this debug node will display when run.")]
        public string message;

        [Tooltip("Whether to log this message as an error.")]
        public bool logAsError;

        public override void Execute()
        {
            #if UNITY_EDITOR
            // If this should be logged as an error, log it as an error instead.
            if (logAsError) Debug.LogError(message);
            else Debug.Log(message);
            #endif

            Next();
        }
    }
}