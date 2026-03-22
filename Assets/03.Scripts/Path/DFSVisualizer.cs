using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DFSVisualizer : MonoBehaviour
{

    PathFinder pathFinder;
    GridManager gridManager;
    void Start()
    {
        gridManager = GetComponent<GridManager>();  
        pathFinder = GetComponent<PathFinder>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ShowPath());
        }
    }

    IEnumerator ShowPath()
    {
        var path = pathFinder.GetDFSPath();
        var tiles = gridManager.tiles;

        foreach (var pos in path)
        {
            tiles[pos.y, pos.x].SetColor(Color.green);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
