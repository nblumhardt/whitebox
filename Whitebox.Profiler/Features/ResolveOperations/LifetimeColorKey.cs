using System.Collections.Generic;
using System.Windows.Media;

namespace Whitebox.Profiler.Features.ResolveOperations
{
    // Just an ugly placeholder while the UI gets shaken out.
    class LifetimeColorKey
    {
        public static readonly LifetimeColorKey Instance = new LifetimeColorKey();

        readonly Queue<Brush> _brushes = new Queue<Brush>(new[]
        {
            Brushes.Olive,
            Brushes.Violet,
            Brushes.Orange
        });

        readonly IDictionary<string, Brush> _descriptionToBrush = new Dictionary<string, Brush>();

        public LifetimeColorKey()
        {
            _descriptionToBrush.Add("root", Brushes.Tomato);
            _descriptionToBrush.Add("httpRequest", Brushes.SlateBlue);
        }

        public Brush GetBrush(string lifetimeDescription)
        {
            if (!_descriptionToBrush.ContainsKey(lifetimeDescription))
                _descriptionToBrush.Add(lifetimeDescription, GetNextBrush());

            return _descriptionToBrush[lifetimeDescription];
        }

        Brush GetNextBrush()
        {
            if (_brushes.Count != 0)
                return _brushes.Dequeue();

            return Brushes.Teal;
        }
    }
}
