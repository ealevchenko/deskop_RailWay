using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;
using log4net;
//using log4net.Config;
using RailwayCL;

namespace RailwayCL
{
    public enum Side { Empty = -1, Even = 0, Odd = 1 } // Четная, Нечетная

    public class MainPresenter
    {
        IMainView view;

        private static readonly ILog log = LogManager.GetLogger(typeof(MainPresenter));

        StationDB stationDB = new StationDB();

        public MainPresenter(IMainView view)
        {
            this.view = view;
        }

        public void onFormLoad()
        {
            loadInitialInfo();
            view.setTabIdx(0);
            view.loadData();
        }

        public void onStatSelect()
        {
            view.setNumSide((Side)Math.Abs((int)view.selectedStation.Outer_side - 1));
            view.loadData();
            view.setRospuskIndicator(false);
        }

        public void onSideSelect()
        {
            view.loadData();
        }

        public void onTabSelect()
        {
            view.loadData();
        }

        public void changeConditionWayOn(VagOperations vo, Way way)
        {
            if (way.Bind_cond.Id != -1)
                vo.cond.Id = way.Bind_cond.Id;
        }

        public void changeConditionWayAfter(VagOperations vo, Way way)
        {
            if (way.Bind_cond.Id != -1)
                vo.cond.Id = way.Bind_cond.Id_cond_after;
        }

        public void changeLoadCond(VagOperations vo, bool isShop)
        {
            if (vo.gruz.Contains("порож") && isShop)
            {
                vo.cond.Id = 6;
            }
            else
            {
                vo.cond.Id = 5;
            }
        }

        public void changeLoadCondAfter(VagOperations vo)
        {
            vo.cond.Id = vo.cond.Id_cond_after;
        }

        //public void changeGruz(VagOperations vo, bool isShop)
        //{
        //    if (vo.gruz.Contains("порож") && isShop)
        //    {
        //        vo.id_gruz = 87;
        //        vo.gruz = "пр.гр";
        //    }
        //    else
        //    {
        //        vo.id_gruz = 6;
        //        vo.gruz = "порож";
        //    }
        //}

        public void onRefreshForm()
        {
            view.loadData();
        }

        public void onVagSearch()
        {
            view.searchVag();
        }

        private void loadInitialInfo()
        {
            view.bindStationsToSource(stationDB.getStations(), StationUtils.GetInstance().CbDisplay, StationUtils.GetInstance().CbValue);
            view.loadSides(SideUtils.GetInstance().CbItems);
            view.setNumSide((Side)Math.Abs((int)(view.selectedStation).Outer_side - 1));
        }

