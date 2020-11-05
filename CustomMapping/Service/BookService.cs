﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomMapping.Model.Dtos;
using CustomMapping.Model.Models;

namespace CustomMapping.Service
{
    public class BookService //For Testing
    {
        private readonly IMapper _mapper;

        public BookService(IMapper mapper)
        {
            _mapper = mapper;
        }

        private List<Books> bookList = new List<Books>
        {
            new Books{Id = 1,Isbn = "ASD123",Name = "First Book",Writer = "Bob"},
            new Books{Id = 2,Isbn = "ABC123",Name = "Second Book",Writer = "Bob"}
        };


        public Books GetFirstBook()
        {
            return bookList.FirstOrDefault();
        }

        public Books GetBookById(int id)
        {
            return bookList.FirstOrDefault(b => b.Id == id);
        }

        public List<Books> GetAllBooks()
        {
            return bookList;
        }
    }
}
