using System.Collections.Generic;
using UnityEngine;
 
namespace UISystem
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance { get; private set; }

        protected MenuStack menuStack = new MenuStack();

        [SerializeField]
        private Transform uiContainer;

        private void Awake()
        {
            Instance = this;
            if (uiContainer == null)
                uiContainer = transform;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        internal virtual Menu OpenMenu<T>() where T : Menu
        {
            var prefab = GetPrefab<T>();
            var instance = Instantiate(prefab, uiContainer);
            instance.name = typeof(T).ToString();
            instance.transform.SetAsLastSibling();
            instance.gameObject.SetActive(true);
            OpenTopMenu();

            menuStack.Push(instance);
            return instance;
        }

        internal void CloseMenu(Menu menu)
        {
            menuStack.Remove(menu);
            if (menu.DestroyWhenClosed)
                Destroy(menu.gameObject);
            else menu.gameObject.SetActive(false);
            OpenTopMenu();
        }

        private void OpenTopMenu()
        {
            if (menuStack.Count > 0)
            {
                menuStack.Peek().gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && menuStack.Count > 0)
                menuStack.Peek().OnBack_Pressed();
        }

        private T GetPrefab<T>() where T : Menu
        {
            GameObject menu = (GameObject)Resources.Load("UI/" + typeof(T));
            if (menu == null)
                throw new MissingReferenceException("Prefab not found for type " + typeof(T));
            return menu.GetComponent<T>();
        }
    }
}