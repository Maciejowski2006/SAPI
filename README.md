# SAPI - Simple API
![Nuget](https://img.shields.io/nuget/dt/SAPI?color=%20%230390fc&label=Downloads)
[![NuGet](https://img.shields.io/nuget/v/SAPI?color=%20%230390fc&label=NuGet)](https://www.nuget.org/packages/SAPI)
![GitHub](https://img.shields.io/github/license/Maciejowski2006/SAPI?color=%20%230390fc&label=License)

SAPI is a library for creating APIs with C#. It's simple by design and allows for a lot of flexibility.

## Installation
Add as dependency in NuGet
```shell
Install-Package SAPI -ProjectName <project>
```
In your preferred IDE:
![SAPI in rider's NuGet PM](https://github.com/Maciejowski2006/SAPI/blob/master/Screenshots/docs%20v1/nuget.png)

By [downloading](https://github.com/Maciejowski2006/SAPI/releases) and referencing the DLL (and its dependencies) in your project.

## Usage
For detailed explanation You can also see [docs](https://docs.maciejowski.me/)
```csharp
// Program.cs
using SAPI;
using SAPI.Endpoints;
using Project.Endpoints;

public static void Main(string[] args)
{
    // Init SAPI
    Server sapi = new();
    sapi.Start();
}
```
```csharp
// Endpoints/Ping.cs
using System.Net;
using SAPI;

namespace Project.Endpoints
{
    public class Ping : Endpoint
    {
        public override string url { get; } = "ping";

        private override void Get(ref Packet packet)
        {
            Console.WriteLine("Ping!");
            
            Error.Page(HttpStatus.EnhanceYourCalm, ref packet);
        }
    }
}
```
