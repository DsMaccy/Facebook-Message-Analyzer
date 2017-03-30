using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuleInterface
{
    [TypeDescriptionProvider(typeof(PreferenceControlDescriptionProvider<PreferenceControl, UserControl>))]
    public partial class PreferenceControl : UserControl, IPreference
    {
        /*
        public PreferenceControl() : base()
        {
            InitializeComponent();
        }*/

        public abstract Dictionary<string, object> GetValues();
    }

    public class PreferenceControlDescriptionProvider<TAbstract, TBase> : TypeDescriptionProvider
    {
        public PreferenceControlDescriptionProvider() : base(TypeDescriptor.GetProvider(typeof(TAbstract))) { }

        public override Type GetReflectionType(Type objectType, object instance)
        {
            if (objectType == typeof(TAbstract))
                return typeof(TBase);

            return base.GetReflectionType(objectType, instance);
        }

        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            if (objectType == typeof(TAbstract))
                objectType = typeof(TBase);

            return base.CreateInstance(provider, objectType, argTypes, args);
        }
    }
}
