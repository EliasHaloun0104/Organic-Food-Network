using System.Collections;
using System.Collections.Generic;


public partial class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float Quantity { get; set; }
    public string Unit { get; set; }
    public System.DateTime DateCreated { get; set; }
    public bool IsHidden { get; set; }
    public int PersonID { get; set; }
}

