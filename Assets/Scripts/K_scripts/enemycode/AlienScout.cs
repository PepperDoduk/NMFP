using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienScout : MonoBehaviour
{
    public Transform player;
    public float lookRadius = 10f;
    public float shootingRange = 10f;
    public float meleeRange = 3f;
    public float moveSpeed = 5f;
    public GameObject muzzleFlashParticle;
    public Transform spawnPoint;
    public float shootCooldown = 5f;

    private Rigidbody rb;
    private Animator ani;
    private bool isShooting = false;
    private bool isDead = false;
    private float currentShootCooldown;

    private K_EnemyHp enemyHp;

    public abstract class Node
    {
        public abstract NodeState Evaluate();
    }

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
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        enemyHp = GetComponent<K_EnemyHp>();

        if (enemyHp != null)
        {
            enemyHp.OnDeath += OnEnemyDeath;
        }

        _rootNode = CreateBehaviorTree();
    }

    private void Update()
    {
        if (!isDead)
        {
            _rootNode.Evaluate();
        }
    }

    private Node CreateBehaviorTree()
    {
        Func<bool> isPlayerInRange = () => Vector3.Distance(transform.position, player.position) <= meleeRange;
        Func<bool> isPlayerDetected = () => Vector3.Distance(transform.position, player.position) <= lookRadius;
        Func<bool> canShoot = () => Vector3.Distance(transform.position, player.position) <= shootingRange && currentShootCooldown <= 0;

        Action patrolAction = Patrol;
        Action searchAction = Search;
        Action meleeAttackAction = MeleeAttack;
        Action rangedAttackAction = ShootAtPlayer;

        var patrol = new ActionNode(patrolAction);
        var search = new ActionNode(searchAction);
        var meleeAttack = new ActionNode(meleeAttackAction);
        var rangedAttack = new ActionNode(rangedAttackAction);

        var isPlayerInMeleeRange = new ConditionNode(isPlayerInRange);
        var isPlayerSpotted = new ConditionNode(isPlayerDetected);
        var canShootNode = new ConditionNode(canShoot);

        var attackSelector = new Selector(new List<Node>
        {
            new Sequence(new List<Node> { isPlayerInMeleeRange, meleeAttack }),
            new Sequence(new List<Node> { canShootNode, rangedAttack })
        });

        return new Selector(new List<Node>
        {
            new Sequence(new List<Node> { isPlayerSpotted, attackSelector }),
            search,
            patrol
        });
    }

    private void Patrol()
    {
        ani.SetBool("isWalking", true);
        Debug.Log("Patrolling...");
    }

    private void Search()
    {
        ani.SetBool("isWalking", false);
        Debug.Log("Searching...");
    }

    private void MeleeAttack()
    {
        ani.SetTrigger("MeleeAttack");
        ShootAtPlayer();
    }

    private void ShootAtPlayer()
    {
        if (currentShootCooldown <= 0)
        {
            currentShootCooldown = shootCooldown;
            ani.SetTrigger("Shoot");

            GameObject particle = Instantiate(muzzleFlashParticle, spawnPoint.position, spawnPoint.rotation);
            Destroy(particle, 0.1f);

            if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out RaycastHit hitInfo, shootingRange))
            {
                if (hitInfo.collider.CompareTag("Player"))
                {
                    var playerHp = hitInfo.collider.GetComponent<N_PlayerModel>();
                    if (playerHp != null)
                    {
                        playerHp.TakeDamage(10);
                    }
                }
            }
        }
        else
        {
            currentShootCooldown -= Time.deltaTime;
        }
    }

    private void OnEnemyDeath()
    {
        isDead = true;
        ani.SetBool("isWalking", false);
        Debug.Log("Enemy has died.");
    }
}
