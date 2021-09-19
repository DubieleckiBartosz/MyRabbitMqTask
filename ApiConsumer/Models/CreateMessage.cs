using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiConsumer.Models
{
    public class CreateMessage
    {
        public string Content { get; set; }
        public string ElementId { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
    }
}
