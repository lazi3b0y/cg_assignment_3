using assignment_3.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.Handlers
{
    public class SystemHandler
    {
        private static SystemHandler _instance = new SystemHandler();
        private readonly ICollection<UpdateSystem> _updateSystems = new List<UpdateSystem>();
        private readonly ICollection<RenderSystem> _renderSystems = new List<RenderSystem>();

        static SystemHandler()
        {

        }

        private SystemHandler()
        {

        }

        public static SystemHandler Instance
        {
            get { return _instance; }
        }

        public void AddSystem(assignment_3.Systems.System system)
        {
            if (system is RenderSystem)
                _renderSystems.Add((RenderSystem)system);
            else if (system is UpdateSystem)
                _updateSystems.Add((UpdateSystem)system);
        }

        public void Render(GameTime gameTime)
        {
            foreach (var system in _renderSystems)
            {
                system.Render(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var system in _updateSystems)
            {
                system.Update(gameTime);
            }
        }
    }
}
