using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites;

public class DataPoint
{
    public DataPoint()
    {
    }


    // Constructor to initialize the DataPoint
    public DataPoint(string name, int value, DateTime time)
    {
        Name = name;
        Value = value;
        Time = time;
    }


    // Name: String identifier for the data point
    public string Name { get; set; }

    // Value: Random or simulated value
    public int Value { get; set; }

    // Time: Timestamp when the data point was generated
    public DateTime Time { get; set; }



    // Optional: Override ToString() for easy display of the DataPoint
    public override string ToString()
    {
        return $"Name: {Name}, Value: {Value}, Time: {Time}";
    }


}
