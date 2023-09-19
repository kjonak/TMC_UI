using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMC_API
{
    internal class TMC_Parser
    {
        private TMC_Model _tmc;
        public TMC_Parser(TMC_Model tmc_api) 
        {
            _tmc = tmc_api;
        }
        public void HandleNewBytes(byte[] bytes)
        {

        }




    }
}
