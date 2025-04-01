using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace WIn_UUP_Iso
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = "WinFetch - ISO�_�E�����[�_�[";
            this.Icon = new Icon("app.ico");
            richTextBox1.SelectionColor = Color.FromArgb(255, 255, 255);
            Log("���̃\�t�g�́AUUP-DUMP API���g�p���Ă��܂��B", Color.White);
            Log("�`�`�`�`�`���ӎ����`�`�`�`�`", Color.Yellow);
            Log("�� �S�{�b�N�X�I����Ɂu�����v�������Ă��������B", Color.Yellow);
            Log("�����A�t�@�C�����_�E�����[�h��������Aiso����邽�߂�cmd�g�p�����ԈႦ�Ă��Ȃ������ꍇ�́AISO�쐬�{�^���������Ă��������B", Color.Yellow);
            Log("�o�[�W������ς������ꍇ�́A���Z�b�g�{�^���������Ă��������B", Color.Yellow);
            Log("�`�`�`�`�`�`�`�`�`�`�`�`�`�`", Color.Yellow);
        }

        private async void ComboBox_Changer(string WinVersion, string Architex)
        {
            string all_url = $"https://api.uupdump.net/listid.php?&sortByDate=1";
            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(all_url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                // ������responseBody����������
                JsonDocument jsonDocument = JsonDocument.Parse(responseBody);
                if (jsonDocument.RootElement.TryGetProperty("response", out JsonElement responseElement) &&
                    responseElement.TryGetProperty("builds", out JsonElement buildsElement))
                {
                    foreach (JsonElement build in buildsElement.EnumerateArray())
                    {
                        if (!build.TryGetProperty("title", out JsonElement titleElement))
                        {
                            throw new Exception("�^�C�g�����Ȃ�");
                        }
                        // �A�[�L�e�N�`���̂Ƃ�
                        if (!build.TryGetProperty("arch", out JsonElement archElement))
                        {
                            throw new Exception("�A�[�L�e�N�`�����Ȃ�");
                        }
                        // �r���h�ԍ��擾
                        if (!build.TryGetProperty("uuid", out JsonElement uuidElement))
                        {
                            throw new Exception("UUID���Ȃ�");
                        }

                        string title = titleElement.GetString();
                        string arch = archElement.GetString();
                        string uuid = uuidElement.GetString();

                        if (title.StartsWith($"{WinVersion}")
                            && arch.Equals($"{Architex}", StringComparison.OrdinalIgnoreCase)
                            && uuid.Equals($"{uuid}", StringComparison.OrdinalIgnoreCase))
                        {
                            var item = $"{title} {arch}";
                            comboBox3.Items.Add(new { Text = item, Uuid = uuid });
                        }
                    }
                }
                else
                {
                    Log("Invalid response format.", Color.Red);
                }
            }
            catch (Exception ex)
            {
                Log($"Error: {ex}", Color.Red);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || comboBox4.SelectedIndex == -1)
            {
                MessageBox.Show("�o�[�W�����ƃA�[�L�e�N�`����I�����Ă��������B", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                Log($"WindowsVersion:{comboBox4.Text}���I������܂����B", Color.Cyan);
                Log($"Arch:{comboBox1.Text}���I������܂����B", Color.Cyan);

                ComboBox_Changer(comboBox4.Text, comboBox1.Text);

                comboBox3.Enabled = true;
                button5.Enabled = true;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (comboBox3.Items.Count < 0 || comboBox3.Enabled == false)
            {
                Log("�o�[�W�����ꗗ����o�[�W�������w�肳�ꂸ�Ɂu�_�E�����[�h�v��������܂����B", Color.Red);
                MessageBox.Show("�o�[�W�������w�肳��Ă��܂���B��ɁA�o�[�W�����ꗗ����o�[�W�������w�肵�Ă��������B", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var selectedItem = comboBox3.SelectedItem;

            if (selectedItem != null)
            {
                string uuid = ((dynamic)selectedItem).Uuid;

                if (!string.IsNullOrEmpty(uuid))
                {
                    Log($"�I�����ꂽUUID: {uuid}", Color.Cyan);

                    string targetUrl = $"https://api.uupdump.net/get.php?id={uuid}&lang=ja-jp&edition=professional&noLinks=0";
                    using HttpClient client = new HttpClient();

                    Log("JSON���N�G�X�g�𑗐M���܂�...", Color.White);
                    Log($"URL: {targetUrl}", Color.White);

                    await Task.Delay(2000);

                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(targetUrl);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // ������responseBody����������
                        JsonDocument jsonDocument = JsonDocument.Parse(responseBody);
                        if (jsonDocument.RootElement.TryGetProperty("response", out JsonElement responseElement) &&
                            responseElement.TryGetProperty("files", out JsonElement filesElement))
                        {
                            foreach (var file in filesElement.EnumerateObject())
                            {
                                if (file.Value.TryGetProperty("url", out JsonElement urlElement) &&
                                file.Value.TryGetProperty("debug", out JsonElement debugElement))
                                {
                                    string fileUrl = urlElement.GetString();
                                    string fileDebug = debugElement.GetString();

                                    string fileName = file.Name;

                                    Log($"�t�@�C����: {fileName}", Color.LightGreen);
                                    Log($"URL: {fileUrl}", Color.LightGreen);

                                    // �_�E�����[�h����
                                    await DownloadFileAsync(fileUrl, fileName);
                                }
                            }
                        }
                        else
                        {
                            richTextBox1.SelectionColor = Color.FromArgb(231, 76, 60);
                            Log("Url��������܂���ł����B", Color.Red);
                            richTextBox1.SelectionColor = Color.FromArgb(255, 255, 255);
                            richTextBox1.ScrollToCaret();
                        }
                    }
                    catch (HttpRequestException exce)
                    {
                        Log($"Request error: {exce.Message}", Color.Red);

                    }
                }
                else
                {
                    Log("�o�[�W�������I������Ă��܂���B", Color.Red);
                }

                // ISO�쐬
                await CreateIso();
            }
        }

        private async Task DownloadFileAsync(string fileUrl, string fileName2)
        {
            using HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(30);
            string fileName = Path.GetFileName(fileUrl);

            // ISO_FOLDER/UUPs �t�H���_���쐬
            string isoFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "ISO_FOLDER");
            string uupsFolderPath = Path.Combine(isoFolderPath, "UUPs");
            Directory.CreateDirectory(uupsFolderPath);

            string filePath = Path.Combine(uupsFolderPath, fileName2);


            Log($"�t�@�C���̃_�E�����[�h���J�n���܂�: {fileName2}", Color.White);

            try
            {
                using (HttpResponseMessage response = await client.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    long? totalBytes = response.Content.Headers.ContentLength;

                    if (totalBytes == null || totalBytes.Value <= 0)
                    {
                        Log("�_�E�����[�h����t�@�C���̃T�C�Y���s���܂��͖����ł��B", Color.White);
                        progressBar1.Maximum = 1;
                        progressBar1.Value = 0;
                        return;
                    }

                    // Ensure totalBytes is within the valid range for an integer
                    progressBar1.Maximum = (int)Math.Clamp(totalBytes.Value, 1, int.MaxValue);
                    progressBar1.Value = 0;

                    using (Stream stream = await response.Content.ReadAsStreamAsync())
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = new byte[8192];
                        long totalBytesRead = 0;
                        int bytesRead;

                        int lastLoggedProgress = -1;

                        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            totalBytesRead += bytesRead;
                            await fs.WriteAsync(buffer, 0, bytesRead);

                            UpdateProgress(totalBytesRead, totalBytes.Value, ref lastLoggedProgress);
                        }
                    }
                }
                Log($"�_�E�����[�h���������܂���: {filePath}", Color.LightGreen);
            }
            catch (HttpRequestException e)
            {
                Log($"�_�E�����[�h���ɃG���[���������܂���: {e.Message}", Color.Red);
            }
        }

        private async Task CreateIso()
        {
            string isoFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "ISO_FOLDER");

            // ISO_FOLDER�t�H���_����convert-UUP.cmd��bin�t�H���_�������ꍇ�A���[�g�f�B���N�g������R�s�[
            string rootDirectory = Directory.GetCurrentDirectory();
            string convertCmdSourcePath = Path.Combine(rootDirectory, "convert-UUP.cmd");
            string binSourcePath = Path.Combine(rootDirectory, "bin");

            string convertCmdTargetPath = Path.Combine(isoFolderPath, "convert-UUP.cmd");
            string binTargetPath = Path.Combine(isoFolderPath, "bin");

            if (!File.Exists(convertCmdTargetPath))
            {
                Log("convert-UUP.cmd��������܂���B�R�s�[���J�n���܂�...", Color.White);
                File.Copy(convertCmdSourcePath, convertCmdTargetPath, true);
                Log("convert-UUP.cmd���R�s�[���܂����B", Color.White);
            }

            if (!Directory.Exists(binTargetPath))
            {
                Log("bin�t�H���_��������܂���B�R�s�[���J�n���܂�...", Color.White);
                Directory.CreateDirectory(binTargetPath);
                foreach (string dirPath in Directory.GetDirectories(binSourcePath, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(binSourcePath, binTargetPath));
                }
                foreach (string Local_filePath in Directory.GetFiles(binSourcePath, "*.*", SearchOption.AllDirectories))
                {
                    File.Copy(Local_filePath, Local_filePath.Replace(binSourcePath, binTargetPath), true);
                }
                Log("bin�t�H���_���R�s�[���܂����B", Color.White);
            }

            // convert-UUP.cmd�����s���A������܂őҋ@
            Log("convert-UUP.cmd�����s���܂�...", Color.White);
            MessageBox.Show("cmd�g�p�����o�܂��̂ŁA�u�͂��v��I�����Ă��������B", "����", MessageBoxButtons.OK, MessageBoxIcon.Information);
            string cmdPath = Path.Combine(isoFolderPath, "convert-UUP.cmd");

            var processStartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = cmdPath,
                WorkingDirectory = isoFolderPath,
                UseShellExecute = false,
                Verb = "runas",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = false
            };

            using (var process = System.Diagnostics.Process.Start(processStartInfo))
            {
                if (process != null)
                {
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            Log(e.Data, Color.White);
                        }
                    };

                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            Log("�G���[: " + e.Data, Color.Red);
                        }
                    };

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();


                    await process.WaitForExitAsync();
                    Log("convert-UUP.cmd���N�����܂����B", Color.White);
                    Log("�`�`�`�`�`�`�`�`�`�`�`�`�`�`", Color.Yellow);
                    Log("�����Acmd�g�p���Łu�������v��I�����Ă��܂����ꍇ�A������ISO�쐬�{�^���������Ă��������B", Color.Yellow);
                    Log("�`�`�`�`�`�`�`�`�`�`�`�`�`�`", Color.Yellow);
                }
                else
                {
                    Log("convert-UUP.cmd�̋N���Ɏ��s���܂����B", Color.Red);
                }
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = comboBox3.SelectedItem;

            if (selectedItem != null)
            {
                string uuid = ((dynamic)selectedItem).Uuid;

                Log($"{comboBox3.Text}���I������܂����BUUID: {uuid}", Color.Cyan);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("���Ƀt�@�C�����_�E�����[�h���Ă���܂����H", "�m�F", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Log($"ISO�ɕϊ����܂��B", Color.Cyan);
                CreateIso();
            }
        }

        private void Log(string message, Color? color = null, bool addNewLine = true)
        {
            color ??= Color.White;

            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action(() => Log(message, color, addNewLine)));
            }
            else
            {
                richTextBox1.SelectionFont = new Font("���C���I", 12, FontStyle.Regular);
                richTextBox1.SelectionColor = color.Value;

                richTextBox1.AppendText(message + (addNewLine ? "\n" : ""));
                richTextBox1.ScrollToCaret();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            comboBox3.Enabled = false;
            Log("�o�[�W�����ꗗ�����Z�b�g����܂����B", Color.White);
            MessageBox.Show("���Z�b�g����", "���Z�b�g", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        private void UpdateProgress(long totalBytesRead, long totalBytes, ref int lastLoggedProgress)
        {
            totalBytesRead = Math.Clamp(totalBytesRead, 0, totalBytes);

            progressBar1.Value = (int)Math.Clamp(totalBytesRead, 0, progressBar1.Maximum);

            int progressPercentage = (int)((totalBytesRead * 100) / totalBytes);

            if (progressPercentage != lastLoggedProgress && progressPercentage % 20 == 0)
            {
                lastLoggedProgress = progressPercentage;
                Log($"�_�E�����[�h�i�s��: {progressPercentage}% ({totalBytesRead}/{totalBytes}) �o�C�g", Color.Cyan);
            }
        }

        private void �t�@�C��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("�쐬�r��", "�g����", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox3.Items.Count > 0)
            {
                comboBox3.SelectedIndex = 0;
            }
            else
            {
                Log("�������s��ꂸ�Ɂu�ŐV�o�[�W�������w�肷��v��������܂����B", Color.Red);
                MessageBox.Show("�������s���Ă��܂���B��ɁA���������s���Ă��������B", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}