using UnityEngine;

public class SlotManager : EntityComponent
{
    [System.Serializable]
    public class Slot
    {
        public enum Type { Waiting, Attacking }

        public Type type = Type.Waiting;
        public int index = -1;

        public Slot(Type _type = Type.Waiting, int _index = -1)
        {
            type = _type;
            index = _index;
        }
    }

    [Header("Slot Manager settings")]
    [SerializeField]
    protected float attackingDistaceToEntity = 1.5f;
    public float AttackingDistaceToEntity { get { return attackingDistaceToEntity; } }
    [SerializeField]
    protected float waitingDistaceToEntity = 5.0f;
    [SerializeField]
    protected int numberOfAttackingSlots = 3;
    [SerializeField]
    protected int numberOfWaitingSlots = 10;

    [Header("Edittor debugging")]
    [SerializeField]
    protected GameObject[] attackingSlots;
    [SerializeField]
    protected GameObject[] waitingSlots;

    protected void OnDrawGizmosSelected()
    {
        for (int index = 0; index < numberOfWaitingSlots; ++index)
        {
            if (waitingSlots == null || waitingSlots.Length <= index || waitingSlots[index] == null)
                Gizmos.color = Color.black;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetWaitingSlotPosition(index), 0.5f);
        }

        for (int index = 0; index < numberOfAttackingSlots; ++index)
        {
            if (attackingSlots == null || attackingSlots.Length <= index || attackingSlots[index] == null)
                Gizmos.color = Color.black;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetAttackingSlotPosition(index), 0.5f);
        }

        return;
    }

    protected void Start()
    {
        attackingSlots = new GameObject[numberOfAttackingSlots];
        for (int i = 0; i < numberOfAttackingSlots; i++)
        {
            attackingSlots[i] = null;
        }

        waitingSlots = new GameObject[numberOfWaitingSlots];
        for (int i = 0; i < numberOfWaitingSlots; i++)
        {
            waitingSlots[i] = null;
        }

        return;
    }

    public Vector3 GetSlotPosition(Slot _slot)
    {
        Vector3 position = transform.position;
        switch(_slot.type)
        {
            case Slot.Type.Attacking:
                position = GetAttackingSlotPosition(_slot.index);
                break;

            case Slot.Type.Waiting:
                position = GetWaitingSlotPosition(_slot.index);
                break;

            default:
                break;
        }
        return position;
    }

	protected Vector3 GetWaitingSlotPosition(int index)
    {
        float degreesPerIndex = 360f / numberOfWaitingSlots;
        Vector3 posistion = transform.position;
        Vector3 offset = new Vector3(0f, 0f, waitingDistaceToEntity);

        return posistion + (Quaternion.Euler(new Vector3(0f, degreesPerIndex * index, 0f)) * offset);
    }

    protected Vector3 GetAttackingSlotPosition(int index)
    {
        float degreesPerIndex = 360f / numberOfAttackingSlots;
        Vector3 posistion = transform.position;
        Vector3 offset = new Vector3(0f, 0f, attackingDistaceToEntity);

        return posistion + (Quaternion.Euler(new Vector3(0f, degreesPerIndex * index, 0f)) * offset);
    }

    protected bool IsThereAFreeAtackingSlot()
    {
        bool isfree = false;

        if(attackingSlots.Length == 0) { return isfree; }

        for (int index = 0; index < attackingSlots.Length; ++index)
        {
            if (attackingSlots[index] == null)
            {
                isfree = true;
                break;
            }
        }

        return isfree;
    }

    public Slot Reserve(GameObject attacker)
    {
        Vector3 bestPosition = transform.position;
        Vector3 offset = (attacker.transform.position - bestPosition).normalized * waitingDistaceToEntity;
        bestPosition += offset;
        Slot bestSlot = null;
        float bestDist = 99999f;

        if(IsThereAFreeAtackingSlot())
        {
            for (int index = 0; index < attackingSlots.Length; ++index)
            {
                if (attackingSlots[index] != null)
                    continue;
                var dist = (GetAttackingSlotPosition(index) - bestPosition).sqrMagnitude;
                if (dist < bestDist)
                {
                    bestSlot = new Slot(Slot.Type.Attacking, index);
                    bestDist = dist;
                }
            }

            if (bestSlot.index != -1)
                attackingSlots[bestSlot.index] = attacker;
        }

        else
        {
            for (int index = 0; index < waitingSlots.Length; ++index)
            {
                if (waitingSlots[index] != null)
                    continue;
                var dist = (GetWaitingSlotPosition(index) - bestPosition).sqrMagnitude;
                if (dist < bestDist)
                {
                    bestSlot = new Slot(Slot.Type.Waiting, index);
                    bestDist = dist;
                }
            }

            if (bestSlot.index != -1)
                waitingSlots[bestSlot.index] = attacker;
        }

        return bestSlot;
    }

	public void Release(Slot _slot)
    {
        switch (_slot.type)
        {
            case Slot.Type.Attacking:
                attackingSlots[_slot.index] = null;
                break;

            case Slot.Type.Waiting:
                waitingSlots[_slot.index] = null; ;
                break;

            default:
                break;
        }

        return;
    }
}
