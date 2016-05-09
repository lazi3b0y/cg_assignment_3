using assignment_3.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.Systems
{
    public class System
    {
        protected ComponentHandler ComponentHandler = ComponentHandler.Instance;
        protected CameraHandler CameraHandler = CameraHandler.Instance;
    }
}
