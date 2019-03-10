using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace jam
{
    public enum RESOURCE_TYPE
    {
        NONE,
        OXYGEN,
        LASAGNA,
        TITANIUM
    }
    public class GameState : MonoBehaviour
    {
        [SerializeField]
        private float m_dayTick;
        
        [Header("Resources")]
        [SerializeField]
        private float m_titanium;
        [SerializeField]
        private float m_lasagna;
        [SerializeField]
        private float m_tree;
        [SerializeField]
        private float m_oxygen;
        [SerializeField]
        private float m_habitatCount;
        [SerializeField]
        private float m_supplyLimit;

        [Header("Sprites")]
        public Sprite habitatSprite;
        public Sprite[] treeSprites;
        public Sprite[] lasagnaSprites;

        public static GameState Instance;

        private Grid m_grid = new Grid(7, 7);
        public static Grid Grid { get { return Instance.m_grid; } }

        private bool m_gameOver = false;
        public static bool GameOver { get { return Instance.m_gameOver; } }

        private void Awake()
        {
            Instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            TimeManager.Register("Day", OnDayEnd, m_dayTick);
            Slot slot = Slot.Create(3, 3);            
            Slot slot3 = Slot.Create(6, 6);
            slot3.MakeBuilding(new Building(habitatSprite, null, null, RESOURCE_TYPE.TITANIUM, MENU_FLAGS.WORKER, false));
            slot.MakeBuilding(new Building(habitatSprite, null, null, RESOURCE_TYPE.NONE, MENU_FLAGS.NONE, true));
            EventSystem.AddListener<Event_BuildingYield>(OnBuildingYield);
        }

        void OnBuildingYield(Event_BuildingYield e)
        {
            switch(e.type)
            {
                case RESOURCE_TYPE.LASAGNA:
                    m_lasagna += e.amount;
                    break;
                case RESOURCE_TYPE.OXYGEN:
                    m_oxygen += e.amount;
                    break;
                case RESOURCE_TYPE.TITANIUM:
                    m_titanium += e.amount;
                    break;
            }
        }

        void OnDayEnd()
        {
            EventSystem.Dispatch(new Event_DayEnded());
            if (!CheckGameOver())
                EventSystem.Dispatch(new Event_CatSpawned());
        }

        public static bool ConsumeLasagna(float number)
        {
            if (Instance.m_lasagna <= 0) return false;
            Instance.m_lasagna -= number;
            Instance.m_lasagna = Mathf.Clamp(Instance.m_lasagna, 0, Instance.m_lasagna);

            return true;
        }

        public static bool ConsumeTitanium(float amount)
        {
            if (Instance.m_titanium <= 0) return false;
            Instance.m_titanium -= amount;
            Instance.m_titanium = Mathf.Clamp(Instance.m_titanium, 0.0f, Instance.m_titanium);

            return true;
        }

        private bool CheckGameOver()
        {
            m_gameOver = false;
            if (LifetimeManager.CatCount / m_habitatCount > m_supplyLimit) m_gameOver = true;
            else if (m_oxygen <= 0.0f) m_gameOver = true;
            else if (LifetimeManager.CatCount <= 0) m_gameOver = true;

            if (m_gameOver)
            {
                TimeManager.Unregister("Day", OnDayEnd);
                EventSystem.Dispatch(new Event_GameOver());
                EventSystem.Dispatch(new Event_UIText("GAME OVER SWEETIES"));
            }

            return m_gameOver;
        }
    }
}
