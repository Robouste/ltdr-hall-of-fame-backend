using AutoMapper;
using ltdr_hall_of_fame_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ltdr_hall_of_fame_backend.ViewModels
{
    public class Mapping: Profile
    {
        public Mapping()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<Joke, JokeViewModel>();
        }
    }
}
