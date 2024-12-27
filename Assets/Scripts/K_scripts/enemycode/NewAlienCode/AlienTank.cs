using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AlienTank : MonoBehaviour
{
    public Transform player;
    public float lookRadius = 10f;
    public float shootingRange = 10f;
    public float meleeRange = 3f;
    public float moveSpeed = 5f;
    public GameObject muzzleFlashParticle;
    public Transform spawnPoint;
    public float shootCooldown = 5f;

    public GameObject enemybullet;
    public float missileSpeed = 20f;
    public int Speed;
    [SerializeField] private float time = 5f;
    private float bulletTime;

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
        MoveTowardsPlayer();
    }

    private void Search()
    {
        ani.SetBool("isWalking", false);
        MoveTowardsPlayer();
    }

    private void MeleeAttack()
    {
        ani.SetTrigger("MeleeAttack");
        MoveTowardsPlayer();
        ShootAtPlayer();
        Debug.Log("Melee Attack!");
    }

    private void ShootAtPlayer()
    {
        bulletTime -= Time.deltaTime;
        if (bulletTime <= 0)
        {
            bulletTime = time;
            GameObject bulletObj = Instantiate(enemybullet, spawnPoint.position, spawnPoint.rotation);
            K_missilebullet bulletScript = bulletObj.GetComponent<K_missilebullet>();
            if (bulletScript != null)
            {
                bulletScript.SetTarget(player);
            }

            Destroy(bulletObj, 5f);
        }

    }

    private void OnEnemyDeath()
    {
        isDead = true;
        ani.SetBool("isWalking", false);
        Debug.Log("Enemy has died.");
    }
    private void MoveTowardsPlayer()
    {
        if (!isDead && player != null)
        {
            ani.SetBool("isWalking", true);

           
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;

          
            if (direction.magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
            }

          
            Vector3 movement = direction * moveSpeed;
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }
        else
        {
            
            rb.velocity = Vector3.zero;
        }
    }

}
