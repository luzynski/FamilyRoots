namespace FamilyRoots.Data
{
    public class Person
    {
        public Person(string surname, string[] names)
        {
            Surname = surname;
            Names = names;
        }

        public string Surname { get; }
        public string[] Names { get; }
    }
}