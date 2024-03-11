## Scafold Database

```powershell
dotnet ef dbContext scaffold "Name=ConnectionStrings:eMeter" Microsoft.EntityFrameworkCore.SqlServer --context-dir Data --output-dir Entities
```