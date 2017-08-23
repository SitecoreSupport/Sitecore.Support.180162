using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Events.Hooks;

namespace Sitecore.Support
{
    public class CustomCreator
    {
        public void Initialize()
        {
            Sitecore.Resources.Media.MediaManager.Creator = new CustomMediaCreator();
        }
    }
}