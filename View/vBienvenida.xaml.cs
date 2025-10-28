using System.Globalization;
using System.Reflection;
namespace jmedinaS3.View;

public partial class vBienvenida : ContentPage
{
	public vBienvenida()
	{
		InitializeComponent();

        BindingContext = this;

        studentPicker.SelectedIndex = 0;

        datePicker.MaximumDate = DateTime.Now;
    }

    private void OnNotaTextChanged(object sender, TextChangedEventArgs e)
    {
        CalculatePartialGrades();
    }

    private void CalculatePartialGrades()
    {
        try
        {
            // Calcular parcial 1
            if (double.TryParse(seguimiento1Entry.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double seg1) &&
                double.TryParse(examen1Entry.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double ex1))
            {
                // Validar rango
                if (seg1 >= 0 && seg1 <= 10 && ex1 >= 0 && ex1 <= 10)
                {
                    double parcial1 = (seg1 * 0.3) + (ex1 * 0.2);
                    notaParcial1Label.Text = $"Nota Parcial 1: {parcial1:F2}";
                    resumenParcial1Label.Text = parcial1.ToString("F2", CultureInfo.InvariantCulture);
                }
            }

            // Calcular parcial 2
            if (double.TryParse(seguimiento2Entry.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double seg2) &&
                double.TryParse(examen2Entry.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double ex2))
            {
                // Validar rango
                if (seg2 >= 0 && seg2 <= 10 && ex2 >= 0 && ex2 <= 10)
                {
                    double parcial2 = (seg2 * 0.3) + (ex2 * 0.2);
                    notaParcial2Label.Text = $"Nota Parcial 2: {parcial2:F2}";
                    resumenParcial2Label.Text = parcial2.ToString("F2", CultureInfo.InvariantCulture);
                }
            }

            // Calcular nota final
            if (double.TryParse(resumenParcial1Label.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double p1) &&
                double.TryParse(resumenParcial2Label.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double p2))
            {
                double final = p1 + p2;
                resumenFinalLabel.Text = final.ToString("F2", CultureInfo.InvariantCulture);

                // Determinar estado
                string estado = final >= 7 ? "APROBADO" :
                               (final >= 5 && final <= 6.9) ? "COMPLEMENTARIO" : "REPROBADO";
                estadoLabel.Text = $"Estado: {estado}";

                // Cambiar color según estado
                estadoLabel.TextColor = estado switch
                {
                    "APROBADO" => Color.FromArgb("#27AE60"),
                    "COMPLEMENTARIO" => Color.FromArgb("#E67E22"),
                    "REPROBADO" => Color.FromArgb("#E74C3C"),
                    _ => Colors.Black
                };
            }
        }
        catch
        {
            // Ignorar errores en tiempo real
        }
    }

    private async void OnCalculateClicked(object sender, EventArgs e)
    {
        // Validaciones
        if (studentPicker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Por favor, seleccione un estudiante", "OK");
            return;
        }

        if (!ValidateNotas())
        {
            await DisplayAlert("Error", "Por favor, ingrese todas las notas válidas en el rango de 0 a 10", "OK");
            return;
        }

        // Crear objeto estudiante - AHORA FUNCIONARÁ
        var student = new Models.Student
        {
            Name = studentPicker.SelectedItem.ToString(),
            Date = datePicker.Date,
            Seguimiento1 = double.Parse(seguimiento1Entry.Text, CultureInfo.InvariantCulture),
            Examen1 = double.Parse(examen1Entry.Text, CultureInfo.InvariantCulture),
            Seguimiento2 = double.Parse(seguimiento2Entry.Text, CultureInfo.InvariantCulture),
            Examen2 = double.Parse(examen2Entry.Text, CultureInfo.InvariantCulture)
        };

        // Mostrar resultados en DisplayAlert
        await DisplayAlert("🎓 REPORTE DE CALIFICACIONES - UISRAEL",
            $"\n📝 Nombre: {student.Name}" +
            $"\n📅 Fecha: {student.Date:dd/MM/yyyy}" +
            $"\n📊 Nota Parcial 1: {student.NotaParcial1:F2}" +
            $"\n📊 Nota Parcial 2: {student.NotaParcial2:F2}" +
            $"\n🎯 Nota Final: {student.NotaFinal:F2}" +
            $"\n📋 Estado: {student.Estado}",
            "ACEPTAR");
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        // Limpiar todos los campos
        seguimiento1Entry.Text = string.Empty;
        examen1Entry.Text = string.Empty;
        seguimiento2Entry.Text = string.Empty;
        examen2Entry.Text = string.Empty;

        notaParcial1Label.Text = "Nota Parcial 1: 0.00";
        notaParcial2Label.Text = "Nota Parcial 2: 0.00";

        resumenParcial1Label.Text = "0.00";
        resumenParcial2Label.Text = "0.00";
        resumenFinalLabel.Text = "0.00";
        estadoLabel.Text = "Estado: -";
        estadoLabel.TextColor = Colors.Black;
    }

    private bool ValidateNotas()
    {
        string[] notas = { seguimiento1Entry.Text, examen1Entry.Text,
                             seguimiento2Entry.Text, examen2Entry.Text };

        foreach (string nota in notas)
        {
            if (string.IsNullOrWhiteSpace(nota))
            {
                return false;
            }

            if (!double.TryParse(nota, NumberStyles.Any, CultureInfo.InvariantCulture, out double valor) ||
                valor < 0 || valor > 10)
            {
                return false;
            }
        }
        return true;
    }

}