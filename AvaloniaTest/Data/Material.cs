using System;
using System.Collections.Generic;

namespace AvaloniaTest.Data;

public partial class Material
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? MaterialTypeId { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal QuantityInStock { get; set; }

    public decimal MinimumQuantity { get; set; }

    public int QuantityInPackage { get; set; }

    public string UnitOfMeasure { get; set; } = null!;

    public virtual MaterialType? MaterialType { get; set; }

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
