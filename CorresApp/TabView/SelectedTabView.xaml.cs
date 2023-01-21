using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace CorresApp.TabView
{
    public partial class SelectedTabView : ContentView
    {
        public SelectedTabView()
        {
            InitializeComponent();
        }
            public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(
                propertyName: "Command",
                returnType: typeof(ICommand),
                declaringType: typeof(SelectedTabView),
                defaultValue: null,
                defaultBindingMode: BindingMode.OneWay,
                validateValue: null,
                propertyChanged: OnItemTappedChanged
                );
            public ICommand Command
            {
                get { return (ICommand)GetValue(CommandProperty); }
                set { SetValue(CommandProperty, value); }
            }

            public static void OnItemTappedChanged(BindableObject bindable, object oldValue, object newValue)
            {
                var control = bindable as SelectedTabView;

                if (control != null)
                {
                    control.StackLayout.GestureRecognizers.Clear();
                    control.StackLayout.GestureRecognizers.Add(
                        new TapGestureRecognizer()
                        {
                            Command = new Command((o) => {

                                var command = (ICommand)bindable.GetValue(CommandProperty);

                                if (command != null && command.CanExecute(null))
                                    command.Execute(null);
                            })
                        }
                    );
                }
            }

            public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(
                                                            propertyName: "TitleText",
                                                            returnType: typeof(string),
                                                            declaringType: typeof(SelectedTabView),
                                                            defaultValue: "",
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: TitleTextPropertyChanged);

            public string TitleText
            {
                get { return base.GetValue(TitleTextProperty).ToString(); }
                set { base.SetValue(TitleTextProperty, value); }
            }

            private static void TitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            {
                var control = (SelectedTabView)bindable;
                control.Text.Text = newValue.ToString();
            }

            public static readonly BindableProperty ImageProperty = BindableProperty.Create(
                                                            propertyName: "Image",
                                                            returnType: typeof(string),
                                                            declaringType: typeof(SelectedTabView),
                                                            defaultValue: "",
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: ImageSourcePropertyChanged);

            public string Image
            {
                get { return base.GetValue(ImageProperty).ToString(); }
                set { base.SetValue(ImageProperty, value); }
            }

            private static void ImageSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
            {
                var control = (SelectedTabView)bindable;
                control.image.Source = ImageSource.FromFile(newValue.ToString());
            }

        

    }
}
