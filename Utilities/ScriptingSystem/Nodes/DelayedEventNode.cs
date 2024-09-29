using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> A node that executes each available node path until going to the next node sequence.
    /// </summary>
    [NodeType("Control Flow Node", "ScriptingSystemIcons/DelayedEventNode")]
    [BrowserDisplayName("Delayed Event Node", false)]
    [NodeBrowserCategory("ControlFlow")]
    [BrowserKeywords("Event", "Delayed", "Sequence", "Trigger")]
    [BrowserTooltip("Begin a sequence after a timed delay from the moment this node loads in.")]
    public class DelayedEventNode : ScriptingNode
    {
        [Header("Event Settings")]
        [Tooltip("The amount of time to wait before triggering the next node.")]
        public float m_timeDelay = 2.0f;
        [Tooltip("Whether to repeat the wait and invocation of the sequence after it has completed once.")]
        public bool m_repeatEvent = false;

        /// <summary>
        /// The amount of time the node has waited already.
        /// </summary>
        float m_currentTimeDelay = 0.0f;

        /// <summary>
        /// Has the delay been completed?
        /// </summary>
        bool m_delayCompleted = false;

        // Execute serves no function right now, as it has nothing to call it.
        public override void Execute() { }

        void Update()
        {
            // If the delay has completed, check if we're to repeat the event.
            // If so, then reset currentTimeDelay and delayCompleted. Otherwise, return.
            if (m_delayCompleted)
            {
                if (m_repeatEvent)
                {
                    m_currentTimeDelay = 0.0f;
                    m_delayCompleted = false;
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (m_currentTimeDelay < m_timeDelay)
                {
                    m_currentTimeDelay += Time.deltaTime;
                }
                else
                {
                    m_delayCompleted = true;
                    Next();
                }
            }
        }
    }
}