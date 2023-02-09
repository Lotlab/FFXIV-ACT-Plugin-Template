using Advanced_Combat_Tracker;
using Lotlab.PluginCommon.FFXIV;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace PluginTemplate
{
    public partial class ACTPlugin : IActPluginV1
    {
        /// <summary>
        /// FFXIV 插件引用
        /// </summary>
        ACTPluginProxy ffxiv { get; set; } = null;

        /// <summary>
        /// 状态 Label
        /// </summary>
        Label statusLabel { get; set; } = null;

        /// <summary>
        /// ACT插件接口 - 初始化插件
        /// </summary>
        /// <remarks>
        /// 在这里初始化整个插件
        /// </remarks>
        /// <param name="pluginScreenSpace">插件所在的Tab页面</param>
        /// <param name="pluginStatusText">插件列表的状态标签</param>
        void IActPluginV1.InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            // 设置状态标签引用方便后续使用
            statusLabel = pluginStatusText;

            // 查找解析插件
            var plugins = ActGlobals.oFormActMain.ActPlugins;
            foreach (var item in plugins)
            {
                if (ACTPluginProxy.IsFFXIVPlugin(item.pluginObj))
                {
                    ffxiv = new ACTPluginProxy(item.pluginObj);
                    break;
                }
            }

            // 若没有找到，则直接退出
            if (ffxiv == null || !ffxiv.PluginStarted)
            {
                pluginStatusText.Text = "FFXIV ACT Plugin is not loaded.";
                return;
            }

            // 注册网络事件
            ffxiv.DataSubscription.NetworkReceived += onNetworkReceived;

            // 初始化UI
            var vm = new PluginControlViewModel();
            var control = new PluginControl();
            control.DataContext = vm;
            var host = new ElementHost()
            {
                Dock = DockStyle.Fill,
                Child = control
            };

            pluginScreenSpace.Text = "PluginTemplate";
            pluginScreenSpace.Controls.Add(host);

            // 更新状态标签的内容
            statusLabel.Text = "Plugin Inited.";
        }

        /// <summary>
        /// ACT插件接口 - 反初始化插件
        /// </summary>
        void IActPluginV1.DeInitPlugin()
        {
            // 反注册事件
            if (ffxiv != null)
            {
                ffxiv.DataSubscription.NetworkReceived -= onNetworkReceived;
            }
            ffxiv = null;

            // 更新状态
            if (statusLabel != null)
            {
                statusLabel.Text = "Plugin Exit.";
            }
            statusLabel = null;
        }

        /// <summary>
        /// 收到网络数据包的回调
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="epoch"></param>
        /// <param name="message"></param>
        void onNetworkReceived(string connection, long epoch, byte[] message)
        {
            // todo
        }
    }
}
