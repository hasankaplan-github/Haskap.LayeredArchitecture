using System;
using System.Collections.Generic;
using System.Text;

namespace Haskap.LayeredArchitecture.Services
{
    public class UserIdProviderService<TId>
    {
        public UserIdProviderService()
        {
            UserId = default;
        }

        public TId UserId { get; set; }
    }
}
