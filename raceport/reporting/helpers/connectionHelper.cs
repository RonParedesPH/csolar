using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace stockroomDap.helpers
{
    public static class connectionHelper
    {
        public static string GetConnectionString => ConfigurationManager.ConnectionStrings["stockroom"].ConnectionString;

    }
}