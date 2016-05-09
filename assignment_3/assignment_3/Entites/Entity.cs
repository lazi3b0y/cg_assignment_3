using assignment_3.Components;
using assignment_3.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.Entites
{
    public class Entity
    {
        public void AddComponent(Component component)
        {
            ComponentHandler.Instance.MapComponent(this, component);
        }

        public T GetComponent<T>() where T : Component
        {
            return ComponentHandler.Instance.GetComponent<T>(this);
        }
    }
}
