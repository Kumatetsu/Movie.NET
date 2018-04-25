using ClassLibrary1.Interface;
using ClassLibrary1.ServiceFacade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Factory
{
    class ServiceFacadeFactory
    {
        static IServiceFacade serviceFacade = null;

        static IServiceFacade GetServiceFacade() {
            if (null == serviceFacade)
            {
                serviceFacade = new ServiceFacade.ServiceFacade();
            }

            return serviceFacade;
        }
    }
}
