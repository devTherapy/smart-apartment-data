using System.Collections.Generic;

namespace Smart_Data.Application.Dtos
{
    public  class SearchParams 
    {

        public SearchParams(SearchQueryParams parameters)
        {
            Keyword = parameters.Keyword;
            Market = parameters.Market;
            Size = parameters.Size;
            PageNumber = parameters.PageNumber;
        }
        public int Offset { get; private set; }
        public string Keyword { get; set; }
        public ICollection<string> Market { get; set; }
        public int Size { get; set; }
        private int _pageNumber { get; set; }
        public int PageNumber
        {
            get { return _pageNumber;}
            set
            {
                _pageNumber = value;
                Offset = (_pageNumber - 1) * Size;
            }
        }

    }
}
