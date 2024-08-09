using AutoMapper;
using CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar;
using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Persistance.Mapping
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCarCommand, Car>().ReverseMap();  // createMap ile command'dan car türüne dönüsüm yaparken, aynı zamanda reverseMap ile de Car'dan Command'a dönüşüm yapabileceğimizi söyülüyoruz.
        }
    }
}
