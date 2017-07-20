using DotNetBrowser.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nRemoter.Entity
{
    public class Mir : Machine
    {
        private string _account;           //account
        private string _pwd;               //password
        private bool _isconnected = false; //is connected
        private WinFormsBrowserView _http;          //control
        private int _refrash_time = 0;
        private Timer refrashTimer;
        private string _dir;

        #region Property

        public string Account
        {
            set { _account = value; }
            get
            {
                return _account;
            }
        }

        public string Pwd
        {
            set { _pwd = value; }
            get
            {
                return _pwd;
            }
        }

        public int Refrash_time
        {
            set { _refrash_time = value; }
            get { return _refrash_time; }
        }

        public string Dir
        {
            set { _dir = value; }
            get { return _dir; }
        }

        public WinFormsBrowserView HTTP
        {
            set { _http = value; }
            get
            {
                if (_http == null)
                {
                    _http = new WinFormsBrowserView();
                    _http.Dock = DockStyle.Fill;
                }
                return _http;
            }
        }

        public bool IsConnected
        {
            get { return _isconnected; }
        }

        public Timer RefrashTimer
        {
            get
            {
                if (refrashTimer == null)
                {
                    refrashTimer = new Timer();
                    refrashTimer.Interval = Refrash_time * 1000;
                    refrashTimer.Tick += new EventHandler(refrashTimer_Tick);
                }
                return refrashTimer;
            }
        }

        void refrashTimer_Tick(object sender, EventArgs e)
        {
            HTTP.Refresh();
        }

        #endregion

        #region Method

        /// <summary>
        /// connect
        /// </summary>
        public void Connect()
        {
            if (_isconnected)
            {
                return;
            }

            _isconnected = true;
            string strHost = this.Server;
            string strAuth = string.Empty;
            string tailHostText = string.Empty;
            string headHostText = string.Empty;

            if (_account != null && _pwd != null)
            {
                if (_account != string.Empty && _pwd != string.Empty)
                {
                    strAuth = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(_account + ":" + Pwd)) + "\r\n";
                }
            }

            if (!strHost.Contains("http" + "://"))
            {
                strHost = "http" + "://" + strHost;
            }

            if (this.Port != string.Empty)
            {
                strHost = strHost + ":" + this.Port;
            }
            if (this.Dir!=string.Empty)
            {
                strHost = strHost + this.Dir;
            }
            
            _http.URL = null;
            
            _http.Browser.LoadURL(strHost);
            _http.Show();
            if (this.TreeNode == null)
            {
                return;
            }
            
            if (this.TreeNode.ImageIndex == 179)//lefttreecontent中imagelist最大节点数
            {
                return;
            }
            this.TreeNode.ImageIndex = this.TreeNode.SelectedImageIndex += 1;

            foreach (TreeNode node in this.CustomTNode)
            {
                node.ImageIndex = node.SelectedImageIndex += 1;
            }

            if (Refrash_time > 0)
            {
                RefrashTimer.Enabled = true;
            }
        }

        /// <summary>
        /// disconnect
        /// </summary>
        public void Disconnect()
        {
            if (!_isconnected)
            {
                return;
            }

            if (Refrash_time > 0)
            {
                RefrashTimer.Enabled = false;
            }

            _isconnected = false;
            _http.URL = null;
            _http.Hide();
            if (this.TreeNode == null)
            {
                return;
            }
           
            if (this.TreeNode.ImageIndex == 1)
            {
                return;
            }
            this.TreeNode.ImageIndex = this.TreeNode.SelectedImageIndex -= 1;

            foreach (TreeNode node in this.CustomTNode)
            {
                node.ImageIndex = node.SelectedImageIndex -= 1;
            }
        }

        #endregion
    }
}
