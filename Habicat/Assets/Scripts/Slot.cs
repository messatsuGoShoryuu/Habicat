using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace jam
{
    public class Building
    {
        public Building(Sprite level1, Sprite level2, Sprite level3, RESOURCE_TYPE type, MENU_FLAGS menuFlags, bool isConnected)
        {
            m_sprites = new Sprite[3];
            m_sprites[0] = level1;
            m_sprites[1] = level2;
            m_sprites[2] = level3;

            m_workerCats = new bool[3];
            m_resourceType = type;

            IsConnected = isConnected;
            m_flags = menuFlags;

            EventSystem.AddListener<Event_DayEnded>(OnDayEnded);
        }

        private bool[] m_workerCats;
        public bool[] WorkerCats { get { return m_workerCats; } }

        public void AddWorkerCat(int slotIndex)
        {
            if (m_workerCatCount < 3)
            {
                m_workerCats[slotIndex] = true;
                ++m_workerCatCount;
            }
            else throw new System.Exception("Worker cat index out of bounds");
        }

        public void RemoveWorkerCat(int slotIndex)
        {
            if (m_workerCatCount > 0)
            {
                m_workerCats[slotIndex] = false;
                --m_workerCatCount;
            }
            else throw new System.Exception("Worker cat index out of bounds");
        }

        private void OnDayEnded(Event_DayEnded e)
        {
            EventSystem.Dispatch(new Event_BuildingYield(m_resourceType, GetYieldAmount()));
        }

        private void AddFlag(MENU_FLAGS flag)
        {
            m_flags |= flag;
        }

        private void RemoveFlag(MENU_FLAGS flag)
        {
            m_flags &= (~flag);
        }

        private float GetYieldAmount()
        {
            return m_workerCatCount * (m_level + 1);
        }

        private MENU_FLAGS m_flags;
        public MENU_FLAGS Flags { get { return m_flags; } }
        private RESOURCE_TYPE m_resourceType;
        public RESOURCE_TYPE ResourceType { get { return m_resourceType; } }
        private int m_level = 0;
        private Sprite[] m_sprites;

        public Sprite GetSprite() { return m_sprites[m_level]; }
        public int Level { get { return m_level; } }
        public void Upgrade()
        {
            GameState.ConsumeTitanium(1);
            ++m_level;
        }

        private int m_workerCatCount;
        public bool IsConnected { protected set; get; }

        public void Connect()
        {
            IsConnected = true;
        }
    }

    public class Slot : MonoBehaviour
    {
        [SerializeField] Color m_default;
        [SerializeField] Color m_hover;
        [SerializeField] Color m_clicked;
        [SerializeField]
        Slot[] m_neighbours = new Slot[4];

        const int NORTH = 0;
        const int WEST = 1;
        const int SOUTH = 2;
        const int EAST = 3;

        private Color m_targetColor;
        private Color m_lastColor;
        private float m_timeStamp;

        [SerializeField]
        private Vector2Int m_coordinates;
        public Vector2Int Coordinates { get { return m_coordinates; } }

        private SpriteRenderer m_spriteRenderer;

        public Building building {protected set; get;}


        public static Slot Create(int x, int y)
        {
            GameObject parent = GameObject.Find("Grid");
            Slot slot = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Habitat")).GetComponent<Slot>();
            slot.transform.parent = parent.transform;
            slot.transform.localPosition = new Vector3(x, y, 0);
            slot.m_coordinates = new Vector2Int(x, y);
            slot.m_spriteRenderer = slot.GetComponent<SpriteRenderer>();
            GameState.Grid.SetSlot(x, y, true);
            return slot;
        }

        // Start is called before the first frame update
        void Start()
        {
            m_spriteRenderer.color = building == null ? m_default : m_hover;
            m_lastColor = m_spriteRenderer.color;
            m_targetColor = m_spriteRenderer.color;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (m_timeStamp < 1.0f)
            {
                m_timeStamp += 5.0f * Time.deltaTime;
                m_spriteRenderer.color = Color.Lerp(m_lastColor, m_targetColor, m_timeStamp);
            }

        }

        private void InitializeSlots()
        {
            Vector2Int slotCoords = new Vector2Int(m_coordinates.x, m_coordinates.y + 1);

            CreateSlotIfPossible(slotCoords);

            slotCoords.y--;
            slotCoords.x++;

            CreateSlotIfPossible(slotCoords);

            slotCoords.x -= 2;

            CreateSlotIfPossible(slotCoords);

            slotCoords.x++;
            slotCoords.y--;

            CreateSlotIfPossible(slotCoords);
        }

        void CreateSlotIfPossible(Vector2Int coords)
        {
            GridInfo info = GameState.Grid[coords.x, coords.y];

            if (!info.isOccupied && !info.hasSlot)
            {
                Slot.Create(coords.x, coords.y);
            }
        }

        private void SetColor(Color color)
        {
            m_lastColor = m_spriteRenderer.color;
            m_targetColor = color;
            m_timeStamp = 0.0f;
        }

        public void MakeBuilding(Building building)
        {
            this.building = building;
            if(building.ResourceType != RESOURCE_TYPE.TITANIUM) InitializeSlots();
            GameState.Grid.SetOccupied(m_coordinates.x, m_coordinates.y, true);
            m_spriteRenderer.sprite = building.GetSprite();
            m_spriteRenderer.color = m_hover;          
            m_lastColor = m_hover;
            m_targetColor = m_hover;
        }

        void OnMouseEnter()
        {
            SetColor(m_hover);
        }

        private void OnMouseDown()
        {
            if (UIManager.IsActive) return;
            SetColor(m_clicked);
            MENU_FLAGS flags = building == null ? MENU_FLAGS.BUILD : building.Flags;
            EventSystem.RemoveListener<Event_UIBuild>(OnBuild);
            EventSystem.RemoveListener<Event_UIUpgrade>(OnUpgrade);
            EventSystem.RemoveListener<Event_UIWorker>(OnWorker);

            List<GameObject> prefabs = new List<GameObject>();
            if (IsFlagSet(flags, MENU_FLAGS.BUILD))
            {
                prefabs.Add(GameObject.Instantiate(Resources.Load("Prefabs/Build")) as GameObject);   
                EventSystem.AddListener<Event_UIBuild>(OnBuild);
            }
            if (IsFlagSet(flags, MENU_FLAGS.UPGRADE))
            {
                prefabs.Add(GameObject.Instantiate(Resources.Load("Prefabs/Upgrade")) as GameObject);
                EventSystem.Dispatch(new Event_UIUpgrade());
            }
            if (IsFlagSet(flags, MENU_FLAGS.WORKER))
                prefabs.Add(GameObject.Instantiate(Resources.Load("Prefabs/Worker")) as GameObject);

            EventSystem.Dispatch(new Event_BuildMenu(prefabs.ToArray()));
            Debug.Log("Mouse down");

        }

        private void OnUpgrade(Event_UIUpgrade e)
        {

        }

        private void OnWorker(Event_UIWorker e)
        {

        }

        private void OnBuild(Event_UIBuild e)
        {
            EventSystem.RemoveListener<Event_UIBuild>(OnBuild);
            this.MakeBuilding(new Building(GameState.Instance.habitatSprite, null, null, RESOURCE_TYPE.LASAGNA, MENU_FLAGS.UPGRADE | MENU_FLAGS.WORKER, true));
        }

        private bool IsFlagSet(MENU_FLAGS flags, MENU_FLAGS flag)
        {
            return (flags & flag) == flag;
        }

        private void OnMouseExit()
        {
            if (building != null) SetColor(m_hover);
            else SetColor(m_default);
        }
    }
}