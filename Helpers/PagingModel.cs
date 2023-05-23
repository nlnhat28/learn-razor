using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAZOR_EF.Helpers
{
    public class PagingModel
    {
        public int currentPage { get; set; }
        public int countPages { get; set; }
        public Func<int?, string> generateUrl {get; set;}

    }
}