using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace XamlIslandsSample.UwpApp
{
    public sealed partial class EntryForm : UserControl, INotifyPropertyChanged
    {
        private readonly IList<Entry> _entries = new List<Entry>();

        public event PropertyChangedEventHandler PropertyChanged;

        public EntryForm()
        {
            this.InitializeComponent();
        }

        public int QuantitySaved { get; set; } = 0;

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveEntry();
            ClearFields();
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";
            interviewDatePicker.SelectedDate = null;
            acceptedComboBox.SelectedIndex = -1;
        }

        private void SaveEntry()
        {
            var entry = new Entry
            {
                FirstName = firstNameTextBox.Text,
                Lastname = lastNameTextBox.Text,
                InterviewDate = interviewDatePicker.SelectedDate.HasValue
                    ? interviewDatePicker.SelectedDate.Value.DateTime
                    : DateTime.MinValue,
                Accepted = acceptedComboBox.SelectedValue.ToString() == "Yes"
            };

            _entries.Add(entry);
            QuantitySaved++;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QuantitySaved)));
        }
    }
}
