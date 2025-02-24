namespace _2025.Services.AuthAPI
{
    public interface IHelloService
    {
        string sayHello(string name);
    }

    public class HelloService : IHelloService
    {
        public string sayHello(string name)
        {
            return "Xin chào: " + name;
        }
    }
}
