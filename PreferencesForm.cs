﻿using System;
using System.Linq;
using Microsoft.Win32;
using System.Windows.Forms;

namespace pl.polidea.lab.Web_Page_Screensaver
{
    public partial class PreferencesForm : Form
    {
        public const string URL_PREF = "Url";
        public const string CLOSE_ON_ACTIVITY_PREF = "CloseOnActivity";
        public const string INTERVAL_PREF = "RotationInterval";
        public const string RANDOMIZE_PREF = "RandomOrder";

        //set your default URL for the screen saver
        public const string URL_PREF_DEFAULT = "http://yourwebpageURL";
        public const string CLOSE_ON_ACTIVITY_PREF_DEFAULT = "True";
        public const string INTERVAL_PREF_DEFAULT = "30";
        public const string RANDOMIZE_PREF_DEFAULT = "True";

        private ContextMenuStrip urlsContextMenu;

        public PreferencesForm()
        {
            InitializeComponent();

            urlsContextMenu = new ContextMenuStrip();
            urlsContextMenu.Opening += UrlsContextMenu_Opening;

            var moveUrlUp = urlsContextMenu.Items.Add("Move Up");
            moveUrlUp.Click += MoveUrlUp_Click;

            var moveUrlDown = urlsContextMenu.Items.Add("Move Down");
            moveUrlDown.Click += MoveUrlDown_Click;

            var removeUrl = urlsContextMenu.Items.Add("Remove URL");
            removeUrl.Click += RemoveUrl_Click;

            lbUrls.ContextMenuStrip = urlsContextMenu;
            lbUrls.MouseDown += LbUrls_MouseDown;
        }

        private void MoveUrlDown_Click(object sender, EventArgs e)
        {
            var selected = lbUrls.SelectedItem;
            if (selected != null)
            {
                var newIndex = Math.Min(lbUrls.Items.Count - 1, lbUrls.SelectedIndex + 1);
                lbUrls.Items.RemoveAt(lbUrls.SelectedIndex);
                lbUrls.Items.Insert(newIndex, selected);
                lbUrls.SelectedIndex = newIndex;
            }
        }

        private void MoveUrlUp_Click(object sender, EventArgs e)
        {
            var selected = lbUrls.SelectedItem;
            if (selected != null)
            {
                var newIndex = Math.Max(0, lbUrls.SelectedIndex - 1);
                lbUrls.Items.RemoveAt(lbUrls.SelectedIndex);
                lbUrls.Items.Insert(newIndex, selected);
                lbUrls.SelectedIndex = newIndex;
            }
        }

        private void UrlsContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lbUrls.SelectedItem == null)
            {
                e.Cancel = true;
            }
        }

        private void LbUrls_MouseDown(object sender, MouseEventArgs e)
        {
            var clickedIndex = lbUrls.IndexFromPoint(e.Location);
            lbUrls.SelectedIndex = lbUrls.IndexFromPoint(e.Location);
        }

        private void RemoveUrl_Click(object sender, EventArgs e)
        {
            if (lbUrls.SelectedItem != null)
            {
                lbUrls.Items.Remove(lbUrls.SelectedItem);
            }
        }

        private void PreferencesForm_Load(object sender, EventArgs e)
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey(Program.KEY);
            loadUrls(reg);
            cbCloseOnActivity.Checked = Boolean.Parse((string)reg.GetValue(CLOSE_ON_ACTIVITY_PREF, CLOSE_ON_ACTIVITY_PREF_DEFAULT));
            nudRotationInterval.Value = int.Parse((string)reg.GetValue(INTERVAL_PREF, INTERVAL_PREF_DEFAULT));
            cbRandomize.Checked = Boolean.Parse((string)reg.GetValue(RANDOMIZE_PREF, RANDOMIZE_PREF_DEFAULT));
            reg.Close();
        }

        private void loadUrls(RegistryKey reg)
        {
            lbUrls.Items.Clear();

            var urls = ((string)reg.GetValue(URL_PREF, URL_PREF_DEFAULT)).Split(' ');

            foreach (var url in urls)
            {
                lbUrls.Items.Add(url);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                RegistryKey reg = Registry.CurrentUser.CreateSubKey(Program.KEY);
                saveUrls(reg);
                reg.SetValue(CLOSE_ON_ACTIVITY_PREF, cbCloseOnActivity.Checked);
                reg.SetValue(INTERVAL_PREF, nudRotationInterval.Value);
                reg.SetValue(RANDOMIZE_PREF, cbRandomize.Checked);
                reg.Close();
            }

            base.OnClosed(e);
        }

        private void saveUrls(RegistryKey reg)
        {
            var urls = String.Join(" ", lbUrls.Items.Cast<String>());

            reg.SetValue(URL_PREF, urls);
        }

      

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void addUrlButton_Click(object sender, EventArgs e)
        {
            var url = tbUrlToAdd.Text;

            lbUrls.Items.Add(url);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
