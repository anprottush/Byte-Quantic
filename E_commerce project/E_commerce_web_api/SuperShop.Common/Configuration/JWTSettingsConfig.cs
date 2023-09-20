using System;
using System.Collections.Generic;
using System.Text;

namespace SuperShop.Common.Configuration
{
    public class JWTSettingsConfig
    {
        public string Secret { get; set; }
        public int ExpiresInMinutes { get; set; }
    }
}
