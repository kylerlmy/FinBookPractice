using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.ViewModels
{
    public class TagContactViewModel
    {
        public int ContactId { get; set; }


        public List<string> Tags { get; set; }
    }
}
