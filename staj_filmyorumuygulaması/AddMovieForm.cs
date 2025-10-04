using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MovieReviewApp.Models;
using MovieReviewApp.Services;
using MovieReviewApp.Utils;

namespace MovieReviewApp.Forms
{
    public partial class AddMovieForm : Form  // Form'dan inherit ediyor
    {
        public Movie NewMovie { get; private set; }

        private readonly PosterService _posterService;
        private TextBox _nameTextBox;
        private ComboBox _genreComboBox;
        private ComboBox _yearComboBox;
        private Button _selectImageButton;
        private PictureBox _previewPictureBox;
        private Button _okButton;
        private Button _cancelButton;
        private string _selectedImagePath;

        public AddMovieForm()
        {
            _posterService = new PosterService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Yeni Film Ekle";
            Size = new Size(450, 350);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            CreateControls();
            SetupEventHandlers();
        }

        private void CreateControls()
        {
            // Film adı
            var nameLabel = new Label
            {
                Text = "Film Adı:",
                Location = new Point(20, 20),
                Size = new Size(80, 20)
            };

            _nameTextBox = new TextBox
            {
                Location = new Point(110, 20),
                Size = new Size(200, 20)
            };

            // Tür
            var genreLabel = new Label
            {
                Text = "Tür:",
                Location = new Point(20, 60),
                Size = new Size(80, 20)
            };

            _genreComboBox = new ComboBox
            {
                Location = new Point(110, 60),
                Size = new Size(200, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            foreach (MovieGenre genre in Enum.GetValues(typeof(MovieGenre)))
            {
                _genreComboBox.Items.Add(EnumHelper.GetGenreText(genre));
            }
            _genreComboBox.SelectedIndex = 0;

            // Yıl
            var yearLabel = new Label
            {
                Text = "Yıl:",
                Location = new Point(20, 100),
                Size = new Size(80, 20)
            };

            _yearComboBox = new ComboBox
            {
                Location = new Point(110, 100),
                Size = new Size(200, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            for (int year = DateTime.Now.Year; year >= 1900; year--)
            {
                _yearComboBox.Items.Add(year);
            }
            _yearComboBox.SelectedItem = DateTime.Now.Year;

            // Poster seçme
            var imageLabel = new Label
            {
                Text = "Poster:",
                Location = new Point(20, 140),
                Size = new Size(80, 20)
            };

            _selectImageButton = new Button
            {
                Text = "Poster Seç",
                Location = new Point(110, 140),
                Size = new Size(100, 25)
            };

            // Poster önizlemesi
            _previewPictureBox = new PictureBox
            {
                Location = new Point(330, 20),
                Size = new Size(100, 150),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.LightGray
            };

            // Butonlar
            _okButton = new Button
            {
                Text = "Tamam",
                Location = new Point(250, 280),
                Size = new Size(75, 25),
                DialogResult = DialogResult.OK
            };

            _cancelButton = new Button
            {
                Text = "İptal",
                Location = new Point(335, 280),
                Size = new Size(75, 25),
                DialogResult = DialogResult.Cancel
            };

            Controls.AddRange(new Control[]
            {
                nameLabel, _nameTextBox, genreLabel, _genreComboBox,
                yearLabel, _yearComboBox, imageLabel, _selectImageButton,
                _previewPictureBox, _okButton, _cancelButton
            });

            AcceptButton = _okButton;
            CancelButton = _cancelButton;
        }

        private void SetupEventHandlers()
        {
            _selectImageButton.Click += OnSelectImageButtonClick;
            _okButton.Click += OnOkButtonClick;
        }

        private void OnSelectImageButtonClick(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Tüm Dosyalar|*.*";
                openFileDialog.Title = "Poster Seçin";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var previewImage = Image.FromFile(openFileDialog.FileName);
                        _previewPictureBox.Image = previewImage;

                        _selectedImagePath = _posterService.SavePosterToAppDirectory(openFileDialog.FileName);

                        if (!string.IsNullOrEmpty(_selectedImagePath))
                        {
                            _selectImageButton.Text = "Poster Seçildi ✓";
                            _selectImageButton.BackColor = Color.LightGreen;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Resim yüklenirken hata oluştu: {ex.Message}", "Hata",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void OnOkButtonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_nameTextBox.Text))
            {
                MessageBox.Show("Film adı boş olamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NewMovie = new Movie
            {
                Name = _nameTextBox.Text.Trim(),
                Genre = EnumHelper.GetGenreFromText(_genreComboBox.SelectedItem.ToString()),
                Year = (int)_yearComboBox.SelectedItem,
                ImagePath = _selectedImagePath
            };

            if (!string.IsNullOrEmpty(_selectedImagePath))
            {
                NewMovie.LoadPosterImage();
            }
        }
    }
}
}
