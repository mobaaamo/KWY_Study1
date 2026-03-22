using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int[,] gridData = new int[,]
    {
        {0,1,0,0,0, 0,0,1,0,0},
        {0,1,0,1,0, 1,0,1,0,0},
        {0,0,0,1,0, 0,0,1,1,0},
        {1,1,0,0,0, 1,0,0,0,0},
        {0,0,0,1,0, 1,1,1,0,1},
        {0,1,0,0,0, 0,0,1,0,0},
        {0,1,1,1,1, 1,0,1,0,0},
        {0,0,0,0,0, 0,0,0,0,0},
        {1,1,1,1,1, 1,1,1,1,0},
        {0,0,0,0,0, 0,0,0,0,0}
    };
    public Tile[,] tiles;
    void Start()
    {
        GernerateGrid();
    }
    void GernerateGrid()
    {
        int rows = gridData.GetLength(0);
        int cols = gridData.GetLength(1);

        tiles = new Tile[rows, cols]; 


        for(int y = 0;y< rows; y++)
        {
            for(int x =  0;x<cols;x++)
            {
                var tileGo = Instantiate(tilePrefab, new Vector3(x, -y, 0), Quaternion.identity);
                var tile = tileGo.GetComponent<Tile>();
                tile.gridPosition = new Vector2Int(x, y);

                Color tileColor = (gridData[y, x] == 0) ? Color.white : Color.black;
                tile.SetColor(tileColor);

                tiles[y, x] = tile;
            }
        }
    }
}
