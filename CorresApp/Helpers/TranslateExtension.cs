using System;
using System.Reflection;
using System.Resources;
using Plugin.Multilingual;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CorresApp.Helpers
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        const string ResourceId = "CorresApp.Resources.LangaugeResource";
        static readonly Lazy<ResourceManager> resmgr = new Lazy<ResourceManager>(() => new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly));

        public string Text { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return null;

            var assembly = typeof(TranslateExtension).GetTypeInfo().Assembly;
            var assemblyName = assembly.GetName();
            ResourceManager resourceManager = new ResourceManager($"{assemblyName.Name}.Resources.LangaugeResource", assembly);
            var text = resourceManager.GetString(Text, CrossMultilingual.Current.CurrentCultureInfo);

            return text;
            //return resourceManager.GetString(Text, CrossMultilingual.Current.CurrentCultureInfo);
        }
        //        public object ProvideValue(IServiceProvider serviceProvider)
        //        {
        //            if (Text == null)
        //                return "";

        //            var ci = CrossMultilingual.Current.CurrentCultureInfo;

        //            var translation = resmgr.Value.GetString(Text, ci);

        //            if (translation == null)
        //            {

        //#if DEBUG
        //                throw new ArgumentException(
        //                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
        //                    "Text");
        //#else
        //				translation = Text; // returns the key, which GETS DISPLAYED TO THE USER
        //#endif
        //            }
        //            return translation;
        //        }

        public class IzeExtension : TranslateExtension { }

        public string GetTranslatedEnum(Enum value)
        {
            string path = String.Format("Resources.{0}", ResourceId);

            ResourceManager resources = new ResourceManager("LangaugeResource", global::System.Reflection.Assembly.Load(ResourceId));

            if (resources != null)
            {
                return resources.GetString(value.ToString());
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
