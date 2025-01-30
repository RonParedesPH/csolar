using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reporting.helpers
{
    public static class ExceptionHelper
    {
        public static string RollUp(Exception ex)
        {
            string message = ex.Message.IndexOf("See the inner exception") == -1 ? ex.Message : "";
            var tit = ex.InnerException;
            while (tit != null)
            {
                if (tit.Message.IndexOf("See the inner exception") == -1)
                    message += "\r\n" + tit.Message;
                tit = tit.InnerException;
            }

            return message;
        }

        public static string Verbose(Exception ex)
        {
            string message = ex.Message;
            var tit = ex.InnerException;
            while (tit != null)
            {
                message += "\r\n" + tit.Message;
                tit = tit.InnerException;
            }

            return message;
        }
    }
}