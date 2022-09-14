# SAPI - Simple API
SAPI is a library for creating APIs with C#. It's simple by design and allows for a lot of flexibility.

## Installation
Add as dependency in NuGet
```shell
Install-Package SAPI -ProjectName <project>
```
or in your preffered IDE

you can also use SAPI by [downloading](https://github.com/Maciejowski2006/SAPI/releases) and referencing the DLL in your project.

## Usage

```csharp
// Program.cs
using SAPI;
using SAPI.Endpoints;
using Project.Endpoints;

public static void Main(string[] args)
{
    Server sapi = new Server("http://localhost:8000/");
    
    // Init HttpListener
    sapi.Init();
    
    // Mount endpoints(routes)
    sapi.MountEndpoint(new Ping("ping", Method.GET))
    
    
    // Finally start SAPI
    sapi.Start();
}
```
```csharp
// Endpoints/Ping.cs
using System.Net;
using System.Text;
using SAPI.Endpoints;

namespace Project.Endpoints
{
    public class Ping : Endpoint
    {
        public override void Task(ref HttpListenerRequest request, ref HttpListenerResponse response)
        {
            Console.WriteLine("Ping!");
            
            response.StatusCode = 200;
            byte[] data = Encoding.UTF8.GetBytes("Pong!");
            response.ContentType = "text/html";
            response.ContentEncoding = Encoding.UTF8;
            response.ContentLength64 = data.LongLength;
            
            await response.OutputStream.WriteAsync(data, 0, data.Length);
        }
        public Ping(string url, Method method) : base(url, method) { }
    }
}
```