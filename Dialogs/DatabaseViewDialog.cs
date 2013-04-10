/*
====================================================================================
This file is part of ListPlayers, the open-source S.T.A.L.K.E.R. multiplayer
statistics organizing tool for game server administrators.
Copyright (C) 2013 Pavel Kovalenko.

You should have received a copy of the MIT License along with ListPlayers sources.
If not, see <http://www.opensource.org/licenses/mit-license.php>.

For support and more information about ListPlayers,
visit <http://mpnetworks.ru> or <https://github.com/nitrocaster/ListPlayers>
====================================================================================
*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ListPlayers.Common;
using ListPlayers.PcdbModel;
using ListPlayers.Properties;
using Settings = ListPlayers.Common.Settings;

namespace ListPlayers.Dialogs
{
    public sealed partial class DatabaseViewDialog : FormEx
    {
        private readonly Dictionary<TreeNode, IPcdbField> sample = new Dictionary<TreeNode, IPcdbField>();

        private TreeNode currentNode;
        private DatabaseViewer databaseViewer;

        public DatabaseViewDialog(DatabaseViewer databaseViewer)
        {
            InitializeComponent();
            ttNameFilter.SetToolTip(pnNameInfo, StringTable.FilterIsCaseSensetive);
            tbDbPath.SuspendLayout();
            tbDbPath.Items.AddRange(Settings.RecentDatabases);
            tbDbPath.ResumeLayout();

            chkAllRelatedData.Checked = Settings.ShowAllRelatedData;
            chkHashPattern.Checked = Settings.HashPattern;
            chkNamePattern.Checked = Settings.NamePattern;
            chkIpPattern.Checked = Settings.IpPattern;

            this.databaseViewer = databaseViewer;
            databaseViewer.SearchCompleted += OnSearchCompleted;
            databaseViewer.ConnectionStatusChanged += OnConnectionStatusChanged;
        }

        private TreeNode CurrentNode
        {
            get
            {
                return currentNode;
            }
            set
            {
                const string emptyFormat = " ";
                currentNode = value;
                var field = sample[currentNode];

                switch (field.Id)
                {
                    case PcdbFieldId.Comment:
                    {
                        tbDetails.ReadOnly = false;
                        tbDetails.Text = (PcdbItemContainer<string>)field;
                        dtpTimestamp.Enabled = false;
                        dtpTimestamp.CustomFormat = emptyFormat;
                        break;
                    }
                    case PcdbFieldId.Hash:
                    case PcdbFieldId.Group | PcdbFieldId.Hash:
                    {
                        tbDetails.ReadOnly = true;
                        tbDetails.Text = currentNode.Text;
                        dtpTimestamp.Enabled = false;
                        dtpTimestamp.CustomFormat = emptyFormat;
                        break;
                    }
                    case PcdbFieldId.Group | PcdbFieldId.Name:
                    case PcdbFieldId.Group | PcdbFieldId.Ip:
                    case PcdbFieldId.Group | PcdbFieldId.Gsid:
                    {
                        tbDetails.ReadOnly = true;
                        tbDetails.Text = "";
                        dtpTimestamp.Enabled = false;
                        dtpTimestamp.CustomFormat = emptyFormat;
                        break;
                    }
                    default:
                    {
                        tbDetails.ReadOnly = true;
                        tbDetails.Text = currentNode.Text;
                        dtpTimestamp.Enabled = true;
                        dtpTimestamp.CustomFormat = Utils.DateTimePatternLong;
                        switch (field.Id)
                        {
                            case PcdbFieldId.Name:
                            {
                                dtpTimestamp.Value = ((PcdbName)field).Timestamp;
                                break;
                            }
                            case PcdbFieldId.Ip:
                            {
                                dtpTimestamp.Value = ((PcdbIp)field).Timestamp;
                                break;
                            }
                            case PcdbFieldId.Gsid:
                            {
                                dtpTimestamp.Value = ((PcdbGsid)field).Timestamp;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void OnSearchCompleted(List<PcdbEntry> data, bool cancelled)
        {
            if (data.Count == 0 && !cancelled)
            {
                OnNotFound();
            }
            InvokeAsync(() =>
            {
                foreach (var entry in data)
                {
                    var nodeEntry = new TreeNode(entry.Hash);
                    var nodeInfo  = new TreeNode("Info");
                    nodeEntry.Nodes.Add(nodeInfo);

                    sample.Add(nodeEntry, entry);
                    sample.Add(nodeInfo, entry.Info);

                    var nodeNames = new TreeNode("Names");
                    nodeEntry.Nodes.Add(nodeNames);
                    sample.Add(nodeNames, entry.Names);

                    foreach (var field in entry.Names)
                    {
                        var node = new TreeNode(field.Name);
                        nodeNames.Nodes.Add(node);
                        sample.Add(node, field);
                    }

                    var nodeIps = new TreeNode("IPs");
                    nodeEntry.Nodes.Add(nodeIps);
                    sample.Add(nodeIps, entry.Ips);

                    foreach (var field in entry.Ips)
                    {
                        var node = new TreeNode(field.Ip);
                        nodeIps.Nodes.Add(node);
                        sample.Add(node, field);
                    }

                    if (databaseViewer.GameVersion == PcdbGameVersion.COP)
                    {
                        var nodeGsids = new TreeNode("GSIDs");
                        nodeEntry.Nodes.Add(nodeGsids);
                        sample.Add(nodeGsids, entry.Gsids);

                        foreach (var field in entry.Gsids)
                        {
                            var node = new TreeNode(field.Gsid.ToString());
                            nodeGsids.Nodes.Add(node);
                            sample.Add(node, field);
                        }
                    }

                    tvResult.Nodes.Add(nodeEntry);
                }
                //
                btnExport.Enabled = true;
                tvResult.Show();
                pnlSearchProgress.Hide();
                lSearchProgress.Hide();
                pbSearch.Hide();
                tbDetails.Clear();
                btnRefresh.Text = StringTable.Refresh;
            });
        }

        private void OnNotFound()
        {
            if (InvokeRequired)
            {
                Callback.Invoke(this, OnNotFound);
                return;
            }
            MsgBox.Info(StringTable.NothingFound);
        }

        private void OnConnectionStatusChanged(bool connected)
        {
            if (connected)
            {
                Settings.SetLastDatabase(databaseViewer.FileName);
            }
            tbDetails.ReadOnly = true;
            tbDetails.Clear();
            btnRefresh.Enabled = connected;
        }

        private void OnBeginSearch()
        {
            InvokeAsync(() =>
            {
                btnExport.Enabled = false;
                tvResult.Nodes.Clear();
                tvResult.Hide();
                pnlSearchProgress.Show();
                lSearchProgress.Show();
                pbSearch.Show();
                btnRefresh.Text = StringTable.Cancel;
            });
        }

        private void SelectedBranchToClipboard()
        {
            UseWaitCursor = true;
            try
            {
                databaseViewer.DataToClipboard(sample[CurrentNode]);
            }
            finally
            {
                UseWaitCursor = false;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string path;
            if (!PcdbFileDialog.ShowOpenDialog(out path))
            {
                return;
            }

            if (!tbDbPath.Items.Contains(path))
            {
                if (tbDbPath.Items.Count >= Settings.LastPathesCapacity)
                {
                    tbDbPath.Items.RemoveAt(Settings.LastPathesCapacity);
                }
                tbDbPath.Items.Insert(0, path);
            }
            tbDbPath.SelectedItem = path;
        }

        
        private void btnExport_Click(object sender, EventArgs e)
        {
            databaseViewer.ExportCurrentSample();
        }

        private SearchFilter GetSearchFilter()
        {
            var filter = new SearchFilter
            {
                IncludeRelatedData = chkAllRelatedData.Checked,
                UseHashPattern     = chkHashPattern.Checked,
                UseNamePattern     = chkNamePattern.Checked,
                UseIpPattern       = chkIpPattern.Checked,
                Hashes             = tbSearchHash.Lines,
                Names              = tbSearchName.Lines,
                Ips                = tbSearchIp.Lines,
                Gsids              = tbSearchGsid.Lines
            };
            return filter;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (databaseViewer.IsBusy)
            {
                databaseViewer.CancelSearch();
            }
            else
            {
                OnBeginSearch();
                databaseViewer.Filter = GetSearchFilter();
                databaseViewer.BeginSearch();
            }
        }


        private void ApplyChanges()
        {
            PcdbEntry entry;
            var node = CurrentNode;
            switch (sample[node].Id)
            {
                case PcdbFieldId.Comment:
                    entry = (PcdbEntry)sample[node.Parent];
                    entry.Info.Item = tbDetails.Text;
                    databaseViewer.UpdateEntry(entry, entry.Info);
                    break;

                case PcdbFieldId.Name:
                    entry = (PcdbEntry)sample[node.Parent.Parent];
                    var name = (PcdbName)sample[node];
                    name.Timestamp = dtpTimestamp.Value;
                    databaseViewer.UpdateEntry(entry, name);
                    break;

                case PcdbFieldId.Ip:
                    entry = (PcdbEntry)sample[node.Parent.Parent];
                    var ip = (PcdbIp)sample[node];
                    ip.Timestamp = dtpTimestamp.Value;
                    databaseViewer.UpdateEntry(entry, ip);
                    break;

                case PcdbFieldId.Gsid:
                    entry = (PcdbEntry)sample[node.Parent.Parent];
                    var gsid = (PcdbGsid)sample[node];
                    gsid.Timestamp = dtpTimestamp.Value;
                    databaseViewer.UpdateEntry(entry, gsid);
                    break;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            btnApply.Enabled = false;
            ApplyChanges();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dtTimestamp_ValueChanged(object sender, EventArgs e)
        {
            if (dtpTimestamp.Focused)
            {
                btnApply.Enabled = true;
            }
        }

        private void tbDetails_TextChanged(object sender, EventArgs e)
        {
            if (tbDetails.Focused)
            {
                btnApply.Enabled = true;
            }
        }

        private void DatabaseViewDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            databaseViewer.CloseDatabase();

            Settings.ShowAllRelatedData = chkAllRelatedData.Checked;
            Settings.HashPattern = chkHashPattern.Checked;
            Settings.NamePattern = chkNamePattern.Checked;
            Settings.IpPattern = chkIpPattern.Checked;
        }

        private void tbDbPath_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                databaseViewer.SafeOpenDatabase(tbDbPath.Text);
            }
            catch (DatabaseViewerException ex)
            {
                MsgBox.Info(ex.Message);
            }
        }

        private void cmResults_Popup(object sender, EventArgs e)
        {
            var state = (tvResult.SelectedNode != null);

            cmiExpandAll.Enabled      = state;
            cmiCollapseAll.Enabled    = state;
            cmiExpandBranch.Enabled   = state;
            cmiCollapseBranch.Enabled = state;
            cmiCopy.Enabled           = state;
        }

        private void cmiExpandBranch_Click(object sender, EventArgs e)
        {
            tvResult.SelectedNode.ExpandAll();
        }

        private void cmiExpandAll_Click(object sender, EventArgs e)
        {
            tvResult.ExpandAll();
        }

        private void cmiCollapseBranch_Click(object sender, EventArgs e)
        {
            tvResult.SelectedNode.Collapse(false);
        }

        private void cmiCollapseAll_Click(object sender, EventArgs e)
        {
            tvResult.CollapseAll();
        }

        private void tvResult_MouseDown(object sender, MouseEventArgs e)
        {
            tvResult.SelectedNode = tvResult.HitTest(e.Location).Node;
        }

        private void tvResult_AfterSelect(object sender, TreeViewEventArgs e)
        {
            btnApply.Enabled = false;
            CurrentNode = e.Node;
        }

        private void tvResult_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.C && tvResult.SelectedNode != null)
                {
                    e.Handled = false;
                    SelectedBranchToClipboard();
                }
            }
        }

        private void cmiCopy_Click(object sender, EventArgs e)
        {
            SelectedBranchToClipboard();
        }
    }
}
