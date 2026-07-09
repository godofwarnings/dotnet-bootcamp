### `Models/Product.cs`

```csharp
namespace ProductManagementSystem.Models;

public sealed class Product
{
    public int ID { get; set; }
    public string ProdName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Brand { get; set; } = string.Empty;
}
```

---

### `DTOs/CreateProductRequestDto.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace ProductManagementSystem.DTOs;

public sealed class CreateProductRequestDto
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string ProdName { get; set; } = string.Empty;

    [Range(0.01, 100000)]
    public decimal Price { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(50)]
    public string Brand { get; set; } = string.Empty;
}
```

---

### `DTOs/UpdateProductRequestDto.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace ProductManagementSystem.DTOs;

public sealed class UpdateProductRequestDto
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string ProdName { get; set; } = string.Empty;

    [Range(0.01, 100000)]
    public decimal Price { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(50)]
    public string Brand { get; set; } = string.Empty;
}
```

---

### `DTOs/ProductResponseDto.cs`

```csharp
namespace ProductManagementSystem.DTOs;

public sealed class ProductResponseDto
{
    public int ID { get; set; }
    public string ProdName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Brand { get; set; } = string.Empty;
}
```
