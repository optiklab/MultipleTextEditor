using System.Windows;
using System.Text;

namespace MultipleTextEditor
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AboutDialog : Window
    {
        public AboutDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var text = new StringBuilder();
            text.AppendLine(@"Программа позволяет выполнить следующие действия:");
            text.AppendLine(@"- Поиск файлов по маске в указанной папке (и всех подпапках),");
            text.AppendLine(@"- Редактирование текста во всех найденных файлах,");
            text.AppendLine(@"- Замену, вставку и удаление текста.");
            Description.Text = text.ToString();

            DescriptionContinue.Text = @"Все вопросы, пожелания и комментарии Вы также можете слать мне на электронную почту:";

            TechSupportMail.Text = @" anton.yarkov@gmail.com";
        }

    }
}
