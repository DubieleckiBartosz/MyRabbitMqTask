using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiConsumer.Models.Requests
{
    public abstract class SearchParameters
    {
        private int _maxPageSize = 50;
        private int _defaultPageNumber = 1;
        private int _pageSize = 10;
        public int PageNumber
        {
            get => _defaultPageNumber;
            set => _defaultPageNumber = (value < 1 || value == default) ? _defaultPageNumber : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
        }
    }
}
