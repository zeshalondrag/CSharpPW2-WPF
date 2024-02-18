using System.Collections.ObjectModel;
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

namespace Daily_planner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Note> Notes { get; set; } = new ObservableCollection<Note>();
        private Note selectedNote;
        public MainWindow()
        {
            InitializeComponent();
            calendar.SelectedDate = DateTime.Today;
            note.ItemsSource = Notes;
            LoadNotes();
        }

        private void SaveNotes() // Сохранение заметки
        {
            JsonSerializationDeserialization.Serialize(Notes, "notes.json");
        }

        private void LoadNotes() // Загрузка заметок
        {
            Notes.Clear();
            IEnumerable<Note> loadedNotes = JsonSerializationDeserialization.Deserialize<Note>("notes.json");
            if (loadedNotes != null)
            {
                foreach (var note in loadedNotes)
                {
                    Notes.Add(note);
                }
                date_SelectedDateChanged(null, null);
            }
        }

        private void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e) // Календарь
        {
            DateTime date = calendar.SelectedDate ?? DateTime.Today;
            var filteredNotes = Notes.Where(n => n.Date.Date == date.Date).ToList();
            note.ItemsSource = filteredNotes;
        }

        private void create_note_Click(object sender, RoutedEventArgs e) // Кнопка для создания заметки
        {
            string title = title_note.Text.Trim();
            string description = description_note.Text.Trim();
            DateTime date = calendar.SelectedDate ?? DateTime.Today;

            if (!string.IsNullOrWhiteSpace(title) || !string.IsNullOrWhiteSpace(description))
            {
                Note newNote = new Note(title, description, date); // Передача даты заметки
                Notes.Add(newNote);
                date_SelectedDateChanged(null, null);
                SaveNotes();
            }
        }

        private void delete_note_Click(object sender, RoutedEventArgs e) // Кнопка для удаления заметки
        {
            if (selectedNote != null)
            {
                Notes.Remove(selectedNote);
                date_SelectedDateChanged(null, null);
                SaveNotes();
            }
        }

        private void save_note_Click(object sender, RoutedEventArgs e) // Кнопка для сохранения заметки
        {
            if (selectedNote != null)
            {
                selectedNote.Title = title_note.Text;
                selectedNote.Description = description_note.Text;
                note.Items.Refresh();
                SaveNotes();
            }
        }
        private void note_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedNote = note.SelectedItem as Note;
            if (selectedNote != null)
            {
                title_note.Text = selectedNote.Title;
                description_note.Text = selectedNote.Description;
            }
        }
    }
}