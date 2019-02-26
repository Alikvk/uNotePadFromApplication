using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace uNotepad
{
    public partial class uNote : Form
    {
        public uNote()
        {
            InitializeComponent();
        }

        bool _textDegistiMi = false;

        bool _basildiMi = false;

        public bool _kapatildiMi = false;

        public string fileName { get; set; } // Bu kısımda dosya ismini diğer foruma taşımak amacıyla böyle bir property tanımlıyoruz

        public void Kaydet()
        {

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //FilterIndex indislemeye 0'dan degil 1'den basladigi icin ve dun oraya 'FilterIndex == 0' diye sart yazdigimiz icin hata aliyorduk
                //Kullanici RTF format secmis olsa bile, FilterIndex hicbir zaman '0' olmadigi icin, her dokumani PlainText formatinda kaydediyorduk
                if (saveFileDialog1.FilterIndex == 1)
                {
                    richBox.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichText);

                }
                else
                {
                    richBox.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                    this.fileName = Path.GetFileName(saveFileDialog1.FileName);// Bu kısımda filename propertyye dosya ismini aktarıyoruz
                }
                //ternary if
                //richBox.SaveFile(saveFileDialog1.FileName, (saveFileDialog1.FilterIndex == 0) ? RichTextBoxStreamType.RichText : RichTextBoxStreamType.PlainText);
                this.fileName = Path.GetFileName(saveFileDialog1.FileName);
            }
        }

        public void DosyaAc(string acilacakDosyaninYolu)
        {
            //EndsWith string bir ifadenin en sonunda, belirtilen kelimenin olup olmadigini bakar
            if (acilacakDosyaninYolu.EndsWith("txt"))
            {
                richBox.LoadFile(acilacakDosyaninYolu, RichTextBoxStreamType.PlainText);

            }
            else
            {
                richBox.LoadFile(acilacakDosyaninYolu);
            }
        }

        public void YaziRengiDegistir(Color renk)
        {
            richBox.SelectionColor = renk;
        }

        public void YaziTipiDegistir(Font yeniFont)
        {
            richBox.SelectionFont = yeniFont;
        }

        public void YaziBuyukHarfYap()
        {
            richBox.SelectedText = richBox.SelectedText.ToUpper();
        }

        public void YaziKucukHarfYap()
        {
            richBox.SelectedText = richBox.SelectedText.ToLower();

        }

        public void Ara(string arananMetin)
        {
            richBox.Find(arananMetin);
        }

        private void kesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richBox.Cut();
        }

        private void kopyalaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richBox.Copy();
        }

        private void yapistirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richBox.Paste();
        }

        private void uNote_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_textDegistiMi)
            {
                DialogResult result = MessageBox.Show("Dosyayi kaydedeyim mi?", "uNotepad", MessageBoxButtons.YesNoCancel,
                     MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                switch (result)
                {
                    case DialogResult.Yes:
                        //SaveFileDialog acilacak
                        Kaydet();
                        break;
                    case DialogResult.Cancel:
                        //kapatma islemi iptal edilecek
                        e.Cancel = true;
                        break;
                }
            }
            _kapatildiMi = !e.Cancel;
        }

        //AK 2 -- Increase row and column when writing
        private void richBox_TextChanged(object sender, EventArgs e)
        {
            if (!_textDegistiMi)
            {
                richBox.Parent.Parent.Text += " *";
                _textDegistiMi = true;
            }
            
            int line = 0;
            int column = 0;
            _textDegistiMi = true;
            if (_textDegistiMi)
            {

                //this.textCount = richBox.Text.Length;
                toolStripStatusLabel2.Text = richBox.TextLength.ToString();
                line = 1 + richBox.GetLineFromCharIndex(richBox.GetFirstCharIndexOfCurrentLine());
                column = 1 + richBox.SelectionStart - richBox.GetFirstCharIndexOfCurrentLine();
                toolStripStatusLabel3.Text = "Line :" + line + "  Column : " + column;
            }

        }

        private void hepsiniSecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richBox.SelectAll();
        }
        //AK 3 -- Line Column Add
        private void richBox_SelectionChange(object sender, EventArgs e)
        {
            //Mousea tıkladığımız zaman nerede olduğumuzu göstermektedir.

            int line = 0;
            int column = 0;
            _basildiMi = true;
            if (_basildiMi)
            {
                line = 1 + richBox.GetLineFromCharIndex(richBox.GetFirstCharIndexOfCurrentLine());
                column = 1 + richBox.SelectionStart - richBox.GetFirstCharIndexOfCurrentLine();
                toolStripStatusLabel3.Text = "Line :" + line + "  Column : " + column;
                this.fileName = "* " + Path.GetFileName(saveFileDialog1.FileName);
            }
        }
    }

}
