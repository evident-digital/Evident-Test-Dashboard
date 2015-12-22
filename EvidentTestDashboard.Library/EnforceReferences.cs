using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvidentTestDashboard.Library
{
    internal class EnforceReferences
    {
        // If you don't have a direct reference to EntityFramework.SqlServer.dll, 
        // this file won't be copied to the web project..
        // Also see: http://stackoverflow.com/questions/14033193/entity-framework-provider-type-could-not-be-loaded
        private EnforceReferences()
        {
            var x = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }
    }
}
