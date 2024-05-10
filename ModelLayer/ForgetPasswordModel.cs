using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer
{
    public class ForgetPasswordModel
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

    }
}
