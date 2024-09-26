using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> A node that alters the next node property of another node.
    /// </summary>
    [NodeType("Contorl Flow Node", "ScriptingSystemIcons/AlterNodeSequence")]
    [BrowserDisplayName("AlterSequence")]
    [NodeBrowserCategory("ControlFlow")]
    [BrowserKeywords("Alter", "Sequence", "Change", "NextNode")]
    [BrowserTooltip("A node that will change the \"nextNode\" property of another node to the provided node.")]
    public class AlterNodeSequenceNode : ScriptingNode
    {
        [Header("Sequence Alterer Settings")]
        [Tooltip("The node to be modified during script execution.\n\nThis can result in extremely dangerous behaviours if misused.")]
        public ScriptingNode targetedNode = null;
        [Tooltip("The new next node to set to targeted Node.\nThis will refuse to work if this AlterNodeSequence node or targetedNode itself are set as the new next node.")]
        public ScriptingNode newNextNode = null;

        public override void Execute()
        {
            // Prevent really dangerous cyclical node execution conditions here.
            if (targetedNode != null && newNextNode != targetedNode && newNextNode != this)
            {
                // If there's no weird node conditions that can happen, alter the next Node for the targeted node's sequence.
                targetedNode.nextNode = newNextNode;
            }

            // Then continue.
            Next();
        }
    }
}