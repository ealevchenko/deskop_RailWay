using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ServicesStatus;

namespace RailwayCL
{
    public class VagWaitRemoveAdmissPresenter
    {
        IMainView main;
        IVagWaitRemoveAdmissView view;

        VagWaitRemoveAdmissDB vagWaitRemoveAdmissDB = new VagWaitRemoveAdmissDB();
        WayDB wayDB = new WayDB();

        public VagWaitRemoveAdmissPresenter(IMainView main, IVagWaitRemoveAdmissView view)
        {
            this.view = view;
            this.main = main;
        }

        public void loadVagWaitRemoveAdmissTab()
        {
            try
            {
                if (view.dgvVagColumnsCount == 0) view.makeDgvVagColumns();
                if (view.dgvVagToCancColumnsCount == 0) view.makeDgvVagToCancColumns();

                view.setTrainNumAndDt("", "");
                view.clearVagToCanc();
                
                view.bindTrainsWaitRemoveAdmissToSource(vagWaitRemoveAdmissDB.getTrains(main.selectedStation));
                loadVagWaitRemoveAdmiss();
                main.setFieldWithSelVagAmount("");

                view.changeColumnsPositions(getFirstVagCondName() == "для отправки на УЗ");
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void onTrainSelect()
        {
            try
            {
                loadVagWaitRemoveAdmiss();
                view.clearVagToCanc();

                view.changeColumnsPositions(getFirstVagCondName() == "для отправки на УЗ");
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void cancelAllToCanc()
        {
            //view.selectAllVagToAdm();
            clearYellowSelectionMultipleVagons();
            view.clearVagToCanc();
            view.clearVagWaitRemoveAdmissSel();
        }

        public void onRemoveVagToCanc()
        {
            int srCount = view.selVagToCancCount;

            for (int i = 0; i <= srCount - 1; i++)
            {
                view.setVagColor(view.firstSelVagToCanc.num_vag_on_way - 1, Color.Empty);
                view.removeFromVagToCanc(view.firstSelVagToCanc);
            }
            view.clearVagWaitRemoveAdmissSel();
        }

        public void cancelTrainSending(bool isAllVag)
        {
            try
            {
                Train train = view.getSelTrain();
                if (train == null)
                {
                    main.showErrorMessage("Поезд не выбран!");
                    return;
                }
                List<VagWaitRemoveAdmiss> list = new List<VagWaitRemoveAdmiss>();
                if (!isAllVag)
                {
                    list = view.listToCancel;
                    if (list.Count == 0)
                    {
                        main.showErrorMessage("Вагоны не выбраны!");
                        return;
                    }
                }
                string mess_send = String.Format("Пользователь отменил отправку состава со станции {0}, на станцию {1}", main.selectedStation.Name,view.getSelTrain().StationTo.Name);
                string status = "";
                vagWaitRemoveAdmissDB.changeNumVagsAfterCancel(wayDB.getWayByIdOper(view.listVagons[0].id_oper), train, list.Cast<VagOperations>().ToList());
                vagWaitRemoveAdmissDB.cancelTrainSending(view.getSelTrain(), main.selectedStation, list);
                foreach (VagWaitRemoveAdmiss item in list)
                {
                    status += String.Format("[состав:{0}, №:{1}, дата АМКР:{2}]; ", item.id_sostav, item.num_vag, item.dt_amkr);
                }
                main.showInfoMessage("Отмена произведена успешно!");
                mess_send.SaveLogEvents(status, service.DesktopRailCars);

                //удаляем строку поезда
                if (isAllVag || view.listVagons.Count == view.listToCancel.Count)
                    view.removeTrain(train);
                else
                {
                    train.Vag_amount = train.Vag_amount - view.listToCancel.Count;
                    view.refreshTrains();
                }
                view.clearVagToCanc();
                loadVagWaitRemoveAdmiss();
            }
            catch (Exception ex)
            {
                main.showErrorMessage(ex.Message);
            }
        }

        public void onVagSelect()
        {
            try
            {
                if (view.selVagCount != 0)
                {
                    if (view.selVagCount > 1)
                    {
                        for (int i = 0; i <= view.listVagons.Count - 1; i++)
                        {
                            if (view.isVagSelected(i))
                            {
                                if (!view.isVagColored(i))
                                {
                                    view.addVagToCancFromVags(i);
                                    view.setVagColor(i, Color.Yellow);
                                }
                            }
                            else
                            {
                                if (view.listToCancel[view.listToCancel.Count - 1] == view.listVagons[i])
                                {
                                    VagWaitRemoveAdmiss vagon = view.listVagons[i];
                                    view.removeFromVagToCanc(vagon);
                                    view.setVagColor(i, Color.Empty);
                                }
                            }
                        }
                    }
                    else //if (dgv.SelectedRows.Count == 1)
                    {
                        if (!view.isVagColored(view.idxFirstSelVag))
                        {
                            view.addVagToCancFromVags(view.idxFirstSelVag);
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

        public void onVagToCancListChanged()
        {
            main.setFieldWithSelVagAmount(view.listToCancel.Count.ToString());
        }

        public void searchVag()
        {
            if (main.numVagForSearch == 0) return;
            Tuple<DateTime, int> tuple = vagWaitRemoveAdmissDB.findVagLocation(main.selectedStation, main.numVagForSearch);
            if (tuple.Item1 != DateTime.MinValue && tuple.Item2 != 0)
            {
                Train train = (from w in view.trainsList where w.DateFromStat == tuple.Item1 select w).FirstOrDefault();
                int vagNumInTrain = tuple.Item2;

                view.setCurrTrain(view.trainsList.IndexOf(train));
                onTrainSelect();

                main.findVagByNumPerforming = true;
                view.selectVagByIdx(vagNumInTrain);
                main.findVagByNumPerforming = false;

                view.vagTableSetScrollToSelRow();
            }
            else main.showWarningMessage("Вагон не найден.");
        }


        private void loadVagWaitRemoveAdmiss()
        {
            Train train = view.getSelTrain();
            if (train != null)
            {
                view.setTrainNumAndDt(train.Num.ToString(), train.DateFromStat.ToString("g"));
                view.bindVagWaitRemoveAdmissToSource(vagWaitRemoveAdmissDB.getVagons(train, main.selectedStation));
            }
            else view.bindVagWaitRemoveAdmissToSource(new List<VagWaitRemoveAdmiss>());
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

        private void placeInRightOrder()
        {
            try
            {
                List<VagWaitRemoveAdmiss> list = view.listToCancel;
                view.bindVagToCancToSource(list.OrderBy(x => x.num_vag_on_way).ToList());
            }
            catch (ArgumentNullException)
            {
            }
        }

        private void clearYellowSelectionMultipleVagons()
        {
            for (int i = 0; i <= view.listVagons.Count - 1; i++)
            {
                view.setVagColor(i, Color.Empty);
            }
        }
    }
}
