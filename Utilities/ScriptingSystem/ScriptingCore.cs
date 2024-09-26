using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using System.Reflection;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> Scripting Core is the class responsible for starting and running through object-based mission scripts.
    /// </summary>
    public static class ScriptingCore
    {
        /// <summary>
        /// The list of scripting nodes located within a level.
        /// </summary>
        private static ScriptingNode[] nodeList;

        /// <summary>
        /// The scripting node determined to be the entry point due to it containing the IScriptingSystemEntryPoint interface.
        /// </summary>
        private static ScriptingNode entryPoint;

        /// <summary>
        /// The scripting node that the mission is on.
        /// </summary>
        private static ScriptingNode current;

        /// <summary>
        /// When moving to the next node, this is used to revert previous node's name. It then holds the name of the new node. <br/>
        /// The new node will have " [Running]" suffixed to it's name for the duration of it's run time.
        /// </summary>
        /// <remarks>
        /// <see langword="Radikon.ScriptingSystem:"/> Used to let developers know where the current node is.
        /// </remarks>
        private static string currentNodeOriginalName = "";

        /// <summary>
        /// Called when the Unity Application starts.
        /// </summary>  
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void OnApplicationStart()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }

        // Node Methods
        /// <summary>
        /// Change the current node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="calledByArbitraryScriptTree">Was the node in question changed to be called by an arbitrary script tree or sequence?</param>
        public static void ChangeCurrentNode(ScriptingNode node, bool calledByArbitraryScriptTree = false)
        {
            if (calledByArbitraryScriptTree) StartArbitraryNodeSequence(node, typeof(ScriptingCore).FullName);
            else
            {
                // Trigger the last node's Finalizer, if there is a prior node and available finalizer.
                // Then revert the current node's name.
                if (current != null)
                {
                    if (current.IsNodeTypeof<IFinalizeMethodProvider>()) ((IFinalizeMethodProvider)current).FinalizeNode();
                    current.gameObject.name = currentNodeOriginalName;
                }

                // Setup the new Current Node.
                current = node;

                // Cache the current node's name then append the running status.
                currentNodeOriginalName = node.gameObject.name;
                node.gameObject.name += " [Running]";
                current.state = ScriptingNodeState.RUNNING;

                // Trigger an Initializer if available
                if (current.IsNodeTypeof<ISetupMethodProvider>()) ((ISetupMethodProvider)current).SetupNode();
                current.Execute();
            }
        }

        /// <summary>
        /// Starts a node sequence not tied to the main node. <br/>
        /// This will set the flag "calledByArbitraryScriptTree" in the given node and run separate to the main level scripting.
        /// </summary>
        /// <param name="node"></param>
        public static void StartArbitraryNodeSequence(ScriptingNode node, string callerIdentifier)
        {
            if (node != null)
            {
                node.calledByArbitraryScriptTree = true;
                node.state = ScriptingNodeState.RUNNING;
                node.Execute();
            }
            else
            {
#if UNITY_EDITOR
                string callerName = callerIdentifier != null ? callerIdentifier : "Unknown Caller";
                Debug.Log($"Call to Radikon.ScriptingCore.StartArbitraryNodeSequence from {callerName} failed. Node reference failed.");
#endif
            }
        }

        // Scene Methods
        private static void OnSceneUnloaded(Scene oldScene)
        {
            entryPoint = null;
            nodeList = null;
        }

        /// <summary>
        /// Called on every scene load to gather all scripting nodes.
        /// </summary>
        /// <param name="newScene"></param>
        /// <param name="loadingMode"></param>
        private static void OnSceneLoaded(Scene newScene, LoadSceneMode loadingMode)
        {
            nodeList = Object.FindObjectsOfType<ScriptingNode>();

            foreach (ScriptingNode node in nodeList)
            {
                // An entry point is needed as a definitive flow for scripting nodes are necessary.
                if (node.IsNodeTypeof<IScriptingSystemEntryPoint>())
                {
                    entryPoint = node;
                    break;
                }
            }

            if (entryPoint != null) 
            {
                ((IScriptingSystemEntryPoint)entryPoint).SetupLevel();
                ChangeCurrentNode(entryPoint);
            }
            #if UNITY_EDITOR
            else Debug.LogWarning("No Entry Point for Scripting System in Scene.");
            #endif
        }

        #region Utility Methods
        /// <summary>
        /// Is the provided Scripting Node an instance of a specific type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNodeTypeof<T>(this ScriptingNode node) => typeof(T).IsInstanceOfType(node);
        #endregion
    }
}