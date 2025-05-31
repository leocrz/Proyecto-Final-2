namespace Proyecto2progra
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            lblTitulo = new Label();
            btnDetectarUbicacion = new Button();
            dtpFecha = new DateTimePicker();
            cmbAmbiente = new ComboBox();
            btnConsultar = new Button();
            btnGuardar = new Button();
            txtResultado = new RichTextBox();
            textUbicacion = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtMedidasTerreno = new TextBox();
            txtComentarioExtra = new TextBox();
            label4 = new Label();
            buttLimpiar = new Button();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.BackColor = Color.Yellow;
            lblTitulo.Location = new Point(233, 22);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(313, 20);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "RECOMENDADOR INTELIGENTE DE CULTIVOS";
            // 
            // btnDetectarUbicacion
            // 
            btnDetectarUbicacion.BackColor = Color.Khaki;
            btnDetectarUbicacion.Location = new Point(12, 90);
            btnDetectarUbicacion.Name = "btnDetectarUbicacion";
            btnDetectarUbicacion.Size = new Size(163, 29);
            btnDetectarUbicacion.TabIndex = 2;
            btnDetectarUbicacion.Text = "Detectar Ubicación";
            btnDetectarUbicacion.UseVisualStyleBackColor = false;
            btnDetectarUbicacion.Click += btnDetectarUbicacion_Click;
            // 
            // dtpFecha
            // 
            dtpFecha.Location = new Point(552, 88);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(275, 27);
            dtpFecha.TabIndex = 3;
            
            // 
            // cmbAmbiente
            // 
            cmbAmbiente.FormattingEnabled = true;
            cmbAmbiente.Location = new Point(196, 127);
            cmbAmbiente.Name = "cmbAmbiente";
            cmbAmbiente.Size = new Size(202, 28);
            cmbAmbiente.TabIndex = 4;
            // 
            // btnConsultar
            // 
            btnConsultar.Location = new Point(552, 125);
            btnConsultar.Name = "btnConsultar";
            btnConsultar.Size = new Size(215, 26);
            btnConsultar.TabIndex = 6;
            btnConsultar.Text = "Consultar Recomendación";
            btnConsultar.UseVisualStyleBackColor = true;
            btnConsultar.Click += btnConsultar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(552, 157);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(177, 29);
            btnGuardar.TabIndex = 7;
            btnGuardar.Text = "Guardar Consulta";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // txtResultado
            // 
            txtResultado.Location = new Point(114, 260);
            txtResultado.Name = "txtResultado";
            txtResultado.ReadOnly = true;
            txtResultado.Size = new Size(558, 190);
            txtResultado.TabIndex = 8;
            txtResultado.Text = "";
            // 
            // textUbicacion
            // 
            textUbicacion.Location = new Point(196, 90);
            textUbicacion.Name = "textUbicacion";
            textUbicacion.ReadOnly = true;
            textUbicacion.Size = new Size(202, 27);
            textUbicacion.TabIndex = 9;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Khaki;
            label1.Location = new Point(477, 91);
            label1.Name = "label1";
            label1.Size = new Size(65, 20);
            label1.TabIndex = 10;
            label1.Text = "FECHA  :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Khaki;
            label2.Location = new Point(12, 135);
            label2.Name = "label2";
            label2.Size = new Size(167, 20);
            label2.TabIndex = 11;
            label2.Text = "SELECCIONE AMBIENTE";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Khaki;
            label3.Location = new Point(12, 168);
            label3.Name = "label3";
            label3.Size = new Size(206, 20);
            label3.TabIndex = 12;
            label3.Text = "MEDIDA DEL TERRENO L / A :";
            // 
            // txtMedidasTerreno
            // 
            txtMedidasTerreno.Location = new Point(233, 165);
            txtMedidasTerreno.Name = "txtMedidasTerreno";
            txtMedidasTerreno.Size = new Size(165, 27);
            txtMedidasTerreno.TabIndex = 13;
            
            // 
            // txtComentarioExtra
            // 
            txtComentarioExtra.Location = new Point(285, 214);
            txtComentarioExtra.Multiline = true;
            txtComentarioExtra.Name = "txtComentarioExtra";
            txtComentarioExtra.Size = new Size(209, 31);
            txtComentarioExtra.TabIndex = 14;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Khaki;
            label4.Location = new Point(12, 217);
            label4.Name = "label4";
            label4.Size = new Size(256, 20);
            label4.TabIndex = 15;
            label4.Text = "\"Comentarios adicionales (opcional)\"";
            // 
            // buttLimpiar
            // 
            buttLimpiar.Location = new Point(552, 192);
            buttLimpiar.Name = "buttLimpiar";
            buttLimpiar.Size = new Size(94, 29);
            buttLimpiar.TabIndex = 16;
            buttLimpiar.Text = "LIMPIAR";
            buttLimpiar.UseVisualStyleBackColor = true;
            buttLimpiar.Click += buttLimpiar_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(830, 490);
            Controls.Add(buttLimpiar);
            Controls.Add(label4);
            Controls.Add(txtComentarioExtra);
            Controls.Add(txtMedidasTerreno);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textUbicacion);
            Controls.Add(txtResultado);
            Controls.Add(btnGuardar);
            Controls.Add(btnConsultar);
            Controls.Add(cmbAmbiente);
            Controls.Add(dtpFecha);
            Controls.Add(btnDetectarUbicacion);
            Controls.Add(lblTitulo);
            Name = "Form1";
            WindowState = FormWindowState.Minimized;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private TextBox txtUbicacion;
        private Button btnDetectarUbicacion;
        private DateTimePicker dtpFecha;
        private ComboBox cmbAmbiente;
        private Button btnConsultar;
        private Button btnGuardar;
        private RichTextBox txtResultado;
        private TextBox textUbicacion;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtMedidasTerreno;
        private TextBox txtComentarioExtra;
        private Label label4;
        private Button buttLimpiar;
    }
}
