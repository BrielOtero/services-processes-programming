using System.Diagnostics;
using System.IO;
using System.Security;

namespace _01_exercise
{
    public partial class Form1 : Form
    {
        private DirectoryInfo currentDirInfo;
        private Dictionary<string, DirectoryInfo> currentDirs = new Dictionary<string, DirectoryInfo> { };
        private Dictionary<string, FileInfo> currentFiles = new Dictionary<string, FileInfo> { };

        public Form1()
        {
            InitializeComponent();
            this.Text = "The magic of paths";
        }

        private void btnChangePath_Click(object sender, EventArgs e)
        {
            string path = txtPath.Text.Trim();
            try
            {
                if (path[0] == '%' && path[path.Length - 1] == '%')
                {
                    path = Environment.GetEnvironmentVariable(path.Substring(1, path.Length - 2));
                }


                currentDirInfo = new DirectoryInfo(path);
                loadList();
            }
            catch (UnauthorizedAccessException u)
            {
                errorWithPath();
                Trace.WriteLine("Error reading path");
                Trace.WriteLine($"Error: {u.Message}");
            }
            catch (Exception a)
            {
                errorWithPath();
                Trace.WriteLine("Error reading path");
                Trace.WriteLine($"Error: {a.Message}");
            }
        }

        private void loadList()
        {
            currentDirs.Clear();
            lstSubdirectories.Items.Clear();
            lstFiles.Items.Clear();

            DirectoryInfo[] subDirectories = currentDirInfo.GetDirectories();

            bool areParent = currentDirInfo.Parent != null;
            bool areSubDirectories = subDirectories.Length > 0;

            if (areParent)
            {
                currentDirs.Add("...", new DirectoryInfo(currentDirInfo.Parent.FullName));
            }

            if (areSubDirectories)
            {
                Array.ForEach(subDirectories, (x) => currentDirs.Add(x.Name, x));
            }

            if (areParent || areSubDirectories)
            {
                lstSubdirectories.Items.AddRange(currentDirs.Keys.ToArray());

                FileInfo[] files = currentDirInfo.GetFiles();
                currentFiles.Clear();
                Array.ForEach(files, (x) => currentFiles.Add(x.Name, x));
                if (files.Length > 0)
                {
                    lstFiles.Items.AddRange(currentFiles.Keys.ToArray());
                }
            }
        }

        private void lstSubdirectories_SelectedValueChanged(object sender, EventArgs e)
        {
            DirectoryInfo lastPath = currentDirInfo;

            try
            {
                if (lstSubdirectories.SelectedItem != null)
                {
                    updateCurrentPath(currentDirs[lstSubdirectories.SelectedItem.ToString()]);
                }
            }
            catch (UnauthorizedAccessException u)
            {
                errorWithPath();
                updateCurrentPath(lastPath);
                Trace.WriteLine("Error updating  path");
                Trace.WriteLine($"Error: {u.Message}");
            }
            catch (Exception a)
            {
                errorWithPath();
                updateCurrentPath(lastPath);
                Trace.WriteLine("Error updating path");
                Trace.WriteLine($"Error: {a.Message}");
            }
        }

        private void errorWithPath()
        {
            MessageBox.Show(this, "This path can't show", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void updateCurrentPath(DirectoryInfo newDirectory)
        {
            currentDirInfo = newDirectory;

            txtPath.Text = currentDirInfo.FullName;

            lblSizeInfo.Text = "";

            loadList();
        }

        private void lstFiles_SelectedValueChanged(object sender, EventArgs e)
        {
            checkFileSize();

            if (currentFiles[lstFiles.SelectedItem.ToString()].Extension == ".txt")
            {
                TxtFormContent txtFormContent = new TxtFormContent(currentFiles[lstFiles.SelectedItem.ToString()].FullName);
                txtFormContent.Text = lstFiles.SelectedItem.ToString();
                if (txtFormContent.ShowDialog() == DialogResult.Yes)
                {
                    string newContent = txtFormContent.Content;
                    if (MessageBox.Show(this, "The file was modified\nDo you want overwrite the file?", "Overwrite?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        File.WriteAllText(currentFiles[lstFiles.SelectedItem.ToString()].FullName, newContent);
                    }
                }
            }
        }

        private void checkFileSize()
        {
            lblSizeInfo.Text = sizeFormat(currentFiles[lstFiles.SelectedItem.ToString()].Length);
        }

        private string sizeFormat(long size)
        {
            switch (size)
            {

                case > 1000 and < 1000000:
                    return $"{size / 1000} KB";
                case > 1000000:
                    return $"{size / 1000000} MB";
                default:
                    return $"{size} Bytes";

            }
        }
    }
}