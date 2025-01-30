using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace reporting.repositories
{

    public abstract class IGenericRepository
{
        private readonly string _connection;
        public IGenericRepository(string connection)
        {
            _connection = connection;
        }
    }
}