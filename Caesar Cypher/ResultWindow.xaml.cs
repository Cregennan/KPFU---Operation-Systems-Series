namespace Caesar_Cypher
{
    /// <summary>
    /// Логика взаимодействия для ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow
    {
        public ResultWindow()
        {
            InitializeComponent();
            page.NavigateToString("<html><body style=\"background: #f00;\" height=100% width=100%></body></html>");
        }
    }
}
