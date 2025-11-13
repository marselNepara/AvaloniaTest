using System;
using System.Collections.Generic;

namespace AvaloniaTest.Data;

public partial class Supplier
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Inn { get; set; } = null!;

    public int? Rating { get; set; }

    public DateOnly? StartDate { get; set; }

    public int? SupplierTypeId { get; set; }

    public virtual SupplierType? SupplierType { get; set; }

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
