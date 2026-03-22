using UnityEngine;
public class EnemyFSM : MonoBehaviour
{
    public enum State
    {
        Idle,
        Chase,
        Attack
    }
    public State currentState = State.Idle;
    public Transform target;
    public float chaseDistance = 1.5f;
    public float attackDistance = 1.5f;
    public float speed = 2.0f;


    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    

    void Update()
    {
        switch(currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
        }
        TransitionCheck();
    }

    private void Idle()
    {
        AnimatorChange("IDLE");
    }
    private void Chase()
    {
        //이동+회전
        Rotate();
        AnimatorChange("MOVE");

    }
    private void Attack()
    {
        Rotate();
        AnimatorChange("ATTACK");
    }

    private void Rotate()
    {
        transform.position = Vector3.MoveTowards(transform.position,target.position,speed* Time.deltaTime);

        Vector3 direction = (target.position - transform.position).normalized;

        direction.y = 0;

        if (direction == Vector3.zero) return;

        transform.forward = direction;

    }
    private void TransitionCheck()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if(distance<attackDistance)
        {
            currentState = State.Attack;
        }
        else if(distance<chaseDistance)
        {
            currentState = State.Chase;
        }
        else
        {
            currentState = State.Idle;
        }
    }
    private void AnimatorChange(string temp)
    {
        animator.SetBool("IDLE", false);
        animator.SetBool("MOVE", false);
        animator.SetBool("ATTACK", false);

        animator.SetBool(temp, true);
    }
}
