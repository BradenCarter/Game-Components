using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    public abstract class Menu<T> : Menu where T : Menu<T>
    {
        public static T Instance { get; private set; }

        public static bool IsOpen
        { get { return Instance != null && Instance.gameObject.activeSelf; } }

        public event Action onOpen;
        public event Action onClose;

        protected virtual void Awake()
        {
            Instance = (T)this;
        }

        protected virtual void OnDestroy()
        {
            Instance = null;
        }

        public static void Open()
        {
            if (Instance != null)
            {
                if (Instance.DestroyWhenClosed)
                {
                    Instance.transform.SetAsLastSibling();
                    return;
                }
                else
                {
                    Instance.gameObject.SetActive(true);
                    Instance.transform.SetAsLastSibling();
                    if (Instance.onOpen != null)
                        Instance.onOpen();
                    return;
                }
            }

            MenuManager.Instance.OpenMenu<T>();
            if (Instance.onOpen != null)
                Instance.onOpen();
        }

        public static Menu MenuOpen()
        {
            if (Instance != null)
            {
                if (Instance.DestroyWhenClosed)
                    return Instance;
                else
                {
                    Instance.gameObject.SetActive(true);
                    Instance.transform.SetAsLastSibling();
                    if (Instance.onOpen != null)
                        Instance.onOpen();
                    return Instance;
                }
            }

            Menu menuOpened = MenuManager.Instance.OpenMenu<T>();

            if (Instance.onOpen != null)
                Instance.onOpen();

            return menuOpened;
        }

        public static void Close()
        {
            if (Instance == null)
            {
                Debug.LogErrorFormat("No menu of type: " + typeof(T).Name + " exists");
                return;
            }

            MenuManager.Instance.CloseMenu(Instance);
            if (Instance.onClose != null)
                Instance.onClose();
        }

        public override void OnBack_Pressed()
        {
            Close();
        }
    }

    public abstract class Menu : MonoBehaviour
    {
        public bool DestroyWhenClosed = true;
        public bool DisableMenusUnderneath = true;

        public abstract void OnBack_Pressed();
    }
}