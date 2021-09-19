using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiConsumer.Models
{
    public class CreateSlotInfo
    {
        public int NumberMessages { get; set; }
        public string FirstAndLastCharacters { get; set; }
        public string ChangedCharacters { get; set; }
        public string MaxTimeBetweenContentChanges { get; set; }
    }
}
