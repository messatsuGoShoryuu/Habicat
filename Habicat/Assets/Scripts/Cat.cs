using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jam
{
    public enum TASK
    {
        NONE,
        TITANIUM,
        LASAGIUM
    }
    public class Cat
    {
        private TASK m_currentTask;
        private int m_currentRoom;
        private float m_creationDate;

        public float CreationDate
        {
            get
            {
                return m_creationDate;
            }
        }

        public Cat(int room)
        {
            m_creationDate = Time.time;
            m_currentRoom = room;
            Debug.Log("Cat spawend in room " + room);
        }

        public static int Sort(Cat lhs, Cat rhs)
        {
            if (lhs.m_creationDate < rhs.m_creationDate) return -1;
            else if (lhs.m_creationDate > rhs.m_creationDate) return 1;
            return 0;
        }

        public void Die()
        {
            EventSystem.Dispatch(new Event_CatDied());
        }
    }

}