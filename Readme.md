## SNIKT! ##
Snkit! is nothing but a light-weight micro-ORM. Originally, it is the switchblade-sound of Wolverine's claws locking into place.

## OVERVIEW ##
I wrote Snikt! to take away the pain of data access logic.
* It is a simple Assembly you can refer in your Data Access Layer that will provide utilities, wrappers, and extensions to simplify work in the data access objects.
* Snikt! provides database connection lifetime management and execution store commands.
* It materialize CLR-types given a data reader as well as result set of a store command.
* It has templates and abstract classes for business entities, data access objects, and search query arguments.
* Snikt! has no database specific implementations. Technically, you should be able to use it on MySQL, SQL Server, or even on Oracle.

## LICENSE ##
Snikt! code is free for commercial and non-commercial deployments and distribution. Snikt! is release under [MIT License](http://www.opensource.org/licenses/mit-license.php).

## FEATURES ##
Snikt! provides 3 simple helpers:
* [Materializer.cs](https://github.com/kosalanuwan/snikt/blob/master/Snikt/Materializer.cs)
* [Database.cs](https://github.com/kosalanuwan/snikt/blob/master/Snikt/Database.cs)
* [DbConnectionFactory.cs](https://github.com/kosalanuwan/snikt/blob/master/Snikt/DbConnectionFactory.cs)

### Map query results to a strong-typed list ###
```csharp
IDataReader queryResult = command.ExecuteReader();
Materializer<Category> categoryMaterializer = new Materializer<Category>(queryResult);
List<Category> categories = new List<Category>();
while (queryResult.Read())
{
    categories.Add(categoryMaterializer.Materialize(queryResult));
}

Assert.IsTrue(categories.Any());
```

### Execute a Stored Procedure ###
```csharp
IDatabase db = new Database("name=DefaultConnection");

List<Category> categories = db.SqlQuery<Category>("dbo.GetAllCategories").ToList();

Assert.IsTrue(categories.Any());
```

### Execute a Stored Procedure that obtain Parameters ###
```csharp
IDatabase db = new Database("name=DefaultConnection");
var criteria = new { Id = 1 };

List<Category> categories = db.SqlQuery<Category>("dbo.GetCategory", criteria).ToList();

foreach (Category cat in categories)
{
    Assert.AreEqual(criteria.Id, cat.Id);
}
```

### Provide DbContext as the Connection ###
```csharp
public class DbContextConnectionFactory : IDbConnectionFactory
{
    public IDbConnection CreateIfNotExists(string nameOrConnectionString)
    {
		MiniNWDbContext dbContext = new MiniNWDbContext("name=DefaultConnection");
        return dbContext.Database.Connection;
    }
}

IDatabase db = new Database("name=DefaultConnection", new DbContextConnectionFactory());
List<Category> categories = db.SqlQuery<Category>("dbo.GetAllCategories").ToList();
Assert.IsTrue(categories.Any());
```

## OTHER RESOURCES ##
Snikt! is created to demonstrate How to apply some of the available .NET technologies with the Data Access Layer in Layered Architecture design pattern. The main focus of the specifications is How to code the Data Access Layer? and not the actual functionality of the chosen MiniNW application.

/KP

e: kosala.nuwan@gmail.com
t: [@kosalanuwan](https://www.twitter.com/kosalanuwan)
b: http://kosalanuwan.tumblr.com
