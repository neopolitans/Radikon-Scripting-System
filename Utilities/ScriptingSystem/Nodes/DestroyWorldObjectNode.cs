using System.Collections.Generic;
using UnityEngine;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> A node that destroys one or more game objects in the world.
    /// </summary>
    [NodeType("Object Modifier Node", "ScriptingSystemIcons/DestroyWorldObjectNodeIcon")]
    [BrowserDisplayName("DestroyObject", false)]
    [NodeBrowserCategory("Gameplay")]
    [NodeBrowserCategory("ObjectModifiers")]
    [BrowserKeywords("Object", "Delete", "Destroy")]
    [BrowserTooltip("Destroy any set of world object(s).\n\nThis can include Trigger Box nodes if you wish to make choice-based gameplay.")]
    public class DestroyWorldObject : ScriptingNode
    {
        // Members
        // - Public
        [Header("Destruction Settings")]
        [Tooltip("Whether to wait before destroying the object.")]
        public bool waitBeforeDestroy = false;
        [ShowIfCondition(nameof(waitBeforeDestroy), true)]
        [Tooltip("The amount of time to wait before destroying the object.")]
        public float destructionDelayTime = 1.0f;

        [Header("Object(s) to Modify")]
        [Tooltip("Whether to edit the enabled state of one object or multiple.")]
        public bool destroyListOfObjects = false;

        [Tooltip("The object to destroy.")]
        [ShowIfCondition(nameof(destroyListOfObjects), false)]
        public GameObject targetGameObject = null;

        [Tooltip("The list of objects to destroy.")]
        [ShowIfCondition(nameof(destroyListOfObjects), true)]
        public List<GameObject> targetGameObjects = new List<GameObject>();

        public override void Execute()
        {
            // If there is a list of objects with entries, set the active state of each object in the list to the given value.
            // Otherwise, if there is a single game object to modify, set that object's active state to the given value.
            // Both options wil also set the enabled state of colliders and triggers depending on the settings above.
            if (destroyListOfObjects)
            {
                if (targetGameObjects.Count > 0)
                {
                    foreach (GameObject go in targetGameObjects.ToArray())
                    {
                        if (waitBeforeDestroy) Destroy(go, destructionDelayTime);
                        else Destroy(go);
                    }

                    targetGameObjects.Clear();
                }
            }
            else
            {
                if (targetGameObject != null)
                {
                    if (waitBeforeDestroy) Destroy(targetGameObject, destructionDelayTime);
                    else Destroy(targetGameObject);
                }
            }

            // Then, just go to the next node.
            Next();
        }
    }
}