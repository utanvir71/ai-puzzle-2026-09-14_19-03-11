using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine.InputSystem;

public class IDS_loader : MonoBehaviour
{
    [Header("JSON File")]

    public string jsonPath =
        "/Users/tanvir/dev/semester 7/programming classes/ai with python/ai puzzle game/python codes/ids_path.json"; 

    private List<Vector2Int> IDSPath = new List<Vector2Int>();


    [Header("Grid References")]

    public Transform cell00;
    public Transform cell01;
    public Transform cell10;


    [Header("Player Settings")]

    public float playerHeight = 0.3f;
    public float stepDelay = 0.25f;


    void Start()
    {
        LoadIDSPath();   
    }


    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine(ShowIDS());
        }
    }


    void LoadIDSPath()
    {
        // Read JSON file
        string jsonText = File.ReadAllText(jsonPath);

        // Parse JSON
        JObject jsonData = JObject.Parse(jsonText);

        // Get IDS_path array
        JArray pathArray = (JArray)jsonData["ids_path"];


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


            IDSPath.Add(
                new Vector2Int(row, col)
            );
        }


        foreach (Vector2Int node in IDSPath)
        {
            Debug.Log(
                "Row: " + node.x +
                " Col: " + node.y
            );
        }


        Debug.Log(
            "IDS path loaded. Nodes: " +
            IDSPath.Count
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


    IEnumerator ShowIDS()
    {
        foreach (Vector2Int node in IDSPath)
        {
            int row = node.x;
            int col = node.y;


            Vector3 targetPosition =
                GridToWorld(row, col);


            transform.position = targetPosition;


            Debug.Log(
                "IDS visiting: (" +
                row + ", " +
                col + ")"
            );


            yield return new WaitForSeconds(stepDelay);
        }
    }
}