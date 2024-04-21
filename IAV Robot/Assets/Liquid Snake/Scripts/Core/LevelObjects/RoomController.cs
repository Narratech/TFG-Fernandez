using LiquidSnake.LevelObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField]
    private Transform health;

    [SerializeField]
    private Button button;

    [SerializeField]
    private Transform hideSpot;

    [SerializeField]
    private List<Access> exits;

    private PlayerController playerController;
    private WorldState worldState;

    public void UpdateWorldState()
    {
        if (health != null)
        {
            List<Transform> list = worldState.GetValue<List<Transform>>("TotalHealths");
            if (list == null) list = new List<Transform>();
            if (!list.Contains(health)) list.Add(health);

            worldState.ChangeValue("TotalHealths", list);
            worldState.ChangeValue("CanHeal", list.Count > 0);
        }

        if (button != null)
        {
            List<Transform> list = worldState.GetValue<List<Transform>>("TotalButtons");
            if (list == null) list = new List<Transform>();
            if (!button.Triggered() && !list.Contains(button.GetPosition())) list.Add(button.GetPosition());

            worldState.ChangeValue("TotalButtons", list);

            worldState.ChangeValue("CurrentButton", button.GetPosition());
            worldState.ChangeValue("CanButton", !button.Triggered());
        }
        else
        {
            worldState.ChangeValue("CanButton", false);
        }

        if (hideSpot != null)
        {
            worldState.ChangeValue("HideSpot", hideSpot);
            worldState.ChangeValue("CanHide", true);
        }
        else
        {
            worldState.ChangeValue("CanHide", false);
        }

        if (exits != null)
        {
            worldState.ChangeValue("RoomExits", exits);

            List<Access> list = worldState.GetValue<List<Access>>("TotalExits");
            if (list == null) list = new List<Access>();
            foreach (Access exit in exits)
            {
                if (!list.Contains(exit) && !exit.Used)
                {
                    list.Add(exit);
                }
            }

            worldState.ChangeValue("TotalExits", list);
        }

        playerController.SetWorldState(worldState);
    }

    public void RemoveButton(Transform button)
    {
        List<Transform> list = worldState.GetValue<List<Transform>>("TotalButtons");
        if (list.Contains(button)) list.Remove(button);
        worldState.ChangeValue("TotalButtons", list);
    }

    public void RemoveHealth(Transform health)
    {
        List<Transform> list = worldState.GetValue<List<Transform>>("TotalHealths");
        if (list.Contains(health)) list.Remove(health);
        worldState.ChangeValue("TotalHealths", list);
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
}