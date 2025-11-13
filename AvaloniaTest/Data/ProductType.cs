using System;
using System.Collections.Generic;

namespace AvaloniaTest.Data;

public partial class ProductType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Coefficient { get; set; }
}
