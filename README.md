# SqlServerExecuteStoredProcedureMapper
it is an extenion for mapping data to your model that returned from efcore stored procedure executed.


you can use like this:


var parameters = new List<SqlParameter>();
parameters.Add(new SqlParameter("LetterId", 80));          

using(var context= new ...DBContext())//or use dbcontext as dependency injection
{
var results= context.ExecuteSqlMapper<DabirName>("SelectAllLetter", parameters);
}
  
// model that passed to ==> context.ExecuteSqlMapper<DabirName>
public class DabirName 
{
  [Key]
  public long LetterId { set; get; }
  public long AndicatorNo { set; get; }
}

