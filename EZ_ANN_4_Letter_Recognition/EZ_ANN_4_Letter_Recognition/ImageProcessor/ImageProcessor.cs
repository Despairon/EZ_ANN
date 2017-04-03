using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace EZ_ANN_4_Letter_Recognition
{
    public class ImageProcessor
    {
        public ImageProcessor(ToolStripMenuItem loadMenu,
                              ToolStripMenuItem saveMenu,
                              ToolStripMenuItem clearMenu,
                              PictureBox pBox)
        {
            this.pBox = pBox;

            isImageLoaded = true;

            onImageLoad(null, null);

            loadMenu.Click  += new EventHandler(onImageLoad);
            saveMenu.Click  += new EventHandler(onImageSave);
            clearMenu.Click += new EventHandler(onImageClear);

        }

        private Drawer drawer;
        private PictureBox pBox;
        private bool isImageLoaded;

        private void onImageLoad(object sender, EventArgs e)
        {
            MessageBox.Show("Select image to draw on please...");
            try
            {
                OpenFileDialog openDlg = new OpenFileDialog();
                if (openDlg.ShowDialog() != DialogResult.OK)
                    isImageLoaded = false;

                if (isImageLoaded)
                {
                    FileStream fs = new FileStream(openDlg.FileName, FileMode.Open, FileAccess.Read);

                    pBox.Image = Image.FromStream(fs);

                    drawer = new Drawer(pBox);

                    fs.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Can't open img. Please, try again");
            }
        }

        private void onImageSave(object sender, EventArgs e)
        {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.ShowDialog();
            try
            {
                drawer.bitmap.Save(saveDlg.FileName);
            }
            catch (Exception) { }
        }

        private void onImageClear(object sender, EventArgs e)
        {
            drawer.clear();
        }

    }
}
