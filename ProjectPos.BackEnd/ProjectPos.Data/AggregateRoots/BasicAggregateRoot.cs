using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Data.AggregateRoots
{
    public class BasicAggregateRoot<T>
    {
        public T? Id { get; set; }
    }
}
