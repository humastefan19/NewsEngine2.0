﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NewsEngine2._0.Startup))]
namespace NewsEngine2._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
