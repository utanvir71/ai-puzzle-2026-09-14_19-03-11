using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine.InputSystem;

public class BFS_loader : MonoBehaviour
{
    [Header("JSON File")]
    public string jsonPath="/Users/tanvir/dev/semester 7/programming classes/ai with python/ai puzzle game/python codes/bfs_path.json";

    private List<Vector2Int> bfsPath = new List<Vector2Int>();


    [Header("Grid References")]

    // Cell (0,0)
    public Transform cell00;

    // Cell (0,1)
    public Transform cell01;

    // Cell (1,0)
    public Transform cell10;


    [Header("Player Settings")]

    public float playerHeight = 0.3f;

    public float stepDelay = 0.25f;


    void Start()
    {
        LoadBFSPath();
    }


    void Update()
    {
        // Press SPACE to visualize BFS
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine(ShowBFS());
        }
    }


    void LoadBFSPath()
    {
        // Read JSON file
        string jsonText = File.ReadAllText(jsonPath);

        // Convert JSON text into JObject
        JObject jsonData = JObject.Parse(jsonText);

        // Get bfs_path array
        JArray pathArray = (JArray)jsonData["bfs_path"];


        foreach (JToken nodeToken in pathArray)
        {
            JObject node = nodeToken as JObject;

            if (node == null)
            {
                Debug.LogError("Node is not an object: " + nodeToken);
                continue;
            }


            int row = node["row"].Value<int>();
            int col = node["col"].Value<int>();


            bfsPath.Add(
                new Vector2Int(row, col)
            );
        }


        // Print loaded BFS path
        foreach (Vector2Int node in bfsPath)
        {
            Debug.Log(
                "Row: " + node.x +
                " Col: " + node.y
            );
        }


        Debug.Log(
            "BFS path loaded. Nodes: " + bfsPath.Count
        );
    }


    Vector3 GridToWorld(int row, int col)
    {
        // Distance/direction from (0,0) to (0,1)
        Vector3 columnStep =
            cell01.position - cell00.position;


        // Distance/direction from (0,0) to (1,0)
        Vector3 rowStep =
            cell10.position - cell00.position;


        // Calculate world position
        Vector3 targetPosition =
            cell00.position
            + (rowStep * row)
            + (columnStep * col);


        // Put character slightly above the floor
        targetPosition.y += playerHeight;


        return targetPosition;
    }


    IEnumerator ShowBFS()
    {
        foreach (Vector2Int node in bfsPath)
        {
            int row = node.x;
            int col = node.y;


            Vector3 targetPosition =
                GridToWorld(row, col);


            // Move the character
            transform.position = targetPosition;


            Debug.Log(
                "BFS visiting: (" +
                row + ", " +
                col + ")"
            );


            // Wait before moving to next explored node
            yield return new WaitForSeconds(stepDelay);
        }
    }
}