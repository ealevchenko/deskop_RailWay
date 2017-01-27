using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using EFRailCars.Helpers;

namespace RailwayCL
{
    public interface IVagManeuverView
    {
        // работа с таблицами вагонов, интерфейсом
        int dgvForManColumnsCount { get; }
        int dgvOnManColumnsCount { get; }

        void makeDgvForManColumns();
        void makeDgvOnManColumns();
        void changeColumnsPositions(bool isDepart);
        void changeLabelsSize();

        // операции с путями ("WayFrom" и "WayTo")
        Way selectedWayFrom { get; }
        Way selectedWayTo { get; }
        List<Way> listWayFrom { get; }
        List<Way> listWayTo { get; }
        //int selWayFromIdx { get; } // индекс выделенного пути "С" ///

        void bindWaysFromToSource(List<Way> list);
        void clearWaysFrom();
        void clearWaysFromSelection();
        void setCurrentWayFrom(int idxWay);
        void bindWaysOnToSource(List<Way> list);
        void clearWaysOn();
        void clearWaysOnSelection();
        void setCurrentWayTo(int wayIdx);
        void clearColorAndDtFromWayMultipleVag(); // убрать выделение цветом и дату снятия с пути для нескольких вагонов 
        void refreshWaysTables();

        // -- локомотивы, горловина...
        Side selectedSide { get; }
        Locomotive selectedLocom { get; }
        bool otherStatLocomotives { get; }

        void selectSide(Side side);
        int? selectLocom(int idx);
        void setOtherStatLocomotives(bool othStat);
        void loadSides(object[] items, string nonSelected);
        void loadLocomotives(List<Locomotive> list, string cbDisplay, string cbValue, string nonSelected);
        void clearSide(string text);
        void clearLocom(string text);

        // -- конкретный вагон
        //string firstVagCondName { get; } ///
        int idxCurVagForMan { get; } 
        //VagManeuver getCurVagForMan { get; }
        //VagManeuver firstSelVagForMan { get; }
        int idxFirstSelVagForMan { get; }
        //VagManeuver getVagForManByIdx(int idx); ///
        //int idxVagOnMan(VagManeuver vagon); ///
        bool isVagForManSelected(int vagIdx);
        bool isVagForManColored(int vagIdx); // выделен ли вагон для маневра цветом

        //VagManeuver lastVagOnMan { get; } // последний вагон, стоящий на маневре ///
        VagManeuver firstSelVagOnMan { get; } // первый выделенный вагон на маневре


        void addVagOnManFromVagsForMan(int vagForManIdx); // добавить вагон в список поставленных на маневр, по указанному индексу из табл. вагонов для маневра
        void setVagForManColor(int vagIdx, Color color); // установить цвет для вагона поставленного на маневр
        void removeFromVagOnMan(VagManeuver vagon);
        void moveToLastVagForMan();
        void moveToPrevVagForMan();
        void moveToFirstVagForMan();
        void moveToNextVagForMan();
        void selectVagForManByIdx(int idx); // выделить вагон в первой табл. по индексу
        void vagTableSetScrollToSelRow(); // при поиске вагона прокрутить scroll таблицы с вагонами, чтобы был виден найденный вагон

        // -- вагоны
        List<VagManeuver> listVagForMan { get; }
        List<VagManeuver> listVagOnMan { get; }   // Получить список вагонов в окне перенести
        List<VagManeuver> getRemainedVagForMan(); // Получить список вагонов в окне вагоны на пути
        int selVagForManCount { get; } // кол-во выделенных вагонов для маневра
        int selVagOnManCount { get; } // кол-во выделенных вагонов на маневре
        //int vagForManCount { get; } // кол-во вагонов для маневров ///
        //int vagOnManCount { get; } // кол-во вагонов на маневре ///

        void bindVagForManToSource(List<VagManeuver> list);
        void clearVagForManSelection();
        void changeColorVagSelectedForMan(List<int> rowsIdx);
        void bindVagOnManToSource(List<VagManeuver> list);
        void clearVagOnMan();
    }
}
