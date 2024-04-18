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
    private Transform buttonPosition;

    [SerializeField]
    private Transform hideSpot;

    [SerializeField]
    private List<Exit> exits;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            WorldState worldState = player.GetWorldState();

            if (health != null)
            {
                List<Transform> list = worldState.GetValue<List<Transform>>("Healths");
                if (list == null) list = new List<Transform>();
                if (!list.Contains(health)) list.Add(health);

                worldState.ChangeValue("Healths", list);
            }

            if (button != null)
            {
                worldState.ChangeValue("Button", buttonPosition);
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
                List<Exit> list = worldState.GetValue<List<Exit>>("Exits");
                if (list == null) list = new List<Exit>();
                foreach (Exit exit in exits)
                {
                    if (!list.Contains(exit))
                    {
                        list.Add(exit);
                    }
                }

                worldState.ChangeValue("Exits", list);
            }

            player.SetWorldState(worldState);
        }
    }
}