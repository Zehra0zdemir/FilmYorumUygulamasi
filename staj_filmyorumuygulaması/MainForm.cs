using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MovieReviewApp.Controls;
using MovieReviewApp.Models;
using MovieReviewApp.Services;
using MovieReviewApp.Utils;

namespace MovieReviewApp.Forms
{
    public partial class MainForm : Form
    {
        private readonly MovieService _movieService;
        private readonly ReviewService _reviewService;

        private FlowLayoutPanel _moviesPanel;
        private TextBox _searchTextBox;
        private ComboBox _genreComboBox;
        private ComboBox _yearComboBox;
        private Button _searchButton;
        private Button _addMovieButton;

        public MainForm()
        {
            _movieService = new MovieService();
            _reviewService = new ReviewService();

            InitializeComponent();
            LoadData();
            DisplayMovies();
        }

        private void InitializeComponent()
        {
            Text = "Film Yorum Uygulaması";
            Size = new Size(1000, 700);
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(800, 600);

            CreateTopPanel();
            CreateMoviesPanel();

            FormClosing += OnFormClosing;
        }

        private void CreateTopPanel()
        {
            var topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.LightSteelBlue
            };

            CreateSearchControls(topPanel);
            Controls.Add(topPanel);
        }

        private void CreateSearchControls(Panel topPanel)
        {
            _searchTextBox = new TextBox
            {
                Location = new Point(10, 20),
                Size = new Size(200, 20),
                Text = "Film adı ara...",
                ForeColor = Color.Gray
            };

            _searchTextBox.Enter += OnSearchTextBoxEnter;
            _searchTextBox.Leave += OnSearchTextBoxLeave;

            _genreComboBox = new ComboBox
            {
                Location = new Point(220, 20),
                Size = new Size(120, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _genreComboBox.Items.Add("Tüm Türler");
            foreach (MovieGenre genre in Enum.GetValues(typeof(MovieGenre)))
            {
                _genreComboBox.Items.Add(EnumHelper.GetGenreText(genre));
            }
            _genreComboBox.SelectedIndex = 0;

            _yearComboBox = new ComboBox
            {
                Location = new Point(350, 20),
                Size = new Size(80, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _yearComboBox.Items.Add("Tüm Yıllar");
            for (int year = DateTime.Now.Year; year >= 1900; year--)
            {
                _yearComboBox.Items.Add(year.ToString());
            }
            _yearComboBox.SelectedIndex = 0;

            _searchButton = new Button
            {
                Text = "Ara",
                Location = new Point(440, 18),
                Size = new Size(60, 25)
            };
            _searchButton.Click += OnSearchButtonClick;

            _addMovieButton = new Button
            {
                Text = "Yeni Film Ekle",
                Location = new Point(520, 18),
                Size = new Size(120, 25),
                BackColor = Color.LightGreen
            };
            _addMovieButton.Click += OnAddMovieButtonClick;

            topPanel.Controls.AddRange(new Control[]
            {
                _searchTextBox, _genreComboBox, _yearComboBox, _searchButton, _addMovieButton
            });
        }

        private void CreateMoviesPanel()
        {
            _moviesPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = Color.White
            };

            Controls.Add(_moviesPanel);
        }

        private void LoadData()
        {
            _movieService.LoadMovies();
        }

        private void DisplayMovies()
        {
            var movies = _movieService.GetAllMovies();
            DisplayMovieList(movies);
        }

        private void DisplayMovieList(System.Collections.Generic.List<Movie> movies)
        {
            _moviesPanel.Controls.Clear();

            foreach (var movie in movies)
            {
                var moviePanel = new MoviePanel(movie);
                _moviesPanel.Controls.Add(moviePanel);
            }
        }

        private void OnSearchTextBoxEnter(object sender, EventArgs e)
        {
            if (_searchTextBox.Text == "Film adı ara...")
            {
                _searchTextBox.Text = "";
                _searchTextBox.ForeColor = Color.Black;
            }
        }

        private void OnSearchTextBoxLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_searchTextBox.Text))
            {
                _searchTextBox.Text = "Film adı ara...";
                _searchTextBox.ForeColor = Color.Gray;
            }
        }

        private void OnSearchButtonClick(object sender, EventArgs e)
        {
            var searchText = _searchTextBox.Text == "Film adı ara..." ? null : _searchTextBox.Text;

            MovieGenre? selectedGenre = null;
            if (_genreComboBox.SelectedIndex > 0)
            {
                selectedGenre = EnumHelper.GetGenreFromText(_genreComboBox.SelectedItem.ToString());
            }

            int? selectedYear = null;
            if (_yearComboBox.SelectedIndex > 0)
            {
                selectedYear = int.Parse(_yearComboBox.SelectedItem.ToString());
            }

            var filteredMovies = _movieService.SearchMovies(searchText, selectedGenre, selectedYear);
            DisplayMovieList(filteredMovies);
        }

        private void OnAddMovieButtonClick(object sender, EventArgs e)
        {
            using (var addForm = new AddMovieForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    _movieService.AddMovie(addForm.NewMovie);
                    DisplayMovies();
                }
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            _movieService.SaveMovies();
            _reviewService.SaveReviewsToFile(_movieService.GetAllMovies());
        }
    }
}
