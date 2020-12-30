using System;

namespace FamilyRoots.Data.Requests
{
    public class CreatePersonRequest
    {
        public string Surname { get; set; }
        
        public string[] Names { get; set; }
        
        public Sex Sex { get; set; }
        
        public DateTime? BirthDate { get; set; }

        public string BirthPlace { get; set; }
        
        public DateTime? DeathDate { get; set; }
     
        public string BurialPlace { get; set; }
    }
}