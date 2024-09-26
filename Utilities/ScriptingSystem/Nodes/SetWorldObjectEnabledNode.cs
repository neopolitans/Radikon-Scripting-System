using System.Collections.Generic;
using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> A node that modifies the enabled state of game objects in the world.
    /// </summary>
    [NodeType("Object Modifier Node", "ScriptingSystemIcons/SetWorldObjectEnabledNode")]
    [BrowserDisplayName("SetObjectEnabled", false)]
    [NodeBrowserCategory("Gameplay")]
    [NodeBrowserCategory("ObjectModifiers")]
    [BrowserKeywords("Object", "Enabled", "Disabled", "Set", "Active", "Activate", "Inactive", "Deactivate")]
    [BrowserTooltip("Set the enabled state of any world object(s).\n\nThis can include Trigger Box nodes if you wish to make choice-based gameplay.")]
    public class SetWorldObjectEnabledNode : ScriptingNode
    {
        // Members
        // - Public
        [Header("Object Modifier Settings")]
        [Tooltip("The value to set the active state of the Game Object(s) to.")]
        public bool enabledState = false;

        [Header("Collider & Trigger Settings")]
        [Tooltip("Whether to explicity enforce any collider components within the Game Object(s) to be modified.")]
        public bool enforceModifyingColliders = false;
        [ShowIfCondition(nameof(enforceModifyingColliders), true)]
        [Tooltip("Whether to explicity enforce any collider components marked as triggers within the Game Object(s) to be modified too.")]
        public bool enforceModifyingTriggers = false;

        [Header("Object(s) to Modify")]
        [Tooltip("Whether to edit the enabled state of one object or multiple.")]
        public bool setListOfObjects = false;

        [Tooltip("The object to set the enabled state of.")]
        [ShowIfCondition(nameof(setListOfObjects), false)]
        public GameObject targetGameObject = null;

        [Tooltip("The list of objects to set the enabled state of.")]
        [ShowIfCondition(nameof(setListOfObjects), true)]
        public List<GameObject> targetGameObjects = new List<GameObject>();

        public override void Execute()
        {
            // If there is a list of objects with entries, set the active state of each object in the list to the given value.
            // Otherwise, if there is a single game object to modify, set that object's active state to the given value.
            // Both options wil also set the enabled state of colliders and triggers depending on the settings above.
            if (setListOfObjects)
            {
                if (targetGameObjects.Count > 0)
                {
                    foreach (GameObject go in targetGameObjects)
                    {
                        if (enforceModifyingColliders)
                        {
                            foreach (Collider col in go.GetComponentsInChildren<Collider>())
                            {
                                if (col.isTrigger && !enforceModifyingTriggers) continue;
                                else col.enabled = enabledState;
                            }
                        }

                        go.SetActive(enabledState);
                    }
                }
            }
            else
            {
                if (targetGameObject != null)
                {
                    foreach (Collider col in targetGameObject.GetComponentsInChildren<Collider>())
                    {
                        if (col.isTrigger && !enforceModifyingTriggers) continue;
                        else col.enabled = enabledState;
                    }

                    targetGameObject.SetActive(enabledState);
                }
            }

            // Then, just go to the next node.
            Next();
        }
    }
}