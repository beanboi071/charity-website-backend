﻿using System.Globalization;

namespace charity_website_backend.Entities
{
    public class EDonor
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public string Password_Hash { get; set; }
        public string? Image_Path { get; set; }
        public decimal Balance { get; set; } = 1000000;
    }
}
