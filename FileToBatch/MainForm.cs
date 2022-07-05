using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileToBatch
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnBrowseInput_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog open = new OpenFileDialog()
            {
                Title = "請選擇輸入檔案"
            })
            {
                if (open.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                txtInput.Text = open.FileName;
            }
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog save = new SaveFileDialog()
            {
                Filter = "BAT 檔案 (*.bat)|*.bat|CMD 檔案 (*.cmd)|*.cmd",
                Title = "請指定輸出檔案"
            })
            {
                if (save.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                txtOutput.Text = save.FileName;
            }
        }

        private void enableControl(bool enable)
        {
            btnBrowseInput.Enabled = btnBrowseOutput.Enabled = btnBuild.Enabled
                = enable;
        }

        private async void btnBuild_Click(object sender, EventArgs e)
        {
            string input = txtInput.Text;
            string output = txtOutput.Text;

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("尚未選擇輸入檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(output))
            {
                MessageBox.Show("尚未指定輸出檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            enableControl(false);
            labelStatus.Text = "處理中";
            await Task.Run(() =>
            {
                try
                {
                    string batch = BatchCreator.createBatch(input);
                    BatchCreator.writeConfuseBatchFile(output, batch);
                    MessageBox.Show("檔案處理完成", "資訊", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"處理檔案時發生例外狀況 : {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
            labelStatus.Text = "就緒";
            enableControl(true);
        }
    }
}