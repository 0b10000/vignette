// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System.Threading.Tasks;
using Vignette.Core.Extensions.Host;

namespace Vignette.Core.Tests.Extensions
{
    public class TestHostExtension : HostExtension
    {
        public override string Name => @"Test";
        public override string Description => @"Test";
        public override string Identifier => @"Test";

        [Listen("sayHello")]
        public string SayHello(string name)
        {
            return $"Hello {name} from {Name}";
        }

        [Listen("sayHelloAsync")]
        public Task<string> SayHelloAsync(string name)
        {
            return Task.FromResult($"Hello {name} from {Name}");
        }
    }
}
