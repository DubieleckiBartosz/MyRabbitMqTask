using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace ApiConsumer.Models
{
    public class SlotInfo
    {
        public int Id { get; set; }
        public int NumberMessages { get; set; }
        public string FirstAndLastCharacters { get; set; }
        public string ChangedCharacters { get; set; }
        public string MaxTimeBetweenContentChanges { get; set; }
    }
}
