﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.EntityDtos
{
    public class EntityDto<T>
    {
        public T? Id { get; set; }
    }
}
