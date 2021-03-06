﻿using AutoMapper;
using BlowTrial.Domain.Tables;
using BlowTrial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlowTrial.Infrastructure.AutoMapper
{
    class BackupConfigurations : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<BackupData, CloudDirectoryModel>()
                .ForMember(d => d.Error, o => o.Ignore());
            Mapper.CreateMap<BackupService, CloudDirectoryModel>()
                .ForMember(d => d.Error, o => o.Ignore())
                .ForMember(d => d.BackupIntervalMinutes, o => o.MapFrom(s => s.IntervalMins));
        }
    }
}
