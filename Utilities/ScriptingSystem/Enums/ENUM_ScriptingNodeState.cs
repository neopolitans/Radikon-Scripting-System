namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// An enuemrator describing the state of a ScriptingNode. <br/>
    /// This has been added to make the
    /// </summary>
    public enum ScriptingNodeState
    {
        /// <summary>
        /// A scripting error has caused the node to error. <br/><br/>
        /// Not normally seen and is here for future nodes.
        /// </summary>
        ERROR       = -1,

        /// <summary>
        /// The node has yet to be picked up by the main or any arbitrary scripting sequence.
        /// </summary>
        AWAITING    = 0,

        /// <summary>
        /// The node is currently running on the main or any arbitrary scripting sequence.
        /// </summary>
        /// <remarks>
        /// <see langword="Radikon.ScriptingSystem:"/> This is set by the Scripting Core when the node is being called.
        /// </remarks>
        RUNNING     = 1,

        /// <summary>
        /// The node has been completed and all nodes in it's sequence have been completed.
        /// </summary>
        COMPLETE    = 2,

        /// <summary>
        /// The node has been completed, but a node further in the sequence is still running.
        /// </summary>
        ONNEXTNODE  = 3,
    }
}