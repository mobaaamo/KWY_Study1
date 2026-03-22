using System.Collections.Generic;
using UnityEngine;

//자식노드들을 위에서부터 차례대로 실행해보면서 할수 있는 행동을 고르는 역할

//원리
//1.첫번째 자식부터 Evaluate()실행
//2.만약 자식이 Success->즉시 Success반환
//3.만약 자식이 Running->아직 행동중이므로 Running을 반환
//4.모든 자식이 Failure->Selector전체도 Failure를 반환
public class BT_Selector : BT_Node
{
    private List<BT_Node> children;

    public BT_Selector(List<BT_Node>children)
    {
        this.children = children;
    }
    public override BT_NodeStatus Evaluate()
    {
        foreach (var node in children)
        {
            var status = node.Evaluate();

            if(status==BT_NodeStatus.Success)
            {
                return BT_NodeStatus.Success;
            }
            else if(status==BT_NodeStatus.Running)
            {
                return BT_NodeStatus.Running;
            }
        }
        return BT_NodeStatus.Failure;
    }
}
