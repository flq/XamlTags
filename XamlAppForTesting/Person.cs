namespace XamlAppForTesting
{

    public interface ILifeform
    {
        
    }
    
    public class Person
    {
        
    }

    public class Employee : Person
    {
        
    }

    public class Slave : Person, ILifeform
    {
        
    }

    public class Boss : Employee
    {
        
    }
}