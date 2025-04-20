using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Extensions
{
    public static class IntExtensions
    {
        public static bool HasRequiredInputs(this int inputs, int req_inputs) =>
            inputs == req_inputs;        
    }
}
