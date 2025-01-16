using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(VoidGameEvent))]
public class VoidGameEventEditor : Editor
{
    private Vector2 scrollPosition;
    private bool showSceneObjects = true;
    private bool showPrefabObjects = true;

    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        EditorGUILayout.Space(10);
        
        VoidGameEvent voidEvent = (VoidGameEvent)target;

        // Button to test raise event
        GUI.enabled = Application.isPlaying;
        if (GUILayout.Button("Raise Event", GUILayout.Height(30)))
        {
            voidEvent.Raise();
        }
        GUI.enabled = true;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Event Subscribers", EditorStyles.boldLabel);

        // Toggle buttons for filtering
        EditorGUILayout.BeginHorizontal();
        showSceneObjects = EditorGUILayout.ToggleLeft("Show Scene Objects", showSceneObjects, GUILayout.Width(150));
        showPrefabObjects = EditorGUILayout.ToggleLeft("Show Prefab Objects", showPrefabObjects);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(5);

        // Get all GameEventListeners in the scene
        List<GameObject> subscribers = new List<GameObject>();
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();
        
        foreach (GameObject rootObject in rootObjects)
        {
            var listeners = rootObject.GetComponentsInChildren<VoidEventListener>(true);
            foreach (var listener in listeners)
            {
                if (listener.GameEvent == voidEvent)
                {
                    subscribers.Add(listener.gameObject);
                }
            }
        }

        // Begin scroll view
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));

        if (subscribers.Count == 0)
        {
            EditorGUILayout.HelpBox("No subscribers found in the current scene.", MessageType.Info);
        }
        else
        {
            foreach (GameObject subscriber in subscribers)
            {
                bool isPrefab = PrefabUtility.IsPartOfPrefabInstance(subscriber);
                
                if ((isPrefab && showPrefabObjects) || (!isPrefab && showSceneObjects))
                {
                    EditorGUILayout.BeginHorizontal("box");

                    // Object field for the subscriber
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.ObjectField(subscriber, typeof(GameObject), true);
                    EditorGUI.EndDisabledGroup();

                    // Ping button
                    if (GUILayout.Button("Ping", GUILayout.Width(50)))
                    {
                        EditorGUIUtility.PingObject(subscriber);
                    }

                    // Select button
                    if (GUILayout.Button("Select", GUILayout.Width(50)))
                    {
                        Selection.activeGameObject = subscriber;
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        EditorGUILayout.EndScrollView();

        // Add info about total subscribers
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField($"Total Subscribers: {subscribers.Count}", EditorStyles.miniLabel);
    }
}