using Lotlab.PluginCommon.Overlay;
using Newtonsoft.Json.Linq;
using RainbowMage.OverlayPlugin;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PluginTemplate
{
    /// <summary>
    /// 悬浮窗集成
    /// </summary>
    /// <remarks>
    /// 若不需要悬浮窗，则可直接删除此文件和对应的依赖项目
    /// </remarks>
    public partial class ACTPlugin : IOverlayAddonV2
    {
        void IOverlayAddonV2.Init()
        {
            var container = Registry.GetContainer();
            var registry = container.Resolve<Registry>();

            // 注册事件源
            var eventSource = new TemplateEventSource(container);
            registry.StartEventSource(eventSource);

            // 注册悬浮窗预设
            registry.RegisterOverlayPreset2(new TemplateOverlayPresent());
        }
    }

    /// <summary>
    /// 悬浮窗事件源
    /// </summary>
    public class TemplateEventSource : EventSourceBase
    {
        const string SAMPLE_EVENT = "templateSampleEvent";

        public TemplateEventSource(TinyIoCContainer c) : base(c)
        {
            // 设置事件源名称，必须是唯一的
            Name = "TemplatePluginES";

            // 注册数据源名称。
            // 此数据源提供给悬浮窗监听，悬浮窗使用addOverlayListener注册对应事件的回调。
            RegisterEventTypes(new List<string>()
            {
                SAMPLE_EVENT
            });

            // 注册事件接收器。
            // 注册完毕后，悬浮窗可以使用callOverlayHandler调用已经注册的方法
            RegisterEventHandler("templateSampleFunction", (obj) =>
            {
                return JObject.FromObject(new
                {
                    message = "Hello, world."
                });
            });
        }
        public override Control CreateConfigControl()
        {
            return null;
        }

        public override void LoadConfig(IPluginConfig config)
        {
        }

        public override void SaveConfig(IPluginConfig config)
        {
        }

        public void InvokeSampleEvent(string data)
        {
            // 将数据发送给悬浮窗
            DispatchEvent(JObject.FromObject(new
            {
                type = SAMPLE_EVENT,
                data = data
            }));
        }
    }

    /// <summary>
    /// 悬浮窗预设
    /// </summary>
    public class TemplateOverlayPresent : IOverlayPreset
    {
        string IOverlayPreset.Name => "悬浮窗模板";

        string IOverlayPreset.Type => "MiniParse";

        string IOverlayPreset.Url => "https://example.org";

        int[] IOverlayPreset.Size => new int[2] { 200, 200 };

        bool IOverlayPreset.Locked => false;

        List<string> IOverlayPreset.Supports => new List<string> { "modern" };

        public override string ToString()
        {
            return ((IOverlayPreset)this).Name;
        }
    }
}
