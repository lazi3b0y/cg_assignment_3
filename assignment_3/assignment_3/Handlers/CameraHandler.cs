using assignment_3.Components;
using assignment_3.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_3.Handlers
{
    public class CameraHandler
    {
        private static CameraHandler _instance = new CameraHandler();
        public CameraComponent ActiveCamera { get; set; }

        static CameraHandler()
        {

        }

        private CameraHandler()
        {

        }

        public static CameraHandler Instance
        {
            get { return _instance; }
        }


        public void SetActiveCameraFromEntity(Entity entity)
        {
            ActiveCamera = entity.GetComponent<CameraComponent>();
        }
    }
}
