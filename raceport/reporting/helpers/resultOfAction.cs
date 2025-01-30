using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reporting.helpers
{
    public class ResultOfAction
    {
        public bool Success { get; set; } = false;
        public List<string> Errors = new List<string>();
    }

}