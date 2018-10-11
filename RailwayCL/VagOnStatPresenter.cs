using EFRailCars.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public class VagOnStatPresenter
    {
        IVagOnStatView view;
        IMainView main;

        VagOnStatDB vagOnStatDB = new VagOnStatDB();
        GruzFrontDB gruzFrontDB = new GruzFrontDB();
        ShopDB shopDB = new ShopDB();
        WayDB wayDB = new WayDB();

        public VagOnStatPresenter(IMainView main, IVagOnStatView view)
        {
            this.view = view;
            this.main = main;
        }

        public void loadVagOnStatTab()
        {
            try
            {
                if (view.dgvColumnsCount == 0) view.makeDgvColumns();
                loadWays();
                loadVagOnStat(view.selectedWay, main.numSide);

                view.changeColumnsPositions(getFirstVagCondName() == "для отправки на УЗ");

                showGrFrAndShops();
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void onWaySelect()
        {
            try
            {
                loadVagOnStat(view.selectedWay, main.numSide);
                view.changeColumnsPositions(getFirstVagCondName() == "для отправки на УЗ");
                view.clearGfSelection();
                view.clearShopsSelection();
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }

            try
            {
                main.wayIdxToSelect = view.listWays.IndexOf(view.selectedWay);
            }
            catch (ArgumentOutOfRangeException) { }
        }
        // Выбран вагоноопрокид
        public void onGfSelect()
        {
            try
            {
                view.bindVagOnStatToSource(vagOnStatDB.getVagons(view.selectedGf));
                view.clearWaysSelection();
                view.clearShopsSelection();
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }
        // Выбран цех
        public void onShopSelect()
        {
            try
            {
                view.bindVagOnStatToSource(vagOnStatDB.getVagons(view.selectedShop));
                view.clearWaysSelection();
                view.clearGfSelection();
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void searchVag()
        {
            if (main.numVagForSearch == 0) return;
            Tuple<int, int> tuple = vagOnStatDB.findVagLocation(main.selectedStation, main.numVagForSearch);
            if (tuple.Item1 != 0 && tuple.Item2 != 0)
            {
                Way way = (from w in view.listWays where w.ID == tuple.Item1 select w).FirstOrDefault();
                int vagNumOnWay = tuple.Item2;

                view.setCurrentWay(view.listWays.IndexOf(way));
                onWaySelect();

                main.findVagByNumPerforming = true;
                view.selectVagByIdx(vagNumOnWay - 1);
                main.findVagByNumPerforming = false;

                view.vagTableSetScrollToSelRow();
            }
            else main.showWarningMessage("Вагон не найден.");
        }

        private List<Way> getWays(Station station, bool rospusk)
        {
            List<Way> list = wayDB.getWays(station, rospusk);
            if (list.Count == 0) list.Add(new Way(-1, station, "0", "-без пути-"));
            return list;
        }

        private void loadWays()
        {
            view.clearWays();
            view.bindWaysToSource(getWays(main.selectedStation, false));
            view.setCurrentWay(main.wayIdxToSelect);
        }
        // Загрузить вагоны
        private void loadVagOnStat(Way way, Side numSide)
        {
            view.bindVagOnStatToSource(vagOnStatDB.getVagons(way, numSide));
            main.setFieldWithSelVagAmount("");
        }

        private void showGrFrAndShops()
        {
            bool hasShops = false;
            bool hasGF = false;

            //try
            //{

                view.bindShopsToSource(shopDB.getShops(main.selectedStation));
                view.clearShopsSelection();
                if (view.shopsCount > 0) hasShops = true;

                view.bindGfToSource(gruzFrontDB.getGruzFronts(main.selectedStation));
                view.clearGfSelection();
                if (view.gfCount > 0) hasGF = true;

                if (hasGF || hasShops)
                {
                    view.showGfAndShopsOnForm(hasGF, hasShops);
                }
                else
                {
                    view.hideGfAndShopsOnForm();
                }
            //}
            //catch (Exception ex)
            //{
            //    main.showErrorMessage(ex.Message);
            //}
        }

        private string getFirstVagCondName()
        {
            string condName = "";
            try
            {
                condName = view.listVagons[0].cond.Name;
            }
            catch (ArgumentOutOfRangeException) { }
            return condName;
        }
    }
}
