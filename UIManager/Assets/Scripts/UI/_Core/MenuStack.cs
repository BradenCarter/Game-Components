using System.Collections.Generic;

namespace UISystem
{
    public class MenuStack
    {
        private List<Menu> items = new List<Menu>();
        public int Count { get { return items.Count; } }

        public void Push(Menu item)
        {
            items.Add(item);
        }

        public Menu Peek()
        {
            return items[items.Count - 1];
        }

        public Menu Pop()
        {
            if (items.Count > 0)
            {
                Menu temp = items[items.Count - 1];
                items.RemoveAt(items.Count - 1);
                return temp;
            }
            else return null;
        }

        public void Remove(Menu item)
        {
            items.Remove(item);
        }

        public void PlaceAtBottom(Menu item)
        {
            items.Insert(0, item);
        }
    }
}