﻿using System.ComponentModel.DataAnnotations;

namespace customerChallenge.models
{
    public class Customer
    {

        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string name { get; set; }

        [Required]
        [StringLength(200)]
        [EmailAddress]
        public string email { get; set; }

    }
}
