using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomMapping.Service;
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
        public IActionResult Get()
        {
            var model = _bookService.GetFirstBook();

            return Ok(model);
        }
    }
}
