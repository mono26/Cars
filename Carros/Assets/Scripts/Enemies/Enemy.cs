using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(AIStateMachine))]
public class Enemy : Entity
{
    [System.Serializable]
    public class EnemyStats
    {
        [SerializeField]
        protected float acceleration; // m / s2
        public float Acceleration { get { return acceleration; } }
        [SerializeField]
        protected float maxHealth;
        public float MaxHealth { get { return maxHealth; } }
        [SerializeField]
        protected float maxSpeed; // m / s
        public float MaxSpeed { get { return maxSpeed; } }
    }
    [Header("Enemy settings")]
    [SerializeField]
    protected EnemyStats stats; // Set in the editor.

    [Header("Enemy components")]
    [SerializeField]
    protected EnemyMovement movement;
    public EnemyMovement Movement { get { return movement; } }
    [SerializeField]
    protected AIStateMachine stateMachine;

    [Header("Editor debugging")]
    [SerializeField]
    protected AIState startingState;
    [SerializeField]
    protected Transform target;
    public Transform Target { get { return target; } }

    protected override void Awake()
    {
        if (movement == null)
            GetComponent<HorizontalAndVerticalMovement>();
        if (stateMachine == null)
            GetComponent<AIStateMachine>();

        return;
    }

    protected virtual void Start()
    {
        movement.SetMovementValues(stats.Acceleration, stats.MaxSpeed);
        stateMachine.ChangeState(startingState);

        return;
    }

    protected override void Update()
    {
        if(stateMachine == null) { return; }

        stateMachine.UpdateState();

        return;
    }

    protected override void FixedUpdate()
    {
        return;
    }

    protected override void LateUpdate()
    {
        return;
    }
}
