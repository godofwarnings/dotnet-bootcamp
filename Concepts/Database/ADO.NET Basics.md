---
tags: [csharp, database, sql, adonet]
aliases: [ADO.NET, SqlDataReader, SqlCommand, SqlConnection]
---
# ADO.NET Basics

ADO.NET is the data access technology in the .NET Framework used to communicate with databases.

## Key Components

1. **`SqlConnection`**: Represents a connection to a SQL Server database.
2. **`SqlCommand`**: Represents a Transact-SQL statement or stored procedure to execute against a SQL Server database.
3. **`SqlDataReader`**: Provides a way of reading a forward-only stream of rows from a SQL Server database.

## Reading Data Example

See [ADO.NET Demo.cs](../../Code%20Examples/Day%202%20-%20ADO.NET/ADO.NET%20Demo.cs) for full CRUD examples. Here is a breakdown of how to read data:

```csharp
// 1. Define the query
string query = "SELECT Id, Name, Category, Price, Stock FROM Products";

// 2. Create the command
using SqlCommand command = new SqlCommand(query, connection);

// 3. Execute the query and get a reader
using SqlDataReader reader = command.ExecuteReader();

// 4. Read each row
while (reader.Read())
{
    // 5. Extract values
    int id = Convert.ToInt32(reader["Id"]);
    string name = reader["Name"].ToString();
    string category = reader["Category"].ToString()!;
    
    Console.WriteLine($"{id} - {name}");
}
```

### Syntax Breakdown

- **`using` statement:** Classes like `SqlCommand`, `SqlConnection`, and `SqlDataReader` use unmanaged resources (network connections, file handles). The `using` statement ensures that these objects are properly disposed of and resources are released as soon as the block ends.
- **`ExecuteReader()`:** Used when the query returns multiple rows (like a `SELECT` statement).
- **`ExecuteScalar()`:** Used when the query returns a single value (like `SELECT COUNT(*)`).
- **`ExecuteNonQuery()`:** Used for DML statements (`INSERT`, `UPDATE`, `DELETE`) where you just want to know how many rows were affected.
- **`reader.Read()`:** Moves the reader to the next row. Returns `true` if a row exists, `false` otherwise.

### The Null-Forgiving Operator (`!`)
In the code `reader["Category"].ToString()!`, the `!` at the end is the **null-forgiving operator**. 

It tells the compiler: "I know this value should not be null, do not warn me about it." It does nothing at runtime; it only suppresses compiler warnings.

---
## See Also
- [ADO.NET Demo.cs](../../Code%20Examples/Day%202%20-%20ADO.NET/ADO.NET%20Demo.cs)
