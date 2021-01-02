using System;

namespace FamilyRoots.Data
{
    public class Person
    {
        public Guid Id { get; set; }
        
        public string Surname { get; set; }
        
        public string[] Names { get; set; }
        
        public Sex Sex { get; set; }
    }

    public enum Sex
    {
        Male,
        Female
    }
}