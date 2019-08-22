using System;

namespace Zaaby.Abstractions
{
    public class ZaabyError
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
    }
}