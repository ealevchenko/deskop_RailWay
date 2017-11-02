using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using EFRailCars.Helpers;
using ServicesStatus;
using RWOperations.Helpers;

namespace RailwayCL
{
    public class VagSendOthStPresenter
    {
        IMainView main;
        IVagSendOthStView view;
        MainPresenter mainPresenter;

        GruzFrontDB gruzFrontDB = new GruzFrontDB();
        LocomotiveDB locomotiveDB = new LocomotiveDB();
        NeighbourStationsDB neighbourStationsDB = new NeighbourStationsDB();
        ShopDB shopDB = new ShopDB();
        StationDB stationDB = new StationDB();
        WayDB wayDB = new WayDB();
        VagSendOthStDB vagSendOthStDB = new VagSendOthStDB();

        private RWO_Desktop rwoperation = new RWO_Desktop();

        public VagSendOthStPresenter(IMainView main, IVagSendOthStView view)
        {
            this.main = main;
            this.view = view;
            this.mainPresenter = new MainPresenter(main);
        }

        public void loadVagSendOthStTab()
        {
            try
            {
                view.setStatLabel(main.selectedStation.Name);
                if (view.dgvForSendColumnsCount == 0) view.makeDgvForSendColumns();
                if (view.dgvToSendColumnsCount == 0) view.makeDgvToSendColumns();

                loadWays();

                view.loadSides(SideUtils.GetInstance().CbItems, SideUtils.GetInstance().CbNonSelected);

                List<GruzFront> gfList = gruzFrontDB.getGruzFronts(main.selectedStation);
                view.loadGF(gfList, GruzFrontUtils.GetInstance().CbDisplay, GruzFrontUtils.GetInstance().CbValue, GruzFrontUtils.GetInstance().CbNonSelected);
                List<Shop> shopList = shopDB.getShops(main.selectedStation);
                view.loadShops(shopList, ShopUtils.GetInstance().CbDisplay, ShopUtils.GetInstance().CbValue, ShopUtils.GetInstance().CbNonSelected);
                view.showHideGfAndShopsOnForm(gfList.Count > 0, shopList.Count > 0);

                view.loadStationsTo(stationDB.getStations(main.selectedStation), StationUtils.GetInstance().CbDisplay, StationUtils.GetInstance().CbValue, StationUtils.GetInstance().CbNonSelected);
                loadLocomotives();

                loadVagForSending();
                loadVagToSend();

                view.changeColumnsPositions(getFirstVagCondName() == "для отправки на УЗ");
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
                loadVagForSending();
                //view.clearVagToSendSelection();
                view.clearStationsTo(StationUtils.GetInstance().CbNonSelected);
                view.setTrainNum(0);
                view.clearSide(SideUtils.GetInstance().CbNonSelected);
                view.clearGF(GruzFrontUtils.GetInstance().CbNonSelected);
                view.clearShop(ShopUtils.GetInstance().CbNonSelected);
                view.clearLocoms(LocomotiveUtils.GetInstance().CbNonSelected);
                view.loadGF(gruzFrontDB.getGruzFronts(main.selectedStation), GruzFrontUtils.GetInstance().CbDisplay, GruzFrontUtils.GetInstance().CbValue, GruzFrontUtils.GetInstance().CbNonSelected);
                view.loadShops(shopDB.getShops(main.selectedStation), ShopUtils.GetInstance().CbDisplay, ShopUtils.GetInstance().CbValue, ShopUtils.GetInstance().CbNonSelected);
                loadVagToSend();

                view.changeColumnsPositions(getFirstVagCondName() == "для отправки на УЗ");
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

        public void onStatAcceptSelect()
        {
            try
            {
                if (main.selectedStation == view.selectedStatAccept)
                {
                    view.loadGF(gruzFrontDB.getGruzFronts(main.selectedStation), GruzFrontUtils.GetInstance().CbDisplay, GruzFrontUtils.GetInstance().CbValue, GruzFrontUtils.GetInstance().CbNonSelected);
                    view.loadShops(shopDB.getShops(main.selectedStation), ShopUtils.GetInstance().CbDisplay, ShopUtils.GetInstance().CbValue, ShopUtils.GetInstance().CbNonSelected);
                }
                else
                {
                    view.setSide(neighbourStationsDB.getSendSide(main.selectedStation, view.selectedStatAccept));

                    view.deleteGfItems();
                    view.clearGF(GruzFrontUtils.GetInstance().CbNonSelected);
                    view.deleteShopItems();
                    view.clearShop(ShopUtils.GetInstance().CbNonSelected);
                }

                addListToSend();
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void onSideSelect()
        {
            try
            {
                SendPlaceInRightOrder();
                addListToSend();
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void onGfSelect()
        {
            try
            {
                //view.setStatAcceptByName(main.selectedStation.Name);
                addListToSend();
                view.deleteShopItems(); // добавление невозможности выбрать еще и цех
                view.clearShop(ShopUtils.GetInstance().CbNonSelected);
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void onGfTextChanged()
        {
            try
            {
                view.clearGF(GruzFrontUtils.GetInstance().CbNonSelected);
                view.loadShops(shopDB.getShops(main.selectedStation), ShopUtils.GetInstance().CbDisplay, ShopUtils.GetInstance().CbValue, ShopUtils.GetInstance().CbNonSelected);
                view.clearStationsTo(StationUtils.GetInstance().CbNonSelected);
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void onShopSelect()
        {
            try
            {
                //view.setStatAcceptByName(main.selectedStation.Name);
                addListToSend();
                view.deleteGfItems(); // добавление невозможности выбрать еще и вагоноопрокид
                view.clearGF(GruzFrontUtils.GetInstance().CbNonSelected);
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void onShopTextChanged()
        {
            try
            {
                view.clearShop(ShopUtils.GetInstance().CbNonSelected);
                view.loadGF(gruzFrontDB.getGruzFronts(main.selectedStation), GruzFrontUtils.GetInstance().CbDisplay, GruzFrontUtils.GetInstance().CbValue, GruzFrontUtils.GetInstance().CbNonSelected);
                view.clearStationsTo(StationUtils.GetInstance().CbNonSelected);
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void addListToSend()
        {
            try
            {
                List<VagSendOthSt> list = view.listToSend;
                foreach (VagSendOthSt item in list)
                {
                    addToSend(item, list.IndexOf(item));
                }
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void onVagForSendingSelect()
        {
            try
            {
                if (view.selVagForSendingCount != 0)
                {
                    if (view.selVagForSendingCount > 1)
                    {
                        if (view.idxCurVagForSend < view.idxFirstSelVagForSend) // сверху вниз
                        {
                            for (int i = 0; i <= view.listForSending.Count - 1; i++)
                            {
                                addOrRemoveSelection(i);
                            }
                        }
                        else // снизу вверх
                        {
                            for (int i = view.listForSending.Count - 2; i >= 0; i--)
                            {
                                addOrRemoveSelection(i);
                            }
                        }
                    }
                    else //if (dgvForSending.SelectedRows.Count == 1)
                    {
                        if (!view.isVagForSendColored(view.idxFirstSelVagForSend))
                        {
                            view.addVagToSendFromVagsForSend(view.idxFirstSelVagForSend);
                            view.setVagForSendColor(view.idxFirstSelVagForSend, Color.Yellow);
                        }
                    }

                    // проверка на невозм. перепрыгнуть через вагон... и запись в базу
                    if (view.selectedSide != Side.Empty)
                        SendPlaceInRightOrder();

                    if (!main.selectAllBtnClicked) addListToSend();
                    //------
                }
                //}
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void selectAll(bool fromEnd)
        {
            try
            {
                ///dgvForSending.Focus();
                main.selectAllBtnClicked = true;
                view.clearVagForSendSelection();
                if (fromEnd)
                {
                    view.moveToLastVagForSend();
                    for (int i = view.listForSending.Count - 1; i >= 0; i--)
                    {
                        view.selectVagForSendByIdx(i);
                        view.moveToPrevVagForSend();
                    }
                }
                else
                {
                    view.moveToFirstVagForSend();
                    foreach (VagSendOthSt vag in view.listForSending)
                    {
                        view.selectVagForSendByIdx(view.listForSending.IndexOf(vag));
                        view.moveToNextVagForSend();
                    }
                }
                addListToSend();
                main.selectAllBtnClicked = false;
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }
        /// <summary>
        /// Отправим вагоны на другую станцию
        /// </summary>
        public void performSending()
        {
            try
            {
                if (view.selectedStatAccept == null && view.selectedGF == null && view.selectedShop == null)
                {
                    main.showErrorMessage(StationUtils.GetInstance().CbNonSelected + "!");
                    return;
                }
                if (view.selectedSide == Side.Empty && view.selectedGF == null && view.selectedShop == null)
                {
                    main.showErrorMessage(SideUtils.GetInstance().CbNonSelected + "!");
                    return;
                }
                if (view.listToSend.Count == 0)
                {
                    main.showErrorMessage("Выберите вагоны для отправки!");
                    return;
                }
                DateTime? dt_from_stat = DateTime.Now;

                if (view.selectedGF != null || view.selectedShop != null)
                {
                    if (!checkVagOrder()) return;
                }
                string mess_send = String.Format("Пользователь отправил состав со станции {0}, пути {1}, на станцию {2}", main.selectedStation.Name, view.selectedWay.NumName, view.selectedStatAccept != null ? view.selectedStatAccept.Name : view.selectedGF != null ? "Вагоноопрокид : " + view.selectedGF.Name : view.selectedShop!=null ? "Цех:"+view.selectedShop.Name :  "?" );
                string status = "";
                //TODO: RW-ОПЕРАЦИИ Включил логирование rw-операций отправки на другие станции
                rwoperation.DispatchCars(view.listToSend.Select(s => (int)s.num_vag).ToArray(), main.selectedStation.ID, view.selectedWay.ID, (view.selectedStatAccept != null ?  (int?)view.selectedStatAccept.ID : null), 
                    view.selectedGF != null ? (int?)view.listToSend[0].St_gruz_front : null,
                    view.selectedShop != null ? (int?)view.listToSend[0].St_shop : null,
                    new int[] {view.listToSend[0].St_lock_locom1, view.listToSend[0].St_lock_locom2});
                foreach (VagSendOthSt item in view.listToSend)
                {
                    

                    if (view.selectedGF != null || view.selectedShop != null)
                    {
                        bool isShop = false;
                        if (view.selectedShop != null) isShop = true;
                        dt_from_stat = item.dt_from_stat;
                        mainPresenter.changeLoadCond(item, isShop);

                    }
                    mainPresenter.changeConditionWayAfter(item, view.selectedWay);
                    vagSendOthStDB.send(item.id_oper, item.cond.Id, dt_from_stat, DateTime.Now);
                    status += String.Format("[состав:{0}, №:{1}, дата АМКР:{2}]; ", item.id_sostav, item.num_vag, item.dt_amkr);
                }
                mess_send.SaveLogEvents(status, service.DesktopRailCars);
                changeVagNumsWayFrom(); // изменить нумерацию вагонов на пути изъятия

                changeVagAmountOnWayAfterSending(); //  изменить кол-во вагонов на путях после отправки

                //CommitTransaction / Rollback
                view.clearVagToSend();
                view.clearStationsTo(StationUtils.GetInstance().CbNonSelected);
                view.setTrainNum(0);
                view.clearSide(SideUtils.GetInstance().CbNonSelected);
                view.loadGF(gruzFrontDB.getGruzFronts(main.selectedStation), GruzFrontUtils.GetInstance().CbDisplay, GruzFrontUtils.GetInstance().CbValue, GruzFrontUtils.GetInstance().CbNonSelected);
                view.loadShops(shopDB.getShops(main.selectedStation), ShopUtils.GetInstance().CbDisplay, ShopUtils.GetInstance().CbValue, ShopUtils.GetInstance().CbNonSelected);
                view.clearGF(GruzFrontUtils.GetInstance().CbNonSelected);
                view.clearShop(ShopUtils.GetInstance().CbNonSelected);
                view.clearLocoms(LocomotiveUtils.GetInstance().CbNonSelected);
                loadVagForSending();
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void loadLocomotives()
        {
            try
            {
                List<Locomotive> list;
                if (view.otherStatLocomotives) list = locomotiveDB.getLocomotives();
                else list = locomotiveDB.getLocomotives(main.selectedStation);
                view.loadLocomotives(list, LocomotiveUtils.GetInstance().CbDisplay, LocomotiveUtils.GetInstance().CbValue, LocomotiveUtils.GetInstance().CbNonSelected);
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void cancelAllToSend()
        {
            try
            {
                if (vagSendOthStDB.cancelToSend(view.selectedWay))
                {
                    view.clearColorAndDtFromWayMultipleVag();

                    view.clearVagToSend();
                    view.clearStationsTo(StationUtils.GetInstance().CbNonSelected);
                    view.setTrainNum(0);
                    view.clearSide(SideUtils.GetInstance().CbNonSelected);
                    view.clearGF(GruzFrontUtils.GetInstance().CbNonSelected);
                    view.clearShop(ShopUtils.GetInstance().CbNonSelected);
                    view.clearLocoms(LocomotiveUtils.GetInstance().CbNonSelected);
                    view.clearVagForSendSelection();
                }
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void onRemoveVagFromSend()
        {
            try
            {
                int srCount = view.selVagToSendCount;
                VagSendOthSt vagon = view.firstSelVagToSend;
                for (int i = 0; i <= srCount - 1; i++)
                {
                    if (vagSendOthStDB.cancelVagToSend(vagon.id_oper))
                    {
                        // убрать выделение цветом
                        view.setVagForSendColor(vagon.num_vag_on_way - 1, Color.Empty);
                        // убрать дату снятия с пути
                        view.listForSending[vagon.num_vag_on_way - 1].dt_from_way = null;
                        view.removeFromVagToSend(vagon);
                    }
                }
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void onVagToSendListChanged()
        {
            try
            {
                main.setFieldWithSelVagAmount(view.listToSend.Count.ToString());
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void searchVag()
        {
            if (main.numVagForSearch == 0) return;
            Tuple<int, int> tuple = vagSendOthStDB.findVagLocation(main.selectedStation, main.numVagForSearch);
            if (tuple.Item1 != 0 && tuple.Item2 != 0)
            {
                Way way = (from w in view.listWays where w.ID == tuple.Item1 select w).FirstOrDefault();
                int vagNumOnWay = tuple.Item2;

                view.setCurrentWay(view.listWays.IndexOf(way));
                onWaySelect();

                main.findVagByNumPerforming = true;
                view.selectVagForSendByIdx(vagNumOnWay - 1);
                main.findVagByNumPerforming = false;

                view.vagTableSetScrollToSelRow();
            }
            else main.showWarningMessage("Вагон не найден.");
        }

        /// <summary>
        /// Проверка следования вагонов
        /// </summary>
        /// <returns></returns>
        private bool checkVagOrder()
        {
            bool questResult = true;

            List<VagSendOthSt> listSendOthSt = new List<VagSendOthSt>(view.listToSend);
            listSendOthSt = listSendOthSt.OrderBy(x => x.num_vag_on_way).ToList();
            if (!listSendOthSt.SequenceEqual(view.listToSend))
            {
                listSendOthSt.OrderByDescending(x => x.num_vag_on_way).ToList();
                if (!listSendOthSt.SequenceEqual(view.listToSend))
                {
                    questResult = false;
                    questResult = main.showQuestMessage("Неверный порядок следования вагонов! Продолжить?");
                    return questResult;
                }
            }

            for (int i = 0; i < view.listToSend.Count; i++)
            {
                try
                {
                    if (Math.Abs(view.listToSend[i + 1].num_vag_on_way - view.listToSend[i].num_vag_on_way) > 1)
                    {
                        questResult = false;
                        questResult = main.showQuestMessage("Возможно Вы пропустили некоторые вагоны! Продолжить?");
                        break;
                    }
                }
                catch (ArgumentOutOfRangeException) { }
            }
            return questResult;
        }

        private void addOrRemoveSelection(int i)
        {
            //try
            //{
                if (view.isVagForSendSelected(i))
                {
                    if (!view.isVagForSendColored(i))
                    {
                        view.addVagToSendFromVagsForSend(i);
                        view.setVagForSendColor(i, Color.Yellow);
                    }
                }
                else
                {
                    if (view.listToSend[view.listToSend.Count-1] == view.listForSending[i])
                    {
                        VagSendOthSt vagon = view.listForSending[i];
                        view.removeFromVagToSend(vagon);
                        view.setVagForSendColor(i, Color.Empty);
                        vagon.dt_from_way = null;
                        vagSendOthStDB.cancelVagToSend(vagon.id_oper);
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    main.showErrorMessage(ex.Message);
            //}
        }

        private bool addToSend(VagSendOthSt vagSendOthSt, int order)
        {
            vagSendOthSt.St_lock_side = view.selectedSide;
            vagSendOthSt.St_lock_order = order;

            try
            {
                vagSendOthSt.St_gruz_front = view.selectedGF.ID;
            }
            catch (NullReferenceException)
            {
                vagSendOthSt.St_gruz_front = -1;
            }

            try
            {
                vagSendOthSt.St_shop = view.selectedShop.ID;
            }
            catch (NullReferenceException)
            {
                vagSendOthSt.St_shop = -1;
            }

            try
            {
                vagSendOthSt.St_lock_id_stat = view.selectedStatAccept.ID;
            }
            catch (NullReferenceException)
            {
                if (vagSendOthSt.St_gruz_front > -1 || vagSendOthSt.St_shop > -1)
                    vagSendOthSt.St_lock_id_stat = main.selectedStation.ID;
                else vagSendOthSt.St_lock_id_stat = -1;
            }

            if (vagSendOthSt.St_gruz_front == -1 && vagSendOthSt.St_shop == -1)
            {
                if (view.trainNum == 0)
                {
                    vagSendOthSt.St_lock_train = vagSendOthStDB.getMaxTrainNum() + 1;
                    view.setTrainNum(vagSendOthSt.St_lock_train);
                }
                else vagSendOthSt.St_lock_train = view.trainNum;
            }
            else
            {
                vagSendOthSt.St_lock_train = -1;
            }

            try
            {
                vagSendOthSt.St_lock_locom1 = view.locom1.ID;
            }
            catch (NullReferenceException)
            {
                vagSendOthSt.St_lock_locom1 = -1;
            }

            try
            {
                vagSendOthSt.St_lock_locom2 = view.locom2.ID;
            }
            catch (NullReferenceException)
            {
                vagSendOthSt.St_lock_locom2 = -1;
            }

            //try
            //{
                return vagSendOthStDB.addToSend(vagSendOthSt);
            //}
            //catch (Exception ex)
            //{
            //    main.showErrorMessage(ex.Message);
            //    return false;
            //}
        }

        private List<Way> getWays(Station station, bool rospusk)
        {
            List<Way> list = wayDB.getWays(station, rospusk);
            if (list.Count == 0) list.Add(new Way(-1, station, "0", "-без пути-"));
            return list;
        }

        private void loadWays()
        {
            List<Way> list = getWays(main.selectedStation, false);
            view.clearWays();
            view.bindWaysToSource(list);
            view.setCurrentWay(main.wayIdxToSelect);
        }

        private void loadVagForSending()
        {
            view.bindVagForSendToSource(new VagSendOthStDB().getVagons(view.selectedWay, view.selectedSide));
            view.clearVagForSendSelection();
        }

        private void loadVagToSend()
        {
            List<VagSendOthSt> listToSend = new List<VagSendOthSt>();
            List<int> rowIdx = new List<int>();

            for (int i = 0; i < view.listForSending.Count; i++)
            {
                if (((VagSendOthSt)view.listForSending[i]).St_lock_order != -1)
                {
                    listToSend.Add((VagSendOthSt)view.listForSending[i]);
                    rowIdx.Add(i);
                }
            }

            view.changeColorVagSelectedForSend(rowIdx);
            view.bindVagToSendToSource(listToSend.OrderBy(x => x.St_lock_order).ToList());

            if (listToSend.Count > 0)
            {
                view.setTrainNum(listToSend[0].St_lock_train);
                view.setStationTo(listToSend[0].St_lock_id_stat);
                view.setSide(neighbourStationsDB.getSendSide(main.selectedStation, view.selectedStatAccept));
                view.setGF(listToSend[0].St_gruz_front);
                view.setShop(listToSend[0].St_shop);
                setLocoms(listToSend[0].St_lock_locom1, listToSend[0].St_lock_locom2);
            }
        }

        private void setLocoms(int locom1, int locom2)
        {
            int? val1 = view.setLocom1(locom1);
            if (locom1 != -1 && val1 == null)
            {
                view.setOtherStatLocomotives(true);
                view.setLocom1(locom1);
            }

            int? val2 = view.setLocom2(locom2);
            if (locom2 != -1 && val2 == null)
            {
                view.setOtherStatLocomotives(true);
                view.setLocom2(locom2);
            }
        }

        private void SendPlaceInRightOrder()
        {
            List<VagSendOthSt> listSendOthSt = view.listToSend;
            if (main.numSide == view.selectedSide)
                view.bindVagToSendToSource(listSendOthSt.OrderBy(x => x.num_vag_on_way).ToList());
            else view.bindVagToSendToSource(listSendOthSt.OrderByDescending(x => x.num_vag_on_way).ToList());
        }

        private void changeVagNumsWayFrom()
        {
            List<VagSendOthSt> remainedVagons = view.getRemainedVagForSending();
            foreach (VagSendOthSt item in remainedVagons)
            {
                vagSendOthStDB.changeVagNumsWayFrom(remainedVagons.IndexOf(item) + 1, item.id_oper);
            }
        }

        private void changeVagAmountOnWayAfterSending()
        {
            view.selectedWay.Vag_amount = view.selectedWay.Vag_amount - view.listToSend.Count;
            view.refreshWayTable();
        }

        private string getFirstVagCondName()
        {
            string condName = "";
            try
            {
                condName = view.listForSending[0].cond.Name;
            }
            catch (ArgumentOutOfRangeException) { }
            return condName;
        }
    }
}
