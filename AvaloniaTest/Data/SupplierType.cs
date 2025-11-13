using System;
using System.Collections.Generic;

namespace AvaloniaTest.Data;

public partial class SupplierType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
