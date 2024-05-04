using UnityEngine;

namespace LiquidSnake.LevelObjects
{
    public class Access : MonoBehaviour
    {
        [SerializeField]
        private bool isDoor = false;

        [SerializeField]
        private bool isEnd = false;

        private RoomController room;
        private ToggleBarrier door;

        public bool IsDoor
        {
            get { return isDoor; }
            set { isDoor = value; }
        }

        public bool IsEnd
        {
            get { return isEnd; }
            set { isEnd = value; }
        }

        public RoomController Room
        {
            get { return room; }
            set { room = value; }
        }

        public ToggleBarrier Door
        {
            get { return door; }
            set { door = value; }
        }
    }
} // namespace LiquidSnake.LevelObjects