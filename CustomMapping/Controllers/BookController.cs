using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomMapping.Service;
using CustomMapping.Service.DataShaping;
using Microsoft.AspNetCore.Mvc;

namespace CustomMapping.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;

        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public IActionResult Get(string fields)
        {
            var model = _bookService.GetFirstBook();

            if(string.IsNullOrEmpty(fields))
                return Ok(model);

            if (!model.HasFields(fields))
                return BadRequest("Required fields does not exist");

            var shapedData = model.ShapeData(fields);

            return Ok(shapedData);
        }
    }
}
