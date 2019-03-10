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
        [SerializeField]
        private Canvas m_canvas;

        private static bool m_isActive = false;
        public static bool IsActive { get { return m_isActive; } }

        private void Awake()
        {
            EventSystem.AddListener<Event_BuildMenu>(OnBuildMenu);
            EventSystem.AddListener<Event_UIBuild>(OnUIBuild);
            EventSystem.AddListener<Event_UIUpgrade>(OnUIUpgrade);
            EventSystem.AddListener<Event_UIWorker>(OnUIWorker);
        }

        void OnUIBuild(Event_UIBuild e)
        {
            Deactivate();
        }

        void OnUIWorker(Event_UIWorker e)
        {
            Deactivate();
        }

        void OnUIUpgrade(Event_UIUpgrade e)
        {
            Deactivate();
        }

        private void ClearChildren()
        {
            int count = m_buildMenuPanel.transform.childCount;
            for (int i = 0; i < count; ++i)
            {
                GameObject.Destroy(m_buildMenuPanel.transform.GetChild(0).gameObject);
            }
        }

        private void Activate()
        {
            m_buildMenuPanel.SetActive(true);
            RectTransform rt = m_canvas.transform as RectTransform;

            Vector2 transformPoint = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, m_canvas.worldCamera, out transformPoint);
            m_buildMenuPanel.transform.position = m_canvas.transform.TransformPoint(transformPoint);
            m_isActive = true;
        }

        private void Deactivate()
        {
            ClearChildren();
            m_buildMenuPanel.SetActive(false);
            m_isActive = false;
        }

        void OnBuildMenu(Event_BuildMenu e)
        {
            if (e.prefabs.Length == 0) return;
            Activate();
            for (int i = 0; i < e.prefabs.Length; ++i)
            {
                GameObject go = GameObject.Instantiate(e.prefabs[i]);
                go.transform.parent = m_buildMenuPanel.transform;
            }
        }
    }
}
