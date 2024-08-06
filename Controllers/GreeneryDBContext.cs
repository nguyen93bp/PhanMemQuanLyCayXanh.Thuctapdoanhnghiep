using System;
using System.Collections;

namespace QuanLyCayXanh.Controllers
{
    internal class GreeneryDBContext
    {
        public IEnumerable Trees { get; internal set; }
        public object Fertilizers { get; internal set; }

        internal void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}