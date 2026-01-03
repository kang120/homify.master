using MasterDatabase.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MasterLib.Dtos.Master.User;
using MasterService.Dtos.Master.Unit;

namespace MasterService.MappingProfiles
{
    public class UnitMappingProfile : Profile
    {
        public UnitMappingProfile()
        {
            CreateMap<UnitCreateDto, Unit>();
            CreateMap<UnitUpdateDto, Unit>();
        }
    }
}
