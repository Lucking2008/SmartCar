namespace ArduinoGUI
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.num_puerto = new System.Windows.Forms.NumericUpDown();
            this.btn_conectar = new System.Windows.Forms.Button();
            this.label_estado = new System.Windows.Forms.Label();
            this.update = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.num_puerto)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "COM";
            // 
            // num_puerto
            // 
            this.num_puerto.Location = new System.Drawing.Point(41, 12);
            this.num_puerto.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.num_puerto.Name = "num_puerto";
            this.num_puerto.Size = new System.Drawing.Size(41, 20);
            this.num_puerto.TabIndex = 2;
            this.num_puerto.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btn_conectar
            // 
            this.btn_conectar.Location = new System.Drawing.Point(88, 12);
            this.btn_conectar.Name = "btn_conectar";
            this.btn_conectar.Size = new System.Drawing.Size(82, 23);
            this.btn_conectar.TabIndex = 3;
            this.btn_conectar.Text = "Conectar";
            this.btn_conectar.UseVisualStyleBackColor = true;
            this.btn_conectar.Click += new System.EventHandler(this.btn_conectar_Click);
            // 
            // label_estado
            // 
            this.label_estado.AutoSize = true;
            this.label_estado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_estado.Location = new System.Drawing.Point(574, 17);
            this.label_estado.Name = "label_estado";
            this.label_estado.Size = new System.Drawing.Size(45, 13);
            this.label_estado.TabIndex = 4;
            this.label_estado.Text = "estado";
            // 
            // update
            // 
            this.update.Enabled = true;
            this.update.Interval = 20;
            this.update.Tick += new System.EventHandler(this.update_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label_estado);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.num_puerto);
            this.groupBox1.Controls.Add(this.btn_conectar);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(672, 44);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(525, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Estado:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 518);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "ArduinoGUI";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.num_puerto)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown num_puerto;
        private System.Windows.Forms.Button btn_conectar;
        private System.Windows.Forms.Label label_estado;
        private System.Windows.Forms.Timer update;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
    }
}

