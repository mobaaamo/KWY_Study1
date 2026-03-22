using System.Collections;
using System.Collections.Generic;

//실제로 행동을 수행하는 노드
public class BT_Leaf : BT_Node
{
    private System.Func<BT_NodeStatus> action;

    public BT_Leaf(System.Func<BT_NodeStatus> action)
    {
        this.action = action;
    }
    public override BT_NodeStatus Evaluate()
    {
        return action();
    }
}
