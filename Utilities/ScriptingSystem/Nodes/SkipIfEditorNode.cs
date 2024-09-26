using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> A node that skips to a different node if the game is running in the UNITY EDITOR.
    /// </summary>
    [NodeType("Contorl Flow Node", "ScriptingSystemIcons/SkipIfEditorNodeIcon")]
    [BrowserDisplayName("SkipIfEditorNode")]
    [NodeBrowserCategory("ControlFlow")]
    [NodeBrowserCategory("DebugTools")]
    [BrowserKeywords("Alter", "Change", "NextNode", "Editor", "Editor Only", "Skip")]
    [BrowserTooltip("A node that will skip to another node if the project is being accessed via UnityEditor.")]
    public class SkipIfEditorNode : ScriptingNode
    {
        [Header("UnityEditor Skip Settings")]
        [Tooltip("The node to skip to if in the Unity Editor.")]
        public ScriptingNode editorNextNode;
        [Tooltip("Whether to temporarily disable the node skip.\n\nA preferred option for temporarily testing a section, instead of deleting the node.")]
        public bool disableNodeSkip = false;

        public override void Execute()
        {
            #if UNITY_EDITOR

            // if this project is being run in UnityEditor and the node skip isn't disabled,
            // change the nextNode to the editor-only next node.
            if (!disableNodeSkip) nextNode = editorNextNode;

            #endif

            // Then continue.
            Next();
        }
    }
}