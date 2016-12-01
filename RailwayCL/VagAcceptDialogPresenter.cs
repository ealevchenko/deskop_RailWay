using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public class VagAcceptDialogPresenter
    {
        IVagAcceptDialogView view;

        NeighbourStationsDB neighbourStationsDB = new NeighbourStationsDB();
        WayDB wayDB = new WayDB();

        public VagAcceptDialogPresenter(IVagAcceptDialogView view)
        {
            this.view = view;
        }

        public void loadInitValues()
        {
            loadWaysToCb();
            loadSidesToCb();
        }


        private void loadWaysToCb()
        {
            WayUtils wayUtils = WayUtils.GetInstance();
            view.loadWays(wayDB.getWays(view.getStation, false), wayUtils.CbDisplay, wayUtils.CbValue, "");
        }

        private void loadSidesToCb()
        {
            int selIdx = -1;
            if (view.getSentFrom.GetType().IsAssignableFrom(typeof(Station)))
                selIdx = (int)neighbourStationsDB.getArrivSide(view.getStation, (Station)view.getSentFrom);
            view.loadSides(SideUtils.GetInstance().CbItems, selIdx, SideUtils.GetInstance().CbNonSelected);
        }
    }
}
