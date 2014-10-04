using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows.Controls.Primitives;

namespace WpfStackerLibrary
{
    /// <summary>
    /// Выполните шаги 1a или 1b, а затем 2, чтобы использовать этот пользовательский элемент управления в файле XAML.
    ///
    /// Шаг 1a. Использование пользовательского элемента управления в файле XAML, существующем в текущем проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfStackerLibrary"
    ///
    ///
    /// Шаг 1б. Использование пользовательского элемента управления в файле XAML, существующем в другом проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfStackerLibrary;assembly=WpfStackerLibrary"
    ///
    /// Потребуется также добавить ссылку из проекта, в котором находится файл XAML,
    /// на данный проект и пересобрать во избежание ошибок компиляции:
    ///
    ///     Щелкните правой кнопкой мыши нужный проект в обозревателе решений и выберите
    ///     "Добавить ссылку"->"Проекты"->[Поиск и выбор проекта]
    ///
    ///
    /// Шаг 2)
    /// Теперь можно использовать элемент управления в файле XAML.
    ///
    ///     <MyNamespace:PasswordButton/>
    ///
    /// </summary>
    public class PasswordButton : Button
    {
        public PasswordButton()
            : base()
        { }

        public object BindingProperty
        {
            get { return (object)GetValue(BindingPropertyDP); }
            set { SetValue(BindingPropertyDP, value); }
        }

        /* Using a DependencyProperty as the backing store for RadioBinding.
           This enables animation, styling, binding, etc...*/
        public static readonly DependencyProperty BindingPropertyDP =
            DependencyProperty.Register(
                "BindingProperty",
                typeof(object),
                typeof(PasswordButton),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnRadioBindingChanged));

        private static void OnRadioBindingChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            PasswordButton rb = (PasswordButton)d;
           /* if (rb.RadioValue.Equals(e.NewValue))
                rb.SetCurrentValue(RadioButton.IsCheckedProperty, true);*/
        }

        private bool show_window = true;
        public String Password { get; set; }

        protected override void OnClick()
        {
            if (show_window)
            {
                PasswWin w = new PasswWin();
                w.Passw = Password;
                w.ShowDialog();
                if (w.DialogResult == true)
                {
                    base.OnClick();
                }
                
            }
            else
                base.OnClick();
        }

    }
        public class PasswordToggleButton : ToggleButton
        {
            public PasswordToggleButton()
                : base()
            { }

            public object BindingProperty
            {
                get { return (object)GetValue(BindingPropertyDP); }
                set { SetValue(BindingPropertyDP, value); }
            }

            /* Using a DependencyProperty as the backing store for RadioBinding.
               This enables animation, styling, binding, etc...*/
            public static readonly DependencyProperty BindingPropertyDP =
                DependencyProperty.Register(
                    "BindingProperty",
                    typeof(object),
                    typeof(PasswordToggleButton),
                    new FrameworkPropertyMetadata(
                        null,
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                        OnRadioBindingChanged));

            private static void OnRadioBindingChanged(
                DependencyObject d,
                DependencyPropertyChangedEventArgs e)
            {
           /*     MyRadioButton rb = (MyRadioButton)d;
                if (rb.RadioValue.Equals(e.NewValue))
                    rb.SetCurrentValue(RadioButton.IsCheckedProperty, true);*/
            }

            private bool show_window = true;
            public String Password { get; set; }
            public bool FreeReset { get; set; }

            protected override void OnClick()
            {
                if ((base.IsChecked==true) && FreeReset)
                {
                    base.OnClick();
                    return;
                }

                PasswWin w = new PasswWin();
                w.Passw = Password;
                w.ShowDialog();
                if (w.DialogResult == true)
                {
                    base.OnClick();
                }
            }

        }

}
