using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RailwayCL
{
    public interface IVagWaitRemoveAdmissView
    {
        // --
        int dgvVagColumnsCount { get; }
        int dgvVagToCancColumnsCount { get; }

        void makeDgvVagColumns();
        void makeDgvVagToCancColumns();
        void changeColumnsPositions(bool isDepart);

        // --
        Train getSelTrain();
        void removeTrain(Train train);
        List<Train> trainsList { get; }
        void setCurrTrain(int idx);

        void setTrainNumAndDt(string trainNum, string dt);
        void bindTrainsWaitRemoveAdmissToSource(List<Train> list);
        void refreshTrains();

        // конкретный вагон
        bool isVagSelected(int vagIdx); 
        bool isVagColored(int vagIdx); // выделен ли вагон цветом 
        int idxFirstSelVag { get; }
        VagWaitRemoveAdmiss firstSelVagToCanc { get; } // первый выделенный вагон на отмену

        void addVagToCancFromVags(int vagIdx); // добавить вагон в список выбранных для отмены, по указанному индексу из табл. вагонов 
        void setVagColor(int vagIdx, Color color);
        void removeFromVagToCanc(VagWaitRemoveAdmiss vagon);
        void selectVagByIdx(int idx);

        // вагоны
        int selVagCount { get; }
        int selVagToCancCount { get; }
        List<VagWaitRemoveAdmiss> listVagons { get; }
        List<VagWaitRemoveAdmiss> listToCancel { get; } // список выбранных вагонов для отмены 
 
        void bindVagWaitRemoveAdmissToSource(List<VagWaitRemoveAdmiss> list);
        void bindVagToCancToSource(List<VagWaitRemoveAdmiss> list); 
        //void clearVagWaitRemoveAdmiss();
        void clearVagWaitRemoveAdmissSel();
        void clearVagToCanc();

        void vagTableSetScrollToSelRow(); // при поиске вагона прокрутить scroll таблицы с вагонами, чтобы был виден найденный вагон
    }
}
