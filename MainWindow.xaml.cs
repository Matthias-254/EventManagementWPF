using System;
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
using EventManagement.Models;
using EventManagement.Models.ViewModels;
using EventManagement.Resources;
using Microsoft.EntityFrameworkCore;


namespace EventManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EM_Context context = App.Context;
        Boolean textChanged = false;
        List<Event> eventList { get; set; } = new List<Event>();
        Event selectedEvent { get; set; } = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void dgStaffs_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StaffDatagridViewModel staff = (StaffDatagridViewModel)dgStaffs.CurrentItem;
            if (staff != null)
            {
                dgStaffs.SelectedItem = staff;
                tbFirstName.Text = staff.FirstName;
                tbLastName.Text = staff.LastName;
                tbUserName.Text = staff.Name;
                tbUserName.IsEnabled = false;
                btDelete.IsEnabled = true;
            }
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            tbFirstName.Text = "";
            tbLastName.Text = "";
            tbUserName.Text = "";
            tbUserName.IsEnabled = true;
            btDelete.IsEnabled = false;
        }

        private void InitTiStaff()
        {
            try
            {
                dgStaffs.ItemsSource = (from staff in App.Context.Staffs
                                        where staff.Deleted > DateTime.Now
                                            && (tbSelecting.Text == ""
                                                || staff.Name.Contains(tbSelecting.Text)
                                                || staff.FirstName.Contains(tbSelecting.Text)
                                                || staff.LastName.Contains(tbSelecting.Text))
                                        orderby staff.LastName, staff.FirstName
                                        select new StaffDatagridViewModel(staff)
                                        ).ToList();

                dgStaffs.Columns[0].Visibility = Visibility.Collapsed;
                dgStaffs.Columns[1].Width = 100;
                dgStaffs.Columns[2].Width = 200;
                dgStaffs.Columns[3].Width = 200;
                dgStaffs.Columns[2].Header = "First name";
                dgStaffs.Columns[3].Header = "Last name";
                btSave.IsEnabled = false;
                tbUserName.IsEnabled = false;
                btDelete.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Er is een fout opgetreden bij het ophalen van medewerkers: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            Staff staff = context.Staffs.FirstOrDefault(p => p.Name == tbUserName.Text);
            if (staff == null)
                staff = new Staff();
            if (staff != null)
            {
                staff.Name = tbUserName.Text;
                staff.FirstName = tbFirstName.Text;
                staff.LastName = tbLastName.Text;
                if (staff.Id > 0)
                    context.Update(staff);
                else
                    context.Add(staff);
                context.SaveChanges();
                InitTiStaff();
            }
        }

        private void tbSelecting_KeyUp(object sender, KeyEventArgs e)
        {
            InitTiStaff();
        }

        private void tiEvents_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!lbEventSelect.HasItems)
            {
                InitTiEvents();
            }
        }

        private void InitTiEvents()
        {
            try
            {
                eventList = context.Events
                                .Where(p => p.Deleted > DateTime.Now
                                            && (tbEventSelect.Text == "" || p.Name.Contains(tbEventSelect.Text)))
                                .OrderBy(p => p.Name)
                                .Include(p => p.EventStaffs)
                                    .ThenInclude(pp => pp.Staff)
                                .ToList();

                lbEventSelect.ItemsSource = (from even in eventList
                                             select new ListBoxItem { Content = even.Name + "   - " + (even.Description.Length > 30 ? even.Description.Substring(0, 30) + " ..." : even.Description) });

                foreach (ListBoxItem item in lbEventSelect.Items)
                {
                    item.MouseEnter += new MouseEventHandler(lbEventSelectItem_MouseEnter);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Er is een fout opgetreden bij het ophalen van evenementen: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            textChanged = true;
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {

            string s = string.Format(Strings.ResourceManager.GetString("ConfirmDelete"), tbUserName.Text);
            if (System.Windows.MessageBox.Show(s, Strings.ResourceManager.GetString("Delete?"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Staff staff = App.Context.Staffs.FirstOrDefault(p => p.Name == tbUserName.Text);
                staff.Deleted = DateTime.Now;
                context.Update(staff);
                context.SaveChanges();
                InitTiStaff();
            }
        }

        private void tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textChanged)
            {
                if (!(tbUserName.Text == "" || tbFirstName.Text == "" || tbLastName.Text == ""))
                {
                    btSave.IsEnabled = true;
                }
                textChanged = false;
            }
        }

        private void lbEventSelectItem_MouseEnter(object sender, MouseEventArgs e)
        {
            lbEventSelect.ToolTip = null;

            ListBoxItem item = (ListBoxItem)sender;
            int index = lbEventSelect.Items.IndexOf(sender);
            if (eventList[index].Description.Length > 30)
            {
                lbEventSelect.ToolTip = eventList[index].Description;
            }
        }

        private void lbEventSelect_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            e.Handled = ((ListBox)sender).ToolTip == null;
        }

        private void lbEventSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedEvent = eventList[((ListBox)sender).SelectedIndex];
            tiEvents.DataContext = selectedEvent;
        }

        private void tbEventSelect_TextChanged(object sender, TextChangedEventArgs e)
        {
            InitTiEvents();
        }
    }
}