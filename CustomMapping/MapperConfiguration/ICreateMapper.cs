using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace CustomMapping.Model.Common
{
    interface ICreateMapper<TSource>
    {
        void Map(Profile profile)
        {
            profile.CreateMap(typeof(TSource), GetType()).ReverseMap();
        }
    }
}
