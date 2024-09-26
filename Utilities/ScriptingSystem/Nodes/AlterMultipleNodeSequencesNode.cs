using System.Collections.Generic;
using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> A node that for each pair of nodes provided, <br/>
    /// will set the nextNode of the first node in the pair to that of the second node in the pair.
    /// </summary>
    [NodeType("Contorl Flow Node", "ScriptingSystemIcons/AlterMultipleNodeSequences")]
    [BrowserDisplayName("AlterSequences")]
    [NodeBrowserCategory("ControlFlow")]
    [BrowserKeywords("Alter", "Multiple", "Sequence", "Change", "NextNode")]
    [BrowserTooltip("A node that will change the \"nextNode\" property of a list of nodes to each provided node in each pair.")]
    [System.Serializable]
    public class AlterMultipleNodeSequencesNode : ScriptingNode
    {
        [Header("Sequence Alterer Settings")]
        [Tooltip("A list of nodes to be modified during script execution and their node to be linked to.\n\nThis can result in extremely dangerous behaviours if misused.")]
        public List<ScriptingNode> targetedNodes = new List<ScriptingNode>();
        [Tooltip("The list of new next nodes to set to targeted Node.\nThis will refuse to work if this AlterNodeSequence node or targetedNode itself are set as the new next node.\n\nShould be the same size as targetedNodes.")]
        public List<ScriptingNode> newNextNodes = new List<ScriptingNode>();

        public override void Execute()
        {
            for (int i = 0; i < targetedNodes.Count; i++)
            {
                // Skip any mismatched sizes.
                if (i >= newNextNodes.Count) continue;

                // Prevent really dangerous cyclical node execution conditions here.
                if (targetedNodes[i] != null && newNextNodes[i] != targetedNodes[i] && newNextNodes[i] != this)
                {
                    // If there's no weird node conditions that can happen, alter the next Node for the targeted node's sequence.
                    targetedNodes[i].nextNode = newNextNodes[i];
                }
            }

            // Then continue.
            Next();
        }

    }
}