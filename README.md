# Human Date Parser
Parser human readable dates to `System.DateTime`. Based on TimeStamper and updated to work with Asp.Net Core/.NET Framwork.

Installation:  
https://www.nuget.org/packages/HumanDateParser/
---
**Nuget Package Manager:**
```
Install-Package HumanDateParser -Version 1.1.0
```

**Dotnet CLI:**
```
dotnet add package HumanDateParser --version 1.1.0
```


Usage:
---
```csharp
DateParser.Parse("1 month ago")
DateParser.Parse("after 15 days")
DateParser.Parse("15th feb 2010 at 5:30pm")
```

Contribute:
---
Please add more unit tests create a pull request.
