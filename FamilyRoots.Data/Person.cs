using System;

namespace FamilyRoots.Data
{
    public class Person
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string FamilyName { get; set; }

        public string[] MiddleNames { get; set; }

        public string MaidenName { get; set; }
        
        public Sex Sex { get; set; }
    }

    public enum Sex
    {
        Male,
        Female
    }
}