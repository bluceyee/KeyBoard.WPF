﻿using KeyBoard.WPF.Controls.Attach;
using KeyBoard.WPF.UControl;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace KeyBoard.WPF.Behavior
{
    /// <summary>
    /// 全键盘
    /// </summary>
    public class KeyboardBehavior : Behavior<Control>
    {
        /// <summary>
        /// 触发键盘的控件所在的容器
        /// </summary>
        public Panel? Panel { get; set; }

        Popup popup = new Popup();

        /// <summary>
        /// 小键盘背景色
        /// </summary>
        public Brush? UCBackground { get; set; }

        public KeyboardBehavior()
        {
            this.popup.AllowsTransparency= true;
            this.popup.AllowDrop = true;
        }

        protected override void OnAttached()
        {
            this.AssociatedObject.GotFocus += AssociatedObject_GotFocus;
            this.AssociatedObject.LostFocus += AssociatedObject_LostFocus;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
            this.AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
            base.OnDetaching();
        }

        private void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.popup != null)
            {
                this.popup.IsOpen = false;
            }
            if (this.Panel != null)
            {
                this.Panel.Children.Remove(this.popup);
            }
        }

        private void AssociatedObject_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Panel = this.GetPanel(this.AssociatedObject);
            if (this.Panel == null)
            {
                return;
            }
            if (VisualTreeHelper.GetParent(this.popup) != null)
            {
                return;
            }
            this.Panel.Children.Add(popup);
            Keyboard keyboard = new Keyboard();
            if (this.UCBackground != null)
            {
                keyboard.SetValue(UCAttach.UCBackgroundProperty, this.UCBackground);
            }
            keyboard.ClosedEvent += Keyboard_ClosedEvent;
            popup.Child = keyboard;
            popup.IsOpen = true;
            popup.StaysOpen = true;
            popup.Placement = PlacementMode.Mouse;
            
        }

        private void Keyboard_ClosedEvent(object? sender, EventArgs e)
        {
            if (this.popup != null)
            {
                this.popup.IsOpen = false;
            }
            if (this.Panel != null)
            {
                this.Panel.Children.Remove(this.popup);
            }
        }

        private Panel? GetPanel(DependencyObject dependencyObject)
        {
            dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            if (dependencyObject == null)
            {
                return null;
            }
            Panel? pl = dependencyObject as Panel;
            if (pl != null)
            {
                return pl;
            }
            else
            {
                return this.GetPanel(dependencyObject);
            }
        }
    }
}
