using System.Collections.Generic;
using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> A node that can trigger an independent series of nodes that run in parallel to the main node tree.
    /// </summary>
    [NodeType("Contorl Flow Node", "ScriptingSystemIcons/TriggerBoxNodeIcon")]
    [BrowserDisplayName("TriggerBox")]
    [NodeBrowserCategory("ControlFlow")]
    [BrowserKeywords("Trigger", "Sequence", "Collider", "OnEnter", "OnExit")]
    [BrowserTooltip("A node that has a trigger which, when entered, runs through a scripting sequence independent of the initial level sequence.")]
    [RequireComponent(typeof(BoxCollider))]
    public class TriggerBoxNode : ScriptingNode
    {
        // Members
        // - Public 
        [Header("Control Flow Settings")]
        [Tooltip("Whether to restrict the sequence to running only once.")]
        public bool runSequenceOnce = false;
        [Tooltip("The tag to try filter for in the other collider.")]
        public string otherObjectTag;

        // - Private
        /// <summary>
        /// Whether the sequence has already been triggered.
        /// </summary>
        private bool sequenceAlreadyTriggered = false;

        /// <summary>
        /// A list of scripting nodes tied to this trigger box and their original names to revert to. <br/>
        /// This was added so that the scripting node didn't constantly append to the sequence it leads into. <br/><br/>
        /// A little bit of memory use is better than a string memory leak!
        /// </summary>
        private List<(ScriptingNode, string)> originalNodeNames = new List<(ScriptingNode, string)>();

        // This auto-assigns prefixes to names of any linked nodes in it's tree so that developers can
        // see the full tree quickly.
        void OnEnable()
        {
            if (TryGetComponent(out BoxCollider collider))
            {
                if (!collider.isTrigger) Debug.LogWarning($"BoxCollider of {name} not set as a trigger! This sequence can't be triggered.");
            }

            ScriptingNode node = nextNode;
            while (node != null)
            {
                // Cache the original node name here.
                originalNodeNames.Add((node, node.name));

                // Then modify the node's name.
                node.name = $"<{name}> " + node.name;
                node = node.nextNode;
            }
        }

        // Reset the name of any nodes 
        private void OnDisable()
        {
            // Reset all node names.
            foreach ((ScriptingNode node, string originalName) obj in originalNodeNames)
            {
                obj.node.name = obj.originalName;
            }

            // Clear the list to free memory if this isn't enabled again.
            // Also prevents duplicate entries if this is ever re-enabled.
            originalNodeNames.Clear();
        }

        public override void Execute()
        {
            Debug.Log($"Trigger Node {name} is not a standard node. Please unlink it from the main level script.");
        }

        public void OnTriggerEnter(Collider other)
        {
            if (sequenceAlreadyTriggered) return;

            if (other.CompareTag(otherObjectTag))
            {
                Next();
                if (runSequenceOnce)
                {
                    sequenceAlreadyTriggered = true;
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (!runSequenceOnce)
            {
                sequenceAlreadyTriggered = false;
            }
        }

        // Custom Override to enforce arbitrary scripting to run without interrupting the main tree.
        public override void Next()
        {
            if (nextNode != null)
            {
                ScriptingCore.ChangeCurrentNode(nextNode, true);
            }
        }
    }
}
