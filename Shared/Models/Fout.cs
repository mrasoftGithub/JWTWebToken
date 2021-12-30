using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTWebToken.Shared.Models
{
    public class Fout
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int StatusCode { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }
        public string TraceId { get; set; }
    }
}