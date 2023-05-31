using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ai
{
    public interface IAlgorithm
    {
        public int? ReturnNextMove(Node node);
    }
}
