using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Deployment.Application;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
//using Railway.Properties;
//using log4net;
//using log4net.Config;
using RailwayCL;
using EFRailCars.Helpers;
using System.Threading;
using ServicesStatus;
using ServicesStatus.Concrete;

namespace RailwayUI
{
    public partial class MainForm : Form, IMainView, IVagOnStatView, IVagManeuverView, IVagSendOthStView, IVagWaitAdmissView, IVagWaitRemoveAdmissView
    {
        MainPresenter mainPresenter;
        VagOnStatPresenter vagOnStatPresenter;
        VagManeuverPresenter vagManeuverPresenter;
        VagSendOthStPresenter vagSendOtherStPresenter;
        VagWaitAdmissPresenter vagWaitAdmissPresenter;
        VagWaitRemoveAdmissPresenter vagWaitRemoveAdmissPresenter;

        VagOnStatUtils vagOnStatUtils = new VagOnStatUtils();
        VagOperationsUtils vagOperationsUtils = new VagOperationsUtils();

        VagAcceptForm vagAcceptForm;

        fTransit transitForm;
        fRepVagHist repVagHist;
        fRepPeriod repPeriod;

        VagOperationsDB vagOperationsDB = new VagOperationsDB();
        ReportsDB reportsDB = new ReportsDB();

        int editingValue = -1;
        int wayIdx = 0;
        bool selectAllBtnClicked = false;
        bool findVagByNumPerforming = false; 

        //private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
        // IMainView

        int IMainView.wayIdxToSelect
        {
            get { return wayIdx; }
            set { wayIdx = value; }
        }

        Side IMainView.numSide
        {
        get { return (Side)cbSide.SelectedIndex; }
        }

        Station IMainView.selectedStation
        {
            get { return (Station)cbStat.SelectedItem; }
        }

        bool IMainView.selectAllBtnClicked
        {
            get { return selectAllBtnClicked; }
            set { selectAllBtnClicked = value; }
        }

        int IMainView.numVagForSearch
        {
            get 
            { 
                int nV = 0;
                Int32.TryParse(tbVagSearch.Text, out nV);
                return nV;
            }
        }

        bool IMainView.findVagByNumPerforming
        {
            get { return this.findVagByNumPerforming; }
            set { this.findVagByNumPerforming = value; }
        }

        void IMainView.bindStationsToSource(List<Station> list, string cbDisplay, string cbValue)
        {
            cbStat.Text = "";
            cbStat.DataSource = list;
            cbStat.DisplayMember = cbDisplay;
            cbStat.ValueMember = cbValue;
            cbStat.SelectedIndex = 0;
        }

        void IMainView.setNumSide(Side side)
        {
            cbSide.SelectedIndex = (int)side;
        }

        void IMainView.setFieldWithSelVagAmount(string text)
        {
            sbVagAm.Text = "Кол-во выделенных вагонов: "+ text;
        }

        //void IMainView.setSideDueToOuterSide(Side side)
        //{
        //    cbSide.SelectedIndex = Math.Abs((int)side - 1);
        //}

        void IMainView.setRospuskIndicator(bool value)
        {
            chRospusk.Checked = value;
        }

        void IMainView.setTabIdx(int idx)
        {
            tabControl1.SelectedIndex = idx;
        }

        void IMainView.loadSides(object[] range)
        {
            cbSide.Items.AddRange(range);
        }

        void IMainView.loadData()
        {
            this.Cursor = Cursors.WaitCursor;

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    {
                        vagOnStatPresenter.loadVagOnStatTab();
                        break;
                    }
                case 1: // маневры
                    {
                        vagManeuverPresenter.loadVagManeuverTab();
                        break;
                    }
                case 2: // др. станц, ГФ, цеха
                    {
                        vagSendOtherStPresenter.loadVagSendOthStTab();
                        break;
                    }
                case 3:
                    {
                        vagWaitAdmissPresenter.loadVagWaitAdmissTab();
                        break;
                    }
                case 4:
                    {
                        vagWaitRemoveAdmissPresenter.loadVagWaitRemoveAdmissTab();
                        break;
                    }
            }
            this.Cursor = Cursors.Default;
        }

