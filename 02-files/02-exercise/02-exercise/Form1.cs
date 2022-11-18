using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace _02_exercise
{
    public partial class Form1 : Form
    {
        private DirectoryInfo currentDirectory;
        private List<FileInfo> files;
        private string extensionsPath;
        private string[] allowedExtensions;
        private string textToSearch;
        public Form1()
        {
            InitializeComponent();

            loadExtensions();
        }

        private void loadExtensions()
        {
            extensionsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "extensions.txt");
            string extensionContent;
            string[] extensions;

            if (File.Exists(extensionsPath))
            {
                extensionContent = File.ReadAllText(extensionsPath);
                extensions = extensionContent.Split(",");
                bool areValidExtensions = true;
                Array.ForEach(extensions, x =>
                {
                    x = x.Trim();
                    if (x.Length <= 0 || (x[0] != '*' && x[1] != '.'))
                    {
                        if (areValidExtensions)
                        {
                            areValidExtensions = false;
                        }
                    }
                });

                if (!areValidExtensions)
                {
                    extensionContent = "*.txt";
                    extensions = new string[] { "*.txt" };
                }
            }
            else
            {
                extensionContent = "*.txt";
                extensions = new string[] { "*.txt" };
            }

            allowedExtensions = extensions;
            txtExtensions.Text = extensionContent;
            txtExtensions.Modified = false;

        }

        private bool checkExtensions()
        {
            if (txtExtensions.Modified)
            {
                string extensionContent = txtExtensions.Text;
                bool areValidExtensions = true;

                if (extensionContent.Length <= 0)
                {
                    error("Extensions format");
                    return false;
                }

                string[] extensions = extensionContent.Split(",");
                Array.ForEach(extensions, x =>
                {
                    x = x.Trim();

                    if (x.Length <= 0 || (x[0] != '*' && x[1] != '.'))
                    {
                        if (areValidExtensions)
                        {
                            areValidExtensions = false;
                        }
                    }
                });

                if (!areValidExtensions)
                {
                    error("Extensions format");
                }
                else
                {
                    allowedExtensions = extensions;
                }

                txtExtensions.Modified = false;
                return areValidExtensions;
            }
            else
            {
                return true;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!checkExtensions())
            {
                return;
            }

            string path = txtPath.Text;

            lstFiles.Items.Clear();

            textToSearch = txtTextToSearch.Text.Trim();

            if (textToSearch.Length <= 0)
            {
                error("Text to search");
                return;
            }

            if (Directory.Exists(path))
            {
                try
                {
                    currentDirectory = new DirectoryInfo(path);
                }
                catch (ArgumentNullException)
                {
                    error("Path");
                    return;
                }
                catch (Exception)
                {
                    error("Path");
                    return;
                }
            }
            else
            {
                error("Path");
                return;
            }
            try
            {
                files = allowedExtensions.SelectMany(i => currentDirectory.GetFiles(i, SearchOption.TopDirectoryOnly)).ToList();
            }
            catch (UnauthorizedAccessException)
            {
                error("Files");
                return;
            }
            catch (Exception)
            {
                error("Files");
                return;

            }

            lstFiles.Items.Add($"{"Concurrences",-14} | {"Path"}");
            foreach (var item in files)
            {
                new Thread(x =>
                {
                    FileSearch f = addFileCont;
                    this.Invoke(f, item);
                }).Start();

            }

        }
        delegate void FileSearch(FileInfo file);
        private void addFileCont(FileInfo file)
        {
            try
            {
                string fileContent = File.ReadAllText(file.FullName);
                int contWordMatch = Regex.Matches(fileContent, textToSearch, chbCase.Checked ? RegexOptions.None : RegexOptions.IgnoreCase).Count();

                if (contWordMatch > 0)
                {
                    lstFiles.Items.Add($"{contWordMatch,-14} | {file.Name}");
                }
            }
            catch (ArgumentException a)
            {
                Trace.WriteLine("Error reading file " + a.Message);
            }
            catch (Exception e)
            {
                Trace.WriteLine("Error reading file " + e.Message);
            }
            GC.Collect();
        }

        private void error(string value)
        {
            MessageBox.Show(this, $"{value} not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string text = "";
            Array.ForEach(allowedExtensions, x => text += x + ",");
            text = text.Substring(0, text.Length - 1);
            File.WriteAllText(extensionsPath,text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}