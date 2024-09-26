using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> A node for setting a delay between things happening.
    /// </summary>
    [NodeType("Control Flow Node", "ScriptingSystemIcons/TimerNodeIcon")]
    [BrowserDisplayName("DelayNextNode", false)]
    [NodeBrowserCategory("ControlFlow")]
    [BrowserKeywords("Timer", "Wait", "Delay")]
    [BrowserTooltip("Wait a set amount of time before the next node.")]
    public class DelayNode : ScriptingNode
    {
        // Members
        // - Public 
        [Header("Timer Settings")]
        [Tooltip("The amount of time to wait before passing control to the next node.")]
        public float delayTime;

        // - Private
        private bool countdownActive = false;
        private float remainingTime = 0f;

        public override void Execute()
        {
            countdownActive = true;
            remainingTime = delayTime;
        }

        void Update()
        {
            if (!countdownActive) return;
            else
            {
                remainingTime -= Time.deltaTime;

                if (remainingTime <= 0f)
                {
                    remainingTime = 0f;
                    countdownActive = false;
                    Next();
                }
            }
        }
    }
}
