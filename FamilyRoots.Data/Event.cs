using System;

namespace FamilyRoots.Data
{
    public abstract class Event
    {
        public Guid Id { get; set; }
        
        public Type Type
        {
            get
            {
                switch (this)
                {
                    case BirthEvent _:
                        return Type.Birth;
                    case DeathEvent _:
                        return Type.Death;
                    case MarriageEvent _:
                        return Type.Marriage;
                    //TODO: add more types
                    default:
                        throw new ApplicationException($"Unsupported event type: {GetType().Name}");
                }
            }
        }
    }

    public enum Type
    {
        Birth,
        Death,
        Marriage
    }

    public class BirthEvent : Event
    {
        public Guid ChildId { get; set; }

        public Guid? FatherId { get; set; }

        public Guid? MotherId { get; set; }

        public DateTime? BirthDate { get; set; }
     
        public string BirthPlace { get; set; }
    }
    
    public class DeathEvent : Event
    {
        public Guid PersonId { get; set; }
        
        public DateTime? DeathDate { get; set; }
     
        public string BurialPlace { get; set; }
    }
    
    public class MarriageEvent : Event
    {
        public Guid FirstSpouseId { get; set; }
        
        public Guid SecondSpouseId { get; set; }
        
        public DateTime? MarriageDate { get; set; }
        
        public DateTime? DivorceDate { get; set; }
    }
}