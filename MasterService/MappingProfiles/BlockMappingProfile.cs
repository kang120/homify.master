using MasterDatabase.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MasterLib.Dtos.Master.User;
using MasterService.Dtos.Master.Block;

namespace MasterService.MappingProfiles
{
    public class BlockMappingProfile : Profile
    {
        public BlockMappingProfile()
        {
            CreateMap<BlockCreateDto, Block>();
            CreateMap<BlockUpdateDto, Block>();
        }
    }
}
