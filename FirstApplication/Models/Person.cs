namespace FirstApplication.Models
{
    public class Person
    {
        public Person(int id, string name, string lastName)
        {
            Id = id;
            Name = name;
            LastName = lastName;
        }

        public int Id { get;private set; }
        public string Name { get; private set; }
        public string LastName { get; set; }
        
    }
}
