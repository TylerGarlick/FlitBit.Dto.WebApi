﻿using FlitBit.Wireup;
using FlitBit.Wireup.Meta;

[assembly: WireupDependency(typeof(FlitBit.IoC.AssemblyWireup))]
[assembly: Wireup(typeof(AssemblyWireup))]
namespace FlitBit.Dto.WebApi
{
    public class AssemblyWireup : IWireupCommand
    {
        public void Execute(IWireupCoordinator coordinator)
        {
        }
    }
}
