using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomMapping.Model.Common;
using CustomMapping.Model.Models;

namespace CustomMapping.Model.Dtos
{
    public class BookDto:ICreateMapper<Books>
    {
        public string Name { get; set; }
        public string Isbn { get; set; }

    }
}
