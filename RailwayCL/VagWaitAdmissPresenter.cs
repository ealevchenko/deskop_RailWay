using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;
using log4net;
using log4net.Config;
using EFRailCars.Helpers;

namespace RailwayCL
{
    public class VagWaitAdmissPresenter
    {
        IMainView main;
        IVagWaitAdmissView view;
        MainPresenter mainPresenter;

        VagOperationsDB vagOperationsDB = new VagOperationsDB();
        NeighbourStationsDB neighbourStationsDB = new NeighbourStationsDB();
        WayDB wayDB = new WayDB();
        VagSendOthStDB vagSendOthStDB = new VagSendOthStDB();
        VagWaitAdmissDB vagWaitAdmissDB = new VagWaitAdmissDB();

        private static readonly ILog log = LogManager.GetLogger(typeof(VagManeuverPresenter));

        public VagWaitAdmissPresenter(IMainView main, IVagWaitAdmissView view)
        {
            this.main = main;
            this.view = view;
            this.mainPresenter = new MainPresenter(main);
        }
        /// <summary>
        /// Загрузка панели прибытие вагонов
        /// </summary>
        public void loadVagWaitAdmissTab()
        {
            try
            {
                if (view.dgvVagColumnsCount == 0) view.makeDgvVagColumns();
                if (view.dgvSelVagColumnsCount == 0) view.makeDgvSelVagColumns();
                view.fillRospColumns(wayDB.getWays(main.selectedStation, true), WayUtils.GetInstance().CbDisplay, WayUtils.GetInstance().CbValue);

                view.setTrainNumAndDt("", "");
                view.clearVagToAdm();

                loadTrains();
                loadVagWaitAdmiss(view.hasGfVag, view.hasShopVag);
                main.setFieldWithSelVagAmount("");

                view.changeColumnsPositions(getFirstVagCondName() == "для отправки на УЗ");
                showHideGrFrAndShops();
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void onTrainStatSelect()
        {
            try
            {
                loadVagWaitAdmiss(false, false);
                view.clearVagToAdm();
                view.clearTrainsGfSelection();
                view.clearTrainsShopsSelection();

                view.changeColumnsPositions(getFirstVagCondName() == "для отправки на УЗ");
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void onTrainGfSelect()
        {
            try
            {
                loadVagWaitAdmiss(true, false);
                view.clearVagToAdm();
                view.clearTrainsFromStatSelection();
                view.clearTrainsShopsSelection();
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void onTrainShopSelect()
        {
            try
            {
                loadVagWaitAdmiss(false, true);
                view.clearVagToAdm();
                view.clearTrainsFromStatSelection();
                view.clearTrainsGfSelection();
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void onEditingRospWay(Way selWay)
        {
            try
            {
                Way way = view.listWaitAdmiss[view.idxCurVag].WayFact;
                way.ID = view.editingValue;
                way.Num = selWay.Num;
                way.Name = selWay.Name;
                way.NumName = selWay.NumName;
                way.Stat = main.selectedStation;
                way.Vag_amount = selWay.Vag_amount;
                way.Bind_cond.Id = selWay.Bind_cond.Id;
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }
        /// <summary>
        /// Выбран вагон для зачисления
        /// </summary>
        public void onVagSelect()
        {
            try
            {
                if (view.selVagCount != 0)
                {
                    //if (bs2P2.DataSource == null) bs2P2.DataSource = new List<VagWaitAdmiss>();

                    if (view.selVagCount > 1)
                    {
                        for (int i = 0; i <= view.listWaitAdmiss.Count - 1; i++)
                        {
                            if (view.isVagSelected(i))
                            {
                                if (!view.isVagColored(i))
                                {
                                    view.addVagToAdmFromVags(i);
                                    view.setVagColor(i, Color.Yellow);
                                }
                            }
                            else
                            {
                                if (view.listToAdmiss()[view.listToAdmiss().Count - 1] == view.listWaitAdmiss[i])
                                {
                                    VagWaitAdmiss vagon = view.listWaitAdmiss[i];
                                    view.removeFromVagToAdm(vagon);
                                    view.setVagColor(i, Color.Empty);
                                }
                            }
                        }
                    }
                    else //if (dgv.SelectedRows.Count == 1)
                    {
                        if (!view.isVagColored(view.idxFirstSelVag))
                        {
                            view.addVagToAdmFromVags(view.idxFirstSelVag);
                            view.setVagColor(view.idxFirstSelVag, Color.Yellow);
                        }
                    }

                    placeInRightOrder();
                }
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }
        // Убрать выделеный вагон
        public void cancelAllToAdm()
        {
            //view.selectAllVagToAdm();
            clearYellowSelectionMultipleVagons();
            view.clearVagToAdm();
            view.clearVagForAdmSel();
        }

        public void onRemoveVagToAdm()
        {
            int srCount = view.selVagToAdmCount;

            for (int i = 0; i <= srCount - 1; i++)
            {
                view.setVagColor(view.firstSelVagToAdm.num_vag_on_way - 1, Color.Empty);
                view.removeFromVagToAdm(view.firstSelVagToAdm);
            }
            view.clearVagForAdmSel();
        }
        /// <summary>
        /// Принять поезд целиком
        /// </summary>
        public void performAdmissTrain()
        {
            try
            {
                if (view.listWaitAdmiss.Count == 0) return;

                bool isStat = view.hasSelFromStatVag;
                bool isGF = view.hasSelFromGfVag;
                bool isShop = view.hasSelFromShopVag;

                if (view.getDialogTrainResult(main.selectedStation, view.getSelTrain(isGF, isShop).SendingPoint, isStat||main.selectedStation.ID==17))
                {
                    //try
                    //{
                    List<VagWaitAdmiss> list = view.listWaitAdmiss;

                    bool leftVagOnGf = false;
                    // Оставить вагон на вагонаопрокиде
                    if (isGF)
                    {
                        if (list.Count > 1 && main.showQuestMessage("Оставить один вагон на опрокиде?"))
                        {
                            list.RemoveAt(list.Count - 1);
                            leftVagOnGf = true;
                        }
                    }

                    foreach (VagWaitAdmiss item in list)
                    {
                        if (!isStat)
                        {
                            changeGruz(item, isShop, view.getSelTrain(isGF, isShop).SendingPoint, view.getSelTrain(isGF, isShop).DateFromStat);
                            mainPresenter.changeLoadCondAfter(item);
                        }
                        // Установили путь
                        mainPresenter.changeConditionWayOn(item, view.wayPerformAdmissTrain);
                        // Определение четной нечетной стороны sidePerformAdmissTrain (диалоговое окно)
                        log.Info("sidePerformAdmissTrain: " + view.sidePerformAdmissTrain.ToString());
                        //if (main.numSide == view.sidePerformAdmissTrain || !isStat)
                        if (main.numSide == view.sidePerformAdmissTrain || view.sidePerformAdmissTrain == Side.Empty)
                        {
                            if (main.selectedStation.ID == 17 && isShop)
                                item.num_vag_on_way = list.IndexOf(item) + 1;
                            else item.num_vag_on_way = list.Count - list.IndexOf(item);
                            log.Info("item.num_vag_on_way:" + item.num_vag_on_way.ToString());
                        }
                        else
                        {
                            item.num_vag_on_way = view.wayPerformAdmissTrain.Vag_amount + list.IndexOf(item) + 1;
                            log.Info("item.num_vag_on_way:" + item.num_vag_on_way.ToString());
                        }

                        int ins_result = vagWaitAdmissDB.execAdmissOthStat(item, main.selectedStation,
                            view.wayPerformAdmissTrain, item.dt_on_stat/*view.dtArriveAdmissTrain*/, view.getSelTrain(isGF, isShop).St_lock_locom1, view.getSelTrain(isGF, isShop).St_lock_locom2);
                        //if (ins_result != null) ((VagWaitAdmiss)bs1P2.List[bs1P2.IndexOf(item)]).id_oper = Convert.ToInt32(ins_result);
                        if (ins_result != -1)
                        {
                            item.id_oper = ins_result;
                            log.Info("ins_result: " + ins_result);
                        }
                        else return;
                    }

                    //if (main.numSide == view.sidePerformAdmissTrain || !isStat)
                    if (main.numSide == view.sidePerformAdmissTrain || view.sidePerformAdmissTrain == Side.Empty)
                    // изменить нумерацию вагонов на пути назначения
                    //new VagOperationsDB().changeVagNumsWayOn(list.Count, ((VagWaitAdmiss)bs1P2.List[bs1P2.IndexOf(list[0])]).id_oper, vagAcceptForm.getWay());
                    {
                        vagOperationsDB.changeVagNumsWayOn(list.Count, list[0].id_oper, view.wayPerformAdmissTrain);
                        log.Info("list[0].id_oper" + list[0].id_oper);
                    }

                    if (!isGF || !leftVagOnGf)
                    {
                        view.removeTrain(isGF, isShop, view.getSelTrain(isGF, isShop));
                    }
                    else
                    {
                        view.getSelTrain(isGF, isShop).Vag_amount = 1;
                        view.refreshTrains(true, false);
                    }
                    loadVagWaitAdmiss(isGF, isShop);
                    main.showInfoMessage("Вагоны успешно зачислены на путь!");
                    //}
                    //catch (Exception ex)
                    //{
                    //    main.showErrorMessage(ex.Message);
                    //}
                }
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }
        /// <summary>
        /// Принять партию вагонов
        /// </summary>
        public void performAdmissVagPart()
        {
            try
            {
                bool isStat = view.hasSelFromStatVag;
                bool isGF = view.hasSelFromGfVag;
                bool isShop = view.hasSelFromShopVag;

                if (view.getDialogTrainResult(main.selectedStation, view.getSelTrain(isGF, isShop).SendingPoint, isStat||main.selectedStation.ID == 17))
                {
                    //try
                    //{
                    List<VagWaitAdmiss> list = view.listToAdmiss();
                    foreach (VagWaitAdmiss item in list)
                    {
                        if (!isStat)
                        {
                            // определение состояния вагона
                            changeGruz(item, isShop, view.getSelTrain(isGF, isShop).SendingPoint, view.getSelTrain(isGF, isShop).DateFromStat);
                            mainPresenter.changeLoadCondAfter(item);
                        }

                        mainPresenter.changeConditionWayOn(item, view.wayPerformAdmissTrain);
                        // -----------------------------
                        log.Info("sidePerformAdmissTrain: " + view.sidePerformAdmissTrain.ToString());
                        //if (main.numSide == view.sidePerformAdmissTrain)
                        if (main.numSide == view.sidePerformAdmissTrain || view.sidePerformAdmissTrain == Side.Empty)
                        {
                            if (main.selectedStation.ID == 17 && isShop)
                                item.num_vag_on_way = list.IndexOf(item) + 1;
                            else item.num_vag_on_way = list.Count - list.IndexOf(item);
                            log.Info("item.num_vag_on_way:" + item.num_vag_on_way.ToString());
                        }
                        else
                        {
                            item.num_vag_on_way = view.wayPerformAdmissTrain.Vag_amount + list.IndexOf(item) + 1;
                            log.Info("item.num_vag_on_way:" + item.num_vag_on_way.ToString());
                        }

                        int ins_result = vagWaitAdmissDB.execAdmissOthStat(item, main.selectedStation,
                            view.wayPerformAdmissTrain, view.dtArriveAdmissTrain, view.getSelTrain(isGF, isShop).St_lock_locom1, view.getSelTrain(isGF, isShop).St_lock_locom2);
                        if (ins_result != -1)
                        {
                            item.id_oper = ins_result;
                            log.Info("ins_result: " + ins_result);
                        }
                        else return;
                    }

                    //if (main.numSide == view.sidePerformAdmissTrain || !isStat)
                    if (main.numSide == view.sidePerformAdmissTrain || view.sidePerformAdmissTrain == Side.Empty)
                    // изменить нумерацию вагонов на пути назначения
                    {
                        vagOperationsDB.changeVagNumsWayOn(list.Count, list[0].id_oper, view.wayPerformAdmissTrain);
                        log.Info("list[0].id_oper" + list[0].id_oper);
                    }

                    //убрать вагоны выделенные желтым
                    int i = view.listWaitAdmiss.Count - 1;
                    while (i >= 0)
                    {
                        if (view.isVagColored(i))
                            view.removeFromVagWaitAdm(view.listWaitAdmiss[i]);
                        i--;
                    }

                    //если не осталось вагонов удалить строку о поезде
                    if (view.listWaitAdmiss.Count == 0)
                    {
                        view.removeTrain(isGF, isShop, view.getSelTrain(isGF, isShop));
                    }
                    else
                    {
                        view.getSelTrain(isGF, isShop).Vag_amount = view.getSelTrain(isGF, isShop).Vag_amount - view.listToAdmiss().Count;
                        view.refreshTrains(isGF, isShop);
                    }

                    view.clearVagToAdm();
                    loadVagWaitAdmiss(isGF, isShop);
                    main.showInfoMessage("Вагоны успешно зачислены на путь!");
                    //}
                    //catch (Exception ex)
                    //{
                    //    main.showErrorMessage(ex.Message);
                    //}
                }
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }
        /// <summary>
        /// Выбран флажок роспуск
        /// </summary>
        public void onRospCheckChanged()
        {
            view.rospCheckChanged();
        }

        public void performRospusk()
        {
            if (!view.hasSelFromStatVag) return;
            try
            {
                Side arriveSide = neighbourStationsDB.getArrivSide(main.selectedStation, (Station)view.getSelTrain(false, false).SendingPoint);

                List<VagWaitAdmiss> list = (from v in view.listWaitAdmiss where v.WayFact.ID != -1 select v).ToList();

                foreach (VagWaitAdmiss item in list)
                {
                    List<VagWaitAdmiss> listOnSameWay = (from v in list where v.WayFact.ID == item.WayFact.ID select v).ToList();

                    if (main.numSide == arriveSide)
                    {
                        item.num_vag_on_way = listOnSameWay.Count - listOnSameWay.IndexOf(item);
                    }
                    else item.num_vag_on_way = item.WayFact.Vag_amount + listOnSameWay.IndexOf(item) + 1;

                    //определение состояния вагона
                    mainPresenter.changeConditionWayOn(item, item.WayFact);
                    //----------------------------

                    int ins_result = vagWaitAdmissDB.execAdmissOthStat(item, main.selectedStation,
                        item.WayFact, DateTime.Now, view.getSelTrain(false, false).St_lock_locom1, view.getSelTrain(false, false).St_lock_locom2);
                    if (ins_result != -1) view.listWaitAdmiss[view.listWaitAdmiss.IndexOf(item)].id_oper = ins_result;
                    else return;
                }

                if (main.numSide == arriveSide)
                // изменить нумерацию вагонов на пути назначения
                {
                    var result = list.GroupBy(l => l.WayFact.ID).Select(c1 =>
                        new { Quantity = c1.Count(), id_oper = c1.First().id_oper, wayFact = c1.First().WayFact }).ToList();

                    foreach (var item in result)
                    {
                        vagOperationsDB.changeVagNumsWayOn(item.Quantity, item.id_oper, item.wayFact);
                    }
                }

                //убрать вагоны размеченные по путям
                int i = view.listWaitAdmiss.Count - 1;
                while (i >= 0)
                {
                    if (view.listWaitAdmiss[i].WayFact.ID != -1)
                        view.removeFromVagWaitAdm(view.listWaitAdmiss[i]);
                    i--;
                }

                //если не осталось вагонов удалить строку о поезде
                if (view.listWaitAdmiss.Count == 0)
                {
                    view.removeTrain(false, false, view.getSelTrain(false, false));
                    loadVagWaitAdmiss(false, false);
                }
                else
                {
                    view.getSelTrain(false, false).Vag_amount = view.getSelTrain(false, false).Vag_amount - list.Count;
                    view.refreshTrains(false, false);
                }
                main.showInfoMessage("Роспуск успешно выполнен!");
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }
        /// <summary>
        /// Пересылка на другую станцию функция "транзит"
        /// </summary>
        public void performAccSend() // пересылание на др. станцию
        {
            if (!view.hasSelFromStatVag) return;

            try
            {
                if (view.getDialogTransitResult(main.selectedStation))
                {
                    List<VagWaitAdmiss> list = view.listWaitAdmiss;

                    foreach (VagWaitAdmiss item in list)
                    {
                        item.num_vag_on_way = list.IndexOf(item) + 1;

                        //принятие на станцию транзита
                        object ins_result = vagWaitAdmissDB.execAdmissOthStat(item, main.selectedStation,
                            view.wayPerformTransit, view.dtArriveTransit, view.getSelTrain(false, false).St_lock_locom1, view.getSelTrain(false, false).St_lock_locom2);
                        if (ins_result != null) view.listWaitAdmiss[view.listWaitAdmiss.IndexOf(item)].id_oper = Convert.ToInt32(ins_result);
                        else return;

                        //данные о след. станции, на кот. отправл. 
                        VagSendOthSt vagSendOthSt = new VagSendOthSt(item);
                        vagSendOthSt.St_lock_id_stat = view.statPerformTransit.ID;
                        vagSendOthSt.St_lock_order = item.num_vag_on_way;
                        vagSendOthSt.St_lock_train = view.getSelTrain(false, false).Num;
                        vagSendOthSt.St_lock_side = neighbourStationsDB.getSendSide(main.selectedStation, view.statPerformTransit);
                        vagSendOthStDB.addToSend(vagSendOthSt);

                        //отправление со станции транзита
                        vagSendOthStDB.send(item.id_oper, item.cond.Id, view.dtArriveTransit, view.dtArriveTransit);
                    }

                    //удаляем строку поезда
                    view.removeTrain(false, false, view.getSelTrain(false, false));
                    loadVagWaitAdmiss(false, false);
                }
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }
        /// <summary>
        /// Очистить поле с количеством выделенных вагонов
        /// </summary>
        public void onVagToAdmListChanged()
        {
            main.setFieldWithSelVagAmount(view.listToAdmiss().Count.ToString());
        }

        public void cancelVagInGfOrShop(bool isGf, bool isAllVag)
        {
            try
            {
                Train train = view.getSelTrain(isGf, !isGf);
                if (train == null)
                {
                    main.showErrorMessage("Поезд не выбран!");
                    return;
                }
                List<VagWaitAdmiss> list = new List<VagWaitAdmiss>();
                if (!isAllVag)
                {
                    list = view.listToAdmiss();
                    if (list.Count == 0)
                    {
                        main.showErrorMessage("Вагоны не выбраны!");
                        return;
                    }
                }
                vagWaitAdmissDB.changeNumVagsAfterCancel(wayDB.getWayByIdOper(view.listWaitAdmiss[0].id_oper), train, list.Cast<VagOperations>().ToList());
                vagWaitAdmissDB.cancelGfOrShopSending(train, main.selectedStation, isGf, list);
                main.showInfoMessage("Отмена произведена успешно!");


                //удаляем строку поезда
                if (isAllVag || view.listWaitAdmiss.Count == view.listToAdmiss().Count)
                    view.removeTrain(isGf, !isGf, train);
                else
                {
                    train.Vag_amount = train.Vag_amount - view.listToAdmiss().Count;
                    view.refreshTrains(isGf, !isGf);
                }
                view.clearVagToAdm();
                loadVagWaitAdmiss(isGf, !isGf);
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void searchVag()
        {
            if (main.numVagForSearch == 0) return;
            Tuple<DateTime, int, int, int> tuple = vagWaitAdmissDB.findVagLocation(main.selectedStation, main.numVagForSearch);
            if (tuple.Item1 != DateTime.MinValue && tuple.Item2 != 0)
            {
                bool isGf = false; bool isShop = false;
                if (tuple.Item3 > 0) isGf = true;
                if (tuple.Item4 > 0) isShop = true;

                List<Train> trainsList = view.getTrainsList(isGf, isShop);
                Train train = (from w in trainsList where w.DateFromStat == tuple.Item1 select w).FirstOrDefault();
                int vagNumInTrain = tuple.Item2;
               
                view.setCurrTrain(isGf, isShop, trainsList.IndexOf(train));
                if (isGf) onTrainGfSelect();
                else if (isShop) onTrainShopSelect();
                else onTrainStatSelect();

                main.findVagByNumPerforming = true;
                view.selectVagByIdx(vagNumInTrain);
                main.findVagByNumPerforming = false;

                view.vagTableSetScrollToSelRow();
            }
            else main.showWarningMessage("Вагон не найден.");
        }

        public void trainToExcel()
        {

            bool brospusk = view.chRospuskCheck;
            Train train = view.getSelTrain(view.hasGfVag, view.hasShopVag);
            if (train == null) return;
            Excel.Application ExcelApp = new Excel.Application();
            //Книга.
            Excel.Workbook WBE = ExcelApp.Workbooks.Add(Type.Missing);
            int field = 1;
            try
            {
                Excel._Worksheet WSE = (Excel.Worksheet)WBE.Sheets[1];
                WSE.Cells[1, 1].Value = "Поезд №" + train.Num;
                WSE.Cells[2, 1].Value = "Пункт отправки: " + train.SendingPoint.Name;
                WSE.Cells[3, 1].Value = "Дата отправления: " + train.DateFromStat;
                WSE.Cells[5, 1].Value = "Станция назначения: " + train.StationTo.Name;

                List<VagWaitAdmiss> list = view.listWaitAdmiss;
                int row = 7;
                
                foreach (VagWaitAdmiss item in list)
                {
                    field = 1;
                    if (row == 7)
                    {
                        WSE.Cells[row, field++].Value = "№";
                        WSE.Cells[row, field++].Value = "№ вагона";
                        if (brospusk) { WSE.Cells[row, field++].Value = "Роспуск факт"; }
                        WSE.Cells[row, field++].Value = "Род вагона";
                        WSE.Cells[row, field++].Value = "Собственник";
                        WSE.Cells[row, field++].Value = "Страна собств.";
                        WSE.Cells[row, field++].Value = "Годность";
                        WSE.Cells[row, field++].Value = "Род груза";
                        if (!brospusk) { WSE.Cells[row, field++].Value = "Состояние"; }
                        WSE.Cells[row, field++].Value = "Дата/время захода на АМКР";
                        WSE.Cells[row, field++].Value = "Станц. отправл. УЗ";
                        WSE.Cells[row, field++].Value = "Груз по прибыт. на АМКР";
                        if (!brospusk) { WSE.Cells[row, field++].Value = "Вес груза"; }
                        WSE.Cells[row, field++].Value = "Цех-получ. груза";
                        WSE.Cells[row, field++].Value = "Дата письма";
                        WSE.Cells[row, field++].Value = "№ письма";
                        WSE.Cells[row, field++].Value = "Текст письма";
                        WSE.Cells[row, field++].Value = "Станц. указ. в письме";
                        WSE.Cells[row, field++].Value = "Собств. указ. в письме";
                        if (!brospusk)
                        {
                            WSE.Cells[row, field++].Value = "Станция назнач.";
                            WSE.Cells[row, field++].Value = "Примечан.";
                            WSE.Cells[row, field++].Value = "Страна назнач.";
                            WSE.Cells[row, field++].Value = "№ тупика";
                            WSE.Cells[row, field++].Value = "Грузоподъемность";
                            WSE.Cells[row, field++].Value = "Грузополучатель";
                            WSE.Cells[row, field++].Value = "Дата/время постановки на путь";
                        }
                        //WSE.Cells[row, field++].Value = "Роспуск план";
                        
                        row++;
                        // -- f o r m a t t i n g --
                        Excel.Range range = WSE.get_Range((Excel.Range)WSE.Cells[7, 1], (Excel.Range)WSE.Cells[7, 27]);
                        range.Font.Bold = true;
                        range.WrapText = true;
                    }
                    field = 1;
                    //foreach (var prop in item.GetType().GetProperties())
                    WSE.Cells[row, field++].Value = item.num_vag_on_way;
                    WSE.Cells[row, field++].Value = item.num_vag;
                    if (brospusk) { WSE.Cells[row, field++].Value = item.WayFact.NumName; }
                    WSE.Cells[row, field++].Value = item.rod;
                    WSE.Cells[row, field++].Value = item.owner;
                    WSE.Cells[row, field++].Value = item.own_country;
                    WSE.Cells[row, field++].Value = item.godn;
                    WSE.Cells[row, field++].Value = item.gruz;
                    if (!brospusk) { WSE.Cells[row, field++].Value = item.cond.Name; }
                    WSE.Cells[row, field++].Value = item.dt_amkr;
                    WSE.Cells[row, field++].Value = item.outer_station;
                    WSE.Cells[row, field++].Value = item.gruz_amkr;
                    if (!brospusk) { WSE.Cells[row, field++].Value = item.weight_gruz; }
                    WSE.Cells[row, field++].Value = item.ceh_gruz;
                    WSE.Cells[row, field++].Value = item.MailDate;
                    WSE.Cells[row, field++].Value = item.MailNum;
                    WSE.Cells[row, field++].Value = item.MailText;
                    WSE.Cells[row, field++].Value = item.MailStat;
                    WSE.Cells[row, field++].Value = item.MailSobstv;
                    if (!brospusk)
                    {
                        WSE.Cells[row, field++].Value = item.gdstait;
                        WSE.Cells[row, field++].Value = item.note;
                        WSE.Cells[row, field++].Value = item.nazn_country;
                        WSE.Cells[row, field++].Value = item.tupik;
                        WSE.Cells[row, field++].Value = item.grvuSAP;
                        WSE.Cells[row, field++].Value = item.ngruSAP;
                        WSE.Cells[row, field++].Value = item.dt_on_way;
                    }

                    //WSE.Cells[row, field++].Value = item.WayPlan.NumName;
                    
                    row++;
                }
                // -- f o r m a t t i n g --
                Excel.Range range1 = WSE.get_Range((Excel.Range)WSE.Cells[7, 1], (Excel.Range)WSE.Cells[row - 1, field]);
                range1.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                range1.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                //range1.WrapText = true;
                range1.Columns.AutoFit();
                WSE.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                WSE.PageSetup.Zoom = false;
                WSE.PageSetup.FitToPagesWide = 1;
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
                WBE.Close(false, Type.Missing, Type.Missing);  // ---- закрыть Excel не сохраняя
            }
            ExcelApp.Visible = true;
            GC.Collect();
        }

        public void trainDelete() 
        {
            List<VagWaitAdmiss> list = view.listWaitAdmiss;
            foreach (VagWaitAdmiss item in list)
            {
                vagWaitAdmissDB.deleteVagOperations(item.id_oper);
            }
        }
        /// <summary>
        /// Убрать цвет выделенного вагона
        /// </summary>
        private void clearYellowSelectionMultipleVagons()
        {
            for (int i = 0; i <= view.listWaitAdmiss.Count - 1; i++)
            {
                view.setVagColor(i, Color.Empty);
            }
        }
        /// <summary>
        /// ?? Растановка в правильном порядке
        /// </summary>
        private void placeInRightOrder()
        {
            try
            {
                List<VagWaitAdmiss> listVagFromCeh = view.listToAdmiss();
                view.bindVagToAdmToSource(listVagFromCeh.OrderBy(x => x.num_vag_on_way).ToList());
            }
            catch (ArgumentNullException)
            {
            }
        }
        /// <summary>
        /// Загрузить поезда в окна (станции,вагоноопрокиды, цеха)
        /// </summary>
        private void loadTrains()
        {
            view.bindTrainsFromStatToSource(vagWaitAdmissDB.getTrains(main.selectedStation, new Station()));
            view.clearTrainsFromStatSelection();
            view.bindTrainsGfToSource(vagWaitAdmissDB.getTrains(main.selectedStation, new GruzFront()));
            view.clearTrainsGfSelection();
            view.bindTrainsShopsToSource(vagWaitAdmissDB.getTrains(main.selectedStation, new Shop()));
            view.clearTrainsShopsSelection();

            if (view.hasGfVag)
            {
                view.setCurrTrain(true, false, 0);//selectFirstGfVag();
                //return new GruzFront();
            }
            else if (view.hasShopVag)
            {
                view.setCurrTrain(false, true, 0);//selectFirstShopVag();
                //return new Shop();
            }
            else
            {
                if (!view.hasSelFromStatVag && view.hasFromStatVag)
                {
                    view.setCurrTrain(false, false, 0);//selectFirstFromStatVag();
                }
                //return new Station();
            }
        }
        /// <summary>
        /// Получить перечень вагонов (опрокид, цех, )
        /// </summary>
        /// <param name="isGf"></param>
        /// <param name="isShop"></param>
        private void loadVagWaitAdmiss(bool isGf, bool isShop)
        {
            Train train = view.getSelTrain(isGf, isShop);
            if (train != null)
            {
                view.setTrainNumAndDt(train.Num.ToString(), train.DateFromStat.ToString("g"));
                view.bindVagWaitAdmissToSource(vagWaitAdmissDB.getVagons(train, main.selectedStation, main.numSide, isGf, isShop));
            }
            else view.bindVagWaitAdmissToSource(new List<VagWaitAdmiss>());
        }
        /// <summary>
        /// Установка размеров видимости гаражи опрокиды, цеха
        /// </summary>
        private void showHideGrFrAndShops()
        {
            view.showHideTrainsGfShops(view.hasGfVag || view.hasShopVag, view.hasGfVag, view.hasShopVag);
            //view.showHideShopVag(view.hasShopVag);
        }

        private void changeGruz(VagWaitAdmiss vag, bool isShop, SendingPoint sp, DateTime dtFromStat)
        {
            if (vag.gruz.Contains("порож") && isShop)
            {
                getLoadingData(vag, (Shop)sp, dtFromStat);
            }
            else
            {
                vag.id_gruz = 6;
                vag.gruz = "порож";
            }
        }

        private void getLoadingData(VagWaitAdmiss vag, Shop shop, DateTime dtFromStat)
        {
            vagWaitAdmissDB.getLoadingData(vag, shop, dtFromStat);
        }
        /// <summary>
        /// Показать поле состояние вагона
        /// </summary>
        /// <returns></returns>
        private string getFirstVagCondName()
        {
            string condName = "";
            try
            {
                condName = view.listWaitAdmiss[0].cond.Name;
            }
            catch (ArgumentOutOfRangeException) { }
            return condName;
        }
    }
}
