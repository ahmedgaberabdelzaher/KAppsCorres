using System;
using System.Collections.Generic;
using System.Windows.Input;
using CorresApp.Model;
using Xamarin.Forms;

namespace CorresApp.Controls
{
    public partial class CalenderEvent : ContentView
    {
        public static BindableProperty CalenderEventCommandProperty =
             BindableProperty.Create(nameof(CalenderEventCommand), typeof(ICommand), typeof(CalenderEvent), null);

        public CalenderEvent()
        {
            InitializeComponent();
        }

        public ICommand CalenderEventCommand
        {
            get => (ICommand)GetValue(CalenderEventCommandProperty);
            set => SetValue(CalenderEventCommandProperty, value);
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            if (BindingContext is EventModel eventModel)
                CalenderEventCommand?.Execute(eventModel);
        }
    }
}
