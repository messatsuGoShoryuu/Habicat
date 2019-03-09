using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jam
{
    public class Event_GameOver : BaseEventData
    {
    }

    public class Event_CatSpawned : BaseEventData
    {

    }

    public class Event_CatDied : BaseEventData
    {

    }

    public class Event_DayEnded : BaseEventData
    {

    }

    public class Event_UIText: BaseEventData
    {
        public Event_UIText(string text, bool add = false)
        {
            this.text = text;
            this.add = add;
        }

        public string text;
        public bool add;          
    }

    public class Event_BuildMenu: BaseEventData
    {
        public Event_BuildMenu(GameObject[] prefabs)
        {
            this.prefabs = prefabs;
        }

        public GameObject[] prefabs;
    }

    public class Event_BuildingYield : BaseEventData
    {
        public Event_BuildingYield(RESOURCE_TYPE type, float amount)
        {
            this.type = type;
            this.amount = amount;
        }
        public RESOURCE_TYPE type;
        public float amount;
    }

    public class Event_UIBuild: BaseEventData
    {

    }

    public class Event_UIUpgrade: BaseEventData
    {

    }

    public class Event_UIWorker : BaseEventData
    {

    }
}
