using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using EFRailCars.Helpers;

namespace RailwayCL
{
    public interface IVagWaitAdmissView
    {
        // работа с таблицами вагонов
        int dgvVagColumnsCount { get; }
        int dgvSelVagColumnsCount { get; }

        void makeDgvVagColumns();
        void makeDgvSelVagColumns();
        void changeColumnsPositions(bool isDepart);
        void fillRospColumns(List<Way> list, string cbDisplay, string cbValue);

        // инф. по пунктам отправки
        bool hasFromStatVag { get; }
        bool hasSelFromStatVag { get; }

        bool hasGfVag { get; }
        bool hasSelFromGfVag { get; }

        bool hasShopVag { get; }
        bool hasSelFromShopVag { get; }

        //void showHideShopVag(bool show);

        // поезда
        Train getSelTrain(bool isGf, bool isShop);
        List<Train> getTrainsList(bool isGf, bool isShop);
        void setCurrTrain(bool isGf, bool isShop, int idx);

        void setTrainNumAndDt(string trainNum, string dt);
        void showHideTrainsGfShops(bool show, bool hasGf, bool hasShops);

        void bindTrainsFromStatToSource(List<Train> list);
        void clearTrainsFromStatSelection();
        void bindTrainsGfToSource(List<Train> list);
        void clearTrainsGfSelection();
        void bindTrainsShopsToSource(List<Train> list);
        void clearTrainsShopsSelection();
        void removeTrain(bool isGF, bool isShop, Train train);
        void refreshTrains(bool isGf, bool isShop);

        // конкретный вагон
        //string firstVagCondName { get; } ///
        bool isVagSelected(int vagIdx);
        bool isVagColored(int vagIdx); // выделен ли вагон цветом
        //VagWaitAdmiss currVag { get; } ///
        //VagWaitAdmiss lastVag { get; } ///
        //VagWaitAdmiss getVagByIdx(int idx); ///
        int idxFirstSelVag { get; }
        int idxCurVag { get; }
        //int idxVag(VagWaitAdmiss vagon); ///
        VagWaitAdmiss firstSelVagToAdm { get; } // первый выделенный вагон на зачислении

        void selectVagByIdx(int idx);

        void addVagToAdmFromVags(int vagIdx); // добавить вагон в список выбранных для зачисления, по указанному индексу из табл. прибывших вагонов
        void setVagColor(int vagIdx, Color color);
        void removeFromVagWaitAdm(VagWaitAdmiss vagon);
        void removeFromVagToAdm(VagWaitAdmiss vagon);
        void vagTableSetScrollToSelRow(); // при поиске вагона прокрутить scroll таблицы с вагонами, чтобы был виден найденный вагон

        // вагоны
        int selVagCount { get; }
        int selVagToAdmCount { get; }
        //int vagCount { get; } ///
        //int vagToAdmCount { get; } ///
        List<VagWaitAdmiss> listWaitAdmiss { get; } // список прибывших вагонов
        List<VagWaitAdmiss> listToAdmiss(); // список выбранных вагонов на зачисление

        void clearVagToAdm();
        void clearVagForAdmSel();
        void bindVagWaitAdmissToSource(List<VagWaitAdmiss> list);
        void bindVagToAdmToSource(List<VagWaitAdmiss> list);
        void selectAllVagToAdm();

        // диал.окно зачисления поезда на путь
        Way wayPerformAdmissTrain { get; }
        Side sidePerformAdmissTrain { get; }
        DateTime dtArriveAdmissTrain { get; }

        bool getDialogTrainResult(Station stat, SendingPoint sp, bool sideActiv);

        // диал.окно транзита
        Way wayPerformTransit { get; }
        DateTime dtArriveTransit { get; }
        Station statPerformTransit { get; }

        bool getDialogTransitResult(Station stat);


        int editingValue { get; }

        void rospCheckChanged(); 

        bool chRospuskCheck { get; }
    }
}
