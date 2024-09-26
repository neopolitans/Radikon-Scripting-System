using System.Collections.Generic;
using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> A node that disconnects the next node property of a list of nodes.
    /// </summary>
    [NodeType("Contorl Flow Node", "ScriptingSystemIcons/BreakMultipleNodeSequences")]
    [BrowserDisplayName("BreakSequences")]
    [NodeBrowserCategory("ControlFlow")]
    [BrowserKeywords("Break", "Multiple", "Sequences", "Disconnect", "NextNode")]
    [BrowserTooltip("A node that will disconnect the \"nextNode\" property of a list of other nodes.")]
    public class BreakMultipleNodeSequencesNode : ScriptingNode
    {
        [Header("Sequence Breaker Settings")]
        [Tooltip("The nodes to be modified during script execution.\n\nThis can result in extremely dangerous behaviours if misused.")]
        public List<ScriptingNode> targetedNodes = new List<ScriptingNode>();

        public override void Execute()
        {
            foreach (ScriptingNode targetedNode in targetedNodes)
            {
                if (targetedNode != null && targetedNode != this && targetedNode.nextNode != null)
                {
                    targetedNode.nextNode = null;
                }
            }

            Next();
        }

    }
}