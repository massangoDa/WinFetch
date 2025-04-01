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
            this.Text = "WinFetch - ISOダウンローダー";
            this.Icon = new Icon("app.ico");
            richTextBox1.SelectionColor = Color.FromArgb(255, 255, 255);
            Log("このソフトは、UUP-DUMP APIを使用しています。", Color.White);
            Log("～～～～～注意事項～～～～～", Color.Yellow);
            Log("※ 全ボックス選択後に「検索」を押してください。", Color.Yellow);
            Log("もし、ファイルをダウンロードが完了後、isoを作るためのcmd使用許可を間違えてしなかった場合は、ISO作成ボタンを押してください。", Color.Yellow);
            Log("バージョンを変えたい場合は、リセットボタンを押してください。", Color.Yellow);
            Log("～～～～～～～～～～～～～～", Color.Yellow);
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

                // ここでresponseBodyを処理する
                JsonDocument jsonDocument = JsonDocument.Parse(responseBody);
                if (jsonDocument.RootElement.TryGetProperty("response", out JsonElement responseElement) &&
                    responseElement.TryGetProperty("builds", out JsonElement buildsElement))
                {
                    foreach (JsonElement build in buildsElement.EnumerateArray())
                    {
                        if (!build.TryGetProperty("title", out JsonElement titleElement))
                        {
                            throw new Exception("タイトルがない");
                        }
                        // アーキテクチャのとこ
                        if (!build.TryGetProperty("arch", out JsonElement archElement))
                        {
                            throw new Exception("アーキテクチャがない");
                        }
                        // ビルド番号取得
                        if (!build.TryGetProperty("uuid", out JsonElement uuidElement))
                        {
                            throw new Exception("UUIDがない");
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
                MessageBox.Show("バージョンとアーキテクチャを選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                Log($"WindowsVersion:{comboBox4.Text}が選択されました。", Color.Cyan);
                Log($"Arch:{comboBox1.Text}が選択されました。", Color.Cyan);

                ComboBox_Changer(comboBox4.Text, comboBox1.Text);

                comboBox3.Enabled = true;
                button5.Enabled = true;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (comboBox3.Items.Count < 0 || comboBox3.Enabled == false)
            {
                Log("バージョン一覧からバージョンが指定されずに「ダウンロード」が押されました。", Color.Red);
                MessageBox.Show("バージョンが指定されていません。先に、バージョン一覧からバージョンを指定してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var selectedItem = comboBox3.SelectedItem;

            if (selectedItem != null)
            {
                string uuid = ((dynamic)selectedItem).Uuid;

                if (!string.IsNullOrEmpty(uuid))
                {
                    Log($"選択されたUUID: {uuid}", Color.Cyan);

                    string targetUrl = $"https://api.uupdump.net/get.php?id={uuid}&lang=ja-jp&edition=professional&noLinks=0";
                    using HttpClient client = new HttpClient();

                    Log("JSONリクエストを送信します...", Color.White);
                    Log($"URL: {targetUrl}", Color.White);

                    await Task.Delay(2000);

                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(targetUrl);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // ここでresponseBodyを処理する
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

                                    Log($"ファイル名: {fileName}", Color.LightGreen);
                                    Log($"URL: {fileUrl}", Color.LightGreen);

                                    // ダウンロード処理
                                    await DownloadFileAsync(fileUrl, fileName);
                                }
                            }
                        }
                        else
                        {
                            richTextBox1.SelectionColor = Color.FromArgb(231, 76, 60);
                            Log("Urlが見つかりませんでした。", Color.Red);
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
                    Log("バージョンが選択されていません。", Color.Red);
                }

                // ISO作成
                await CreateIso();
            }
        }

        private async Task DownloadFileAsync(string fileUrl, string fileName2)
        {
            using HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(30);
            string fileName = Path.GetFileName(fileUrl);

            // ISO_FOLDER/UUPs フォルダを作成
            string isoFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "ISO_FOLDER");
            string uupsFolderPath = Path.Combine(isoFolderPath, "UUPs");
            Directory.CreateDirectory(uupsFolderPath);

            string filePath = Path.Combine(uupsFolderPath, fileName2);


            Log($"ファイルのダウンロードを開始します: {fileName2}", Color.White);

            try
            {
                using (HttpResponseMessage response = await client.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    long? totalBytes = response.Content.Headers.ContentLength;

                    if (totalBytes == null || totalBytes.Value <= 0)
                    {
                        Log("ダウンロードするファイルのサイズが不明または無効です。", Color.White);
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
                Log($"ダウンロードが完了しました: {filePath}", Color.LightGreen);
            }
            catch (HttpRequestException e)
            {
                Log($"ダウンロード中にエラーが発生しました: {e.Message}", Color.Red);
            }
        }

        private async Task CreateIso()
        {
            string isoFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "ISO_FOLDER");

            // ISO_FOLDERフォルダ内にconvert-UUP.cmdとbinフォルダが無い場合、ルートディレクトリからコピー
            string rootDirectory = Directory.GetCurrentDirectory();
            string convertCmdSourcePath = Path.Combine(rootDirectory, "convert-UUP.cmd");
            string binSourcePath = Path.Combine(rootDirectory, "bin");

            string convertCmdTargetPath = Path.Combine(isoFolderPath, "convert-UUP.cmd");
            string binTargetPath = Path.Combine(isoFolderPath, "bin");

            if (!File.Exists(convertCmdTargetPath))
            {
                Log("convert-UUP.cmdが見つかりません。コピーを開始します...", Color.White);
                File.Copy(convertCmdSourcePath, convertCmdTargetPath, true);
                Log("convert-UUP.cmdをコピーしました。", Color.White);
            }

            if (!Directory.Exists(binTargetPath))
            {
                Log("binフォルダが見つかりません。コピーを開始します...", Color.White);
                Directory.CreateDirectory(binTargetPath);
                foreach (string dirPath in Directory.GetDirectories(binSourcePath, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(binSourcePath, binTargetPath));
                }
                foreach (string Local_filePath in Directory.GetFiles(binSourcePath, "*.*", SearchOption.AllDirectories))
                {
                    File.Copy(Local_filePath, Local_filePath.Replace(binSourcePath, binTargetPath), true);
                }
                Log("binフォルダをコピーしました。", Color.White);
            }

            // convert-UUP.cmdを実行し、閉じられるまで待機
            Log("convert-UUP.cmdを実行します...", Color.White);
            MessageBox.Show("cmd使用許可が出ますので、「はい」を選択してください。", "注意", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                            Log("エラー: " + e.Data, Color.Red);
                        }
                    };

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();


                    await process.WaitForExitAsync();
                    Log("convert-UUP.cmdが起動しました。", Color.White);
                    Log("～～～～～～～～～～～～～～", Color.Yellow);
                    Log("もし、cmd使用許可で「いいえ」を選択してしまった場合、左側のISO作成ボタンを押してください。", Color.Yellow);
                    Log("～～～～～～～～～～～～～～", Color.Yellow);
                }
                else
                {
                    Log("convert-UUP.cmdの起動に失敗しました。", Color.Red);
                }
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = comboBox3.SelectedItem;

            if (selectedItem != null)
            {
                string uuid = ((dynamic)selectedItem).Uuid;

                Log($"{comboBox3.Text}が選択されました。UUID: {uuid}", Color.Cyan);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("既にファイルをダウンロードしてありますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Log($"ISOに変換します。", Color.Cyan);
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
                richTextBox1.SelectionFont = new Font("メイリオ", 12, FontStyle.Regular);
                richTextBox1.SelectionColor = color.Value;

                richTextBox1.AppendText(message + (addNewLine ? "\n" : ""));
                richTextBox1.ScrollToCaret();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            comboBox3.Enabled = false;
            Log("バージョン一覧がリセットされました。", Color.White);
            MessageBox.Show("リセット完了", "リセット", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        private void UpdateProgress(long totalBytesRead, long totalBytes, ref int lastLoggedProgress)
        {
            totalBytesRead = Math.Clamp(totalBytesRead, 0, totalBytes);

            progressBar1.Value = (int)Math.Clamp(totalBytesRead, 0, progressBar1.Maximum);

            int progressPercentage = (int)((totalBytesRead * 100) / totalBytes);

            if (progressPercentage != lastLoggedProgress && progressPercentage % 20 == 0)
            {
                lastLoggedProgress = progressPercentage;
                Log($"ダウンロード進行中: {progressPercentage}% ({totalBytesRead}/{totalBytes}) バイト", Color.Cyan);
            }
        }

        private void ファイルToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("作成途中", "使い方", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox3.Items.Count > 0)
            {
                comboBox3.SelectedIndex = 0;
            }
            else
            {
                Log("検索が行われずに「最新バージョンを指定する」が押されました。", Color.Red);
                MessageBox.Show("検索が行われていません。先に、検索を実行してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}