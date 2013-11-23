using System;
using System.Web.Mvc;
using StructureMap;


namespace CreativeFactory.Web.Infrastructure
{
    public class CreativeFactoryControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return ObjectFactory.GetInstance(controllerType) as IController;
        }
    }
}