        // отчеты
        public void makeReportPosition(List<RepPos> list, string firstColName)
        {
            Excel.Application ExcelApp = new Excel.Application();
            //Книга.
            Excel.Workbook WBE = ExcelApp.Workbooks.Add(Type.Missing);
            Excel._Worksheet WSE1 = (Excel.Worksheet)WBE.Sheets[1];
            WBE.Sheets.Add(After:WBE.Sheets[1]); 
            Excel._Worksheet WSE2 = (Excel.Worksheet)WBE.Sheets[2];

            try
            {
                int row = 3;
                // отбираем строки по путям, считая кол-во вагонов на кажд. пути
                var distinctWaysOrStations = list.GroupBy(l => l.WayOrStatId).Select(c1 =>
                    new { id = c1.First().WayOrStatId, name = c1.First().WayOrStatName, amount = c1.Sum(l => l.Amount) }).ToList();
                // заголовок
                WSE1.Cells[1, 1].Value = "Положение по станции " + view.selectedStation.Name + " на " + DateTime.Now.ToString();
                // ----------
                // отображение по каждому пути груженых
                foreach (var item in distinctWaysOrStations)
                {
                    row = 3;
                    // show way num
                    if (distinctWaysOrStations.IndexOf(item) == 0)
                    {
                        WSE2.Cells[row, distinctWaysOrStations.IndexOf(item) + 1].Value = firstColName;//"№ пути";
                        // -- f o r m a t t i n g --
                        WSE2.Cells[row, distinctWaysOrStations.IndexOf(item) + 1].Font.Bold = true;
                        WSE2.Cells[row, distinctWaysOrStations.IndexOf(item) + 1].Interior.Color =
                System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(165, 165, 165));
                        // -------------------------
                    }
                    WSE2.Cells[row, distinctWaysOrStations.IndexOf(item) + 2].Value = item.name;
                    // -- f o r m a t t i n g --
                    WSE2.Cells[row, distinctWaysOrStations.IndexOf(item) + 2].Font.Bold = true;
                    WSE2.Cells[row, distinctWaysOrStations.IndexOf(item) + 2].Interior.Color =
                System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(165, 165, 165));
                    // -------------------------
                    row++;
                    // show total way amount
                    if (distinctWaysOrStations.IndexOf(item) == 0)
                    {
                        WSE2.Cells[row, 1].Value = "Кол-во ваг.";
                        // -- f o r m a t t i n g --
                        WSE2.Cells[row, 1].Font.Bold = true;
                        WSE2.Cells[row, 1].Interior.Color =
                    System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                        // -------------------------
                    }
                    WSE2.Cells[row, distinctWaysOrStations.IndexOf(item) + 2].Value = item.amount;
                    // -- f o r m a t t i n g --
                    WSE2.Cells[row, distinctWaysOrStations.IndexOf(item) + 2].Font.Bold = true;
                    WSE2.Cells[row, distinctWaysOrStations.IndexOf(item) + 2].Interior.Color =
                System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                    // -------------------------
                    row++;
                    // show loaded amount
                    if (distinctWaysOrStations.IndexOf(item) == 0)
                    {
                        WSE2.Cells[row, 1].Value = "в т.ч. груженых";
                        // -- f o r m a t t i n g --
                        WSE2.Cells[row, 1].Font.Bold = true;
                        // -------------------------
                    }
                    WSE2.Cells[row, distinctWaysOrStations.IndexOf(item) + 2].Value = list.Where(l => l.IsLoaded == 1 && l.WayOrStatId == item.id).Sum(l => l.Amount);
                    // -- f o r m a t t i n g --
                    WSE2.Cells[row, distinctWaysOrStations.IndexOf(item) + 2].Font.Bold = true;
                    // -------------------------
                    row++;
                    // отображение по каждому собственнику
                    // show loaded details
                    List<RepPos> listLoaded = (from v in list where v.WayOrStatId == item.id && v.IsLoaded == 1 select v).ToList();
                    foreach (RepPos repPos in listLoaded)
                    {
                        WSE2.Cells[row, distinctWaysOrStations.IndexOf(item) + 2].Value = repPos.DetailsName + ": " + repPos.Amount.ToString();
                        row++;
                    }
                }

                // "in total" column
                WSE2.Cells[3, distinctWaysOrStations.Count + 2].Value = "Итого";
                WSE2.Cells[3, distinctWaysOrStations.Count + 2].Font.Bold = true;
                WSE2.Cells[3, distinctWaysOrStations.Count + 2].Interior.Color =
                    System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(165, 165, 165));
                String Col = Convert.ToChar(65 + ((distinctWaysOrStations.Count + 1 - 1) % 26)).ToString();
                if (distinctWaysOrStations.Count > 26) Col = "A" + Col;
                WSE2.Cells[4, distinctWaysOrStations.Count + 2].Formula = "=SUM(B4:" + Col + "4)";
                WSE2.Cells[5, distinctWaysOrStations.Count + 2].Formula = "=SUM(B5:" + Col + "5)";

