using System;
using System.Collections.Generic;

namespace AvaloniaTest.Data;

public partial class MaterialType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal LossPercentage { get; set; }

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
