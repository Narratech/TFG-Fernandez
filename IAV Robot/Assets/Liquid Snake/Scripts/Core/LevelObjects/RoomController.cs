using LiquidSnake.LevelObjects;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField]
    private Transform health;

    [SerializeField]
    private Transform hideSpot;

    [SerializeField]
    private List<Access> exits;

    private PlayerController playerController;
    private WorldState worldState;

    private bool discovered;

    private void Awake()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        Collider[] colliders = Physics.OverlapBox(box.bounds.center, box.bounds.size / 2, quaternion.identity, Physics.AllLayers, QueryTriggerInteraction.Collide);

        foreach (Collider collider in colliders)
        {
            Access access = collider.GetComponent<Access>();
            if (access != null)
            {
                access.Room = this;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (playerController == null)
            {
                playerController = other.gameObject.GetComponent<PlayerController>();
                worldState = playerController.GetWorldState();
            }

            UpdateWorldState();
        }
    }

    private void UpdateWorldState()
    {
        if (health != null)
        {
            List<Transform> list = worldState.GetValue<List<Transform>>("TotalHealths");
            if (list == null) list = new List<Transform>();
            if (!list.Contains(health)) list.Add(health);

            worldState.ChangeValue("TotalHealths", list);
            worldState.ChangeValue("CanHeal", list.Count > 0);
        }

        if (hideSpot != null)
        {
            List<Transform> list = worldState.GetValue<List<Transform>>("TotalHideSpots");
            if (list == null) list = new List<Transform>();
            if (!list.Contains(hideSpot)) list.Add(hideSpot);

            worldState.ChangeValue("TotalHideSpots", list);
            worldState.ChangeValue("CanHide", list.Count > 0);
        }

        if (exits != null)
        {
            worldState.ChangeValue("RoomExits", exits);

            List<Access> list = worldState.GetValue<List<Access>>("TotalExits");
            if (list == null) list = new List<Access>();
            foreach (Access exit in exits)
            {
                if (!list.Contains(exit) && !exit.Room.Discovered)
                {
                    list.Add(exit);
                }
            }

            worldState.ChangeValue("TotalExits", list);
        }

        playerController.SetWorldState(worldState);
    }

    public void RemoveHealth(Transform health)
    {
        List<Transform> list = worldState.GetValue<List<Transform>>("TotalHealths");
        if (list.Contains(health)) list.Remove(health);
        worldState.ChangeValue("TotalHealths", list);
    }

    public bool Discovered
    {
        get { return discovered; }
        set { discovered = value; }
    }
}