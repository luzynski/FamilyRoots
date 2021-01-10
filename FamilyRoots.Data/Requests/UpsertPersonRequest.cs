using System;
using System.ComponentModel.DataAnnotations;
using FamilyRoots.Data.Validation;

namespace FamilyRoots.Data.Requests
{
    [ImmutableSex]
    public class UpsertPersonRequest
    {
        [GuidNotEmpty]
        public Guid? Id { get; set; }
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string FamilyName { get; set; }

        [BlankNotAllowed]
        public string[] MiddleNames { get; set; }

        [BlankNotAllowed]
        public string MaidenName { get; set; }
        
        public Sex? Sex { get; set; }
    }
}