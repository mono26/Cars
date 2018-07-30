﻿using System.Collections;
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
    public AIStateMachine StateMachine { get { return stateMachine; } }

    [Header("Editor debugging")]
    protected Ability[] abilities;
    public Ability[] Abilities { get { return abilities; } }
    [SerializeField]
    protected AIState startingState;
    [SerializeField]
    protected Ability nextAbility;
    public Ability NextAbility { get { return nextAbility; } }

    /// <summary>
    /// The next ability to cast.
    /// </summary>
    /// <param name="_nextAbility"> The next ability to cast, can be NULL.</param>
    public void SetNextAbility(Ability _nextAbility) { nextAbility = _nextAbility; }

    protected override void Awake()
    {
        base.Awake();

        if (movement == null)
            GetComponent<HorizontalAndVerticalMovement>();
        if (stateMachine == null)
            GetComponent<AIStateMachine>();

        abilities = GetComponents<Ability>();

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
        base.Update();

        if(stateMachine == null) { return; }

        stateMachine.UpdateState();

        return;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        return;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        return;
    }
}