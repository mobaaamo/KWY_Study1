using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class AStarVisualizer : MonoBehaviour
{
    PathFinder pathFinder;
    GridManager gridManager;
    void Start()
    {
        pathFinder =  GetComponent<PathFinder>();
        gridManager = GetComponent<GridManager>();     
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(ShowPath());
        }
    }
    IEnumerator ShowPath()
    {
        var path = pathFinder.GetAStarPath();
        var tiles = gridManager.tiles;
        foreach (var pos in path)
        {
            tiles[pos.y, pos.x].SetColor(Color.red);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