                // Unloaded
                int afterLoadedRow = WSE2.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing).Row + 1;
                foreach (var wayOrStat in distinctWaysOrStations)
                {
                    row = afterLoadedRow;
                    // show unloaded amount
                    if (distinctWaysOrStations.IndexOf(wayOrStat) == 0)
                    {
                        WSE2.Cells[row, 1].Value = "в т.ч. порожних";
                        // -- f o r m a t t i n g --
                        WSE2.Cells[row, 1].Font.Bold = true;
                        // -------------------------
                    }
                    WSE2.Cells[row, distinctWaysOrStations.IndexOf(wayOrStat) + 2].Value = list.Where(l => l.IsLoaded == 0 && l.WayOrStatId == wayOrStat.id).Sum(l => l.Amount);
                    // -- f o r m a t t i n g --
                    WSE2.Cells[row, distinctWaysOrStations.IndexOf(wayOrStat) + 2].Font.Bold = true;
                    // -------------------------
                    row++;
                    // show unloaded details
                    List<RepPos> listUnloaded = (from v in list where v.WayOrStatId == wayOrStat.id && v.IsLoaded == 0 select v).ToList();
                    foreach (RepPos item in listUnloaded)
                    {
                        WSE2.Cells[row, distinctWaysOrStations.IndexOf(wayOrStat) + 2].Value = item.DetailsName + ": " + item.Amount.ToString();
                        row++;
                    }
                }
                // "in total" column
                WSE2.Cells[afterLoadedRow, distinctWaysOrStations.Count + 2].Formula = "=SUM(B" + afterLoadedRow.ToString() + ":" + Col + "" + afterLoadedRow.ToString() + ")";

                // -- f o r m a t t i n g --
                row = WSE2.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing).Row;
                Excel.Range range = WSE2.get_Range((Excel.Range)WSE2.Cells[4, distinctWaysOrStations.Count + 2], (Excel.Range)WSE2.Cells[row, distinctWaysOrStations.Count + 2]);
                range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                range.Font.Bold = true;

                range = WSE2.get_Range((Excel.Range)WSE2.Cells[3, 1], (Excel.Range)WSE2.Cells[row, distinctWaysOrStations.Count + 2]);
                range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                range.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black); //FromArgb(165, 165, 165));
                // транспонировать
                range.Copy();
                Excel.Range destRange = WSE1.get_Range((Excel.Range)WSE1.Cells[3, 1], (Excel.Range)WSE1.Cells[row, distinctWaysOrStations.Count + 2]);                
                destRange.PasteSpecial(Excel.XlPasteType.xlPasteAll, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, true);
                // ---
                WSE1.get_Range((Excel.Range)WSE1.Cells[3, 1], (Excel.Range)WSE1.Cells[range.Columns.Count, range.Rows.Count]).Columns.AutoFit();
                ExcelApp.DisplayAlerts = false;
                WBE.Sheets[2].Delete();
                ExcelApp.DisplayAlerts = true;
            }
            catch (Exception ex)
            {
                view.showErrorMessage(ex.Message);
                WBE.Close(false, Type.Missing, Type.Missing);  // ---- закрыть Excel не сохраняя
            }

            ExcelApp.Visible = true;
            GC.Collect();
        }

        public void makeRepProst(List<RepProst> list, bool is_stat)
        {
            Excel.Application ExcelApp = new Excel.Application();
            //Книга.
            Excel.Workbook WBE = ExcelApp.Workbooks.Add(Type.Missing);
            Excel._Worksheet WSE = (Excel.Worksheet)WBE.Sheets[1];

            try
            {
                int row = 4;
                int col = 4;
                // заголовок
                if (is_stat)
                    WSE.Cells[1, 1].Value = "Простои по станции " + view.selectedStation.Name + " на " + DateTime.Now.ToString();
                else WSE.Cells[1, 1].Value = "Простои по АМКР на " + DateTime.Now.ToString();

                WSE.Cells[3, 1].Value = "№ вагона";
                WSE.Cells[3, 2].Value = "Собственник";
                WSE.Cells[3, 3].Value = "Тип";
                if (!is_stat)
                {
                    WSE.Cells[3, 4].Value = "Станция";
                    col = 5;
                }
                WSE.Cells[3, col++].Value = "Путь";
                WSE.Cells[3, col++].Value = "Род груза";
                WSE.Cells[3, col++].Value = "Годность";
                WSE.Cells[3, col++].Value = "Состояние";
                WSE.Cells[3, col++].Value = "Д/В входа на АМКР";
                WSE.Cells[3, col++].Value = "Простой на АМКР (ч)";
                WSE.Cells[3, col++].Value = "Д/В входа на станцию";
                WSE.Cells[3, col].Value = "Простой на станции (ч)";

                // -- f o r m a t t i n g --
                Excel.Range range = WSE.get_Range((Excel.Range)WSE.Cells[3, "A"], (Excel.Range)WSE.Cells[3, col]);
                range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                //range.Interior.Color = System.Drawing.Color.LightSkyBlue;
                range.Font.Bold = true;
                // ----------

                foreach (RepProst item in list)
                {
                    WSE.Cells[row, 1].Value = item.NumVag;
                    WSE.Cells[row, 2].Value = item.owner;
                    WSE.Cells[row, 3].Value = item.TypeVag;
                    if (!is_stat)
                    {
                        WSE.Cells[row, 4].Value = item.Station;
                        col = 5;
                    }
                    else col = 4;
                    WSE.Cells[row, col++].Value = item.NumWay;
                    WSE.Cells[row, col++].Value = item.gruz;
                    WSE.Cells[row, col++].Value = item.godn;
                    WSE.Cells[row, col++].Value = item.cond;
                    WSE.Cells[row, col++].Value = item.dt_amkr;
                    WSE.Cells[row, col++].Value = item.Hour_Amkr;
                    repProstColor(item.Hour_Amkr, WSE.get_Range((Excel.Range)WSE.Cells[row, col-1], (Excel.Range)WSE.Cells[row, col - 1]));
                    WSE.Cells[row, col++].Value = item.dt_on_stat;
                    WSE.Cells[row, col].Value = item.Hour_on_stat;
                    repProstColor(item.Hour_on_stat, WSE.get_Range((Excel.Range)WSE.Cells[row, col], (Excel.Range)WSE.Cells[row, col]));
                    row++;
                }

                // -- f o r m a t t i n g --
                range = WSE.get_Range((Excel.Range)WSE.Cells[3, "A"], (Excel.Range)WSE.Cells[row - 1, col]);
                range.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                range.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
                range.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;
                range.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
                range.Columns.AutoFit();
            }
            catch (Exception ex)
            {
                view.showErrorMessage(ex.Message);
                WBE.Close(false, Type.Missing, Type.Missing);  // ---- закрыть Excel не сохраняя
            }

            ExcelApp.Visible = true;
            GC.Collect();
        }

        private void repProstColor(int hours, Excel.Range range)
        {
            if (hours >= 72)
                range.Interior.Color = Color.FromArgb(255, 102, 102);
            else if (hours >= 48)
                range.Interior.Color = Color.FromArgb(255, 178, 102);
            else if (hours >= 24)
                range.Interior.Color = Color.FromArgb(255, 255, 102);
        }

        public void makeRepVagHistory()
        {
            if (view.getDialogVagHistResult())
            {
                List<RepVagHist> list = new ReportsDB().getVagHist(view.repVagHistNumVag);
                if (list.Count == 0)
                {
                    view.showErrorMessage("Нет данных по запрашиваемому вагону.");
                    return;
                }
                Excel.Application ExcelApp = new Excel.Application();
                //Книга.
                Excel.Workbook WBE = ExcelApp.Workbooks.Add(Type.Missing);
                Excel._Worksheet WSE = (Excel.Worksheet)WBE.Sheets[1];

                try
                {
                    int row = 3;
                    // отбираем строки по станциям, считая общее время на кажд.
                    var distinctStations = list.GroupBy(l => new { l.Station_name, l.DtOnStat}).Select(c1 =>
                        new { station_name = c1.First().Station_name, hours = c1.Sum(l => l.HoursAtPoint), minutes = c1.Sum(l => l.MinAtPoint), dtOnStat = c1.First().DtOnStat, dtFromStat = c1.Last().DtFromStat }).ToList();
                    // заголовок
                    WSE.Cells[1, 1].Value = "Длительность простоя вагона №" + view.repVagHistNumVag + " по путям и станциям предприятия";
                    // ----------
                    WSE.Cells[row, 1].Value = "Номер вагона";
                    // -- f o r m a t t i n g --
                    WSE.get_Range((Excel.Range)WSE.Cells[row, 1], (Excel.Range)WSE.Cells[row + 2, 1]).Merge();
                    // --
                    WSE.Cells[row + 3, 1].Value = view.repVagHistNumVag;
                    int col = 2;
                    foreach (var item in distinctStations)
                    {
                        var waysOfStation = from v in list where v.Station_name == item.station_name && v.DtOnStat == item.dtOnStat select v;

                        WSE.Cells[row, col].Value = item.station_name;
                        // -- f o r m a t t i n g --
                        WSE.get_Range((Excel.Range)WSE.Cells[row, col], (Excel.Range)WSE.Cells[row, col+2 + waysOfStation.Count()]).Merge();
                        // --
                        WSE.Cells[row + 1, col+2].Value = "№ пути";
                        // -- f o r m a t t i n g --
                        WSE.get_Range((Excel.Range)WSE.Cells[row + 1, col+2], (Excel.Range)WSE.Cells[row + 1, col+2 + waysOfStation.Count() - 1]).Merge();
                        // --

                        WSE.Cells[row + 2, col].Value = "Дата приб.на станц.";
                        // -- f o r m a t t i n g --
                        WSE.get_Range((Excel.Range)WSE.Cells[row + 1, col], (Excel.Range)WSE.Cells[row + 2, col]).Merge();
                        // --
                        WSE.Cells[row + 3, col++].Value = item.dtOnStat;

                        WSE.Cells[row + 2, col].Value = "Дата отпр.со станц.";
                        // -- f o r m a t t i n g --
                        WSE.get_Range((Excel.Range)WSE.Cells[row + 1, col], (Excel.Range)WSE.Cells[row + 2, col]).Merge();
                        // --
                        WSE.Cells[row + 3, col++].Value = item.dtFromStat;

                        foreach (var subItem in waysOfStation)
                        {
                            WSE.Cells[row + 2, col].Value = subItem.Point;
                            WSE.Cells[row + 3, col++].Value = subItem.HoursAtPoint.ToString() + " ч " + subItem.MinAtPoint.ToString() + " мин";
                        }

                        WSE.Cells[row + 1, col].Value = "ИТОГО";
                        // -- f o r m a t t i n g --
                        WSE.get_Range((Excel.Range)WSE.Cells[row + 1, col], (Excel.Range)WSE.Cells[row + 2, col]).Merge();
                        WSE.get_Range((Excel.Range)WSE.Cells[row + 1, col], (Excel.Range)WSE.Cells[row + 3, col]).Font.Bold = true;
                        // --
                        WSE.Cells[row + 3, col++].Value = item.hours.ToString() + " ч " + item.minutes.ToString() + " мин";
                    }
                    // -- f o r m a t t i n g --
                    Excel.Range range = WSE.get_Range((Excel.Range)WSE.Cells[row, 1], (Excel.Range)WSE.Cells[row + 3, col - 1]);
                    range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    range.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    //range.WrapText = true;
                    range.Columns.AutoFit();
                    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                    // --
                }
                catch (Exception ex)
                {
                    view.showErrorMessage(ex.Message);
                    WBE.Close(false, Type.Missing, Type.Missing);  // ---- закрыть Excel не сохраняя
                }

                ExcelApp.Visible = true;
                GC.Collect();
            }
        }

        public void makeRepLoadWayStat()
        {
            if (view.getDialogPeriod())
            {
                List<RepWaysLoaded> list = new ReportsDB().getWaysLoaded(view.repPeriodBd, view.repPeriodEd);
                if (list.Count == 0)
                {
                    view.showErrorMessage("Нет данных за выбранный период.");
                    return;
                }
                Excel.Application ExcelApp = new Excel.Application();
                //Книга.
                Excel.Workbook WBE = ExcelApp.Workbooks.Add(Type.Missing);
                Excel._Worksheet WSE = (Excel.Worksheet)WBE.Sheets[1];

                try
                {
                    int row = 3;
                    // отбираем строки по станциям, считая общее время на кажд.
                    var distinctStations = list.GroupBy(l => l.Station_name).Select(c1 =>
                        new { station_name = c1.First().Station_name, vag_amount = c1.Sum(l => l.Vag_amount) }).ToList();
                    // заголовок
                    WSE.Cells[1, 1].Value = "Загруженность путей и станций предприятия в период с " + view.repPeriodBd.ToString("g") + " по " + view.repPeriodEd.ToString("g") + ", вагонов";
                    // ----------
                    int col = 1;
                    foreach (var item in distinctStations)
                    {
                        var waysOfStation = from v in list where v.Station_name == item.station_name select v;

                        WSE.Cells[row, col].Value = item.station_name;
                        // -- f o r m a t t i n g --
                        WSE.get_Range((Excel.Range)WSE.Cells[row, col], (Excel.Range)WSE.Cells[row, col + waysOfStation.Count()]).Merge();
                        // --
                        WSE.Cells[row + 1, col].Value = "№ пути";
                        // -- f o r m a t t i n g --
                        WSE.get_Range((Excel.Range)WSE.Cells[row + 1, col], (Excel.Range)WSE.Cells[row + 1, col + waysOfStation.Count() - 1]).Merge();
                        // --

                        foreach (var subItem in waysOfStation)
                        {
                            WSE.Cells[row + 2, col].Value = subItem.Way_num;
                            WSE.Cells[row + 3, col++].Value = subItem.Vag_amount.ToString();
                        }

                        WSE.Cells[row + 1, col].Value = "ИТОГО";
                        // -- f o r m a t t i n g --
                        WSE.get_Range((Excel.Range)WSE.Cells[row + 1, col], (Excel.Range)WSE.Cells[row + 2, col]).Merge();
                        WSE.get_Range((Excel.Range)WSE.Cells[row + 1, col], (Excel.Range)WSE.Cells[row + 3, col]).Font.Bold = true;
                        // --
                        WSE.Cells[row + 3, col++].Value = item.vag_amount.ToString();
                    }
                    // -- f o r m a t t i n g --
                    Excel.Range range = WSE.get_Range((Excel.Range)WSE.Cells[row, 1], (Excel.Range)WSE.Cells[row + 3, col - 1]);
                    range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    range.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    //range.WrapText = true;
                    //range.Columns.AutoFit();
                    range.ColumnWidth = 10;
                    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                    // --
                }
                catch (Exception ex)
                {
                    view.showErrorMessage(ex.Message);
                    WBE.Close(false, Type.Missing, Type.Missing);  // ---- закрыть Excel не сохраняя
                }

                ExcelApp.Visible = true;
                GC.Collect();
            }
        }

        public void makeRepGfUnlTurnover()
        {
            if (view.getDialogPeriod())
            {
                List<RepGfUnlTurnover> list = new ReportsDB().getGfUnloadTurnover(view.repPeriodBd, view.repPeriodEd);
                if (list.Count == 0)
                {
                    view.showErrorMessage("Нет данных за выбранный период.");
                    return;
                }
                Excel.Application ExcelApp = new Excel.Application();
                //Книга.
                Excel.Workbook WBE = ExcelApp.Workbooks.Add(Type.Missing);
                Excel._Worksheet WSE = (Excel.Worksheet)WBE.Sheets[1];

                try
                {
                    int row = 3;
                    int subRow = 0;
                    // отбираем строки по станциям, считая общее время на кажд.
                    var distinctStations = list.GroupBy(l => l.Station_name).Select(c1 =>
                        new { station_name = c1.First().Station_name }).ToList();
                    // заголовок
                    WSE.Cells[1, 1].Value = "Перерабатывающая способность грузовых фронтов с " + view.repPeriodBd.ToString("g") + " по " + view.repPeriodEd.ToString("g") + ", вагонов";
                    // ----------
                    int col = 1;
                    foreach (var item in distinctStations)
                    {
                        var unlOfStation = from v in list where v.Station_name == item.station_name select v;

                        WSE.Cells[row, col].Value = item.station_name;

                        var distinctGruzs = unlOfStation.GroupBy(l => l.Gruz_name).Select(c1 => c1.First().Gruz_name).ToList();
                        // -- f o r m a t t i n g --
                        WSE.get_Range((Excel.Range)WSE.Cells[row, col], (Excel.Range)WSE.Cells[row, col + distinctGruzs.Count() * 3]).Merge();
                        // --                        
                        var distinctGF = unlOfStation.GroupBy(l => l.Gf_sh).Select(c1 => c1.First().Gf_sh).ToList();
                        col++;
                        foreach (var gruz in distinctGruzs)
                        {
                            WSE.Cells[row + 1, col].Value = "Выгрузка " + gruz;
                            WSE.Cells[row + 1, col + 2].Value = "Отклонение";
                            // -- f o r m a t t i n g --
                            WSE.get_Range((Excel.Range)WSE.Cells[row + 1, col], (Excel.Range)WSE.Cells[row + 1, col + 1]).Merge();
                            WSE.get_Range((Excel.Range)WSE.Cells[row + 1, col + 2], (Excel.Range)WSE.Cells[row + 2, col + 2]).Merge();
                            WSE.Columns[col + 2].ColumnWidth = 12;
                            // --
                            WSE.Cells[row + 2, col].Value = "План";
                            WSE.Cells[row + 2, col + 1].Value = "Факт";
                            col += 3;
                            subRow = row + 3;
                            foreach (var gf in distinctGF)
                            {
                                if (distinctGruzs.IndexOf(gruz) == 0)
                                {
                                    WSE.Cells[subRow, 1].Value = gf;
                                }
                                try
                                {
                                    WSE.Cells[subRow, col - 2].Value = (from v in unlOfStation where v.Gf_sh == gf && v.Gruz_name == gruz select v.Vag_amount).FirstOrDefault();
                                }
                                catch (Exception) { }
                                String ColFact = Convert.ToChar(65 + ((col - 2 - 1) % 26)).ToString();
                                if (col - 2 > 27) ColFact = "A" + ColFact;
                                String ColPlan = Convert.ToChar(65 + ((col - 3 - 1) % 26)).ToString();
                                if (col - 3 > 27) ColPlan = "A" + ColPlan;
                                WSE.Cells[subRow, col - 1].Formula = "=" + ColFact + "" + subRow + "-" + ColPlan + "" + subRow;
                                subRow++;
                            }
                        }


                        //row = row+3;
                        //foreach (var gfItem in distinctGF)
                        //{
                        //    WSE.Cells[row++, col].Value = gfItem.gf_sh;
                        //    //var unlOfGf = from v in distinctGF where v.gf_sh == gfItem.gf_sh.Gf_sh select v;
                        //}
                        //foreach (var subItem in unlOfStation)
                        //{
                        //    WSE.Cells[row + 2, col].Value = subItem.Way_num;
                        //    WSE.Cells[row + 3, col++].Value = subItem.Vag_amount.ToString();
                        //}

                        //WSE.Cells[row + 1, col].Value = "ИТОГО";
                        //// -- f o r m a t t i n g --
                        //WSE.get_Range((Excel.Range)WSE.Cells[row + 1, col], (Excel.Range)WSE.Cells[row + 2, col]).Merge();
                        //WSE.get_Range((Excel.Range)WSE.Cells[row + 1, col], (Excel.Range)WSE.Cells[row + 3, col]).Font.Bold = true;
                        //// --
                        //WSE.Cells[row + 3, col++].Value = item.vag_amount.ToString();
                    }
                    // -- f o r m a t t i n g --
                    Excel.Range range = WSE.get_Range((Excel.Range)WSE.Cells[3, 1], (Excel.Range)WSE.Cells[subRow - 1, col - 1]);
                    range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    range.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    WSE.Columns[1].ColumnWidth = 20;
                    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                }
                catch (Exception ex)
                {
                    view.showErrorMessage(ex.Message);
                    WBE.Close(false, Type.Missing, Type.Missing);  // ---- закрыть Excel не сохраняя
                }

                ExcelApp.Visible = true;
                GC.Collect();
            }
        }

        public void makeRepVagOnCleanWays()
        {
            if (view.getDialogPeriod())
            {
                List<RepVagOnCleanWays> list = new ReportsDB().getVagOnCleaWays(view.repPeriodBd, view.repPeriodEd, view.selectedStation);
                if (list.Count == 0)
                {
                    view.showErrorMessage("Нет данных за выбранный период.");
                    return;
                }
                Excel.Application ExcelApp = new Excel.Application();
                //Книга.
                Excel.Workbook WBE = ExcelApp.Workbooks.Add(Type.Missing);
                Excel._Worksheet WSE = (Excel.Worksheet)WBE.Sheets[1];

                try
                {
                    int row = 5;
                    // отбираем строки по путям, считая общее кол-во вагонов на кажд.
                    var distinctWays = list.GroupBy(l => l.NumWay).Select(c1 =>
                        new { numWay = c1.First().NumWay, vagAmount = c1.Count() }).ToList();
                    // заголовок
                    WSE.Cells[1, 1].Value = "Вагоны на путях очистки станции "+view.selectedStation.Name+" в период с " + view.repPeriodBd.ToString("g") + " по " + view.repPeriodEd.ToString("g");
                    // ----------
                    WSE.Cells[3, 1].Value = "№ вагона";
                    int col = 2;
                    List<int> distinctVagons = list.GroupBy(l => l.NumVag).Select(c1 => c1.First().NumVag).ToList();

                    foreach (var item in distinctWays)
                    {
                        row = 5;

                        WSE.Cells[3, col].Value = item.numWay;
                        // -- f o r m a t t i n g --
                        WSE.get_Range((Excel.Range)WSE.Cells[3, 1], (Excel.Range)WSE.Cells[4, 1]).Merge();
                        WSE.get_Range((Excel.Range)WSE.Cells[3, col], (Excel.Range)WSE.Cells[3, col + 1]).Merge();
                        // --
                        WSE.Cells[4, col].Value = "Дата постановки";
                        WSE.Cells[4, col + 1].Value = "Дата снятия";

                        foreach (int vag in distinctVagons)
                        {
                            if (distinctWays.IndexOf(item) == 0)
                            {
                                WSE.Cells[row, 1].Value = vag;
                            }
                            WSE.Cells[row, col].Value = (from v in list where v.NumWay == item.numWay && v.NumVag ==  vag select v.DtOnWay.ToString("g")).FirstOrDefault();
                            string dtFrom =  (from v in list where v.NumWay == item.numWay && v.NumVag == vag select v.DtFromWay.ToString("g")).FirstOrDefault();
                            if (dtFrom != "01.01.1900 0:00") WSE.Cells[row, col + 1].Value = dtFrom;
                            row++;
                        }
                        col+=2;
                    }
                    // -- f o r m a t t i n g --
                    Excel.Range range = WSE.get_Range((Excel.Range)WSE.Cells[3, 1], (Excel.Range)WSE.Cells[row - 1, col - 1]);
                    range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    range.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    range.WrapText = true;
                    //range.Columns.AutoFit();
                    range.ColumnWidth = 13;
                    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                    // --
                }
                catch (Exception ex)
                {
                    view.showErrorMessage(ex.Message);
                    WBE.Close(false, Type.Missing, Type.Missing);  // ---- закрыть Excel не сохраняя
                }

                ExcelApp.Visible = true;
                GC.Collect();
            }
        }
    }
}
