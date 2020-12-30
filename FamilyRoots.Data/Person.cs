using System;

namespace FamilyRoots.Data
{
    public class Person
    {
        public Guid? Id { get; set; }
        
        public string Surname { get; set; }
        
        public string[] Names { get; set; }
        
        public Sex Sex { get; set; }
        
        public DateTime? BirthDate { get; set; }

        public string BirthPlace { get; set; }
        
        public DateTime? DeathDate { get; set; }
     
        public string BurialPlace { get; set; }
    }

    public enum Sex
    {
        Male,
        Female
    }
}