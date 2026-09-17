using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine.InputSystem;

public class DFS_loader : MonoBehaviour
{
    [Header("JSON File")]

    public string jsonPath =
        "/Users/tanvir/dev/semester 7/programming classes/ai with python/ai puzzle game/python codes/dfs_path.json";

    private List<Vector2Int> dfsPath = new List<Vector2Int>();


    [Header("Grid References")]

    public Transform cell00;
    public Transform cell01;
    public Transform cell10;


    [Header("Player Settings")]

    public float playerHeight = 0.3f;
    public float stepDelay = 0.25f;


    void Start()
    {
        LoadDFSPath();   
    }


    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine(ShowDFS());
        }
    }


    void LoadDFSPath()
    {
        // Read JSON file
        string jsonText = File.ReadAllText(jsonPath);

        // Parse JSON
        JObject jsonData = JObject.Parse(jsonText);

        // Get dfs_path array
        JArray pathArray = (JArray)jsonData["dfs_path"];


        foreach (JToken nodeToken in pathArray)
        {
            JObject node = nodeToken as JObject;

            if (node == null)
            {
                Debug.LogError(
                    "Node is not an object: " + nodeToken
                );

                continue;
            }


            int row = node["row"].Value<int>();
            int col = node["col"].Value<int>();


            dfsPath.Add(
                new Vector2Int(row, col)
            );
        }


        foreach (Vector2Int node in dfsPath)
        {
            Debug.Log(
                "Row: " + node.x +
                " Col: " + node.y
            );
        }


        Debug.Log(
            "DFS path loaded. Nodes: " +
            dfsPath.Count
        );
    }


    Vector3 GridToWorld(int row, int col)
    {
        Vector3 columnStep =
            cell01.position - cell00.position;

        Vector3 rowStep =
            cell10.position - cell00.position;


        Vector3 targetPosition =
            cell00.position
            + (rowStep * row)
            + (columnStep * col);


        targetPosition.y += playerHeight;

        return targetPosition;
    }


    IEnumerator ShowDFS()
    {
        foreach (Vector2Int node in dfsPath)
        {
            int row = node.x;
            int col = node.y;


            Vector3 targetPosition =
                GridToWorld(row, col);


            transform.position = targetPosition;


            Debug.Log(
                "DFS visiting: (" +
                row + ", " +
                col + ")"
            );


            yield return new WaitForSeconds(stepDelay);
        }
    }
}