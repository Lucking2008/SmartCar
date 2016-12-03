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
            this.btnIzquierda = new System.Windows.Forms.Button();
            this.btnDerecha = new System.Windows.Forms.Button();
            this.btnParar = new System.Windows.Forms.Button();
            this.btnAcelerar = new System.Windows.Forms.Button();
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
            // btnIzquierda
            // 
            this.btnIzquierda.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIzquierda.Location = new System.Drawing.Point(106, 197);
            this.btnIzquierda.Name = "btnIzquierda";
            this.btnIzquierda.Size = new System.Drawing.Size(154, 91);
            this.btnIzquierda.TabIndex = 6;
            this.btnIzquierda.Text = "<-";
            this.btnIzquierda.UseVisualStyleBackColor = true;
            this.btnIzquierda.Click += new System.EventHandler(this.btnIzquierda_Click);
            // 
            // btnDerecha
            // 
            this.btnDerecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDerecha.Location = new System.Drawing.Point(426, 197);
            this.btnDerecha.Name = "btnDerecha";
            this.btnDerecha.Size = new System.Drawing.Size(154, 91);
            this.btnDerecha.TabIndex = 7;
            this.btnDerecha.Text = "->";
            this.btnDerecha.UseVisualStyleBackColor = true;
            this.btnDerecha.Click += new System.EventHandler(this.btnDerecha_Click);
            // 
            // btnParar
            // 
            this.btnParar.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnParar.Location = new System.Drawing.Point(266, 197);
            this.btnParar.Name = "btnParar";
            this.btnParar.Size = new System.Drawing.Size(154, 91);
            this.btnParar.TabIndex = 8;
            this.btnParar.Text = "X";
            this.btnParar.UseVisualStyleBackColor = true;
            this.btnParar.Click += new System.EventHandler(this.btnParar_Click);
            // 
            // btnAcelerar
            // 
            this.btnAcelerar.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAcelerar.Location = new System.Drawing.Point(266, 100);
            this.btnAcelerar.Name = "btnAcelerar";
            this.btnAcelerar.Size = new System.Drawing.Size(154, 91);
            this.btnAcelerar.TabIndex = 9;
            this.btnAcelerar.Text = "^";
            this.btnAcelerar.UseVisualStyleBackColor = true;
            this.btnAcelerar.Click += new System.EventHandler(this.btnAcelerar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 518);
            this.Controls.Add(this.btnAcelerar);
            this.Controls.Add(this.btnParar);
            this.Controls.Add(this.btnDerecha);
            this.Controls.Add(this.btnIzquierda);
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
        private System.Windows.Forms.Button btnIzquierda;
        private System.Windows.Forms.Button btnDerecha;
        private System.Windows.Forms.Button btnParar;
        private System.Windows.Forms.Button btnAcelerar;
    }
}

