using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSVisualizer : MonoBehaviour
{

    PathFinder pathFinder;
    GridManager gridManager;
    void Start()
    {
        pathFinder = GetComponent<PathFinder>();
        gridManager = GetComponent<GridManager>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(ShowPath());
        }
    }
    IEnumerator ShowPath()
    {
        var path = pathFinder.GetBFSPath();
        var tiles = gridManager.tiles;


        foreach (var pos in path)
        {
            tiles[pos.y, pos.x].SetColor(Color.blue);
            yield return new WaitForSeconds(0.2f);
        }


    }
}
