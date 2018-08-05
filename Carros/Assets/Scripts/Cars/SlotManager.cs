using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : EntityComponent
{
    [SerializeField]
    protected float distaceToEntity;
    [SerializeField]
    protected int numberOfSlots = 10;
    [SerializeField]
    protected GameObject[] slots;

	protected void OnDrawGizmosSelected()
    {
        for (int index = 0; index < numberOfSlots; ++index)
        {
            if (slots == null || slots.Length <= index || slots[index] == null)
                Gizmos.color = Color.black;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetSlotPosition(index), 0.5f);
        }

        return;
    }

    protected void Start()
    {
        slots = new GameObject[numberOfSlots];
        for(int i = 0; i < numberOfSlots; i++)
        {
            slots[i] = null;
        }

        return;
    }

	public Vector3 GetSlotPosition(int index)
    {
        float degreesPerIndex = 360f / numberOfSlots;
        Vector3 posistion = transform.position;
        Vector3 offset = new Vector3(0f, 0f, distaceToEntity);

        return posistion + (Quaternion.Euler(new Vector3(0f, degreesPerIndex * index, 0f)) * offset);
    }

	public int Reserve(GameObject attacker)
    {
        Vector3 bestPosition = transform.position;
        Vector3 offset = (attacker.transform.position - bestPosition).normalized * distaceToEntity;
        bestPosition += offset;
        int bestSlot = -1;
        float bestDist = 99999f;
        for (int index = 0; index < slots.Length; ++index)
        {
            if (slots[index] != null)
                continue;
            var dist = (GetSlotPosition(index) - bestPosition).sqrMagnitude;
            if (dist < bestDist)
            {
                bestSlot = index;
                bestDist = dist;
            }
        }
        if (bestSlot != -1)
            slots[bestSlot] = attacker;

        return bestSlot;
    }

	public void Release(int slot)
    {
        slots[slot] = null;

        return;
    }
}
