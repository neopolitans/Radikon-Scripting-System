using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> An abstract scripting node that describes common functionality of all node types.
    /// </summary>
    [System.Serializable]
    public abstract class ScriptingNode : MonoBehaviour
    {
        /// <summary>
        /// The next node in the sequence.
        /// </summary>
        [Header("Scripting Node Settings")]
        [Tooltip("The next node that will be called when this node has completed it's job.")]
        public ScriptingNode nextNode = null;

        /// <summary>
        /// Was this node called by a script sequence (or tree) that's not the main script tree?
        /// </summary>
        [HideInInspector] public bool calledByArbitraryScriptTree;

        /// <summary>
        /// The current state of the scripting node. <br/>
        /// This always starts as AWAITING.
        /// </summary>
        [HideInInspector] public ScriptingNodeState state = ScriptingNodeState.AWAITING;

        /// <summary>
        /// The time since last frame.
        /// </summary>
        private static float deltaTime => Time.deltaTime;

        /// <summary>
        /// The behaviour of the scripting node which gets run when control is passed to this node. <br/>
        /// This method is called for setting up any behaviour that the node may need during it's immediate runtime.
        /// </summary>
        /// <remarks>
        /// <see cref="MonoBehaviour"/>.<see langword="Start"/>() may also be used for the same purposes.
        /// </remarks>
        public abstract void Execute();

        /// <summary>
        /// Trigger the next node.
        /// </summary>
        public virtual void Next()
        {
            if (nextNode != null)
            {
                state = ScriptingNodeState.ONNEXTNODE;
                ScriptingCore.ChangeCurrentNode(nextNode, calledByArbitraryScriptTree);
            }
            else state = ScriptingNodeState.COMPLETE;
        }
    }

}