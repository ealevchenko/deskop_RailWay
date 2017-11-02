using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using EFRailCars.Helpers;

namespace RailwayCL
{
    public interface IVagSendOthStView
    {
        // работа с таблицами вагонов
        int dgvForSendColumnsCount { get; }
        int dgvToSendColumnsCount { get; }

        void makeDgvForSendColumns();
        void makeDgvToSendColumns();
        void changeColumnsPositions(bool isDepart);

        // операции с путями       
        Way selectedWay { get; }
        List<Way> listWays { get; }
        //int selWayFromIdx { get; } ///
       
        void clearWays();
        void bindWaysToSource(List<Way> list);
        void setCurrentWay(int idxWay);
        void refreshWayTable();

        // -- локомотивы
        bool otherStatLocomotives { get; }
        Locomotive locom1 { get; }
        Locomotive locom2 { get; }
        void loadLocomotives(List<Locomotive> list, string cbDisplay, string cbValue, string cbNonSelected);
        void clearLocoms(string text);
        int? setLocom1(int locom1);
        int? setLocom2(int locom2);
        void setOtherStatLocomotives(bool othStat);
        // -- горловина
        Side selectedSide { get; }
        void setSide(Side side);
        void loadSides(object[] items, string nonSelected);
        void clearSide(string text);
        // -- станция назначен.
        Station selectedStatAccept { get; }
        void setStatAcceptByName(string name);
        void setStatLabel(string text);
        void loadStationsTo(List<Station> list, string cbDisplay, string cbValue, string cbNonSelected);
        void clearStationsTo(string text);
        void setStationTo(int id_stat);
        // -- ГФ
        GruzFront selectedGF { get; } // вагоноопрокид
        void loadGF(List<GruzFront> list, string cbDisplay, string cbValue, string cbNonSelected);
        void clearGF(string text);
        void deleteGfItems();
        void setGF(int idGF);
        // -- Цех
        Shop selectedShop { get; }
        void loadShops(List<Shop> list, string cbDisplay, string cbValue, string cbNonSelected);
        void clearShop(string text);
        void deleteShopItems();
        void setShop(int idShop);

        void showHideGfAndShopsOnForm(bool hasGF, bool hasShops);

        // -- Поезд
        int trainNum { get; }
        void setTrainNum(int num);

        // -- конкретный вагон
        //string firstVagCondName { get; }  ///
        int idxCurVagForSend { get; }
        int idxFirstSelVagForSend { get; }
        bool isVagForSendSelected(int vagIdx);
        bool isVagForSendColored(int vagIdx); // выделен ли вагон для отправки цветом
        //VagSendOthSt lastVagToSend { get; } ///
        //VagSendOthSt getVagForSendByIdx(int idx); ///
        VagSendOthSt firstSelVagToSend { get; } // первый выделенный вагон на отправке

        void addVagToSendFromVagsForSend(int vagForSendIdx); // добавить вагон в список поставленных на отправление, по указанному индексу из табл. вагонов для оптравления
        void setVagForSendColor(int vagIdx, Color color); // установить цвет для вагона поставленного на маневр
        void removeFromVagToSend(VagSendOthSt vagon);
        void moveToLastVagForSend();
        void moveToPrevVagForSend();
        void moveToFirstVagForSend();
        void moveToNextVagForSend();
        void selectVagForSendByIdx(int idx);

        // -- вагоны
        List<VagSendOthSt> listForSending { get; }
        List<VagSendOthSt> listToSend { get; }
        List<VagSendOthSt> getRemainedVagForSending();
        int selVagForSendingCount { get; }
        int selVagToSendCount { get; }
        //int vagForSendCount { get; } // кол-во вагонов для отправки ///
        //int vagToSendCount { get; } // кол-во вагонов на отправку ///

        void bindVagForSendToSource(List<VagSendOthSt> list);
        void clearVagForSendSelection();
        //void clearVagToSendSelection();
        void changeColorVagSelectedForSend(List<int> rowsIdx);
        void bindVagToSendToSource(List<VagSendOthSt> list);
        void clearVagToSend();
        void clearColorAndDtFromWayMultipleVag(); // убрать выделение цветом и дату снятия с пути для нескольких вагонов
        void vagTableSetScrollToSelRow();
    }
}
