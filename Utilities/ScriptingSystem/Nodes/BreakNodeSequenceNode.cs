using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> A node that disconnects the next node property of another node.
    /// </summary>
    [NodeType("Contorl Flow Node", "ScriptingSystemIcons/BreakNodeSequence")]
    [BrowserDisplayName("BreakSequence")]
    [NodeBrowserCategory("ControlFlow")]
    [BrowserKeywords("Break", "Sequence", "Disconnect", "NextNode")]
    [BrowserTooltip("A node that will disconnect the \"nextNode\" property of another node.")]
    public class BreakNodeSequenceNode : ScriptingNode
    {
        [Header("Sequence Breaker Settings")]
        [Tooltip("The node to be modified during script execution.\n\nThis can result in extremely dangerous behaviours if misused.")]
        public ScriptingNode targetedNode;

        public override void Execute()
        {
            if (targetedNode != null && targetedNode != this && targetedNode.nextNode != null)
            {
                targetedNode.nextNode = null;
            }

            Next();
        }

    }
}