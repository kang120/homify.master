using System;
using System.Collections.Generic;
using System.Text;

namespace MasterService.Dtos.Master.Auth
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
