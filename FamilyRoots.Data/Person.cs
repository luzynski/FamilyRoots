using System;

namespace FamilyRoots.Data
{
    public class Person
    {
        public Person(string surname, params string[] names)
        {
            Surname = surname;
            Names = names;
        }

        public string Surname { get; }
        public string[] Names { get; }
        public DateTime BirthDate { get; set; }
    }
}