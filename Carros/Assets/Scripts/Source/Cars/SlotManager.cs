using UnityEngine;

public enum SlotType { Waiting, Attacking }

[System.Serializable]
public class Slot
{
    [Header("Slot editor debugging")]
    [SerializeField] private SlotType type = SlotType.Waiting;
    [SerializeField] private int index = -1;

    public SlotType GetSlotType { get { return type; } }
    public int GetIndex { get { return index; } }

    public Slot(SlotType _type = SlotType.Waiting, int _index = -1)
    {
        type = _type;
        index = _index;
    }
}

public class SlotManager : EntityComponent
{
    [Header("Slot Manager settings")]
    [SerializeField] private float attackingDistaceToEntity = 1.5f;
    [SerializeField] private float waitingDistaceToEntity = 5.0f;
    [SerializeField] private int numberOfAttackingSlots = 5;
    [SerializeField] private int numberOfWaitingSlots = 10;

    [Header("Edittor debugging")]
    [SerializeField] private GameObject[] attackingSlots;
    [SerializeField] private GameObject[] waitingSlots;

    public float GetAttackingDistaceToEntity { get { return attackingDistaceToEntity; } }
    public GameObject[] GetAttackingSlots { get { return attackingSlots; } }
    public GameObject[] GetWaitingSlots { get { return waitingSlots; } }

    private void OnDrawGizmosSelected()
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

    private void Start()
    {
        attackingSlots = new GameObject[numberOfAttackingSlots];
        for (int i = 0; i < numberOfAttackingSlots; i++) {
            attackingSlots[i] = null;
        }
        waitingSlots = new GameObject[numberOfWaitingSlots];
        for (int i = 0; i < numberOfWaitingSlots; i++) {
            waitingSlots[i] = null;
        }
        return;
    }

	private Vector3 GetWaitingSlotPosition(int index)
    {
        float degreesPerIndex = 360f / numberOfWaitingSlots;
        Vector3 posistion = transform.position;
        Vector3 offset = new Vector3(0f, 0f, waitingDistaceToEntity);
        return posistion + (Quaternion.Euler(new Vector3(0f, degreesPerIndex * index, 0f)) * offset);
    }

    private Vector3 GetAttackingSlotPosition(int index)
    {
        float degreesPerIndex = 360f / numberOfAttackingSlots;
        Vector3 posistion = transform.position;
        Vector3 offset = new Vector3(0f, 0f, attackingDistaceToEntity);
        return posistion + (Quaternion.Euler(new Vector3(0f, degreesPerIndex * index, 0f)) * offset);
    }

    private bool HasAFreeAtackingSlot()
    {
        bool isfree = false;
        if(attackingSlots.Length > 0)
        {
            for (int index = 0; index < attackingSlots.Length; ++index)
            {
                if (attackingSlots[index] == null)
                {
                    isfree = true;
                    break;
                }
            }
        }
        return isfree;
    }

    private Slot ReserveAttackingSlot(GameObject _attacker, Vector3 _bestPosition)
    {
        Slot bestSlot = null;
        float bestDist = 99999f;
        int bestSlotIndex = -1;
        for (int index = 0; index < attackingSlots.Length; ++index)
        {
            if (attackingSlots[index] == null)
            {
                float dist = (GetAttackingSlotPosition(index) - _bestPosition).sqrMagnitude;
                if (dist < bestDist)
                {
                    bestSlotIndex = index;
                    bestDist = dist;
                }
            }
        }
        if (bestSlotIndex != -1)
        {
            attackingSlots[bestSlotIndex] = _attacker;
            bestSlot = new Slot(SlotType.Attacking, bestSlotIndex);
        }
        return bestSlot;
    }

    private Slot ReserveWaitingSlot(GameObject _attacker, Vector3 _bestPosition)
    {
        Slot bestSlot = null;
        float bestDist = 99999f;
        int bestSlotIndex = -1;
        for (int index = 0; index < waitingSlots.Length; ++index)
        {
            if (waitingSlots[index] == null)
            {
                var dist = (GetWaitingSlotPosition(index) - _bestPosition).sqrMagnitude;
                if (dist < bestDist)
                {
                    bestSlotIndex = index;
                    bestDist = dist;
                }
            }
        }
        if (bestSlotIndex != -1)
        {
            waitingSlots[bestSlotIndex] = _attacker;
            bestSlot = new Slot(SlotType.Waiting, bestSlotIndex);
        }
        return bestSlot;
    }

    public Slot Reserve(GameObject attacker)
    {
        Vector3 bestPosition = transform.position;
        Vector3 offset = (attacker.transform.position - bestPosition).normalized * waitingDistaceToEntity;
        bestPosition += offset;
        Slot bestSlot = null;
        if (HasAFreeAtackingSlot()) {
            bestSlot = ReserveAttackingSlot(attacker, bestPosition);
        }
        else {
            bestSlot = ReserveWaitingSlot(attacker, bestPosition);
        }
        return bestSlot;
    }

    public void Release(Slot _slot)
    {
        switch (_slot.GetSlotType)
        {
            case SlotType.Attacking:
                attackingSlots[_slot.GetIndex] = null;
                break;
            case SlotType.Waiting:
                waitingSlots[_slot.GetIndex] = null;
                break;
            default:
                break;
        }
        return;
    }

    public Vector3 GetSlotPosition(Slot _slot)
    {
        Vector3 position = transform.position;
        switch (_slot.GetSlotType)
        {
            case SlotType.Attacking:
                position = GetAttackingSlotPosition(_slot.GetIndex);
                break;
            case SlotType.Waiting:
                position = GetWaitingSlotPosition(_slot.GetIndex);
                break;
            default:
                break;
        }
        return position;
    }
}
