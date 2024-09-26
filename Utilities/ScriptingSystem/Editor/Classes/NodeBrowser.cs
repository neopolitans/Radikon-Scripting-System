// #define ENABLE_FILTEREDNODES_STOPWATCH

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
namespace Radikon.ScriptingSystem.Editor
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEditor.SceneManagement;

    using UEditorStyles = Radikon.Editor.UnlockedEditorStyles;

    public class NodeBrowser : EditorWindow
    {
        #region Classes
        /// <summary>
        /// A struct that describes a Node Prefab while isoalating it from being unintentionally edited.
        /// </summary>
        private struct NodePrefabDescriptor
        {
            // Members
            /// <summary>
            /// The display name to show on the grid list for the node prefab.
            /// </summary>
            public string displayName;

            /// <summary>
            /// The tooltip that shows when the corresponding node's button is hovered over.
            /// </summary>
            public string tooltip;

            /// <summary>
            /// Whether to filter out the node suffix in the display name.
            /// </summary>
            public bool removeNodeSuffix;

            /// <summary>
            /// The scripting node bound to this descriptor.
            /// </summary>
            public ScriptingNode node;

            /// <summary>
            /// The prefab that the ScriptingNode is a part of.
            /// </summary>
            private GameObject prefab;

            /// <summary>
            /// The file path of the icon associated with this node.
            /// </summary>
            private string iconDirectory;

            /// <summary>
            /// The category filters that apply to this node.
            /// </summary>
            private List<string> categoryFilters;

            /// <summary>
            /// The list of keywords associated with this node.
            /// </summary>
            public List<string> keywords;

            // - Properties
            /// <summary>
            /// Get the name of the node's class.
            /// </summary>
            public string Name
            {
                get
                {
                    string[] arr = node.GetType().ToString().Split('.');

                    return arr[arr.Length - 1];
                }
            }

            /// <summary>
            /// Get the icon for the node, if one exists.
            /// </summary>
            public Texture2D Icon
            {
                get
                {
                    return Resources.Load<Texture2D>(iconDirectory);
                }
            }

            // Methods
            /// <summary>
            /// Create an instance of the enclosed node.
            /// </summary>
            /// <returns></returns>
            public GameObject Create(Transform parent)
            {
                GameObject result = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);

                return result;
            }

            /// <summary>
            /// Whether the node type is categorized with a given prefab.
            /// </summary>
            /// <param name="categoryName"></param>
            /// <returns></returns>
            public bool HasCategory(string categoryName)
            {
                return categoryFilters.Contains(categoryName);
            }

            // Constructors (Ctors)
            public NodePrefabDescriptor(ScriptingNode node, List<string> filters, string displayName, bool removeNodeSuffix)
            {
                // Set Defaults
                this.displayName = null;
                this.tooltip = null;
                this.keywords = new List<string>();
                this.removeNodeSuffix = removeNodeSuffix;

                // Set Prefab and Filters
                this.node = node;
                prefab = node.gameObject;
                categoryFilters = filters;

                // Get the Tooltip Attribute and try set node's tooltip if available.
                BrowserTooltipAttribute tooltipAttrib = GetAttribute<BrowserTooltipAttribute>(node.GetType());
                if (tooltipAttrib != null) tooltip = tooltipAttrib.tooltip;

                // Get the Node's Icon Directory, if available, or display the Custom Node Icon as a failsafe.
                NodeTypeAttribute nodeType = GetAttribute<NodeTypeAttribute>(node.GetType());

                if (nodeType != null) iconDirectory = nodeType.nodeIconDirectory;
                else iconDirectory = "ScriptingSystemIcons/CustomNodeIcon";

                // Try get all keywords associated with this node, if available.
                BrowserKeywordsAttribute keywordsList = GetAttribute<BrowserKeywordsAttribute>(node.GetType());
                if (keywordsList != null)
                { 
                    foreach (string word in keywordsList.keywords)
                    {
                        this.keywords.Add(word.ToLower());
                    }
                }

                // Set DisplayName to null temporarily, before attempting to set displayName to something else.

                if (displayName == null)
                {
                    string[] arr = node.GetType().ToString().Split('.');
                    this.displayName = arr[arr.Length - 1];
                }
                else this.displayName = displayName;
            }
        }
        #endregion

        /// <summary>
        /// A list of descriptors that are available for node prefabs.
        /// </summary>
        List<NodePrefabDescriptor> descriptors = new List<NodePrefabDescriptor>();

        /// <summary>
        /// A list of found categories that can be used for filtering for nodes.
        /// </summary>
        List<string> categories = new List<string>();

        /// <summary>
        /// A list of active categories to filter nodes through.
        /// </summary>
        List<string> activeFilters = new List<string>();

        /// <summary>
        /// The search term wihtin the selection grid search field.
        /// </summary>
        string searchTerm = null;

        /// <summary>
        /// The scalar representing the width of the IMGUIFilterList element compared to the window width.
        /// </summary>
        float filterListWidthScalar = 0f;

        /// <summary>
        /// The scalar representing the width of the IMGUINodeSelectionGrid element compared to the window width.
        /// </summary>
        float nodeSelectionGridWidthScalar = 0f;

        /// <summary>
        /// The scroll position for the filter list.
        /// </summary>
        Vector2 filterListScrollPoint = new Vector2(0f, 0f);

        /// <summary>
        /// The scroll position for the node selection grid.
        /// </summary>
        Vector2 selectionGridScrollPoint = new Vector2(0f, 0f);

        /// <summary>
        /// An object in the scene called "ScriptingNodeList". <br/>
        /// If not found, this is created.
        /// </summary>
        Transform scriptingListSceneObject;

        /// <summary>
        /// The size of buttons in the node grid.
        /// </summary>
        float nodeGridButtonSize = 112f;

        /// <summary>
        /// Open the Scripting Node Browser
        /// </summary>
        [MenuItem("Radikon/Node Browser ^#SPACE")]
        public static void OpenNodeBrowser()
        {
            GetWindow<NodeBrowser>();
        }

        public void OnEnable()
        {
            minSize = new Vector2(576f, 384f);

            if (position.size.x < minSize.x || position.size.y < minSize.y) { position = new Rect(position.position, minSize); }

            titleContent = new GUIContent(
                "Node Browser",
                "Browser for creating Nodes from a list of existing Prefabs."
                );

            AssemblyReloadEvents.afterAssemblyReload += OnAssemblyReload;
            LoadAllNodePrefabs();
        }

        public void OnDisable()
        {
            AssemblyReloadEvents.afterAssemblyReload -= OnAssemblyReload;
        }

        public void OnFocus()
        {
            NodeBrowserStyles.ConstructStyles();
            LoadAllNodePrefabs();
        }

        // GUI Drawing Methods
        // - UIToolkit
        public void CreateGUI()
        {
            // This is intentional, I want MonitorStyles to remain
            NodeBrowserStyles.ConstructStyles();

            // Get Root Directiory.
            VisualElement root = rootVisualElement;

            // Create IMGUIFilterList IMGUIElement
            IMGUIContainer filterList = new IMGUIContainer(IMGUIFilterList);
            filterList.style.minWidth = new StyleLength(192f);
            filterList.style.backgroundColor = new StyleColor(new Color32(45, 45, 45, 255));
            filterList.RegisterCallback<GeometryChangedEvent>(OnFilterListGeometryChanged);

            // Create IMGUISelectionGrid IMGUIElement
            IMGUIContainer selectionGrid = new IMGUIContainer(IMGUINodeSelectionGrid);
            selectionGrid.style.minWidth = new StyleLength(384f);
            selectionGrid.style.backgroundColor = new StyleColor(new Color32(51, 51, 51, 255));
            selectionGrid.RegisterCallback<GeometryChangedEvent>(OnSelectionGridGeometryChanged);

            // Create Two Pane Split View for IMGUIFilterList and IMGUISelectionGrid.
            TwoPaneSplitView splitView = new TwoPaneSplitView(0, 192f, TwoPaneSplitViewOrientation.Horizontal);
            splitView.Add(filterList);
            splitView.Add(selectionGrid);

            // Get the dragline and change it's color.
            VisualElement dragLine = splitView.Query(className: "unity-two-pane-split-view__dragline-anchor--horizontal");
            dragLine.style.backgroundColor = new StyleColor(new Color32(35, 35, 35, 255));
            dragLine.style.width = new StyleLength(1.5f);

            // Add SplitView to RootVisualElement.
            root.Add(splitView);
        }

        // - Immediate-Mode GUI
        /// <summary>
        /// Create the Filter List content for the node categories.
        /// </summary>
        public void IMGUIFilterList()
        {
            GUILayout.BeginVertical();
            
            // Draw Filters Top Bar
            // - Filter header and Clear Filters button
            GUILayout.BeginHorizontal(NodeBrowserStyles.searchbarStyle, GUILayout.Height(19f));
                GUILayout.Space(4f);
                GUILayout.Label("Filters", NodeBrowserStyles.filterHeaderText, GUILayout.Height(19f));

                if (GUILayout.Button(" Clear Filters ", NodeBrowserStyles.searchbarButtonStyle, GUILayout.ExpandWidth(false), GUILayout.Height(19f)))
                {
                    activeFilters.Clear();
                }
            GUILayout.EndHorizontal();

            GUILayout.Space(4f);

            // Draw all available categories in a GUI List
            filterListScrollPoint = GUILayout.BeginScrollView(filterListScrollPoint);

            foreach (string category in categories)
            {
                string categoryName = SplitCamelCase(category);
                // Check if the category filter is active and store it here.
                // Additionally, determine the style of the button from the category filter.
                bool categoryFilterActive = activeFilters.Contains(category);
                GUIStyle buttonStyle = categoryFilterActive ? NodeBrowserStyles.buttonSelected : NodeBrowserStyles.buttonNormal;

                // Draw the button in an isolated horizontal space.
                // If not currently being used for a filter
                GUILayout.BeginHorizontal();
                    GUILayout.Space(6f);

                    if (GUILayout.Button(categoryName, buttonStyle))
                    {
                        if (categoryFilterActive) activeFilters.Remove(category);
                        else activeFilters.Add(category);
                    }

                    GUILayout.Space(4f);
                GUILayout.EndHorizontal();
                GUILayout.Space(2f);
            }

            GUILayout.EndScrollView();

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Create the selection grid content for the available nodes.
        /// </summary>
        public void IMGUINodeSelectionGrid()
        {
            // A quick patch here to prevent devs from accidentally creating nodes while doing stuff in run time.
            GUI.enabled = !EditorApplication.isPlaying;

            float activeWidth = position.width * nodeSelectionGridWidthScalar;

            GUILayout.BeginVertical(GUILayout.Width(activeWidth));

            GUILayout.BeginHorizontal(NodeBrowserStyles.searchbarStyle, GUILayout.Height(19f));
                GUILayout.Space(4f);

                // Is there a search term present in the searchTerm variable?
                bool hasTerm = !string.IsNullOrWhiteSpace(searchTerm);

                // Create the Search Field cancel button for resetting the search terms first.
                if (GUI.Button(new Rect(activeWidth - 19f, 1f, 18f, 18f), GUIContent.none, !hasTerm ? UEditorStyles.SearchFieldCancelButtonEmpty : UEditorStyles.SearchFieldCancelButton))
                {
                    // Reset search term and relinquish SearchField from having keyboard control.
                    searchTerm = null;
                    GUIUtility.keyboardControl = 0;    

                    // Repaint the IMGUI area if pressed.
                    Repaint();
                }

                // Create the Search Field for search terms second, so that the cancel button can take effect
                searchTerm = EditorGUI.TextField(new Rect(activeWidth - 274f, 1f, 255f, 18f), searchTerm, EditorStyles.toolbarSearchField);

            GUILayout.EndHorizontal();

            // Search for all the relavent nodes based on categories first, then the Search Bar terms.
            List<NodePrefabDescriptor> foundNodes = GetFilteredNodes();

            if (foundNodes.Count > 0)
            {
                selectionGridScrollPoint = GUILayout.BeginScrollView(selectionGridScrollPoint);
                
                // Get the amount of buttons to draw and the amount of rows. (this was a lot of trial and error)
                int buttonsPerGridRow = Mathf.RoundToInt(activeWidth / nodeGridButtonSize);
                if (buttonsPerGridRow > 1) { buttonsPerGridRow--; } // Prevent nodes from exceeding viewport width while docked, just in case. Only affects docked windows.

                int rowsToDraw = Mathf.CeilToInt((float)foundNodes.Count / (float)buttonsPerGridRow);

                // Store the current amount of buttons drawn here.
                int buttonsDrawn = 0;

                // Iterate over the rows and the buttons to draw the grid here.
                for (int row = 0; row < rowsToDraw; row++)
                {
                    GUILayout.BeginHorizontal(GUILayout.Height(nodeGridButtonSize + 18f));
                    GUILayout.Space(2f);
                    for (int btn = 0; btn < buttonsPerGridRow; btn++)
                    {
                        DrawNodeGridButton(foundNodes[buttonsDrawn]);
                        buttonsDrawn++;
                        if (buttonsDrawn > foundNodes.Count - 1) break;
                    }
                    GUILayout.EndHorizontal();

                    if (buttonsDrawn > foundNodes.Count - 1) break;
                }

                GUILayout.EndScrollView();
            }
            else
            {
                GUILayout.Label("No nodes found matching the provided filters.", NodeBrowserStyles.nodeSearcherNoNodesStyle, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            }

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Create a Grid Button
        /// </summary>
        /// <param name="descriptor"></param>
        private void DrawNodeGridButton(NodePrefabDescriptor descriptor)
        {
            float padding = 4f;

            // Updated: Uses DisplayName over Name and caches Name if displayName is null.
            string descriptorNameString = descriptor.displayName;

            // If the display name contains "Node", such as for a class name,
            // filter this out unless otherwise stated by the display name attribute when the browser reloads.
            if (descriptor.removeNodeSuffix && descriptorNameString.ToLower().Contains("node"))
            {
                descriptorNameString = descriptorNameString.Substring(0, descriptorNameString.Length - 4);
            }

            // SplitCamelCase on the final resulting string.
            descriptorNameString = SplitCamelCase(descriptorNameString);

            // Create the GUIContent Label for the Node's image icon.
            GUIContent imageButtonContent = descriptor.tooltip != null ? new GUIContent(descriptor.Icon, descriptor.tooltip) : new GUIContent(descriptor.Icon);

            GUILayout.Space(padding);

            // Create the Initial Vertical Area for outer padding
            GUILayout.BeginVertical(GUILayout.Height(nodeGridButtonSize), GUILayout.Width(nodeGridButtonSize - 2f));
            GUILayout.Space(padding);
                
                // Create the second Vertical Area for inner padding
                GUILayout.BeginVertical(NodeBrowserStyles.darkStyle);
                    GUILayout.Space(2f);

                    // Create the first horizontal area for padding and the GUI Button
                    GUILayout.BeginHorizontal();
                        GUILayout.Space(2f);

                        // The button that creates a new node usign PrefabUtility, if available.
                        if (GUILayout.Button(imageButtonContent, NodeBrowserStyles.nodeSearcherGridButton, GUILayout.Height(nodeGridButtonSize - 2f), GUILayout.Width(nodeGridButtonSize - 2f)))
                        {
                            CreateNodeFromDescriptor(descriptor);
                        }
                        GUILayout.Space(2f);

                    GUILayout.EndHorizontal();

                    GUILayout.Space(2f);
            
                    // Create the second horizontal area for the name of the node.
                    GUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent(descriptorNameString), NodeBrowserStyles.nodeSearcherGridLabel, GUILayout.Height(18f));
                    GUILayout.EndHorizontal();

                GUILayout.EndVertical();
                GUILayout.Space(padding);
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Create the node from the provided descriptor. 
        /// </summary>
        /// <param name="descriptor"></param>
        private void CreateNodeFromDescriptor(NodePrefabDescriptor descriptor)
        {
            // Attempt to find the ScriptingNodeList object in the scene. If it doesn't exist, it'll be created.
            if (scriptingListSceneObject == null)
            {
                GameObject obj = GameObject.Find("ScriptingNodeList");
                if (obj == null)
                {
                    obj = new GameObject("ScriptingNodeList");
                }
                scriptingListSceneObject = obj.transform;
            }

            // Try find if there's an active node already selected. If it's nextNode is null, the new node will be appended after it in the order.
            // Otherwise, create the node as-is and then assign it as the new active game object in Selection.
            ScriptingNode node = Selection.activeGameObject != null ? Selection.activeGameObject.GetComponent<ScriptingNode>() : null;
            if (node != null)
            {
                // - Originally selected Node as SerializedObject
                SerializedObject OldNodeSO = new SerializedObject(node);
                SerializedProperty ONSO_NextNode = OldNodeSO.FindProperty("nextNode");

                // Create the new active game object.
                Selection.activeGameObject = descriptor.Create(scriptingListSceneObject);
                ScriptingNode newNode = Selection.activeGameObject.GetComponent<ScriptingNode>();

                // If there's no nextNode in the old selected node object, set the nextNode to the new node.
                if (ONSO_NextNode.objectReferenceValue == null)
                {
                    ONSO_NextNode.objectReferenceValue = newNode;
                    OldNodeSO.ApplyModifiedProperties();
                }
            }
            else
            {
                Selection.activeGameObject = descriptor.Create(scriptingListSceneObject);
            }

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        // Asset Loading
        private void OnAssemblyReload()
        {
            LoadAllNodePrefabs();
        }

        /// <summary>
        /// Loads all node prefabs and creates a category
        /// </summary>
        private void LoadAllNodePrefabs()
        {
            // Reset descriptors and categories
            descriptors = new List<NodePrefabDescriptor>();
            categories = new List<string>();

            ScriptingNode[] nodePrefabs = Resources.LoadAll<ScriptingNode>("NodePrefabs");

            // Filter through all nodes to assemble categories.
            foreach (ScriptingNode prefab in nodePrefabs)
            {
                // Sometimes, Resources.LoadAll loads all resources twice?
                // Check if the prefab already exists.
                // If the prefab already exits, skip this object.
                bool hasPrefabAlready = false;
                foreach (NodePrefabDescriptor desc in descriptors)
                {
                    if (desc.node == prefab)
                    {
                        hasPrefabAlready = true;
                        break;
                    }
                }
                if (hasPrefabAlready) continue;

                // Get all category tags attached to each node.
                NodeBrowserCategoryAttribute[] categoryFilterList = GetAttributeList<NodeBrowserCategoryAttribute>(prefab.GetType());

                // Create a list of filters/categories for this node
                List<string> filters = new List<string>();

                // Iterate through all the attached categories
                // If a category doesn't exist globally, add it to the global filter list.
                // And add the category to the list of filters for the current node prefab.
                foreach (NodeBrowserCategoryAttribute category in  categoryFilterList)
                {
                    if (!categories.Contains(category.categoryName)) categories.Add(category.categoryName);
                    filters.Add(category.categoryName);
                }

                // Try Get the displayName attribute.
                // If one exists, this will be the override name in place of NodePrefabDescriptor.Name
                BrowserDisplayNameAttribute displayNameAttribute = GetAttribute<BrowserDisplayNameAttribute>(prefab.GetType());
                string displayName = displayNameAttribute != null ? displayNameAttribute.displayName : null;
                bool filterNodeSuffix = displayNameAttribute != null ? displayNameAttribute.removeNodeSuffix : true;

                descriptors.Add(new NodePrefabDescriptor(prefab, filters, displayName, filterNodeSuffix));
            }

            // Sort the list of available categories alphabetically.
            categories.Sort();
        }

        // Asset Sorting
        /// <summary>
        /// Query the list of all NodePrefabDescriptors to find any that match any current category filter and/or the search field terms if applicable.
        /// </summary>
        /// <returns></returns>
        private List<NodePrefabDescriptor> GetFilteredNodes()
        {
            #if ENABLE_FILTEREDNODES_STOPWATCH
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            #endif

            List<NodePrefabDescriptor> nodes = new List<NodePrefabDescriptor>();

            // Rewritten Search Parser - Slightly more performant due to reduced checks and instructions used
            foreach (NodePrefabDescriptor node in descriptors)
            {
                // Create a variable for storing if all category filters are contained in the internal node.categoryList list. Start as true.
                // Also create a variable that stores whether the node matches the search terms. If the searchTerm is null or whitespaced, parsing is bypassed.
                // finally, create a variable that will hold if any keywords match the seacrh term
                bool matchesAllCategoryFilters = true;
                bool matchesSearchTerm = string.IsNullOrWhiteSpace(searchTerm);
                bool matchesAllKeywords = false;

                // Only run through all active filters if there are any.
                // Otherwise matchesAllCategoryFilters remains true.
                if (activeFilters.Count > 0)
                {
                    // If a category is missing, set matchesAllCategoryFilters to false and break.
                    // Otherwise, we can be assured that all filters are matched.
                    foreach (string category in activeFilters)
                    {
                        if (!node.HasCategory(category))
                        {
                            matchesAllCategoryFilters = false;
                            break;
                        }
                    }
                }

                // Try match the search term to the node name (in lower-case), if matchesSearchTerm needs checking.
                // A scoring system with minimum 70% match threshold could be implemented in future.
                if (!matchesSearchTerm)
                {
                    // Cache searchTerm.ToLower - Less CPU instructions in exchange for more memory use.
                    string lowerST = searchTerm.ToLower();

                    // *Slightly* less computationally expensive now. Regex has a chance of being skipped if either of the first conditions evaluate to true.
                    // This is done to ensure search term matching is thorough.
                    // PATCHNOTE: Also added displayName support. This may be more computationally expensive the more sepcific the seacrh term is.
                    matchesSearchTerm = 
                        node.Name.ToLower().Contains(lowerST) || 
                        node.displayName.ToLower().Contains(lowerST) || 
                        SplitCamelCase(node.Name).ToLower().Contains(lowerST) || 
                        SplitCamelCase(node.displayName).ToLower().Contains(lowerST);

                    // If there are any keywords as well, try to find if the search term has any and that they all fit
                    if (node.keywords.Count > 0)
                    {
                        // Create a list of search terms
                        List<string> foundWords = new List<string>();
                        
                        // Split up the search term and try add lower case variants of any word found in the string array.
                        foreach(string word in searchTerm.Split(' ', ',', '.'))
                        {
                            if (string.IsNullOrWhiteSpace(word)) continue;
                            else foundWords.Add(word.ToLower());
                        }

                        // If there were any search terms found, try find if any keywords are inside the search terms list.
                        if (foundWords.Count > 0)
                        {
                            matchesAllKeywords = true;
                            foreach (string query in foundWords)
                            {
                                matchesAllKeywords = matchesAllKeywords && node.keywords.Contains(query);
                            }
                        }
                    }
                }

                // The final check is simplified to a simple and-operation where all category filters must match AND either the search term or all keywords in the search term should match.
                // These two flags can only ever evaluate to true or false, prior checks and filtering ensures this.
                if (matchesAllCategoryFilters && (matchesSearchTerm || matchesAllKeywords)) nodes.Add(node);
            }
            
            #if ENABLE_FILTEREDNODES_STOPWATCH
            timer.Stop();
            Debug.Log($"Filtering Nodes took {(double)timer.ElapsedTicks / 10000d}ms");
            #endif

            return nodes;
        }

        // Utilities
        /// <summary>
        /// Get a list of the attribue type attached to a given object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static T[] GetAttributeList<T>(System.Type objectType) where T : System.Attribute
        {
            return (T[])System.Attribute.GetCustomAttributes(objectType, typeof(T));
        }

        /// <summary>
        /// Get the first available attribute type attached to a given object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(System.Type objectType) where T : System.Attribute
        {
            return (T)System.Attribute.GetCustomAttribute(objectType, typeof(T));
        }

        /// <summary>
        /// Get the type data from a string that contains the full path of an object.
        /// </summary>
        /// <param name="objectFullType">The full path of the object type.</param>
        /// <param name="objectType">The name of the type itself, isolated from the full path.</param>
        /// <param name="objectNamespace">The namespace the type is in, isolated from the full path.</param>
        /// <returns></returns>
        protected static void GetTypeData(string objectFullType, out string objectType, out string objectNamespace)
        {
            string[] typeData = objectFullType.Split('.');

            objectType = typeData[typeData.Length - 1];

            if (typeData.Length <= 1) objectNamespace = "Global";
            else objectNamespace = objectFullType.Substring(0, objectFullType.Length - (typeData[typeData.Length - 1].Length + 1));
        }

        // [COPIED FROM BASENODEINSEPCTOR]
        // From a very insightful post from 2005 (funnily enough). Source: https://weblogs.asp.net/jongalloway/426087
        // With the changes in processor technology & .NET, this should be inconsequential for only a few uses every inspector update frame.
        /// <summary>
        /// Split a string containing words visually distinguished with camel-casing while keeping acronyms intact. 
        /// </summary>
        /// <remarks>
        /// Thanks to Galloway, J. (2005) &amp; Heidt, J. (2007)
        /// </remarks>
        /// <param name="s"></param>
        /// <returns></returns>
        protected static string SplitCamelCase(string s) => Regex.Replace(s, "([A-Z][A-Z]*)", " $1", RegexOptions.Compiled).Trim();

        #region GeometryChanged Events
        /// <summary>
        /// Get the relative height value from 0-1, representing 0 to position.width, of the IMGUIFilterList via the GeometryChangedEvent.
        /// </summary>
        /// <param name="geometryEvent"></param>
        private void OnFilterListGeometryChanged(GeometryChangedEvent geometryEvent) => filterListWidthScalar = geometryEvent.newRect.width / position.width;

        /// <summary>
        /// Get the relative height value from 0-1, representing 0 to position.width, of the IMGUINodeSelectionGrid via the GeometryChangedEvent.
        /// </summary>
        /// <param name="geometryEvent"></param>
        private void OnSelectionGridGeometryChanged(GeometryChangedEvent geometryEvent) => nodeSelectionGridWidthScalar = geometryEvent.newRect.width / position.width;
        #endregion

    }

}

#endif