﻿using BasicDependencyInjection;
using BasicDependencyInjection.Container;

namespace WorkConsoleApp
{
    public static class Startup
    {
        public static IBasicContainer BuildContainer()
        {
            var container = new BasicContainer();

            //TODO: register classes and stuff here

            return container;
        }
    }
}