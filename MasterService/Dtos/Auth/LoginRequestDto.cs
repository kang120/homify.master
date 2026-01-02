using System;
using System.Collections.Generic;
using System.Text;

namespace MasterService.Dtos.Master.Auth
{
    public class LoginRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