        void IMainView.searchVag()
        {
            this.Cursor = Cursors.WaitCursor;

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    {
                        vagOnStatPresenter.searchVag();
                        break;
                    }
                case 1: // маневры
                    {
                        vagManeuverPresenter.searchVag();
                        break;
                    }
                case 2: // др. станц, ГФ, цеха
                    {
                        vagSendOtherStPresenter.searchVag();
                        break;
                    }
                case 3:
                    {
                        vagWaitAdmissPresenter.searchVag();
                        break;
                    }
                case 4:
                    {
                        vagWaitRemoveAdmissPresenter.searchVag();
                        break;
                    }
            }
            this.Cursor = Cursors.Default;
        }

        void IMainView.showInfoMessage(string message)
        {
            MessageBox.Show(message, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void IMainView.showWarningMessage(string message)
        {
            MessageBox.Show(message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void IMainView.showErrorMessage(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        bool IMainView.showQuestMessage(string message)
        {
            DialogResult dr = MessageBox.Show(message, "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes) return true;
            else return false;
        }

        int IMainView.repVagHistNumVag
        {
            get { return repVagHist.Num_Vag; }
        }

        bool IMainView.getDialogVagHistResult()
        {
            repVagHist = new fRepVagHist();
            if (repVagHist.ShowDialog() == System.Windows.Forms.DialogResult.OK) return true;
            else return false;
        }

        DateTime IMainView.repPeriodBd 
        {
            get { return repPeriod.Bd; }
        }

        DateTime IMainView.repPeriodEd 
        {
            get { return repPeriod.Ed; } 
        }

        bool IMainView.getDialogPeriod()
        {
            repPeriod = new fRepPeriod();
            if (repPeriod.ShowDialog() == System.Windows.Forms.DialogResult.OK) return true;
            else return false;
        }

        // IVagOnStatView

        Way IVagOnStatView.selectedWay
        {
            get { return (Way)bsWayP1.List[dgvWaysP1.SelectedRows[0].Index]; }
        }

        GruzFront IVagOnStatView.selectedGf
        {
            get { return (GruzFront)bsGruzFronts.List[dgvGruzFronts.SelectedRows[0].Index]; }
        }

        Shop IVagOnStatView.selectedShop
        {
            get { return (Shop)bsShops.List[dgvShops.SelectedRows[0].Index]; }
        }

        int IVagOnStatView.dgvColumnsCount 
        {
            get { return dgvVagP1.Columns.Count; }
        }

        //string IVagOnStatView.firstVagCondName
        //{
        //    get { try { return ((VagOnStat)bs1P1.List[0]).Cond.Name; } catch (ArgumentException) { return ""; } }
        //}
        List<VagOnStat> IVagOnStatView.listVagons
        {
            get { return (List<VagOnStat>)bs1P1.List; }
        }

        int IVagOnStatView.shopsCount
        {
            get { return bsShops.List.Count; }
        }

        int IVagOnStatView.gfCount
        {
            get { return bsGruzFronts.List.Count; }
        }

        //int IVagOnStatView.selWayIdx
        //{
        //    get { return dgvWaysP1.SelectedRows[0].Index; }
        //}
        List<Way> IVagOnStatView.listWays
        {
            get { return (List<Way>)bsWayP1.List; }
        }

        void IVagOnStatView.makeDgvColumns()
        {
            vagOnStatUtils.makeVagDgvColumns(dgvVagP1);
        }

        void IVagOnStatView.changeColumnsPositions(bool isDepart)
        {
            vagOperationsUtils.changeColumnsPosition(dgvVagP1, isDepart);
        }

        void IVagOnStatView.setCurrentWay(int idxWay)
        {
            try
            {
                bsWayP1.Position = idxWay;
                dgvWays.Rows[wayIdx].Selected = true;
            }
            catch (ArgumentOutOfRangeException)
            { }
        }

        void IVagOnStatView.bindWaysToSource(List<Way> list)
        {
            bsWayP1.DataSource = list;
        }

        void IVagOnStatView.clearWays()
        {
            bsWayP1.Clear();
        }

        void IVagOnStatView.clearWaysSelection()
        {
            dgvWaysP1.ClearSelection();
        }

        void IVagOnStatView.bindVagOnStatToSource(List<VagOnStat> list)
        {
            bs1P1.DataSource = list;
        }

        void IVagOnStatView.bindShopsToSource(List<Shop> list)
        {
            bsShops.DataSource = list;
        }

        void IVagOnStatView.clearShopsSelection()
        {
            dgvShops.ClearSelection();
        }

        void IVagOnStatView.bindGfToSource(List<GruzFront> list)
        {
            bsGruzFronts.DataSource = list;
        }

        void IVagOnStatView.clearGfSelection()
        {
            dgvGruzFronts.ClearSelection();
        }

        void IVagOnStatView.showGfAndShopsOnForm(bool hasGF, bool hasShops)
        {
            int heightDgv = 0;
            foreach ( DataGridViewRow row in dgvWaysP1.Rows)
            {
                heightDgv = heightDgv + row.Height;
            }
            splitContainer9.SplitterDistance = heightDgv + dgvWaysP1.ColumnHeadersHeight + splitContainer9.SplitterWidth;
            //splitContainer9.SplitterDistance = splitContainer9.Height / 2 + splitContainer9.Height / 6;
            if (hasGF)
            {
                heightDgv = 0;
                foreach (DataGridViewRow row in dgvGruzFronts.Rows)
                {
                    heightDgv = heightDgv + row.Height;
                }
                splitContainer10.SplitterDistance = heightDgv + dgvGruzFronts.ColumnHeadersHeight+panel16.Height+splitContainer10.SplitterWidth;
            }
            else
            {
                splitContainer10.SplitterDistance = 6;
            }
            lGF.Visible = hasGF;
            dgvGruzFronts.Visible = hasGF;
            lShop.Visible = hasShops;
            dgvShops.Visible = hasShops;
        }

        void IVagOnStatView.hideGfAndShopsOnForm()
        {
            splitContainer9.SplitterDistance = splitContainer9.Height;
            lGF.Visible = false;
            lShop.Visible = false;
        }

        void IVagOnStatView.selectVagByIdx(int idx)
        {
            dgvVagP1.Rows[idx].Selected = true;
        }

        void IVagOnStatView.vagTableSetScrollToSelRow()
        {
            dgvVagP1.FirstDisplayedScrollingRowIndex = dgvVagP1.SelectedRows[0].Index;
        }


        // IVagManeuverView
        bool IVagManeuverView.enablePerform
        {
            get { return bPerform.Enabled; }
            set { bPerform.Enabled = value; }
        }

        bool IVagManeuverView.visiblePerform
        {
            get { return bPerform.Visible; }
            set
            {
                bPerform.Visible = value;
            }

        }
        VagManeuverUtils vagManeuverUtils = new VagManeuverUtils();

        int IVagManeuverView.dgvForManColumnsCount
        {
            get { return dgvForMan.Columns.Count; }
        }

        int IVagManeuverView.dgvOnManColumnsCount
        {
            get { return dgvOnMan.Columns.Count; }
        }

        bool IVagManeuverView.otherStatLocomotives
        {
            get { return chbOtherStLocom.Checked; }
        }

        Way IVagManeuverView.selectedWayFrom
        {
            get 
            {
                try
                {
                    return (Way)ManBsWayFrom.List[dgvWayFrom.SelectedRows[0].Index];
                }
                catch (ArgumentOutOfRangeException)
                {
                    return null;
                }
            }
        }

        Way IVagManeuverView.selectedWayTo
        {
            get 
            {
                try
                {
                    return (Way)ManBsWayTo.List[dgvWayTo.SelectedRows[0].Index];
                }
                catch (ArgumentOutOfRangeException)
                {
                    return null;
                }
            }
        }

        Side IVagManeuverView.selectedSide
        {
            get { return (Side)ManCbSide.SelectedIndex; }
        }

        List<VagManeuver> IVagManeuverView.listVagForMan
        {
            get { return (List<VagManeuver>)ManBs1.List; }
        }

        List<VagManeuver> IVagManeuverView.listVagOnMan
        {
            get { return (List<VagManeuver>)ManBs2.List; }
        }

        List<Way> IVagManeuverView.listWayFrom
        {
            get { return (List<Way>)ManBsWayFrom.List; }
        }

        List<Way> IVagManeuverView.listWayTo
        {
            get { return (List<Way>)ManBsWayTo.List; }
        }

        //string IVagManeuverView.firstVagCondName
        //{
        //    get { try { return ((VagManeuver)ManBs1.List[0]).Cond.Name; } catch (ArgumentOutOfRangeException) { return ""; } }
        //}

        //int IVagManeuverView.selWayFromIdx
        //{
        //    get { return dgvWayFrom.SelectedRows[0].Index; }
        //}

        int IVagManeuverView.selVagForManCount
        {
            get { return dgvForMan.SelectedRows.Count; }
        }

        int IVagManeuverView.selVagOnManCount
        {
            get { return dgvOnMan.SelectedRows.Count; }
        }

        int IVagManeuverView.idxCurVagForMan 
        {
            get { return ManBs1.IndexOf(ManBs1.Current); } 
        }

        //VagManeuver IVagManeuverView.getCurVagForMan
        //{
        //    get { return (VagManeuver)ManBs1.Current; }
        //}

        //VagManeuver IVagManeuverView.firstSelVagForMan
        //{
        //    get { return (VagManeuver)ManBs1.List[dgvForMan.SelectedRows[0].Index]; }
        //}

        int IVagManeuverView.idxFirstSelVagForMan 
        {
            get { return dgvForMan.SelectedRows[0].Index; } 
        }

        //int IVagManeuverView.idxVagOnMan(VagManeuver vagon)
        //{
        //    return ManBs2.IndexOf(vagon);
        //}

        //int IVagManeuverView.vagForManCount
        //{
        //    get { return dgvForMan.Rows.Count; }
        //}

        //int IVagManeuverView.vagOnManCount
        //{
        //    get { return dgvOnMan.Rows.Count; }
        //}

        //int IVagManeuverView.getRowOfTableWithVagForManByIdx(int idx)
        //{
        //    return dgvForMan.Rows[idx].Index; 
        //}

        bool IVagManeuverView.isVagForManSelected(int vagIdx)
        {
            return dgvForMan.Rows[vagIdx].Selected;
        }

        bool IVagManeuverView.isVagForManColored(int vagIdx)
        {
            return !dgvForMan.Rows[vagIdx].DefaultCellStyle.BackColor.IsEmpty;
        }

        //VagManeuver IVagManeuverView.lastVagOnMan
        //{
        //    get { return (VagManeuver)ManBs2.List[ManBs2.Count - 1]; }
        //}

        VagManeuver IVagManeuverView.firstSelVagOnMan
        {
            get { return (VagManeuver)ManBs2.List[dgvOnMan.SelectedRows[0].Index]; }
        }

        //VagManeuver IVagManeuverView.getVagForManByIdx(int idx)
        //{
        //    return (VagManeuver)ManBs1.List[idx];
        //}

        Locomotive IVagManeuverView.selectedLocom
        {
            get { return (Locomotive)cbLocom.SelectedItem; }
        }

        List<VagManeuver> IVagManeuverView.getRemainedVagForMan()
        {
            List<VagManeuver> list = new List<VagManeuver>();
            foreach (DataGridViewRow row in dgvForMan.Rows)
            {
                if (row.DefaultCellStyle.BackColor == Color.Empty)
                {
                    list.Add((VagManeuver)ManBs1.List[row.Index]);
                }
            }
            return list;
        }

        void IVagManeuverView.makeDgvForManColumns()
        {
            vagManeuverUtils.makeVagDgvColumns(dgvForMan);
        }

        void IVagManeuverView.makeDgvOnManColumns()
        {
            vagManeuverUtils.makeVagDgvColumns(dgvOnMan);
        }

        void IVagManeuverView.bindWaysFromToSource(List<Way> list)
        {
            ManBsWayFrom.DataSource = list;
        }

        void IVagManeuverView.clearWaysFrom()
        {
            ManBsWayFrom.Clear();
        }

        void IVagManeuverView.setCurrentWayFrom(int idxWay)
        {
            try
            {
                //if (findVagByNumPerforming)
                //{
                //    findVagByNumPerforming = false;
                //    dgvWayFrom.ClearSelection();
                //    findVagByNumPerforming = true;
                //}
                //else 
                dgvWayFrom.ClearSelection();
                ManBsWayFrom.Position = idxWay;
                dgvWayFrom.Rows[idxWay].Selected = true;
            }
            catch (ArgumentOutOfRangeException)
            { }
        }

        void IVagManeuverView.bindWaysOnToSource(List<Way> list)
        {
            ManBsWayTo.DataSource = list;
        }

        void IVagManeuverView.clearWaysOn()
        {
            ManBsWayTo.Clear();
        }

        void IVagManeuverView.clearWaysFromSelection()
        {
            dgvWayFrom.ClearSelection();
        }

        void IVagManeuverView.clearWaysOnSelection()
        {
            dgvWayTo.ClearSelection();
        }

        void IVagManeuverView.loadSides(object[] items, string nonSelected)
        {
            ManCbSide.Items.Clear();
            ManCbSide.Items.AddRange(items);
            ManCbSide.SelectedIndex = -1;
            ManCbSide.Text = nonSelected;
        }

        void IVagManeuverView.loadLocomotives(List<Locomotive> list, string cbDisplay, string cbValue, string nonSelected)
        {
            cbLocom.DataSource = list;
            cbLocom.DisplayMember = cbDisplay;
            cbLocom.ValueMember = cbValue;
            cbLocom.SelectedIndex = -1;
            cbLocom.Text = nonSelected;
        }

        void IVagManeuverView.changeLabelsSize()
        {
            lFromWay.Width = dgvWayFrom.Width + splitContainer1.SplitterWidth;
            lVagForMan.Width = dgvForMan.Width + splitContainer2.SplitterWidth;
            lOnWay.Width = dgvWayTo.Width;
        }

        void IVagManeuverView.bindVagForManToSource(List<VagManeuver> list)
        {
            ManBs1.DataSource = list;
        }

        void IVagManeuverView.clearVagForManSelection()
        {
            dgvForMan.ClearSelection();
        }

        void IVagManeuverView.changeColorVagSelectedForMan(List<int> rowsIdx)
        {
            for (int i = 0; i < dgvForMan.Rows.Count; i++)
            {
                if (rowsIdx.Contains(i))
                    dgvForMan.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
            }
        }

        void IVagManeuverView.bindVagOnManToSource(List<VagManeuver> list)
        {
            ManBs2.DataSource = list;
        }

        void IVagManeuverView.clearVagOnMan()
        {
            ManBs2.Clear();
        }

        void IVagManeuverView.setCurrentWayTo(int wayIdx)
        {
            dgvWayTo.Rows[wayIdx].Selected = true;
        }

        void IVagManeuverView.selectSide(Side side)
        {
            ManCbSide.SelectedIndex = (int)side;
        }

        int? IVagManeuverView.selectLocom(int idx)
        {
            cbLocom.SelectedValue = idx;
            return (int?)cbLocom.SelectedValue;
        }

        void IVagManeuverView.setOtherStatLocomotives(bool otherStat)
        {
            chbOtherStLocom.Checked = otherStat;
        }

        void IVagManeuverView.changeColumnsPositions(bool isDepart)
        {
            vagOperationsUtils.changeColumnsPosition(dgvForMan, isDepart);
            vagOperationsUtils.changeColumnsPosition(dgvOnMan, isDepart);
        }

        void IVagManeuverView.clearSide(string text)
        {
            ManCbSide.SelectedIndex = -1;
            ManCbSide.Text = text;
        }

        void IVagManeuverView.clearLocom(string text)
        {
            cbLocom.SelectedIndex = -1;
            cbLocom.Text = text;
        }

        void IVagManeuverView.addVagOnManFromVagsForMan(int vagForManIdx)
        {
            ManBs2.Add(ManBs1.List[dgvForMan.Rows[vagForManIdx].Index]);
        }

        void IVagManeuverView.setVagForManColor(int vagIdx, Color color)
        {
            dgvForMan.Rows[vagIdx].DefaultCellStyle.BackColor = color;
        }

        void IVagManeuverView.removeFromVagOnMan(VagManeuver vagon)
        {
            ManBs2.Remove(vagon);
        }

        void IVagManeuverView.clearColorAndDtFromWayMultipleVag()
        {
            foreach (DataGridViewRow row in dgvForMan.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.Empty;
                ((VagManeuver)ManBs1.List[row.Index]).dt_from_way = null;
            }
        }

        void IVagManeuverView.moveToLastVagForMan()
        {
            ManBs1.MoveLast();
        }

        void IVagManeuverView.moveToPrevVagForMan()
        {
            ManBs1.MovePrevious();
        }

        void IVagManeuverView.moveToFirstVagForMan()
        {
            ManBs1.MoveFirst();
        }

        void IVagManeuverView.moveToNextVagForMan()
        {
            ManBs1.MoveNext();
        }

        void IVagManeuverView.selectVagForManByIdx(int idx)
        {
            dgvForMan.Rows[idx].Selected = true;
        }

        void IVagManeuverView.refreshWaysTables()
        {
            dgvWayFrom.Refresh();
            dgvWayTo.Refresh();
        }

        void IVagManeuverView.vagTableSetScrollToSelRow()
        {
            dgvForMan.FirstDisplayedScrollingRowIndex = dgvForMan.SelectedRows[0].Index;
        }


        // IVagSendOthStView

        VagSendOthStUtils vagSendOthStUtils = new VagSendOthStUtils();

        int IVagSendOthStView.dgvForSendColumnsCount
        {
            get { return dgvForSending.Columns.Count; }
        }

        int IVagSendOthStView.dgvToSendColumnsCount
        {
            get { return dgvToSend.Columns.Count; }
        }

        bool IVagSendOthStView.otherStatLocomotives
        {
            get { return chSendLocomOthSt.Checked; }
        }

        Way IVagSendOthStView.selectedWay
        {
            get { return (Way)SendBsWays.List[dgvWays.SelectedRows[0].Index]; }
        }

        Side IVagSendOthStView.selectedSide
        {
            get { return (Side)SendCbSide.SelectedIndex; }
        }

        Station IVagSendOthStView.selectedStatAccept
        {
            get { try { return (Station)SendCbStat.SelectedItem; } catch { return null; } }
        }

        GruzFront IVagSendOthStView.selectedGF
        {
            get { try { return (GruzFront)cbGruzFront.SelectedItem; } catch { return null; } }
        }

        Shop IVagSendOthStView.selectedShop
        {
            get { try { return (Shop)cbShop.SelectedItem; } catch { return null; } }
        }

        Locomotive IVagSendOthStView.locom1
        {
            get { return (Locomotive)cbSendLocom1.SelectedItem; }
        }

        Locomotive IVagSendOthStView.locom2
        {
            get { return (Locomotive)cbSendLocom2.SelectedItem; }
        }

        List<VagSendOthSt> IVagSendOthStView.listForSending
        {
            get { return (List<VagSendOthSt>)SendBs1.List; }
        }

        List<VagSendOthSt> IVagSendOthStView.listToSend
        {
            get { return (List<VagSendOthSt>)SendBs2.List; }
        }

        //string IVagSendOthStView.firstVagCondName
        //{
        //    get { try { return ((VagSendOthSt)SendBs1.List[0]).Cond.Name; } catch (ArgumentOutOfRangeException) { return ""; } }
        //}

        //int IVagSendOthStView.selWayFromIdx 
        //{ 
        //    get { try { return dgvWays.SelectedRows[0].Index; } catch (ArgumentOutOfRangeException) { return 0; } }
        //}

        List<Way> IVagSendOthStView.listWays
        {
            get { return (List<Way>)SendBsWays.List; }
        }

        int IVagSendOthStView.trainNum
        {
            get { return Convert.ToInt32(lNumTrain.Text); }
        }

        void IVagSendOthStView.setStatAcceptByName(string name)
        {
            SendCbStat.SelectedIndex = SendCbStat.FindStringExact(name); 
        }

        int IVagSendOthStView.selVagForSendingCount
        {
            get { return dgvForSending.SelectedRows.Count; }
        }

        int IVagSendOthStView.selVagToSendCount
        {
            get { return dgvToSend.SelectedRows.Count; }
        }

        int IVagSendOthStView.idxCurVagForSend
        {
            get { return SendBs1.IndexOf(SendBs1.Current); }
        }

        int IVagSendOthStView.idxFirstSelVagForSend
        {
            get { return dgvForSending.SelectedRows[0].Index; } 
        }

        //int IVagSendOthStView.vagForSendCount
        //{
        //    get { return dgvForSending.Rows.Count; }
        //}

        //int IVagSendOthStView.vagToSendCount
        //{
        //    get { return dgvToSend.Rows.Count; }
        //}

        bool IVagSendOthStView.isVagForSendSelected(int vagIdx)
        {
            return dgvForSending.Rows[vagIdx].Selected; 
        }

        bool IVagSendOthStView.isVagForSendColored(int vagIdx)
        {
            return !dgvForSending.Rows[vagIdx].DefaultCellStyle.BackColor.IsEmpty;
        }

        //VagSendOthSt IVagSendOthStView.lastVagToSend
        //{
        //    get { return (VagSendOthSt)SendBs2.List[SendBs2.Count - 1]; }
        //}

        //VagSendOthSt IVagSendOthStView.getVagForSendByIdx(int idx)
        //{
        //    return (VagSendOthSt)SendBs1.List[idx];
        //}

        //int IVagSendOthStView.getRowOfTableWithVagForSendByIdx(int idx)
        //{
        //    return dgvForSending.Rows[idx].Index;
        //}

        List<VagSendOthSt> IVagSendOthStView.getRemainedVagForSending()
        {
            List<VagSendOthSt> list = new List<VagSendOthSt>();
            foreach (DataGridViewRow row in dgvForSending.Rows)
            {
                if (row.DefaultCellStyle.BackColor == Color.Empty)
                {
                    list.Add((VagSendOthSt)SendBs1.List[row.Index]);
                }
            }
            return list;
        }

        VagSendOthSt IVagSendOthStView.firstSelVagToSend
        {
            get { return (VagSendOthSt)SendBs2.List[dgvToSend.SelectedRows[0].Index]; }
        }

        void IVagSendOthStView.setStatLabel(string text)
        {
            lStatSend.Text = text;
        }

        void IVagSendOthStView.makeDgvForSendColumns()
        {
            vagSendOthStUtils.makeVagDgvColumns(dgvForSending);
        }

        void IVagSendOthStView.makeDgvToSendColumns()
        {
            vagSendOthStUtils.makeVagDgvColumns(dgvToSend);
        }

        void IVagSendOthStView.clearWays()
        {
            SendBsWays.Clear();
        }

        void IVagSendOthStView.bindWaysToSource(List<Way> list)
        {
            SendBsWays.DataSource = list;
        }

        void IVagSendOthStView.setCurrentWay(int idxWay)
        {
            try
            {
                SendBsWays.Position = idxWay;
                dgvWays.Rows[idxWay].Selected = true;
            }
            catch (ArgumentOutOfRangeException)
            { }
        }

        void IVagSendOthStView.loadSides(object[] items, string nonSelected)
        {
            SendCbSide.Items.Clear();
            SendCbSide.Items.AddRange(items);
            SendCbSide.SelectedIndex = -1;
            SendCbSide.Text = nonSelected;
        }

        void IVagSendOthStView.loadGF(List<GruzFront> list, string cbDisplay, string cbValue, string cbNonSelected)
        {
            cbGruzFront.DataSource = list;
            cbGruzFront.DisplayMember = cbDisplay;
            cbGruzFront.ValueMember = cbValue;
            cbGruzFront.SelectedIndex = -1;
            cbGruzFront.Text = cbNonSelected;
        }

        void IVagSendOthStView.loadShops(List<Shop> list, string cbDisplay, string cbValue, string cbNonSelected)
        {
            cbShop.DataSource = list;
            cbShop.DisplayMember = cbDisplay;
            cbShop.ValueMember = cbValue;
            cbShop.SelectedIndex = -1;
            cbShop.Text = cbNonSelected;
        }

        void IVagSendOthStView.loadStationsTo(List<Station> list, string cbDisplay, string cbValue, string cbNonSelected)
        {
            SendCbStat.DataSource = list;
            SendCbStat.DisplayMember = cbDisplay;
            SendCbStat.ValueMember = cbValue;
            SendCbStat.SelectedIndex = -1;
            SendCbStat.Text = cbNonSelected;
        }

        void IVagSendOthStView.loadLocomotives(List<Locomotive> list, string cbDisplay, string cbValue, string cbNonSelected)
        {
            cbSendLocom1.DataSource = list;
            cbSendLocom1.DisplayMember = cbDisplay;
            cbSendLocom1.ValueMember = cbValue;
            cbSendLocom1.Text = cbNonSelected;

            cbSendLocom2.DataSource = list;
            cbSendLocom2.DisplayMember = cbDisplay;
            cbSendLocom2.ValueMember = cbValue;
            cbSendLocom2.Text = cbNonSelected;
        }

        void IVagSendOthStView.clearStationsTo(string text)
        {
            SendCbStat.SelectedIndex = -1;
            SendCbStat.Text = text;
        }

        void IVagSendOthStView.clearSide(string text)
        {
            SendCbSide.SelectedIndex = -1;
            SendCbSide.Text = text;
        }

        void IVagSendOthStView.clearGF(string text)
        {
            cbGruzFront.SelectedIndex = -1;
            cbGruzFront.Text = text;
        }

        void IVagSendOthStView.clearShop(string text)
        {
            cbShop.SelectedIndex = -1;
            cbShop.Text = text;
        }

        void IVagSendOthStView.clearLocoms(string text)
        {
            chbOtherStLocom.Checked = false;
            cbSendLocom1.SelectedIndex = -1;
            cbSendLocom1.Text = text;
            cbSendLocom2.SelectedIndex = -1;
            cbSendLocom2.Text = text;
        }

        void IVagSendOthStView.deleteGfItems()
        {
            cbGruzFront.DataSource = null;
            cbGruzFront.Items.Clear();
        }

        void IVagSendOthStView.deleteShopItems()
        {
            cbShop.DataSource = null;
            cbShop.Items.Clear();
        }

        void IVagSendOthStView.bindVagForSendToSource(List<VagSendOthSt> list)
        {
            SendBs1.DataSource = list;
        }

        void IVagSendOthStView.clearVagForSendSelection()
        {
            dgvForSending.ClearSelection();
        }

        //void IVagSendOthStView.clearVagToSendSelection()
        //{
        //    dgvToSend.ClearSelection();
        //}

        void IVagSendOthStView.bindVagToSendToSource(List<VagSendOthSt> list)
        {
            SendBs2.DataSource = list;
        }

        void IVagSendOthStView.clearVagToSend()
        {
            SendBs2.Clear();
        }

        void IVagSendOthStView.setTrainNum(int num)
        {
            lNumTrain.Text = num.ToString();
        }

        void IVagSendOthStView.setStationTo(int id_stat)
        {
            SendCbStat.SelectedValue = id_stat;
        }

        void IVagSendOthStView.setSide(Side side)
        {
            SendCbSide.SelectedIndex = (int)side;
        }

        void IVagSendOthStView.setGF(int idGF)
        {
            cbGruzFront.SelectedValue = idGF;
        }

        void IVagSendOthStView.setShop(int idShop)
        {
            cbShop.SelectedValue = idShop;
        }

        int? IVagSendOthStView.setLocom1(int locom1)
        {
            cbSendLocom1.SelectedValue = locom1;
            return (int?)cbSendLocom1.SelectedValue;
        }

        int? IVagSendOthStView.setLocom2(int locom2)
        {
            cbSendLocom2.SelectedValue = locom2;
            return (int?)cbSendLocom2.SelectedValue;
        }

        void IVagSendOthStView.setOtherStatLocomotives(bool otherStat)
        {
            chSendLocomOthSt.Checked = otherStat;
        }

        void IVagSendOthStView.changeColorVagSelectedForSend(List<int> rowsIdx)
        {
            for (int i = 0; i < dgvForSending.Rows.Count; i++)
            {
                if (rowsIdx.Contains(i))
                    dgvForSending.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
            }
        }

        void IVagSendOthStView.changeColumnsPositions(bool isDepart)
        {
            vagOperationsUtils.changeColumnsPosition(dgvForSending, isDepart);
            vagOperationsUtils.changeColumnsPosition(dgvToSend, isDepart);
        }

        void IVagSendOthStView.addVagToSendFromVagsForSend(int vagForSendIdx)
        {
            SendBs2.Add(SendBs1.List[dgvForSending.Rows[vagForSendIdx].Index]);
        }

        void IVagSendOthStView.setVagForSendColor(int vagIdx, Color color)
        {
            dgvForSending.Rows[vagIdx].DefaultCellStyle.BackColor = color;
        }

        void IVagSendOthStView.removeFromVagToSend(VagSendOthSt vagon)
        {
            SendBs2.Remove(vagon);
        }

        void IVagSendOthStView.moveToLastVagForSend()
        {
            SendBs1.MoveLast();
        }

        void IVagSendOthStView.moveToPrevVagForSend()
        {
            SendBs1.MovePrevious();
        }

        void IVagSendOthStView.moveToFirstVagForSend()
        {
            SendBs1.MoveFirst();
        }

        void IVagSendOthStView.moveToNextVagForSend()
        {
            SendBs1.MoveNext();
        }

        void IVagSendOthStView.selectVagForSendByIdx(int idx)
        {
            dgvForSending.Rows[idx].Selected = true;
        }

        void IVagSendOthStView.refreshWayTable()
        {
            dgvWays.Refresh();
        }

        void IVagSendOthStView.clearColorAndDtFromWayMultipleVag() 
        {
            foreach (DataGridViewRow row in dgvForSending.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.Empty;
                ((VagSendOthSt)SendBs1.List[row.Index]).dt_from_way = null;
            }
        }

        void IVagSendOthStView.showHideGfAndShopsOnForm(bool hasGF, bool hasShops)
        {
            label17.Visible = hasGF;
            cbGruzFront.Visible = hasGF;

            label16.Visible = hasShops;
            cbShop.Visible = hasShops;
        }

        void IVagSendOthStView.vagTableSetScrollToSelRow()
        {
            dgvForSending.FirstDisplayedScrollingRowIndex = dgvForSending.SelectedRows[0].Index;
        }

        // IVagWaitAdmissView

        VagWaitAdmissUtils vagWaitAdmissUtils = new VagWaitAdmissUtils();

        int IVagWaitAdmissView.dgvVagColumnsCount
        {
            get { return dgvVagP2.Columns.Count; }
        }

        int IVagWaitAdmissView.dgvSelVagColumnsCount
        {
            get { return dgvSelVagP2.Columns.Count; }
        }

        //int IVagWaitAdmissView.getRowOfTableWithVagByIdx(int idx)
        //{
        //    return dgvVagP2.Rows[idx].Index;
        //}

        bool IVagWaitAdmissView.hasFromStatVag 
        {
            get { return dgvTrainsP2.Rows.Count > 0; } 
        }

        bool IVagWaitAdmissView.hasGfVag 
        {
            get { return dgvTrainsGF.Rows.Count > 0; } 
        }

        bool IVagWaitAdmissView.hasShopVag 
        {
            get { return dgvTrainsShops.Rows.Count > 0; } 
        }

        bool IVagWaitAdmissView.hasSelFromStatVag
        {
            get { return dgvTrainsP2.SelectedRows.Count > 0; }
        }

        Train IVagWaitAdmissView.getSelTrain(bool isGf, bool isShop)
        {
            try
            {
                if (isGf) return (Train)bsTrainsGF.List[dgvTrainsGF.SelectedRows[0].Index];
                else if (isShop) return (Train)bsTrainsShops.List[dgvTrainsShops.SelectedRows[0].Index];
                else return (Train)bsTrainP2.List[dgvTrainsP2.SelectedRows[0].Index];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        List<Train> IVagWaitAdmissView.getTrainsList(bool isGf, bool isShop)
        {
            if (isGf) return (List<Train>)bsTrainsGF.List;
            else if (isShop) return (List<Train>)bsTrainsShops.List;
            else return (List<Train>)bsTrainP2.List;
        }

        void IVagWaitAdmissView.setCurrTrain(bool isGf, bool isShop, int idx)
        {
            try
            {
                if (isGf)
                {
                    dgvTrainsGF.ClearSelection();
                    bsTrainsGF.Position = idx;
                    dgvTrainsGF.Rows[idx].Selected = true;
                }
                else if (isShop)
                {
                    dgvTrainsShops.ClearSelection();
                    bsTrainsShops.Position = idx;
                    dgvTrainsShops.Rows[idx].Selected = true;
                }
                else
                {
                    dgvTrainsP2.ClearSelection();
                    bsTrainP2.Position = idx;
                    dgvTrainsP2.Rows[idx].Selected = true;
                }
            }
            catch (ArgumentOutOfRangeException)
            { }
        }
        //VagWaitAdmiss IVagWaitAdmissView.currVag
        //{
        //    get { return (VagWaitAdmiss)bs1P2.Current; }
        //}

        int IVagWaitAdmissView.editingValue
        {
            get { return editingValue; }
        }

        int IVagWaitAdmissView.selVagCount
        {
            get { return dgvVagP2.SelectedRows.Count; }
        }

        int IVagWaitAdmissView.selVagToAdmCount
        {
            get { return dgvSelVagP2.SelectedRows.Count; }
        }

        //int IVagWaitAdmissView.vagCount
        //{
        //    get { return dgvVagP2.Rows.Count; }
        //}

        //int IVagWaitAdmissView.vagToAdmCount
        //{
        //    get { return dgvSelVagP2.Rows.Count; }
        //}

        bool IVagWaitAdmissView.isVagSelected(int vagIdx)
        {
            return dgvVagP2.Rows[vagIdx].Selected;
        }

        bool IVagWaitAdmissView.isVagColored(int vagIdx)
        {
            return !dgvVagP2.Rows[vagIdx].DefaultCellStyle.BackColor.IsEmpty;
        }

        //VagWaitAdmiss IVagWaitAdmissView.lastVag 
        //{
        //    get { return (VagWaitAdmiss)bs2P2.List[bs2P2.Count - 1]; } 
        //}

        //VagWaitAdmiss IVagWaitAdmissView.getVagByIdx(int idx)
        //{
        //    return (VagWaitAdmiss)bs1P2.List[idx];
        //}

        int IVagWaitAdmissView.idxFirstSelVag
        {
            //get { return bs1P2.IndexOf(bs1P2.Current); } 
            get { return dgvVagP2.SelectedRows[0].Index; }
        }

        int IVagWaitAdmissView.idxCurVag
        {
            get { return bs1P2.IndexOf(bs1P2.Current); }
        }
        //int IVagWaitAdmissView.idxVag(VagWaitAdmiss vagon)
        //{
        //    return bs1P2.IndexOf(vagon);
        //}

        List<VagWaitAdmiss> IVagWaitAdmissView.listToAdmiss()
        {
            try
            {
                if (bs2P2.List.GetType() == typeof(BindingList<VagWaitAdmiss>))
                    return ((BindingList<VagWaitAdmiss>)bs2P2.List).ToList();
                else return (List<VagWaitAdmiss>)bs2P2.List;
            }
            catch (InvalidCastException)
            {
                return new List<VagWaitAdmiss>();
            }
            //return (List<VagWaitAdmiss>)bs2P2.DataSource;
        }

        List<VagWaitAdmiss> IVagWaitAdmissView.listWaitAdmiss
        {
            get { return (List<VagWaitAdmiss>)bs1P2.List; }
        }

        VagWaitAdmiss IVagWaitAdmissView.firstSelVagToAdm
        {
            get { return (VagWaitAdmiss)bs2P2.List[dgvSelVagP2.SelectedRows[0].Index]; }
        }

        bool IVagWaitAdmissView.hasSelFromGfVag
        {
            get { return dgvTrainsGF.SelectedRows.Count > 0; }
        }

        bool IVagWaitAdmissView.hasSelFromShopVag
        {
            get { return dgvTrainsShops.SelectedRows.Count > 0; }
        }

        Way IVagWaitAdmissView.wayPerformAdmissTrain
        {
            get { return vagAcceptForm.getWay(); }
        }

        Side IVagWaitAdmissView.sidePerformAdmissTrain
        {
            get { return vagAcceptForm.getSide(); }
        }

        DateTime IVagWaitAdmissView.dtArriveAdmissTrain
        {
            get { return vagAcceptForm.getDtArriv(); }
        }

        Way IVagWaitAdmissView.wayPerformTransit 
        {
            get { return transitForm.getWay(); } 
        }

        DateTime IVagWaitAdmissView.dtArriveTransit 
        {
            get { return transitForm.getDtArriv(); } 
        }

        Station IVagWaitAdmissView.statPerformTransit
        {
            get { return transitForm.getStatTo(); }
        }

        void IVagWaitAdmissView.makeDgvVagColumns()
        {
            vagWaitAdmissUtils.makeVagDgvColumns(dgvVagP2);
        }

        void IVagWaitAdmissView.makeDgvSelVagColumns()
        {
            vagWaitAdmissUtils.makeVagDgvColumns(dgvSelVagP2);
        }

        void IVagWaitAdmissView.fillRospColumns(List<Way> list, string cbDisplay, string cbValue)
        {
            DataGridViewComboBoxColumn col;
            if (dgvVagP2.Columns["RospPlan"] is DataGridViewComboBoxColumn)
            {
                col = (DataGridViewComboBoxColumn)dgvVagP2.Columns["RospPlan"];
                //col.DataGridView.Name ="RospPlan";
                //col.Name = "RospPlan";
                //col.Tag = "RospPlan";
                //col.HeaderText = "RospPlan";
                col.DataSource = list;
                col.DisplayMember = cbDisplay;
                col.ValueMember = cbValue;
                col.AutoComplete = false;
            }
            if (dgvVagP2.Columns["RospFact"] is DataGridViewComboBoxColumn)
            {
                col = (DataGridViewComboBoxColumn)dgvVagP2.Columns["RospFact"];
                //col.Name = "RospFact";
                //col.DataGridView.Name = "RospFact";
                //col.Tag =  "RospFact";
                col.DataSource = list;
                col.DisplayMember = cbDisplay;
                col.ValueMember = cbValue;
            }
        }

        void IVagWaitAdmissView.setTrainNumAndDt(string trainNum, string dt)
        {
            lTrainNumP2.Text = trainNum;
            lDTsentP2.Text = dt;
        }

        void IVagWaitAdmissView.clearVagToAdm()
        {
            bs2P2.Clear();
        }

        void IVagWaitAdmissView.clearVagForAdmSel()
        {
            dgvVagP2.ClearSelection();
        }

        void IVagWaitAdmissView.bindTrainsFromStatToSource(List<Train> list)
        {
            bsTrainP2.DataSource = list;
        }

        void IVagWaitAdmissView.clearTrainsFromStatSelection()
        {
            dgvTrainsP2.ClearSelection();
        }

        void IVagWaitAdmissView.bindTrainsGfToSource(List<Train> list)
        {
            bsTrainsGF.DataSource = list;
        }

        void IVagWaitAdmissView.clearTrainsGfSelection()
        {
            dgvTrainsGF.ClearSelection();
        }

        void IVagWaitAdmissView.bindTrainsShopsToSource(List<Train> list)
        {
            bsTrainsShops.DataSource = list;
        }

        void IVagWaitAdmissView.clearTrainsShopsSelection()
        {
            dgvTrainsShops.ClearSelection();
        }

        void IVagWaitAdmissView.selectVagByIdx(int idx)
        {
            dgvVagP2.Rows[idx].Selected = true;
        }

        //void IVagWaitAdmissView.selectTrainByIdx(bool isGf, bool isShop, int idx)
        //{
        //    if (isGf) dgvTrainsGF.Rows[idx].Selected = true;
        //    else if (isShop) dgvTrainsShops.Rows[idx].Selected = true;
        //    else dgvTrainsP2.Rows[idx].Selected = true;
        //}

        //void IVagWaitAdmissView.selectFirstFromStatVag()
        //{
        //    dgvTrainsP2.Rows[0].Selected = true;
        //}

        //void IVagWaitAdmissView.selectFirstGfVag()
        //{
        //    dgvTrainsGF.Rows[0].Selected = true;
        //}

        //void IVagWaitAdmissView.selectFirstShopVag()
        //{
        //    dgvTrainsShops.Rows[0].Selected = true;
        //}

        void IVagWaitAdmissView.bindVagWaitAdmissToSource(List<VagWaitAdmiss> list)
        {
            bs1P2.DataSource = list;
        }

        void IVagWaitAdmissView.changeColumnsPositions(bool isDepart)
        {
            vagOperationsUtils.changeColumnsPosition(dgvVagP2, isDepart);
            vagOperationsUtils.changeColumnsPosition(dgvSelVagP2, isDepart);
        }

        //string IVagWaitAdmissView.firstVagCondName
        //{
        //    get { try { return ((VagWaitAdmiss)bs2P2.List[0]).Cond.Name; } catch (ArgumentOutOfRangeException) { return ""; } }
        //}

        void IVagWaitAdmissView.showHideTrainsGfShops(bool show, bool hasGF, bool hasShops)
        {
            //if (show)
            //{
            //    splitContainer11.SplitterDistance = splitContainer11.Height / 2;
            //    if (hasGF)
            //    {
            //        splitContainer12.SplitterDistance = 130;
            //    }
            //    else
            //    {
            //        splitContainer12.SplitterDistance = 26;
            //    }
            //    if (hasShops)
            //    {
            //        splitContainer12.SplitterDistance = 130;
            //    }
            //    else
            //    {
            //        splitContainer12.SplitterDistance = splitContainer12.Height;
            //    }
            //}
            //else splitContainer11.SplitterDistance = splitContainer11.Height;

            //lGFTrains.Visible = hasGF;
            //lShopTrains.Visible = hasShops;
            if (show)
            {
                int heightDgv = 0;
                foreach (DataGridViewRow row in dgvTrainsP2.Rows)
                {
                    heightDgv = heightDgv + 44;//row.Height;
                }
                if (heightDgv == 0) heightDgv = 44;
                if (heightDgv > splitContainer11.Height)
                    splitContainer11.SplitterDistance = splitContainer11.Height - splitContainer11.Height / 3;
                else splitContainer11.SplitterDistance = heightDgv + 42 + splitContainer11.SplitterWidth;

                if (hasGF)
                {
                    heightDgv = 0;
                    foreach (DataGridViewRow row in dgvTrainsGF.Rows)
                    {
                        heightDgv = heightDgv + row.Height;
                    }
                    splitContainer12.SplitterDistance = heightDgv + 42 + panel18.Height + splitContainer12.SplitterWidth;
                }
                else
                {
                    try
                    {
                        splitContainer12.SplitterDistance = 6;
                    }
                    catch (Exception) { }
                }
            }
            else splitContainer11.SplitterDistance = splitContainer11.Height;
            lGFTrains.Visible = hasGF;
            dgvTrainsGF.Visible = hasGF;
            lShopTrains.Visible = hasShops;
            dgvTrainsShops.Visible = hasShops;
        }

        //void IVagWaitAdmissView.showHideShopVag(bool show)
        //{
        //    if (show) splitContainer3.SplitterDistance = splitContainer11.Height / 2;
        //    else splitContainer3.SplitterDistance = splitContainer3.Height;
        //    bTakeVagCeh.Visible = show;
        //    lSelVagP2.Visible = show;
        //    bCancelTrainCeh.Visible = show;
        //}

        void IVagWaitAdmissView.addVagToAdmFromVags(int vagIdx)
        {
            bs2P2.Add(bs1P2.List[dgvVagP2.Rows[vagIdx].Index]);
        }

        void IVagWaitAdmissView.setVagColor(int vagIdx, Color color)
        {
            dgvVagP2.Rows[vagIdx].DefaultCellStyle.BackColor = color;
        }

        void IVagWaitAdmissView.bindVagToAdmToSource(List<VagWaitAdmiss> list)
        {
            bs2P2.DataSource = list;
        }

        void IVagWaitAdmissView.selectAllVagToAdm()
        {
            dgvSelVagP2.SelectAll();
        }

        bool IVagWaitAdmissView.getDialogTrainResult(Station stat, SendingPoint sp, bool sideActiv)
        {
            vagAcceptForm = new VagAcceptForm(stat, sp, sideActiv);
            if (vagAcceptForm.ShowDialog() == System.Windows.Forms.DialogResult.OK) return true;
            else return false;
        }

        bool IVagWaitAdmissView.getDialogTransitResult(Station stat)
        {
            transitForm = new fTransit(stat);
            if (transitForm.ShowDialog() == System.Windows.Forms.DialogResult.OK) return true;
            else return false;
        }

        void IVagWaitAdmissView.removeFromVagWaitAdm(VagWaitAdmiss vagon)
        {
            bs1P2.Remove(vagon);
        }

        void IVagWaitAdmissView.removeFromVagToAdm(VagWaitAdmiss vagon)
        {
            bs2P2.Remove(vagon);
        }

        void IVagWaitAdmissView.removeTrain(bool isGF, bool isShop, Train train)
        {
            if (isGF)
                bsTrainsGF.Remove(train);
            else if (isShop)
                bsTrainsShops.Remove(train);
            else bsTrainP2.Remove(train);
        }

        void IVagWaitAdmissView.refreshTrains(bool isGf, bool isShop)
        {
            if (isGf) dgvTrainsGF.Refresh();
            else if (isShop) dgvTrainsShops.Refresh();
            else dgvTrainsP2.Refresh();
        }

        void IVagWaitAdmissView.rospCheckChanged()
        {
            dgvVagP2.Columns["RospPlan"].Visible = chRospusk.Checked;
            dgvVagP2.Columns["RospFact"].Visible = chRospusk.Checked;
            bRosp.Visible = chRospusk.Checked;
            bTakeTrain.Visible = !chRospusk.Checked;
            bTakeVagPart.Visible = !chRospusk.Checked;
        }

        void IVagWaitAdmissView.vagTableSetScrollToSelRow()
        {
            dgvVagP2.FirstDisplayedScrollingRowIndex = dgvVagP2.SelectedRows[0].Index;
        }


        // IVagWaitRemoveAdmissView

        VagWaitRemoveAdmissUtils vagWaitRemoveAdmissUtils = new VagWaitRemoveAdmissUtils();

        int IVagWaitRemoveAdmissView.dgvVagColumnsCount 
        {
            get { return dgvVagWaitRemoveAdmiss.Columns.Count; } 
        }

        int IVagWaitRemoveAdmissView.dgvVagToCancColumnsCount
        {
            get { return dgvVagWaitRemoveAdmiss2.Columns.Count; } 
        }

        Train IVagWaitRemoveAdmissView.getSelTrain()
        {
            try
            {
                return (Train)bsTrainsWaitRemoveAdmiss.List[dgvTrainsVagWaitRemoveAdmiss.SelectedRows[0].Index];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        bool IVagWaitRemoveAdmissView.isVagSelected(int vagIdx)
        {
            return dgvVagWaitRemoveAdmiss.Rows[vagIdx].Selected;
        }

        bool IVagWaitRemoveAdmissView.isVagColored(int vagIdx)
        {
            return !dgvVagWaitRemoveAdmiss.Rows[vagIdx].DefaultCellStyle.BackColor.IsEmpty;
        }

        int IVagWaitRemoveAdmissView.idxFirstSelVag
        {
            get { return dgvVagWaitRemoveAdmiss.SelectedRows[0].Index; }
        }

        int IVagWaitRemoveAdmissView.selVagCount
        {
            get { return dgvVagWaitRemoveAdmiss.SelectedRows.Count; }
        }

        int IVagWaitRemoveAdmissView.selVagToCancCount
        {
            get { return dgvVagWaitRemoveAdmiss2.SelectedRows.Count; }
        }

        VagWaitRemoveAdmiss IVagWaitRemoveAdmissView.firstSelVagToCanc
        {
            get { return (VagWaitRemoveAdmiss)bsVagWaitRemoveAdmiss2.List[dgvVagWaitRemoveAdmiss2.SelectedRows[0].Index]; }
        }

        //string IVagWaitRemoveAdmissView.firstVagCondName
        //{
        //    get { try { return ((VagWaitRemoveAdmiss)bsVagWaitRemoveAdmiss.List[0]).Cond.Name; } catch (ArgumentException) { return ""; } }
        //}
        List<VagWaitRemoveAdmiss> IVagWaitRemoveAdmissView.listVagons
        {
            get {
                try
                {
                    return (List<VagWaitRemoveAdmiss>)bsVagWaitRemoveAdmiss.List;
                }
                catch (InvalidCastException)
                {
                    return new List<VagWaitRemoveAdmiss>();
                }
            }
        }

        List<VagWaitRemoveAdmiss> IVagWaitRemoveAdmissView.listToCancel
        {
            get
            {
                try
                {
                    if (bsVagWaitRemoveAdmiss2.List.GetType() == typeof(BindingList<VagWaitRemoveAdmiss>))
                        return ((BindingList<VagWaitRemoveAdmiss>)bsVagWaitRemoveAdmiss2.List).ToList();
                    else return (List<VagWaitRemoveAdmiss>)bsVagWaitRemoveAdmiss2.List;
                }
                catch (InvalidCastException)
                {
                    return new List<VagWaitRemoveAdmiss>();
                }
                //try
                //{
                    //return (List<VagWaitRemoveAdmiss>)bsVagWaitRemoveAdmiss2.List;
                //}
                //catch (InvalidCastException)
                //{
                //    return new List<VagWaitRemoveAdmiss>();
                //}
            }
        }

        void IVagWaitRemoveAdmissView.setVagColor(int vagIdx, Color color)
        {
            dgvVagWaitRemoveAdmiss.Rows[vagIdx].DefaultCellStyle.BackColor = color;
        }

        void IVagWaitRemoveAdmissView.removeFromVagToCanc(VagWaitRemoveAdmiss vagon)
        {
            bsVagWaitRemoveAdmiss2.Remove(vagon);
        }

        void IVagWaitRemoveAdmissView.selectVagByIdx(int idx)
        {
            dgvVagWaitRemoveAdmiss.Rows[idx].Selected = true;
        }

        void IVagWaitRemoveAdmissView.addVagToCancFromVags(int vagIdx)
        {
            bsVagWaitRemoveAdmiss2.Add(bsVagWaitRemoveAdmiss.List[dgvVagWaitRemoveAdmiss.Rows[vagIdx].Index]);
        }

        void IVagWaitRemoveAdmissView.makeDgvVagColumns()
        {
            vagWaitRemoveAdmissUtils.makeVagDgvColumns(dgvVagWaitRemoveAdmiss);
        }

        void IVagWaitRemoveAdmissView.makeDgvVagToCancColumns()
        {
            vagWaitRemoveAdmissUtils.makeVagDgvColumns(dgvVagWaitRemoveAdmiss2);
        }

        void IVagWaitRemoveAdmissView.setTrainNumAndDt(string trainNum, string dt)
        {
            lTrainNumP5.Text = trainNum;
            lDTsentP5.Text = dt;
        }

        void IVagWaitRemoveAdmissView.removeTrain(Train train)
        {
            bsTrainsWaitRemoveAdmiss.Remove(train);
        }

        List<Train> IVagWaitRemoveAdmissView.trainsList
        {
            get { return (List<Train>)bsTrainsWaitRemoveAdmiss.List; }
        }

        void IVagWaitRemoveAdmissView.setCurrTrain(int idx)
        {
            try
            {
                dgvTrainsVagWaitRemoveAdmiss.ClearSelection();
                bsTrainsWaitRemoveAdmiss.Position = idx;
                dgvTrainsVagWaitRemoveAdmiss.Rows[idx].Selected = true;
            }
            catch (ArgumentOutOfRangeException)
            { }
        }

        void IVagWaitRemoveAdmissView.bindTrainsWaitRemoveAdmissToSource(List<Train> list)
        {
            bsTrainsWaitRemoveAdmiss.DataSource = list;
        }

        void IVagWaitRemoveAdmissView.bindVagWaitRemoveAdmissToSource(List<VagWaitRemoveAdmiss> list)
        {
            bsVagWaitRemoveAdmiss.DataSource = list;
        }

        void IVagWaitRemoveAdmissView.bindVagToCancToSource(List<VagWaitRemoveAdmiss> list)
        {
            bsVagWaitRemoveAdmiss2.DataSource = list;
        }

        void IVagWaitRemoveAdmissView.changeColumnsPositions(bool isDepart)
        {
            vagOperationsUtils.changeColumnsPosition(dgvVagWaitRemoveAdmiss, isDepart);
        }

        //void IVagWaitRemoveAdmissView.clearVagWaitRemoveAdmiss()
        //{
        //    bsVagWaitRemoveAdmiss.Clear();
        //}

        void IVagWaitRemoveAdmissView.clearVagToCanc()
        {
            bsVagWaitRemoveAdmiss2.Clear();
        }

        void IVagWaitRemoveAdmissView.clearVagWaitRemoveAdmissSel()
        {
            dgvVagWaitRemoveAdmiss.ClearSelection();
        }

        void IVagWaitRemoveAdmissView.refreshTrains()
        {
            dgvTrainsVagWaitRemoveAdmiss.Refresh();
        }

        void IVagWaitRemoveAdmissView.vagTableSetScrollToSelRow()
        {
            dgvVagWaitRemoveAdmiss.FirstDisplayedScrollingRowIndex = dgvVagWaitRemoveAdmiss.SelectedRows[0].Index;
        }

        // --- methods ---

        public MainForm()
        {
            try
            {
                InitializeComponent();
                //log4net.Config.XmlConfigurator.Configure();

                mainPresenter = new MainPresenter(this);
                vagOnStatPresenter = new VagOnStatPresenter(this, this);
                vagManeuverPresenter = new VagManeuverPresenter(this, this);
                vagSendOtherStPresenter = new VagSendOthStPresenter(this, this);
                vagWaitAdmissPresenter = new VagWaitAdmissPresenter(this, this);
                vagWaitRemoveAdmissPresenter = new VagWaitRemoveAdmissPresenter(this, this);
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    this.Text = "Система мониторинга вагонов АМКР. Версия " + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4);
                    string mess_user_start = String.Format("Пользователь запустил приложение Railcars ver:{0}", ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4));
                    mess_user_start.SaveLogEvents(EventStatus.Ok, service.DesktopRailCars);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //log.Error("Error in MainForm constructor: "+ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mainPresenter.onFormLoad();
        }

        private void cbStat_SelectionChangeCommitted(object sender, EventArgs e)
        {
            mainPresenter.onStatSelect();
            bRefresh.Focus();
        }

        private void cbSide_SelectionChangeCommitted(object sender, EventArgs e)
        {
            mainPresenter.onSideSelect();
        }

        private void dgvWays_SelectionChanged(object sender, EventArgs e)
        {
            if (new StackTrace().GetFrame(4).GetMethod().Name != "OnCellMouseDown" &&
                new StackTrace().GetFrame(6).GetMethod().Name != "OnKeyDown") return;
            vagOnStatPresenter.onWaySelect();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            mainPresenter.onTabSelect();
        }

        private void dgvTrainsP2_SelectionChanged(object sender, EventArgs e)
        {
            //if (new StackTrace().GetFrame(4).GetMethod().Name != "OnCellMouseDown" && 
            //new StackTrace().GetFrame(6).GetMethod().Name != "OnKeyDown") return;
            if (!dgvTrainsP2.Focused)
            {
                if (new StackTrace().GetFrame(4).GetMethod().Name == "SetAndSelectCurrentCellAddress")
                    dgvTrainsP2.ClearSelection(); 
                return;
            }
            vagWaitAdmissPresenter.onTrainStatSelect();
        }

        private void bTakeTrain_Click(object sender, EventArgs e)
        {
            vagWaitAdmissPresenter.performAdmissTrain();
        }


        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            object result = myDGVSettings.getCellValue((DataGridView)sender, e.RowIndex, e.ColumnIndex);
            if (result != null) e.Value = result;
        }

        private void dgvTrains_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            object result = myDGVSettings.getCellValue((DataGridView)sender, e.RowIndex, e.ColumnIndex);
            if (result != null) e.Value = result;
        }


        private void dgvVagP2_SelectionChanged(object sender, EventArgs e)
        {
            if (!findVagByNumPerforming && new StackTrace().GetFrame(4).GetMethod().Name != "OnCellMouseDown" &&
                new StackTrace().GetFrame(6).GetMethod().Name != "OnKeyDown" &&
                new StackTrace().GetFrame(4).GetMethod().Name != "OnMouseMove" || (!dgvVagP2.Focused && !findVagByNumPerforming))
            //if (!dgvVagP2.Focused)
            {
                dgvVagP2.ClearSelection();
                return;
            }
            vagWaitAdmissPresenter.onVagSelect();
        }


        private void bTakeVagCeh_Click(object sender, EventArgs e)
        {
            //if (dgvTrainsShops.SelectedRows.Count == 0) return;
            if (bs2P2.List.Count == 0)
            {
                MessageBox.Show("Выберите вагоны для зачисления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            vagWaitAdmissPresenter.performAdmissVagPart();
        }


        private void bCancel_Click(object sender, EventArgs e)
        {
            vagWaitAdmissPresenter.cancelAllToAdm();
        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            vagWaitAdmissPresenter.onRemoveVagToAdm();
        }


        private void bRefresh_Click(object sender, EventArgs e)
        {
            mainPresenter.onRefreshForm();
        }

        private void ManCbSide_SelectionChangeCommitted(object sender, EventArgs e)
        {
            vagManeuverPresenter.ManPlaceInRightOrder();
            vagManeuverPresenter.addListOnManeuver();
        }

        private void bPerform_Click(object sender, EventArgs e)
        {
            vagManeuverPresenter.performManeuver();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            ManResize();
        }

        private void ManResize()
        {
            lFromWay.Width = dgvWayFrom.Width + splitContainer1.SplitterWidth;
            lVagForMan.Width = dgvForMan.Width + splitContainer2.SplitterWidth;
            lOnWay.Width = dgvWayTo.Width;
        }

        private void dgvWayFrom_SelectionChanged(object sender, EventArgs e)
        {
            //if (new StackTrace().GetFrame(6).GetMethod().Name != "OnMouseDown"
            //    && new StackTrace().GetFrame(6).GetMethod().Name != "OnKeyDown") return;
            if (!dgvWayFrom.Focused) return;
            vagManeuverPresenter.onWayFromSelect();
        }

        private void dgvWayTo_SelectionChanged(object sender, EventArgs e)
        {
            //if (new StackTrace().GetFrame(6).GetMethod().Name != "OnMouseDown"
            // && new StackTrace().GetFrame(6).GetMethod().Name != "OnKeyDown") return;
            if (!dgvWayTo.Focused) return;
            vagManeuverPresenter.addListOnManeuver();
        }

        private void dgvForMan_SelectionChanged(object sender, EventArgs e)
        {
            if (!dgvForMan.Focused && !findVagByNumPerforming)
            {
                dgvForMan.ClearSelection();
                return;
            }

            vagManeuverPresenter.onVagForManSelect();
        }

        private void bCancelMan_Click(object sender, EventArgs e)
        {
            vagManeuverPresenter.cancelManeuver();
        }

        private void chbOtherStLocom_CheckedChanged(object sender, EventArgs e)
        {
            vagManeuverPresenter.loadLocomotives();
        }

        private void cbLocom_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (new StackTrace().GetFrame(4).GetMethod().Name != "OnMessage" &&
            //    new StackTrace().GetFrame(4).GetMethod().Name != "OnKeyDown") return;
            if (!cbLocom.Focused) return;
            vagManeuverPresenter.addListOnManeuver();
        }

        private void сНачалаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvForMan.Focus();
            this.Cursor = Cursors.WaitCursor;
            vagManeuverPresenter.ManSelectAll(false);
            this.Cursor = Cursors.Default;
        }

        private void убратьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vagManeuverPresenter.onRemoveVagFromMan();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String openPDFFile = @"guide.pdf";
            System.IO.File.WriteAllBytes(openPDFFile, global::RailwayUI.Properties.Resources.guide);
            System.Diagnostics.Process.Start(openPDFFile);   
        }

        private void bCancelSend_Click(object sender, EventArgs e)
        {
            vagSendOtherStPresenter.cancelAllToSend();
        }

        private void dgvWays_SelectionChanged_1(object sender, EventArgs e)
        {
            //if (new StackTrace().GetFrame(6).GetMethod().Name != "OnMouseDown"
            //    && new StackTrace().GetFrame(6).GetMethod().Name != "OnKeyDown") return;
            if (!dgvWays.Focused) return;
            vagSendOtherStPresenter.onWaySelect();
        }

        private void bSend_Click(object sender, EventArgs e)
        {
            vagSendOtherStPresenter.performSending();
        }

        private void SendCbStat_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (SendCbStat.SelectedIndex == -1 || SendCbStat.SelectedValue.GetType() != typeof(int)) return;
            if (!SendCbStat.Focused) return;
            vagSendOtherStPresenter.onStatAcceptSelect();
        }

        private void cbGruzFront_SelectionChangeCommitted(object sender, EventArgs e)
        {
            vagSendOtherStPresenter.onGfSelect();
        }

        private void cbShop_SelectionChangeCommitted(object sender, EventArgs e)
        {
            vagSendOtherStPresenter.onShopSelect();
        }

        private void dgvForSending_SelectionChanged(object sender, EventArgs e)
        {
            if (!dgvForSending.Focused && !findVagByNumPerforming)
            {
                dgvForSending.ClearSelection();
                return;
            }

            vagSendOtherStPresenter.onVagForSendingSelect();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            vagSendOtherStPresenter.onRemoveVagFromSend();
        }

        private void SendCbSide_SelectionChangeCommitted(object sender, EventArgs e)
        {
            vagSendOtherStPresenter.onSideSelect();
        }

        private void dgvForSending_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //try
            //{
            //    if (dgvToSend.Rows.Count > 0 && dgvForSending.Rows[((VagSendOthSt)SendBs2.List[0]).Num_vag_on_way-1].DefaultCellStyle.BackColor.IsEmpty)
            //    {
            //        foreach (VagSendOthSt item in (List<VagSendOthSt>)SendBs1.DataSource)
            //        {
            //            if (item.St_lock_order > -1) dgvForSending.Rows[item.Num_vag_on_way - 1].DefaultCellStyle.BackColor = Color.Yellow;
            //        }
            //    }
            //}
            //catch (ArgumentOutOfRangeException)
            //{
            //}
        }

        private void dgvForMan_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //try
            //{
            //    if (dgvOnMan.Rows.Count > 0 && dgvForMan.Rows[((VagManeuver)ManBs2.List[0]).Num_vag_on_way-1].DefaultCellStyle.BackColor.IsEmpty)
            //    {
            //        foreach (VagManeuver item in (List<VagManeuver>)ManBs1.DataSource)
            //        {
            //            if (item.Lock_order > -1) dgvForMan.Rows[item.Num_vag_on_way - 1].DefaultCellStyle.BackColor = Color.Yellow;
            //        }
            //    }
            //}
            //catch (ArgumentOutOfRangeException)
            //{
            //}
        }

        private void сКонцаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvForMan.Focus();
            this.Cursor = Cursors.WaitCursor;
            vagManeuverPresenter.ManSelectAll(true);
            this.Cursor = Cursors.Default;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            dgvForSending.Focus();
            this.Cursor = Cursors.WaitCursor;
            vagSendOtherStPresenter.selectAll(false);
            this.Cursor = Cursors.Default;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            dgvForSending.Focus();
            this.Cursor = Cursors.WaitCursor;
            vagSendOtherStPresenter.selectAll(true);
            this.Cursor = Cursors.Default;
        }

        private void ManBs2_ListChanged(object sender, ListChangedEventArgs e)
        {
            vagManeuverPresenter.onVagOnManListChanged();
        }

        private void SendBs2_ListChanged(object sender, ListChangedEventArgs e)
        {
            vagSendOtherStPresenter.onVagToSendListChanged();
        }

        private void bs2P2_ListChanged(object sender, ListChangedEventArgs e)
        {
            vagWaitAdmissPresenter.onVagToAdmListChanged();
        }

        private void cbGruzFront_TextChanged(object sender, EventArgs e)
        {
            if (cbGruzFront.Focused && cbGruzFront.Text == "")
            {
                vagSendOtherStPresenter.onGfTextChanged();
            }
        }

        private void cbShop_TextChanged(object sender, EventArgs e)
        {
            if (cbShop.Focused && cbShop.Text == "")
            {
                vagSendOtherStPresenter.onShopTextChanged();
            }
        }

        private void dgvOnMan_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dgvOnMan.HitTest(e.X, e.Y);
                dgvOnMan.ClearSelection();
                dgvOnMan.Rows[hti.RowIndex].Selected = true;
            }
        }

        private void dgvToSend_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dgvToSend.HitTest(e.X, e.Y);
                //dgvToSend.ClearSelection();
                dgvToSend.Rows[hti.RowIndex].Selected = true;
            }
        }

        private void dgvSelVagP2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dgvSelVagP2.HitTest(e.X, e.Y);
                //dgvSelVagP2.ClearSelection();
                try
                {
                    dgvSelVagP2.Rows[hti.RowIndex].Selected = true;
                }
                catch (ArgumentOutOfRangeException)
                { }
            }
        }

        private void chRospusk_CheckedChanged(object sender, EventArgs e)
        {
            vagWaitAdmissPresenter.onRospCheckChanged();
        }

        private void dgvVagP2_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvVagP2.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        /// <summary>
        /// Обработка выпадающих списков путей роспуска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvVagP2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var combo = e.Control as ComboBox;
            if (combo != null)
            {
                combo.TextChanged += (s, ev) =>
                {
                    try
                    {
                        string se = sender.ToString();
                        editingValue = Int32.Parse(combo.SelectedValue.ToString());

                        vagWaitAdmissPresenter.onEditingRospWay((Way)combo.SelectedItem);
                    }
                    catch (Exception)
                    {
                        editingValue = -1;
                    }
                };
                combo.KeyDown += (s, ev) =>
                {
                    if (ev.KeyCode == Keys.Delete)
                    {
                        clearRospDgvCb();
                    }
                };
            }

            e.Control.KeyDown += new KeyEventHandler(dgvVagP2ComboBox_KeyDown);
        }

        private void clearRospDgvCb()
        {
            dgvVagP2.CurrentCell.Value = null;
            (dgvVagP2.EditingControl as ComboBox).SelectedValue = -1;
            if (dgvVagP2.Columns[dgvVagP2.CurrentCell.ColumnIndex].Name == "RospPlan")
            {
                dgvVagP2["RospFact", dgvVagP2.CurrentCell.RowIndex].Value = null;
            }
        }

        private void dgvVagP2ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && dgvVagP2.CurrentCell.IsInEditMode)
                dgvVagP2.CancelEdit();       
        }

        private void dgvVagP2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var comboColumn = dgvVagP2.Columns[e.ColumnIndex] as DataGridViewComboBoxColumn;
            if (comboColumn != null && editingValue != -1)
            {
                dgvVagP2[e.ColumnIndex, e.RowIndex].Value = editingValue;
                if (comboColumn.Name == "RospPlan")
                    dgvVagP2["RospFact", e.RowIndex].Value = editingValue;
            }
            //else dgvVagP2[e.ColumnIndex, e.RowIndex].Value = null;
        }

        private void dgvVagP2_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!(dgvVagP2[e.ColumnIndex, e.RowIndex] is DataGridViewComboBoxCell)) e.Cancel = true;
        }

        private void bRosp_Click(object sender, EventArgs e)
        {
            vagWaitAdmissPresenter.performRospusk();
        }

        private void cmVagWaitAdmiss1_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                if (!(dgvVagP2.Columns[dgvVagP2.CurrentCell.ColumnIndex] is DataGridViewComboBoxColumn))
                {
                    e.Cancel = true;
                }
            }
            catch (NullReferenceException)
            {
                e.Cancel = true;
            }
        }

        private void готовоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvVagP2.CurrentCell.IsInEditMode)
            {
                dgvVagP2.EditingPanel.Hide();
            }
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearRospDgvCb();
        }

        private void dgvGruzFronts_SelectionChanged(object sender, EventArgs e)
        {
            if (!dgvGruzFronts.Focused) return;
            vagOnStatPresenter.onGfSelect();
        }

        private void dgvShops_SelectionChanged(object sender, EventArgs e)
        {
            if (!dgvShops.Focused) return;
            vagOnStatPresenter.onShopSelect();
        }

        private void dgvTrainsGF_SelectionChanged(object sender, EventArgs e)
        {
            if (!dgvTrainsGF.Focused)
            {
                if (new StackTrace().GetFrame(4).GetMethod().Name == "SetAndSelectCurrentCellAddress")
                    dgvTrainsGF.ClearSelection(); 
                return;
            }
            vagWaitAdmissPresenter.onTrainGfSelect();
        }

        private void dgvTrainsShops_SelectionChanged(object sender, EventArgs e)
        {
            if (!dgvTrainsShops.Focused)
            {
                if (new StackTrace().GetFrame(4).GetMethod().Name == "SetAndSelectCurrentCellAddress")
                    dgvTrainsShops.ClearSelection();
                return;
            }
            vagWaitAdmissPresenter.onTrainShopSelect();
        }

        private void bAccSend_Click(object sender, EventArgs e)
        {
            vagWaitAdmissPresenter.performAccSend();
        }

        private void chkTransit_CheckedChanged(object sender, EventArgs e)
        {
            bAccSend.Visible = chTransit.Checked;
            chRospusk.Visible = !chTransit.Checked;
            bRosp.Visible = !chTransit.Checked && chRospusk.Checked;
            bTakeTrain.Visible = !chTransit.Checked;
            bTakeVagPart.Visible = !chTransit.Checked;
        }

        private void chSendLocomOthSt_CheckedChanged(object sender, EventArgs e)
        {
            vagSendOtherStPresenter.loadLocomotives();
        }

        private void cbSendLocom1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cbSendLocom1.Focused) return;
            vagSendOtherStPresenter.addListToSend();
        }

        private void cbSendLocom2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cbSendLocom2.Focused) return;
            vagSendOtherStPresenter.addListToSend();
        }

        private void dgvTrainsVagWaitRemoveAdmiss_SelectionChanged(object sender, EventArgs e)
        {
            if (!dgvTrainsVagWaitRemoveAdmiss.Focused) return;
            vagWaitRemoveAdmissPresenter.onTrainSelect();
        }

        private void поСобственникамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            mainPresenter.makeReportPosition(reportsDB.getPos((Station)cbStat.SelectedItem, 1), "№ пути");
            this.Cursor = Cursors.Default;
        }

        private void поСобственникамToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            mainPresenter.makeReportPosition(reportsDB.getPos(1), "Станция");
            this.Cursor = Cursors.Default;
        }

        private void поГрузамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            mainPresenter.makeReportPosition(reportsDB.getPos((Station)cbStat.SelectedItem, 2), "№ пути");
            this.Cursor = Cursors.Default;
        }

        private void поГрузамToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            mainPresenter.makeReportPosition(reportsDB.getPos(2), "Станция");
            this.Cursor = Cursors.Default;
        }

        private void поЦехамToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            mainPresenter.makeReportPosition(reportsDB.getPos(3), "Станция");
            this.Cursor = Cursors.Default;
        }

        private void поСостояниюToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            mainPresenter.makeReportPosition(reportsDB.getPos(4), "Станция");
            this.Cursor = Cursors.Default;
        }

        private void поЦехамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            mainPresenter.makeReportPosition(reportsDB.getPos((Station)cbStat.SelectedItem, 3), "№ пути");
            this.Cursor = Cursors.Default;
        }

        private void поСостояниюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            mainPresenter.makeReportPosition(reportsDB.getPos((Station)cbStat.SelectedItem, 4), "№ пути");
            this.Cursor = Cursors.Default;
        }

        private void простоиНаСтанцииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            mainPresenter.makeRepProst(reportsDB.getProst((Station)cbStat.SelectedItem), true);
            this.Cursor = Cursors.Default;
        }

        private void простоиПоПредприятиюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            mainPresenter.makeRepProst(reportsDB.getProst(), false);
            this.Cursor = Cursors.Default;
        }

        private void историяДвиженияВагонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            mainPresenter.makeRepVagHistory();
            this.Cursor = Cursors.Default; 
        }

        private void загруженностьПутейИСтанцийПредприятияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            mainPresenter.makeRepLoadWayStat();
            this.Cursor = Cursors.Default; 
        }

        private void перерабатывающаяСпособностьГФToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            mainPresenter.makeRepGfUnlTurnover();
            this.Cursor = Cursors.Default; 
        }

        private void вагоныНаПутяхОчисткиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            mainPresenter.makeRepVagOnCleanWays();
            this.Cursor = Cursors.Default; 
        }

        private void выгрузитьВExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            vagWaitAdmissPresenter.trainToExcel();
            this.Cursor = Cursors.Default;
        }

        private void отменитьОтправкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            vagWaitRemoveAdmissPresenter.cancelTrainSending(true);
            this.Cursor = Cursors.Default;
        }

        private void StTrainsContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            tsmItemCancel.Visible = !(((ContextMenuStrip)sender).SourceControl.Name == "dgvTrainsP2");
            tsmItemCancelPart.Visible = !(((ContextMenuStrip)sender).SourceControl.Name == "dgvTrainsP2");
        }

        private void tsmItemCancel_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            vagWaitAdmissPresenter.cancelVagInGfOrShop(((ContextMenuStrip)(((ToolStripMenuItem)sender).Owner)).SourceControl.Name == "dgvTrainsGF", true);
            this.Cursor = Cursors.Default;
        }

        private void отменитьОтправкуВыбранныхВагоновToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            vagWaitAdmissPresenter.cancelVagInGfOrShop(((ContextMenuStrip)(((ToolStripMenuItem)sender).Owner)).SourceControl.Name == "dgvTrainsGF", false);
            this.Cursor = Cursors.Default;
        }

        private void dgvVagWaitRemoveAdmiss_SelectionChanged(object sender, EventArgs e)
        {
            if (!findVagByNumPerforming && new StackTrace().GetFrame(4).GetMethod().Name != "OnCellMouseDown" &&
            new StackTrace().GetFrame(6).GetMethod().Name != "OnKeyDown" &&
            new StackTrace().GetFrame(4).GetMethod().Name != "OnMouseMove" || (!dgvVagWaitRemoveAdmiss.Focused && !findVagByNumPerforming))
            //if (!dgvVagP2.Focused)
            {
                dgvVagWaitRemoveAdmiss.ClearSelection();
                return;
            }
            vagWaitRemoveAdmissPresenter.onVagSelect();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            vagWaitRemoveAdmissPresenter.onRemoveVagToCanc();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            vagWaitRemoveAdmissPresenter.cancelAllToCanc();
        }

        private void dgvVagWaitRemoveAdmiss2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dgvVagWaitRemoveAdmiss2.HitTest(e.X, e.Y);
                try
                {
                    dgvVagWaitRemoveAdmiss2.Rows[hti.RowIndex].Selected = true;
                }
                catch (ArgumentOutOfRangeException)
                { }
            }
        }

        private void bsVagWaitRemoveAdmiss2_ListChanged(object sender, ListChangedEventArgs e)
        {
            vagWaitRemoveAdmissPresenter.onVagToCancListChanged();
        }

        private void отменитьОтправкуВыбранныхВагоновToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            vagWaitRemoveAdmissPresenter.cancelTrainSending(false);
            this.Cursor = Cursors.Default;
        }

        private void tbVagSearch_Click(object sender, EventArgs e)
        {
            /*if (tbVagSearch.Text == "Введите №")*/ tbVagSearch.Text = "";
            tbVagSearch.ForeColor = Color.SteelBlue;
        }

        private void bVagSearch_Click(object sender, EventArgs e)
        {
            mainPresenter.onVagSearch();
        }

        private void tbVagSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && !(Char.IsControl(e.KeyChar)) && (e.KeyChar != ','))
            {
                e.Handled = true;
            }
        }

        private void tsmiDelete_Click_1(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            vagWaitAdmissPresenter.trainDelete();
            this.Cursor = Cursors.Default;
            vagWaitAdmissPresenter.onTrainStatSelect();
        }

        /// <summary>
        /// Состояние CheckBox роспуск
        /// </summary>
        public bool chRospuskCheck
        {
            get { return chRospusk.Checked; }
        }


    }
}
