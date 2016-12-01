using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public interface IMainView
    {
        int wayIdxToSelect { get; set; } // переменная для хранения выбранного пути (для автоматич. выбора при переключ. на др. вкладку
        Side numSide { get; } // выбранная горловина нумерации вагонов
        Station selectedStation { get; } // выбранная станция
        bool selectAllBtnClicked { get; set; }
        int numVagForSearch { get; }
        bool findVagByNumPerforming { get; set; } // вспомогат. переменная, т.к. событие onSelectionChanged происходит
        // неявно (не только при выделении мышью)

        void bindStationsToSource(List<Station> list, string cbDisplay, string cbValue); // загрузить станции в компонент отображения (ComboBox)
        void loadSides(object[] range); // -/-/ горловины
        void setNumSide(Side side); // установить горловину
        void setFieldWithSelVagAmount(string text); // очистить поле с кол-вом выделенных вагонов
        void setRospuskIndicator(bool value); // установить индикатор роспуска
        void setTabIdx(int idx); // установить индекс вкладки интерфейса (в случае TabControl)

        void loadData(); // загрузка данных
        void searchVag(); // поиск вагона
        void showInfoMessage(string message);
        void showWarningMessage(string message);
        void showErrorMessage(string message);
        bool showQuestMessage(string message);

        // отчеты
        int repVagHistNumVag { get; } // №вагона из диалог.окна 
        bool getDialogVagHistResult(); // результат диалог.окна с №вагона перед построен. отчета 

        DateTime repPeriodBd { get; } // дата начала из диалог.окна
        DateTime repPeriodEd { get; } // дата конца из диалог.окна
        bool getDialogPeriod(); // результат диалог.окна с датой перед построен. отчета 
    }
}
