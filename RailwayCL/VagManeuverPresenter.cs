using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using log4net;
using log4net.Config;

namespace RailwayCL
{
    public class VagManeuverPresenter
    {
        IVagManeuverView view;
        IMainView main;
        MainPresenter mainPresenter;

        LocomotiveDB locomotiveDB = new LocomotiveDB();
        WayDB wayDB = new WayDB();
        VagManeuverDB vagManeuverDB = new VagManeuverDB();

        private static readonly ILog log = LogManager.GetLogger(typeof(VagManeuverPresenter));

        public VagManeuverPresenter(IMainView main, IVagManeuverView view)
        {
            this.view = view;
            this.main = main;
            this.mainPresenter = new MainPresenter(main);
        }

        public void loadVagManeuverTab()
        {
            try
            {
                if (view.dgvForManColumnsCount == 0) view.makeDgvForManColumns();
                if (view.dgvOnManColumnsCount == 0) view.makeDgvOnManColumns();

                loadWays();

                view.loadSides(SideUtils.GetInstance().CbItems, SideUtils.GetInstance().CbNonSelected);
                loadLocomotives();

                view.changeLabelsSize();

                loadVagForMan();
                loadVagOnMan(); // загрузить вагоны на маневре (снятые с пути) 

                view.changeColumnsPositions(getFirstVagCondName() == "для отправки на УЗ");
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void loadLocomotives()
        {
            List<Locomotive> list;
            try
            {
                if (view.otherStatLocomotives) list = locomotiveDB.getLocomotives();
                else list = locomotiveDB.getLocomotives(main.selectedStation);
                view.loadLocomotives(list, LocomotiveUtils.GetInstance().CbDisplay, LocomotiveUtils.GetInstance().CbValue, LocomotiveUtils.GetInstance().CbNonSelected);
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void onWayFromSelect()
        {
            try
            {
                loadVagForMan();
                view.clearWaysOnSelection();
                view.clearSide(SideUtils.GetInstance().CbNonSelected);
                view.setOtherStatLocomotives(false);
                view.clearLocom(LocomotiveUtils.GetInstance().CbNonSelected);
                loadVagOnMan();

                view.changeColumnsPositions(getFirstVagCondName() == "для отправки на УЗ");
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }

            try
            {
                main.wayIdxToSelect = view.listWayFrom.IndexOf(view.selectedWayFrom);
            }
            catch (ArgumentOutOfRangeException) { }
        }

        public void onVagForManSelect()
        {
            try
            {
                if (view.selVagForManCount != 0)
                {
                    if (view.selVagForManCount > 1)
                    {
                        if (view.idxCurVagForMan < view.idxFirstSelVagForMan) // сверху вниз
                        {
                            for (int i = 0; i <= view.listVagForMan.Count - 1; i++)
                            {
                                addOrRemoveSelection(i);
                            }
                        }
                        else // снизу вверх
                        {
                            for (int i = view.listVagForMan.Count - 2; i >= 0; i--)
                            {
                                addOrRemoveSelection(i);
                            }
                        }
                    }
                    else //if (dgvForMan.SelectedRows.Count == 1)
                    {
                        if (!view.isVagForManColored(view.idxFirstSelVagForMan))
                        {
                            view.addVagOnManFromVagsForMan(view.idxFirstSelVagForMan);
                            view.setVagForManColor(view.idxFirstSelVagForMan, Color.Yellow);
                        }
                    }

                    // проверка на невозм. перепрыгнуть через вагон... и запись в базу
                    if (view.selectedSide != Side.Empty)
                        ManPlaceInRightOrder();

                    if (!main.selectAllBtnClicked) addListOnManeuver();
                    //------
                }
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void cancelManeuver()
        {
            try
            {
                if (vagManeuverDB.cancelManeuver(view.selectedWayFrom))
                {
                    view.clearColorAndDtFromWayMultipleVag();

                    view.clearVagOnMan();
                    view.clearSide(SideUtils.GetInstance().CbNonSelected);
                    view.clearLocom(LocomotiveUtils.GetInstance().CbNonSelected);
                    view.clearWaysOnSelection();
                    view.clearVagForManSelection();
                }
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void onRemoveVagFromMan()
        {
            try
            {
                int srCount = view.selVagOnManCount;
                VagManeuver vagon = view.firstSelVagOnMan;
                for (int i = 0; i <= srCount - 1; i++)
                {
                    if (vagManeuverDB.cancelVagOnMan(vagon.id_oper))
                    {
                        // убрать выделение цветом
                        view.setVagForManColor(vagon.num_vag_on_way - 1, Color.Empty);
                        // убрать дату снятия с пути
                        view.listVagForMan[vagon.num_vag_on_way - 1].dt_from_way = null;
                        view.removeFromVagOnMan(vagon);
                    }
                }
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void ManSelectAll(bool fromEnd)
        {
            try
            {
                main.selectAllBtnClicked = true;
                view.clearVagForManSelection();
                if (fromEnd)
                {
                    view.moveToLastVagForMan();
                    for (int i = view.listVagForMan.Count - 1; i >= 0; i--)
                    {
                        view.selectVagForManByIdx(i);
                        view.moveToPrevVagForMan();
                    }
                }
                else
                {
                    view.moveToFirstVagForMan();
                    foreach (VagManeuver vag in view.listVagForMan)
                    {
                        view.selectVagForManByIdx(view.listVagForMan.IndexOf(vag));
                        view.moveToNextVagForMan();
                    }
                }
                addListOnManeuver();
                main.selectAllBtnClicked = false;
                //Log(DateTime.Now.ToString() + " selectAll " + fromEnd.ToString() + "\r\n");
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void ManPlaceInRightOrder()
        {
            try
            {
                List<VagManeuver> listOnMan = view.listVagOnMan;
                if (main.numSide == view.selectedSide)
                    view.bindVagOnManToSource(listOnMan.OrderBy(x => x.num_vag_on_way).ToList());
                else view.bindVagOnManToSource(listOnMan.OrderByDescending(x => x.num_vag_on_way).ToList());
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void addListOnManeuver()
        {
            try
            {
                foreach (VagManeuver item in view.listVagOnMan)
                {
                    addOnManuever(item, view.listVagOnMan.IndexOf(item));
                }
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void performManeuver()
        {
    //        Log("\r\n" + DateTime.Now.ToString() + " Before maneuver. bsWayFrom: " + ((Way)ManBsWayFrom.List[dgvWayFrom.SelectedRows[0].Index]).Vag_amount.ToString() +
    //", DGVwayFrom: " + dgvWayFrom.SelectedRows[0].Cells[2].Value.ToString() + "\r\n");
    //        Log(DateTime.Now.ToString() + " Before maneuver. bsWayTo: " + ((Way)ManBsWayTo.List[dgvWayTo.SelectedRows[0].Index]).Vag_amount.ToString() +
    //            ", DGVwayTo: " + dgvWayTo.SelectedRows[0].Cells[2].Value.ToString() + "\r\n");
    //        Log(DateTime.Now.ToString() + " Before maneuver. bs2.Count: " + ManBs2.Count.ToString() + "\r\n");
    //        Log(DateTime.Now.ToString() + " Before maneuver. " + cbLocom.Text + "\r\n");
    //        Log(DateTime.Now.ToString() + " Before maneuver. " + ManCbSide.Text + "\r\n");
    //        Log(DateTime.Now.ToString() + " Number of the first Vag on Maneuver: " + dgvOnMan.Rows[0].Cells[1].Value.ToString() + "\r\n");
            try
            {
                // проверка на выбор горловины
                if (view.selectedSide == Side.Empty)
                {
                    main.showErrorMessage(SideUtils.GetInstance().CbNonSelected + "!");
                    log.Error(SideUtils.GetInstance().CbNonSelected + "!");
                    return;
                }
                // проверка на выбор пути
                if (view.selectedWayTo == null)
                {
                    main.showErrorMessage("Выберите путь для постановки вагонов!");
                    log.Error("Выберите путь для постановки вагонов!");
                    return;
                }
                // проверка на превышение вместимости пути
                if (view.selectedWayFrom.Num != view.selectedWayTo.Num &&
                    view.selectedWayTo.Vag_amount + view.listVagOnMan.Count > view.selectedWayTo.Capacity)
                {
                    if (!main.showQuestMessage("Количество вагонов превышает вместимость пути! Продолжить?"))
                    {
                        log.Info("Количество вагонов превышает вместимость пути");
                        return;
                    }
                }
                int locomNum = 0;
                if (view.selectedLocom != null) locomNum = view.selectedLocom.Num;
                log.Info("Начало маневра с пути  " + view.selectedWayFrom.NumName + ", кол-во вагонов: " + view.selectedWayFrom.Vag_amount.ToString() +
                    " на путь " + view.selectedWayTo.NumName + ", кол-во вагонов: " + view.selectedWayTo.Vag_amount + ". Вагонов на маневре: " + view.listVagOnMan.Count +
                    ". Горловина маневра: " + view.selectedSide.ToString() + ". Локомотив: " + locomNum);
                foreach (VagManeuver item in view.listVagForMan)
                {
                    log.Info("Вагон " + item.num_vag + " до маневра имеет № на пути: " + item.num_vag_on_way);
                }
                //BeginTransaction
                foreach (VagManeuver item in view.listVagOnMan)
                {
                    log.Info("Вагон на маневре №" + item.num_vag + " до маневра имеет № на пути: " + item.num_vag_on_way);
                    mainPresenter.changeConditionWayOn(item, view.selectedWayTo);
                    mainPresenter.changeConditionWayAfter(item, view.selectedWayFrom);

                    if (main.numSide == view.selectedSide)
                        item.num_vag_on_way = view.listVagOnMan.IndexOf(item) + 1;
                    else item.num_vag_on_way = view.selectedWayTo.Vag_amount + view.listVagOnMan.Count - view.listVagOnMan.IndexOf(item);
                    int ins_result = vagManeuverDB.execManeuver(item, view.selectedWayTo);
                    if (ins_result != -1)
                    {
                        item.id_oper = ins_result;
                        log.Info("По вагону " + item.num_vag + " маневр выполнен.");
                    }
                }
                //Log(DateTime.Now.ToString() + " Maneuver added to DB" + "\r\n");

                if (main.numSide == view.selectedSide)
                {
                    // изменить нумерацию вагонов на пути назначения
                    vagManeuverDB.changeVagNumsWayOn(view.listVagOnMan.Count, view.listVagOnMan[0].id_oper, view.selectedWayTo);
                    log.Info("Изменение нумерации вагонов на пути назначения. First vagon id_oper=" + view.listVagOnMan[0].id_oper);
                }

                ManChangeVagNumsWayFrom(); // изменить нумерацию вагонов на пути изъятия

                changeVagAmountOnWaysAfterManeuver(); //  изменить кол-во вагонов на путях после маневра

                //CommitTransaction / Rollback

                view.clearVagOnMan();
                view.clearWaysOnSelection();
                view.clearSide(SideUtils.GetInstance().CbNonSelected);
                view.clearLocom(LocomotiveUtils.GetInstance().CbNonSelected);
                loadVagForMan();
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void onVagOnManListChanged()
        {
            try
            {
                main.setFieldWithSelVagAmount(view.listVagOnMan.Count.ToString());
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void searchVag()
        {
            if (main.numVagForSearch == 0) return;
            Tuple<int, int> tuple = vagManeuverDB.findVagLocation(main.selectedStation, main.numVagForSearch);
            if (tuple.Item1 != 0 && tuple.Item2 != 0)
            {
                Way way = (from w in view.listWayFrom where w.ID == tuple.Item1 select w).FirstOrDefault();
                int vagNumOnWay = tuple.Item2;

                view.setCurrentWayFrom(view.listWayFrom.IndexOf(way));
                onWayFromSelect();

                main.findVagByNumPerforming = true;
                view.selectVagForManByIdx(vagNumOnWay - 1);
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
            List<Way> list = getWays(main.selectedStation, false);
            view.clearWaysFrom();
            view.bindWaysFromToSource(list);
            view.setCurrentWayFrom(main.wayIdxToSelect);

            view.clearWaysOn();
            view.bindWaysOnToSource(list);
        }

        private void loadVagForMan()
        {
            view.bindVagForManToSource(vagManeuverDB.getVagons(view.selectedWayFrom, view.selectedSide));
            view.clearVagForManSelection();
        }

        private void loadVagOnMan() 
        {
            List<VagManeuver> listOnMan = new List<VagManeuver>();
            List<int> rowsIdx = new List<int>();

            for (int i = 0; i < view.listVagForMan.Count; i++)
            {
                if (((VagManeuver)view.listVagForMan[i]).Lock_order != -1)
                {
                    listOnMan.Add((VagManeuver)view.listVagForMan[i]);
                    rowsIdx.Add(i);
                }
            }
            view.changeColorVagSelectedForMan(rowsIdx);
            view.bindVagOnManToSource(listOnMan.OrderBy(x => x.Lock_order).ToList());

            if (listOnMan.Count > 0)
            {
                ManSelectWayTo(listOnMan[0].Lock_id_way);
                view.selectSide(listOnMan[0].Lock_side);
                ManSelectLocom(listOnMan[0].Lock_id_locom);
            }
        }

        private void ManSelectWayTo(int id_way)
        {
            for (int i = 0; i < view.listWayTo.Count; i++)
            {
                if (((Way)view.listWayTo[i]).ID == id_way)
                {
                    view.setCurrentWayTo(i);
                    break;
                }
            }
        }

        private void ManSelectLocom(int id_locom)
        {
            int? selValue = view.selectLocom(id_locom);
            if (id_locom != -1 && selValue == null)
            {
                view.setOtherStatLocomotives(true);
                view.selectLocom(id_locom);
            }
        }

        private void addOrRemoveSelection(int i)
        {
            //try
            //{
                if (view.isVagForManSelected(i))
                {
                    if (!view.isVagForManColored(i))
                    {
                        view.addVagOnManFromVagsForMan(i);
                        view.setVagForManColor(i, Color.Yellow);
                    }
                }
                else
                {
                    if (view.listVagOnMan[view.listVagOnMan.Count-1] == view.listVagForMan[i])
                    {
                        VagManeuver vagon = view.listVagForMan[i];
                        view.removeFromVagOnMan(vagon);
                        view.setVagForManColor(i, Color.Empty);
                        vagon.dt_from_way = null;
                        vagManeuverDB.cancelVagOnMan(vagon.id_oper);
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    main.showErrorMessage(ex.Message);
            //}
        }

        private bool addOnManuever(VagManeuver vagManeuver, int order)
        {
            try
            {
                vagManeuver.Lock_id_way =  view.selectedWayTo.ID;
            }
            catch (NullReferenceException)
            {
                vagManeuver.Lock_id_way = -1;
            }

            vagManeuver.Lock_side = view.selectedSide;

            try
            {
                vagManeuver.Lock_id_locom = Int32.Parse(view.selectedLocom.ID.ToString());
            }
            catch (NullReferenceException)
            {
                /*if (cbLocom.Text.Trim() != "" && cbLocom.Text.Trim() != "ВЫБЕРИТЕ")
                    vagManeuver.Lock_id_locom = qJournalBUS.addDevice(_newElement.device, (Shop)cbShop.SelectedItem).id;
                else*/
                vagManeuver.Lock_id_locom = -1;
            }

            vagManeuver.Lock_order = order;
            vagManeuver.dt_from_way = DateTime.Now;
            //try
            //{
                return vagManeuverDB.addOnManeuver(vagManeuver);
            //}
            //catch (Exception ex)
            //{
            //    main.showErrorMessage(ex.Message);
            //    return false;
            //}
        }

        private void ManChangeVagNumsWayFrom()
        {
            List<VagManeuver> remainedVagons = view.getRemainedVagForMan();
            foreach (VagManeuver item in remainedVagons)
            {
                vagManeuverDB.changeVagNumsWayFrom(remainedVagons.IndexOf(item) + 1, item.id_oper);
                log.Info("Вагон " + item.num_vag + ", remainedVagons.IndexOf(item)=" + remainedVagons.IndexOf(item) + ", item.id_oper=" + item.id_oper);
            }
        }

        private void changeVagAmountOnWaysAfterManeuver()
        {
            view.selectedWayTo.Vag_amount = view.selectedWayTo.Vag_amount + view.listVagOnMan.Count;

            view.selectedWayFrom.Vag_amount = view.selectedWayFrom.Vag_amount - view.listVagOnMan.Count;


            //Log(DateTime.Now.ToString() + " After. bsWayTo: " + ((Way)ManBsWayTo.List[dgvWayTo.SelectedRows[0].Index]).Vag_amount.ToString() +
            //    ", After. bsWayFrom: " + ((Way)ManBsWayFrom.List[dgvWayFrom.SelectedRows[0].Index]).Vag_amount.ToString() + "\r\n");

            view.refreshWaysTables();
            log.Info("Кол-во вагонов на пути_С после маневра: " + view.selectedWayFrom.Vag_amount +
                    ". Кол-во вагонов на пути_На после маневра: " + view.selectedWayTo.Vag_amount);
            //Log(DateTime.Now.ToString() + " Refresh made. bsWayTo: " + ((Way)ManBsWayTo.List[dgvWayTo.SelectedRows[0].Index]).Vag_amount.ToString() +
            //    ", bsWayFrom: " + ((Way)ManBsWayFrom.List[dgvWayFrom.SelectedRows[0].Index]).Vag_amount.ToString() + "\r\n");
            //Log(DateTime.Now.ToString() + " Refresh made. DGVwayFrom: " + dgvWayFrom.SelectedRows[0].Cells[2].Value.ToString() + "\r\n");
        }

        private string getFirstVagCondName()
        {
            string condName = "";
            try
            {
                condName = view.listVagForMan[0].cond.Name;
            }
            catch (ArgumentOutOfRangeException) { }
            return condName;
        }
    }
}
