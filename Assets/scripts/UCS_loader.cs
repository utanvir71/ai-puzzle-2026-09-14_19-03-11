using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine.InputSystem;

public class UCS_loader : MonoBehaviour
{
    [Header("JSON File")]

    public string jsonPath =
        "/Users/tanvir/dev/semester 7/programming classes/ai with python/ai puzzle game/python codes/ucs_path.json"; 

    private List<Vector2Int> UCSPath = new List<Vector2Int>();


    [Header("Grid References")]

    public Transform cell00;
    public Transform cell01;
    public Transform cell10;


    [Header("Player Settings")]

    public float playerHeight = 0.3f;
    public float stepDelay = 0.25f;


    void Start()
    {
        LoadUCSPath();   
    }


    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine(ShowUCS());
        }
    }


    void LoadUCSPath()
    {
        // Read JSON file
        string jsonText = File.ReadAllText(jsonPath);

        // Parse JSON
        JObject jsonData = JObject.Parse(jsonText);

        // Get UCS_path array
        JArray pathArray = (JArray)jsonData["ucs_path"];


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


            UCSPath.Add(
                new Vector2Int(row, col)
            );
        }


        foreach (Vector2Int node in UCSPath)
        {
            Debug.Log(
                "Row: " + node.x +
                " Col: " + node.y
            );
        }


        Debug.Log(
            "UCS path loaded. Nodes: " +
            UCSPath.Count
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


    IEnumerator ShowUCS()
    {
        foreach (Vector2Int node in UCSPath)
        {
            int row = node.x;
            int col = node.y;


            Vector3 targetPosition =
                GridToWorld(row, col);


            transform.position = targetPosition;


            Debug.Log(
                "UCS visiting: (" +
                row + ", " +
                col + ")"
            );


            yield return new WaitForSeconds(stepDelay);
        }
    }
}