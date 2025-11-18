using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSteam.Application.Dtos.Licence
{
    public class LicenceRequestDto
    {
        public int IdGamerUser { get; set; }
        public int IdGame { get; set; }
    }
}
