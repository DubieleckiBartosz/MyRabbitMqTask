using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiConsumer.Models.Requests
{
    public class SearchMessages: SearchParameters
    {
        private DateTime _toDate = DateTime.Now;

        [Required]
        public DateTime FromDate { get; set; }
        public DateTime ToDate
        {
            get => _toDate;
            set => _toDate = (value == default) ? _toDate : value;
        }
        public int ContentLength { get; set; }

        public string Content { get; set; }
    }
}
