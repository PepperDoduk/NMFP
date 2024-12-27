using System;
using System.Collections.Generic;
using UnityEngine;

public class K_Node : MonoBehaviour
{
    private K_AlienScout enemy;

    public abstract class Node
    {
        public abstract NodeState Evaluate();
    }
    public Transform player;//예비로 만든것
    public enum NodeState
    {
        Success,
        Failure,
        Running
    }


    public class Selector : Node
    {
        private List<Node> _nodes;

        public Selector(List<Node> nodes) => _nodes = nodes;

        public override NodeState Evaluate()
        {
            foreach (var node in _nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Success:
                        return NodeState.Success;
                    case NodeState.Running:
                        return NodeState.Running;
                }
            }
            return NodeState.Failure;
        }
    }

    public class Sequence : Node
    {
        private List<Node> _nodes;

        public Sequence(List<Node> nodes) => _nodes = nodes;

        public override NodeState Evaluate()
        {
            foreach (var node in _nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Failure:
                        return NodeState.Failure;
                    case NodeState.Running:
                        return NodeState.Running;
                }
            }
            return NodeState.Success;
        }
    }


    public class ConditionNode : Node
    {
        private Func<bool> _condition;

        public ConditionNode(Func<bool> condition) => _condition = condition;

        public override NodeState Evaluate() =>
            _condition.Invoke() ? NodeState.Success : NodeState.Failure;
    }


    public class ActionNode : Node
    {
        private Action _action;

        public ActionNode(Action action) => _action = action;

        public override NodeState Evaluate()
        {
            _action.Invoke();
            return NodeState.Success;
        }
    }


    private Node _rootNode;

    private void Start()
    {
        _rootNode = CreateBehaviorTree();
        
    }

    private void Update()
    {
        _rootNode.Evaluate();
    }

    private Node CreateBehaviorTree()
    {

        Func<bool> isPlayerInRange = () => Vector3.Distance(transform.position, player.position) < 5f;
        Func<bool> isPlayerDetected = () => Vector3.Distance(transform.position, player.position) < 10f;


        Action patrolAction = () => Debug.Log("Patrolling...");

        Action MoveAction = () => Debug.Log("move");



        Action meleeAttackAction = () => enemy.StartCoroutine(enemy.ShootAtPlayer());


        Action rangedAttackAction = () => Debug.Log("Ranged Attack!");




        var patrol = new ActionNode(patrolAction);
        var Move = new ActionNode(MoveAction);
        var meleeAttack = new ActionNode(meleeAttackAction);
        var rangedAttack = new ActionNode(rangedAttackAction);

        var isPlayerInMeleeRange = new ConditionNode(isPlayerInRange);
        var isPlayerSpotted = new ConditionNode(isPlayerDetected);


        var attackSelector = new Selector(new List<Node>
        {
            new Sequence(new List<Node> { isPlayerInMeleeRange, meleeAttack }),
            rangedAttack
        });
        

        return new Selector(new List<Node>
        {
            new Sequence(new List<Node> { isPlayerSpotted, attackSelector }),
            Move,
            patrol
        });
    }
}
