using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiConsumer.Models.Requests
{
    public class SearchSlots: SearchParameters
    {
        private int _fromId = 1;

        public int FromId
        {
            get => _fromId;
            set => _fromId = (value > 1) ? value : _fromId;
        }
    }
}
