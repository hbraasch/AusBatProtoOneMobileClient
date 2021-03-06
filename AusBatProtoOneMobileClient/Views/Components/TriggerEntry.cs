using AusBatProtoOneMobileClient.Models;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AusBatProtoOneMobileClient.Views.Components
{
    public class TriggerEntry : Entry
    {

        #region *// OnChangedProperty
        public static readonly BindableProperty OnChangedProperty = BindableProperty.Create(nameof(OnChanged), typeof(ICommand), typeof(TriggerEntry), null);

        public ICommand OnChanged
        {
            get { return (ICommand)GetValue(OnChangedProperty); }
            set { SetValue(OnChangedProperty, value); }
        }
        #endregion

        // Reactive extensions
        #region *// Used to debounce the search text. Only start search action after a 1 second delay of no input
        public Subject<string> typingSubject;
        private IDisposable typingEventSequence;
        #endregion

        public TriggerEntry()
        {
            this.TextChanged += (s, e) => {
                typingSubject.OnNext(Text);
            };

            // Reactive extension to debounce search term as keyed in
            typingSubject = new Subject<string>();
            typingEventSequence = typingSubject.Throttle(TimeSpan.FromSeconds(Constants.NUMERIC_ENTRY_TRIGGER_DELAY_IN_SEC))
                .Subscribe((text) =>
                Device.BeginInvokeOnMainThread(() =>
                    {
                        this.Unfocus();
                        if (!string.IsNullOrEmpty(text)) OnChanged?.Execute(null);
                    }));
        }



    }
}
