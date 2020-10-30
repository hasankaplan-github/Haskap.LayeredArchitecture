using System;
using System.Collections.Generic;
using System.Text;

namespace Haskap.LayeredArchitecture.Utilities.Providers
{
    public class UserIdProvider<TId>
    {
        public UserIdProvider()
        {
            UserId = default;
        }

        public TId UserId { get; set; }
    }
}
