using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jam
{
    public class LifetimeManager : MonoBehaviour
    {
        [SerializeField]
        private int m_initialCatCount;
        private List<Cat> m_cats = new List<Cat>();
        public static LifetimeManager Instance;

        public static int CatCount { get { return Instance.m_cats.Count; } }

        // Start is called before the first frame update
        void Awake()
        {
            Instance = this;
            EventSystem.AddListener<Event_CatSpawned>(OnCatSpawned);
            EventSystem.AddListener<Event_DayEnded>(OnDayEnd);

            for (int i = 0; i < m_initialCatCount; ++i) m_cats.Add(new Cat(0));
        }

        private void OnCatSpawned(Event_CatSpawned e)
        {
            m_cats.Add(new Cat(0));
            m_cats.Sort(Cat.Sort);

            for (int i = 0; i < m_cats.Count; ++i)
                Debug.Log(m_cats[i].CreationDate);
        }

        private void OnDayEnd(Event_DayEnded e)
        {

            for (int i = m_cats.Count - 1; i >= 0; --i)
            {
                bool live = GameState.ConsumeLasagna(1);
                if (!live)
                {
                    m_cats[i].Die();
                    m_cats.Remove(m_cats[i]);
                }
            }

            EventSystem.Dispatch(new Event_UIText(""));
            string uiString = "";
            for (int i = 0; i < m_cats.Count; ++i)
            {
                uiString += (string.Format("Cats[{0}].CreationDate = {1}\n", i, m_cats[i].CreationDate));
                EventSystem.Dispatch(new Event_UIText(uiString));
            }
        }
    }
}
