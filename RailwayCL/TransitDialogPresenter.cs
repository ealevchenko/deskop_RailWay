using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public class TransitDialogPresenter
    {
        ITransitDialogView view;

        StationDB stationDB = new StationDB();
        WayDB wayDB = new WayDB();

        public TransitDialogPresenter(ITransitDialogView view)
        {
            this.view = view;
        }

        public void loadInitValues()
        {
            loadWaysToCb();
            loadStationsToCb();
        }


        private void loadWaysToCb()
        {
            WayUtils wayUtils = WayUtils.GetInstance();
            view.loadWays(wayDB.getWays(view.getStation, false), wayUtils.CbDisplay, wayUtils.CbValue, "");
        }

        private void loadStationsToCb()
        {
            StationUtils stationUtils = StationUtils.GetInstance();
            view.loadStations(stationDB.getStations(view.getStation), stationUtils.CbDisplay, stationUtils.CbValue, stationUtils.CbNonSelected);
        }
    }
}
