using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jam
{
    public enum UIBUTTON_TYPE
    {
        BUILD,
        WORKER,
        UPGRADE
    }
    public class UIButton : MonoBehaviour
    {
        public UIBUTTON_TYPE type;

        public void BtnClick()
        {
            switch (type)
            {
                case UIBUTTON_TYPE.BUILD:
                    EventSystem.Dispatch(new Event_UIBuild());
                    break;
                case UIBUTTON_TYPE.WORKER:
                    EventSystem.Dispatch(new Event_UIWorker());
                    break;
                case UIBUTTON_TYPE.UPGRADE:
                    EventSystem.Dispatch(new Event_UIUpgrade());
                    break;

            }
        }
    }
}
