using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> A node that executes each available node path until going to the next node sequence.
    /// </summary>
    [NodeType("Control Flow Node", "ScriptingSystemIcons/SequenceNodeIcon")]
    [BrowserDisplayName("Sequencer", false)]
    [NodeBrowserCategory("ControlFlow")]
    [BrowserKeywords("Sequence", "Series")]
    [BrowserTooltip("Run multiple sequences of nodes at once.")]
    public class SequencerNode : ScriptingNode
    {
        // Members
        // - Public
        [Header("Sequence Settings")]
        [Tooltip("The list of nodes that are start point to sequences.")]
        public List<ScriptingNode> sequenceStartPoints = new List<ScriptingNode>();
        // [Tooltip("Whether to wait for all sequences to complete while dp.")]
        // public bool waitForAllSequencesComplete = false;

        // - Private
        /*
        /// <summary>
        /// Is the logic for awaiting for all sequences to be completed enabled?
        /// </summary>
        bool waitActive = false;

        /// <summary>
        /// The list of sequences completed. <br/><br/>
        /// Always the same length as that of <see cref="sequenceStartPoints"/>.
        /// </summary>
        List<bool> sequencesCompleted = new List<bool>();
        */

        // Methods
        // - Public

        public override void Execute()
        {
            foreach (ScriptingNode node in sequenceStartPoints)
            {
                // A fail-safe flag where if the next main scripting node is within the sequence of nodes, it is skipped from this list.
                if (node is null || nextNode == node) continue;

                ScriptingCore.StartArbitraryNodeSequence(node, GetType().FullName);

                // Add a bool to sequencesCompleted which will get set when tracking the sequence progress, if the sequencer will wait for all nodes to complete.
                // if (waitForAllSequencesComplete) sequencesCompleted.Add(false);
            }

            Next();
            /*
            // if there are no sequences or we are not waiting for all sequences to complete, just go to the next node right after starting all of them.
            if (!waitForAllSequencesComplete || sequenceStartPoints.Count < 1)
            {
                Next();
            }
            else waitActive = true;
            */
        }

        // - Private
        private void OnEnable() 
        {
            // This will go through all sequences to alter the name of each start point and subsequent nodes for developer clarity when debugging.
            for (int i = 0; i < sequenceStartPoints.Count; i++)
            {
                ScriptingNode node = sequenceStartPoints[i];
                while (node != null)
                {
                    node.name = $"<{name}_Sequence {i}> {node.name}";
                    node = node.nextNode;
                }
            }
        }

        /*
        private void FixedUpdate()
        {
            if (!waitForAllSequencesComplete) return;
            else
            {
                if (!waitActive) return;

                bool allSequencesCompleted = false;

                for (int i = 0; i < sequenceStartPoints.Count; i++)
                {
                    // if any sequence in the list doesn't resort to 
                    if (sequencesCompleted[i])
                    {
                        allSequencesCompleted = true;
                        continue;
                    }
                    else allSequencesCompleted = false;

                    // Go through all nodes attached to this sequence start point.
                    bool sequenceCompleted = false;
                    ScriptingNode currentNode = sequenceStartPoints[i];

                    // This will progress through each nextNode in the sequence being currently checked and check the running state of that node.
                    // If it finds the sequence is complete, this sequence will be marked as such and no longer checked afterwads.
                    while (currentNode != null)
                    {
                        switch (currentNode.state)
                        {
                            // if awaiting, running or error do nothing.
                            default: break;

                            // if this node is onto the next node, go to the next node and check that.
                            // It also error-handles a condition where nextNode is null but ONNEXTNODE is the value.
                            case ScriptingNodeState.ONNEXTNODE: 
                                if (currentNode.nextNode != null)
                                {
                                    currentNode = currentNode.nextNode;
                                }
                                else 
                                { 
                                    Debug.LogWarning("SequencerNode found a sequence node with no nextNode and a state of \"ONNEXTNODE\".\nThis has been handled but please double-check your sequences!!!");
                                    currentNode.state = ScriptingNodeState.COMPLETE;
                                    sequenceCompleted = true;
                                    break;
                                }
                                break;

                            // The node sequence has been fully completed. all nodes prior will 
                            case ScriptingNodeState.COMPLETE:
                                sequenceCompleted = true;
                                break;
                        }
                    }

                    // Set the value of this sequence's completion state. If true, the sequencer node will stop checking this sequence.
                    sequencesCompleted[i] = sequenceCompleted;
                }

                if (allSequencesCompleted)
                {
                    waitActive = false;
                    Next();
                }
            }
        }
        */
    }
}
