using assignment_3.Components;
using assignment_3.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.Handlers
{
    public class ComponentHandler
    {
        private static ComponentHandler _instance = new ComponentHandler();
        private readonly Dictionary<Type, Dictionary<Entity, Component>> _componentsDictionary = new Dictionary<Type, Dictionary<Entity, Component>>();

        static ComponentHandler()
        {

        }

        private ComponentHandler()
        {

        }

        public static ComponentHandler Instance
        {
            get { return _instance; }
        }

        public void MapComponent(Entity entity, Component component)
        {
            var t = component.GetType();

            component.Owner = entity;
            if (!_componentsDictionary.ContainsKey(t))
                _componentsDictionary[t] = new Dictionary<Entity, Component>();

            _componentsDictionary[t][entity] = component;
        }

        public IEnumerable<T> GetAllComponents<T>() where T : Component
        {
            var t = typeof(T);
            if (!_componentsDictionary.ContainsKey(t)) return null;

            return _componentsDictionary[t].Values.Select(i => (T)i).ToList();
        }

        public T GetComponent<T>(Entity entity) where T : Component
        {
            var t = typeof(T);

            if (!_componentsDictionary.ContainsKey(t))
                return default(T);

            if (_componentsDictionary[t].ContainsKey(entity))
                return (T)_componentsDictionary[t][entity];

            return default(T);
        }
    }
}
