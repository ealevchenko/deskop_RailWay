using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    /// <summary>
    /// Интерфейс для выполнения "Транзит"
    /// </summary>
    public interface ITransitDialogView
    {
        Station getStation { get; }

        void loadWays(List<Way> list, string cbDisplay, string cbValue, string nonSelected);
        void loadStations(List<Station> list, string cbDisplay, string cbValue, string nonSelected);
    }
}
