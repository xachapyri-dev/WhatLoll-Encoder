using System;
using Whatloll.Encoder;
using System.Windows.Forms;

namespace whatlollEncoder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Icon = Properties.Resources.iconca;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string input = richTextBox1.Text;
                string encrypted = WhatLoll.Encrypt(input);
                richTextBox2.Text = encrypted;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при шифровании: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string input = richTextBox1.Text;
                string decrypted = WhatLoll.Decrypt(input);
                richTextBox2.Text = decrypted;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при дешифровании: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            
            try
            {
                string input = richTextBox3.Text;
                string decrypted = WhatLoll.DecryptV2(input);
                richTextBox4.Text = decrypted;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при шифровании: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string input = richTextBox3.Text;
                string decrypted = WhatLoll.EncryptV2(input);
                richTextBox4.Text = decrypted;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при дешифровании: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}