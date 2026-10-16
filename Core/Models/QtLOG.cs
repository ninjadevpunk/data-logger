using Core.Models.App_Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    /// <summary>
    /// Class for Qt LOGs.
    /// </summary>
    [Table("CodingLOG")]
    public class QtLOG : CodingLOG
    {
        public QtLOG()
        {
        }

        public QtLOG(ApplicationClass qtApp)
        {
            Application = qtApp;
        }
    }
}
