using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace jam
{
    public enum MENU_FLAGS
    {
        NONE = 0,
        BUILD = 1,
        UPGRADE = (1 << 1),
        WORKER = (1 << 2)
    }

    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_buildMenuPanel;

        private void Awake()
        {
            EventSystem.AddListener<Event_BuildMenu>(OnBuildMenu);
            EventSystem.AddListener<Event_UIBuild>(OnUIBuild);
        }

        void OnUIBuild(Event_UIBuild e)
        {
            ClearChildren();
            m_buildMenuPanel.SetActive(false);
        }

        private void ClearChildren()
        {
            int count = m_buildMenuPanel.transform.childCount;
            for (int i = 0; i < count; ++i)
            {
                GameObject.Destroy(m_buildMenuPanel.transform.GetChild(0).gameObject);
            }
        }

        void OnBuildMenu(Event_BuildMenu e)
        {
 

            m_buildMenuPanel.SetActive(true);
            for (int i = 0; i<e.prefabs.Length;++i)
            {
                GameObject go = GameObject.Instantiate(e.prefabs[i]);
                go.transform.parent = m_buildMenuPanel.transform;
            }

        }
    }
}
