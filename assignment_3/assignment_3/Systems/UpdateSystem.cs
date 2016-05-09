using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.Systems
{
    public abstract class UpdateSystem : System
    {
        public abstract void Update(GameTime gameTime);
    }
}
