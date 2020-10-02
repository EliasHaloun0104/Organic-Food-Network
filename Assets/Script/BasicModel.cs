namespace Test_for_DB.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BasicModel    {
        

        

    }
    [Serializable]
    public class Person
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public float XCoordinate { get; set; }
        public float YCoordinate { get; set; }


    }
    [Serializable]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Quantity { get; set; }
        public string Unit { get; set; }
        

    }
}