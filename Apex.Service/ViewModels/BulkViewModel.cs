using System;
using System.Collections.Generic;

namespace Apex.Service.ViewModels
{
    public class BulkViewModel<T>
    {
        public List<T> Succeeded { get; set; } = new List<T>();
        public List<Tuple<T, string>> Failed { get; set; } = new List<Tuple<T, string>>();
    }
}
