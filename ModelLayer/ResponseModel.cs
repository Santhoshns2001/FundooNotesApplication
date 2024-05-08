﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer
{
    public class ResponseModel <T>
    {
        public bool IsSuccuss { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
