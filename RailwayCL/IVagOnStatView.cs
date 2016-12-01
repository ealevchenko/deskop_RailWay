using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public interface IVagOnStatView
    {
        int dgvColumnsCount{ get; }

        void makeDgvColumns();
        void changeColumnsPositions(bool isDepart);

        // операции с путями станции
        Way selectedWay { get; }
        List<Way> listWays { get; }
        //int selWayIdx { get; } ///

        void bindWaysToSource(List<Way> list);
        void clearWays();
        void clearWaysSelection();
        void setCurrentWay(int idxWay);


        // операции с вагонами на выбранном пути станции
        //string firstVagCondName { get; } ///
        List<VagOnStat> listVagons { get; }
        void bindVagOnStatToSource(List<VagOnStat> list);
        void selectVagByIdx(int p);
        void vagTableSetScrollToSelRow(); // при поиске вагона прокрутить scroll таблицы с вагонами, чтобы был виден найденный вагон
 
        // операции с цехами и ГФ
        GruzFront selectedGf { get; }
        Shop selectedShop { get; }
        int gfCount { get; }
        int shopsCount { get; }

        void bindShopsToSource(List<Shop> list);
        void clearShopsSelection();
        void bindGfToSource(List<GruzFront> list);
        void clearGfSelection();
        void showGfAndShopsOnForm(bool hasGF, bool hasShops);
        void hideGfAndShopsOnForm();

    }
}
