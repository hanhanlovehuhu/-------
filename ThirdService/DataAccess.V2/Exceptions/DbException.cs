using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.V2.Exceptions
{
    public class DbException : System.Exception
    {
        public DbException()
            : base()
        {
        }
        public DbException(string message)
            : base(message)
        {
        }
        public DbException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
