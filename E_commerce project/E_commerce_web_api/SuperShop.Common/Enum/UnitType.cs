using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Common.Enum
{
    public enum UnitType
    {
        // For products that can be counted individually, such as electronics or books, the unit is typically the number of items
        CountableProducts,

        // For products sold by weight, such as fruits, vegetables, or meat, the unit can be determined based on a standard measurement.
        // For example, the unit could be grams, kilograms, pounds, or ounces.
        WeightProducts,

        // For liquids or bulk products, the unit can be determined by volume measurements.
        // Common units include milliliters, liters, fluid ounces, or gallons.
        VolumeProducts,

        // For items sold by length, such as fabrics or cables, the unit can be measured in meters, centimeters, feet, or inches
        LengthProducts
    }
}
