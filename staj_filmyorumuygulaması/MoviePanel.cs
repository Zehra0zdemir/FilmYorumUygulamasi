using System;
using System.Drawing;
using System.Windows.Forms;
using MovieReviewApp.Models;
using MovieReviewApp.Utils;

namespace MovieReviewApp.Controls
{
    public class MoviePanel : Panel  // Panel'den inherit ediyor
    {
        public Movie Movie { get; private set; }

        private readonly PictureBox _pictureBox;
        private readonly Label _nameLabel;
        private readonly Label _genreLabel;
        private readonly Label _yearLabel;
        private TextBox _reviewTextBox;
        private Button _addReviewButton;

        private readonly Size _originalSize = new Size(160, 240);
        private readonly Size _enlargedSize = new Size(170, 255);

        public MoviePanel(Movie movie)
        {
            Movie = movie ?? throw new ArgumentNullException(nameof(movie));
            InitializePanel();
        }

        private void InitializePanel()
        {
            Size = new Size(180, 320);
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = Color.WhiteSmoke;
            Margin = new Padding(10);

            _pictureBox = CreatePictureBox();
            _nameLabel = CreateNameLabel();
            _genreLabel = CreateGenreLabel();
            _yearLabel = CreateYearLabel();

            Controls.AddRange(new Control[] { _pictureBox, _nameLabel, _genreLabel, _yearLabel });
        }

        private PictureBox CreatePictureBox()
        {
            var pictureBox = new PictureBox
            {
                Size = _originalSize,
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.LightGray,
                BorderStyle = BorderStyle.FixedSingle
            };

            CreatePosterImage(pictureBox);

            pictureBox.MouseEnter += (s, e) => {
                pictureBox.Size = _enlargedSize;
                pictureBox.Location = new Point(5, 5);
            };

            pictureBox.MouseLeave += (s, e) => {
                pictureBox.Size = _originalSize;
                pictureBox.Location = new Point(10, 10);
            };

            pictureBox.Click += (s, e) => ShowReviewControls();

            return pictureBox;
        }

        private void CreatePosterImage(PictureBox pictureBox)
        {
            if (Movie.PosterImage != null)
            {
                pictureBox.Image = new Bitmap(Movie.PosterImage, _originalSize);
                return;
            }

            var defaultImage = new Bitmap(_originalSize.Width, _originalSize.Height);
            using (var g = Graphics.FromImage(defaultImage))
            {
                g.FillRectangle(Brushes.LightGray, 0, 0, defaultImage.Width, defaultImage.Height);
                g.DrawRectangle(Pens.DarkGray, 0, 0, defaultImage.Width - 1, defaultImage.Height - 1);

                const string text = "POSTER";
                var font = new Font("Arial", 12, FontStyle.Bold);
                var textSize = g.MeasureString(text, font);
                var x = (defaultImage.Width - textSize.Width) / 2;
                var y = (defaultImage.Height - textSize.Height) / 2;
                g.DrawString(text, font, Brushes.DarkGray, x, y);
            }
            pictureBox.Image = defaultImage;
        }

        private Label CreateNameLabel()
        {
            return new Label
            {
                Text = Movie.Name,
                Location = new Point(10, 260),
                Size = new Size(160, 20),
                Font = new Font("Arial", 9, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
        }

        private Label CreateGenreLabel()
        {
            return new Label
            {
                Text = EnumHelper.GetGenreText(Movie.Genre),
                Location = new Point(10, 280),
                Size = new Size(160, 15),
                Font = new Font("Arial", 8),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.DarkBlue
            };
        }

        private Label CreateYearLabel()
        {
            return new Label
            {
                Text = Movie.Year.ToString(),
                Location = new Point(10, 295),
                Size = new Size(160, 15),
                Font = new Font("Arial", 8),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.DarkGreen
            };
        }

        private void ShowReviewControls()
        {
            if (_reviewTextBox != null) return;

            _reviewTextBox = new TextBox
            {
                Location = new Point(10, 315),
                Size = new Size(120, 60),
                Multiline = true,
                Text = "Yorumunuzu yazın...",
                ForeColor = Color.Gray
            };

            _reviewTextBox.Enter += OnReviewTextBoxEnter;
            _reviewTextBox.Leave += OnReviewTextBoxLeave;

            _addReviewButton = new Button
            {
                Text = "Ekle",
                Location = new Point(135, 315),
                Size = new Size(35, 25),
                Font = new Font("Arial", 8)
            };

            _addReviewButton.Click += OnAddReviewButtonClick;

            Height = 385;
            Controls.AddRange(new Control[] { _reviewTextBox, _addReviewButton });
        }

        private void OnReviewTextBoxEnter(object sender, EventArgs e)
        {
            if (_reviewTextBox.Text == "Yorumunuzu yazın...")
            {
                _reviewTextBox.Text = "";
                _reviewTextBox.ForeColor = Color.Black;
            }
        }

        private void OnReviewTextBoxLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_reviewTextBox.Text))
            {
                _reviewTextBox.Text = "Yorumunuzu yazın...";
                _reviewTextBox.ForeColor = Color.Gray;
            }
        }

        private void OnAddReviewButtonClick(object sender, EventArgs e)
        {
            var reviewText = _reviewTextBox.Text;

            if (reviewText == "Yorumunuzu yazın..." || string.IsNullOrWhiteSpace(reviewText))
            {
                MessageBox.Show("Lütfen bir yorum yazın!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Movie.Reviews.Add(reviewText);

            _reviewTextBox.Text = "Yorumunuzu yazın...";
            _reviewTextBox.ForeColor = Color.Gray;

            MessageBox.Show("Yorum eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
