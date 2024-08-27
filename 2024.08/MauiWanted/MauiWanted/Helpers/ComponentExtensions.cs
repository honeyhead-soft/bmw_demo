using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWanted.Helpers
{

    public static class ComponentExtensions
    {
        public static TComponent CustomComponent<TComponent>(this VisualNode node, Action<TComponent>? component = null)
            where TComponent : VisualNode, new()
        {
            var instance = new TComponent();
            component?.Invoke(instance);
            return instance;
        }
    }

}
