using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application5.Messages
{
    internal class SearchMessage
    {
        public string Criteria { get; set; }

        public SearchMessage(string criteria)
        {
            Criteria = criteria;
        }
    }
}