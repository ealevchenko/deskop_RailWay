using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public interface IVagAcceptDialogView
    {
        Station getStation { get; }
        SendingPoint getSentFrom { get; }

        void loadWays(List<Way> list, string cbDisplay, string cbValue, string nonSelected);
        void loadSides(object[] items, int selIdx, string nonSelected);
    }
}
