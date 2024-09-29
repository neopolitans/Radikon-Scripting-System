using UnityEngine;
using UnityEngine.SceneManagement;

namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> A node for finalising the scene and switching to another scene (or main menu).
    /// </summary>
    [NodeType("End Node", "ScriptingSystemIcons/FinishNodeIcon")]
    [BrowserDisplayName("Level End Node")]
    [NodeBrowserCategory("DebugTools")]
    [BrowserKeywords("Change", "Scene", "Load")]
    [BrowserTooltip("A node that loads another scene by build index.")]
    public class LevelExitNode : ScriptingNode
    {
        // Members
        // - Public
        [Header("Scene Loading Settings")]
        [Tooltip("The build index of the scene to go to.")]
        public int m_sceneIndex = 0;
        [Tooltip("The load scene mode to use when calling the Scene Manager.")]
        public LoadSceneMode m_LoadSceneMode = LoadSceneMode.Single;

        public override void Execute()
        {
            SceneManager.LoadScene(m_sceneIndex, m_LoadSceneMode);
        }
    }

}