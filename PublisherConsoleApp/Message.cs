using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublisherConsoleApp
{
    public class Message
    {
        public string ElementId { get; private set; }
        public string Content { get; private set; }
        public string Type { get; private set; }
        public DateTime Date { get; private set; }

        public Message(string content,string type)
        {
            ElementId = Guid.NewGuid().ToString();
            Content = content;
            Type = type; 
            Date = DateTime.Now;
        }
    }
}
