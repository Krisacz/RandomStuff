using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ProclaimFieldsCreator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DelayCounter = new System.Windows.Forms.NumericUpDown();
            this.AlphaPositionLabel = new System.Windows.Forms.Label();
            this.NamePositionLabel = new System.Windows.Forms.Label();
            this.NewPositionLabel = new System.Windows.Forms.Label();
            this.SetAlphaButton = new System.Windows.Forms.Button();
            this.SetNameButton = new System.Windows.Forms.Button();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.SetNewButton = new System.Windows.Forms.Button();
            this.FieldsGroupBox = new System.Windows.Forms.GroupBox();
            this.CvsInfoButton = new System.Windows.Forms.Button();
            this.LoadCsvFile = new System.Windows.Forms.Button();
            this.FieldsListBox = new System.Windows.Forms.ListBox();
            this.CreateButton = new System.Windows.Forms.Button();
            this.SettingsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DelayCounter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.FieldsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SettingsGroupBox
            // 
            this.SettingsGroupBox.Controls.Add(this.label5);
            this.SettingsGroupBox.Controls.Add(this.label4);
            this.SettingsGroupBox.Controls.Add(this.DelayCounter);
            this.SettingsGroupBox.Controls.Add(this.AlphaPositionLabel);
            this.SettingsGroupBox.Controls.Add(this.NamePositionLabel);
            this.SettingsGroupBox.Controls.Add(this.NewPositionLabel);
            this.SettingsGroupBox.Controls.Add(this.SetAlphaButton);
            this.SettingsGroupBox.Controls.Add(this.SetNameButton);
            this.SettingsGroupBox.Controls.Add(this.pictureBox3);
            this.SettingsGroupBox.Controls.Add(this.pictureBox2);
            this.SettingsGroupBox.Controls.Add(this.pictureBox1);
            this.SettingsGroupBox.Controls.Add(this.SetNewButton);
            this.SettingsGroupBox.Location = new System.Drawing.Point(12, 12);
            this.SettingsGroupBox.Name = "SettingsGroupBox";
            this.SettingsGroupBox.Size = new System.Drawing.Size(335, 162);
            this.SettingsGroupBox.TabIndex = 10;
            this.SettingsGroupBox.TabStop = false;
            this.SettingsGroupBox.Text = "Settings";
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label5.Location = new System.Drawing.Point(6, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(323, 38);
            this.label5.TabIndex = 21;
            this.label5.Text = "To set position press SET button, move mouse cursor to desired and press [F1]  to" +
    " apply.";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(47, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 21);
            this.label4.TabIndex = 20;
            this.label4.Text = "Delay";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DelayCounter
            // 
            this.DelayCounter.Location = new System.Drawing.Point(128, 12);
            this.DelayCounter.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.DelayCounter.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.DelayCounter.Name = "DelayCounter";
            this.DelayCounter.Size = new System.Drawing.Size(74, 20);
            this.DelayCounter.TabIndex = 19;
            this.DelayCounter.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // AlphaPositionLabel
            // 
            this.AlphaPositionLabel.Location = new System.Drawing.Point(245, 99);
            this.AlphaPositionLabel.Name = "AlphaPositionLabel";
            this.AlphaPositionLabel.Size = new System.Drawing.Size(84, 21);
            this.AlphaPositionLabel.TabIndex = 18;
            this.AlphaPositionLabel.Text = "9999,9999";
            this.AlphaPositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NamePositionLabel
            // 
            this.NamePositionLabel.Location = new System.Drawing.Point(125, 99);
            this.NamePositionLabel.Name = "NamePositionLabel";
            this.NamePositionLabel.Size = new System.Drawing.Size(77, 21);
            this.NamePositionLabel.TabIndex = 17;
            this.NamePositionLabel.Text = "9999,9999";
            this.NamePositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NewPositionLabel
            // 
            this.NewPositionLabel.Location = new System.Drawing.Point(3, 99);
            this.NewPositionLabel.Name = "NewPositionLabel";
            this.NewPositionLabel.Size = new System.Drawing.Size(75, 21);
            this.NewPositionLabel.TabIndex = 16;
            this.NewPositionLabel.Text = "9999,9999";
            this.NewPositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SetAlphaButton
            // 
            this.SetAlphaButton.Location = new System.Drawing.Point(248, 76);
            this.SetAlphaButton.Name = "SetAlphaButton";
            this.SetAlphaButton.Size = new System.Drawing.Size(81, 20);
            this.SetAlphaButton.TabIndex = 15;
            this.SetAlphaButton.Text = "Set";
            this.SetAlphaButton.UseVisualStyleBackColor = true;
            this.SetAlphaButton.Click += new System.EventHandler(this.SetAlphaButton_Click);
            // 
            // SetNameButton
            // 
            this.SetNameButton.Location = new System.Drawing.Point(128, 76);
            this.SetNameButton.Name = "SetNameButton";
            this.SetNameButton.Size = new System.Drawing.Size(74, 20);
            this.SetNameButton.TabIndex = 14;
            this.SetNameButton.Text = "Set";
            this.SetNameButton.UseVisualStyleBackColor = true;
            this.SetNameButton.Click += new System.EventHandler(this.SetNameButton_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::ProclaimFieldsCreator.Properties.Resources.alpha;
            this.pictureBox3.Location = new System.Drawing.Point(248, 43);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(81, 13);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 13;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ProclaimFieldsCreator.Properties.Resources.name;
            this.pictureBox2.Location = new System.Drawing.Point(128, 43);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(74, 21);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 12;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ProclaimFieldsCreator.Properties.Resources._new;
            this.pictureBox1.Location = new System.Drawing.Point(6, 43);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(72, 27);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // SetNewButton
            // 
            this.SetNewButton.Location = new System.Drawing.Point(6, 76);
            this.SetNewButton.Name = "SetNewButton";
            this.SetNewButton.Size = new System.Drawing.Size(72, 20);
            this.SetNewButton.TabIndex = 10;
            this.SetNewButton.Text = "Set";
            this.SetNewButton.UseVisualStyleBackColor = true;
            this.SetNewButton.Click += new System.EventHandler(this.SetNewButton_Click);
            // 
            // FieldsGroupBox
            // 
            this.FieldsGroupBox.Controls.Add(this.CvsInfoButton);
            this.FieldsGroupBox.Controls.Add(this.LoadCsvFile);
            this.FieldsGroupBox.Controls.Add(this.FieldsListBox);
            this.FieldsGroupBox.Location = new System.Drawing.Point(12, 180);
            this.FieldsGroupBox.Name = "FieldsGroupBox";
            this.FieldsGroupBox.Size = new System.Drawing.Size(335, 165);
            this.FieldsGroupBox.TabIndex = 11;
            this.FieldsGroupBox.TabStop = false;
            this.FieldsGroupBox.Text = "Fields CSV";
            // 
            // CvsInfoButton
            // 
            this.CvsInfoButton.Location = new System.Drawing.Point(254, 19);
            this.CvsInfoButton.Name = "CvsInfoButton";
            this.CvsInfoButton.Size = new System.Drawing.Size(75, 23);
            this.CvsInfoButton.TabIndex = 23;
            this.CvsInfoButton.Text = "CSV Info";
            this.CvsInfoButton.UseVisualStyleBackColor = true;
            this.CvsInfoButton.Click += new System.EventHandler(this.CvsInfoButton_Click);
            // 
            // LoadCsvFile
            // 
            this.LoadCsvFile.Location = new System.Drawing.Point(9, 19);
            this.LoadCsvFile.Name = "LoadCsvFile";
            this.LoadCsvFile.Size = new System.Drawing.Size(75, 23);
            this.LoadCsvFile.TabIndex = 22;
            this.LoadCsvFile.Text = "Load CSV";
            this.LoadCsvFile.UseVisualStyleBackColor = true;
            this.LoadCsvFile.Click += new System.EventHandler(this.LoadCsvFile_Click);
            // 
            // FieldsListBox
            // 
            this.FieldsListBox.FormattingEnabled = true;
            this.FieldsListBox.Location = new System.Drawing.Point(6, 48);
            this.FieldsListBox.Name = "FieldsListBox";
            this.FieldsListBox.Size = new System.Drawing.Size(323, 108);
            this.FieldsListBox.TabIndex = 0;
            // 
            // CreateButton
            // 
            this.CreateButton.Location = new System.Drawing.Point(12, 351);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(335, 48);
            this.CreateButton.TabIndex = 12;
            this.CreateButton.Text = "Create all fields";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 411);
            this.Controls.Add(this.CreateButton);
            this.Controls.Add(this.FieldsGroupBox);
            this.Controls.Add(this.SettingsGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "Proclaim Field Creator v.1.0";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.SettingsGroupBox.ResumeLayout(false);
            this.SettingsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DelayCounter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.FieldsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox SettingsGroupBox;
        private Label label4;
        private NumericUpDown DelayCounter;
        private Label AlphaPositionLabel;
        private Label NamePositionLabel;
        private Label NewPositionLabel;
        private Button SetAlphaButton;
        private Button SetNameButton;
        private PictureBox pictureBox3;
        private PictureBox pictureBox2;
        private PictureBox pictureBox1;
        private Button SetNewButton;
        private Label label5;
        private GroupBox FieldsGroupBox;
        private ListBox FieldsListBox;
        private Button CvsInfoButton;
        private Button LoadCsvFile;
        private Button CreateButton;
    }
}

