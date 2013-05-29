using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DifferenceEngine;
using System.Collections;

namespace XmlCompare
{
    public partial class Results : Form
    {
        public Results(DiffList_TextFile source, DiffList_TextFile destination, ArrayList DiffLines)
        {
            InitializeComponent();
            //this.Text = string.Format("Results: {0} secs.", seconds.ToString("#0.00"));

            ListViewItem lviS;
            ListViewItem lviD;
            int cnt = 1;
            int i;

            foreach (DiffResultSpan drs in DiffLines)
            {
                switch (drs.Status)
                {
                    case DiffResultSpanStatus.DeleteSource:
                        for (i = 0; i < drs.Length; i++)
                        {
                            lviS = new ListViewItem(((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line);
                            lviD = new ListViewItem("");
                            lviS.BackColor = Color.Red;
                            lviS.SubItems.Add(cnt.ToString("00000"));
                            lviD.BackColor = Color.LightGray;
                            lviD.SubItems.Add(cnt.ToString("00000"));

                            lvSource.Items.Add(lviS);
                            lvDestination.Items.Add(lviD);
                            cnt++;
                        }

                        break;
                    case DiffResultSpanStatus.NoChange:
                        for (i = 0; i < drs.Length; i++)
                        {
                            lviS = new ListViewItem(((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line);
                            lviD = new ListViewItem(((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line);
                            lviS.BackColor = Color.White;
                            lviD.SubItems.Add(cnt.ToString("00000"));
                            lviD.BackColor = Color.White;
                            lviS.SubItems.Add(cnt.ToString("00000"));

                            lvSource.Items.Add(lviS);
                            lvDestination.Items.Add(lviD);

                            cnt++;
                        }

                        break;
                    case DiffResultSpanStatus.AddDestination:
                        for (i = 0; i < drs.Length; i++)
                        {
                            lviS = new ListViewItem("");
                            lviD = new ListViewItem(((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line);
                            lviS.BackColor = Color.LightGray;
                            lviS.SubItems.Add(cnt.ToString("00000"));
                            lviD.BackColor = Color.LightGreen;
                            lviD.SubItems.Add(cnt.ToString("00000"));

                            lvSource.Items.Add(lviS);
                            lvDestination.Items.Add(lviD);
                            cnt++;
                        }

                        break;
                    case DiffResultSpanStatus.Replace:
                        for (i = 0; i < drs.Length; i++)
                        {
                            lviS = new ListViewItem(((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line);
                            lviD = new ListViewItem(((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line);
                            lviS.BackColor = Color.Red;
                            lviS.SubItems.Add(cnt.ToString("00000"));
                            lviD.BackColor = Color.LightGreen;
                            lviD.SubItems.Add(cnt.ToString("00000"));

                            lvSource.Items.Add(lviS);
                            lvDestination.Items.Add(lviD);
                            cnt++;
                        }

                        break;
                }

            }
        }

        private void lvSource_Resize(object sender, EventArgs e)
        {
            if (lvSource.Width > 100)
            {
                lvSource.Columns[1].Width = -2;
            }
        }

        private void lvDestination_Resize(object sender, EventArgs e)
        {
            if (lvDestination.Width > 100)
            {
                lvDestination.Columns[1].Width = -2;
            }
        }

        private void Results_Resize(object sender, EventArgs e)
        {
            if (lvDestination.Width > 100)
            {
                lvDestination.Columns[1].Width = -2;
            }
        }

        private void lvSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSource.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvDestination.Items[lvSource.SelectedItems[0].Index];
                lvi.Selected = true;
                lvi.EnsureVisible();
            }
        }

        private void lvDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvDestination.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvSource.Items[lvDestination.SelectedItems[0].Index];
                lvi.Selected = true;
                lvi.EnsureVisible();
            }
        }


    }
}
