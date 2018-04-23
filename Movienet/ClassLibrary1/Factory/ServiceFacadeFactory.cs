using ClassLibrary1.Interface;
using ClassLibrary1.ServiceFacade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Factory
{
    public class ServiceFacadeFactory
    {
        static IServiceFacade serviceFacade = null;
        static public IServiceFacade GetServiceFacade() {

            if (null == serviceFacade)
            {
                serviceFacade = new ServiceFacade.ServiceFacade();
            }

            return serviceFacade;
        }
    }
}
