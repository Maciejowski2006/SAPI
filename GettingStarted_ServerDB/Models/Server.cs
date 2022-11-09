namespace ServerAPI.Models;

public class Server
{
	public string Name { get; }
	public string Message { get; }
	
	public Server(string name, string message)
	{
		Name = name;
		Message = message;
	}
}
