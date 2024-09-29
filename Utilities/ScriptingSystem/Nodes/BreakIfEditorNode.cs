using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> A node that skips to a different node if the game is running in the UNITY EDITOR.
    /// </summary>
    [NodeType("Contorl Flow Node", "ScriptingSystemIcons/BreakIfEditorNode")]
    [BrowserDisplayName("SkipIfEditorNode")]
    [NodeBrowserCategory("ControlFlow")]
    [NodeBrowserCategory("DebugTools")]
    [BrowserKeywords("Editor", "Editor Only", "Break")]
    [BrowserTooltip("A node that will stop a sequence when reached if the project is being accessed via UnityEditor.")]
    public class BreakIfEditorNode : ScriptingNode
    {
        public override void Execute()
        {
            #if !UNITY_EDITOR
            Next();
           #endif
        }
    }
}