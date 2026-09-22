using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Logger_1._3.Models.App_Models
{
    /// <summary>
    /// Class for specifying a measuring unit.
    /// </summary>
    [Table("UNIT")]
    public class MeasuringUnitClass
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int unitID { get; set; }

        public string Unit { get; set; } = "cm";


        public MeasuringUnitClass()
        {
            
        }

        public MeasuringUnitClass(string unit)
        {
            Unit = unit;
        }

        public MeasuringUnitClass(int unitID, string unit)
        {
            this.unitID = unitID;
            Unit = unit;
        }

        public override bool Equals(object? obj)
        {
            return obj is MeasuringUnitClass @class &&
                   unitID == @class.unitID &&
                   Unit == @class.Unit;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(unitID, Unit);
        }

        public static bool operator ==(MeasuringUnitClass left, MeasuringUnitClass right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MeasuringUnitClass left, MeasuringUnitClass right)
        {
            return !left.Equals(right);
        }
    }
}
