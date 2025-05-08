using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace SmartGrid
{
    [InitializeOnLoad]
    public class AutoSnapGrid : EditorWindow
    {
        private static bool enableSnap = true;
        private static bool showGrid = true;
        private static float gridSize = 1.0f;
        private static int gridExtent = 50;
        private static Color gridColor = new Color(0.2f, 0.2f, 0.5f, 0.5f);

        private static bool snapX = true;
        private static bool snapY = true;
        private static bool snapZ = true;

        private static bool showProximityDistance = true;
        private const float proximityThreshold = 20f;

        private GUIStyle headerStyle;

        private enum GridType { Square, Hexagonal }
        private static GridType selectedGridType = GridType.Square;

        [MenuItem("Tools/AutoSnap Grid")]
        public static void ShowWindow()
        {
            AutoSnapGrid window = GetWindow<AutoSnapGrid>("AutoSnap Grid");
            window.minSize = new Vector2(300, 300);
        }

        private void OnEnable()
        {
            headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 12,
                normal = new GUIStyleState { textColor = Color.cyan }
            };
        }

        private void OnGUI()
        {
            GUILayout.Space(5);
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter
            };
            GUILayout.Space(5);

            using (new EditorGUILayout.VerticalScope("box"))
            {
                enableSnap = EditorGUILayout.Toggle("Enable Snapping", enableSnap);
                showGrid = EditorGUILayout.Toggle("Show Grid", showGrid);
                gridColor = EditorGUILayout.ColorField("Grid Color", gridColor);
            }

            GUILayout.Space(5);

            GUILayout.Label("Grid Type", EditorStyles.boldLabel);
            selectedGridType = (GridType)EditorGUILayout.EnumPopup("Grid Type", selectedGridType);

            GUILayout.Space(5);
            EditorGUILayout.LabelField("Grid Settings", headerStyle);
            using (new EditorGUILayout.VerticalScope("box"))
            {
                gridSize = EditorGUILayout.FloatField("Grid Size", gridSize);
                gridExtent = EditorGUILayout.IntSlider("Grid Range", gridExtent, 10, 100);
            }

            GUILayout.Space(5);
            EditorGUILayout.LabelField("Snapping Axes", headerStyle);
            using (new EditorGUILayout.HorizontalScope("box"))
            {
                snapX = GUILayout.Toggle(snapX, "X", "Button");
                snapY = GUILayout.Toggle(snapY, "Y", "Button");
                snapZ = GUILayout.Toggle(snapZ, "Z", "Button");
            }

            GUILayout.Space(5);
            EditorGUILayout.LabelField("Distance Indicator", headerStyle);
            using (new EditorGUILayout.VerticalScope("box"))
            {
                showProximityDistance = EditorGUILayout.Toggle("Enable Distance Indicator", showProximityDistance);
            }
        }

        static AutoSnapGrid()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            if (showGrid)
            {
                DrawGrid();
            }

            if (!enableSnap || Selection.activeTransform == null)
                return;

            foreach (GameObject obj in Selection.gameObjects)
            {
                    obj.transform.position = GetSnappedPosition(obj.transform.position);

                if(showProximityDistance)
                {
                    DrawProximityIndicators(obj);
                }
            }
        }

        private static Vector3 GetSnappedPosition(Vector3 position)
        {
            switch (selectedGridType)
            {
                case GridType.Hexagonal:
                    return SnapToHexGrid(position);
                default:
                    return new Vector3(
                        snapX ? Mathf.Round(position.x / gridSize) * gridSize : position.x,
                        snapY ? Mathf.Round(position.y / gridSize) * gridSize : position.y,
                        snapZ ? Mathf.Round(position.z / gridSize) * gridSize : position.z
                    );
            }
        }

        private static Vector3 SnapToHexGrid(Vector3 position)
        {
            float hexWidth = gridSize * Mathf.Sqrt(3);
            float hexHeight = gridSize * 1.5f;

            int row = Mathf.RoundToInt(position.z / hexHeight);
            int col = Mathf.RoundToInt(position.x / hexWidth);

            float snappedX = col * hexWidth;
            float snappedZ = row * hexHeight;

            if (row % 2 != 0)
                snappedX += hexWidth / 2; // Offset for odd rows

            return new Vector3(
                snapX ? snappedX : position.x,
                snapY ? Mathf.Round(position.y / gridSize) * gridSize : position.y,
                snapZ ? snappedZ : position.z
            );
        }

        private static void DrawGrid()
        {
            Handles.color = gridColor;
            float gridY = Selection.activeTransform ? Selection.activeTransform.position.y : 0f;

            switch (selectedGridType)
            {
                case GridType.Hexagonal:
                    DrawHexGrid(gridY);
                    break;
                default:
                    DrawSquareGrid(gridY);
                    break;
            }
        }

        private static void DrawSquareGrid(float gridY)
        {
            for (int x = -gridExtent; x <= gridExtent; x++)
            {
                Vector3 start = new Vector3(x * gridSize, gridY, -gridExtent * gridSize);
                Vector3 end = new Vector3(x * gridSize, gridY, gridExtent * gridSize);
                Handles.DrawLine(start, end);
            }

            for (int z = -gridExtent; z <= gridExtent; z++)
            {
                Vector3 start = new Vector3(-gridExtent * gridSize, gridY, z * gridSize);
                Vector3 end = new Vector3(gridExtent * gridSize, gridY, z * gridSize);
                Handles.DrawLine(start, end);
            }
        }

        private static void DrawHexGrid(float gridY)
        {
            float hexWidth = gridSize * Mathf.Sqrt(3);
            float hexHeight = gridSize * 1.5f;

            for (int x = -gridExtent; x <= gridExtent; x++)
            {
                for (int z = -gridExtent; z <= gridExtent; z++)
                {
                    float offsetX = (z % 2 == 0) ? 0 : hexWidth / 2;
                    Vector3 center = new Vector3(x * hexWidth + offsetX, gridY, z * hexHeight);
                    Handles.DrawWireDisc(center, Vector3.up, gridSize * 0.5f);
                }
            }
        }

        private static void DrawProximityIndicators(GameObject obj)
        {
            GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
            List<GameObject> nearbyObjects = new List<GameObject>();

            foreach (GameObject other in allObjects)
            {
                if (other == obj) continue;
                if (other.GetComponent<Light>() || other.GetComponent<Camera>() || other.GetComponent<MeshRenderer>() == null)
                    continue;

                if (Vector3.Distance(obj.transform.position, other.transform.position) <= proximityThreshold)
                {
                    nearbyObjects.Add(other);
                }
            }

            Handles.color = Color.yellow;

            foreach (GameObject nearby in nearbyObjects)
            {
                Handles.DrawLine(obj.transform.position, nearby.transform.position);
                Vector3 midPoint = (obj.transform.position + nearby.transform.position) / 2;
                float distance = Vector3.Distance(obj.transform.position, nearby.transform.position);
                Handles.Label(midPoint, $"Dist: {distance:F2}m", new GUIStyle { fontStyle = FontStyle.Bold, normal = { textColor = Color.yellow } });
            }
        }
    }
}
