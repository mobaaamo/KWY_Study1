using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class PathFinder : MonoBehaviour
{

    GridManager gridManager;
    public Vector2Int start = new Vector2Int(0, 0);
    public Vector2Int end = new Vector2Int(9, 9);


    private int dfsCount = 0;
    private int bfsCount = 0;
    private int aStarCount = 0;

    [SerializeField] Text DFS_T, BFS_T, AStar_T;


    private void Awake()
    {
        gridManager =  GetComponent<GridManager>();
    }
    #region DFS
    //-  start에서 end까지의 경로를 반환
    public List<Vector2Int> GetDFSPath()
    {
        HashSet<Vector2Int> visited =  new HashSet<Vector2Int>();
        //start위치에서 DFS를 시작해서 end까지의 경로를 찾는다.
        return DFS(start, end, visited);
    }
    //DFS
    List<Vector2Int> DFS(Vector2Int current, Vector2Int end, HashSet<Vector2Int>visited)
    {

        dfsCount++;
        //current가 맵 밖이거나 이미 방문한 칸이면 더이상 진행하지 않는다.
        if(!IsValid(current)|| visited.Contains(current))
        {
            return null;
        }
        //현재 칸을 방문했다고 체크
        visited.Add(current);

        //current가 목표 위치라면
        //current하나만 담긴 경로 리스트를 만들어서 반환
        if(current==end)
        {
            return new List<Vector2Int> { current };
        }

        Vector2Int[] dirs =
        {
            Vector2Int.down,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.right
        };
        //각 방향으로 한칸씩 이동하면서 재귀호출

        foreach (var dir in dirs)
        {
            //다음 칸 위치 계산
            var next = current + dir;
            //next에서 end까지 경로를 재귀적으로 탐색
            var path = DFS(next, end, visited);
            if(path!=null)
            {
                path.Insert(0, current);

                return path;
            }
        }
        DFS_T.text = "DFS : " + dfsCount.ToString();
        Debug.Log(dfsCount);
        return null;
    }
    //해당 좌표가 이동 가능한 칸인지 검사
    bool IsValid(Vector2Int pos)
    {
        //그리드 매니저에서 2차원 배열 형태의 맵 데이터를 가져온다.
        var grid = gridManager.gridData;
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        return pos.x >= 0 &&
               pos.x < width &&
               pos.y >= 0 &&
               pos.y < height &&
               grid[pos.y, pos.x] == 0;
    }
    #endregion
    #region BFS
    public List<Vector2Int> GetBFSPath()
    {
        //이미 방문한 칸을 기록
        var visited = new HashSet<Vector2Int>();
        //각 칸이 어디에서 왔는지 기록
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        //BFS에서 사용할 큐
        var queue = new Queue<Vector2Int>();

        //시작 지점을 큐에 넣고 방문처리
        queue.Enqueue(start);
        visited.Add(start);

        Vector2Int[] dirs =
        {
            Vector2Int.down,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.right
        };

        //큐가 빌 때까지 반복
        while (queue.Count>0)
        {
            bfsCount++;
            //큐에서 가장 먼저 들어온 위치를 꺼내자.
            var current = queue.Dequeue();

            //목표 위치에 도달했다면
            if(current==end)
            {

               BFS_T.text = "BFS : "+bfsCount.ToString();   

                //cameFrom을 이용해 경로를 역추적해서 반환
                return ReconstructPath(cameFrom, end);
            }
            //아직 목표가 아니라면 이웃 네칸을 확인
            foreach (var dir in dirs)
            {
                var next = current + dir;
                //맵 밖이거나 이미 방문했던 칸이면 무시
                if(!IsValid(next)|| visited.Contains(next))
                {
                    continue;
                }
                //방문 예정으로 큐에 넣고
                queue.Enqueue(next);
                //방문체크
                visited.Add(next);
                //next 칸에 오기 직전 칸은 current라고 기록
                cameFrom[next] = current;
            }
        }
        //큐가 빌때까지 end를 찾지 못했다면 경로가 없음.
        return null;


    }
    //경로를 재구성 하는 메서드
    //cameFrom정보와 end위치를 가지고
    //start -> end로 이어지는 실제 경로 리스트를 만든다.
    List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int end)
    {
        //최종 경로를 담을 리스트
        List<Vector2Int> path = new List<Vector2Int>();

        //end에서 시작해서 역으로 start까지 올라감
        var current = end;

        //cameFrom에 Current 키가 존재하는 동안 반복
        while (cameFrom.ContainsKey(current))
        {
            //현재 위치를 경로에 추가
            path.Add(current);
            //한 단계 이전 위치로 이동
            current = cameFrom[current];
        }
        //마지막으로 시작위치도 경로에 포함
        path.Add(current);

        path.Reverse();

        return path;
    }
    #endregion

    #region A*
    public List<Vector2Int> GetAStarPath()
    {
        var openSet = new PriorityQueue<Vector2Int>();

        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        var gScore = new Dictionary<Vector2Int, int>();
        var fScore = new Dictionary<Vector2Int, int>();

        openSet.Enqueue(start, 0);
        gScore[start] = 0;
        fScore[start] = Heuristic(start, end);

        var closedSet = new HashSet<Vector2Int>();

        Vector2Int[] dirs =
        {
            Vector2Int.down,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.right
        };

        while (openSet.Count>0)
        {

            aStarCount++;
            var current =  openSet.Dequeue();

            if(current==end)
            {

                AStar_T.text = "A*  : "+ aStarCount.ToString();
                return ReconstructPath(cameFrom, end);
            }

            closedSet.Add(current);

            foreach (var dir in dirs)
            {
                var neighbor = current + dir;
                if(!IsValid(neighbor)|| closedSet.Contains(neighbor))
                {
                    continue;
                }

                int tentativeG = gScore[current] + 1;

                if(!gScore.ContainsKey(neighbor)|| tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor, end);

                    openSet.Enqueue(neighbor, fScore[neighbor]);
                }
            }
        }
        return null;
    }

    int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
    #endregion
   
}
