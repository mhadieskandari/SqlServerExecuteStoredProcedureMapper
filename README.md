# SqlServerExecuteStoredProcedureMapper
It is an extenion for mapping data to your model that returns by efcore stored procedure executed.

## Usage
You can use like this:

```csharp
//var parameters = new List<SqlParameter>();
  
//parameters.Add(new SqlParameter("LetterId", 80));          

using(var context= new MyDBContext()) // or use dbcontext as dependency injection
{
  //var results = context.ExecuteSqlMapper<DabirName>("SelectAllLetter", parameters);
  var results= context.ExecuteSqlMapper<DabirName>("SelectAllLetter", new SqlParameter("LetterId", 80));           
}
  
// model that passed to context.ExecuteSqlMapper<DabirName>
public class DabirName 
{  
  public long LetterId { set; get; }
  public long AndicatorNo { set; get; }
}
```
