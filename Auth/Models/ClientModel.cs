﻿using System;

namespace BarberAPI.Auth.Models
{
    public class ClientModel
    {
        public Guid Gd { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
    }
}
