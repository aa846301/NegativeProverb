using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class BaseModel
    {
        public bool Success { get; set; }

        public string? Code { get; set; }

        public string? Exception { get; set; }

        public object? Data { get; set; }

    }

    public class BaseModel<T>
    {
        public bool Success { get; set; }

        public string? Code { get; set; }

        public string? Exception { get; set; }

        public T? Data { get; set; }

    }
}
